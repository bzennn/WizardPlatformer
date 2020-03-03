using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using WizardPlatformer.Logic.Level.LevelLoading;

namespace WizardPlatformer.Logic.Level {
	public class Level {
		public static readonly Dictionary<int, int[]> RoomSize = new Dictionary<int, int[]>(){
			{ 0, new int[2]{ 32, 18}},
			{ 1, new int[2]{ 96, 18}},
			{ 2, new int[2]{ 32, 96}},
			{ 3, new int[2]{ 64, 36}}
		};

		#region Fields

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

		private Point playerStartPosition;
		private EntityPlayer player;
		private List<Entity> entities;

		private List<Tile> tileEntities;

		#endregion

		public Level(int levelId, int roomId, LevelLoader levelLoader, Point playerStartPosition) {
			this.levelId = levelId;
			this.roomId = roomId;
			this.levelLoader = levelLoader;
			this.playerStartPosition = playerStartPosition;
			this.tileSideSize = Display.TileSideSize;
			this.entities = new List<Entity>();
			this.tileEntities = new List<Tile>();

			this.isTriggerOn = false;
			this.currentOpacity = 1.0f;
		}

		public void LoadContent(ContentManager contentManager) {
			levelContentManager = contentManager;
			MappedLevelParts mappedLevelParts = levelLoader.LoadLevel(levelId, roomId);

			backLayer = mappedLevelParts.LayerBack;
			baseLayer = mappedLevelParts.LayerBase;
			decoLayer = mappedLevelParts.LayerDeco;
			functionalLayer = mappedLevelParts.LayerFunctional;
			roomSizeId = mappedLevelParts.RoomSize;
			roomWidth = RoomSize[roomSizeId][0];
			roomHeigth = RoomSize[roomSizeId][1];

			background = new Background(roomWidth, roomHeigth);
			background.LoadContent(contentManager, mappedLevelParts.BackgoundId);

			foreach (TileMovingPlatform platform in mappedLevelParts.MovingPlatforms) {
				if (platform != null) {
					platform.SetLevel(this);
					SpawnTileEntity(platform);
				}
			}

			player = new EntityPlayer(5, 2, 5.0f, 0, true, 8, 20, 32, 16, playerStartPosition.X, playerStartPosition.Y, roomSizeId, this);
			SpawnEntity(player);

			LoadEntitiesContent(contentManager);
		}

		public void Update(GameTime gameTime) {
			UpdateLayersVisibility(gameTime, 0.05f);

			UpdateLayer(gameTime, backLayer, roomSizeId);
			UpdateLayer(gameTime, baseLayer, roomSizeId);

			UpdateTileEntities(gameTime);

			UpdateEntities(gameTime);

			UpdateLayer(gameTime, decoLayer, roomSizeId);
			
			UpdateCameraPosition();

			background.Update(gameTime, player.Position);

			if (InputManager.GetInstance().IsMouseLeftButtonPressed()) {
				SpawnEntity(new EntityRangeAttack(3000, 10, 7.0f, true, 4, 4, 44, 40, (int)player.Position.X, (int)player.Position.Y, roomSizeId, this, InputManager.GetInstance().GetMousePosition()));
			}
		}

		public void Draw(SpriteBatch spriteBatch, GameTime gameTime) {
			background.Draw(spriteBatch, gameTime);
			
			DrawLayer(spriteBatch, gameTime, backLayer, roomSizeId);
			DrawLayer(spriteBatch, gameTime, baseLayer, roomSizeId);
			
			DrawTileEntities(spriteBatch, gameTime);

			DrawEntities(spriteBatch, gameTime);
			
			DrawLayer(spriteBatch, gameTime, decoLayer, roomSizeId, currentOpacity);
		}

		private void UpdateLayer(GameTime gameTime, Tile[,] tileLayer, int roomSizeId) {
			for (int i = 0; i < RoomSize[roomSizeId][0]; i++) {
				for (int j = 0; j < RoomSize[roomSizeId][1]; j++) {
					if (tileLayer[i, j] != null) {
						tileLayer[i, j].Update(gameTime);
					}
				}
			}
		}

		private void DrawLayer(SpriteBatch spriteBatch, GameTime gameTime, Tile[,] tileLayer, int roomSizeId, float opacity = 1.0f) {
			for (int i = 0; i < RoomSize[roomSizeId][0]; i++) {
				for (int j = 0; j < RoomSize[roomSizeId][1]; j++) {
					if (tileLayer[i, j] != null) {
						tileLayer[i, j].Draw(spriteBatch, gameTime, opacity);
					}
				}
			}
		}

		

		#region Tiles

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
			tileEntities.Add(tile);
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

		public void SpawnEntity(Entity entity) {
			entities.Add(entity);
			entity.LoadContent(levelContentManager);
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

		public List<Entity> EntitiesList {
			get { return entities; }
		}

		#endregion

		public void UpdateCameraPosition() {
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

		public int RoomWidth {
			get { return roomWidth; }
		}

		public int RoomHeigth {
			get { return roomHeigth; }
		}

		public int RoomSizeId {
			get { return roomSizeId; }
		}
	}
}
