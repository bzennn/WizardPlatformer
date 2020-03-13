using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Xml;
using System.IO;
using System.Collections.Generic;
using WizardPlatformer.Logic.Exceptions;

namespace WizardPlatformer.Logic.Level.LevelLoading {
	public class LevelLoader {
		LevelMapper levelMapper;

		public LevelLoader(Texture2D tileSet, Point tileSetSize, Level level) {
			this.levelMapper = new LevelMapper(tileSet, tileSetSize, level);
		}

		public MappedLevelParts LoadLevel(int levelId, int roomId) {
			UnmappedLevelParts unmappedLevelParts = XMLLevelLoader.XMLLoadUnmappedLevelParts(levelId, roomId);

			return levelMapper.MapUnmappedLevelParts(unmappedLevelParts);
		}

		public TileCreator GetTileCreator() {
			return levelMapper.TileCreator;
		}

		public EntityCreator GetEntityCreator() {
			return levelMapper.EntityCreator;
		}
	}
}