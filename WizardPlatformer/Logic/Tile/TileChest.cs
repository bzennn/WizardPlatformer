using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WizardPlatformer.Logic.Level;
using WizardPlatformer.Logic.Level.LevelLoading;

namespace WizardPlatformer {
	public class TileChest : Tile {
		private TileCollectable.CollectableType[] drop;
		private bool[] dropSpawned;
		private int maxDropQuantity;
		private int dropChance;

		private Level level;
		private EntityCreator entityCreator;

		private int dropDelay;
		private int dropTimeCounter;
		private int currentDrop;
		private int currentDropQuantity;

		private bool isOpening;
		private bool isClosed;
		private bool isEmpty;

		public TileChest(Texture2D texture, Point spritePos, CollisionType collision, PassType pass, TileCollectable.CollectableType[] drop, int maxDropQuantity, int dropDelay, int dropChance, int heatBoxWidth, int heatBoxHeigth, int heatBoxPosX, int heatBoxPosY, int posX, int posY, Level level)
			: base(texture, spritePos, collision, pass, heatBoxWidth, heatBoxHeigth, heatBoxPosX, heatBoxPosY, posX, posY) {

			this.drop = drop;
			this.dropSpawned = new bool[drop.Length];
			this.maxDropQuantity = maxDropQuantity;
			if (dropChance < 0 || dropChance > 100) {
				this.dropChance = 0;
				throw new ArgumentException();
			} else {
				this.dropChance = dropChance;
			}
			this.level = level;
			this.entityCreator = this.level.EntityCreator;

			this.dropDelay = dropDelay;
			this.dropTimeCounter = 0;
			this.currentDrop = 0;
			this.currentDropQuantity = 0;

			this.spriteSize = new Point(4, 1);
			this.isAnimated = true;

			this.isOpening = false;
			this.isClosed = true;
			this.isEmpty = false;
		}

		public override void Update(GameTime gameTime) {
			base.Update(gameTime);

			if (isOpening) {
				UpdateOpenAnimation();
			}

			if (!isClosed) {
				UpdateDropCounter(gameTime);
				SpawnAllDrop();
			}
		}

		public override void Draw(SpriteBatch spriteBatch, GameTime gameTime, float opacity = 1.0f) {
			base.Draw(spriteBatch, gameTime, opacity);
		}

		public void Open() {
			isOpening = true;
		}

		private void UpdateOpenAnimation() {
			if (Animator.Animate(0, 0, 4, false, this.frameTimeCounter, ref this.currentFrame)) {
				isClosed = false;
				isOpening = false;
			}
		}

		private void UpdateDropCounter(GameTime gameTime) {
			dropTimeCounter += gameTime.ElapsedGameTime.Milliseconds;
			if (dropTimeCounter > dropDelay) {
				dropTimeCounter = 0;
			}
		}

		private void SpawnAllDrop() {
			if (dropTimeCounter == 0) {
				if (maxDropQuantity > 0) {
					if (currentDropQuantity == 0) {
						currentDropQuantity = GetDropQuantity();
						maxDropQuantity -= currentDropQuantity;
						currentDrop++;
					} else {
						if (currentDrop < drop.Length && drop[currentDrop] != null) {
							if (Roll()) {
								SpawnDrop(drop[currentDrop]);
							}
							currentDropQuantity--;
						}
					}
				} else if (maxDropQuantity == -1) {
					for (int i = 0; i < drop.Length; i++) {
						if (Roll()) {
							SpawnDrop(drop[i]);
						}
						if (i == drop.Length - 1) {
							maxDropQuantity = 0;
						}
					}
				} else {
					isEmpty = true;
				}

			}
		}

		private void SpawnDrop(TileCollectable.CollectableType dropType) {
			Vector2 centerPosition = this.HeatBox.Center.ToVector2();

			switch (dropType) {
				default:
					break;
				case TileCollectable.CollectableType.NONE:
					break;
				case TileCollectable.CollectableType.COIN:
					level.SpawnEntity(entityCreator.CreateEntity(4, (int)centerPosition.X, (int)centerPosition.Y));
					break;
				case TileCollectable.CollectableType.STAMINA_CRYSTAL:
					level.SpawnEntity(entityCreator.CreateEntity(6, (int)centerPosition.X, (int)centerPosition.Y));
					break;
				case TileCollectable.CollectableType.MANA_CRYSTAL:
					level.SpawnEntity(entityCreator.CreateEntity(5, (int)centerPosition.X, (int)centerPosition.Y));
					break;
				case TileCollectable.CollectableType.HEALTH_CRYSTAL:
					level.SpawnEntity(entityCreator.CreateEntity(7, (int)centerPosition.X, (int)centerPosition.Y));
					break;
				case TileCollectable.CollectableType.STAMINA_UPGRADE:
					level.SpawnEntity(entityCreator.CreateEntity(9, (int)centerPosition.X, (int)centerPosition.Y));
					break;
				case TileCollectable.CollectableType.MANA_UPGRADE:
					level.SpawnEntity(entityCreator.CreateEntity(8, (int)centerPosition.X, (int)centerPosition.Y));
					break;
				case TileCollectable.CollectableType.HEART:
					level.SpawnEntity(entityCreator.CreateEntity(10, (int)centerPosition.X, (int)centerPosition.Y));
					break;
			}
		}

		private int GetDropQuantity() {
			return RandomManager.GetRandom().Next(0, maxDropQuantity + 1);
		}

		private bool Roll() {
			int roll = RandomManager.GetRandom().Next(0, 101);

			return roll <= dropChance;
		}

		public bool IsClosed {
			get { return isClosed; }
		}
	}
}
