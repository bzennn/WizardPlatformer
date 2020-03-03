using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace WizardPlatformer {
	public static class Geometry {
        private static float correctionAngle = MathHelper.Pi / 2;

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

        public static float GetAngleBetweenVectors(Vector2 pointA, Vector2 pointB) {
            return (float)Math.Atan2(pointA.Y - pointB.Y, pointA.X - pointB.X) + MathHelper.Pi;
        }
	}
}
