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

		public UnmappedLevelParts(int backgroundId, int roomSize, int[] layerBase, int[] layerBack, int[] layerDeco) {
			this.backgroundId = backgroundId;
			this.roomSize = roomSize;
			this.layerBase = layerBase;
			this.layerBack = layerBack;
			this.layerDeco = layerDeco;
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
	}
}
