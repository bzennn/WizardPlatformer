using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WizardPlatformer.Logic.Level;

namespace WizardPlatformer {
	public class TileCheckpoint : Tile {
		private Level level;
		private int tileSideSize;

		private Point fireFrame;
		private Point fireSpriteSize;

		private bool isActivating;
		private bool isActivated;

		public TileCheckpoint(Texture2D texture, Point spritePos, CollisionType collision, PassType pass, int heatBoxWidth, int heatBoxHeigth, int heatBoxPosX, int heatBoxPosY, int posX, int posY, Level level)
			: base(texture, spritePos, collision, pass, heatBoxWidth, heatBoxHeigth, heatBoxPosX, heatBoxPosY, posX, posY) {
			this.tileSideSize = Display.CalcTileSideSize;

			this.level = level;
			this.isAnimated = true;
			this.fireSpriteSize = new Point(tileSideSize, tileSideSize * 2);
			this.fireFrame = new Point(0, 0);
			this.spriteSize = new Point(5, 2);

			this.isActivating = false;
			this.isActivated = false;
		}

		public override void Update(GameTime gameTime) {
			base.Update(gameTime);

			UpdateActivation();
			UpdateActivatedChackpoint();
		}

		public override void Draw(SpriteBatch spriteBatch, GameTime gameTime, float opacity = 1.0f) {
			base.Draw(spriteBatch, gameTime, opacity);

			if (isActivating || isActivated) {
				Vector2 spritePosition = new Vector2(TilePosition.X, TilePosition.Y - HeatBox.Height * 2);
				Rectangle frameRectangle = new Rectangle(0 + fireFrame.X * tileSideSize, 96 + fireFrame.Y * tileSideSize * 2, tileSideSize, tileSideSize * 2);

				spriteBatch.Draw(
				sprite,
				spritePosition,
				frameRectangle,
				Color.White * opacity,
				0.0f,
				Vector2.Zero,
				Display.DrawScale,
				SpriteEffects.None,
				0.5f);
			}
		}

		public void Activate() {
			isActivating = true;
		}

		private void UpdateActivation() {
			if (isActivating) {
				if (Animator.Animate(0, 0, 5, false, this.frameTimeCounter, ref this.fireFrame)) {
					isActivating = false;
					isActivated = true;
					CreateSaveQuery();
				}
			}
		}

		private void CreateSaveQuery() {
			level.HasGameSaveQuery = true;
		}

		private void UpdateActivatedChackpoint() {
			if (isActivated) {
				Animator.Animate(1, 0, 3, true, this.frameTimeCounter, ref this.fireFrame);
			}
		}

		public bool IsActivated {
			get { return isActivated; }
			set { isActivated = value; }
		}
	}
}
