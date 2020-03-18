using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace WizardPlatformer.Logic.Save {
	public static class BINSaveDeserializer {
		private static BinaryFormatter formatter = new BinaryFormatter();

		public static SnapshotGameplay Deserialize() {
			using (FileStream fileStream = new FileStream("snapshot_gameplay.dat", FileMode.OpenOrCreate)) {
				if (fileStream.Length > 0) {
					SnapshotGameplay snapshot = (SnapshotGameplay)formatter.Deserialize(fileStream);

					return snapshot;
				}

				return null;
			}
		}
	}
}
