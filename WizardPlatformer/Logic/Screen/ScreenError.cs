using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using System;

namespace WizardPlatformer {
	public class ScreenError : Screen {

		private Exception catchedException;

		public ScreenError(Exception exception) {
			catchedException = exception;
		}

		public override void Initialize() {
			base.Initialize();
		}

		public override void LoadContent(ContentManager contentManager) {
			base.LoadContent(contentManager);

			font = screenContent.Load<SpriteFont>("font/russo_one_12");
		}

		public override void Update(GameTime gameTime) {
			base.Update(gameTime);
		}

		public override void Draw(SpriteBatch spriteBatch, GameTime gameTime) {
			base.Draw(spriteBatch, gameTime);

			spriteBatch.DrawString(font, catchedException.Message + "\n" + catchedException.StackTrace, Vector2.Zero, Color.Black);
		}
	}
}
