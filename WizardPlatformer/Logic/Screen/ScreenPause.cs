using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using WizardPlatformer.Logic.UI;

namespace WizardPlatformer {
	public class ScreenPause : Screen {
		private Vector2 screenCenter;
		private Screen previousScreen;
		private Texture2D background;

		private UIButton resumeButton;
		private UIButton backToMainMenuButton;

		public ScreenPause(Screen previousScreen) {
			this.screenCenter = Display.GetScreenCenter();
			this.previousScreen = previousScreen;
		}

		public override void Initialize() {
			base.Initialize();
		}

		public override void LoadContent(ContentManager contentManager) {
			this.screenContent = new ContentManager(contentManager.ServiceProvider, "Content");

			background = screenContent.Load<Texture2D>("gui/pause_background");
			font = screenContent.Load<SpriteFont>("font/russo_one_32");

			resumeButton = new UIButton((int)screenCenter.X + 10, (int)screenCenter.Y + 70, "Resume");
			resumeButton.LoadContent(screenContent);
			resumeButton.onClick += ResumeGame;

			backToMainMenuButton = new UIButton((int)screenCenter.X + 10, (int)screenCenter.Y + 125, "Back to menu");
			backToMainMenuButton.LoadContent(screenContent);
			backToMainMenuButton.onClick += BackToMainMenu;
		}

		public override void UnloadContent() {
			this.screenContent.Unload();
		}

		public override void Update(GameTime gameTime) {
			base.Update(gameTime);

			if (InputManager.GetInstance().IsKeyPressed(Keys.Escape)) {
				ResumeGame();
			}

			resumeButton.Update(gameTime);
			backToMainMenuButton.Update(gameTime);
		}

		public override void Draw(SpriteBatch spriteBatch, GameTime gameTime) {
			base.Draw(spriteBatch, gameTime);

			previousScreen.Draw(spriteBatch, gameTime);
			spriteBatch.Draw(
				background,
				Display.GetZeroScreenPositionOnLevel(),
				null,
				Color.White,
				0.0f,
				Vector2.Zero,
				Display.DrawScale,
				SpriteEffects.None,
				0.0f);
			spriteBatch.DrawString(font, "Pause", Display.GetScreenCenter(), Color.White);

			resumeButton.Draw(spriteBatch, gameTime);
			backToMainMenuButton.Draw(spriteBatch, gameTime);
		}

		public void ResumeGame() {
			ScreenManager.GetInstance().ReturnPreviousScreen();
		}

		public void BackToMainMenu() {
			ScreenManager.GetInstance().ChangeScreen(new ScreenMainMenu(), true);
		}
	}
}
