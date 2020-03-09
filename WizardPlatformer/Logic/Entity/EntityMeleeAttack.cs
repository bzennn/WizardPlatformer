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
	public class EntityMeleeAttack : EntityAttack {
		
		private bool isStartAnimationEnd;

		public EntityMeleeAttack(int health, int sourceId, int damage, float velocity, bool emulatePhysics, int heatBoxWidth, int heatBoxHeight, int heatBoxSpritePosX, int heatBoxSpritePosY, int posX, int posY, int roomSizeId, Level level, Vector2 directionVector) 
			: base(health, sourceId, damage, velocity, emulatePhysics, heatBoxWidth, heatBoxHeight, heatBoxSpritePosX, heatBoxSpritePosY, posX, posY, roomSizeId, level, directionVector) {

			this.frameUpdateMillis = 40;

			this.drawDebugInfo = false;
			this.isStartAnimationEnd = false;
		}

		public override void LoadContent(ContentManager contentManager) {
			base.LoadContent(contentManager);

			this.sprite = contentManager.Load<Texture2D>("entity/melee_attack_sprite");
			this.spriteSize = new Point(6, 2);
			this.UpdateSpritePosition();
		}

		public override void Update(GameTime gameTime) {
			base.Update(gameTime);

			if (!this.isCollapsing) {
				if (!isStartAnimationEnd) {
					Animator.Animate(0, 0, 6, false, frameTimeCounter, ref currentFrame);

					this.EntityPosition = this.level.GetEntity(EntityPlayer.PlayerID).Position;

					if (currentFrame.X == 5) {
						isStartAnimationEnd = true;
					}
				} else {
					SetStartVelocityX();
					Animator.Animate(1, 0, 1, true, frameTimeCounter, ref currentFrame);
				}
			}
		}
	}
}
