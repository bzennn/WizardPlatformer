using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using WizardPlatformer.Logic.Level;

namespace WizardPlatformer {
	public class EntityCollectable : Entity {
		private TileCollectable.CollectableType collectableForm;
		private int TTL;

		private int maxJumpTime;
		private int maxMovementDuration;
		private int startDurationJump;
		private int startDurationMove;
		private int startMoveDirection;

		public EntityCollectable(int TTL, TileCollectable.CollectableType collectableForm, float velocity, bool emulatePhysics, int heatBoxWidth, int heatBoxHeight, int heatBoxSpritePosX, int heatBoxSpritePosY, int posX, int posY, int roomSizeId, Level level) 
			: base(0, 0, velocity, emulatePhysics, heatBoxWidth, heatBoxHeight, heatBoxSpritePosX, heatBoxSpritePosY, posX, posY, roomSizeId, level) {

			this.TTL = TTL;
			this.collectableForm = collectableForm;

			this.maxJumpTime = 25;
			this.maxMovementDuration = 300;
			this.startDurationJump = RandomManager.GetRandom().Next(1, maxJumpTime);
			this.startDurationMove = RandomManager.GetRandom().Next(maxJumpTime, maxMovementDuration);
			this.startMoveDirection = RandomManager.GetRandom().Next(0, 2);
			if (this.startMoveDirection == 0) {
				this.startMoveDirection = -1;
			}

			this.maxAcceleration = 0.7f;
		}

		public override void LoadContent(ContentManager contentManager) {
			base.LoadContent(contentManager);

			switch(collectableForm) {
				case TileCollectable.CollectableType.COIN:
					this.sprite = contentManager.Load<Texture2D>("entity/coin_sprite");
					this.spriteSize = new Point(1, 1);
					break;
				case TileCollectable.CollectableType.HEALTH_CRYSTAL:
					this.sprite = contentManager.Load<Texture2D>("entity/health_sprite");
					this.spriteSize = new Point(1, 1);
					break;
				case TileCollectable.CollectableType.HEART:
					this.sprite = contentManager.Load<Texture2D>("entity/heart_sprite");
					this.spriteSize = new Point(1, 1);
					break;
				case TileCollectable.CollectableType.MANA_CRYSTAL:
					this.sprite = contentManager.Load<Texture2D>("entity/mana_sprite");
					this.spriteSize = new Point(1, 1);
					break;
				case TileCollectable.CollectableType.STAMINA_CRYSTAL:
					this.sprite = contentManager.Load<Texture2D>("entity/stamina_sprite");
					this.spriteSize = new Point(1, 1);
					break;
			}
		}

		public override void Update(GameTime gameTime) {
			base.Update(gameTime);

			UpdateMovement(gameTime);
			UpdateMovementCounters(gameTime);
		}

		private void UpdateMovement(GameTime gameTime) {
			if (startDurationMove != 0) {
				if (startMoveDirection == -1) {
					this.AccelerateLeft(-this.maxAcceleration, false);
				} else {
					this.AccelerateRight(this.maxAcceleration, false);
				}
			} else {
				this.AccelerateLeft(-this.maxAcceleration, true);
				this.AccelerateRight(this.maxAcceleration, true);
			}

			if (startDurationJump != 0 || isJumping || !isGravityOn) {
				this.AccelerateJump(-8.5f, maxJumpTime, false);
			} else {
				this.AccelerateJump(-8.5f, maxJumpTime, true
					);
			}
		}

		private void UpdateMovementCounters(GameTime gameTime) {
			startDurationJump -= gameTime.ElapsedGameTime.Milliseconds;
			startDurationMove -= gameTime.ElapsedGameTime.Milliseconds;

			if (startDurationJump < 0) {
				startDurationJump = 0;
			}

			if (startDurationMove < 0) {
				startDurationMove = 0;
			}
		}

		public TileCollectable.CollectableType CollectableForm {
			get { return collectableForm; }
		}
	}
}
