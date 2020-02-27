using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using WizardPlatformer.Logic.Exceptions;

namespace WizardPlatformer.Logic.Level.LevelLoading {
	class LevelMapper {
		private TileCreator tileCreator;
		private int tileSideSize;

		public LevelMapper(Texture2D tileSet, Point tileSetSize) {
			this.tileSideSize = Display.TileSideSize;

			this.tileCreator = new TileCreator(XMLTileIdMapLoader.XMLLoadTileIdMap(tileSetSize), tileSet, tileSetSize);
		}

		public MappedLevelParts MapUnmappedLevelParts(UnmappedLevelParts unmappedLevelParts) {
			int roomSizeWidth = Level.RoomSize[unmappedLevelParts.RoomSize][0];
			int roomSizeHeigth = Level.RoomSize[unmappedLevelParts.RoomSize][1];

			Tile[,] backLayer = new Tile[roomSizeWidth, roomSizeHeigth];
			Tile[,] baseLayer = new Tile[roomSizeWidth, roomSizeHeigth];
			Tile[,] decoLayer = new Tile[roomSizeWidth, roomSizeHeigth];
			Tile[,] functionalLayer = new Tile[roomSizeWidth, roomSizeHeigth];

			try {
				int currentTileId = 0;
				for (int i = 0, j = 0, k = 0; i < roomSizeWidth * roomSizeHeigth; i++, k++) {
					if (i % roomSizeWidth == 0 && i != 0) {
						j++;
						k = 0;
					}

					currentTileId = unmappedLevelParts.LayerBack[i];
					if (currentTileId != 0) {
						backLayer[k, j] = tileCreator.CreateTile(currentTileId,  k * tileSideSize, j * tileSideSize);
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
			} catch (Exception e) {
				LevelMappingException mappingException = new LevelMappingException("Level mapping error:\n" + e.Message);
				ScreenManager.GetInstance().ChangeScreen(new ScreenError(mappingException), true);
			}

			return new MappedLevelParts(unmappedLevelParts.BackgroundId, unmappedLevelParts.RoomSize, baseLayer, backLayer, decoLayer, functionalLayer);
		}

		public TileCreator TileCreator {
			get { return tileCreator; }
		}
	}
}
