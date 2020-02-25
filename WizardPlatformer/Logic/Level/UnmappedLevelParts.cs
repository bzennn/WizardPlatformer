using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace WizardPlatformer {
	public class UnmappedLevelParts {
		int backgroundId;
		int roomSize;
		int[] layerBase;
		int[] layerBack;
		int[] layerDeco;
		int[] layerFunctional;

		public UnmappedLevelParts(int backgroundId, int roomSize, int[] layerBase, int[] layerBack, int[] layerDeco, int[] layerFunctional) {
			this.backgroundId = backgroundId;
			this.roomSize = roomSize;
			this.layerBase = layerBase;
			this.layerBack = layerBack;
			this.layerDeco = layerDeco;
			this.layerFunctional = layerFunctional;
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
	}
}
