using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using WizardPlatformer.Logic.Level.LevelLoading;
using WizardPlatformer.Logic.Level;
using WizardPlatformer.Logic.UI;
using WizardPlatformer.Logic.Save;
using System.Collections.Generic;

namespace WizardPlatformer {
	public class ScreenGameplay : Screen {

		#region Fields

		private LevelLoader levelLoader;
		private EntityCreator entityCreator;

		private Texture2D tileSet;
		private Point tileSetSize = new Point(12, 20);

		private List<int[]> levelsList;
		private int levelId;
		private int roomId;
		private Level currentLevel;
		private bool isLevelLoaded;

		private HUD hud;

		#endregion

		public override void Initialize() {
			base.Initialize();
		}

		public override void LoadContent(ContentManager contentManager) {
			base.LoadContent(contentManager);

			tileSet = screenContent.Load<Texture2D>("tile/tileset_export");
			font = screenContent.Load<SpriteFont>("font/russo_one_32");
			levelsList = XMLLevelListLoader.LoadLevelsList();
			

			levelId = 0;
			roomId = 0;
			LoadLevel(levelId, roomId);
		}

		public override void Update(GameTime gameTime) {
			base.Update(gameTime);

			if (InputManager.GetInstance().IsKeyPressed(Keys.Enter)) {
				ScreenManager.GetInstance().ChangeScreen(new ScreenPause(this), false);
			}

			if (InputManager.GetInstance().IsKeyPressed(Keys.P)) {
				BINSaveSerializer.Serialize(GetSnapshot());
			}

			if (InputManager.GetInstance().IsKeyPressed(Keys.O)) {
				RestoreSnapshot(BINSaveDeserializer.Deserialize());
			}

			UpdateLevelSwitchQuery();

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
			int[] previousLevel = null;
			if (currentLevel != null) {
				int prevLevelId = currentLevel.LevelId;
				int prevRoomId = currentLevel.RoomId;

				if ((prevLevelId == levelId && prevRoomId != roomId) ||
					(prevLevelId != levelId && prevRoomId == roomId)) {
					previousLevel = new int[] { prevLevelId, prevRoomId };
				}
				currentLevel.UnloadContent();
			}

			currentLevel = new Level(levelId, roomId, previousLevel);
			levelLoader = new LevelLoader(tileSet, tileSetSize, currentLevel);
			currentLevel.AddLevelLoader(levelLoader);
			currentLevel.LoadContent(screenContent);

			hud = new HUD(currentLevel.Player);
			hud.LoadContent(screenContent);
		}

		private void UpdateLevelSwitchQuery() {
			if (currentLevel != null) {
				if (currentLevel.HasLevelSwitchQuery) {
					SnapshotPlayer snapshotPlayer = currentLevel.Player.GetSnapshot();
					LoadLevel(currentLevel.SwitchLevel[0], currentLevel.SwitchLevel[1]);
					currentLevel.Player.RestoreSnapshot(snapshotPlayer, false);
				}
			}
		}

		public SnapshotGameplay GetSnapshot() {
			return new SnapshotGameplay(
				currentLevel.Player.GetSnapshot(),
				currentLevel.GetSnapshot(),
				levelId,
				roomId);
		}

		public void RestoreSnapshot(SnapshotGameplay snapshot) {
			if (snapshot != null) {
				this.levelId = snapshot.LevelId;
				this.roomId = snapshot.RoomId;
				this.LoadLevel(this.levelId, this.roomId);
				this.currentLevel.RestoreSnapshot(snapshot.SnapshotLevel);
				this.currentLevel.Player.RestoreSnapshot(snapshot.SnapshotPlayer);
			}
		}

		public bool IsLevelLoaded {
			get { return isLevelLoaded; }
		}
	}
}
