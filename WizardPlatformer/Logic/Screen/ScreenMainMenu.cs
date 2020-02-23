using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;

namespace WizardPlatformer {
	class ScreenMainMenu : Screen {

		public override void Initialize() {
			base.Initialize();
		}

		public override void LoadContent(ContentManager contentManager) {
			base.LoadContent(contentManager);

			font = screenContent.Load<SpriteFont>("font/russo_one_32");
		}

		public override void Update(GameTime gameTime) {
			base.Update(gameTime);

			if (InputManager.GetInstance().IsKeyPressed(Keys.Enter)) {
				ScreenManager.GetInstance().ChangeScreen(new ScreenGameplay(), true);
			}
		}

		public override void Draw(SpriteBatch spriteBatch, GameTime gameTime) {
			base.Draw(spriteBatch, gameTime);

			spriteBatch.DrawString(font, "MainMenu Screen", new Vector2(ScreenResolution.BaseResolution.X / 2, ScreenResolution.BaseResolution.Y / 2), Color.Black);
		}
	}
}
