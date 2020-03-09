using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WizardPlatformer.Logic.Level;

namespace WizardPlatformer {
	public class EntityAttack : Entity {
		private int sourceId;
		
		protected int TTL;
		protected float directionAngle;

		public EntityAttack(int TTL, int sourceId, int damage, float velocity, bool emulatePhysics, int heatBoxWidth, int heatBoxHeight, int heatBoxSpritePosX, int heatBoxSpritePosY, int posX, int posY, int roomSizeId, Level level, Vector2 directionVector)
			: base(0, damage, velocity, emulatePhysics, heatBoxWidth, heatBoxHeight, heatBoxSpritePosX, heatBoxSpritePosY, posX, posY, roomSizeId, level) {

			this.sourceId = sourceId;

			this.TTL = TTL;
			this.directionAngle = Geometry.GetAngleBetweenVectors(this.EntityPosition, directionVector);
			
			if ((float)Math.Cos(directionAngle) < 0) {
				this.isSpriteFlipped = true;
			}

			this.isGravityOn = false;
			this.hasAcceleration = false;
		}

		public override void Update(GameTime gameTime) {
			base.Update(gameTime);

			TTL -= gameTime.ElapsedGameTime.Milliseconds;

			if (TTL <= 0) {
				this.Collapse();
			}

			if (this.isCollides) {
				Tile tile = this.lastCollide;

				if (tile != null) {
					if (tile is TileDestroyable) {
						((TileDestroyable)tile).Destroy();
					}

					if (tile.Collision == Tile.CollisionType.PLATFORM) {
						this.FallThrough(false);
					}

					if (tile.Collision == Tile.CollisionType.IMPASSABLE) {
						this.Collapse();
					}
				} else {
					this.Collapse();
				}
			}
		}

		protected override void DrawDebugInfo(SpriteBatch spriteBatch, GameTime gameTime) {
			base.DrawDebugInfo(spriteBatch, gameTime);

			if (drawDebugInfo) {
				spriteBatch.DrawString(debugFont, "AngleCos = " + Math.Cos(this.spriteRotation) +
					"\nAngleSin = " + Math.Sin(this.spriteRotation) +
					"\nVelocity = " + this.currentVelocity, heatBox.Location.ToVector2() + new Vector2(60, 0), Color.AntiqueWhite);
			}
		}

		protected void SetStartVelocity() {
			float angleCos = (float)Math.Cos(directionAngle);
			float angleSin = (float)Math.Sin(directionAngle);

			if (Math.Abs(angleCos) < 0.05f) {
				angleCos = 0.0f;
			}

			if (Math.Abs(angleSin) < 0.05f) {
				angleSin = 0.0f;
			}

			this.currentVelocity.X = angleCos * this.maxVelocity.X;
			this.currentVelocity.Y = angleSin * this.maxVelocity.X;
		}

		protected void SetStartVelocityX() {
			float angleCos = (float)Math.Cos(directionAngle);

			if (Math.Abs(angleCos) < 0.05f) {
				angleCos = 0.0f;
			}

			if (angleCos < 0) {
				this.currentVelocity.X = -1 * this.maxVelocity.X;
			} else {
				this.currentVelocity.X = 1 * this.maxVelocity.X;
			}
		}

		protected void SetStartVelocityY() {
			float angleSin = (float)Math.Sin(directionAngle);

			if (Math.Abs(angleSin) < 0.05f) {
				angleSin = 0.0f;
			}

			if (angleSin < 0) {
				this.currentVelocity.Y = -1 * this.maxVelocity.X;
			} else {
				this.currentVelocity.Y = 1 * this.maxVelocity.X;
			}
		}

		public int Damage {
			get { return this.damage; }
		}

		public int SourceID {
			get { return sourceId; }
		}
	}
}
