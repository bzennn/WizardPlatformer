using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WizardPlatformer.Logic.Level.LevelLoading {
	public class XMLLevelParts {
		private string backgroundId;
		private string roomSize;
		private string layerBase;
		private string layerBack;
		private string layerDeco;
		private string layerFunctional;
		private List<string> movingPlatforms;
		private List<string> entities;

		public XMLLevelParts(string backgroundId, string roomSize, string layerBase, string layerBack, string layerDeco, string layerFunctional, List<string> movingPlatforms, List<string> entities) {
			this.backgroundId = backgroundId;
			this.roomSize = roomSize;
			this.layerBase = layerBase;
			this.layerBack = layerBack;
			this.layerDeco = layerDeco;
			this.layerFunctional = layerFunctional;
			this.movingPlatforms = movingPlatforms;
			this.entities = entities;
		}

		public string BackgroundId {
			get { return backgroundId; }
		}

		public string RoomSize {
			get { return roomSize; }
		}

		public string LayerBase {
			get { return layerBase; }
		}

		public string LayerBack {
			get { return layerBack; }
		}

		public string LayerDeco {
			get { return layerDeco; }
		}

		public string LayerFunctional {
			get { return layerFunctional; }
		}

		public List<string> MovingPlatforms {
			get { return movingPlatforms; }
		}

		public List<string> Entities {
			get { return entities; }
		}
	}
}
