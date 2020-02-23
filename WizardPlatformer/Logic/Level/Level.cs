﻿using Microsoft.Xna.Framework;
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
			this.TileSideSize = ScreenResolution.TileSideSize;
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

			player = new EntityPlayer(5, 2, 5.0f, 0, true, 8, 20, 32, 16, 130, 630, roomSizeId, this);
			player.LoadContent(contentManager);
		}

		public void Update(GameTime gameTime) {
			UpdateLayer(gameTime, backLayer, roomSizeId);
			UpdateLayer(gameTime, baseLayer, roomSizeId);
			
			player.Update(gameTime);

			UpdateLayer(gameTime, decoLayer, roomSizeId);
		}

		public void Draw(SpriteBatch spriteBatch, GameTime gameTime) {
			DrawBackground(spriteBatch, gameTime, background);

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
				ScreenResolution.DrawScale,
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

		public Tile GetTile(float posX, float posY) {
			int x = (int)Math.Floor(posX / TileSideSize);
			int y = (int)Math.Floor(posY / TileSideSize);

			if (x >= 0 && x < roomWidth && y >= 0 && y < roomHeigth) {
				return baseLayer[x, y];
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
	}
}