using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using WizardPlatformer.Logic.UI;

namespace WizardPlatformer {
	public class ScreenGameOver : Screen {
		private Vector2 screenCenter;
		private ScreenGameplay level;

		private Texture2D background;

		private UIButton loadLastSaveButton;
		private UIButton backToMainMenuButton;

		public ScreenGameOver(ScreenGameplay level) {
			this.screenCenter = Display.GetScreenCenter();
			this.level = level;
		}

		public override void Initialize() {
			base.Initialize();
		}

		public override void LoadContent(ContentManager contentManager) {
			this.screenContent = new ContentManager(contentManager.ServiceProvider, "Content");

			background = screenContent.Load<Texture2D>("gui/pause_background");
			font = screenContent.Load<SpriteFont>("font/russo_one_32");

			loadLastSaveButton = new UIButton((int)screenCenter.X + 10, (int)screenCenter.Y + 70, "Continue");
			loadLastSaveButton.LoadContent(screenContent);
			loadLastSaveButton.onClick += LoadLastSave;

			backToMainMenuButton = new UIButton((int)screenCenter.X + 10, (int)screenCenter.Y + 125, "Back to menu");
			backToMainMenuButton.LoadContent(screenContent);
			backToMainMenuButton.onClick += BackToMainMenu;
		}

		public override void UnloadContent() {
			base.UnloadContent();
		}

		public override void Update(GameTime gameTime) {
			base.Update(gameTime);

			level.Update(gameTime);

			loadLastSaveButton.Update(gameTime);
			backToMainMenuButton.Update(gameTime);
		}

		public override void Draw(SpriteBatch spriteBatch, GameTime gameTime) {
			base.Draw(spriteBatch, gameTime);

			Vector2 displayCenter = Display.GetScreenCenter();

			level.Draw(spriteBatch, gameTime);
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
			spriteBatch.DrawString(font, "Game Over", displayCenter, Color.White);

			loadLastSaveButton.Draw(spriteBatch, gameTime);
			backToMainMenuButton.Draw(spriteBatch, gameTime);
		}

		public void LoadLastSave() {
			ScreenManager.GetInstance().ChangeScreen(new ScreenGameplay(true), true);
		}

		public void BackToMainMenu() {
			ScreenManager.GetInstance().ChangeScreen(new ScreenMainMenu(), true);
		}
	}
}
