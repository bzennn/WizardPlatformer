using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using System.Collections.Generic;
using System;

namespace WizardPlatformer {
	public class ScreenManager {
		private static ScreenManager instance;
		private Stack<Screen> screenStack = new Stack<Screen>();
		private ContentManager screenContent;

		private ScreenManager() { }

		public static ScreenManager GetInstance() {
			if (instance == null) {
				instance = new ScreenManager();
			}

			return instance;
		}

		public void ChangeScreen(Screen screen, bool clearPrevious) {
			if (clearPrevious) {
				screenStack.Pop().UnloadContent();
				screenStack.Push(screen);
				screenStack.Peek().LoadContent(screenContent);
			} else {
				screenStack.Push(screen);
				screenStack.Peek().LoadContent(screenContent);
			}
		}

		public void ReturnPreviousScreen() {
			if (screenStack.Count > 1) {
				screenStack.Pop().UnloadContent();
			} else {
				throw new Exception("ScreenStack is empty.");
			}
		}

		public void Initialize() {
			screenStack.Push(new ScreenMainMenu());
		}

		public void LoadContent(ContentManager contentManager) {
			screenContent = new ContentManager(contentManager.ServiceProvider, "Content");
			screenStack.Peek().LoadContent(screenContent);
		}

		public void UnloadContent() {
			while (screenStack.Count != 0) {
				screenStack.Pop().UnloadContent();
			}
		}

		public void Update(GameTime gameTime) {
			screenStack.Peek().Update(gameTime);
		}

		public void Draw(SpriteBatch spriteBatch, GameTime gameTime) {
			screenStack.Peek().Draw(spriteBatch, gameTime);
		}
	}
}
