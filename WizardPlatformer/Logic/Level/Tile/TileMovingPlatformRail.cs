using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WizardPlatformer {
	public class TileMovingPlatformRail : Tile {
		public enum Direction {
			VERTICAL,
			HORIZONTAL,
			UP_RIGHT,
			UP_LEFT,
			DOWN_RIGHT,
			DOWN_LEFT
		}

		private Direction direction;

		public TileMovingPlatformRail(Texture2D texture, Point spritePos, CollisionType collision, PassType pass, Direction direction, int heatBoxWidth, int heatBoxHeigth, int heatBoxPosX, int heatBoxPosY, int posX, int posY) 
			: base(texture, spritePos, collision, pass, heatBoxWidth, heatBoxHeigth, heatBoxPosX, heatBoxPosY, posX, posY) {

			this.direction = direction;
		}

		public override void Update(GameTime gameTime) {
			base.Update(gameTime);
		}

		public override void Draw(SpriteBatch spriteBatch, GameTime gameTime, float opacity = 1.0f) {
			base.Draw(spriteBatch, gameTime, opacity);
		}

		public Vector2 GetBaseDirectionVelocity(bool invert) {
			return GetBaseDirectionVelocity(direction, invert);
		}

		public Vector2 GetBaseDirectionVelocity(Direction direction, bool invert) {
			Vector2 directionVelocity = Vector2.Zero;

			switch(direction) {
				case Direction.VERTICAL:
					directionVelocity = new Vector2(0, -1);
					break;
				case Direction.HORIZONTAL:
					directionVelocity = new Vector2(-1, 0);
					break;
				case Direction.UP_RIGHT:
					directionVelocity = new Vector2(1, -1);
					break;
				case Direction.UP_LEFT:
					directionVelocity = new Vector2(-1, -1);
					break;
				case Direction.DOWN_RIGHT:
					directionVelocity = new Vector2(1, 1);
					break;
				case Direction.DOWN_LEFT:
					directionVelocity = new Vector2(-1, 1);
					break;
			}

			if (invert) {
				directionVelocity *= -1;
			}

			return directionVelocity;
		}

		public Vector2 GetUpdateDirectionVelocity(Vector2 currentVelocity) {
			Vector2 directionVelocity = currentVelocity;

			switch (direction) {
				case Direction.VERTICAL:
					if (currentVelocity.Y > 0) {
						directionVelocity = new Vector2(0, 1);
					} else {
						directionVelocity = new Vector2(0, -1);
					}
					break;
				case Direction.HORIZONTAL:
					if (currentVelocity.X > 0) {
						directionVelocity = new Vector2(1, 0);
					} else {
						directionVelocity = new Vector2(-1, 0);
					}
					break;
				case Direction.UP_RIGHT:
					if (currentVelocity.X > 0 || currentVelocity.Y < 0) {
						directionVelocity = new Vector2(1, -1);
					} else {
						directionVelocity = new Vector2(-1, 1);
					}
					break;
				case Direction.UP_LEFT:
					if (currentVelocity.X < 0 || currentVelocity.Y < 0) {
						directionVelocity = new Vector2(-1, -1);
					} else {
						directionVelocity = new Vector2(1, 1);
					}
					break;
				case Direction.DOWN_RIGHT:
					if (currentVelocity.X > 0 || currentVelocity.Y > 0) {
						directionVelocity = new Vector2(1, 1);
					} else {
						directionVelocity = new Vector2(-1, -1);
					}
					break;
				case Direction.DOWN_LEFT:
					if (currentVelocity.X < 0 || currentVelocity.Y > 0) {
						directionVelocity = new Vector2(-1, 1);
					} else {
						directionVelocity = new Vector2(1, -1);
					}
					break;
			}

			return directionVelocity;
		}
	}
}
