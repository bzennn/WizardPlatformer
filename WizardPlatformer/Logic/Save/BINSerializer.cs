using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace WizardPlatformer.Logic.Save {
	public static class BINSerializer {
		private static BinaryFormatter formatter = new BinaryFormatter();

		public static void Serialize<T>(T serializable, string filePath) where T : class {
			using (FileStream fileStream = new FileStream(filePath, FileMode.OpenOrCreate)) {
				formatter.Serialize(fileStream, serializable);

				Console.WriteLine("Object Serialized");
			}
		}
	}
}
