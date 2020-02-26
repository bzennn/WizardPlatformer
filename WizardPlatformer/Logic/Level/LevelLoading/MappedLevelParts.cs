using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace WizardPlatformer.Logic.Level.LevelLoading {
	public class MappedLevelParts {
		int backgroundId;
		int roomSize;
		Tile[,] layerBase;
		Tile[,] layerBack;
		Tile[,] layerDeco;
		Tile[,] layerFunctional;

		public MappedLevelParts(int backgroundId, int roomSize, Tile[,] layerBase, Tile[,] layerBack, Tile[,] layerDeco, Tile[,] layerFunctional) {
			this.backgroundId = backgroundId;
			this.roomSize = roomSize;
			this.layerBase = layerBase;
			this.layerBack = layerBack;
			this.layerDeco = layerDeco;
			this.layerFunctional = layerFunctional;
		}

		public int BackgoundId {
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
	}
}
