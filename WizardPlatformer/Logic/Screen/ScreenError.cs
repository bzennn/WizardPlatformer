using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using System;
using WizardPlatformer.Logic.UI;
using System.IO;

namespace WizardPlatformer {
	public class ScreenError : Screen {

		private Vector2 zeroScreenPosition;
		private Exception catchedException;
		private SpriteFont fontSmall;

		private UIButton exitButton;

		private string crashReportPath;

		public ScreenError(Exception exception) {
			catchedException = exception;

			crashReportPath = WizardPlatformer.DIRECTORY + "crash_report_" + DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss") + ".txt";
			File.WriteAllText(crashReportPath, catchedException.Message + "\r\n" + catchedException.StackTrace);
		}

		public override void Initialize() {
			base.Initialize();
		}

		public override void LoadContent(ContentManager contentManager) {
			base.LoadContent(contentManager);

			zeroScreenPosition = Display.GetZeroScreenPositionOnLevel();
			exitButton = new UIButton((int)zeroScreenPosition.X + 10, (int)zeroScreenPosition.Y + 10, "Exit Game");
			exitButton.LoadContent(screenContent); 
			exitButton.onClick += ExitGame;

			font = screenContent.Load<SpriteFont>("font/russo_one_20");
			fontSmall = screenContent.Load<SpriteFont>("font/russo_one_12");
		}

		public override void Update(GameTime gameTime) {
			base.Update(gameTime);

			exitButton.Update(gameTime);
		}

		public override void Draw(SpriteBatch spriteBatch, GameTime gameTime) {
			base.Draw(spriteBatch, gameTime);

			exitButton.Draw(spriteBatch, gameTime);
			spriteBatch.DrawString(font, "Game Crashed!", new Vector2(zeroScreenPosition.X + 10, zeroScreenPosition.Y + 60), Color.White);
			spriteBatch.DrawString(fontSmall, "Crash report saved to: " + crashReportPath, new Vector2(zeroScreenPosition.X + 10, zeroScreenPosition.Y + 100), Color.White);
			spriteBatch.DrawString(fontSmall, "Message: " + PrepareString(catchedException.Message), new Vector2(zeroScreenPosition.X + 10, zeroScreenPosition.Y + 120), Color.White);
			spriteBatch.DrawString(fontSmall, PrepareString(catchedException.StackTrace), new Vector2(zeroScreenPosition.X + 10, zeroScreenPosition.Y + 140), Color.White);
		}

		public void ExitGame() {
			WizardPlatformer.GetInstance().Exit();
		}

		private string PrepareString(string str) {
			string outStr = "";
			for (int i = 0; i < str.Length; i++) {
				outStr += str[i];
				if (i % 100 == 0 && i != 0) {
					outStr += "\r\n";
				}
			}

			return outStr;
		} 
	}
}
