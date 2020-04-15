using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using WizardPlatformer.Logic.Level;
using WizardPlatformer.Logic.Save;

namespace WizardPlatformer {
	public class ScreenLevelComplete : Screen {
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

		public ScreenLevelComplete(ScreenGameplay level, int nextLevelId, int nextRoomId) {
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
		}

		public override void UnloadContent() {
			this.screenContent.Unload();
		}

		public override void Update(GameTime gameTime) {
			base.Update(gameTime);

			if (InputManager.GetInstance().IsKeyPressed(Keys.Enter)) {
				if (nextLevelId != -1 && nextRoomId != -1) {
					ScreenManager.GetInstance().ChangeScreen(new ScreenGameplay(nextLevelId, nextRoomId, true, snapshotPlayer), true);
				} else {
					ScreenManager.GetInstance().ChangeScreen(new ScreenGameEnd(), true);
				}
			}

			UpdateDrawScore(gameTime);

			level.Update(gameTime);
			if (InputManager.GetInstance().IsKeyPressed(Keys.Back)) {
				ScreenManager.GetInstance().ChangeScreen(new ScreenMainMenu(), true);
			}
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
	}
}
