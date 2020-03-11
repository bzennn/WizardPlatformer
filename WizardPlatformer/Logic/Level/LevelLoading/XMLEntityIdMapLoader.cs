using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using WizardPlatformer.Logic.Exceptions;

namespace WizardPlatformer.Logic.Level.LevelLoading {
	public class XMLEntityIdMapLoader {
		private static IdMapFormatException entityIdMapFormatException = new IdMapFormatException("Unacceptable ids map format!");

		public static Dictionary<int, string> XMLLoadTileIdMap(int entitiesQuantity) {
			string filePath = "Content/entity/entity_id_map.dat";

			Dictionary<int, string> entityIdDictionary = new Dictionary<int, string>();

			string entityIdStr;
			int entityId;

			string entityMapValueStr;

			if (!File.Exists(filePath)) {
				throw new FileNotFoundException("Entities ids map not found! \nFile: \"" + filePath + "\" not exist!");
			}

			XmlDocument entityIdMap = new XmlDocument();
			entityIdMap.Load(filePath);

			XmlElement xMap = entityIdMap.DocumentElement;
			foreach (XmlNode xEntity in xMap) {
				if (xEntity.Attributes.Count != 2) {
					throw entityIdMapFormatException;
				} else {
					XmlNode attribute = xEntity.Attributes.GetNamedItem("id");
					if (attribute == null) {
						throw entityIdMapFormatException;
					} else {
						entityIdStr = attribute.Value;
					}

					attribute = xEntity.Attributes.GetNamedItem("map_value");
					if (attribute == null) {
						throw entityIdMapFormatException;
					} else {
						entityMapValueStr = attribute.Value;
					}

					if (entityIdStr.Length == 0 || entityMapValueStr.Length == 0) {
						throw entityIdMapFormatException;
					} else {
						if (!int.TryParse(entityIdStr, out entityId)) {
							throw entityIdMapFormatException;
						}

						entityIdDictionary.Add(entityId, entityMapValueStr);
					}
				}
			}

			if (entityIdDictionary.Count != entitiesQuantity) {
				throw entityIdMapFormatException;
			}

			return entityIdDictionary;
		}
	}
}
