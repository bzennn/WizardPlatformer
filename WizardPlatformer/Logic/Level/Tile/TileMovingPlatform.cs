using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WizardPlatformer.Logic.Level;

namespace WizardPlatformer {
	public class TileMovingPlatform : Tile {
		private Level level;

		private Vector2 velocity;
		private Vector2 previousPosition;
		private float velocityCoefficient;

		private TileMovingPlatformRail rail;

		public TileMovingPlatform(Texture2D texture, Point spritePos, CollisionType collision, PassType pass, float velocityCoefficient, int heatBoxWidth, int heatBoxHeigth, int heatBoxPosX, int heatBoxPosY, int posX, int posY)
			: base(texture, spritePos, collision, pass, heatBoxWidth, heatBoxHeigth, heatBoxPosX, heatBoxPosY, posX, posY) {

			this.level = null;
			this.velocity = Vector2.Zero;
			this.velocityCoefficient = velocityCoefficient;
			this.rail = null;
			this.drawDebugInfo = true;
			this.TilePosition += new Vector2(0, 8);
			this.previousPosition = this.TilePosition;
		}

		public override void Update(GameTime gameTime) {
			base.Update(gameTime);

			this.previousPosition = this.TilePosition;
			UpdateMoving();
		}

		public override void Draw(SpriteBatch spriteBatch, GameTime gameTime, float opacity = 1.0f) {
			base.Draw(spriteBatch, gameTime, opacity);
		}

		public Level Level {
			set { level = value; }
		}

		private void UpdateMoving() {
			UpdateRail();
			UpdateVelocity();
			UpdatePosition();
		}

		private void UpdateRail() {
			Vector2 centerPosition = this.HeatBox.Center.ToVector2();
			if (level != null) {
				Tile backTile = level.GetTile(centerPosition.X, centerPosition.Y, "base");
				if (backTile is TileMovingPlatformRail) {
					rail = (TileMovingPlatformRail)backTile;
				} else {
					rail = null;
				}
			}
		}

		private void UpdateVelocity() {
			if (rail != null) {
				if (velocity == Vector2.Zero) {
					velocity = rail.GetBaseDirectionVelocity(false) * velocityCoefficient;
				} else {
					velocity = rail.GetUpdateDirectionVelocity(velocity) * velocityCoefficient;
				}
			} else {
				velocity *= -1;
			}
		}

		private void UpdatePosition() {
			this.TilePosition += velocity;
		}
	}
}
