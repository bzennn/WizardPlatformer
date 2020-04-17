using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using WizardPlatformer.Logic.Level;
using WizardPlatformer.Logic.Save;
using WizardPlatformer.Logic.UI;

namespace WizardPlatformer {
	public class ScreenLevelComplete : Screen {
		private Vector2 screenCenter;
		private ScreenGameplay level;
		private int nextLevelId;
		private int nextRoomId;

		private SnapshotPlayer snapshotPlayer;
		private int score;
		private int drawScore;
		private long drawScoreTimer;
		private int drawScoreSpeed;

		private SpriteFont fontSmall;
		private Texture2D background;

		private UIButton nextLevelButton;
		private UIButton backToMainMenuButton;

		public ScreenLevelComplete(ScreenGameplay level, int nextLevelId, int nextRoomId) {
			this.screenCenter = Display.GetScreenCenter();
			this.level = level;
			this.nextLevelId = nextLevelId;
			this.nextRoomId = nextRoomId;
			this.snapshotPlayer = level.Level.Player.GetSnapshot();
			this.score = ComputePlayerScore();
			this.drawScore = 0;
			this.drawScoreTimer = 0;
			this.drawScoreSpeed = 1;
		}

		public override void Initialize() {
			base.Initialize();
		}

		public override void LoadContent(ContentManager contentManager) {
			this.screenContent = new ContentManager(contentManager.ServiceProvider, "Content");

			background = screenContent.Load<Texture2D>("gui/pause_background");
			font = screenContent.Load<SpriteFont>("font/russo_one_32");
			fontSmall = screenContent.Load<SpriteFont>("font/russo_one_20");

			nextLevelButton = new UIButton((int)screenCenter.X + 10, (int)screenCenter.Y + 90, "Next level");
			nextLevelButton.LoadContent(screenContent);
			nextLevelButton.onClick += NextLevel;

			backToMainMenuButton = new UIButton((int)screenCenter.X + 10, (int)screenCenter.Y + 145, "Back to menu");
			backToMainMenuButton.LoadContent(screenContent);
			backToMainMenuButton.onClick += BackToMainMenu;
		}

		public override void UnloadContent() {
			this.screenContent.Unload();
		}

		public override void Update(GameTime gameTime) {
			base.Update(gameTime);

			UpdateDrawScore(gameTime);

			level.Update(gameTime);

			nextLevelButton.Update(gameTime);
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
			spriteBatch.DrawString(font, "Level Complete", displayCenter, Color.White);
			spriteBatch.DrawString(fontSmall, "Score: " + drawScore, new Vector2(displayCenter.X, displayCenter.Y + 50), Color.White);

			nextLevelButton.Draw(spriteBatch, gameTime);
			backToMainMenuButton.Draw(spriteBatch, gameTime);
		}

		private void UpdateDrawScore(GameTime gameTime) {
			drawScoreTimer += gameTime.ElapsedGameTime.Milliseconds;

			if (drawScoreTimer > drawScoreSpeed) {
				drawScoreTimer = 0;

				if (drawScore < score - 100) {
					drawScore += 10;
				} else if (drawScore != score) {
					drawScore++;
				}
			}
		}

		private int ComputePlayerScore() {
			int score = 0;

			for (int i = 0; i < snapshotPlayer.Coins; i++) {
				score += 10;
			}

			for (int i = 0; i < snapshotPlayer.Damage; i++) {
				score += 75;
			}

			for (int i = 0; i < snapshotPlayer.Health; i++) {
				score += 125;
			}

			for (int i = 0; i < snapshotPlayer.Mana; i++) {
				score += 13;
			}

			for (int i = 0; i < snapshotPlayer.Stamina; i++) {
				score += 23;
			}

			return score;
		}

		public void NextLevel() {
			if (nextLevelId != -1 && nextRoomId != -1) {
				ScreenManager.GetInstance().ChangeScreen(new ScreenGameplay(nextLevelId, nextRoomId, true, snapshotPlayer), true);
			} else {
				ScreenManager.GetInstance().ChangeScreen(new ScreenGameEnd(level), true);
			}
		}

		public void BackToMainMenu() {
			ScreenManager.GetInstance().ChangeScreen(new ScreenMainMenu(), true);
		}
	}
}
