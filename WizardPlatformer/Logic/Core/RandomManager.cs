using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WizardPlatformer {
	public static class RandomManager {
		private static Random random = new Random(DateTime.Now.Millisecond);

		public static Random GetRandom() {
			return random;
		}
	}
}
