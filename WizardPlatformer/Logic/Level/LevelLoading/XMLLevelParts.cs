using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WizardPlatformer.Logic.Level.LevelLoading {
	public class XMLLevelParts {
		private string backgroundId;
		private string roomSize;
		private string saveOnEntrance;
		private string playerPosX;
		private string playerPosY;
		private string layerBase;
		private string layerBack;
		private string layerDeco;
		private string layerFunctional;
		private List<string> movingPlatforms;
		private List<string> entities;
		private List<string> chestsLoot;
		private List<string> exits;
		private List<string> levelComplete;

		public XMLLevelParts(string backgroundId, string roomSize, string saveOnEntrance, string playerPosX, string playerPosY, string layerBase, string layerBack, string layerDeco, string layerFunctional, List<string> movingPlatforms, List<string> entities, List<string> chestsLoot, List<string> exits, List<string> levelComplete) {
			this.backgroundId = backgroundId;
			this.roomSize = roomSize;
			this.saveOnEntrance = saveOnEntrance;
			this.playerPosX = playerPosX;
			this.playerPosY = playerPosY;
			this.layerBase = layerBase;
			this.layerBack = layerBack;
			this.layerDeco = layerDeco;
			this.layerFunctional = layerFunctional;
			this.movingPlatforms = movingPlatforms;
			this.entities = entities;
			this.chestsLoot = chestsLoot;
			this.exits = exits;
			this.levelComplete = levelComplete;
		}

		public string BackgroundId {
			get { return backgroundId; }
		}

		public string RoomSize {
			get { return roomSize; }
		}

		public string SaveOnEntrance {
			get { return saveOnEntrance; }
		}

		public string PlayerPosX {
			get { return playerPosX; }
		}

		public string PlayerPosY {
			get { return playerPosY; }
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

		public List<string> ChestsLoot {
			get { return chestsLoot; }
		}

		public List<string> Exits {
			get { return exits; }
		}

		public List<string> LevelComplete {
			get { return levelComplete; }
		}
	}
}
