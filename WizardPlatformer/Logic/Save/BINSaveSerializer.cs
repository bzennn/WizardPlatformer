using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace WizardPlatformer.Logic.Save {
	public static class BINSaveSerializer {
		private static BinaryFormatter formatter = new BinaryFormatter();

		public static void Serialize(SnapshotGameplay snapshot) {
			using (FileStream fileStream = new FileStream("snapshot_gameplay.dat", FileMode.OpenOrCreate)) {
				formatter.Serialize(fileStream, snapshot);

				Console.WriteLine("Object Serialized");
			}
		}
	}
}
