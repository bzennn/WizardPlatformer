using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WizardPlatformer.Logic.Level;

namespace WizardPlatformer {
	public class TileMovingPlatform : Tile {
		private Level level;

		private Vector2 velocity;
		private Vector2 previousPosition;
		private float velocityCoefficient;

		private TileMovingPlatformRail rail;
		private TileMovingPlatformRail previousRail;

		private Tile leftTile;
		private Tile rightTile;

		private bool isMoving;
		private bool isEntityOnPlatform;
		Rectangle intersectionHeatbox;

		public TileMovingPlatform(Texture2D texture, Point spritePos, CollisionType collision, PassType pass, float velocityCoefficient, int heatBoxWidth, int heatBoxHeigth, int heatBoxPosX, int heatBoxPosY, int posX, int posY)
			: base(texture, spritePos, collision, pass, heatBoxWidth, heatBoxHeigth, heatBoxPosX, heatBoxPosY, posX, posY) {

			this.level = null;

			this.velocity = Vector2.Zero;
			this.TilePosition += new Vector2(0, 8);
			this.previousPosition = this.TilePosition;
			this.velocityCoefficient = velocityCoefficient;
			
			this.rail = null;
			this.previousRail = null;

			this.leftTile = null;
			this.rightTile = null;

			this.isMoving = true;
			this.isEntityOnPlatform = false;
			UpdateIntersectionHeatbox();

			this.drawDebugInfo = false;
		}

		public override void Update(GameTime gameTime) {
			base.Update(gameTime);

			if (isMoving) {
				UpdateMoving();
			} else {
				UpdateEntityOnPLatform();
			}

			if (isEntityOnPlatform) {
				Activate();
			}
		}

		public override void Draw(SpriteBatch spriteBatch, GameTime gameTime, float opacity = 1.0f) {
			base.Draw(spriteBatch, gameTime, opacity);

			if (rightTile != null) {
				rightTile.Draw(spriteBatch, gameTime);
			}

			if (leftTile != null) {
				leftTile.Draw(spriteBatch, gameTime);
			}
		}

		private void UpdateMoving() {
			previousRail = rail;
			UpdateRail();
			UpdateVelocity();
			
			previousPosition = this.TilePosition;
			UpdatePosition();
		}

		private void UpdateRail() {
			Vector2 centerPosition = this.HeatBox.Center.ToVector2();
			if (level != null) {
				Tile backTile;
				if (rightTile != null && leftTile == null) {
					backTile = level.GetTile(centerPosition.X - rightTile.HeatBox.Width / 2, centerPosition.Y, "base");
				} else if (rightTile == null && leftTile != null) {
					backTile = level.GetTile(centerPosition.X + leftTile.HeatBox.Width / 2, centerPosition.Y, "base");
				} else {
					backTile = level.GetTile(centerPosition.X, centerPosition.Y, "base");
				}
				
				if (backTile is TileMovingPlatformRail) {
					rail = (TileMovingPlatformRail)backTile;
				} else {
					rail = null;
				}
			}
		}

		private void UpdateVelocity() {
			if (rail != null) {
				if (velocity == Vector2.Zero) {
					velocity = rail.GetBaseDirectionVelocity(false) * velocityCoefficient;
				} else {
					if (previousRail != null && rail.GetDirection() != previousRail.GetDirection()) {
						if (rail.GetDirection() == TileMovingPlatformRail.Direction.VERTICAL) {
							this.TilePosition -= velocity;
							this.TilePosition = new Vector2(rail.TilePosition.X, this.TilePosition.Y);
						}

						if (rail.GetDirection() == TileMovingPlatformRail.Direction.HORIZONTAL) {
							this.TilePosition -= velocity;
							this.TilePosition = new Vector2(this.TilePosition.X, rail.TilePosition.Y + 8);
						}
					}
					velocity = rail.GetUpdateDirectionVelocity(velocity) * velocityCoefficient;
				}
			} else {
				velocity *= -1;
			}
		}

		private void UpdatePosition() {
			this.TilePosition += velocity;
			this.TilePosition = new Vector2((int)this.TilePosition.X, (int)this.TilePosition.Y);

			if (rightTile != null) {
				rightTile.TilePosition = new Vector2(this.TilePosition.X + rightTile.HeatBox.Width, this.TilePosition.Y);
			} 
			
			if (leftTile != null) {
				leftTile.TilePosition = new Vector2(this.TilePosition.X - leftTile.HeatBox.Width, this.TilePosition.Y);
			}
		}

		public float VelocityCoefficient {
			get { return velocityCoefficient; }
			set { velocityCoefficient = value; }
		}

		public bool ActivationByEntity {
			get { return isMoving; }
			set { 
				isMoving = !value;
				UpdateMoving();	
			}
		}

		public Vector2 Velocity {
			get { return velocity; }
		}

		public void SetLevel(Level level) {
			this.level = level;
		}

		public void SetRightTile(Tile tile) {
			rightTile = tile;
			rightTile.Collision = CollisionType.IMPASSABLE;
			rightTile.TilePosition = new Vector2(this.TilePosition.X + rightTile.HeatBox.Width , this.TilePosition.Y);
			this.HeatBox = new Rectangle(HeatBox.X, HeatBox.Y, HeatBox.Width + rightTile.HeatBox.Width, HeatBox.Height);

			UpdateIntersectionHeatbox();
		}

		public void SetLeftTile(Tile tile) {
			leftTile = tile;
			leftTile.Collision = CollisionType.IMPASSABLE;
			leftTile.TilePosition = new Vector2(this.TilePosition.X - leftTile.HeatBox.Width, this.TilePosition.Y);
			this.HeatBox = new Rectangle(HeatBox.X, HeatBox.Y, HeatBox.Width + leftTile.HeatBox.Width, HeatBox.Height);
			this.HeatBoxOffset = new Vector2(leftTile.HeatBox.Width, 0);

			UpdateIntersectionHeatbox();
		}

		private void UpdateEntityOnPLatform() {
			foreach (Entity entity in level.EntitiesList) {
				if (entity is EntityLiving) {
					if (entity.HeatBox.Intersects(intersectionHeatbox) &&
						entity.HeatBox.Bottom <= this.HeatBox.Top) {
						isEntityOnPlatform = true;
					}
				}
			}
		}

		private void UpdateIntersectionHeatbox() {
			this.intersectionHeatbox = new Rectangle(this.HeatBox.X - (int)this.HeatBoxOffset.X, this.HeatBox.Y - (int)this.HeatBoxOffset.Y - 1, this.HeatBox.Width, this.HeatBox.Height + 1);
		}

		private void Activate() {
			if (!isMoving) {
				isMoving = true;
				isEntityOnPlatform = false;
			}
		}
	}
}
