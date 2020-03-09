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
			: base(TTL, sourceId, damage, velocity, emulatePhysics, heatBoxWidth, heatBoxHeight, heatBoxSpritePosX, heatBoxSpritePosY, posX, posY, roomSizeId, level, directionVector) {

			this.isRotatable = true;
			this.isSpriteFlipping = false;

			this.spriteRotation = this.directionAngle;

			this.frameUpdateMillis = 150;

			this.drawDebugInfo = false;
			this.isStartAnimationEnd = false;

			SetStartVelocity();
		}

		public override void LoadContent(ContentManager contentManager) {
			base.LoadContent(contentManager);

			this.sprite = contentManager.Load<Texture2D>("entity/ice_arrow_sprite");
			this.spriteSize = new Point(3, 5);
			this.spriteRotationOrigin = new Vector2(12, 12);
			this.UpdateSpritePosition();
		}

		public override void Update(GameTime gameTime) {
			base.Update(gameTime);

			if (!this.isCollapsing) {
				if (!isStartAnimationEnd) {
					Animator.Animate(0, 0, 3, false, frameTimeCounter, ref currentFrame);
					if (currentFrame.X == 2) {
						isStartAnimationEnd = true;
					}
				} else {
					Animator.Animate(1, 0, 2, true, frameTimeCounter, ref currentFrame);
				}
			}
		}

		public override void Collapse() {
			this.currentVelocity = Vector2.Zero;
			this.isCollapsing = true;

			Animator.Animate(2, 0, 3, true, frameTimeCounter, ref currentFrame);

			if (currentFrame.X == 2) {
				base.Collapse();
			}
		}
	}
}
