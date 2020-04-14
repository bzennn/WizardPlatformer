using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace WizardPlatformer.Logic.Level.LevelLoading {
	public class MappedLevelParts {
		private string backgroundId;
		private int roomSize;
		private bool saveOnEntrance;
		private Vector2 playerPosition;
		private Tile[,] layerBase;
		private Tile[,] layerBack;
		private Tile[,] layerDeco;
		private Tile[,] layerFunctional;
		private List<Entity> entities;
		private List<TileMovingPlatform> movingPlatforms;

		public MappedLevelParts(string backgroundId, int roomSize, bool saveOnEntrance, Vector2 playerPosition, Tile[,] layerBase, Tile[,] layerBack, Tile[,] layerDeco, Tile[,] layerFunctional, List<Entity> entities, List<TileMovingPlatform> movingPlatforms) {
			this.backgroundId = backgroundId;
			this.roomSize = roomSize;
			this.saveOnEntrance = saveOnEntrance;
			this.playerPosition = playerPosition;
			this.layerBase = layerBase;
			this.layerBack = layerBack;
			this.layerDeco = layerDeco;
			this.layerFunctional = layerFunctional;
			this.entities = entities;
			this.movingPlatforms = movingPlatforms;
		}

		public string BackgoundId {
			get { return backgroundId; }
		}

		public int RoomSize {
			get { return roomSize; }
		}

		public bool SaveOnEntrance {
			get { return saveOnEntrance; }
		}

		public Vector2 PlayerPosition {
			get { return playerPosition; }
		}

		public Tile[,] LayerBase {
			get { return layerBase; }
		}
		public Tile[,] LayerBack {
			get { return layerBack; }
		}
		public Tile[,] LayerDeco {
			get { return layerDeco; }
		}

		public Tile[,] LayerFunctional {
			get { return layerFunctional; }
		}

		public List<Entity> Entities {
			get { return entities; }
		}

		public List<TileMovingPlatform> MovingPlatforms {
			get { return movingPlatforms; }
		}
	}
}
