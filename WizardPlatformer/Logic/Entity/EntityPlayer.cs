﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using WizardPlatformer.Logic.Level;

namespace WizardPlatformer {
	public class EntityPlayer : Entity {
		private int coins;

		public EntityPlayer(int health, int damage, float velocity, int coins, bool emulatePhysics, int heatBoxWidth, int heatBoxHeight, int heatBoxSpritePosX, int heatBoxSpritePosY, int posX, int posY, int roomSizeId, Level level)
			: base(health, damage, velocity, emulatePhysics, heatBoxWidth, heatBoxHeight, heatBoxSpritePosX, heatBoxSpritePosY, posX, posY, roomSizeId, level) {

			this.coins = coins;
			this.spriteSize = new Point(6, 6);
			this.drawDebugInfo = false;
		}

		public override void LoadContent(ContentManager contentManager) {
			base.LoadContent(contentManager);

			this.sprite = contentManager.Load<Texture2D>("entity/wizard_sprite");
		}

		public override void Update(GameTime gameTime) {
			base.Update(gameTime);

			UpdateInput(gameTime);
			UpdateExtraCollisions(gameTime);

			// Test Animate
			if (this.currentVelocity.Y < 0) {
				Animate(2, 4);
			} else if (this.currentVelocity.Y > 10e-4f) {
				currentFrame = new Point(3, 2);
			} else if (this.currentVelocity.X < 0 || this.currentVelocity.X > 10e-4f) {
				Animate(1, 6);
			} else {
				Animate(0, 2);
			}
		}

		protected override void DrawDebugInfo(SpriteBatch spriteBatch, GameTime gameTime) {
			base.DrawDebugInfo(spriteBatch, gameTime);

			if (drawDebugInfo) {
				spriteBatch.DrawString(debugFont, "Coins = " + coins +
					"\nPosition = " + Position, heatBox.Location.ToVector2() + new Vector2(60, 0), Color.AntiqueWhite);
			}
		}

		private void UpdateInput(GameTime gameTime) {
			if (InputManager.GetInstance().IsKeyPressed(Keys.Back)) {
				ScreenManager.GetInstance().ChangeScreen(new ScreenMainMenu(), true);
			}

			if (InputManager.GetInstance().IsKeyDown(Keys.A)) {
				this.AccelerateLeft(-maxAcceleration, false);
			} else {
				this.AccelerateLeft(-maxAcceleration, true);
			}

			if (InputManager.GetInstance().IsKeyDown(Keys.D)) {
				this.AccelerateRight(maxAcceleration, false);
			} else {
				this.AccelerateRight(maxAcceleration, true);
			}

			if (InputManager.GetInstance().IsKeyDown(Keys.Space)) {
				if ((isOnGround && InputManager.GetInstance().IsKeyPressed(Keys.Space)) || isJumping || !isGravityOn) {
					this.AccelerateJump(-8.5f, 32, false);
				}
			} else {
				this.AccelerateJump(-8.5f, 32, true);
			}

			if (InputManager.GetInstance().IsKeyPressed(Keys.S)) {
				this.FallThrough(false);
			} else {
				this.FallThrough(true);
			}
		}

		private void UpdateExtraCollisions(GameTime gameTime) {
			surroundingTiles = GetSurrondingTiles();

			for (int i = 0; i < 10; i++) {
				if (surroundingTiles[i] != null && heatBox.Intersects(surroundingTiles[i].HeatBox) && surroundingTiles[i] is TileCollectable) {
					coins++;
					this.level.DestroyTile(surroundingTiles[i]);
					break;
				}
			}
		}

		protected override void HandleFunctionalTile(TileFunctional tile) {
			base.HandleFunctionalTile(tile);

			if (tile.Type == TileFunctional.FunctionType.TRIGGER) {
				level.HandleTrigger();
			}
		}

		private void Animate(int row, int frameQuantity) {
			if (currentFrame.Y != row) {
				currentFrame.Y = row;
				frameTimeCounter = 0;
			}

			if (frameTimeCounter == 0) {
				currentFrame.X++;

				if (currentFrame.X > frameQuantity - 1) {
					currentFrame.X = 0;
				}
			}
		}

		public Vector2 Position {
			get { return this.heatBox.Center.ToVector2(); }
		}

		public Vector2 Velocity {
			get { return this.currentVelocity; }
		}
	}
}
