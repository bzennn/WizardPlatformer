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
		private Texture2D tileSet;

		private List<RoomIdentifier> levelsList;
		private int levelId;
		private int roomId;
		private bool saveOnStart;
		private SnapshotPlayer snapshotPlayer;
		private Level currentLevel;
		private bool isLevelLoaded;

		private bool isLevelComplete;
		private bool restorePreviousGame;
		private bool isGameOver;

		private HUD hud;

		#endregion

		public ScreenGameplay(int levelId, int roomId, bool saveOnStart, SnapshotPlayer snapshotPlayer = null) {
			this.levelId = levelId;
			this.roomId = roomId;
			this.saveOnStart = saveOnStart;
			this.snapshotPlayer = snapshotPlayer;
			this.isLevelComplete = false;
			this.isGameOver = false;
		}

		public ScreenGameplay(bool restorePreviousGame) {
			this.levelId = 0;
			this.roomId = 0;
			this.saveOnStart = false;
			this.snapshotPlayer = null;
			this.isLevelComplete = false;
			this.restorePreviousGame = true;
		}

		public override void Initialize() {
			base.Initialize();
		}

		public override void LoadContent(ContentManager contentManager) {
			base.LoadContent(contentManager);

			tileSet = screenContent.Load<Texture2D>("tile/tileset_export");
			font = screenContent.Load<SpriteFont>("font/russo_one_32");
			levelsList = XMLLevelListLoader.LoadLevelsList();

			if (restorePreviousGame) {
				RestoreGame();
			} else {
				LoadLevel(levelId, roomId, false, null, snapshotPlayer, false);

				if (saveOnStart) {
					SaveGame();
				}
			}
		}

		public override void Update(GameTime gameTime) {
			base.Update(gameTime);

			if (InputManager.GetInstance().IsKeyPressed(Keys.Escape) && !isLevelComplete && !isGameOver) {
				ScreenManager.GetInstance().ChangeScreen(new ScreenPause(this), false);
			}

			UpdateLevelSwitchQuery();
			UpdateLevelSaveQuary();
			UpdateLevelRestoreQuery();
			if (!isLevelComplete) {
				UpdateLevelCompleteQuery();
			}
			if (!isGameOver) {
				UpdateGameOver();
			}

			if (currentLevel != null) {
				currentLevel.Update(gameTime);
				hud.Update(gameTime);

				if (currentLevel.IsLevelLoaded) {
					isLevelLoaded = true;
				}
			}

			// Debug
			if (InputManager.GetInstance().IsKeyPressed(Keys.P)) {
				SaveGame();
			}

			if (InputManager.GetInstance().IsKeyPressed(Keys.O)) {
				RestoreGame();
			}
		}

		public override void Draw(SpriteBatch spriteBatch, GameTime gameTime) {
			base.Draw(spriteBatch, gameTime);

			if (currentLevel != null) {
				currentLevel.Draw(spriteBatch, gameTime);
				hud.Draw(spriteBatch, gameTime);
			}
		}

		private void LoadLevel(int levelId, int roomId, bool savePreviousLevelSnapshot, SnapshotLevel snapshotLevel, SnapshotPlayer snapshotPlayer, bool restorePlayerPos) {
			if (!levelsList.Contains(new RoomIdentifier(levelId, roomId))) {
				throw new System.ArgumentException("Level " + levelId + "-" + roomId + " does not exist!");
			}

			int[] previousLevel = null;
			SnapshotLevel previousLevelSnapshot = null;

			if (currentLevel != null) {
				int prevLevelId = currentLevel.LevelId;
				int prevRoomId = currentLevel.RoomId;
				int playerXPos = (int)currentLevel.Player.HitBox.X;
				int playerYPos = (int)currentLevel.Player.HitBox.Y;

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
			
			levelLoader = new LevelLoader(tileSet, WizardPlatformer.TILESET_SIZE, currentLevel);
			currentLevel.AddLevelLoader(levelLoader);
			currentLevel.LoadContent(screenContent);

			if (snapshotLevel != null) {
				currentLevel.RestoreSnapshot(snapshotLevel);
			}
			
			if (snapshotPlayer != null) {
				currentLevel.Player.RestoreSnapshot(snapshotPlayer, restorePlayerPos);
			}
			

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

							LoadLevel(levelId, roomId, true, snapshotLevel, snapshotPlayer, restorePos);
							//if (snapshotLevel != null) {
							//	currentLevel.RestoreSnapshot(snapshotLevel);
							//}
							//currentLevel.Player.RestoreSnapshot(snapshotPlayer, restorePos);
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

		private void UpdateLevelCompleteQuery() {
			if (currentLevel != null) {
				if (currentLevel.HasLevelCompleteQuery) {
					if (currentLevel.SwitchLevel != null) {
						int levelId = currentLevel.SwitchLevel[0];
						int roomId = currentLevel.SwitchLevel[1];

						isLevelComplete = true;

						currentLevel.DespawnAllEntitiesExceptPlayer();
						currentLevel.HasLevelCompleteQuery = false;
						SaveGame();

						ScreenManager.GetInstance().ChangeScreen(new ScreenLevelComplete(this, levelId, roomId), false);
					}
				}
			}
		}

		private void UpdateGameOver() {
			if (currentLevel != null) {
				if (currentLevel.IsPlayerDied) {
					currentLevel.IsPlayerDied = false;
					isGameOver = true;

					ScreenManager.GetInstance().ChangeScreen(new ScreenGameOver(this), false);
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
				this.LoadLevel(this.levelId, this.roomId, false, snapshot.SnapshotLevel, snapshot.SnapshotPlayer, true);
			}
		}

		public void SaveGame() {
			BINSerializer.Serialize(GetSnapshot(), WizardPlatformer.GAMEPLAY_SAVE_PATH);
		}

		public void RestoreGame() {
			RestoreSnapshot(BINDeserializer.Deserialize<SnapshotGameplay>(WizardPlatformer.GAMEPLAY_SAVE_PATH));
		}

		public bool IsLevelLoaded {
			get { return isLevelLoaded; }
		}

		public Level Level {
			get { return currentLevel; }
		}
	}
}
