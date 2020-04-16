using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using WizardPlatformer.Logic.UI;
using WizardPlatformer.Logic.Level;
using System.Collections.Generic;
using System.IO;

namespace WizardPlatformer {
	class ScreenMainMenu : Screen {
		private Vector2 screenCenter;
		private UIButton newGameButton;
		private UIButton continueGameButton;
		private UIButton optionsButton;
		private UIButton exitButton;

		private Background background;
		private List<UIButton> mainButtons;
		private List<UIButton> optionButtons;

		private bool hasPreviousSave;

		public ScreenMainMenu() {
			this.screenCenter = Display.GetScreenCenter();
			this.background = new Background(32, 18);
			this.mainButtons = new List<UIButton>();
			this.optionButtons = new List<UIButton>();
			this.hasPreviousSave = File.Exists("snapshot_gameplay.dat");
		}

		public override void Initialize() {
			base.Initialize();
		}

		public override void LoadContent(ContentManager contentManager) {
			base.LoadContent(contentManager);

			font = screenContent.Load<SpriteFont>("font/russo_one_32");

			// 55 dots per button by y side
			continueGameButton = new UIButton((int)screenCenter.X + 10, (int)screenCenter.Y + 70, "Continue Game");
			continueGameButton.LoadContent(screenContent);
			continueGameButton.onClick += ContinueGame;
			continueGameButton.IsEnabled = hasPreviousSave;
			mainButtons.Add(continueGameButton);

			newGameButton = new UIButton((int)screenCenter.X + 10, (int)screenCenter.Y + 125, "New Game");
			newGameButton.LoadContent(screenContent);
			newGameButton.onClick += StartNewGame;
			mainButtons.Add(newGameButton);

			optionsButton = new UIButton((int)screenCenter.X + 10, (int)screenCenter.Y + 180, "Options");
			optionsButton.LoadContent(screenContent);
			optionsButton.onClick += OpenOptions;
			optionsButton.IsEnabled = false;
			mainButtons.Add(optionsButton);

			exitButton = new UIButton((int)screenCenter.X + 10, (int)screenCenter.Y + 235, "Exit Game");
			exitButton.LoadContent(screenContent);
			exitButton.onClick += GameExit;
			mainButtons.Add(exitButton);

			background.LoadContent(screenContent, "01110");

			foreach (UIButton button in mainButtons) {
				//button.UpdateButtonPosition();
			}

			foreach (UIButton button in optionButtons) {
				button.UpdateButtonPosition();
			}
		}

		public override void Update(GameTime gameTime) {
			base.Update(gameTime);

			// Debug
			if (InputManager.GetInstance().IsKeyPressed(Keys.Enter)) {
				if (hasPreviousSave) {
					ContinueGame();
				} else {
					StartNewGame();
				}
			}

			foreach (UIButton button in mainButtons) {
				button.Update(gameTime);
			}

			background.Update(gameTime, InputManager.GetInstance().GetMouseScreenPosition());
		}

		public override void Draw(SpriteBatch spriteBatch, GameTime gameTime) {
			base.Draw(spriteBatch, gameTime);

			background.Draw(spriteBatch, gameTime);
			spriteBatch.DrawString(font, "Main menu", Display.GetScreenCenter(), Color.White);

			foreach (UIButton button in mainButtons) {
				button.Draw(spriteBatch, gameTime);
			}
		}

		public void StartNewGame() {
			ScreenManager.GetInstance().ChangeScreen(new ScreenGameplay(0, 0, true, null), true);
		}

		public void ContinueGame() {
			ScreenManager.GetInstance().ChangeScreen(new ScreenGameplay(true), true);
		}

		public void OpenOptions() {

		}

		public void GameExit() {
			WizardPlatformer.GetInstance().Exit();
		}
	}
}
