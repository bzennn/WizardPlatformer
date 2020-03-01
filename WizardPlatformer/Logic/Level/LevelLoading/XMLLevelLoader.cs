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
			string layerBase = "";
			string layerBack = "";
			string layerDeco = "";
			string layerFunctional = "";
			List<string> movingPlatforms = new List<string>();
			List<string> entities = new List<string>();

			try {
				if (!File.Exists(filePath)) {
					throw new FileNotFoundException("Level not found! \nFile: \"" + filePath + "\" not exist!");
				}

				XmlDocument room = new XmlDocument();
				room.Load(filePath);

				XmlElement xLevel = room.DocumentElement;
				foreach (XmlNode xPart in xLevel) {
					if (xPart.Name.Equals("room")) {
						if (xPart.Attributes.Count == 2) {
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
							if (xPlatform.Attributes.Count != 5) {
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

								movingPlatforms.Add(platform);
							}
						}
					}
				}
			} catch (Exception e) {
				Exception levelE = new Exception("Level load error:\n" + e.Message);
				ScreenManager.GetInstance().ChangeScreen(new ScreenError(levelE), true);
			}

			return new XMLLevelParts(backgroundId, roomSize, layerBase, layerBack, layerDeco, layerFunctional, movingPlatforms, entities);
		}

		private static UnmappedLevelParts PrepareRawLevel(XMLLevelParts levelParts) {
			string backgroundId = "";
			int roomSize = 0;
			int[] layerBase = null;
			int[] layerBack = null;
			int[] layerDeco = null;
			int[] layerFunctional = null;
			List<int[]> movingPlatforms = new List<int[]>();

			try {
				backgroundId = levelParts.BackgroundId;

				if (!int.TryParse(levelParts.RoomSize, out roomSize)) {
					throw levelFormatException;
				}

				if (backgroundId.Length != 5) {
					throw levelFormatException;
				}

				if (roomSize < 0 || roomSize > Level.RoomSize.Count - 1) {
					throw levelFormatException;
				}

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

					if (platformData.Length != 5) {
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
			} catch (Exception e) {
				Exception levelE = new Exception("Level load error:\n" + e.Message);
				ScreenManager.GetInstance().ChangeScreen(new ScreenError(levelE), true);
			}

			return new UnmappedLevelParts(backgroundId, roomSize, layerBase, layerBack, layerDeco, layerFunctional, movingPlatforms);
		}
	}
}
