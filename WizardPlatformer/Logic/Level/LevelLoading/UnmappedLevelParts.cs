using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace WizardPlatformer.Logic.Level.LevelLoading {
	public class UnmappedLevelParts {
		private string backgroundId;
		private int roomSize;
		private bool saveOnEntrance;

		private int[] playerPosition;

		private int[] layerBase;
		private int[] layerBack;
		private int[] layerDeco;
		private int[] layerFunctional;
		private List<int[]> movingPlatforms;
		private List<int[]> entities;
		Dictionary<string, string[]> chestsLoot;
		private Dictionary<string, int[]> exits;

		public UnmappedLevelParts(string backgroundId, int roomSize, bool saveOnEntrance, int[] playerPosition, int[] layerBase, int[] layerBack, int[] layerDeco, int[] layerFunctional, List<int[]> movingPlatforms, List<int[]> entities, Dictionary<string, string[]> chestsLoot, Dictionary<string, int[]> exits) {
			this.backgroundId = backgroundId;
			this.roomSize = roomSize;
			this.saveOnEntrance = saveOnEntrance;
			this.playerPosition = playerPosition;
			this.layerBase = layerBase;
			this.layerBack = layerBack;
			this.layerDeco = layerDeco;
			this.layerFunctional = layerFunctional;
			this.movingPlatforms = movingPlatforms;
			this.entities = entities;
			this.chestsLoot = chestsLoot;
			this.exits = exits;
		}

		public string BackgroundId {
			get { return backgroundId; }
		}

		public int RoomSize {
			get { return roomSize; }
		}

		public bool SaveOnEntrance {
			get { return saveOnEntrance; }
		}

		public int[] PlayerPosition {
			get { return playerPosition; }
		}

		public int[] LayerBase {
			get { return layerBase; }
		}
		public int[] LayerBack {
			get { return layerBack; }
		}
		public int[] LayerDeco {
			get { return layerDeco; }
		}

		public int[] LayerFunctional {
			get { return layerFunctional; }
		}

		public List<int[]> MovingPlatforms {
			get { return movingPlatforms; }
		}

		public List<int[]> Entities {
			get { return entities; }
		}

		public Dictionary<string, string[]> ChestsLoot {
			get { return chestsLoot; }
		}

		public Dictionary<string, int[]> Exits {
			get { return exits; }
		}
	}
}
