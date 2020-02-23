using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace WizardPlatformer {
	public static class Geometry {

		public static Vector2 GetCollisionDepth(Rectangle heatBoxA, Rectangle heatBoxB) {
			Vector2 centerA = heatBoxA.Center.ToVector2();
			Vector2 centerB = heatBoxB.Center.ToVector2();
          
            float offsetX = centerA.X - centerB.X;
            float offsetY = centerA.Y - centerB.Y;
            float minOffsetX = (heatBoxA.Width / 2) + (heatBoxB.Width / 2);
            float minOffsetY = (heatBoxA.Height / 2) + (heatBoxB.Height / 2);

            if (Math.Abs(offsetX) >= minOffsetX || Math.Abs(offsetY) >= minOffsetY) {
                return Vector2.Zero;
            }   

			float depthX = offsetX > 0 ? minOffsetX - offsetX : -minOffsetX - offsetX;
            float depthY = offsetY > 0 ? minOffsetY - offsetY : -minOffsetY - offsetY;

            return new Vector2(depthX, depthY);
        }
	}
}
