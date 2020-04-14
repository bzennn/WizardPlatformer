using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using WizardPlatformer.Logic.Exceptions;

namespace WizardPlatformer.Logic.Level.LevelLoading {
	class LevelMapper {
		private TileCreator tileCreator;
		private EntityCreator entityCreator;

		private int tileSideSize;

		public LevelMapper(Texture2D tileSet, Point tileSetSize, Level level) {
			this.tileSideSize = Display.TileSideSize;

			this.tileCreator = new TileCreator(XMLTileIdMapLoader.XMLLoadTileIdMap(tileSetSize), tileSet, tileSetSize, level);
			this.entityCreator = new EntityCreator(XMLEntityIdMapLoader.XMLLoadTileIdMap(20), level);
		}

		public MappedLevelParts MapUnmappedLevelParts(UnmappedLevelParts unmappedLevelParts) {
			int roomSizeWidth = Level.RoomSize[unmappedLevelParts.RoomSize][0];
			int roomSizeHeigth = Level.RoomSize[unmappedLevelParts.RoomSize][1];

			tileCreator.AddExitsDictionary(unmappedLevelParts.Exits);
			tileCreator.AddChestsLootTable(unmappedLevelParts.ChestsLoot);

			Vector2 playerPosition = new Vector2(unmappedLevelParts.PlayerPosition[0], unmappedLevelParts.PlayerPosition[1]);
			Tile[,] backLayer = new Tile[roomSizeWidth, roomSizeHeigth];
			Tile[,] baseLayer = new Tile[roomSizeWidth, roomSizeHeigth];
			Tile[,] decoLayer = new Tile[roomSizeWidth, roomSizeHeigth];
			Tile[,] functionalLayer = new Tile[roomSizeWidth, roomSizeHeigth];
			List<Entity> entities = new List<Entity>();
			List<TileMovingPlatform> movingPlatforms = new List<TileMovingPlatform>();

			int currentTileId = 0;
			for (int i = 0, j = 0, k = 0; i < roomSizeWidth * roomSizeHeigth; i++, k++) {
				if (i % roomSizeWidth == 0 && i != 0) {
					j++;
					k = 0;
				}

				currentTileId = unmappedLevelParts.LayerBack[i];
				if (currentTileId != 0) {
					backLayer[k, j] = tileCreator.CreateTile(currentTileId, k * tileSideSize, j * tileSideSize);
				}

				currentTileId = unmappedLevelParts.LayerBase[i];
				if (currentTileId != 0) {
					baseLayer[k, j] = tileCreator.CreateTile(currentTileId, k * tileSideSize, j * tileSideSize);
				}

				currentTileId = unmappedLevelParts.LayerDeco[i];
				if (currentTileId != 0) {
					decoLayer[k, j] = tileCreator.CreateTile(currentTileId, k * tileSideSize, j * tileSideSize);
				}

				currentTileId = unmappedLevelParts.LayerFunctional[i];
				if (currentTileId != 0) {
					functionalLayer[k, j] = tileCreator.CreateTile(currentTileId, k * tileSideSize, j * tileSideSize);
				}
			}

			foreach (int[] entityData in unmappedLevelParts.Entities) {
				Entity entity = entityCreator.CreateEntity(entityData[0], entityData[1] * tileSideSize + tileSideSize / 2, entityData[2] * tileSideSize + tileSideSize / 2);

				entities.Add(entity);
			}

			foreach (int[] platformData in unmappedLevelParts.MovingPlatforms) {
				TileMovingPlatform platform = null;

				if (platformData[0] == 0) {
					platform = (TileMovingPlatform)tileCreator.CreateTile(56, platformData[3] * tileSideSize, platformData[4] * tileSideSize);

					if (platformData[1] == 1) {
						Tile platformRight = tileCreator.CreateTile(9, 0, 0);
						platform.SetRightTile(platformRight);
					}

					if (platformData[2] == 1) {
						Tile platformLeft = tileCreator.CreateTile(7, 0, 0);
						platform.SetLeftTile(platformLeft);
					}
				} else if (platformData[0] == 1) {
					platform = (TileMovingPlatform)tileCreator.CreateTile(55, platformData[3] * tileSideSize, platformData[4] * tileSideSize);

					if (platformData[1] == 1) {
						Tile platformRight = tileCreator.CreateTile(45, 0, 0);
						platform.SetRightTile(platformRight);
					}

					if (platformData[2] == 1) {
						Tile platformLeft = tileCreator.CreateTile(43, 0, 0);
						platform.SetLeftTile(platformLeft);
					}
				}

				movingPlatforms.Add(platform);
			}

			return new MappedLevelParts(unmappedLevelParts.BackgroundId, unmappedLevelParts.RoomSize, unmappedLevelParts.SaveOnEntrance, playerPosition, baseLayer, backLayer, decoLayer, functionalLayer, entities, movingPlatforms);
		}

		public TileCreator TileCreator {
			get { return tileCreator; }
		}

		public EntityCreator EntityCreator {
			get { return entityCreator; }
		}
	}
}
