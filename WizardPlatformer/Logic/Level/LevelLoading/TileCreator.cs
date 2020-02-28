using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace WizardPlatformer.Logic.Level.LevelLoading {
	public class TileCreator {
		private int calcTileSideSize;
		private Dictionary<int, string> tileIdMap;
		private Texture2D tileSet;
		private Point tileSetSize;

		private Point tilePosOnTexture;

		public TileCreator(Dictionary<int, string> tileIdMap, Texture2D tileSet, Point tileSetSize) {
			this.calcTileSideSize = Display.CalcTileSideSize;
			this.tileIdMap = tileIdMap;
			this.tileSet = tileSet;
			this.tileSetSize = tileSetSize;
		}

		public Tile CreateTile(int tileId, int tilePosX, int tilePosY) {
			tilePosOnTexture = GetTilePosOnTextureById(tileId);

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
				case "destroyable":
					return new TileDestroyable(tileSet, tilePosOnTexture, Tile.CollisionType.IMPASSABLE, Tile.PassType.REGULAR, calcTileSideSize, calcTileSideSize, 0, 0, tilePosX, tilePosY);
				case "collectable":
					return new TileCollectable(tileSet, tilePosOnTexture, Tile.CollisionType.PASSABLE, Tile.PassType.REGULAR, 8, 8, 8, 8, tilePosX, tilePosY);
				case "destroy_collect":
					return new TileDestroyable(tileSet, tilePosOnTexture, Tile.CollisionType.IMPASSABLE, Tile.PassType.REGULAR, calcTileSideSize, calcTileSideSize, 0, 0, tilePosX, tilePosY);
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
					return new TileChest(tileSet, tilePosOnTexture, Tile.CollisionType.PASSABLE, Tile.PassType.REGULAR, calcTileSideSize, calcTileSideSize, 0, 0, tilePosX, tilePosY);
				case "checkpoint":
					return new TileCheckpoint(tileSet, tilePosOnTexture, Tile.CollisionType.PASSABLE, Tile.PassType.REGULAR, calcTileSideSize, calcTileSideSize, 0, 0, tilePosX, tilePosY);
				case "entrance":
					return new TileFunctional(tileSet, tilePosOnTexture, Tile.CollisionType.PASSABLE, Tile.PassType.REGULAR, TileFunctional.FunctionType.ENTRANCE, calcTileSideSize, calcTileSideSize, 0, 0, tilePosX, tilePosY);
				case "exit":
					return new TileFunctional(tileSet, tilePosOnTexture, Tile.CollisionType.PASSABLE, Tile.PassType.REGULAR, TileFunctional.FunctionType.EXIT, calcTileSideSize, calcTileSideSize, 0, 0, tilePosX, tilePosY);
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
	}
}
