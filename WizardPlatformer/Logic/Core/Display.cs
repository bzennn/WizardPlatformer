﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace WizardPlatformer {
	public class Display {
		public static readonly Vector2 CalcResolution = new Vector2(384, 216);
		public static readonly int CalcTileSideSize = 12;

		public static readonly Vector2 BaseResolution = new Vector2(1536, 864);
		public static readonly Vector2 DrawScale = BaseResolution / CalcResolution;
		public static readonly int TileSideSize = CalcTileSideSize * (int)DrawScale.X;
		public static Vector2 TargetResolution;
		public static Matrix ScreenScale;
		public static Matrix GameMatrix;

		private Display() { }

		public static void InitScaleMatrix(GraphicsDeviceManager graphics, int targetWidth, int targetHeigth) {
			TargetResolution = new Vector2(targetWidth, targetHeigth);

			float scaleWidth = TargetResolution.X / BaseResolution.X;
			float scaleHeigth = TargetResolution.Y / BaseResolution.Y;

			ScreenScale = Matrix.CreateScale(new Vector3(scaleWidth, scaleHeigth, 1));
			GameMatrix = ScreenScale;
		}

		public static Vector2 GetMouseCoordinates(MouseState mouseState) {
			return new Vector2(mouseState.X / TargetResolution.X, mouseState.Y / TargetResolution.Y);
		}

		public static Vector2 ScreenToLevelPosition(Vector2 position) {
			Matrix invertedGameMatrix = Matrix.Invert(GameMatrix);

			return Vector2.Transform(position, invertedGameMatrix);
		}

		public static Vector2 GetZeroScreenPositionOnLevel() {
			return ScreenToLevelPosition(Vector2.Zero);
		}

		public static Vector2 GetScreenCenter() {
			Vector2 zeroScreenPosition = Display.GetZeroScreenPositionOnLevel();
			return new Vector2(zeroScreenPosition.X + Display.BaseResolution.X / 2, zeroScreenPosition.Y + Display.BaseResolution.Y / 2);
		}
	}
}
