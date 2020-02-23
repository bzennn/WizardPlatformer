using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace WizardPlatformer {
	public interface IDrawable {
		void Update(GameTime gameTime);
		void Draw(GameTime gameTime, SpriteBatch spriteBatch);
	}
}
