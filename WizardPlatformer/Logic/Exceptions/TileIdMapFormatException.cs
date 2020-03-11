using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WizardPlatformer.Logic.Exceptions {
	class IdMapFormatException : FormatException {
		public IdMapFormatException(string message) : base(message) {
		}

		public IdMapFormatException(string message, Exception innerException) : base(message, innerException) {
		}
	}
}
