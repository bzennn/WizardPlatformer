using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WizardPlatformer.Logic.Exceptions {
	class LevelMappingException : Exception {
		public LevelMappingException(string message) : base(message) {
		}

		public LevelMappingException(string message, Exception innerException) : base(message, innerException) {
		}
	}
}
