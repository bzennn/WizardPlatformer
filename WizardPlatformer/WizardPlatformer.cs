using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using WizardPlatformer.Logic.UI;

namespace WizardPlatformer {
	public class WizardPlatformer : Game {
		private static WizardPlatformer instance;

		private GraphicsDeviceManager graphics;
		private SpriteBatch spriteBatch;

		private UICursor cursor;

		public WizardPlatformer() {
			graphics = new GraphicsDeviceManager(this);
			Content.RootDirectory = "Content";

			cursor = new UICursor(Color.Multiply(Color.AliceBlue, 10));
		}

		public static WizardPlatformer GetInstance() {
			if (instance == null) {
				instance = new WizardPlatformer();
			}

			return instance;
		}

		protected override void Initialize() {
			Display.InitScaleMatrix(graphics, 1280, 720);
			//Display.InitScaleMatrix(graphics, 1920, 1080);
			IsMouseVisible = false;
			graphics.IsFullScreen = false;
			//graphics.IsFullScreen = true;

			graphics.PreferredBackBufferWidth = (int) Display.TargetResolution.X;
			graphics.PreferredBackBufferHeight = (int) Display.TargetResolution.Y;
			graphics.ApplyChanges();

			ScreenManager.GetInstance().Initialize();

			base.Initialize();
		}

		protected override void LoadContent() {
			spriteBatch = new SpriteBatch(GraphicsDevice);

			ScreenManager.GetInstance().LoadContent(Content);

			cursor.LoadContent(Content);
		}

		protected override void UnloadContent() {
			ScreenManager.GetInstance().UnloadContent();
		}

		protected override void Update(GameTime gameTime) {
			InputManager.GetInstance().CurrentKeyboardState = Keyboard.GetState();
			InputManager.GetInstance().CurrentMouseState =  Mouse.GetState();
			
			ScreenManager.GetInstance().Update(gameTime);

			cursor.Update(gameTime);

			base.Update(gameTime);
		}

		protected override void Draw(GameTime gameTime) {
			GraphicsDevice.Clear(new Color(140, 206, 223));

			spriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp, null, null, null, Display.GameMatrix);
			ScreenManager.GetInstance().Draw(spriteBatch, gameTime);
			cursor.Draw(spriteBatch, gameTime);
			spriteBatch.End();

			base.Draw(gameTime);
		}
	}
}
