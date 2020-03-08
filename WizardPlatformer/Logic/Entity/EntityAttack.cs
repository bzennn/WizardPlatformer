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

		public EntityAttack(int TTL, int sourceId, int damage, float velocity, bool emulatePhysics, int heatBoxWidth, int heatBoxHeight, int heatBoxSpritePosX, int heatBoxSpritePosY, int posX, int posY, int roomSizeId, Level level)
			: base(0, damage, velocity, emulatePhysics, heatBoxWidth, heatBoxHeight, heatBoxSpritePosX, heatBoxSpritePosY, posX, posY, roomSizeId, level) {

			this.sourceId = sourceId;

			this.TTL = TTL;

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
				}

				this.Collapse();
			}
		}

		protected override void DrawDebugInfo(SpriteBatch spriteBatch, GameTime gameTime) {
			base.DrawDebugInfo(spriteBatch, gameTime);
		}

		public int Damage {
			get { return this.damage; }
		}

		public int SourceID {
			get { return sourceId; }
		}
	}
}
