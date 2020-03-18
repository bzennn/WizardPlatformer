using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WizardPlatformer.Logic.Save {
	[Serializable]
	public class SnapshotGameplay {
		private SnapshotPlayer snapshotPlayer;
		private SnapshotLevel snapshotLevel;
		private int levelId;
		private int roomId;

		public SnapshotGameplay(SnapshotPlayer snapshotPlayer, SnapshotLevel snapshotLevel, int levelId, int roomId) {
			this.snapshotPlayer = snapshotPlayer;
			this.snapshotLevel = snapshotLevel;
			this.levelId = levelId;
			this.roomId = roomId;
		}

		public SnapshotPlayer SnapshotPlayer {
			get { return snapshotPlayer; }
		}

		public SnapshotLevel SnapshotLevel {
			get { return snapshotLevel; }
		}

		public int LevelId {
			get { return levelId; }
		}

		public int RoomId {
			get { return roomId; }
		}
	}
}
