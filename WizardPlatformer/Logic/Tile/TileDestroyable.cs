using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WizardPlatformer {
	public class TileDestroyable : Tile {
		public enum DropType {
			NONE,
			COIN,
			MANA_CRYSTAL,
			STAMINA_CRYSTAL,
			HEALTH_CRYSTAL,
			STAMINA_UPGRADE,
			HEALTH_UPGRADE,
			HEART
		}

		private DropType[] drops;
		private int maxDropQuantity;

		public TileDestroyable(Texture2D texture, Point spritePos, CollisionType collision, PassType pass, DropType[] drops, int maxDropQuantity, int heatBoxWidth, int heatBoxHeigth, int heatBoxPosX, int heatBoxPosY, int posX, int posY)
			: base(texture, spritePos, collision, pass, heatBoxWidth, heatBoxHeigth, heatBoxPosX, heatBoxPosY, posX, posY) {

			this.drops = drops;
			this.maxDropQuantity = maxDropQuantity;
		}

		public override void Update(GameTime gameTime) {
			base.Update(gameTime);
		}

		public override void Draw(SpriteBatch spriteBatch, GameTime gameTime, float opacity = 1.0f) {
			base.Draw(spriteBatch, gameTime, opacity);
		}

		public void Destroy() {
			this.Collapse();
		}
	}
}
