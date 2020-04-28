using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using WizardPlatformer.Logic.Exceptions;

namespace WizardPlatformer.Logic.Level.LevelLoading {
	public static class XMLLevelLoader {
		private static LevelFormatException levelFormatException = new LevelFormatException("Unacceptable level format!");

		public static UnmappedLevelParts XMLLoadUnmappedLevelParts(int levelId, int roomId) {
			return PrepareRawLevel(XMLLoadLevelRaw(levelId, roomId));
		}

		private static XMLLevelParts XMLLoadLevelRaw(int levelId, int roomId) {
			string filePath = "Content/level/level_" + levelId + "_" + roomId + ".dat";
			string backgroundId = "";
			string roomSize = "";
			string saveOnEntrance = "";
			string playerPosX = "";
			string playerPosY = "";
			string layerBase = "";
			string layerBack = "";
			string layerDeco = "";
			string layerFunctional = "";
			List<string> movingPlatforms = new List<string>();
			List<string> entities = new List<string>();
			List<string> chestsLoot = new List<string>();
			List<string> exits = new List<string>();
			List<string> levelComplete = new List<string>();

			if (!File.Exists(filePath)) {
				throw new FileNotFoundException("Level not found! File: \"" + filePath + "\" not exist!");
			}

			XmlDocument room = new XmlDocument();
			room.Load(filePath);

			XmlElement xLevel = room.DocumentElement;
			foreach (XmlNode xPart in xLevel) {
				if (xPart.Name.Equals("room")) {
					if (xPart.Attributes.Count == 5) {
						XmlNode attribute = xPart.Attributes.GetNamedItem("backgroundId");
						if (attribute == null) {
							throw levelFormatException;
						} else {
							backgroundId = attribute.Value;
						}

						attribute = xPart.Attributes.GetNamedItem("size");
						if (attribute == null) {
							throw levelFormatException;
						} else {
							roomSize = attribute.Value;
						}

						attribute = xPart.Attributes.GetNamedItem("saveOnEntrance");
						if (attribute == null) {
							throw levelFormatException;
						} else {
							saveOnEntrance = attribute.Value;
						}

						attribute = xPart.Attributes.GetNamedItem("playerPosX");
						if (attribute == null) {
							throw levelFormatException;
						} else {
							playerPosX = attribute.Value;
						}

						attribute = xPart.Attributes.GetNamedItem("playerPosY");
						if (attribute == null) {
							throw levelFormatException;
						} else {
							playerPosY = attribute.Value;
						}
					}

					foreach (XmlNode xLayer in xPart) {
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
				} else if (xPart.Name.Equals("moving_platforms")) {
					foreach (XmlNode xPlatform in xPart) {
						if (xPlatform.Attributes.Count < 5) {
							throw levelFormatException;
						} else {
							XmlNode attribute = xPlatform.Attributes.GetNamedItem("type");
							string platform = "";
							if (attribute == null) {
								throw levelFormatException;
							} else {
								platform += attribute.Value + ",";
							}

							attribute = xPlatform.Attributes.GetNamedItem("right");
							if (attribute == null) {
								throw levelFormatException;
							} else {
								platform += attribute.Value + ",";
							}

							attribute = xPlatform.Attributes.GetNamedItem("left");
							if (attribute == null) {
								throw levelFormatException;
							} else {
								platform += attribute.Value + ",";
							}

							attribute = xPlatform.Attributes.GetNamedItem("x");
							if (attribute == null) {
								throw levelFormatException;
							} else {
								platform += attribute.Value + ",";
							}

							attribute = xPlatform.Attributes.GetNamedItem("y");
							if (attribute == null) {
								throw levelFormatException;
							} else {
								platform += attribute.Value;
							}

							attribute = xPlatform.Attributes.GetNamedItem("activate_by_entity");
							if (attribute != null) {
								platform += "," + attribute.Value;
							}

							movingPlatforms.Add(platform);
						}
					}
				} else if (xPart.Name.Equals("entities")) {
					foreach (XmlNode xEntity in xPart) {
						if (xEntity.Attributes.Count != 3) {
							throw levelFormatException;
						} else {
							XmlNode attribute = xEntity.Attributes.GetNamedItem("entityId");
							string entity = "";
							if (attribute == null) {
								throw levelFormatException;
							} else {
								entity += attribute.Value + ",";
							}

							attribute = xEntity.Attributes.GetNamedItem("x");
							if (attribute == null) {
								throw levelFormatException;
							} else {
								entity += attribute.Value + ",";
							}

							attribute = xEntity.Attributes.GetNamedItem("y");
							if (attribute == null) {
								throw levelFormatException;
							} else {
								entity += attribute.Value;
							}

							entities.Add(entity);
						}
					}
				} else if (xPart.Name.Equals("chests_loot")) {
					foreach (XmlNode xLoot in xPart) {
						if (xLoot.Attributes.Count < 3) {
							throw levelFormatException;
						} else {
							XmlNode attribute = xLoot.Attributes.GetNamedItem("x");
							string loot = "";
							if (attribute == null) {
								throw levelFormatException;
							} else {
								loot += attribute.Value + ",";
							}

							attribute = xLoot.Attributes.GetNamedItem("y");
							if (attribute == null) {
								throw levelFormatException;
							} else {
								loot += attribute.Value + ",";
							}

							attribute = xLoot.Attributes.GetNamedItem("quantity");
							if (attribute == null) {
								throw levelFormatException;
							} else {
								loot += attribute.Value + ",";
							}

							int quantity = 0;
							if (!int.TryParse(attribute.Value, out quantity)) {
								throw levelFormatException;
							}

							for (int i = 0; i < quantity; i++) {
								attribute = xLoot.Attributes.GetNamedItem("itm" + (i + 1));
								if (attribute == null) {
									throw levelFormatException;
								} else {
									loot += attribute.Value;

									if (i != quantity - 1) {
										loot += ",";
									}
								}
							}

							chestsLoot.Add(loot);
						}
					}
				} else if (xPart.Name.Equals("exits")) {
					foreach (XmlNode xExit in xPart) {
						if (xExit.Attributes.Count != 4) {
							throw levelFormatException;
						} else {
							XmlNode attribute = xExit.Attributes.GetNamedItem("levelId");
							string exit = "";
							if (attribute == null) {
								throw levelFormatException;
							} else {
								exit += attribute.Value + ",";
							}

							attribute = xExit.Attributes.GetNamedItem("roomId");
							if (attribute == null) {
								throw levelFormatException;
							} else {
								exit += attribute.Value + ",";
							}

							attribute = xExit.Attributes.GetNamedItem("x");
							if (attribute == null) {
								throw levelFormatException;
							} else {
								exit += attribute.Value + ",";
							}

							attribute = xExit.Attributes.GetNamedItem("y");
							if (attribute == null) {
								throw levelFormatException;
							} else {
								exit += attribute.Value;
							}

							exits.Add(exit);
						}
					}
				} else if (xPart.Name.Equals("level_complete")) {
					foreach (XmlNode xExit in xPart) {
						if (xExit.Attributes.Count != 4) {
							throw levelFormatException;
						} else {
							XmlNode attribute = xExit.Attributes.GetNamedItem("levelId");
							string exit = "";
							if (attribute == null) {
								throw levelFormatException;
							} else {
								exit += attribute.Value + ",";
							}

							attribute = xExit.Attributes.GetNamedItem("roomId");
							if (attribute == null) {
								throw levelFormatException;
							} else {
								exit += attribute.Value + ",";
							}

							attribute = xExit.Attributes.GetNamedItem("x");
							if (attribute == null) {
								throw levelFormatException;
							} else {
								exit += attribute.Value + ",";
							}

							attribute = xExit.Attributes.GetNamedItem("y");
							if (attribute == null) {
								throw levelFormatException;
							} else {
								exit += attribute.Value;
							}

							levelComplete.Add(exit);
						}
					}
				}
			}

			return new XMLLevelParts(backgroundId, roomSize, saveOnEntrance, playerPosX, playerPosY, layerBase, layerBack, layerDeco, layerFunctional, movingPlatforms, entities, chestsLoot, exits, levelComplete);
		}

		private static UnmappedLevelParts PrepareRawLevel(XMLLevelParts levelParts) {
			string backgroundId = "";
			int roomSize = 0;
			bool saveOnEntrance = false;
			int[] playerPosition = new int[2];
			int[] layerBase = null;
			int[] layerBack = null;
			int[] layerDeco = null;
			int[] layerFunctional = null;
			List<int[]> entities = new List<int[]>();
			List<int[]> movingPlatforms = new List<int[]>();
			Dictionary<string, string[]> chestsLoot = new Dictionary<string, string[]>();
			Dictionary<string, int[]> exits = new Dictionary<string, int[]>();
			Dictionary<string, int[]> levelComplete = new Dictionary<string, int[]>();

			int tmpValue = 0;
			int tileSideSize = Display.TileSideSize;

			backgroundId = levelParts.BackgroundId;

			if (!int.TryParse(levelParts.RoomSize, out roomSize)) {
				throw levelFormatException;
			}

			if (roomSize < 0 || roomSize > Level.RoomSize.Count - 1) {
				throw levelFormatException;
			}

			if (backgroundId.Length != 5) {
				throw levelFormatException;
			}

			if (!int.TryParse(levelParts.SaveOnEntrance, out tmpValue)) {
				throw levelFormatException;
			}

			if (tmpValue < 0 || tmpValue > 1) {
				throw levelFormatException;
			}
			saveOnEntrance = (tmpValue == 1) ? true : false;

			if (!int.TryParse(levelParts.PlayerPosX, out tmpValue)) {
				throw levelFormatException;
			}
			playerPosition[0] = tmpValue;

			if (!int.TryParse(levelParts.PlayerPosY, out tmpValue)) {
				throw levelFormatException;
			}
			playerPosition[1] = tmpValue;

			int roomTilesQuantity = Level.RoomSize[roomSize][0] * Level.RoomSize[roomSize][1];
			if (levelParts.LayerBase.Length > 0) {
				layerBase = Array.ConvertAll(levelParts.LayerBase.Split(','), int.Parse);
			} else {
				layerBase = new int[roomTilesQuantity];
			}

			if (levelParts.LayerBack.Length > 0) {
				layerBack = Array.ConvertAll(levelParts.LayerBack.Split(','), int.Parse);
			} else {
				layerBack = new int[roomTilesQuantity];
			}

			if (levelParts.LayerDeco.Length > 0) {
				layerDeco = Array.ConvertAll(levelParts.LayerDeco.Split(','), int.Parse);
			} else {
				layerDeco = new int[roomTilesQuantity];
			}

			if (levelParts.LayerFunctional.Length > 0) {
				layerFunctional = Array.ConvertAll(levelParts.LayerFunctional.Split(','), int.Parse);
			} else {
				layerFunctional = new int[roomTilesQuantity];
			}

			if (layerBase.Length != roomTilesQuantity ||
				layerBack.Length != roomTilesQuantity ||
				layerDeco.Length != roomTilesQuantity ||
				layerFunctional.Length != roomTilesQuantity) {
				throw levelFormatException;
			}

			int[] platformData;
			foreach (string platform in levelParts.MovingPlatforms) {
				platformData = Array.ConvertAll(platform.Split(','), int.Parse);

				if (platformData.Length < 5) {
					throw levelFormatException;
				} else {
					if (platformData[0] < 0 || platformData[0] > 1) {
						throw levelFormatException;
					}

					if (platformData[1] < 0 || platformData[1] > 1) {
						throw levelFormatException;
					}

					if (platformData[2] < 0 || platformData[2] > 1) {
						throw levelFormatException;
					}

					if (platformData[3] < 0 || platformData[3] > Level.RoomSize[roomSize][0]) {
						throw levelFormatException;
					}

					if (platformData[4] < 0 || platformData[4] > Level.RoomSize[roomSize][1]) {
						throw levelFormatException;
					}

					movingPlatforms.Add(platformData);
				}
			}

			int[] exitData;
			foreach (string exit in levelParts.Exits) {
				exitData = Array.ConvertAll(exit.Split(','), int.Parse);

				if (exitData.Length != 4) {
					throw levelFormatException;
				} else {
					if (exitData[2] < 0 || exitData[2] > Level.RoomSize[roomSize][0]) {
						throw levelFormatException;
					}

					if (exitData[3] < 0 || exitData[3] > Level.RoomSize[roomSize][1]) {
						throw levelFormatException;
					}

					exits.Add((exitData[2] * tileSideSize) + "-" + (exitData[3] * tileSideSize), new int[] { exitData[0], exitData[1] });
				}
			}

			int[] entityData;
			foreach (string entity in levelParts.Entities) {
				entityData = Array.ConvertAll(entity.Split(','), int.Parse);

				if (entityData.Length != 3) {
					throw levelFormatException;
				} else {
					if (entityData[1] < 0 || entityData[1] > Level.RoomSize[roomSize][0]) {
						throw levelFormatException;
					}

					if (entityData[2] < 0 || entityData[2] > Level.RoomSize[roomSize][1]) {
						throw levelFormatException;
					}

					entities.Add(entityData);
				}
			}

			string[] lootData;
			foreach (string loot in levelParts.ChestsLoot) {
				lootData = loot.Split(',');

				int x = 0;
				if (!int.TryParse(lootData[0], out x)) {
					throw levelFormatException;
				}

				int y = 0;
				if (!int.TryParse(lootData[1], out y)) {
					throw levelFormatException;
				}

				int quantity = 0;
				if (!int.TryParse(lootData[2], out quantity)) {
					throw levelFormatException;
				}

				if (lootData.Length != 3 + quantity) {
					throw levelFormatException;
				}

				string[] lootTypes = new string[quantity];
				for (int i = 0; i < quantity; i++) {
					lootTypes[i] = lootData[i + 3];
				}

				chestsLoot.Add(x + "-" + y, lootTypes);
			}

			int[] levelCompleteData;
			foreach (string exit in levelParts.LevelComplete) {
				levelCompleteData = Array.ConvertAll(exit.Split(','), int.Parse);

				if (levelCompleteData.Length != 4) {
					throw levelFormatException;
				} else {
					if (levelCompleteData[2] < 0 || levelCompleteData[2] > Level.RoomSize[roomSize][0]) {
						throw levelFormatException;
					}

					if (levelCompleteData[3] < 0 || levelCompleteData[3] > Level.RoomSize[roomSize][1]) {
						throw levelFormatException;
					}

					levelComplete.Add((levelCompleteData[2] * tileSideSize) + "-" + (levelCompleteData[3] * tileSideSize), new int[] { levelCompleteData[0], levelCompleteData[1] });
				}
			}

			return new UnmappedLevelParts(backgroundId, roomSize, saveOnEntrance, playerPosition, layerBase, layerBack, layerDeco, layerFunctional, movingPlatforms, entities, chestsLoot, exits, levelComplete);
		}
	}
}
