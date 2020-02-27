using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace WizardPlatformer {
	public class Tile {
		public enum CollisionType {
			PASSABLE,
			IMPASSABLE,
			PLATFORM
		}

		public enum PassType {
			REGULAR,
			HOSTILE
		}

		private Rectangle heatBox;
		private Vector2 spriteOffset;
		public Vector2 tilePosition;

		private Texture2D sprite;
		private Vector2 spritePosition;
		private Rectangle spriteOnMap;
		private int tileSideSize;
		private int scaleFactor;

		private CollisionType collision;
		private PassType pass;

		protected bool drawDebugInfo;

		public Tile(Texture2D texture, Point spritePos, CollisionType collision, PassType pass, int heatBoxWidth, int heatBoxHeigth, int heatBoxPosX, int heatBoxPosY, int posX, int posY) {
			this.scaleFactor = (int)Display.DrawScale.X;
			this.tileSideSize = Display.CalcTileSideSize;

			this.heatBox = new Rectangle(posX, posY, heatBoxWidth * scaleFactor, heatBoxHeigth * scaleFactor);
			this.tilePosition = new Vector2(posX, posY);

			this.sprite = texture;
			this.spritePosition = new Vector2(posX - heatBoxPosX, posY - heatBoxPosY);
			this.spriteOffset = new Vector2(heatBoxPosX, heatBoxPosY);
			this.spriteOnMap = new Rectangle(spritePos.X * tileSideSize, spritePos.Y * tileSideSize, tileSideSize, tileSideSize);

			this.collision = collision;
			this.pass = pass;
			
			this.drawDebugInfo = false;
		}

		public virtual void Update(GameTime gameTime) {
		}

		public virtual void Draw(SpriteBatch spriteBatch, GameTime gameTime, float opacity = 1.0f) {
			spriteBatch.Draw(
				sprite,
				spritePosition,
				spriteOnMap,
				Color.White * opacity,
				0.0f,
				Vector2.Zero,
				Display.DrawScale,
				SpriteEffects.None,
				0.5f);

			DrawDebugInfo(spriteBatch, gameTime);
		}

		private void DrawDebugInfo(SpriteBatch spriteBatch, GameTime gameTime) {
			if (drawDebugInfo) {
				spriteBatch.Draw(
				sprite,
				tilePosition,
				new Rectangle(11 * tileSideSize, 19 * tileSideSize, heatBox.Width / scaleFactor, heatBox.Height / scaleFactor),
				Color.White,
				0.0f,
				Vector2.Zero,
				Display.DrawScale,
				SpriteEffects.None,
				0.5f
				);
			}
		}

		public Vector2 TilePosition {
			get { return tilePosition; }
			set {
				tilePosition = value;
				heatBox.X = (int)tilePosition.X;
				heatBox.Y = (int)tilePosition.Y;
				spritePosition = tilePosition - spriteOffset;
			}
		}

		public CollisionType Collision {
			get { return collision; }
		}

		public PassType Pass {
			get { return pass; }
		}

		public Rectangle HeatBox {
			get { return heatBox; }
		}
		
		public bool DebugInfo {
			get { return drawDebugInfo; }
			set { drawDebugInfo = value; }
		}
	}
}
