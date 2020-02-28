using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace WizardPlatformer.Logic.Level.LevelLoading {
	public class UnmappedLevelParts {
		int backgroundId;
		int roomSize;
		int[] layerBase;
		int[] layerBack;
		int[] layerDeco;
		int[] layerFunctional;
		List<int[]> movingPlatforms;
		List<int[]> entities;

		public UnmappedLevelParts(int backgroundId, int roomSize, int[] layerBase, int[] layerBack, int[] layerDeco, int[] layerFunctional, List<int[]> movingPlatforms) {
			this.backgroundId = backgroundId;
			this.roomSize = roomSize;
			this.layerBase = layerBase;
			this.layerBack = layerBack;
			this.layerDeco = layerDeco;
			this.layerFunctional = layerFunctional;
			this.movingPlatforms = movingPlatforms;
		}

		public int BackgroundId {
			get { return backgroundId; }
		}

		public int RoomSize {
			get { return roomSize; }
		}

		public int[] LayerBase {
			get { return layerBase; }
		}
		public int[] LayerBack {
			get { return layerBack; }
		}
		public int[] LayerDeco {
			get { return layerDeco; }
		}

		public int[] LayerFunctional {
			get { return layerFunctional; }
		}

		public List<int[]> MovingPlatforms {
			get { return movingPlatforms; }
		}
	}
}
