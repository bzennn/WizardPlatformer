using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;

namespace WizardPlatformer {
	public class Level {
		public static readonly Dictionary<int, int[]> RoomSize = new Dictionary<int, int[]>(){
			{ 0, new int[2]{ 32, 18}},
			{ 1, new int[2]{ 96, 18}},
			{ 2, new int[2]{ 32, 96}},
			{ 3, new int[2]{ 64, 36}}
		};

		private int levelId;
		private int roomId;
		private int roomSizeId;
		private int roomWidth;
		private int roomHeigth;

		private Texture2D[] background;
		private int TileSideSize;
		private Texture2D tileSet;
		private Point tileSetSize;

		private Tile[,] backLayer;
		private Tile[,] baseLayer;
		private Tile[,] decoLayer;

		private EntityPlayer player;

		public Level(int levelId, int roomId, Texture2D tileSet, Point tileSetSize) {
			this.levelId = levelId;
			this.roomId = roomId;
			this.tileSet = tileSet;
			this.tileSetSize = tileSetSize;
			this.TileSideSize = Display.TileSideSize;
			this.background = new Texture2D[3];
		}

		public void LoadContent(ContentManager contentManager) {
			MappedLevelParts mappedLevelParts = LevelLoader.GetInstance().LoadLevel(levelId, roomId, tileSet, tileSetSize);

			this.backLayer = mappedLevelParts.LayerBack;
			this.baseLayer = mappedLevelParts.LayerBase;
			this.decoLayer = mappedLevelParts.LayerDeco;
			this.roomSizeId = mappedLevelParts.RoomSize;
			this.roomWidth = RoomSize[roomSizeId][0];
			this.roomHeigth = RoomSize[roomSizeId][1];
			//TODO Background load;
			background[0] = contentManager.Load<Texture2D>("background/test_back");

			// Pos 100 4000 for vert level
			player = new EntityPlayer(5, 2, 5.0f, 0, true, 8, 20, 32, 16, 100, 1500, roomSizeId, this);
			player.LoadContent(contentManager);
		}

		public void Update(GameTime gameTime) {
			UpdateLayer(gameTime, backLayer, roomSizeId);
			UpdateLayer(gameTime, baseLayer, roomSizeId);
			
			player.Update(gameTime);

			UpdateLayer(gameTime, decoLayer, roomSizeId);

			UpdateScrollPosition();
		}

		public void Draw(SpriteBatch spriteBatch, GameTime gameTime) {
			DrawBackground(spriteBatch, gameTime, background);
			//spriteBatch.GraphicsDevice.
			DrawLayer(spriteBatch, gameTime, backLayer, roomSizeId);
			DrawLayer(spriteBatch, gameTime, baseLayer, roomSizeId);

			player.Draw(spriteBatch, gameTime);

			DrawLayer(spriteBatch, gameTime, decoLayer, roomSizeId);
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

		private void DrawLayer(SpriteBatch spriteBatch, GameTime gameTime, Tile[,] tileLayer, int roomSizeId) {
			for (int i = 0; i < RoomSize[roomSizeId][0]; i++) {
				for (int j = 0; j < RoomSize[roomSizeId][1]; j++) {
					if (tileLayer[i, j] != null) {
						tileLayer[i, j].Draw(spriteBatch, gameTime);
					}
				}
			}
		}

		private void DrawBackground(SpriteBatch spriteBatch, GameTime gameTime, Texture2D[] background) {
			for (int i = 0; i < background.Length; i++) {
				if (background[i] != null) {
					spriteBatch.Draw(
				background[i],
				Vector2.Zero,
				null,
				Color.White,
				0.0f,
				Vector2.Zero,
				Display.DrawScale,
				SpriteEffects.None,
				0.0f);
				}
			}
		}

		public Point GetTileLayerCoords(float posX, float posY) {
			int x = (int)Math.Floor(posX / TileSideSize);
			int y = (int)Math.Floor(posY / TileSideSize);

			return new Point(x, y);
		}

		public Tile GetTile(float posX, float posY, string layer = "base") {
			int x = (int)Math.Floor(posX / TileSideSize);
			int y = (int)Math.Floor(posY / TileSideSize);

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
				}
			}

			return null;
		}

		public void DestroyTile(Tile tile) {
			Point layerCoords = GetTileLayerCoords(tile.Position.X, tile.Position.Y);

			if (layerCoords.X >= 0 && layerCoords.X < roomWidth &&
				layerCoords.Y >= 0 && layerCoords.Y < roomHeigth) {
				baseLayer[layerCoords.X, layerCoords.Y] = null;
			}
		}

		public void UpdateScrollPosition() {
			int roomWidthPixels = roomWidth * Display.TileSideSize;
			int roomHeigthPixels = roomHeigth * Display.TileSideSize;
			int halfScreenWidth = (int)Display.BaseResolution.X / 2;
			int halfScreenHeigth = (int)Display.BaseResolution.Y / 2;

			Matrix cameraOffsetX = Matrix.Identity;
			Matrix cameraOffsetY = Matrix.Identity;
			if (roomWidthPixels > Display.BaseResolution.X) {
				if (player.Position.X >= halfScreenWidth &&
					player.Position.X <= roomWidthPixels - halfScreenWidth) {
					//Display.GameMatrix = Matrix.CreateTranslation(-player.Position.X + halfScreenWidth, 0, 0) * Display.ScreenScale;
					cameraOffsetX = Matrix.CreateTranslation(-player.Position.X + halfScreenWidth, 0, 0);
				} else {
					if (player.Position.X >= halfScreenWidth) {
						//Display.GameMatrix = Matrix.CreateTranslation(- roomWidthPixels + halfScreenWidth * 2, 0, 0) * Display.ScreenScale;
						cameraOffsetX = Matrix.CreateTranslation(-roomWidthPixels + halfScreenWidth * 2, 0, 0);
					}

					if (player.Position.X <= roomWidthPixels - Display.BaseResolution.X) {
						//Display.GameMatrix = Matrix.CreateTranslation(0, 0, 0) * Display.ScreenScale;
						cameraOffsetX = Matrix.CreateTranslation(0, 0, 0);
					}
					
				}
			}

			if (roomHeigthPixels > Display.BaseResolution.Y) {
				if (player.Position.Y >= halfScreenHeigth &&
					player.Position.Y <= roomHeigthPixels - halfScreenHeigth) {
					//Display.GameMatrix = Matrix.CreateTranslation(0, -player.Position.Y + halfScreenHeigth, 0) * Display.ScreenScale;
					cameraOffsetY = Matrix.CreateTranslation(0, -player.Position.Y + halfScreenHeigth, 0);
				} else {
					if (player.Position.Y >= halfScreenHeigth) {
						//Display.GameMatrix = Matrix.CreateTranslation(0, -roomHeigthPixels + halfScreenHeigth * 2, 0) * Display.ScreenScale;
						cameraOffsetY = Matrix.CreateTranslation(0, -roomHeigthPixels + halfScreenHeigth * 2, 0);
					}

					if (player.Position.Y <= roomHeigthPixels - Display.BaseResolution.Y) {
						//Display.GameMatrix = Matrix.CreateTranslation(0, 0, 0) * Display.ScreenScale;
						cameraOffsetY = Matrix.CreateTranslation(0, 0, 0);
					}

				}
			}

			//if (roomWidthPixels > Display.BaseResolution.X ||
			//	roomHeigthPixels > Display.BaseResolution.Y) {
				Display.GameMatrix = (cameraOffsetX * cameraOffsetY) * Display.ScreenScale;
			//}
		}

		public int RoomWidth {
			get { return roomWidth; }
		}

		public int RoomHeigth {
			get { return roomHeigth; }
		}
	}
}
