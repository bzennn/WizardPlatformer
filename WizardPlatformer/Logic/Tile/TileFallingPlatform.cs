using Microsoft.Xna.Framework;
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
		Rectangle intersectionHeatbox;

		public TileFallingPlatform(Texture2D texture, Point spritePos, CollisionType collision, PassType pass, int heatBoxWidth, int heatBoxHeigth, int heatBoxPosX, int heatBoxPosY, int posX, int posY, Level level) 
			: base(texture, spritePos, collision, pass, heatBoxWidth, heatBoxHeigth, heatBoxPosX, heatBoxPosY, posX, posY) {

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

			this.intersectionHeatbox = new Rectangle(this.HeatBox.X, this.HeatBox.Y - 1, this.HeatBox.Width, this.HeatBox.Height + 1);
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

			/*if (isVisible || visibleTimer != 0) {
				base.Draw(spriteBatch, gameTime, opacity);
			} else {
				
			}*/
		}

		private void UpdateEntityOnPLatform() {
			foreach (Entity entity in level.EntitiesList) {
				if (entity is EntityLiving) {
					/*if (entity.HeatBox.Bottom == this.HeatBox.Top && 
						entity.Position.X >= this.HeatBox.Left &&
						entity.Position.X <= this.HeatBox.Right) {
						isEntityOnPlatform = true;
					}*/
					
					if (entity.HeatBox.Intersects(intersectionHeatbox) &&
						entity.HeatBox.Bottom <= this.HeatBox.Top) {
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
			Tile tileL = this.level.GetTile(this.HeatBox.Left - 1, this.HeatBox.Top);
			Tile tileR = this.level.GetTile(this.HeatBox.Right + 1, this.HeatBox.Top);

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
