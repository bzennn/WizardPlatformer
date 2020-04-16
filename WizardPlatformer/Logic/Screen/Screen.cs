using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;

namespace WizardPlatformer {
	public abstract class Screen {
		protected SpriteFont font;
		protected ContentManager screenContent;

		public virtual void Initialize() { }

		public virtual void LoadContent(ContentManager contentManager) {
			screenContent = new ContentManager(contentManager.ServiceProvider, "Content");
			//Display.GameMatrix = Display.ScreenScale;
		}

		public virtual void UnloadContent() {
			screenContent.Unload();
			//Display.GameMatrix = Display.ScreenScale;
		}

		public virtual void Update(GameTime gameTime) { }

		public virtual void Draw(SpriteBatch spriteBatch, GameTime gameTime) { }
	}
}
