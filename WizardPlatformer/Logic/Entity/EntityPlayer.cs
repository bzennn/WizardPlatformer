﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using WizardPlatformer.Logic.Level;
using System;

namespace WizardPlatformer {
	public class EntityPlayer : EntityLiving {
		private static int playerID;
		private int coins;

		public EntityPlayer(int health, int damage, float velocity, int coins, bool emulatePhysics, int heatBoxWidth, int heatBoxHeight, int heatBoxSpritePosX, int heatBoxSpritePosY, int posX, int posY, int roomSizeId, Level level)
			: base(health, damage, velocity, emulatePhysics, heatBoxWidth, heatBoxHeight, heatBoxSpritePosX, heatBoxSpritePosY, posX, posY, roomSizeId, level) {

			playerID = this.id;

			this.coins = coins;
			this.drawDebugInfo = true;
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
					"\nSin = " + angleSin, Display.GetZeroScreenPositionOnLevel() + new Vector2(60, 0), Color.AntiqueWhite);
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
				this.level.SpawnEntity(new EntityRangeAttack(3000, this.id, this.damage, 7.0f, true, 4, 4, 44, 40, (int)Position.X, (int)Position.Y, this.level.RoomSizeId, this.level, InputManager.GetInstance().GetMousePosition()));
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

		public Vector2 Position {
			get { return this.heatBox.Center.ToVector2(); }
		}

		public Vector2 Velocity {
			get { return this.currentVelocity; }
		}

		public static int PlayerID {
			get { return playerID; }
		}
	}
}
