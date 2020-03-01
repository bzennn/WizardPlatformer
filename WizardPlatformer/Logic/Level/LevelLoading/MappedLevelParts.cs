using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace WizardPlatformer.Logic.Level.LevelLoading {
	public class MappedLevelParts {
		string backgroundId;
		int roomSize;
		Tile[,] layerBase;
		Tile[,] layerBack;
		Tile[,] layerDeco;
		Tile[,] layerFunctional;
		List<TileMovingPlatform> movingPlatforms;

		public MappedLevelParts(string backgroundId, int roomSize, Tile[,] layerBase, Tile[,] layerBack, Tile[,] layerDeco, Tile[,] layerFunctional, List<TileMovingPlatform> movingPlatforms) {
			this.backgroundId = backgroundId;
			this.roomSize = roomSize;
			this.layerBase = layerBase;
			this.layerBack = layerBack;
			this.layerDeco = layerDeco;
			this.layerFunctional = layerFunctional;
			this.movingPlatforms = movingPlatforms;
		}

		public string BackgoundId {
			get { return backgroundId; }
		}

		public int RoomSize {
			get { return roomSize; }
		}

		public Tile[,] LayerBase {
			get { return layerBase; }
		}
		public Tile[,] LayerBack {
			get { return layerBack; }
		}
		public Tile[,] LayerDeco {
			get { return layerDeco; }
		}

		public Tile[,] LayerFunctional {
			get { return layerFunctional; }
		}

		public List<TileMovingPlatform> MovingPlatforms {
			get { return movingPlatforms; }
		}
	}
}
