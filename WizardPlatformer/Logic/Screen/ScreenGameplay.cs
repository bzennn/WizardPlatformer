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

		private List<RoomIdentifier> levelsList;
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
			LoadLevel(levelId, roomId, false);
		}

		public override void Update(GameTime gameTime) {
			base.Update(gameTime);

			if (InputManager.GetInstance().IsKeyPressed(Keys.Enter)) {
				ScreenManager.GetInstance().ChangeScreen(new ScreenPause(this), false);
			}

			if (InputManager.GetInstance().IsKeyPressed(Keys.P)) {
				SaveGame();
			}

			if (InputManager.GetInstance().IsKeyPressed(Keys.O)) {
				RestoreGame();
			}

			UpdateLevelSwitchQuery();
			UpdateLevelSaveQuary();
			UpdateLevelRestoreQuery();

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

		private void LoadLevel(int levelId, int roomId, bool savePreviousLevelSnapshot) {
			if (!levelsList.Contains(new RoomIdentifier(levelId, roomId))) {
				throw new System.ArgumentException("Level " + levelId + "-" + roomId + " does not exist!");
			}

			int[] previousLevel = null;
			SnapshotLevel previousLevelSnapshot = null;

			if (currentLevel != null) {
				int prevLevelId = currentLevel.LevelId;
				int prevRoomId = currentLevel.RoomId;
				int playerXPos = (int)currentLevel.Player.HeatBox.X;
				int playerYPos = (int)currentLevel.Player.HeatBox.Y;

				if ((prevLevelId == levelId && prevRoomId != roomId) ||
					(prevLevelId != levelId && prevRoomId == roomId)) {
					previousLevel = new int[] { prevLevelId, prevRoomId, playerXPos, playerYPos };
					previousLevelSnapshot = currentLevel.GetSnapshot();
				}
				currentLevel.UnloadContent();
			}

			this.levelId = levelId;
			this.roomId = roomId;
			if (savePreviousLevelSnapshot) {
				currentLevel = new Level(levelId, roomId, previousLevel, previousLevelSnapshot);
			} else {
				currentLevel = new Level(levelId, roomId, null, null);
			}
			
			levelLoader = new LevelLoader(tileSet, tileSetSize, currentLevel);
			currentLevel.AddLevelLoader(levelLoader);
			currentLevel.LoadContent(screenContent);

			hud = new HUD(currentLevel.Player);
			hud.LoadContent(screenContent);
		}

		private void UpdateLevelSwitchQuery() {
			if (currentLevel != null) {
				if (currentLevel.HasLevelSwitchQuery) {
					if (currentLevel.SwitchLevel != null) {
						int levelId = currentLevel.SwitchLevel[0];
						int roomId = currentLevel.SwitchLevel[1];

						//if ((levelId > this.levelId || roomId > this.roomId) ||
						//	((levelId < this.levelId || roomId < this.roomId) && currentLevel.SwitchLevelSnapshot != null)) {
							SnapshotPlayer snapshotPlayer = currentLevel.Player.GetSnapshot();
							SnapshotLevel snapshotLevel = currentLevel.SwitchLevelSnapshot;
							bool restorePos = false;
							if (currentLevel.SwitchLevel[2] != -1 && currentLevel.SwitchLevel[3] != -1) {
								snapshotPlayer.PlayerPositionX = currentLevel.SwitchLevel[2];
								snapshotPlayer.PlayerPositionY = currentLevel.SwitchLevel[3];
								restorePos = true;
							}

							LoadLevel(levelId, roomId, true);
							if (snapshotLevel != null) {
								currentLevel.RestoreSnapshot(snapshotLevel);
							}
							currentLevel.Player.RestoreSnapshot(snapshotPlayer, restorePos);
						//}
					}
				}
			}
		}

		private void UpdateLevelSaveQuary() {
			if (currentLevel != null) {
				if (currentLevel.HasGameSaveQuery) {
					currentLevel.HasGameSaveQuery = false;
					SaveGame();
				}
			}
		}

		private void UpdateLevelRestoreQuery() {
			if (currentLevel != null) {
				if (currentLevel.HasGameRestoreQuery) {
					currentLevel.HasGameRestoreQuery = false;
					RestoreGame();
				}
			}
		}

		public SnapshotGameplay GetSnapshot() {
			return new SnapshotGameplay(
				currentLevel.Player.GetSnapshot(),
				currentLevel.GetSnapshot(),
				currentLevel.LevelId,
				currentLevel.RoomId);
		}

		public void RestoreSnapshot(SnapshotGameplay snapshot) {
			if (snapshot != null) {
				this.levelId = snapshot.LevelId;
				this.roomId = snapshot.RoomId;
				this.LoadLevel(this.levelId, this.roomId, false);
				this.currentLevel.RestoreSnapshot(snapshot.SnapshotLevel);
				this.currentLevel.Player.RestoreSnapshot(snapshot.SnapshotPlayer);
			}
		}

		public void SaveGame() {
			BINSaveSerializer.Serialize(GetSnapshot());
		}

		public void RestoreGame() {
			RestoreSnapshot(BINSaveDeserializer.Deserialize());
		}

		public bool IsLevelLoaded {
			get { return isLevelLoaded; }
		}
	}
}
