using System;
using System.IO;
using System.Xml;
using WizardPlatformer.Logic.Exceptions;

namespace WizardPlatformer.Logic.Level.LevelLoading {
	public static class XMLLevelLoader {
		private static LevelFormatException levelFormatException = new LevelFormatException("Unacceptable level format!");

		public static UnmappedLevelParts XMLLoadUnmappedLevelParts(int levelId, int roomId) {
			return PrepareRawLevel(XMLLoadLevelRaw(levelId, roomId));
		}

		private static string[] XMLLoadLevelRaw(int levelId, int roomId) {
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

		private static UnmappedLevelParts PrepareRawLevel(string[] levelParts) {
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
					layerFunctional = Array.ConvertAll(levelParts[5].Split(','), int.Parse);
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
	}
}
