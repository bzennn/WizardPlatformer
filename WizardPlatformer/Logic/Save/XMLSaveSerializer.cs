using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace WizardPlatformer.Logic.Save {
	public static class XMLSaveSerializer {
		private static XmlSerializer xmlSerializer = new XmlSerializer(typeof(SnapshotGameplay));

		public static void Serialize(SnapshotGameplay snapshot) {
			using (FileStream fileStream = new FileStream("snapshot_gameplay.xml", FileMode.OpenOrCreate)) {
				xmlSerializer.Serialize(fileStream, snapshot);

				Console.WriteLine("Object Serialized");
			}
		}
	}
}
