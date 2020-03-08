using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WizardPlatformer.Logic.Level;

namespace WizardPlatformer {
	public class EntityRangeAttack : EntityAttack {

		private bool isStartAnimationEnd;

		public EntityRangeAttack(int TTL, int sourceId, int damage, float velocity, bool emulatePhysics, int heatBoxWidth, int heatBoxHeight, int heatBoxSpritePosX, int heatBoxSpritePosY, int posX, int posY, int roomSizeId, Level level, Vector2 directionVector)
			: base(TTL, sourceId, damage, velocity, emulatePhysics, heatBoxWidth, heatBoxHeight, heatBoxSpritePosX, heatBoxSpritePosY, posX, posY, roomSizeId, level) {

			this.isRotatable = true;
			this.isSpriteFlipping = false;

			this.spriteRotation = Geometry.GetAngleBetweenVectors(this.EntityPosition, directionVector);

			this.frameUpdateMillis = 150;

			this.drawDebugInfo = true;
			this.isStartAnimationEnd = false;

			SetStartVelocity();
		}

		public override void LoadContent(ContentManager contentManager) {
			base.LoadContent(contentManager);

			this.sprite = contentManager.Load<Texture2D>("entity/ice_arrow_sprite");
			this.spriteSize = new Point(1, 5);
			this.spriteRotationOrigin = new Vector2(12, 12);
			this.UpdateSpritePosition();
		}

		public override void Update(GameTime gameTime) {
			base.Update(gameTime);

			if (!isStartAnimationEnd) {
				Animator.Animate(0, 0, 3, false, frameTimeCounter, ref currentFrame);
				if (currentFrame.X == 2) {
					isStartAnimationEnd = true;
				}
			} else {
				Animator.Animate(1, 0, 2, true, frameTimeCounter, ref currentFrame);
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

		/*protected override void HandleExtraTile(Tile tile) {
			base.HandleExtraTile(tile);

			if (tile.Collision == Tile.CollisionType.IMPASSABLE) {
				this.Collapse();
				this.level.DespawnEntity(this);
			}
		}*/

		private void SetStartVelocity() {
			float angleCos = (float)Math.Cos(this.spriteRotation);
			float angleSin = (float)Math.Sin(this.spriteRotation);

			if (Math.Abs(angleCos) < 0.05f) {
				angleCos = 0.0f;
			}

			if (Math.Abs(angleSin) < 0.05f) {
				angleSin = 0.0f;
			}

			this.currentVelocity.X = angleCos * this.maxVelocity.X;
			this.currentVelocity.Y = angleSin * this.maxVelocity.X;
		}

		public override void Collapse() {
			this.currentVelocity = Vector2.Zero;

			base.Collapse();
		}
	}
}
