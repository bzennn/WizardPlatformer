using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Xml;
using System.IO;
using System.Collections.Generic;

namespace WizardPlatformer {
	public class LevelLoader {
		private static LevelLoader instance;
		private Exception levelFormatException = new Exception("Unacceptable level format!");
		private Exception tileIdMapFormatException = new Exception("Unacceptable tile ids map format!");
		private Exception mapperException = new Exception("An error has during the mapping tile ids to tiles!");
		private Dictionary<int, string> tileIdMap = new Dictionary<int, string>() {};

		private int TileSideSize;
		private int CalcTileSideSize;

		private LevelLoader() {
			this.TileSideSize = Display.TileSideSize;
			this.CalcTileSideSize = Display.CalcTileSideSize;
		}

		public static LevelLoader GetInstance() {
			if (instance == null) {
				instance = new LevelLoader();
			}

			return instance;
		}

		public MappedLevelParts LoadLevel(int levelId, int roomId, Texture2D tileSet, Point tileSetSize) {
			tileIdMap = ParseTileIdMapXml(tileSetSize);
			string[] levelParts = ParseLevelXml(levelId, roomId);
			UnmappedLevelParts unmappedLevelParts = PrepareLoadedLevelParts(levelParts);

			return MapUnmappedLevelParts(unmappedLevelParts, tileSet, tileSetSize);
		}

		private string[] ParseLevelXml(int levelId, int roomId) {
			string filePath = "Content/level/level_" + levelId + "_" + roomId + ".dat";
			string backgroundId = "";
			string roomSize = "";
			string layerBase = "";
			string layerBack = "";
			string layerDeco = "";
			string layerFunctional = "";

			try {
				if (!File.Exists(filePath)) {
					throw new FileNotFoundException("Level not found! \nFile: \"" + filePath + "\" not exist!");
				}

				XmlDocument room = new XmlDocument();
				room.Load(filePath);

				XmlElement xLevel = room.DocumentElement;
				foreach (XmlNode xRoom in xLevel) {
					if (xRoom.Attributes.Count == 2) {
						XmlNode attribute = xRoom.Attributes.GetNamedItem("backgroundId");
						if (attribute == null) {
							throw levelFormatException;
						} else {
							backgroundId = attribute.Value;
						}

						attribute = xRoom.Attributes.GetNamedItem("size");
						if (attribute == null) {
							throw levelFormatException;
						} else {
							roomSize = attribute.Value;
						}
					}

					foreach (XmlNode xLayer in xRoom) {
						if (xLayer.Attributes.Count != 1) {
							throw levelFormatException;
						} else {
							XmlNode attribute = xLayer.Attributes.GetNamedItem("id");

							if (attribute == null) {
								throw levelFormatException;
							} else {
								if (attribute.Value == "base") {
									if (!xLayer.HasChildNodes) {
										throw levelFormatException;
									} else {
										layerBase = xLayer.FirstChild.InnerText;
									}
								} else if (attribute.Value == "back") {
									if (!xLayer.HasChildNodes) {
										throw levelFormatException;
									} else {
										layerBack = xLayer.FirstChild.InnerText;
									}
								} else if (attribute.Value == "deco") {
									if (!xLayer.HasChildNodes) {
										throw levelFormatException;
									} else {
										layerDeco = xLayer.FirstChild.InnerText;
									}
								} else if (attribute.Value == "functional") {
									if (!xLayer.HasChildNodes) {
										throw levelFormatException;
									} else {
										layerFunctional = xLayer.FirstChild.InnerText;
									}
								}
							}
						}
					}
				}
			} catch (Exception e) {
				Exception levelE = new Exception("Level load error:\n" + e.Message);
				ScreenManager.GetInstance().ChangeScreen(new ScreenError(levelE), true);
			}

			return new string[6] { backgroundId, roomSize, layerBase, layerBack, layerDeco, layerFunctional };
		}

		private UnmappedLevelParts PrepareLoadedLevelParts(string[] levelParts) {
			if (levelParts.Length != 6) {
				throw levelFormatException;
			}
			int backgroundId = 0;
			int roomSize = 0;
			int[] layerBase = null;
			int[] layerBack = null;
			int[] layerDeco = null;
			int[] layerFunctional = null;

			try {
				if (!int.TryParse(levelParts[0], out backgroundId)) {
					throw levelFormatException;
				}

				if (!int.TryParse(levelParts[1], out roomSize)) {
					throw levelFormatException;
				}

				if (backgroundId < 0 || backgroundId > 1) {
					throw levelFormatException;
				}

				if (roomSize < 0 || roomSize > Level.RoomSize.Count - 1) {
					throw levelFormatException;
				}

				int roomTilesQuantity = Level.RoomSize[roomSize][0] * Level.RoomSize[roomSize][1];
				if (levelParts[2].Length > 0) {
					layerBase = Array.ConvertAll(levelParts[2].Split(','), int.Parse);
				} else {
					layerBase = new int[roomTilesQuantity];
				}

				if (levelParts[3].Length > 0) {
					layerBack = Array.ConvertAll(levelParts[3].Split(','), int.Parse);
				} else {
					layerBack = new int[roomTilesQuantity];
				}

				if (levelParts[4].Length > 0) {
					layerDeco = Array.ConvertAll(levelParts[4].Split(','), int.Parse);
				} else {
					layerDeco = new int[roomTilesQuantity];
				}

				if (levelParts[5].Length > 0) {
					layerFunctional = Array.ConvertAll(levelParts[4].Split(','), int.Parse);
				} else {
					layerFunctional = new int[roomTilesQuantity];
				}

				if (layerBase.Length != roomTilesQuantity ||
					layerBack.Length != roomTilesQuantity ||
					layerDeco.Length != roomTilesQuantity ||
					layerFunctional.Length != roomTilesQuantity) {
					throw levelFormatException;
				}
			} catch (Exception e) {
				Exception levelE = new Exception("Level load error:\n" + e.Message);
				ScreenManager.GetInstance().ChangeScreen(new ScreenError(levelE), true);
			}

			return new UnmappedLevelParts(backgroundId, roomSize, layerBase, layerBack, layerDeco, layerFunctional);
		}

		private Dictionary<int, string> ParseTileIdMapXml(Point tileSetSize) {
			string filePath = "Content/tile/tile_id_map.dat";

			Dictionary<int, string> tileIdDictionary = new Dictionary<int, string>();
			int dictionarySize = tileSetSize.X * tileSetSize.Y;
			
			string tileIdStr;
			int tileId;

			string tileMapValueStr;

			try {
				if (!File.Exists(filePath)) {
					throw new FileNotFoundException("Level not found! \nFile: \"" + filePath + "\" not exist!");
				}

				XmlDocument tileIdMap = new XmlDocument();
				tileIdMap.Load(filePath);

				XmlElement xMap = tileIdMap.DocumentElement;
				foreach (XmlNode xTile in xMap) {
					if (xTile.Attributes.Count != 2) {
						throw tileIdMapFormatException;
					} else {
						XmlNode attribute = xTile.Attributes.GetNamedItem("id");
						if (attribute == null) {
							throw tileIdMapFormatException;
						} else {
							tileIdStr = attribute.Value;
						}

						attribute = xTile.Attributes.GetNamedItem("map_value");
						if (attribute == null) {
							throw tileIdMapFormatException;
						} else {
							tileMapValueStr = attribute.Value;
						}

						if (tileIdStr.Length == 0 || tileMapValueStr.Length == 0) {
							throw tileIdMapFormatException;
						} else {
							if (!int.TryParse(tileIdStr, out tileId)) {
								throw tileIdMapFormatException;
							}

							tileIdDictionary.Add(tileId, tileMapValueStr);
						}
					}
				}

				if (tileIdDictionary.Count > dictionarySize) {
					throw tileIdMapFormatException;
				}
			} catch (Exception e) {
				Exception levelE = new Exception("Level load error:\n" + e.Message);
				ScreenManager.GetInstance().ChangeScreen(new ScreenError(levelE), true);
			}

			return tileIdDictionary;
		}

		private MappedLevelParts MapUnmappedLevelParts(UnmappedLevelParts unmappedLevelParts, Texture2D tileSet, Point tileSetSize) {
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
						backLayer[k, j] = CreateTile(currentTileId, tileSet, GetTilePosOnTextureById(currentTileId, tileSetSize), k * TileSideSize, j * TileSideSize);
					}

					currentTileId = unmappedLevelParts.LayerBase[i];
					if (currentTileId != 0) {
						baseLayer[k, j] = CreateTile(currentTileId, tileSet, GetTilePosOnTextureById(currentTileId, tileSetSize), k * TileSideSize, j * TileSideSize);
					}

					currentTileId = unmappedLevelParts.LayerDeco[i];
					if (currentTileId != 0) {
						decoLayer[k, j] = CreateTile(currentTileId, tileSet, GetTilePosOnTextureById(currentTileId, tileSetSize), k * TileSideSize, j * TileSideSize);
					}

					currentTileId = unmappedLevelParts.LayerFunctional[i];
					if (currentTileId != 0) {
						functionalLayer[k, j] = CreateTile(currentTileId, tileSet, GetTilePosOnTextureById(currentTileId, tileSetSize), k * TileSideSize, j * TileSideSize);
					}
				}
			} catch (Exception e) {
				Exception levelE = new Exception("Level load error:\n" + e.Message);
				ScreenManager.GetInstance().ChangeScreen(new ScreenError(levelE), true);
			}

			return new MappedLevelParts(unmappedLevelParts.BackgroundId, unmappedLevelParts.RoomSize, baseLayer, backLayer, decoLayer, functionalLayer);
		}

		private Point GetTilePosOnTextureById(int tileId, Point tileSetSize) {
			int x = (tileId - 1) % tileSetSize.X; // Calc tile x pos by tile id
			int y = (int)System.Math.Floor((double)(tileId - 1) / tileSetSize.X); // Calc tile y pos by tile id

			return new Point(x, y);
		}

		private Tile CreateTile(int tileId, Texture2D tileSet, Point tilePosOnTexture, int tilePosX, int tilePosY) {
			switch(tileIdMap[tileId]) {
				default:
					return new Tile(tileSet, new Point(11, 19), Tile.CollisionType.IMPASSABLE, Tile.PassType.REGULAR, CalcTileSideSize, CalcTileSideSize, 0, 0, tilePosX, tilePosY);
				case "air":
					return null;
				case "solid":
					return new Tile(tileSet, tilePosOnTexture, Tile.CollisionType.IMPASSABLE, Tile.PassType.REGULAR, CalcTileSideSize, CalcTileSideSize, 0, 0, tilePosX, tilePosY);
				case "deco":
					return new Tile(tileSet, tilePosOnTexture, Tile.CollisionType.PASSABLE, Tile.PassType.REGULAR, CalcTileSideSize, CalcTileSideSize, 0, 0, tilePosX, tilePosY);
				case "platform":
					return new Tile(tileSet, tilePosOnTexture, Tile.CollisionType.PLATFORM, Tile.PassType.REGULAR, CalcTileSideSize, 8, 0, 0, tilePosX, tilePosY);
				case "hostile":
					return new Tile(tileSet, tilePosOnTexture, Tile.CollisionType.PASSABLE, Tile.PassType.HOSTILE, CalcTileSideSize, CalcTileSideSize, 0, 0, tilePosX, tilePosY);
				case "destroyable":
					return new TileDestroyable(tileSet, tilePosOnTexture, Tile.CollisionType.IMPASSABLE, Tile.PassType.REGULAR, CalcTileSideSize, CalcTileSideSize, 0, 0, tilePosX, tilePosY);
				case "collectable":
					return new TileCollectable(tileSet, tilePosOnTexture, Tile.CollisionType.PASSABLE, Tile.PassType.REGULAR, 8, 8, 8, 8, tilePosX, tilePosY);
				case "destroy_collect":
					return new TileDestroyable(tileSet, tilePosOnTexture, Tile.CollisionType.IMPASSABLE, Tile.PassType.REGULAR, CalcTileSideSize, CalcTileSideSize, 0, 0, tilePosX, tilePosY);
				case "moving_plat":
					return new TileMovingPlatform(tileSet, tilePosOnTexture, Tile.CollisionType.PLATFORM, Tile.PassType.REGULAR, CalcTileSideSize, CalcTileSideSize, 0, 0, tilePosX, tilePosY);
				case "rail_vert":
					return new TileMovingPlatformRail(tileSet, tilePosOnTexture, Tile.CollisionType.PASSABLE, Tile.PassType.REGULAR, CalcTileSideSize, CalcTileSideSize, 0, 0, tilePosX, tilePosY);
				case "rail_hor":
					return new TileMovingPlatformRail(tileSet, tilePosOnTexture, Tile.CollisionType.PASSABLE, Tile.PassType.REGULAR, CalcTileSideSize, CalcTileSideSize, 0, 0, tilePosX, tilePosY);
				case "rail_up_right":
					return new TileMovingPlatformRail(tileSet, tilePosOnTexture, Tile.CollisionType.PASSABLE, Tile.PassType.REGULAR, CalcTileSideSize, CalcTileSideSize, 0, 0, tilePosX, tilePosY);
				case "rail_up_left":
					return new TileMovingPlatformRail(tileSet, tilePosOnTexture, Tile.CollisionType.PASSABLE, Tile.PassType.REGULAR, CalcTileSideSize, CalcTileSideSize, 0, 0, tilePosX, tilePosY);
				case "rail_down_right":
					return new TileMovingPlatformRail(tileSet, tilePosOnTexture, Tile.CollisionType.PASSABLE, Tile.PassType.REGULAR, CalcTileSideSize, CalcTileSideSize, 0, 0, tilePosX, tilePosY);
				case "rail_down_left":
					return new TileMovingPlatformRail(tileSet, tilePosOnTexture, Tile.CollisionType.PASSABLE, Tile.PassType.REGULAR, CalcTileSideSize, CalcTileSideSize, 0, 0, tilePosX, tilePosY);
				case "chest":
					return new TileChest(tileSet, tilePosOnTexture, Tile.CollisionType.PASSABLE, Tile.PassType.REGULAR, CalcTileSideSize, CalcTileSideSize, 0, 0, tilePosX, tilePosY);
				case "checkpoint":
					return new TileCheckpoint(tileSet, tilePosOnTexture, Tile.CollisionType.PASSABLE, Tile.PassType.REGULAR, CalcTileSideSize, CalcTileSideSize, 0, 0, tilePosX, tilePosY);
				
				
				case "debug":
					return new Tile(tileSet, new Point(11, 19), Tile.CollisionType.IMPASSABLE, Tile.PassType.REGULAR, CalcTileSideSize, CalcTileSideSize, 0, 0, tilePosX, tilePosY);
			}
		}
	}
}