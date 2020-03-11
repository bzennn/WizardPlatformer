using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using WizardPlatformer.Logic.Exceptions;

namespace WizardPlatformer.Logic.Level.LevelLoading {
	public static class XMLTileIdMapLoader {
		private static IdMapFormatException tileIdMapFormatException = new IdMapFormatException("Unacceptable tile ids map format!");

		public static Dictionary<int, string> XMLLoadTileIdMap(Point tileSetSize) {
			string filePath = "Content/tile/tile_id_map.dat";

			Dictionary<int, string> tileIdDictionary = new Dictionary<int, string>();
			int dictionarySize = tileSetSize.X * tileSetSize.Y;

			string tileIdStr;
			int tileId;

			string tileMapValueStr;


			if (!File.Exists(filePath)) {
				throw new FileNotFoundException("Tiles ids map not found! \nFile: \"" + filePath + "\" not exist!");
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
			/* catch (Exception e) {
				Exception levelE = new Exception("Level load error:\n" + e.Message);
				ScreenManager.GetInstance().ChangeScreen(new ScreenError(levelE), true);
			}*/

			return tileIdDictionary;
		}
	}
}
