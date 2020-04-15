using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace WizardPlatformer.Logic.UI {
	public class UIButton {
		private Texture2D sprite;
		private Vector2 cursorPosition;
		private Color color;

		private int tileSideSize;

		public UIButton(Color color) {
			this.tileSideSize = Display.TileSideSize;
			this.color = color;

		}

		public void LoadContent(ContentManager contentManager) {
			sprite = contentManager.Load<Texture2D>("gui/cursor");

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
