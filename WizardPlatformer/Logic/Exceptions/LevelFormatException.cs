using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WizardPlatformer.Logic.Exceptions {
	class LevelFormatException : FormatException {
		public LevelFormatException(string message) : base(message) {
		}

		public LevelFormatException(string message, System.Exception innerException) : base(message, innerException) {
		} 
	}
}
