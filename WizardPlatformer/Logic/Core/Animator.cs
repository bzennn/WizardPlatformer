﻿using Microsoft.Xna.Framework;

namespace WizardPlatformer {
	public static class Animator {
		public static bool Animate(int row, int startFrame, int frameQuantity, bool repeat, int frameTimeCounter, ref Point currentFrame) {
			if (currentFrame.Y != row) {
				currentFrame.Y = row;
				currentFrame.X = startFrame;
				frameTimeCounter = 0;
			}

			if (frameTimeCounter == 0) {
				currentFrame.X++;

				if (currentFrame.X > frameQuantity - 1) {
					if (repeat) {
						currentFrame.X = startFrame;
						return true;
					} else {
						currentFrame.X--;
						return true;
					}

				}
			}

			return false;
		}
	}
}
