using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace WizardPlatformer {
	public class Cursor {
		Texture2D sprite;
		Vector2 cursorPosition;
		Color color;

		int tileSideSize;

		public Cursor(Color color) {
			this.tileSideSize = Display.TileSideSize;
			this.color = color;
		}

		public void LoadContent(ContentManager contentLoader) {
			sprite = contentLoader.Load<Texture2D>("gui/cursor");
		}

		public void Update(GameTime gameTime) {
			cursorPosition = InputManager.GetInstance().GetMousePosition();
		}

		public void Draw(SpriteBatch spriteBatch, GameTime gameTime) {
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
	}
}
