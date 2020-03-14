using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using WizardPlatformer.Logic.Level.LevelLoading;
using WizardPlatformer.Logic.Level;
using WizardPlatformer.Logic.UI;
using WizardPlatformer.Logic.Save;

namespace WizardPlatformer {
	public class ScreenGameplay : Screen {

		#region Fields

		private LevelLoader levelLoader;
		private EntityCreator entityCreator;

		private Texture2D tileSet;
		private Point tileSetSize = new Point(12, 20);

		private Level currentLevel;
		private bool isLevelLoaded;

		private HUD hud;

		#endregion

		public override void Initialize() {
			base.Initialize();
		}

		public override void LoadContent(ContentManager contentManager) {
			base.LoadContent(contentManager);

			//SnapshotPlayer snapshot = new SnapshotPlayer(new Vector2(100, 4000), 7, 20, 2, 200, 300, 10, 100, 100, 10, 100, 50, 50);
			tileSet = screenContent.Load<Texture2D>("tile/tileset_export");
			font = screenContent.Load<SpriteFont>("font/russo_one_32");

			LoadLevel(0, 1);
			//currentLevel.Player.RestoreSnapshot(snapshot);
		}

		public override void Update(GameTime gameTime) {
			base.Update(gameTime);

			if (InputManager.GetInstance().IsKeyPressed(Keys.Enter)) {
				ScreenManager.GetInstance().ChangeScreen(new ScreenPause(this), false);
			}

			// For debug

			/*if (InputManager.GetInstance().IsKeyPressed(Keys.NumPad1)) {
				currentLevel = new Level(0, 0, levelLoader, entityCreator, new Point(100, 300));
				entityCreator.AddLevel(currentLevel);
				currentLevel.LoadContent(contentManager);
				hud = new HUD(currentLevel.Player);
				hud.LoadContent(contentManager);
			}

			if (InputManager.GetInstance().IsKeyPressed(Keys.NumPad2)) {
				currentLevel = new Level(0, 1, levelLoader, entityCreator, new Point(100, 300));
				entityCreator.AddLevel(currentLevel);
				currentLevel.LoadContent(contentManager);
				hud = new HUD(currentLevel.Player);
				hud.LoadContent(contentManager);
			}

			if (InputManager.GetInstance().IsKeyPressed(Keys.NumPad3)) {
				currentLevel = new Level(0, 2, levelLoader, entityCreator, new Point(100, 4000));
				entityCreator.AddLevel(currentLevel);
				currentLevel.LoadContent(contentManager);
				hud = new HUD(currentLevel.Player);
				hud.LoadContent(contentManager);
			}

			if (InputManager.GetInstance().IsKeyPressed(Keys.NumPad4)) {
				currentLevel = new Level(0, 3, levelLoader, entityCreator, new Point(100, 1300));
				entityCreator.AddLevel(currentLevel);
				currentLevel.LoadContent(contentManager);
				hud = new HUD(currentLevel.Player);
				hud.LoadContent(contentManager);
			}

			if (InputManager.GetInstance().IsKeyPressed(Keys.NumPad5)) {
				currentLevel = new Level(0, 4, levelLoader, entityCreator, new Point(100, 300));
				entityCreator.AddLevel(currentLevel);
				currentLevel.LoadContent(contentManager);
				hud = new HUD(currentLevel.Player);
				hud.LoadContent(contentManager);
			}*/

			// end debug

			if (currentLevel != null) {
				currentLevel.Update(gameTime);
				hud.Update(gameTime);

				if (currentLevel.IsLevelLoaded) {
					isLevelLoaded = true;
				}
			}
		}

		public override void Draw(SpriteBatch spriteBatch, GameTime gameTime) {
			base.Draw(spriteBatch, gameTime);

			if (currentLevel != null) {
				currentLevel.Draw(spriteBatch, gameTime);
				hud.Draw(spriteBatch, gameTime);
			}
		}

		private void LoadLevel(int levelId, int roomId) {
			currentLevel = new Level(levelId, roomId);
			levelLoader = new LevelLoader(tileSet, tileSetSize, currentLevel);
			currentLevel.AddLevelLoader(levelLoader);
			currentLevel.LoadContent(screenContent);

			hud = new HUD(currentLevel.Player);
			hud.LoadContent(screenContent);
		}

		public bool IsLevelLoaded {
			get { return isLevelLoaded; }
		}
	}
}
