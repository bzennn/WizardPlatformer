using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using WizardPlatformer.Logic.Level;
using System.Threading.Tasks;

namespace WizardPlatformer {
	public class ScreenLoading : Screen {
		//private Task task;
		private ScreenGameplay screenGameplay;
		private Texture2D background;

		private int delayCounter;

		public ScreenLoading(ScreenGameplay screenGameplay) {
			this.screenGameplay = screenGameplay;
			this.delayCounter = 1000;
		}

		public override void Initialize() {
			base.Initialize();
		}

		public override void LoadContent(ContentManager contentManager) {
			this.screenContent = new ContentManager(contentManager.ServiceProvider, "Content");

			//task = new Task(new System.Action());
		}

		public override void UnloadContent() {
			base.UnloadContent();
		}

		public override void Update(GameTime gameTime) {
			base.Update(gameTime);

			if (!screenGameplay.IsLevelLoaded) {
				screenGameplay.Update(gameTime);
			} else {
				if (delayCounter == 0) {
					ScreenManager.GetInstance().ReturnPreviousScreen();
				}
			}
		}

		public override void Draw(SpriteBatch spriteBatch, GameTime gameTime) {
			base.Draw(spriteBatch, gameTime);
		}
	}
}
