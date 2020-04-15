using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using WizardPlatformer.Logic.Level;

namespace WizardPlatformer.Logic.Level.LevelLoading {
	public class TileCreator {
		private int calcTileSideSize;
		private int scaleFactor;
		private Dictionary<int, string> tileIdMap;
		private Texture2D tileSet;
		private Point tileSetSize;

		private Point tilePosOnTexture;

		private Level level;
		private Dictionary<string, string[]> chestsLoot;
		private Dictionary<string, int[]> exits;
		private Dictionary<string, int[]> levelCompleteDict;

		public TileCreator(Dictionary<int, string> tileIdMap, Texture2D tileSet, Point tileSetSize, Level level) {
			this.calcTileSideSize = Display.CalcTileSideSize;
			this.scaleFactor = (int)Display.DrawScale.X;
			this.tileIdMap = tileIdMap;
			this.tileSet = tileSet;
			this.tileSetSize = tileSetSize;

			this.level = level;
		}

		public Tile CreateTile(int tileId, int tilePosX, int tilePosY) {
			tilePosOnTexture = GetTilePosOnTextureById(tileId);
			TileCollectable.CollectableType[] drops;

			switch (tileIdMap[tileId]) {
				default:
					return new Tile(tileSet, new Point(11, 19), Tile.CollisionType.IMPASSABLE, Tile.PassType.REGULAR, calcTileSideSize, calcTileSideSize, 0, 0, tilePosX, tilePosY);
				case "air":
					return null;
				case "solid":
					return new Tile(tileSet, tilePosOnTexture, Tile.CollisionType.IMPASSABLE, Tile.PassType.REGULAR, calcTileSideSize, calcTileSideSize, 0, 0, tilePosX, tilePosY);
				case "deco":
					return new Tile(tileSet, tilePosOnTexture, Tile.CollisionType.PASSABLE, Tile.PassType.REGULAR, calcTileSideSize, calcTileSideSize, 0, 0, tilePosX, tilePosY);
				case "platform":
					return new Tile(tileSet, tilePosOnTexture, Tile.CollisionType.PLATFORM, Tile.PassType.REGULAR, calcTileSideSize, 8, 0, 0, tilePosX, tilePosY);
				case "hostile":
					return new Tile(tileSet, tilePosOnTexture, Tile.CollisionType.PASSABLE, Tile.PassType.HOSTILE, calcTileSideSize, calcTileSideSize, 0, 0, tilePosX, tilePosY);
				case "hostile_lava":
					return new TileLiquid(tileSet, tilePosOnTexture, Tile.CollisionType.PASSABLE, Tile.PassType.HOSTILE, TileLiquid.LiquidType.LAVA, calcTileSideSize, calcTileSideSize, 0, 0, tilePosX, tilePosY);
				case "destroyable":
					return new TileDestroyable(tileSet, tilePosOnTexture, Tile.CollisionType.IMPASSABLE, Tile.PassType.REGULAR, null, 0, 0, calcTileSideSize, calcTileSideSize, 0, 0, tilePosX, tilePosY, level);
				case "collectable_coin":
					return new TileCollectable(tileSet, tilePosOnTexture, Tile.CollisionType.PASSABLE, Tile.PassType.REGULAR, TileCollectable.CollectableType.COIN, 8, 8, 8, 8, tilePosX, tilePosY);
				case "collectable_heart":
					return new TileCollectable(tileSet, tilePosOnTexture, Tile.CollisionType.PASSABLE, Tile.PassType.REGULAR, TileCollectable.CollectableType.HEART, 8, 8, 8, 8, tilePosX, tilePosY);
				case "collectable_stamina":
					return new TileCollectable(tileSet, tilePosOnTexture, Tile.CollisionType.PASSABLE, Tile.PassType.REGULAR, TileCollectable.CollectableType.STAMINA_CRYSTAL, 8, 8, 8, 8, tilePosX, tilePosY);
				case "collectable_mana":
					return new TileCollectable(tileSet, tilePosOnTexture, Tile.CollisionType.PASSABLE, Tile.PassType.REGULAR, TileCollectable.CollectableType.MANA_CRYSTAL, 8, 8, 8, 8, tilePosX, tilePosY);
				case "collectable_health":
					return new TileCollectable(tileSet, tilePosOnTexture, Tile.CollisionType.PASSABLE, Tile.PassType.REGULAR, TileCollectable.CollectableType.HEALTH_CRYSTAL, 8, 8, 8, 8, tilePosX, tilePosY);
				case "destroyable_jar":
					drops = new TileCollectable.CollectableType[] { TileCollectable.CollectableType.COIN, TileCollectable.CollectableType.HEALTH_CRYSTAL, TileCollectable.CollectableType.STAMINA_CRYSTAL };
					return new TileDestroyable(tileSet, tilePosOnTexture, Tile.CollisionType.IMPASSABLE, Tile.PassType.REGULAR, drops, 2, 70, calcTileSideSize, calcTileSideSize, 0, 0, tilePosX, tilePosY, level);
				case "destroyable_stone":
					drops = new TileCollectable.CollectableType[] { TileCollectable.CollectableType.COIN, TileCollectable.CollectableType.MANA_CRYSTAL, TileCollectable.CollectableType.STAMINA_CRYSTAL };
					return new TileDestroyable(tileSet, tilePosOnTexture, Tile.CollisionType.IMPASSABLE, Tile.PassType.REGULAR, drops, 1, 50, calcTileSideSize, calcTileSideSize, 0, 0, tilePosX, tilePosY, level);
				case "destroyable_crate":
					drops = new TileCollectable.CollectableType[] { TileCollectable.CollectableType.COIN, TileCollectable.CollectableType.HEALTH_CRYSTAL, TileCollectable.CollectableType.STAMINA_CRYSTAL};
					return new TileDestroyable(tileSet, tilePosOnTexture, Tile.CollisionType.IMPASSABLE, Tile.PassType.REGULAR, drops, 3, 90, calcTileSideSize, calcTileSideSize, 0, 0, tilePosX, tilePosY, level);
				case "destroyable_barrel":
					drops = new TileCollectable.CollectableType[] { TileCollectable.CollectableType.COIN, TileCollectable.CollectableType.HEALTH_CRYSTAL, TileCollectable.CollectableType.MANA_CRYSTAL };
					return new TileDestroyable(tileSet, tilePosOnTexture, Tile.CollisionType.IMPASSABLE, Tile.PassType.REGULAR, drops, 5, 85, calcTileSideSize, calcTileSideSize, 0, 0, tilePosX, tilePosY, level);
				case "moving_plat_st":
					return new TileMovingPlatform(tileSet, tilePosOnTexture, Tile.CollisionType.IMPASSABLE, Tile.PassType.REGULAR, 2.00f, calcTileSideSize, 8, 0, 0, tilePosX, tilePosY);
				case "moving_plat_gr":
					return new TileMovingPlatform(tileSet, tilePosOnTexture, Tile.CollisionType.IMPASSABLE, Tile.PassType.REGULAR, 2.00f, calcTileSideSize, 8, 0, 0, tilePosX, tilePosY);
				case "rail_vert":
					return new TileMovingPlatformRail(tileSet, tilePosOnTexture, Tile.CollisionType.PASSABLE, Tile.PassType.REGULAR, TileMovingPlatformRail.Direction.VERTICAL, calcTileSideSize, calcTileSideSize, 0, 0, tilePosX, tilePosY);
				case "rail_hor":
					return new TileMovingPlatformRail(tileSet, tilePosOnTexture, Tile.CollisionType.PASSABLE, Tile.PassType.REGULAR, TileMovingPlatformRail.Direction.HORIZONTAL, calcTileSideSize, calcTileSideSize, 0, 0, tilePosX, tilePosY);
				case "rail_up_right":
					return new TileMovingPlatformRail(tileSet, tilePosOnTexture, Tile.CollisionType.PASSABLE, Tile.PassType.REGULAR, TileMovingPlatformRail.Direction.UP_RIGHT, calcTileSideSize, calcTileSideSize, 0, 0, tilePosX, tilePosY);
				case "rail_up_left":
					return new TileMovingPlatformRail(tileSet, tilePosOnTexture, Tile.CollisionType.PASSABLE, Tile.PassType.REGULAR, TileMovingPlatformRail.Direction.UP_LEFT, calcTileSideSize, calcTileSideSize, 0, 0, tilePosX, tilePosY);
				case "rail_down_right":
					return new TileMovingPlatformRail(tileSet, tilePosOnTexture, Tile.CollisionType.PASSABLE, Tile.PassType.REGULAR, TileMovingPlatformRail.Direction.DOWN_RIGHT, calcTileSideSize, calcTileSideSize, 0, 0, tilePosX, tilePosY);
				case "rail_down_left":
					return new TileMovingPlatformRail(tileSet, tilePosOnTexture, Tile.CollisionType.PASSABLE, Tile.PassType.REGULAR, TileMovingPlatformRail.Direction.DOWN_LEFT, calcTileSideSize, calcTileSideSize, 0, 0, tilePosX, tilePosY);
				case "chest":
					drops = new TileCollectable.CollectableType[0];
					if (chestsLoot != null && chestsLoot.Count != 0) {
						string key = tilePosX / calcTileSideSize / scaleFactor + "-" + tilePosY / calcTileSideSize / scaleFactor;
						if (chestsLoot.ContainsKey(key)) {
							string[] lootTypes = chestsLoot[key];
							if (lootTypes != null) {
								drops = new TileCollectable.CollectableType[lootTypes.Length];
								for (int i = 0; i < lootTypes.Length; i++) {
									drops[i] = TileCollectable.CollectableTypeByString(lootTypes[i]);
								}
							}
						}
					} 
					return new TileChest(tileSet, tilePosOnTexture, Tile.CollisionType.PASSABLE, Tile.PassType.REGULAR, drops, -1, 100, 100, calcTileSideSize, calcTileSideSize, 0, 0, tilePosX, tilePosY, level);
				case "checkpoint":
					return new TileCheckpoint(tileSet, tilePosOnTexture, Tile.CollisionType.PASSABLE, Tile.PassType.REGULAR, calcTileSideSize, calcTileSideSize, 0, 0, tilePosX, tilePosY, level);
				case "checkpoint_fire":
					return new TileCheckpointFire(tileSet, tilePosOnTexture, Tile.CollisionType.PASSABLE, Tile.PassType.REGULAR, calcTileSideSize, calcTileSideSize, 0, 0, tilePosX, tilePosY);
				case "entrance":
					return new TileFunctional(tileSet, tilePosOnTexture, Tile.CollisionType.PASSABLE, Tile.PassType.REGULAR, TileFunctional.FunctionType.ENTRANCE, calcTileSideSize, calcTileSideSize, 0, 0, tilePosX, tilePosY);
				case "exit":
					TileFunctional exit = new TileFunctional(tileSet, tilePosOnTexture, Tile.CollisionType.PASSABLE, Tile.PassType.REGULAR, TileFunctional.FunctionType.EXIT, calcTileSideSize, calcTileSideSize, 0, 0, tilePosX, tilePosY);
					if (exits.ContainsKey(tilePosX + "-" + tilePosY)) {
						int[] exitLevel = exits[tilePosX + "-" + tilePosY];
						exit.LevelId = exitLevel[0];
						exit.RoomId = exitLevel[1];
					}
					return exit;
				case "level_complete":
					TileFunctional levelComplete = new TileFunctional(tileSet, tilePosOnTexture, Tile.CollisionType.PASSABLE, Tile.PassType.REGULAR, TileFunctional.FunctionType.LEVEL_COMPLETE, calcTileSideSize, calcTileSideSize, 0, 0, tilePosX, tilePosY);
					if (levelCompleteDict.ContainsKey(tilePosX + "-" + tilePosY)) {
						int[] exitLevel = levelCompleteDict[tilePosX + "-" + tilePosY];
						levelComplete.LevelId = exitLevel[0];
						levelComplete.RoomId = exitLevel[1];
					}
					return levelComplete;
				case "trigger":
					return new TileFunctional(tileSet, tilePosOnTexture, Tile.CollisionType.PASSABLE, Tile.PassType.REGULAR, TileFunctional.FunctionType.TRIGGER, calcTileSideSize, calcTileSideSize, 0, 0, tilePosX, tilePosY);
				case "death":
					return new TileFunctional(tileSet, tilePosOnTexture, Tile.CollisionType.PASSABLE, Tile.PassType.REGULAR, TileFunctional.FunctionType.DEADLY, calcTileSideSize, calcTileSideSize, 0, 0, tilePosX, tilePosY);
				case "debug":
					return new Tile(tileSet, tilePosOnTexture, Tile.CollisionType.IMPASSABLE, Tile.PassType.REGULAR, calcTileSideSize, calcTileSideSize, 0, 0, tilePosX, tilePosY);
			}
		}

		private Point GetTilePosOnTextureById(int tileId) {
			int x = (tileId - 1) % tileSetSize.X; // Calc tile x pos by tile id
			int y = (int)System.Math.Floor((double)(tileId - 1) / tileSetSize.X); // Calc tile y pos by tile id

			return new Point(x, y);
		}

		public void AddExitsDictionary(Dictionary<string, int[]> exits) {
			this.exits = exits;
		}

		public void AddChestsLootDictionary(Dictionary<string, string[]> chestsLoot) {
			this.chestsLoot = chestsLoot;
		}

		public void AddLevelCompleteTileDictionary(Dictionary<string, int[]> levelComplete) {
			this.levelCompleteDict = levelComplete;
		}
	}
}
