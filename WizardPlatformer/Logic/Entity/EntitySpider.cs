using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using WizardPlatformer.Logic.Level;

namespace WizardPlatformer {
	public class EntitySpider : EntityEnemy {
		private bool moveDirection;
		private int moveTimer;
		private int maxMoveTime;
		private int abyssCheckTimer;
		private bool moveTaskStarted;

		private int idleTimer;
		private int maxIdleTime;
		private bool idleTaskStarted;

		public EntitySpider(int health, int damage, float velocity, bool emulatePhysics, int hitBoxWidth, int hitBoxHeight, int hitBoxSpritePosX, int hitBoxSpritePosY, int posX, int posY, int roomSizeId, Level level) 
			: base(health, damage, velocity, emulatePhysics, hitBoxWidth, hitBoxHeight, hitBoxSpritePosX, hitBoxSpritePosY, posX, posY, roomSizeId, level) {

			this.moveDirection = false;
			this.moveTimer = 0;
			this.maxMoveTime = 10000;
			this.moveTaskStarted = false;
			this.abyssCheckTimer = 0;

			this.idleTimer = 0;
			this.maxIdleTime = 5000;
			this.idleTaskStarted = false;
		}

		public override void LoadContent(ContentManager contentManager) {
			base.LoadContent(contentManager);

			this.sprite = contentManager.Load<Texture2D>("entity/spider_sprite");
			this.spriteSize = new Point(3, 2);

			this.AddAITask(AITaskMove);
			this.AddAITask(AITaskIdle);
		}

		public override void Update(GameTime gameTime) {
			base.Update(gameTime);

			UpdateMoveTimers(gameTime);

			if (this.currentVelocity.X < 0 || this.currentVelocity.X > 10e-4f) {
				Animator.Animate(1, 0, 3, true, frameTimeCounter, ref currentFrame);
			} else {
				Animator.Animate(0, 0, 2, true, frameTimeCounter, ref currentFrame);
			}
		}

		public override void Collapse() {
			SpawnDrop();
			base.Collapse();
		}

		private void SpawnDrop() {
			int coinsCount = RandomManager.GetRandom().Next(10);
			//int healthCount = RandomManager.GetRandom().Next(10) > 4 ? 1 : 0;

			for (int i = 0; i < coinsCount; i++) {
				this.level.SpawnEntity(this.level.EntityCreator.CreateEntity(4, (int)this.EntityPosition.X, (int)this.EntityPosition.Y));
			}

			/*for (int i = 0; i < healthCount; i++) {
				this.level.SpawnEntity(this.level.EntityCreator.CreateEntity(7, (int)this.EntityPosition.X, (int)this.EntityPosition.Y));
			}*/
		}

		private bool AITaskMove() {
			if (!moveTaskStarted) {
				moveDirection = RandomManager.GetRandom().Next(1) == 0 ? true : false;
				moveTimer = RandomManager.GetRandom().Next(2000, maxMoveTime);
				moveTaskStarted = true;
			} else {
				if (moveTimer != 0) {
					if (isCollides && !isOnMovingPlatform) {
						moveDirection = !moveDirection;
					} else if (abyssCheckTimer == 0) {
						if (IsAbyssNext()) {
							moveDirection = !moveDirection;
						}
					}

					if (moveDirection) {
						this.AccelerateLeft(-maxAcceleration, false);
						this.AccelerateRight(maxAcceleration, true);
					} else {
						this.AccelerateRight(maxAcceleration, false);
						this.AccelerateLeft(-maxAcceleration, true);
					}
				} else {
					this.AccelerateRight(maxAcceleration, true);
					this.AccelerateLeft(-maxAcceleration, true);
					moveTaskStarted = false;
					return true;
				}
			}

			return false;
		}

		private bool AITaskIdle() {
			if (!idleTaskStarted) {
				idleTimer = RandomManager.GetRandom().Next(1000, maxIdleTime);
				idleTaskStarted = true;
			} else {
				if (idleTimer == 0) {
					idleTaskStarted = false;
					return true;
				}
			}

			return false;
		}

		private bool IsAbyssNext() {
			Tile tileLeft = this.level.GetTile(this.HitBox.Left - 10, this.HitBox.Bottom + 1);
			Tile tileRight = this.level.GetTile(this.HitBox.Right + 10, this.HitBox.Bottom + 1);
			
			if (tileLeft == null || tileLeft.Collision == Tile.CollisionType.PASSABLE) {
				abyssCheckTimer = 150;
				return true;
			}

			if (tileRight == null || tileRight.Collision == Tile.CollisionType.PASSABLE) {
				abyssCheckTimer = 150;
				return true;
			}

			return false;
		}

		private void UpdateMoveTimers(GameTime gameTime) {
			moveTimer -= gameTime.ElapsedGameTime.Milliseconds;
			abyssCheckTimer -= gameTime.ElapsedGameTime.Milliseconds;
			idleTimer -= gameTime.ElapsedGameTime.Milliseconds;

			if (moveTimer < 0) {
				moveTimer = 0;
			}

			if (abyssCheckTimer < 0) {
				abyssCheckTimer = 0;
			}

			if (idleTimer < 0) {
				idleTimer = 0;
			}
		}
	}
}
