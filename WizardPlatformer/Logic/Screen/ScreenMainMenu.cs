using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using WizardPlatformer.Logic.UI;
using WizardPlatformer.Logic.Level;
using System.Collections.Generic;
using System.IO;
using WizardPlatformer.Logic.Save;

namespace WizardPlatformer {
	class ScreenMainMenu : Screen {
		private Vector2 screenCenter;
		private UIButton newGameButton;
		private UIButton continueGameButton;
		private UIButton controlsButton;
		private UIButton optionsButton;
		private UIButton exitButton;

		private UIButton fullscreenOptionButton;
		private UIButton resolutionOptionButton;
		private UIButton saveOptionButton;
		private UIButton closeOptionsButton;
		private bool fullscreen;
		private int resolutionId;
		private SpriteFont fontSmall;

		private UIButton closeControlsButton;
		private Texture2D controlsBack;
		private Texture2D controlsPic;

		private Background background;
		private List<UIButton> mainButtons;
		private List<UIButton> optionButtons;

		private bool hasPreviousSave;
		private bool isOptionMenuOpen;
		private bool isControlsOpen;

		public ScreenMainMenu() {
			this.screenCenter = Display.GetScreenCenter();
			this.background = new Background(32, 18);
			this.mainButtons = new List<UIButton>();
			this.optionButtons = new List<UIButton>();
			this.hasPreviousSave = File.Exists(WizardPlatformer.GAMEPLAY_SAVE_PATH);
			this.isOptionMenuOpen = false;
			this.isControlsOpen = false;
			this.fullscreen = false;
			this.resolutionId = 0;
		}

		public override void Initialize() {
			base.Initialize();
		}

		public override void LoadContent(ContentManager contentManager) {
			base.LoadContent(contentManager);

			font = screenContent.Load<SpriteFont>("font/russo_one_32");
			fontSmall = screenContent.Load<SpriteFont>("font/russo_one_20");

			#region Main menu buttons
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

			controlsButton = new UIButton((int)screenCenter.X + 10, (int)screenCenter.Y + 180, "Controls");
			controlsButton.LoadContent(screenContent);
			controlsButton.onClick += OpenControls;
			mainButtons.Add(controlsButton);

			optionsButton = new UIButton((int)screenCenter.X + 10, (int)screenCenter.Y + 235, "Options");
			optionsButton.LoadContent(screenContent);
			optionsButton.onClick += OpenOptions;
			mainButtons.Add(optionsButton);

			exitButton = new UIButton((int)screenCenter.X + 10, (int)screenCenter.Y + 290, "Exit Game");
			exitButton.LoadContent(screenContent);
			exitButton.onClick += GameExit;
			mainButtons.Add(exitButton);

			#endregion

			#region Options menu buttons
			
			fullscreenOptionButton = new UIButton((int)screenCenter.X + 10, (int)screenCenter.Y + 70, "Fullscreen");
			fullscreenOptionButton.LoadContent(screenContent);
			fullscreenOptionButton.onClick += OnFullscreenOptionClick;
			optionButtons.Add(fullscreenOptionButton);

			resolutionOptionButton = new UIButton((int)screenCenter.X + 10, (int)screenCenter.Y + 125, "Resolution");
			resolutionOptionButton.LoadContent(screenContent);
			resolutionOptionButton.onClick += OnResolutionOptionClick;
			optionButtons.Add(resolutionOptionButton);

			saveOptionButton = new UIButton((int)screenCenter.X + 10, (int)screenCenter.Y + 180, "Save");
			saveOptionButton.LoadContent(screenContent);
			saveOptionButton.IsEnabled = false;
			saveOptionButton.onClick += OnSaveOptionsClick;
			optionButtons.Add(saveOptionButton);

			closeOptionsButton = new UIButton((int)screenCenter.X + 10, (int)screenCenter.Y + 235, "Back to menu");
			closeOptionsButton.LoadContent(screenContent);
			closeOptionsButton.onClick += OnBackToMenuClick;
			optionButtons.Add(closeOptionsButton);

			#endregion

			#region Controls menu buttons

			closeControlsButton = new UIButton((int)screenCenter.X -100, (int)screenCenter.Y + 330, "Back to menu");
			closeControlsButton.LoadContent(screenContent);
			closeControlsButton.onClick += OnBackToMenuClick;

			controlsBack = screenContent.Load<Texture2D>("gui/pause_background");
			controlsPic = screenContent.Load<Texture2D>("gui/controls_hint");

			#endregion

			background.LoadContent(screenContent, "01110");
		}

		public override void Update(GameTime gameTime) {
			base.Update(gameTime);

			if (!isOptionMenuOpen && !isControlsOpen) {
				foreach (UIButton button in mainButtons) {
					button.Update(gameTime);
				}
			} else if (isOptionMenuOpen) {
				foreach (UIButton button in optionButtons) {
					button.Update(gameTime);
				}
			} else if (isControlsOpen) {
				closeControlsButton.Update(gameTime);
			}

			background.Update(gameTime, InputManager.GetInstance().GetMouseScreenPosition());
		}

		public override void Draw(SpriteBatch spriteBatch, GameTime gameTime) {
			base.Draw(spriteBatch, gameTime);

			background.Draw(spriteBatch, gameTime);
			if (!isControlsOpen) {
				spriteBatch.DrawString(font, "Main menu", Display.GetScreenCenter(), Color.White);
			}

			if (!isOptionMenuOpen && !isControlsOpen) {
				foreach (UIButton button in mainButtons) {
					button.Draw(spriteBatch, gameTime);
				}
			} else if (isOptionMenuOpen) {
				foreach (UIButton button in optionButtons) {
					button.Draw(spriteBatch, gameTime);
				}

				Vector2 screenCenter = Display.GetScreenCenter();
				Point currentResolution = WizardPlatformer.RESOLUTION[resolutionId];
				spriteBatch.DrawString(fontSmall, ": " + (fullscreen ? "ON" : "OFF"), new Vector2(screenCenter.X + 76 * Display.DrawScale.X, screenCenter.Y + 75), Color.White);
				spriteBatch.DrawString(fontSmall, ": " + (currentResolution.X + "x" + currentResolution.Y), new Vector2(screenCenter.X + 76 * Display.DrawScale.X, screenCenter.Y + 130), Color.White);
			} else if (isControlsOpen) {
				spriteBatch.Draw(
				controlsBack,
				Display.GetZeroScreenPositionOnLevel(),
				null,
				Color.White,
				0.0f,
				Vector2.Zero,
				Display.DrawScale,
				SpriteEffects.None,
				0.0f);
				spriteBatch.Draw(
				controlsPic,
				Display.GetZeroScreenPositionOnLevel(),
				null,
				Color.White,
				0.0f,
				Vector2.Zero,
				Vector2.One,
				SpriteEffects.None,
				0.0f);
				closeControlsButton.Draw(spriteBatch, gameTime);
			}
		}

		public void StartNewGame() {
			ScreenManager.GetInstance().ChangeScreen(new ScreenGameplay(0, 0, true, null), true);
		}

		public void ContinueGame() {
			ScreenManager.GetInstance().ChangeScreen(new ScreenGameplay(true), true);
		}

		public void OpenControls() {
			isControlsOpen = true;
		}

		public void OpenOptions() {
			isOptionMenuOpen = true;

			SnapshotOptions options = BINDeserializer.Deserialize<SnapshotOptions>(WizardPlatformer.OPTIONS_PATH);
			if (options != null) {
				fullscreen = options.Fullscreen;
				resolutionId = options.Resolution;
			}
		}

		public void GameExit() {
			WizardPlatformer.GetInstance().Exit();
		}

		public void OnFullscreenOptionClick() {
			fullscreen = !fullscreen;
			saveOptionButton.IsEnabled = true;
		}

		public void OnResolutionOptionClick() {
			if (resolutionId + 1 == WizardPlatformer.RESOLUTION.Count) {
				resolutionId = 0;
			} else {
				resolutionId++;
			}
			saveOptionButton.IsEnabled = true;
		}

		public void OnSaveOptionsClick() {
			SnapshotOptions options = new SnapshotOptions(fullscreen, resolutionId);
			BINSerializer.Serialize(options, WizardPlatformer.OPTIONS_PATH);
			saveOptionButton.IsEnabled = false;
		}

		public void OnBackToMenuClick() {
			isOptionMenuOpen = false;
			isControlsOpen = false;
		}
	}
}
