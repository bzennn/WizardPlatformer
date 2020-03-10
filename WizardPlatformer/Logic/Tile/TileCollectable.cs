using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WizardPlatformer {
	public class TileCollectable : Tile {
		public enum CollectableType { 
			COIN,
			MANA_CRYSTAL,
			STAMINA_CRYSTAL,
			HEALTH_CRYSTAL,
			HEART
		}

		private CollectableType collectableForm;

		public TileCollectable(Texture2D texture, Point spritePos, CollisionType collision, PassType pass, CollectableType collectableForm, int heatBoxWidth, int heatBoxHeigth, int heatBoxPosX, int heatBoxPosY, int posX, int posY)
			: base(texture, spritePos, collision, pass, heatBoxWidth, heatBoxHeigth, heatBoxPosX, heatBoxPosY, posX, posY) {

			this.collectableForm = collectableForm;
		}

		public override void Update(GameTime gameTime) {
			base.Update(gameTime);
		}

		public override void Draw(SpriteBatch spriteBatch, GameTime gameTime, float opacity = 1.0f) {
			base.Draw(spriteBatch, gameTime, opacity);
		}

		public CollectableType CollectableForm {
			get { return collectableForm; }
		}
	}
}
