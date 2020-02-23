﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using System;

namespace WizardPlatformer {
	public abstract class Entity {
		
		#region Fields

		protected int health;
		protected int damage;

		protected Level level;
		protected Tile[] surroundingTiles;

		protected Rectangle heatBox;
		private Vector2 entityPosition;
		private float previousEntityBottom;

		protected Vector2 maxVelocity;
		protected float maxAcceleration;
		protected float gravityAcceleration;
		protected Vector2 currentVelocity;
		private Vector2 currentAcceleration;
		private Vector2 movingTime;
		private bool emulatePhysics;

		protected bool isJumping;
		protected bool isFalling;
		protected bool isOnGround;

		protected Texture2D sprite;
		protected Point spriteSize;
		protected SpriteEffects spriteFlip;
		private Vector2 spritePosition;
		private Vector2 spriteOffset;
		private int scaleFactor;

		protected Point currentFrame;
		protected Point frameSize;
		protected int frameTimeCounter;
		protected int frameUpdateMillis;

		protected bool drawDebugInfo;
		protected Texture2D debugSprite;
		protected SpriteFont debugFont;

		#endregion

		public Entity(int health, int damage, float velocity, bool emulatePhysics, int heatBoxWidth, int heatBoxHeight, int heatBoxSpritePosX, int heatBoxSpritePosY, int posX, int posY, int roomSizeId, Level level) {
			this.scaleFactor = (int)ScreenResolution.DrawScale.X; // Only on top of the constructor!!

			this.health = health;
			this.damage = damage;

			this.level = level;
			this.surroundingTiles = new Tile[10];

			this.heatBox = new Rectangle(posX, posY, heatBoxWidth * scaleFactor, heatBoxHeight * scaleFactor);
			this.entityPosition = new Vector2(posX, posY);
			this.previousEntityBottom = heatBox.Bottom;

			this.maxVelocity = new Vector2(velocity, 9.8f);
			this.maxAcceleration = 0.15f;
			this.gravityAcceleration = 0.2f;
			this.currentVelocity = Vector2.Zero;
			this.currentAcceleration = Vector2.Zero;
			this.movingTime = Vector2.Zero;
			this.emulatePhysics = emulatePhysics;

			this.isJumping = false;
			this.isOnGround = false;
			this.isFalling = true;

			this.spriteFlip = SpriteEffects.None;
			this.spritePosition = new Vector2(posX - heatBoxSpritePosX, posY - heatBoxSpritePosY);
			this.spriteOffset = new Vector2(heatBoxSpritePosX, heatBoxSpritePosY);

			this.currentFrame = new Point(0, 0);
			this.frameSize = new Point(ScreenResolution.CalcTileSideSize * 2, ScreenResolution.CalcTileSideSize * 2);
			this.frameTimeCounter = 0;
			this.frameUpdateMillis = 150;
			
			this.drawDebugInfo = false;
		}

		public virtual void LoadContent(ContentManager contentManager) {
			if (drawDebugInfo) {
				debugSprite = contentManager.Load<Texture2D>("entity/debug_sprite");
				debugFont = contentManager.Load<SpriteFont>("font/russo_one_12");
			}
		}

		public virtual void Update(GameTime gameTime) {
			frameTimeCounter += gameTime.ElapsedGameTime.Milliseconds;
			if (frameTimeCounter > frameUpdateMillis) {
				frameTimeCounter = 0;
			}

			if (emulatePhysics) {
				UpdatePhysics(gameTime);
			}
		}

		public void Draw(SpriteBatch spriteBatch, GameTime gameTime) {
			DrawDebugInfo(spriteBatch, gameTime);
			spriteBatch.Draw(
				sprite,
				spritePosition,
				new Rectangle(currentFrame.X * frameSize.X, currentFrame.Y * frameSize.Y, frameSize.X, frameSize.Y),
				Color.White,
				0.0f,
				Vector2.Zero,
				ScreenResolution.DrawScale,
				spriteFlip,
				0.5f);
		}

		protected virtual void DrawDebugInfo(SpriteBatch spriteBatch, GameTime gameTime) {
			if (drawDebugInfo) {
				spriteBatch.Draw(
				debugSprite,
				entityPosition,
				new Rectangle(0, 0, heatBox.Width / scaleFactor, heatBox.Height / scaleFactor),
				Color.White,
				0.0f,
				Vector2.Zero,
				ScreenResolution.DrawScale,
				SpriteEffects.None,
				0.5f
				);

				spriteBatch.DrawString(debugFont, "Entity:\nX= " + heatBox.Left + " " + heatBox.Right +
					"\nHP = " + health +
					"\nY = " + heatBox.Top + " " + heatBox.Bottom + "\nVel = " + currentVelocity +
					"\nAcc = " + currentAcceleration, entityPosition - new Vector2(0, 120), Color.AntiqueWhite);
			}
		}

		protected Vector2 EntityPosition {
			get { return entityPosition; }
			set {
				entityPosition = value;
				heatBox.X = (int)entityPosition.X;
				heatBox.Y = (int)entityPosition.Y;
				spritePosition = entityPosition - spriteOffset;
			}
		}

		#region Moving

		private void UpdatePhysics(GameTime gameTime) {
			Vector2 previousEntityPosition = EntityPosition;

			if (currentVelocity.Y > 10e-4f) {
				currentAcceleration.Y = 0;
				movingTime.Y = 0;

				currentVelocity.Y = 0;
			}

			Tile bottomTileL = level.GetTile(heatBox.Left + heatBox.Width / 2 - 10, heatBox.Bottom);
			Tile bottomTileR = level.GetTile(heatBox.Left + heatBox.Width / 2 + 10, heatBox.Bottom);
			if ((bottomTileL == null || bottomTileL.Collision == Tile.CollisionType.PASSABLE) &&
				(bottomTileR == null || bottomTileR.Collision == Tile.CollisionType.PASSABLE)) {
				isOnGround = false;
			}

			if (currentAcceleration.X > 10e-4f) {
				currentVelocity.X = Math.Min(currentAcceleration.X * movingTime.X, maxVelocity.X);
			} else if (currentAcceleration.X < 0) {
				currentVelocity.X = Math.Max(currentAcceleration.X * movingTime.X, -maxVelocity.X);
			}

			if (!isJumping) {
				if (!isOnGround) {
					currentAcceleration.Y = gravityAcceleration;
					movingTime.Y++;
				}

				if (currentAcceleration.Y < 0) {
					currentVelocity.Y = Math.Min(currentAcceleration.Y * movingTime.Y, -maxVelocity.Y);
				} else if (currentAcceleration.Y > 10e-4f) {
					currentVelocity.Y = Math.Max(currentAcceleration.Y * movingTime.Y, maxVelocity.Y);
				}
			}

			EntityPosition += currentVelocity;

			UpdateCollision();

			if (previousEntityPosition.X == EntityPosition.X) {
				currentVelocity.X = 0;
				currentAcceleration.X = 0;
			}

			if (previousEntityPosition.Y == EntityPosition.Y) {
				currentVelocity.Y = 0;
				currentAcceleration.Y = 0;
			}
		}

		protected void AccelerateLeft(float acceleration, bool crearAcceleration) {
			if (currentAcceleration.X < 0 && crearAcceleration) {
				currentAcceleration.X = 0;
				movingTime.X = 0;
				currentVelocity.X = 0;
				return;
			}

			if (acceleration < 0 && !crearAcceleration) {
				currentAcceleration.X = acceleration;
				movingTime.X++;

				if (spriteFlip != SpriteEffects.FlipHorizontally) {
					spriteFlip = SpriteEffects.FlipHorizontally;
					movingTime.X = 0;
				}
				return;
			}
		}

		protected void AccelerateRight(float acceleration, bool clearAcceleration) {
			if (currentAcceleration.X > 10e-4f && clearAcceleration) {
				currentAcceleration.X = 0;
				movingTime.X = 0;
				currentVelocity.X = 0;
				return;
			}

			if (acceleration > 10e-4f && !clearAcceleration) {
				currentAcceleration.X = acceleration;
				movingTime.X++;

				if (spriteFlip != SpriteEffects.None) {
					spriteFlip = SpriteEffects.None;
					movingTime.X = 0;
				}
				return;
			}
		}

		protected void AccelerateJump(float startVelocity, int maxJumpTime, bool clearAcceleration) {
			if ((currentVelocity.Y < 0 && clearAcceleration) || movingTime.Y >= maxJumpTime) {
				currentAcceleration.Y = 0;
				currentVelocity.Y = 0;

				isFalling = true;
				isOnGround = false;
				isJumping = false;

				movingTime.Y = 0;
				return;
			}

			if (startVelocity < 0 && !clearAcceleration) {
				currentVelocity.Y = startVelocity + gravityAcceleration * movingTime.Y;

				if (movingTime.Y >= maxJumpTime - 10) {
					currentVelocity.Y = currentVelocity.Y - currentVelocity.Y / 10;
				}

				isFalling = false;
				isOnGround = false;
				isJumping = true;

				movingTime.Y++;
				return;
			}
		}

		#endregion

		#region Collision Resolve and Detection

		protected Tile[] GetSurrondingTiles(Vector2 entityPosition) {
			Tile[] surroundingTiles = new Tile[10];

			surroundingTiles[0] = level.GetTile(entityPosition.X, entityPosition.Y);
			surroundingTiles[1] = level.GetTile(entityPosition.X, entityPosition.Y + heatBox.Height / 2);
			surroundingTiles[2] = level.GetTile(entityPosition.X, entityPosition.Y + heatBox.Height);
			surroundingTiles[3] = level.GetTile(entityPosition.X + heatBox.Width / 2, entityPosition.Y);
			surroundingTiles[4] = level.GetTile(entityPosition.X + heatBox.Width / 2, entityPosition.Y + heatBox.Height / 2);
			surroundingTiles[5] = level.GetTile(entityPosition.X + heatBox.Width / 2, entityPosition.Y + heatBox.Height);
			surroundingTiles[7] = level.GetTile(entityPosition.X + heatBox.Width, entityPosition.Y);
			surroundingTiles[8] = level.GetTile(entityPosition.X + heatBox.Width, entityPosition.Y + heatBox.Height / 2);
			surroundingTiles[9] = level.GetTile(entityPosition.X + heatBox.Width, entityPosition.Y + heatBox.Height);

			return surroundingTiles;
		}

		private void UpdateCollision() {
			surroundingTiles = GetSurrondingTiles(EntityPosition);

			for (int i = 0; i < 10; i++) {
				if (surroundingTiles[i] != null && surroundingTiles[i].Collision != Tile.CollisionType.PASSABLE) {
					Vector2 collisionDepth = Geometry.GetCollisionDepth(heatBox, surroundingTiles[i].HeatBox);

					if (collisionDepth != Vector2.Zero) {
						float collisionDepthX = Math.Abs(collisionDepth.X);
						float collisionDepthY = Math.Abs(collisionDepth.Y);

						if (collisionDepthY < collisionDepthX || surroundingTiles[i].Collision == Tile.CollisionType.PLATFORM) {
							
							if (previousEntityBottom <= surroundingTiles[i].HeatBox.Top) {
								isOnGround = true;
							}

							if (surroundingTiles[i].Collision == Tile.CollisionType.IMPASSABLE || isOnGround) {
								EntityPosition = new Vector2(EntityPosition.X, EntityPosition.Y + collisionDepth.Y);
							}
							
						} else if (surroundingTiles[i].Collision == Tile.CollisionType.IMPASSABLE) {
							EntityPosition = new Vector2(EntityPosition.X + collisionDepth.X, EntityPosition.Y);
						}
					}
				}
			}
			previousEntityBottom = heatBox.Bottom;
		}

		#endregion
	}
}