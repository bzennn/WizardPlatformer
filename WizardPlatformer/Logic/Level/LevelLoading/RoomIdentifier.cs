using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WizardPlatformer.Logic.Level.LevelLoading {
	public class RoomIdentifier {
		private int levelId;
		private int roomId;

		public RoomIdentifier(int levelId, int roomId) {
			this.levelId = levelId;
			this.roomId = roomId;
		}

		public override bool Equals(object obj) {
			return obj is RoomIdentifier id &&
				   levelId == id.levelId &&
				   roomId == id.roomId;
		}

		public int LevelId {
			get { return levelId; }
		}

		public int RoomId {
			get { return roomId; }
		}
	}
}
