using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace WizardPlatformer {
	public class InputManager {
		private static InputManager instance;

		private KeyboardState currentKeyboardState;
		private KeyboardState previousKeyboardState;

		private MouseState currentMouseState;
		private MouseState previousMouseState;

		private InputManager() {
			currentKeyboardState = Keyboard.GetState();
			previousKeyboardState = currentKeyboardState;

			currentMouseState = Mouse.GetState();
			previousMouseState = currentMouseState;
		}

		public static InputManager GetInstance() {
			if (instance == null) {
				instance = new InputManager();
			}

			return instance;
		}

		public KeyboardState CurrentKeyboardState {
			get { return currentKeyboardState; }
			set {
				previousKeyboardState = currentKeyboardState;
				currentKeyboardState = value; 
			}
		}

		public MouseState CurrentMouseState {
			get { return currentMouseState; }
			set {
				previousMouseState = currentMouseState;
				currentMouseState = value; }
		}

		public KeyboardState PreviousKeyboardState {
			get { return previousKeyboardState; }
		}

		public bool IsKeyPressed(Keys key) {
			if (currentKeyboardState.IsKeyDown(key) && previousKeyboardState.IsKeyUp(key)) {
				return true;
			}

			return false;
		}

		public bool IsKeyReleased(Keys key) {
			if (currentKeyboardState.IsKeyUp(key) && previousKeyboardState.IsKeyDown(key)) {
				return true;
			}

			return false;
		}

		public bool IsKeyDown(Keys key) {
			if (currentKeyboardState.IsKeyDown(key)) {
				return true;
			}

			return false;
		}

		public Vector2 GetMousePosition() {
			Vector2 mousePosition = Display.ScreenToLevelPosition(currentMouseState.Position.ToVector2());

			return mousePosition;
		}

		public Vector2 GetMouseScreenPosition() {
			//Vector2 mousePosition = Display.ScreenToLevelPosition(currentMouseState.Position.ToVector2());
			Vector2 mousePosition = currentMouseState.Position.ToVector2();

			int mousePositionX = (int)mousePosition.X;
			int mousePositionY = (int)mousePosition.Y;

			if (mousePositionX < 0) {
				mousePositionX = 0;
			} else if (mousePositionX > (int)Display.BaseResolution.X) {
				mousePositionX = (int)Display.BaseResolution.X;
			}

			if (mousePositionY < 0) {
				mousePositionY = 0;
			} else if (mousePositionY > (int)Display.BaseResolution.Y) {
				mousePositionY = (int)Display.BaseResolution.Y;
			}

			return new Vector2(mousePositionX, mousePositionY);
		}

		public bool IsMouseLeftButtonPressed() {
			if (currentMouseState.LeftButton == ButtonState.Pressed &&
				previousMouseState.LeftButton == ButtonState.Released) {
				return true;
			}

			return false;
		}

		public bool IsMouseLeftButtonReleased() {
			if (currentMouseState.LeftButton == ButtonState.Released &&
				previousMouseState.LeftButton == ButtonState.Pressed) {
				return true;
			}

			return false;
		}
		public bool IsMouseLeftButtonDown() {
			if (currentMouseState.LeftButton == ButtonState.Pressed) {
				return true;
			}

			return false;
		}

		public bool IsMouseRightButtonPressed() {
			if (currentMouseState.RightButton == ButtonState.Pressed &&
				previousMouseState.RightButton == ButtonState.Released) {
				return true;
			}

			return false;
		}

		public bool IsMouseRightButtonReleased() {
			if (currentMouseState.RightButton == ButtonState.Released &&
				previousMouseState.RightButton == ButtonState.Pressed) {
				return true;
			}

			return false;
		}
		public bool IsMouseRightButtonDown() {
			if (currentMouseState.RightButton == ButtonState.Pressed) {
				return true;
			}

			return false;
		}
	}
}
