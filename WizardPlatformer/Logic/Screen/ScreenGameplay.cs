﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using WizardPlatformer.Logic.Level.LevelLoading;
using WizardPlatformer.Logic.Level;

namespace WizardPlatformer {
	public class ScreenGameplay : Screen {

		#region Fields
		LevelLoader levelLoader;

		Texture2D tileSet;
		Point tileSetSize = new Point(12, 20);

		Level currentLevel;

		#endregion

		public override void Initialize() {
			base.Initialize();
		}

		public override void LoadContent(ContentManager contentManager) {
			base.LoadContent(contentManager);

			tileSet = screenContent.Load<Texture2D>("tile/tileset_export");
			font = screenContent.Load<SpriteFont>("font/russo_one_32");

			levelLoader = new LevelLoader(tileSet, tileSetSize);
			currentLevel = new Level(0, 3, levelLoader, new Point(100, 1300));
			currentLevel.LoadContent(contentManager);
		}

		public override void Update(GameTime gameTime) {
			base.Update(gameTime);

			if (InputManager.GetInstance().IsKeyPressed(Keys.Enter)) {
				ScreenManager.GetInstance().ChangeScreen(new ScreenPause(this), false);
			}

			currentLevel.Update(gameTime);
		}

		public override void Draw(SpriteBatch spriteBatch, GameTime gameTime) {
			base.Draw(spriteBatch, gameTime);

			currentLevel.Draw(spriteBatch, gameTime);
		}
	}
}
