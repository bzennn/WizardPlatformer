﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WizardPlatformer.Logic.Level;

namespace WizardPlatformer {
	public class TileFallingPlatform : Tile {
		private Level level;

		private int visibleTimer;
		private int visibleMaxTime;

		private int collapsedTimer;
		private int collapsedMaxTime;

		private bool isEntityOnPlatform;
		private bool isVisible;
		private bool isFalling;

		private float fallingPlatOpacity;
		private Vector2 savedSpritePos;
		private int fallingTimer;
		private int fallingMaxTime;
		Rectangle intersectionHitBox;

		public TileFallingPlatform(Texture2D texture, Point spritePos, CollisionType collision, PassType pass, int hitBoxWidth, int hitBoxHeigth, int hitBoxPosX, int hitBoxPosY, int posX, int posY, Level level) 
			: base(texture, spritePos, collision, pass, hitBoxWidth, hitBoxHeigth, hitBoxPosX, hitBoxPosY, posX, posY) {

			this.level = level;

			this.visibleTimer = 0;
			this.visibleMaxTime = 1000;

			this.collapsedTimer = 0;
			this.collapsedMaxTime = 3000;

			this.isEntityOnPlatform = false;
			this.isVisible = true;

			this.fallingPlatOpacity = 1F;
			this.savedSpritePos = this.spritePosition;

			this.fallingTimer = 0;
			this.fallingMaxTime = 20;

			this.intersectionHitBox = new Rectangle(this.HitBox.X, this.HitBox.Y - 1, this.HitBox.Width, this.HitBox.Height + 1);
		}

		public override void Update(GameTime gameTime) {
			base.Update(gameTime);

			if (isVisible) {
				UpdateEntityOnPLatform();

				if (isEntityOnPlatform) {
					Break();
				}
			} else {
				if (visibleTimer != 0) {
					UpdateVisibleTimer(gameTime);
				} else if (collapsedTimer != 0) {
					this.Collision = CollisionType.PASSABLE;
					UpdateCollapsedTimer(gameTime);
				} else {
					this.Collision = CollisionType.PLATFORM;
					this.spritePosition = savedSpritePos;
					isVisible = true;
					isFalling = false;
				}
			}
		}

		public override void Draw(SpriteBatch spriteBatch, GameTime gameTime, float opacity = 1) {
			UpdateFallingDrawParameters(gameTime);
			base.Draw(spriteBatch, gameTime, fallingPlatOpacity);
		}

		private void UpdateEntityOnPLatform() {
			foreach (Entity entity in level.EntitiesList) {
				if (entity is EntityLiving) {			
					if (entity.HitBox.Intersects(intersectionHitBox) &&
						entity.HitBox.Bottom <= this.HitBox.Top) {
						isEntityOnPlatform = true;
					}
				}
			}
		}

		public void Break() {
			isFalling = true;
			isVisible = false;
			isEntityOnPlatform = false;
			visibleTimer = visibleMaxTime;
			collapsedTimer = collapsedMaxTime;

			BreakNeighbours();
		}

		private void BreakNeighbours() {
			Tile tileL = this.level.GetTile(this.HitBox.Left - 1, this.HitBox.Top);
			Tile tileR = this.level.GetTile(this.HitBox.Right + 1, this.HitBox.Top);

			if (tileL is TileFallingPlatform lP) {
				if (!lP.IsFalling) {
					lP.Break();
				}
			}

			if (tileR is TileFallingPlatform rP) {
				if (!rP.IsFalling) {
					rP.Break();
				}
			}
		}

		private void UpdateVisibleTimer(GameTime gameTime) {
			visibleTimer -= gameTime.ElapsedGameTime.Milliseconds;

			if (visibleTimer < 0) {
				visibleTimer = 0;
			}
		}

		private void UpdateCollapsedTimer(GameTime gameTime) {
			collapsedTimer -= gameTime.ElapsedGameTime.Milliseconds;

			if (collapsedTimer < 0) {
				collapsedTimer = 0;
			}
		}

		private void UpdateFallingDrawParameters(GameTime gameTime) {
			fallingTimer -= gameTime.ElapsedGameTime.Milliseconds;

			if (fallingTimer < 0) {
				fallingTimer = fallingMaxTime;

				if (isVisible || visibleTimer != 0) {
					if (Math.Abs(1 - fallingPlatOpacity) > 10e-4f) {
						fallingPlatOpacity += 0.02f;
					}
				} else {
					if (Math.Abs(fallingPlatOpacity) > 10e-4f) {
						fallingPlatOpacity -= 0.02f;
					}

					this.spritePosition = new Vector2(this.spritePosition.X, this.spritePosition.Y + 1);
				}
			}
		}

		public bool IsFalling {
			get { return isFalling; }
		}
	}
}
