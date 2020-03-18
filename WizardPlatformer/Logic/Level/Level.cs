using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using WizardPlatformer.Logic.Level.LevelLoading;
using WizardPlatformer.Logic.Save;

namespace WizardPlatformer.Logic.Level {
	public class Level {
		public static readonly Dictionary<int, int[]> RoomSize = new Dictionary<int, int[]>(){
			{ 0, new int[2]{ 32, 18}},
			{ 1, new int[2]{ 96, 18}},
			{ 2, new int[2]{ 32, 96}},
			{ 3, new int[2]{ 64, 36}}
		};

		#region Fields

		private bool isLevelLoaded;
		private int[] switchLevel;
		private bool hasLevelSwitchQuery;

		private int levelId;
		private int roomId;
		private int roomSizeId;
		private int roomWidth;
		private int roomHeigth;

		private ContentManager levelContentManager;
		private LevelLoader levelLoader;
		private int tileSideSize;

		private Tile[,] backLayer;
		private Tile[,] baseLayer;
		private Tile[,] decoLayer;
		private Tile[,] functionalLayer;

		private Background background;
		
		private bool isTriggerOn;
		private float currentOpacity;

		private EntityPlayer player;
		private List<Entity> entities;
		private List<KeyValuePair<Tile, Entity>> entitiesSchedule;

		private List<Tile> tileEntities;

		#endregion

		public Level(int levelId, int roomId, int[] previousLevel) {
			this.isLevelLoaded = false;
			this.levelId = levelId;
			this.roomId = roomId;
			this.switchLevel = previousLevel;
			this.tileSideSize = Display.TileSideSize;
			this.entities = new List<Entity>();
			this.entitiesSchedule = new List<KeyValuePair<Tile, Entity>>();
			this.tileEntities = new List<Tile>();

			this.isTriggerOn = false;
			this.currentOpacity = 1.0f;
		}

		public void LoadContent(ContentManager contentManager) {
			this.levelContentManager = new ContentManager(contentManager.ServiceProvider, "Content");
			MappedLevelParts mappedLevelParts = levelLoader.LoadLevel(levelId, roomId);

			backLayer = mappedLevelParts.LayerBack;
			baseLayer = mappedLevelParts.LayerBase;
			decoLayer = mappedLevelParts.LayerDeco;
			functionalLayer = mappedLevelParts.LayerFunctional;
			roomSizeId = mappedLevelParts.RoomSize;
			roomWidth = RoomSize[roomSizeId][0];
			roomHeigth = RoomSize[roomSizeId][1];

			background = new Background(roomWidth, roomHeigth);
			background.LoadContent(levelContentManager, mappedLevelParts.BackgoundId);

			foreach (TileMovingPlatform platform in mappedLevelParts.MovingPlatforms) {
				if (platform != null) {
					platform.SetLevel(this);
					SpawnTileEntity(platform);
				}
			}

			player = (EntityPlayer)EntityCreator.CreateEntity(1, (int)mappedLevelParts.PlayerPosition.X, (int)mappedLevelParts.PlayerPosition.Y);
			SpawnEntity(player);

			LoadEntitiesContent(levelContentManager);

			isLevelLoaded = true;
		}

		public void UnloadContent() {
			levelContentManager.Unload();
		}

		public void Update(GameTime gameTime) {
			UpdateLayersVisibility(gameTime, 0.05f);

			UpdateTileLayer(gameTime, backLayer, roomSizeId);
			UpdateTileLayer(gameTime, baseLayer, roomSizeId);

			UpdateTileEntities(gameTime);

			SpawnEntitiesScheduled();
			UpdateEntities(gameTime);

			UpdateTileLayer(gameTime, decoLayer, roomSizeId);
			
			UpdateCameraPosition();

			background.Update(gameTime, player.Position);
		}

		public void Draw(SpriteBatch spriteBatch, GameTime gameTime) {
			background.Draw(spriteBatch, gameTime);
			
			DrawTileLayer(spriteBatch, gameTime, backLayer, roomSizeId);
			DrawTileLayer(spriteBatch, gameTime, baseLayer, roomSizeId);
			
			DrawTileEntities(spriteBatch, gameTime);

			DrawEntities(spriteBatch, gameTime);
			
			DrawTileLayer(spriteBatch, gameTime, decoLayer, roomSizeId, currentOpacity);
		}

		#region Tiles

		private void UpdateTileLayer(GameTime gameTime, Tile[,] tileLayer, int roomSizeId) {
			for (int i = 0; i < RoomSize[roomSizeId][0]; i++) {
				for (int j = 0; j < RoomSize[roomSizeId][1]; j++) {
					if (tileLayer[i, j] != null && tileLayer[i, j].IsCollapsed) {
						tileLayer[i, j] = null;
					}

					if (tileLayer[i, j] != null) {
						tileLayer[i, j].Update(gameTime);
					}
				}
			}
		}

		private void DrawTileLayer(SpriteBatch spriteBatch, GameTime gameTime, Tile[,] tileLayer, int roomSizeId, float opacity = 1.0f) {
			for (int i = 0; i < RoomSize[roomSizeId][0]; i++) {
				for (int j = 0; j < RoomSize[roomSizeId][1]; j++) {
					if (tileLayer[i, j] != null) {
						tileLayer[i, j].Draw(spriteBatch, gameTime, opacity);
					}
				}
			}
		}

		public Point GetTileLayerCoords(float posX, float posY) {
			int x = (int)Math.Floor(posX / tileSideSize);
			int y = (int)Math.Floor(posY / tileSideSize);

			return new Point(x, y);
		}

		public Point GetTileCoordsFromLayer(int x, int y) {
			return new Point(x * tileSideSize, y * tileSideSize);
		}

		public Tile GetTile(float posX, float posY, string layer = "base") {
			int x = (int)Math.Floor(posX / tileSideSize);
			int y = (int)Math.Floor(posY / tileSideSize);

			if (x >= 0 && x < roomWidth && y >= 0 && y < roomHeigth) {
				switch (layer) {
					default:
						return null;
					case "base":
						return baseLayer[x, y];
					case "back":
						return backLayer[x, y];
					case "deco":
						return decoLayer[x, y];
					case "functional":
						return functionalLayer[x, y];
				}
			}

			return null;
		}

		public void DestroyTile(Tile tile) {
			Point layerCoords = GetTileLayerCoords(tile.TilePosition.X, tile.TilePosition.Y);

			if (layerCoords.X >= 0 && layerCoords.X < roomWidth &&
				layerCoords.Y >= 0 && layerCoords.Y < roomHeigth) {
				baseLayer[layerCoords.X, layerCoords.Y] = null;
			}
		}

		#region TileEntities

		public void SpawnTileEntity(Tile tile) {
			if (tile != null) {
				tileEntities.Add(tile);
			}
		}

		public void DespawnTileEntity(Tile tile) {
			if (tile != null) {
				if (tileEntities.Contains(tile)) {
					tileEntities.Remove(tile);
				}
			}
		}

		private void UpdateTileEntities(GameTime gameTime) {
			foreach (Tile tile in tileEntities) {
				tile.Update(gameTime);
			}
		}

		private void DrawTileEntities(SpriteBatch spriteBatch, GameTime gameTime) {
			foreach (Tile tile in tileEntities) {
				tile.Draw(spriteBatch, gameTime);
			}
		}

		public List<Tile> TileEntitiesList {
			get { return tileEntities; }
		}

		#endregion

		#region Triggers

		public virtual void HandleTrigger() {
			isTriggerOn = true;
		}

		private void RestoreTrigger() {
			isTriggerOn = false;
		}

		#endregion

		private void UpdateLayersVisibility(GameTime gameTime, float opacitySpeed) {
			if (!isTriggerOn) {
				if (currentOpacity < 1.0f) {
					currentOpacity += opacitySpeed;
				} else {
					currentOpacity = 1.0f;
				}
			} else if (isTriggerOn) {

				if (currentOpacity > 10e-4f) {
					currentOpacity -= opacitySpeed;
				} else {
					currentOpacity = 0.0f;
				}
			}

			RestoreTrigger();
		}

		#endregion

		#region Entities

		private void SpawnEntitiesScheduled() {
			for (int i = 0; i < entitiesSchedule.Count; i++) {
				Tile tile = entitiesSchedule[i].Key;
				Entity entity = entitiesSchedule[i].Value;

				if (GetTile(tile.TilePosition.X, tile.TilePosition.Y) == null) {
					SpawnEntity(entity);

					entitiesSchedule.RemoveAt(i);
					i--;
				}
			}
		}

		public void SpawnEntityScheduled(Entity entity, Tile tileTrigger) {
			entitiesSchedule.Add(new KeyValuePair<Tile, Entity>(tileTrigger, entity));
		}


		public void SpawnEntity(Entity entity) {
			if (entity != null) {
				entities.Add(entity);
				entity.LoadContent(levelContentManager);
			}
		}

		public void DespawnEntity(Entity entity) {
			if (entity != null) {
				if (!entity.IsAlive) {
					if (entities.Contains(entity)) {
						entities.Remove(entity);
					}
				}
			}
		}

		public void LoadEntitiesContent(ContentManager contentManager) {
			Entity entity;
			for (int i = 0; i < entities.Count; i++) {
				entity = entities[i];
				if (entity != null) {
					entity.LoadContent(contentManager);
				}
			}
		}

		private void UpdateEntities(GameTime gameTime) {
			Entity entity;
			for (int i = 0; i < entities.Count; i++) {
				entity = entities[i];
				
				if (entity != null && !entity.IsAlive) {
					if (!(entity is EntityPlayer)) {
						DespawnEntity(entity);
					}
				}

				if (entity != null) {
					entity.Update(gameTime);
				}
			}
		}

		private void DrawEntities(SpriteBatch spriteBatch, GameTime gameTime) {
			Entity entity;
			for (int i = 0; i < entities.Count; i++) {
				entity = entities[i];
				if (entity != null) {
					entity.Draw(spriteBatch, gameTime);
				}
			}
		}

		public Entity GetEntity(int entityID) {
			return entities.Find((entity1) => { return entity1.EntityID == entityID; });
		}

		public List<Entity> EntitiesList {
			get { return entities; }
		}

		#endregion

		private void UpdateCameraPosition() {
			int roomWidthPixels = roomWidth * Display.TileSideSize;
			int roomHeigthPixels = roomHeigth * Display.TileSideSize;
			int halfScreenWidth = (int)Display.BaseResolution.X / 2;
			int halfScreenHeigth = (int)Display.BaseResolution.Y / 2;

			Matrix cameraOffsetX = Matrix.Identity;
			Matrix cameraOffsetY = Matrix.Identity;
			if (roomWidthPixels > Display.BaseResolution.X) {
				if (player.Position.X >= halfScreenWidth &&
					player.Position.X <= roomWidthPixels - halfScreenWidth) {
					cameraOffsetX = Matrix.CreateTranslation(-player.Position.X + halfScreenWidth, 0, 0);
				} else {
					if (player.Position.X >= halfScreenWidth) {
						cameraOffsetX = Matrix.CreateTranslation(-roomWidthPixels + halfScreenWidth * 2, 0, 0);
					}

					if (player.Position.X <= roomWidthPixels - Display.BaseResolution.X) {
						cameraOffsetX = Matrix.CreateTranslation(0, 0, 0);
					}
					
				}
			}

			if (roomHeigthPixels > Display.BaseResolution.Y) {
				if (player.Position.Y >= halfScreenHeigth &&
					player.Position.Y <= roomHeigthPixels - halfScreenHeigth) {
					cameraOffsetY = Matrix.CreateTranslation(0, -player.Position.Y + halfScreenHeigth, 0);
				} else {
					if (player.Position.Y >= halfScreenHeigth) {
						cameraOffsetY = Matrix.CreateTranslation(0, -roomHeigthPixels + halfScreenHeigth * 2, 0);
					}

					if (player.Position.Y <= roomHeigthPixels - Display.BaseResolution.Y) {
						cameraOffsetY = Matrix.CreateTranslation(0, 0, 0);
					}

				}
			}

			Display.GameMatrix = (cameraOffsetX * cameraOffsetY) * Display.ScreenScale;
		}

		public void AddLevelLoader(LevelLoader levelLoader) {
			this.levelLoader = levelLoader;
		}

		public int RoomWidth {
			get { return roomWidth; }
		}

		public int RoomHeigth {
			get { return roomHeigth; }
		}

		public int RoomSizeId {
			get { return roomSizeId; }
		}

		public EntityPlayer Player {
			get { return player; }
		}

		public EntityCreator EntityCreator {
			get { return levelLoader.GetEntityCreator(); }
		}

		public bool IsLevelLoaded {
			get { return isLevelLoaded; }
		}

		public int LevelId { 
			get { return levelId; }
		}

		public int RoomId {
			get { return roomId; }
		}

		public bool HasLevelSwitchQuery {
			get { return hasLevelSwitchQuery; }
			set { hasLevelSwitchQuery = value; }
		}

		public int[] SwitchLevel {
			get { 
				if (!hasLevelSwitchQuery) {
					return null;
				} else {
					return switchLevel;
				}
			}

			set { switchLevel = value; }
		}

		public SnapshotLevel GetSnapshot() {
			bool[,] baseLayeMask = new bool[baseLayer.GetLength(0), baseLayer.GetLength(1)];
			for (int i = 0; i < baseLayer.GetLength(0); i++) {
				for (int j = 0; j < baseLayer.GetLength(1); j++) {
					Tile tile = baseLayer[i, j];
					if (tile != null) {
						baseLayeMask[i, j] = true;
					}

					if (tile != null && tile is TileChest) {
						if (!((TileChest)tile).IsClosed) {
							baseLayeMask[i, j] = false;
						}
					}
				}
			}

			return new SnapshotLevel(
				baseLayeMask,
				background.GetSnapshot());
		}

		public void RestoreSnapshot(SnapshotLevel snapshot) {
			for (int i = 0; i < snapshot.BaseLayerMask.GetLength(0); i++) {
				for (int j = 0; j < snapshot.BaseLayerMask.GetLength(1); j++) {
					if (!snapshot.BaseLayerMask[i, j]) {
						baseLayer[i, j] = null;
					}
				}
			}

			background.RestoreSnapshot(snapshot.SnapshotBackground);
		}
	}
}
