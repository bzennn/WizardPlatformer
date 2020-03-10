using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using WizardPlatformer.Logic.Level;
using System;

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

			this.drawDebugInfo = false;
		}

		public override void LoadContent(ContentManager contentManager) {
			base.LoadContent(contentManager);

			this.sprite = contentManager.Load<Texture2D>("entity/wizard_sprite");
			this.spriteSize = new Point(6, 6);
		}

		public override void Update(GameTime gameTime) {
			base.Update(gameTime);

			UpdateInput(gameTime);

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

			if (this.IsAlive) {
				RegenMana(gameTime);
				RegenStamina(gameTime);
			}
		}

		protected override void DrawDebugInfo(SpriteBatch spriteBatch, GameTime gameTime) {
			base.DrawDebugInfo(spriteBatch, gameTime);

			if (drawDebugInfo) {
				spriteBatch.DrawString(debugFont, "Coins = " + coins +
					"\nPosition = " + Position, heatBox.Location.ToVector2() + new Vector2(60, 0), Color.AntiqueWhite);

				float angle = Geometry.GetAngleBetweenVectors(this.EntityPosition, InputManager.GetInstance().GetMousePosition());
				float angleCos = (float)Math.Cos(angle);
				float angleSin = (float)Math.Sin(angle);

				spriteBatch.DrawString(debugFont, "Angle:\n angle = " + angle +
					"\nCos = " + angleCos +
					"\nSin = " + angleSin, Display.GetZeroScreenPositionOnLevel() + new Vector2(10, 750), Color.AntiqueWhite);
			}
		}

		private void UpdateInput(GameTime gameTime) {
			if (InputManager.GetInstance().IsKeyPressed(Keys.Back)) {
				ScreenManager.GetInstance().ChangeScreen(new ScreenMainMenu(), true);
			}

			if (InputManager.GetInstance().IsKeyDown(Keys.A) && this.IsAlive) {
				this.AccelerateLeft(-maxAcceleration, false);
			} else {
				this.AccelerateLeft(-maxAcceleration, true);
			}

			if (InputManager.GetInstance().IsKeyDown(Keys.D) && this.IsAlive) {
				this.AccelerateRight(maxAcceleration, false);
			} else {
				this.AccelerateRight(maxAcceleration, true);
			}

			if (InputManager.GetInstance().IsKeyDown(Keys.Space) && this.IsAlive) {
				if ((isOnGround && InputManager.GetInstance().IsKeyPressed(Keys.Space)) || isJumping || !isGravityOn) {
					this.AccelerateJump(-8.5f, 32, false);
				}
			} else {
				this.AccelerateJump(-8.5f, 32, true);
			}

			if (InputManager.GetInstance().IsKeyPressed(Keys.S) && this.IsAlive) {
				this.FallThrough(false);
			} else {
				this.FallThrough(true);
			}

			if (InputManager.GetInstance().IsMouseLeftButtonPressed() && this.IsAlive) {
				if (mana - rangeAttackCost >= 0) {
					this.level.SpawnEntity(new EntityRangeAttack(3000, this.id, this.damage, 7.0f, true, 4, 4, 44, 40, (int)Position.X, (int)Position.Y, this.level.RoomSizeId, this.level, InputManager.GetInstance().GetMousePosition()));
					ConsumeMana(rangeAttackCost);
				}
			}

			if (InputManager.GetInstance().IsMouseRightButtonPressed() && this.IsAlive) {
				if (stamina - meleeAttackCost >= 0) {
					this.level.SpawnEntity(new EntityMeleeAttack(450, this.id, (int)(this.damage * 1.5f), 7.0f, true, 4, 4, 44, 40, (int)Position.X, (int)Position.Y, this.level.RoomSizeId, this.level, InputManager.GetInstance().GetMousePosition()));
					ConsumeStamina(meleeAttackCost);
				}
			}
		}

		protected override void HandleExtraTile(Tile tile) {
			base.HandleExtraTile(tile);

			if (tile is TileCollectable) {
				coins++;
				tile.Collapse();
			}
		}

		protected override void HandleFunctionalTile(TileFunctional tile) {
			base.HandleFunctionalTile(tile);

			if (tile.Type == TileFunctional.FunctionType.TRIGGER) {
				level.HandleTrigger();
			}
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

		public static int PlayerID {
			get { return playerID; }
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
