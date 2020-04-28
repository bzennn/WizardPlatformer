using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WizardPlatformer.Logic.Save {
	[Serializable]
	public class SnapshotLevel {
		private bool[,] baseLayerMask;
		private SnapshotBackground snapshotBackground;
		private List<bool> isCheckpointActivated;
		private int levelId;
		private int roomId;

		public SnapshotLevel(bool[,] baseLayerMask, SnapshotBackground snapshotBackground, List<bool> isCheckpointActivated, int levelId, int roomId) {
			this.baseLayerMask = baseLayerMask;
			this.snapshotBackground = snapshotBackground;
			this.isCheckpointActivated = isCheckpointActivated;
			this.levelId = levelId;
			this.roomId = roomId;
		}

		public bool[,] BaseLayerMask {
			get { return baseLayerMask; }
		}

		public SnapshotBackground SnapshotBackground {
			get { return snapshotBackground; }
		}

		public List<bool> IsCheckpointActivated {
			get { return isCheckpointActivated; }
		}

		public int LevelId {
			get { return levelId; }
		}

		public int RoomId {
			get { return roomId; }
		}
	}
}
