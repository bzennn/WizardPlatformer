using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace WizardPlatformer.Logic.UI {
	public class UIButton {
		private Texture2D sprite;
		private Vector2 screenZero;
		private int xPosition;
		private int yPosition;
		private Vector2 buttonPosition;
		private Rectangle buttonHitBox;
		private int scaleFactor;

		private Rectangle buttonUnfocusedSpriteRectangle;
		private Rectangle buttonFocusedSpriteRectangle;
		private Rectangle buttonDisabledSpriteRectangle;

		private Vector2 mousePosition;
		private bool isButtonFocused;

		private string text;
		private SpriteFont font;

		public delegate void OnClick();
		public event OnClick onClick;

		private bool isEnabled;

		public UIButton(int xPosition, int yPosition, string text) {
			this.scaleFactor = (int)Display.DrawScale.X;
			this.xPosition = xPosition;
			this.yPosition = yPosition;
			this.buttonUnfocusedSpriteRectangle = new Rectangle(0, 0, 72, 12);
			this.buttonFocusedSpriteRectangle = new Rectangle(0, 12, 72, 12);
			this.buttonDisabledSpriteRectangle = new Rectangle(0, 24, 72, 12);
			this.buttonPosition = new Vector2(xPosition, yPosition);
			this.buttonHitBox = new Rectangle((int)buttonPosition.X, (int)buttonPosition.Y, 72 * scaleFactor, 12 * scaleFactor);

			this.isButtonFocused = false;
			this.text = text;
			this.isEnabled = true;
		}

		public void LoadContent(ContentManager contentManager) {
			sprite = contentManager.Load<Texture2D>("gui/ui_button");
			font = contentManager.Load<SpriteFont>("font/russo_one_20");
		}

		public void Update(GameTime gameTime) {
			mousePosition = InputManager.GetInstance().GetMousePosition();

			//UpdateButtonPosition();

			if (buttonHitBox.Contains(mousePosition.ToPoint()) && isEnabled) {
				if (InputManager.GetInstance().IsMouseLeftButtonReleased()) {
					onClick();
				}
				isButtonFocused = true;
			} else {
				isButtonFocused = false;
			}
		}

		public void Draw(SpriteBatch spriteBatch, GameTime gameTime) {
			Rectangle spriteRectangle = isButtonFocused ? buttonFocusedSpriteRectangle : buttonUnfocusedSpriteRectangle;

			spriteBatch.Draw(
				sprite,
				buttonPosition,
				spriteRectangle,
				Color.White,
				0.0f,
				Vector2.Zero,
				Display.DrawScale,
				SpriteEffects.None,
				0.5f
				);
			spriteBatch.DrawString(font, text, new Vector2(buttonPosition.X + 20, buttonPosition.Y + 10), Color.White);

			if (!isEnabled) {
				spriteBatch.Draw(
				sprite,
				buttonPosition,
				buttonDisabledSpriteRectangle,
				Color.White,
				0.0f,
				Vector2.Zero,
				Display.DrawScale,
				SpriteEffects.None,
				0.5f
				);
			}
		}

		public void UpdateButtonPosition() {
			screenZero = Display.GetZeroScreenPositionOnLevel();
			buttonPosition = new Vector2(screenZero.X + xPosition, screenZero.Y + yPosition);
			//buttonPosition = Display.ScreenToLevelPosition(new Vector2(xPosition, yPosition));
			//
			buttonHitBox = new Rectangle((int)buttonPosition.X, (int)buttonPosition.Y, 72 * scaleFactor, 12 * scaleFactor);
		}

		public string Text {
			get { return text; }
			set { text = value; }
		}

		public bool IsEnabled {
			get { return isEnabled; }
			set { isEnabled = value; }
		}
	}
}
