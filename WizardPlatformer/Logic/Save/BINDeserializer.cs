using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace WizardPlatformer.Logic.Save {
	public static class BINDeserializer {
		private static BinaryFormatter formatter = new BinaryFormatter();

		public static T Deserialize<T>(string filePath) where T : class {
			using (FileStream fileStream = new FileStream(filePath, FileMode.OpenOrCreate)) {
				if (fileStream.Length > 0) {
					T deserializable = (T)formatter.Deserialize(fileStream);

					return deserializable;
				}

				return null;
			}
		}
	}
}
