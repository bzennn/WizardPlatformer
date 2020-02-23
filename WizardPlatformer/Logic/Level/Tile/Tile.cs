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

		private Texture2D texture;
		private Rectangle spritePos;
		private int TileSideSize;
		private int ScaleFactor;

		private CollisionType collision;
		private PassType pass;

		private Rectangle heatBox;
		public Vector2 Position;

		protected bool drawDebugInfo;
		private Texture2D debugSprite;
		private SpriteFont debugFont;

		public Tile(Texture2D texture, Point spritePos, CollisionType collision, PassType pass, int heatBoxWidth, int heatBoxHeigth, int heatBoxPosX, int heatBoxPosY, int posX, int posY) {
			this.TileSideSize = ScreenResolution.CalcTileSideSize;
			this.ScaleFactor = (int)ScreenResolution.DrawScale.X;
			this.texture = texture;
			this.spritePos = new Rectangle(spritePos.X * TileSideSize, spritePos.Y * TileSideSize, TileSideSize, TileSideSize);
			this.collision = collision;
			this.pass = pass;
			this.heatBox = new Rectangle(posX + heatBoxPosX, posY + heatBoxPosY, heatBoxWidth * ScaleFactor, heatBoxHeigth * ScaleFactor);
			this.Position = new Vector2(posX, posY);
			
			this.drawDebugInfo = false;
		}

		public virtual void Update(GameTime gameTime) {
			if (drawDebugInfo == true) {
				drawDebugInfo = false;
			} 
		}

		public virtual void Draw(SpriteBatch spriteBatch, GameTime gameTime) {
			spriteBatch.Draw(
				texture,
				Position,
				spritePos,
				Color.White,
				0.0f,
				Vector2.Zero,
				ScreenResolution.DrawScale,
				SpriteEffects.None,
				0.5f);

			DrawDebugInfo(spriteBatch, gameTime);
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

		private void DrawDebugInfo(SpriteBatch spriteBatch, GameTime gameTime) {
			if (drawDebugInfo) {
				/*spriteBatch.Draw(
				texture,
				heatBox.Location.ToVector2(),
				new Rectangle(11 * TileSideSize, 19 * TileSideSize, heatBox.Width / ScaleFactor, heatBox.Height / ScaleFactor),
				Color.White,
				0.0f,
				Vector2.Zero,
				ScreenResolution.DrawScale,
				SpriteEffects.None,
				0.5f
				);*/

				spriteBatch.Draw(
				texture,
				Position,
				new Rectangle(10 * TileSideSize, 19 * TileSideSize, TileSideSize, TileSideSize),
				Color.White,
				0.0f,
				Vector2.Zero,
				ScreenResolution.DrawScale,
				SpriteEffects.None,
				0.5f
				);
			}
		}
	}
}
