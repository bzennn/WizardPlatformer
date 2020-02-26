using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WizardPlatformer.Logic.Exceptions {
	class TileIdMapFormatException : FormatException {
		public TileIdMapFormatException(string message) : base(message) {
		}

		public TileIdMapFormatException(string message, Exception innerException) : base(message, innerException) {
		}
	}
}
