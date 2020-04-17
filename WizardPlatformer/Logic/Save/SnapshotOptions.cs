using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WizardPlatformer.Logic.Save {
	[Serializable]
	public class SnapshotOptions {
		private bool fullscreen;
		private int resolution;

		public SnapshotOptions(bool fullscreen, int resolution) {
			this.fullscreen = fullscreen;
			this.resolution = resolution;
		}

		public bool Fullscreen {
			get { return fullscreen; }
		}

		public int Resolution {
			get { return resolution; }
		}
	}
}
