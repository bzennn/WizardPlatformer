using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace WizardPlatformer {
	public class Cursor {
		private Texture2D sprite;
		private Vector2 cursorPosition;
		private Color color;

		private int tileSideSize;

		private bool drawDebugInfo;
		private Texture2D debugSprite;
		private SpriteFont debugFont;

		public Cursor(Color color) {
			this.tileSideSize = Display.TileSideSize;
			this.color = color;

			this.drawDebugInfo = true;
		}

		public void LoadContent(ContentManager contentManager) {
			sprite = contentManager.Load<Texture2D>("gui/cursor");

			if (drawDebugInfo) {
				debugSprite = contentManager.Load<Texture2D>("entity/debug_sprite");
				debugFont = contentManager.Load<SpriteFont>("font/russo_one_12");
			}
		}

		public void Update(GameTime gameTime) {
			cursorPosition = InputManager.GetInstance().GetMousePosition();
		}

		public void Draw(SpriteBatch spriteBatch, GameTime gameTime) {
			if (drawDebugInfo) {
				DrawDebugInfo(spriteBatch, gameTime);
			}
			spriteBatch.Draw(
				sprite,
				cursorPosition,
				new Rectangle(0, 0, tileSideSize, tileSideSize),
				color,
				0.0f,
				Vector2.Zero,
				Display.DrawScale,
				SpriteEffects.None,
				0.5f
				);
		}

		protected virtual void DrawDebugInfo(SpriteBatch spriteBatch, GameTime gameTime) {
			if (drawDebugInfo) {
				spriteBatch.DrawString(debugFont,
					"Cursor:\nPos = " + cursorPosition, cursorPosition - new Vector2(0, 50), Color.AntiqueWhite);
			}
		}
	}
}
