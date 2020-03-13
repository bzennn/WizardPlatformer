using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WizardPlatformer.Logic.Level;

namespace WizardPlatformer {
	public class TileDestroyable : Tile {
		private TileCollectable.CollectableType[] drop;
		private int maxDropQuantity;

		private Level level;
		private EntityCreator entityCreator;

		public TileDestroyable(Texture2D texture, Point spritePos, CollisionType collision, PassType pass, TileCollectable.CollectableType[] drop, int maxDropQuantity, int heatBoxWidth, int heatBoxHeigth, int heatBoxPosX, int heatBoxPosY, int posX, int posY, Level level)
			: base(texture, spritePos, collision, pass, heatBoxWidth, heatBoxHeigth, heatBoxPosX, heatBoxPosY, posX, posY) {

			this.drop = drop;
			this.maxDropQuantity = maxDropQuantity;

			this.level = level;
			this.entityCreator = this.level.EntityCreator;
		}

		public override void Update(GameTime gameTime) {
			base.Update(gameTime);
		}

		public override void Draw(SpriteBatch spriteBatch, GameTime gameTime, float opacity = 1.0f) {
			base.Draw(spriteBatch, gameTime, opacity);
		}

		private void SpawnAllDrop() {
			for (int i = 0; i < drop.Length; i++) {
				for (int j = 0; j < GetDropQuantity(); j++) {
					SpawnDrop(drop[i]);
				}
			}
		} 

		private void SpawnDrop(TileCollectable.CollectableType dropType) {
			Vector2 centerPosition = this.HeatBox.Center.ToVector2();

			switch(dropType) {
				default:
					break;
				case TileCollectable.CollectableType.NONE:
					break;
				case TileCollectable.CollectableType.COIN:
					level.SpawnEntityScheduled(entityCreator.CreateEntity(4, (int)centerPosition.X, (int)centerPosition.Y), this);
					break;
				case TileCollectable.CollectableType.STAMINA_CRYSTAL:
					level.SpawnEntityScheduled(entityCreator.CreateEntity(6, (int)centerPosition.X, (int)centerPosition.Y), this);
					break;
				case TileCollectable.CollectableType.MANA_CRYSTAL:
					level.SpawnEntityScheduled(entityCreator.CreateEntity(5, (int)centerPosition.X, (int)centerPosition.Y), this);
					break;
				case TileCollectable.CollectableType.HEALTH_CRYSTAL:
					level.SpawnEntityScheduled(entityCreator.CreateEntity(7, (int)centerPosition.X, (int)centerPosition.Y), this);
					break;
				case TileCollectable.CollectableType.STAMINA_UPGRADE:
					level.SpawnEntityScheduled(entityCreator.CreateEntity(9, (int)centerPosition.X, (int)centerPosition.Y), this);
					break;
				case TileCollectable.CollectableType.MANA_UPGRADE:
					level.SpawnEntityScheduled(entityCreator.CreateEntity(8, (int)centerPosition.X, (int)centerPosition.Y), this);
					break;
				case TileCollectable.CollectableType.HEART:
					level.SpawnEntityScheduled(entityCreator.CreateEntity(10, (int)centerPosition.X, (int)centerPosition.Y), this);
					break;
			}
		}

		private int GetDropQuantity() {
			return RandomManager.GetRandom().Next(0, maxDropQuantity + 1);
		}

		public void Destroy() {
			this.Collapse();
			SpawnAllDrop();
		}
	}
}
