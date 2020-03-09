using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using System;
using WizardPlatformer.Logic.Level;

namespace WizardPlatformer {
	public abstract class Entity {
		private static int ID = 0;

		#region Fields

		protected int id;

		protected int health;
		protected int damage;
		private bool isAlive;
		protected bool isCollapsing;

		protected Level level;
		protected Tile[] surroundingTiles;

		protected Rectangle heatBox;
		protected Vector2 entityPosition;
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
		protected bool isFallingThrough;
		protected bool isGravityOn;
		protected bool isOnMovingPlatform;
		protected bool isCollides;

		protected Texture2D sprite;
		protected Point spriteSize;
		protected SpriteEffects spriteFlip;
		protected Vector2 spritePosition;
		protected Vector2 spriteOffset;
		protected bool isRotatable;
		protected float spriteRotation;
		protected Vector2 spriteRotationOrigin;
		protected bool isSpriteFlipping;
		protected bool isSpriteFlipped;
		protected bool hasAcceleration;
		private int scaleFactor;

		protected Point currentFrame;
		protected Point frameSize;
		protected int frameTimeCounter;
		protected int frameUpdateMillis;

		protected Tile lastCollide;
		private Tile movingPlatform;

		protected bool drawDebugInfo;
		protected Texture2D debugSprite;
		protected SpriteFont debugFont;

		#endregion

		public Entity(int health, int damage, float velocity, bool emulatePhysics, int heatBoxWidth, int heatBoxHeight, int heatBoxSpritePosX, int heatBoxSpritePosY, int posX, int posY, int roomSizeId, Level level) {
			this.scaleFactor = (int)Display.DrawScale.X; // Only on top of the constructor!!

			this.id = ID++;

			this.health = health;
			this.damage = damage;
			this.isAlive = true;
			this.isCollapsing = false;

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
			this.isFallingThrough = false;
			this.isGravityOn = true;
			this.isOnMovingPlatform = false;
			this.isCollides = false;
			this.isSpriteFlipping = true;
			this.isSpriteFlipped = false;
			this.hasAcceleration = true;

			this.spriteFlip = SpriteEffects.None;
			this.spritePosition = new Vector2(posX - heatBoxSpritePosX, posY - heatBoxSpritePosY);
			this.spriteOffset = new Vector2(heatBoxSpritePosX, heatBoxSpritePosY);
			this.isRotatable = false;
			this.spriteRotation = 0.0f;
			this.spriteRotationOrigin = Vector2.Zero;

			this.currentFrame = new Point(0, 0);
			this.frameSize = new Point(Display.CalcTileSideSize * 2, Display.CalcTileSideSize * 2);
			this.frameTimeCounter = 0;
			this.frameUpdateMillis = 150;

			this.lastCollide = null;
			this.movingPlatform = null;

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

			if (isCollapsing) {
				Collapse();
			}

			HandleEntities();
			HandleExtraTiles();
			HandleFuctionalTiles();

			UpdateSpriteFlipped();
			UpdateSpriteFlip();
		}

		public void Draw(SpriteBatch spriteBatch, GameTime gameTime) {
			DrawDebugInfo(spriteBatch, gameTime);
			spriteBatch.Draw(
				sprite,
				spritePosition,
				new Rectangle(currentFrame.X * frameSize.X, currentFrame.Y * frameSize.Y, frameSize.X, frameSize.Y),
				Color.White,
				spriteRotation,
				spriteRotationOrigin,
				Display.DrawScale,
				(isSpriteFlipping) ? spriteFlip: SpriteEffects.None,
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
				Display.DrawScale,
				SpriteEffects.None,
				0.5f
				);

				spriteBatch.DrawString(debugFont, "Entity (ID = " + id + "):\nX= " + heatBox.Left + " " + heatBox.Right +
					"\nHP = " + health +
					"\nY = " + heatBox.Top + " " + heatBox.Bottom + 
					"\nVel = " + currentVelocity +
					"\nAcc = " + currentAcceleration +
					"\nIsFallingThrough = " + isFallingThrough, entityPosition - new Vector2(0, 140), Color.AntiqueWhite);
			}
		}

		private void UpdateSpriteFlip() {
			if (isSpriteFlipped) {
				spriteFlip = SpriteEffects.FlipHorizontally;
				return;
			}

			if (!isSpriteFlipped) {
				spriteFlip = SpriteEffects.None;
				return;
			}
		}

		private void UpdateSpriteFlipped() {
			if (currentVelocity.X < 0) {
				isSpriteFlipped = true;
				return;
			}

			if (currentVelocity.X > 10e-4f) {
				isSpriteFlipped = false;
				return;
			}
		}
		protected void UpdateSpritePosition() {
			spritePosition = entityPosition - spriteOffset;
			if (isRotatable) {
				spritePosition += spriteRotationOrigin * scaleFactor;
			}
		}

		#region Properties

		public int EntityID {
			get { return id; }
		}

		public bool IsAlive {
			get { return isAlive; }
		}

		public int Health {
			get { return health; }
		}

		public Vector2 Position {
			get { return heatBox.Center.ToVector2(); }
		}

		public Vector2 Velocity {
			get { return currentVelocity; }
		}

		protected Vector2 EntityPosition {
			get { return entityPosition; }
			set {
				entityPosition = value;
				heatBox.X = (int)entityPosition.X;
				heatBox.Y = (int)entityPosition.Y;
				UpdateSpritePosition();
			}
		}

		#endregion

		#region Moving

		private void UpdatePhysics(GameTime gameTime) {
			Vector2 previousEntityPosition = EntityPosition;

			if (isGravityOn) {
				if (currentVelocity.Y > 10e-4f) {
					currentAcceleration.Y = 0;
					movingTime.Y = 0;

					currentVelocity.Y = 0;
				}
			}
			
			Tile bottomTileL = level.GetTile(heatBox.Left + heatBox.Width / 2 - 10, heatBox.Bottom);
			Tile bottomTileR = level.GetTile(heatBox.Left + heatBox.Width / 2 + 10, heatBox.Bottom);
			if ((bottomTileL == null || bottomTileL.Collision == Tile.CollisionType.PASSABLE) &&
				(bottomTileR == null || bottomTileR.Collision == Tile.CollisionType.PASSABLE) &&
				!(movingPlatform is TileMovingPlatform)) {
				isOnGround = false;
			}

			if (movingPlatform is TileMovingPlatform) {
				if (heatBox.Right < movingPlatform.HeatBox.Left ||
					heatBox.Left > movingPlatform.HeatBox.Right) {
					isOnGround = false;
				}else if (!isJumping && isOnGround) {
					EntityPosition += (movingPlatform as TileMovingPlatform).Velocity;
				}
			}

			if (hasAcceleration) {
				if (currentAcceleration.X > 10e-4f) {
					currentVelocity.X = Math.Min(currentAcceleration.X * movingTime.X, maxVelocity.X);
				} else if (currentAcceleration.X < 0) {
					currentVelocity.X = Math.Max(currentAcceleration.X * movingTime.X, -maxVelocity.X);
				}
			}
			

			if (!isJumping && hasAcceleration) {
				if (!isOnGround && isGravityOn) {
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
				if (currentAcceleration.X > 10e-4f ||
					Math.Abs(currentAcceleration.X) < 10e-4f) {

					movingTime.X = 0;
				}

				currentAcceleration.X = acceleration;
				movingTime.X++;

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
				if (currentAcceleration.X <= 0) {
					movingTime.X = 0;
				}

				currentAcceleration.X = acceleration;
				movingTime.X++;

				return;
			}
		}

		protected void AccelerateUp(float acceleration, bool clearAcceleration) {
			if (currentAcceleration.Y < 0 && clearAcceleration) {
				currentAcceleration.Y = 0;
				movingTime.Y = 0;
				currentVelocity.Y = 0;
				return;
			}

			if (acceleration < 0 && !clearAcceleration) {
				currentAcceleration.Y = acceleration;
				movingTime.Y++;

				return;
			}
		}

		protected void AccelerateDown(float acceleration, bool clearAcceleration) {
			if (currentAcceleration.Y > 10e-4f && clearAcceleration) {
				currentAcceleration.Y = 0;
				movingTime.Y = 0;
				currentVelocity.Y = 0;
				return;
			}

			if (acceleration > 10e-4f && !clearAcceleration) {
				currentAcceleration.Y = acceleration;
				movingTime.Y++;

				return;
			}
		}

		protected void AccelerateJump(float startVelocity, int maxJumpTime, bool clearAcceleration) {
			Tile topTileL = level.GetTile(heatBox.Left + heatBox.Width / 2 - 10, heatBox.Top - 1);
			Tile topTileR = level.GetTile(heatBox.Left + heatBox.Width / 2 + 10, heatBox.Top - 1);
			TileMovingPlatform movingPlatform = this.movingPlatform as TileMovingPlatform;

			bool isTopTileLImpassable = (topTileL != null) && (topTileL.Collision == Tile.CollisionType.IMPASSABLE);
			bool isTopTileRImpassable = (topTileR != null) && (topTileR.Collision == Tile.CollisionType.IMPASSABLE);

			if ((currentVelocity.Y < 0 && clearAcceleration) || movingTime.Y >= maxJumpTime || 
				(isTopTileLImpassable || isTopTileRImpassable) ||
				(movingPlatform != null && heatBox.Top <= movingPlatform.HeatBox.Bottom && heatBox.Top >= movingPlatform.HeatBox.Top)) {
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

		protected void FallThrough(bool destroy) {
			if (destroy) {
				isFallingThrough = false;
				return;
			}

			if (!destroy) {
				Tile bottomTileL = level.GetTile(heatBox.Left + heatBox.Width / 2 - 10, heatBox.Bottom + 1);
				Tile bottomTileR = level.GetTile(heatBox.Left + heatBox.Width / 2 + 10, heatBox.Bottom + 1);

				bool isBottomTileLImpassable = (bottomTileL != null) && (bottomTileL.Collision == Tile.CollisionType.IMPASSABLE);
				bool isBottomTileRImpassable = (bottomTileR != null) && (bottomTileR.Collision == Tile.CollisionType.IMPASSABLE);

				if (!isBottomTileLImpassable && !isBottomTileRImpassable && !(movingPlatform is TileMovingPlatform)) {
					isFallingThrough = true;
					isFalling = true;
					isOnGround = false;
				}
				return;
			}
		}

		#endregion

		#region Collision Resolve and Detection

		protected Tile[] GetSurrondingTiles(string layer = "base") {
			Tile[] surroundingTiles = new Tile[10];

			surroundingTiles[0] = level.GetTile(entityPosition.X, entityPosition.Y, layer);
			surroundingTiles[1] = level.GetTile(entityPosition.X, entityPosition.Y + heatBox.Height / 2, layer);
			surroundingTiles[2] = level.GetTile(entityPosition.X, entityPosition.Y + heatBox.Height, layer);
			surroundingTiles[3] = level.GetTile(entityPosition.X + heatBox.Width / 2, entityPosition.Y, layer);
			surroundingTiles[4] = level.GetTile(entityPosition.X + heatBox.Width / 2, entityPosition.Y + heatBox.Height / 2, layer);
			surroundingTiles[5] = level.GetTile(entityPosition.X + heatBox.Width / 2, entityPosition.Y + heatBox.Height, layer);
			surroundingTiles[7] = level.GetTile(entityPosition.X + heatBox.Width, entityPosition.Y, layer);
			surroundingTiles[8] = level.GetTile(entityPosition.X + heatBox.Width, entityPosition.Y + heatBox.Height / 2, layer);
			surroundingTiles[9] = level.GetTile(entityPosition.X + heatBox.Width, entityPosition.Y + heatBox.Height, layer);

			return surroundingTiles;
		}

		private Tile[] GetTileEntitySurrondingTiles() {
			Tile[] surroundingTiles = level.TileEntitiesList.ToArray();

			return surroundingTiles;
		}

		private void UpdateCollision() {
			Tile[] tiles = GetSurrondingTiles();
			Tile[] tileEntities = GetTileEntitySurrondingTiles();

			surroundingTiles = new Tile[tiles.Length + tileEntities.Length];
			tiles.CopyTo(surroundingTiles, 0);
			tileEntities.CopyTo(surroundingTiles, tiles.Length);

			isCollides = false;

			if (EntityPosition.X < 0) {
				EntityPosition = new Vector2(0, EntityPosition.Y);
				isCollides = true;
			}

			if (EntityPosition.X + heatBox.Width > level.RoomWidth * Display.TileSideSize) {
				EntityPosition = new Vector2(level.RoomWidth * Display.TileSideSize - heatBox.Width, EntityPosition.Y);
				isCollides = true;
			}

			if (EntityPosition.Y + heatBox.Height > level.RoomHeigth * Display.TileSideSize + heatBox.Height) {
				EntityPosition = new Vector2(EntityPosition.X, level.RoomHeigth * Display.TileSideSize + heatBox.Height);
				isCollides = true;
			}

			for (int i = 0; i < surroundingTiles.Length; i++) {
				if (surroundingTiles[i] != null && surroundingTiles[i].Collision != Tile.CollisionType.PASSABLE &&
					!(surroundingTiles[i].Collision == Tile.CollisionType.PLATFORM && isFallingThrough)) {
					Vector2 collisionDepth = Geometry.GetCollisionDepth(heatBox, surroundingTiles[i].HeatBox);

					if (collisionDepth != Vector2.Zero) {
						float collisionDepthX = Math.Abs(collisionDepth.X);
						float collisionDepthY = Math.Abs(collisionDepth.Y);

						lastCollide = surroundingTiles[i];
						isCollides = true;

						if (collisionDepthY < collisionDepthX || surroundingTiles[i].Collision == Tile.CollisionType.PLATFORM) {
							
							if (previousEntityBottom <= surroundingTiles[i].HeatBox.Top) {
								isOnGround = true;
							}

							if (surroundingTiles[i].Collision == Tile.CollisionType.IMPASSABLE || 
								(previousEntityBottom <= surroundingTiles[i].HeatBox.Top)) {

								EntityPosition = new Vector2(EntityPosition.X, EntityPosition.Y + collisionDepth.Y);

								movingPlatform = surroundingTiles[i];
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

		#region FunctionalTilesHandling

		private void HandleFuctionalTiles() {
			surroundingTiles = GetSurrondingTiles("functional");

			foreach (Tile tile in surroundingTiles) {
				if (tile is TileFunctional) {
					HandleFunctionalTile((TileFunctional)tile);
				}
			}
		}

		protected virtual void HandleFunctionalTile(TileFunctional tile) {
			if (tile.Type == TileFunctional.FunctionType.DEADLY) {
				Collapse();
			}
		}

		private void HandleExtraTiles() {
			surroundingTiles = GetSurrondingTiles();

			foreach (Tile tile in surroundingTiles) {
				if (tile != null && heatBox.Intersects(tile.HeatBox)) {
					HandleExtraTile(tile);
					break;
				}
			}
		}

		protected virtual void HandleExtraTile(Tile tile) {
			if (tile.Pass == Tile.PassType.HOSTILE) {
				Collapse();
			}
		}

		#endregion

		#region EntitiesHandling

		private void HandleEntities() {
			foreach (Entity entity in level.EntitiesList) {
				if (HandleEntity(entity)) {
					break;
				}
			}
		}

		protected virtual bool HandleEntity(Entity entity) {
			if (entity is EntityAttack && entity != this) {
				if (this is EntityLiving) {
					EntityAttack attack = (EntityAttack)entity;
					EntityLiving living = (EntityLiving)this;

					if (attack.SourceID != living.EntityID) {
						living.ConsumeHealth(attack.Damage);
						attack.Collapse();
						return true;
					}
				}
			}

			return false;
		}

		#endregion

		public virtual void Collapse() {
			isCollapsing = true;
			health = 0;
			isAlive = false;
		}
	}
}
