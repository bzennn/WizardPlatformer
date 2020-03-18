using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WizardPlatformer.Logic.Save {
	[Serializable]
	public class SnapshotBackground {
		private float currentPositionOffsetX;
		private float currentPositionOffsetY;

		private float offsetL1;
		private float offsetL2;
		private float offsetL3;

		private float offsetR1;
		private float offsetR2;
		private float offsetR3;

		private float velocityCoefficientX;
		private float velocityCoefficientY;

		public SnapshotBackground(float currentPositionOffsetX, float currentPositionOffsetY, float offsetL1, float offsetL2, float offsetL3, float offsetR1, float offsetR2, float offsetR3, float velocityCoefficientX, float velocityCoefficientY) {
			this.currentPositionOffsetX = currentPositionOffsetX;
			this.currentPositionOffsetY = currentPositionOffsetY;
			this.offsetL1 = offsetL1;
			this.offsetL2 = offsetL2;
			this.offsetL3 = offsetL3;
			this.offsetR1 = offsetR1;
			this.offsetR2 = offsetR2;
			this.offsetR3 = offsetR3;
			this.velocityCoefficientX = velocityCoefficientX;
			this.velocityCoefficientY = velocityCoefficientY;
		}

		public float CurrentPositionOffsetX {
			get { return currentPositionOffsetX; }
		}

		public float CurrentPositionOffsetY {
			get { return currentPositionOffsetY; }
		}

		public float OffsetL1 {
			get { return offsetL1; }
		}

		public float OffsetL2 {
			get { return offsetL2; }
		}

		public float OffsetL3 {
			get { return offsetL3; }
		}

		public float OffsetR1 {
			get { return offsetR1; }
		}

		public float OffsetR2 {
			get { return offsetR2; }
		}

		public float OffsetR3 {
			get { return offsetR3; }
		}

		public float VelocityCoefficientX {
			get { return velocityCoefficientX; }
		}

		public float VelocityCoefficientY {
			get { return velocityCoefficientY; }
		}
	}
}
