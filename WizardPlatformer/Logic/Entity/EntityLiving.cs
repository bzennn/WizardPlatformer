using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WizardPlatformer.Logic.Level;

namespace WizardPlatformer {
	public class EntityLiving : Entity {
		public EntityLiving(int health, int damage, float velocity, bool emulatePhysics, int hitBoxWidth, int hitBoxHeight, int hitBoxSpritePosX, int hitBoxSpritePosY, int posX, int posY, int roomSizeId, Level level)
			: base(health, damage, velocity, emulatePhysics, hitBoxWidth, hitBoxHeight, hitBoxSpritePosX, hitBoxSpritePosY, posX, posY, roomSizeId, level) {
		}

		public virtual void ConsumeHealth(int health) {
			this.health -= health;

			if (this.health <= 0) {
				this.health = 0;
				this.Collapse();
			}
		}
	}
}
