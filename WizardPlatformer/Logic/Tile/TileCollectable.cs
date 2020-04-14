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
			NONE,
			COIN,
			MANA_CRYSTAL,
			STAMINA_CRYSTAL,
			HEALTH_CRYSTAL,
			STAMINA_UPGRADE,
			MANA_UPGRADE,
			DAMAGE_UPGRADE,
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

		public static CollectableType CollectableTypeByString(string type) {
			switch(type) {
				default:
					return TileCollectable.CollectableType.NONE;
				case "none":
					return TileCollectable.CollectableType.NONE;
				case "coin":
					return TileCollectable.CollectableType.COIN;
				case "mana_crystal":
					return TileCollectable.CollectableType.MANA_CRYSTAL;
				case "stamina_crystal":
					return TileCollectable.CollectableType.STAMINA_CRYSTAL;
				case "health_crystal":
					return TileCollectable.CollectableType.HEALTH_CRYSTAL;
				case "stamina_upgrade":
					return TileCollectable.CollectableType.STAMINA_UPGRADE;
				case "mana_upgrade":
					return TileCollectable.CollectableType.MANA_UPGRADE;
				case "damage_upgrade":
					return TileCollectable.CollectableType.DAMAGE_UPGRADE;
				case "heart":
					return TileCollectable.CollectableType.HEART;
			}
		}
	}
}
