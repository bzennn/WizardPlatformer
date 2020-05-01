using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.IO;
using WizardPlatformer.Logic.Save;
using WizardPlatformer.Logic.UI;

namespace WizardPlatformer {
	public class WizardPlatformer : Game {
		private static WizardPlatformer instance;

		public static readonly string DIRECTORY = Environment.GetFolderPath(Environment.SpecialFolder.Personal) + "/WizardPlatformer/";
		public static readonly string GAMEPLAY_SAVE_PATH = DIRECTORY + "save.dat";
		public static readonly string OPTIONS_PATH = DIRECTORY + "options.dat";
		public static readonly Dictionary<int, Point> RESOLUTION = new Dictionary<int, Point>() {
			{ 0, new Point(640, 360) },
			{ 1, new Point(960, 540) },
			{ 2, new Point(1280, 720) },
			{ 3, new Point(1360, 768) },
			{ 4, new Point(1366, 768) },
			{ 5, new Point(1600, 900) },
			{ 6, new Point(1920, 1080) },
			{ 7, new Point(2560, 1440) },
			{ 8, new Point(3200, 1800) },
			{ 9, new Point(3840, 2160) }
		};
		public static readonly Point TILESET_SIZE = new Point(12, 30);


		private GraphicsDeviceManager graphics;
		private SpriteBatch spriteBatch;

		private bool fullscreen;
		private int resolutionId;
		private Point maxResolution;

		private UICursor cursor;

		public WizardPlatformer() {
			graphics = new GraphicsDeviceManager(this);
			Content.RootDirectory = "Content";

			cursor = new UICursor(Color.Multiply(Color.AliceBlue, 10));
			maxResolution = new Point(GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width,
				GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height);

			LoadOptions();
		}

		private void LoadOptions() {
			if (!Directory.Exists(DIRECTORY)) {
				Directory.CreateDirectory(DIRECTORY);
			}
			if (!File.Exists(OPTIONS_PATH)) {
				SnapshotOptions options = new SnapshotOptions(false, 0);
				BINSerializer.Serialize(options, OPTIONS_PATH);
				fullscreen = options.Fullscreen;
				resolutionId = options.Resolution;
			} else {
				try {
					SnapshotOptions options = BINDeserializer.Deserialize<SnapshotOptions>(OPTIONS_PATH);
					if (options != null) {
						fullscreen = options.Fullscreen;
						Point currentResolution = RESOLUTION[options.Resolution];
						if (currentResolution.X > maxResolution.X ||
							currentResolution.Y > maxResolution.Y) {
							resolutionId = 0;
						} else {
							resolutionId = options.Resolution;
						}
					} else {
						throw new ArgumentException();
					}
				} catch {
					fullscreen = false;
					resolutionId = 0;
				}
			}
		}

		public static WizardPlatformer GetInstance() {
			if (instance == null) {
				instance = new WizardPlatformer();
			}

			return instance;
		}

		protected override void Initialize() {
			ApplyGraphicsChanges();

			ScreenManager.GetInstance().Initialize();

			base.Initialize();
		}

		public void ApplyGraphicsChanges() {
			LoadOptions();

			Point targetResolution = RESOLUTION[resolutionId];
			Display.InitScaleMatrix(graphics, targetResolution.X, targetResolution.Y);
			IsMouseVisible = false;
			graphics.IsFullScreen = fullscreen;

			graphics.PreferredBackBufferWidth = (int)Display.TargetResolution.X;
			graphics.PreferredBackBufferHeight = (int)Display.TargetResolution.Y;
			graphics.ApplyChanges();
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
