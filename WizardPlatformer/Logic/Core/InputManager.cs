using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace WizardPlatformer {
	public class InputManager {
		private static InputManager instance;

		private KeyboardState currentKeyboardState;
		private KeyboardState previousKeyboardState;

		private InputManager() {
			currentKeyboardState = Keyboard.GetState();
			previousKeyboardState = currentKeyboardState;
		}

		public static InputManager GetInstance() {
			if (instance == null) {
				instance = new InputManager();
			}

			return instance;
		}

		public KeyboardState CurrentKeyboardState {
			get { return this.currentKeyboardState; }
			set {
				this.previousKeyboardState = currentKeyboardState;
				this.currentKeyboardState = value; 
			}
		}

		public KeyboardState PreviousKeyboardState {
			get { return this.previousKeyboardState; }
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
	}
}
