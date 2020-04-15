using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using WizardPlatformer.Logic.Level;
using System;
using WizardPlatformer.Logic.Save;

namespace WizardPlatformer {
	public class EntityPlayer : EntityLiving {
		private static int playerID;
		
		private int maxHealth;

		private int mana;
		private int maxMana;
		private int manaRegenCounter;
		private int manaRegenSpeed;

		private int stamina;
		private int maxStamina;
		private int staminaRegenCounter;
		private int staminaRegenSpeed;

		private int coins;

		private int rangeAttackCost;
		private int meleeAttackCost;

		private int coolDownMax;
		private int coolDown;

		private int invulnerabilityTimer;
		private int maxInvulnerabilityTime;

		private int exitDeep;

		private bool isAnimationOn;
		private bool isControlOn;

		public EntityPlayer(int health, int maxHealth, int mana, int maxMana, int stamina, int maxStamina, int damage, float velocity, int coins, bool emulatePhysics, int heatBoxWidth, int heatBoxHeight, int heatBoxSpritePosX, int heatBoxSpritePosY, int posX, int posY, int roomSizeId, Level level)
			: base(health, damage, velocity, emulatePhysics, heatBoxWidth, heatBoxHeight, heatBoxSpritePosX, heatBoxSpritePosY, posX, posY, roomSizeId, level) {

			playerID = this.id;

			this.maxHealth = maxHealth;

			this.mana = mana;
			this.maxMana = maxMana;
			this.manaRegenCounter = 0;
			this.manaRegenSpeed = 100;

			this.stamina = stamina;
			this.maxStamina = maxStamina;
			this.staminaRegenCounter = 0;
			this.staminaRegenSpeed = 10;

			this.coins = coins;

			this.rangeAttackCost = 30;
			this.meleeAttackCost = 25;

			this.coolDownMax = 100;
			this.coolDown = 0;

			this.maxInvulnerabilityTime = 500;
			this.invulnerabilityTimer = 0;

			this.exitDeep = 0;

			this.isAnimationOn = true;
			this.isControlOn = true;

			this.drawDebugInfo = false;
		}

		public override void LoadContent(ContentManager contentManager) {
			base.LoadContent(contentManager);

			this.sprite = contentManager.Load<Texture2D>("entity/wizard_sprite");
			this.spriteSize = new Point(6, 6);
		}

		public override void Update(GameTime gameTime) {
			base.Update(gameTime);

			UpdateCoolDown(gameTime);
			UpdateInput(gameTime);
			
			if (isAnimationOn) {
				UpdateAnimation();
			}

			if (this.IsAlive) {
				RegenMana(gameTime);
				RegenStamina(gameTime);
				UpdateInvulnerabilityTimer(gameTime);
			}
		}

		protected override void DrawDebugInfo(SpriteBatch spriteBatch, GameTime gameTime) {
			base.DrawDebugInfo(spriteBatch, gameTime);

			if (drawDebugInfo) {
				spriteBatch.DrawString(debugFont, "Coins = " + coins +
					"\nPosition = " + Position + 
					"\nExit deep = " + exitDeep, heatBox.Location.ToVector2() + new Vector2(60, 0), Color.AntiqueWhite);

				float angle = Geometry.GetAngleBetweenVectors(this.EntityPosition, InputManager.GetInstance().GetMousePosition());
				float angleCos = (float)Math.Cos(angle);
				float angleSin = (float)Math.Sin(angle);

				spriteBatch.DrawString(debugFont, "Angle:\n angle = " + angle +
					"\nCos = " + angleCos +
					"\nSin = " + angleSin +
					"\nCollide = " + isCollides, Display.GetZeroScreenPositionOnLevel() + new Vector2(10, 750), Color.AntiqueWhite);
			}
		}

		private void UpdateInput(GameTime gameTime) {
			if (InputManager.GetInstance().IsKeyPressed(Keys.Back)) {
				ScreenManager.GetInstance().ChangeScreen(new ScreenMainMenu(), true);
			}

			if (InputManager.GetInstance().IsKeyDown(Keys.A) && this.IsAlive && isControlOn) {
				this.AccelerateLeft(-maxAcceleration, false);
			} else {
				this.AccelerateLeft(-maxAcceleration, true);
			}

			if (InputManager.GetInstance().IsKeyDown(Keys.D) && this.IsAlive && isControlOn) {
				this.AccelerateRight(maxAcceleration, false);
			} else {
				this.AccelerateRight(maxAcceleration, true);
			}

			if (InputManager.GetInstance().IsKeyDown(Keys.Space) && this.IsAlive && isControlOn) {
				if ((isOnGround && InputManager.GetInstance().IsKeyPressed(Keys.Space)) || isJumping || !isGravityOn) {
					this.AccelerateJump(-8.5f, 32, false);
				}
			} else {
				this.AccelerateJump(-8.5f, 32, true);
			}

			if (InputManager.GetInstance().IsKeyPressed(Keys.S) && this.IsAlive && isControlOn) {
				this.FallThrough(false);
			} else {
				this.FallThrough(true);
			}
	
			if (InputManager.GetInstance().IsMouseLeftButtonPressed() && this.IsAlive && isControlOn && coolDown == 0) {
				if (mana - rangeAttackCost >= 0) {
					this.level.SpawnEntity(this.level.EntityCreator.CreateEntity(2, (int)Position.X, (int)Position.Y, this.EntityID));
					ConsumeMana(rangeAttackCost);
					coolDown = coolDownMax;
				}
			}

			if (InputManager.GetInstance().IsMouseRightButtonPressed() && this.IsAlive && isControlOn && coolDown == 0) {
				if (stamina - meleeAttackCost >= 0) {
					this.level.SpawnEntity(this.level.EntityCreator.CreateEntity(3, (int)Position.X, (int)Position.Y, this.EntityID));
					ConsumeStamina(meleeAttackCost);
					coolDown = coolDownMax;
				}
			}

			// Debug
			/*if (InputManager.GetInstance().IsKeyPressed(Keys.OemTilde)) {
				//Vector2 mousePos = InputManager.GetInstance().GetMousePosition();
				//this.level.SpawnEntity(this.level.EntityCreator.CreateEntity("spider", (int)mousePos.X, (int)mousePos.Y));
				this.level.DespawnAllEntitiesExceptPlayer();
			}*/
		}

		private void UpdateCoolDown(GameTime gameTime) {
			coolDown -= gameTime.ElapsedGameTime.Milliseconds;
			if (coolDown < 0) {
				coolDown = 0;
			}
		}

		protected override void HandleExtraTile(Tile tile) {
			base.HandleExtraTile(tile);

			if (tile is TileCollectable) {
				TileCollectable collectable = (TileCollectable)tile;
				Collect(collectable.CollectableForm);
				tile.Collapse();
			}

			if (tile is TileChest) {
				if (InputManager.GetInstance().IsKeyPressed(Keys.E)) {
					TileChest chest = (TileChest)tile;
					chest.Open();
				}
			}

			if (tile is TileCheckpoint) {
				TileCheckpoint checkpoint = (TileCheckpoint)tile;
				if (!checkpoint.IsActivated) {
					checkpoint.Activate();
				}
			}

			if (tile is TileCheckpointFire) {
				TileCheckpointFire checkpoint_fire = (TileCheckpointFire)tile;
				int x = checkpoint_fire.HeatBox.X;
				int y = checkpoint_fire.HeatBox.Bottom + 1;

				Tile tileCheckpoint = level.GetTile(x, y);
				if (tileCheckpoint is TileCheckpoint) {
					TileCheckpoint checkpoint = (TileCheckpoint) tileCheckpoint;
					if (!checkpoint.IsActivated) {
						checkpoint.Activate();
					}
				}
			}
		}

		protected override void HandleFunctionalTile(TileFunctional tile, GameTime gameTime) {
			base.HandleFunctionalTile(tile, gameTime);

			if (tile.Type == TileFunctional.FunctionType.TRIGGER) {
				level.HandleTrigger();
			}

			if (tile.Type == TileFunctional.FunctionType.ENTRANCE && exitDeep == 0) {
				if (isCollidesEdge) {
					this.level.HasLevelSwitchQuery = true;
					this.exitDeep++;
				}
			} else if (tile.Type == TileFunctional.FunctionType.EXIT) {
				if (isCollidesEdge) {
					this.level.HasLevelSwitchQuery = true;
					this.level.SwitchLevel = new int[] { tile.LevelId, tile.RoomId, -1, -1 };
					if (this.exitDeep > 0) {
						this.exitDeep--;
					}
				}
			}

			if (tile.Type == TileFunctional.FunctionType.LEVEL_COMPLETE) {
				CompleteLevel();
				this.level.HasLevelCompleteQuery = true;
				this.level.SwitchLevel = new int[] { tile.LevelId, tile.RoomId, -1, -1 };
			}
		}

		protected override bool HandleEntity(Entity entity) {
			if (entity is EntityCollectable) {
				EntityCollectable collectable = (EntityCollectable)entity;
				Collect(collectable.CollectableForm);
				entity.Collapse();
				return true;
			}

			if (entity is EntityEnemy) {
				this.ConsumeHealth(entity.Damage);
				return true;
			}

			return base.HandleEntity(entity);
		}

		private void Collect(TileCollectable.CollectableType collectableType) {
			switch(collectableType) {
				case TileCollectable.CollectableType.COIN:
					coins++;
					break;
				case TileCollectable.CollectableType.HEALTH_CRYSTAL:
					AddHealth(1);
					break;
				case TileCollectable.CollectableType.HEART:
					UpgradeHealth(1);
					break;
				case TileCollectable.CollectableType.MANA_CRYSTAL:
					AddMana(40);
					break;
				case TileCollectable.CollectableType.STAMINA_CRYSTAL:
					AddStamina(50);
					break;
				case TileCollectable.CollectableType.MANA_UPGRADE:
					UpgradeMana(50);
					break;
				case TileCollectable.CollectableType.STAMINA_UPGRADE:
					UpgradeStamina(10);
					break;
				case TileCollectable.CollectableType.DAMAGE_UPGRADE:
					UpgradeDamage(1);
					break;
			}	
		}

		private void UpdateAnimation() {
			if (this.health > 0) {
				if (isJumping) {
					Animator.Animate(2, 0, 3, false, frameTimeCounter, ref currentFrame);
				} else {
					if (this.currentVelocity.Y > 10e-4f) {
						currentFrame = new Point(3, 2);
					} else if (this.currentVelocity.X < 0 || this.currentVelocity.X > 10e-4f) {
						Animator.Animate(1, 0, 6, true, frameTimeCounter, ref currentFrame);
					} else {
						Animator.Animate(0, 0, 2, true, frameTimeCounter, ref currentFrame);
					}
				}
			} else {
				Animator.Animate(5, 0, 4, false, frameTimeCounter, ref currentFrame);
			}
		}

		private void CompleteLevel() {
			isAnimationOn = false;
			isControlOn = false;
			Animator.Animate(4, 0, 4, true, this.frameTimeCounter, ref this.currentFrame);
		}

		public void AddMana(int mana) {
			this.mana += mana;
			if (this.mana > maxMana) {
				this.mana = maxMana;
			}
		}

		public void ConsumeMana(int mana) {
			this.mana -= mana;
			if (this.mana < 0) {
				this.mana = 0;
			}
		}

		public void UpgradeMana(int mana) {
			this.maxMana += mana;
		}

		public void AddStamina(int stamina) {
			this.stamina += stamina;
			if (this.stamina > maxStamina) {
				this.stamina = maxStamina;
			}
		}

		public void ConsumeStamina(int stamina) {
			this.stamina -= stamina;
			if (this.stamina < 0) {
				this.stamina = 0;
			}
		}

		public void UpgradeStamina(int stamina) {
			this.maxStamina += stamina;
		}

		public void AddHealth(int health) {
			this.health += health;
			if (this.health > maxHealth) {
				this.health = maxHealth;
			}
		}

		public void UpgradeHealth(int hearts) {
			this.maxHealth += 2 * hearts;
			this.health = this.maxHealth;
		}

		public void UpgradeDamage(int damage) {
			this.damage += damage;
		}

		public void RegenMana(GameTime gameTime) {
			manaRegenCounter += gameTime.ElapsedGameTime.Milliseconds;

			if (manaRegenCounter > manaRegenSpeed) {
				manaRegenCounter = 0;
				AddMana(1);
			}
		}

		public void RegenStamina(GameTime gameTime) {
			staminaRegenCounter += gameTime.ElapsedGameTime.Milliseconds;

			if (staminaRegenCounter > staminaRegenSpeed) {
				staminaRegenCounter = 0;
				AddStamina(1);
			}
		}

		public SnapshotPlayer GetSnapshot() {
			return new SnapshotPlayer(
				this.EntityPosition.X,
				this.EntityPosition.Y,
				this.health,
				this.maxHealth,
				this.damage,
				this.mana,
				this.maxMana,
				this.manaRegenSpeed,
				this.stamina,
				this.maxStamina,
				this.staminaRegenSpeed,
				this.coins,
				this.rangeAttackCost,
				this.meleeAttackCost,
				this.exitDeep); 
		}

		public void RestoreSnapshot(SnapshotPlayer snapshot, bool restorePosition = true) {
			if (restorePosition) {
				this.EntityPosition = new Vector2(snapshot.PlayerPositionX, snapshot.PlayerPositionY);
			}
			this.health = snapshot.Health;
			this.maxHealth = snapshot.MaxHealth;
			this.damage = snapshot.Damage;
			this.mana = snapshot.Mana;
			this.maxMana = snapshot.MaxMana;
			this.manaRegenSpeed = snapshot.ManaRegenSpeed;
			this.stamina = snapshot.Stamina;
			this.maxStamina = snapshot.MaxStamina;
			this.staminaRegenSpeed = snapshot.StaminaRegenSpeed;
			this.coins = snapshot.Coins;
			this.rangeAttackCost = snapshot.RangeAttackCost;
			this.meleeAttackCost = snapshot.MeleeAttackCost;
			this.exitDeep = snapshot.ExitDeep;
		}

		public void RestorePosition(int xPos, int yPos) {
			this.EntityPosition = new Vector2(xPos, yPos);
		}

		private void UpdateInvulnerabilityTimer(GameTime gameTime) {
			invulnerabilityTimer -= gameTime.ElapsedGameTime.Milliseconds;
			
			if (invulnerabilityTimer < 0) {
				invulnerabilityTimer = 0;
			}
		}

		public override void ConsumeHealth(int health) {
			if (invulnerabilityTimer == 0) {
				base.ConsumeHealth(health);
				invulnerabilityTimer = maxInvulnerabilityTime;
			}
		}

		public static int PlayerID {
			get { return playerID; }
		}

		public int ExitDeep {
			get { return ExitDeep; }
		}

		public int MaxHealth {
			get { return maxHealth; }
		}

		public int Mana {
			get { return mana; }
		}

		public int MaxMana {
			get { return maxMana; }
		}

		public int Stamina {
			get { return stamina; }
		}

		public int MaxStamina {
			get { return maxStamina; }
		}

		public int Coins {
			get { return coins; }
		}
	}
}
