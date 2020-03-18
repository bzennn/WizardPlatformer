using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace WizardPlatformer.Logic.Level.LevelLoading {
	public class XMLLevelListLoader {
		private static Exception levelsListFormatException = new Exception("Unacceptable ids map format!");

		public static List<int[]> LoadLevelsList() {
			return PrepareLoadedLevelsList(XMLLoadLevelsList());
		}

		private static List<string[]> XMLLoadLevelsList() {
			string filePath = "Content/level/levels_list.dat";

			List<string[]> levelsList = new List<string[]>();

			string levelId;
			string roomId;

			if (!File.Exists(filePath)) {
				throw new FileNotFoundException("Levels list not found! \nFile: \"" + filePath + "\" not exist!");
			}

			XmlDocument entityIdMap = new XmlDocument();
			entityIdMap.Load(filePath);

			XmlElement xLevelsList = entityIdMap.DocumentElement;
			foreach (XmlNode xLevel in xLevelsList) {
				if (xLevel.Attributes.Count != 1) {
					throw levelsListFormatException;
				} else {
					XmlNode levelIdAttr = xLevel.Attributes.GetNamedItem("id");
					if (levelIdAttr == null) {
						throw levelsListFormatException;
					} else {
						levelId = levelIdAttr.Value;
					}

					foreach (XmlNode xRoom in xLevel) {
						if (xRoom.Attributes.Count != 1) {
							throw levelsListFormatException;
						} else {
							XmlNode roomIdAttr = xRoom.Attributes.GetNamedItem("id");
							if (roomIdAttr == null) {
								throw levelsListFormatException;
							} else {
								roomId = roomIdAttr.Value;
							}
						}

						levelsList.Add(new string[] { levelId, roomId });
					}
				}
			}

			return levelsList;
		}

		private static List<int[]> PrepareLoadedLevelsList(List<string[]> unpreparedLevelsList) {
			List<int[]> levelsList = new List<int[]>();

			int levelId = 0;
			int roomId = 0;

			foreach (string[] pair in unpreparedLevelsList) {
				if (!int.TryParse(pair[0], out levelId)) {
					throw levelsListFormatException;
				}

				if (!int.TryParse(pair[1], out roomId)) {
					throw levelsListFormatException;
				}

				levelsList.Add(new int[] { levelId, roomId });
			}

			return levelsList;
		}
	}
}
