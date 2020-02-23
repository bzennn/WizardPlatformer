using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace WizardPlatformer {
	public class WizardPlatformer : Game {
		GraphicsDeviceManager graphics;
		SpriteBatch spriteBatch;

		public WizardPlatformer() {
			graphics = new GraphicsDeviceManager(this);
			Content.RootDirectory = "Content";
		}

		protected override void Initialize() {
			ScreenResolution.InitScaleMatrix(graphics, 1280, 720);
			//ScreenResolution.InitScaleMatrix(graphics, 1920, 1080);

			IsMouseVisible = true;
			graphics.IsFullScreen = false;

			graphics.PreferredBackBufferWidth = (int) ScreenResolution.TargetResolution.X;
			graphics.PreferredBackBufferHeight = (int) ScreenResolution.TargetResolution.Y;
			graphics.ApplyChanges();

			ScreenManager.GetInstance().Initialize();

			base.Initialize();
		}

		protected override void LoadContent() {
			spriteBatch = new SpriteBatch(GraphicsDevice);

			ScreenManager.GetInstance().LoadContent(Content);
		}

		protected override void UnloadContent() {
			ScreenManager.GetInstance().UnloadContent();
		}

		protected override void Update(GameTime gameTime) {
			if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
				Exit();

			InputManager.GetInstance().CurrentKeyboardState = Keyboard.GetState();
			
			ScreenManager.GetInstance().Update(gameTime);

			base.Update(gameTime);
		}

		protected override void Draw(GameTime gameTime) {
			GraphicsDevice.Clear(new Color(140, 206, 223));

			spriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp, null, null, null, ScreenResolution.ScreenScale);
			ScreenManager.GetInstance().Draw(spriteBatch, gameTime);
			spriteBatch.End();

			base.Draw(gameTime);
		}
	}
}
