﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;

namespace WizardPlatformer {
	public class ScreenPause : Screen {
		private Screen previousScreen;
		private Texture2D background;

		public ScreenPause(Screen previousScreen) {
			this.previousScreen = previousScreen;
		}

		public override void Initialize() {
			base.Initialize();
		}

		public override void LoadContent(ContentManager contentManager) {
			this.screenContent = new ContentManager(contentManager.ServiceProvider, "Content");

			background = screenContent.Load<Texture2D>("gui/pause_background");
			font = screenContent.Load<SpriteFont>("font/russo_one_32");
		}

		public override void UnloadContent() {
			this.screenContent.Unload();
		}

		public override void Update(GameTime gameTime) {
			base.Update(gameTime);

			if (InputManager.GetInstance().IsKeyPressed(Keys.Enter)) {
				ScreenManager.GetInstance().ReturnPreviousScreen();
			}
		}

		public override void Draw(SpriteBatch spriteBatch, GameTime gameTime) {
			base.Draw(spriteBatch, gameTime);

			previousScreen.Draw(spriteBatch, gameTime);
			spriteBatch.Draw(
				background,
				Vector2.Zero,
				null,
				Color.White,
				0.0f,
				Vector2.Zero,
				Display.DrawScale,
				SpriteEffects.None,
				0.0f);
			spriteBatch.DrawString(font, "Pause Screen", new Vector2(Display.TargetResolution.X / 2 - Display.GameMatrix.Translation.X, Display.TargetResolution.Y / 2 - Display.GameMatrix.Translation.Y), Color.White);
		}
	}
}
