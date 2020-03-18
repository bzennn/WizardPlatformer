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
		//public SnapshotLevel() { }

		public SnapshotLevel(bool[,] baseLayerMask, SnapshotBackground snapshotBackground) {
			this.baseLayerMask = baseLayerMask;
			this.snapshotBackground = snapshotBackground;
		}

		public bool[,] BaseLayerMask {
			get { return baseLayerMask; }
		}

		public SnapshotBackground SnapshotBackground {
			get { return snapshotBackground; }
		}
	}
}
