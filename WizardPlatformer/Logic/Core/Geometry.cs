using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace WizardPlatformer {
	public static class Geometry {
		public static Vector2 GetCollisionDepth(Rectangle hitBoxA, Rectangle hitBoxB) {
			Vector2 centerA = hitBoxA.Center.ToVector2();
			Vector2 centerB = hitBoxB.Center.ToVector2();
          
            float offsetX = centerA.X - centerB.X;
            float offsetY = centerA.Y - centerB.Y;
            float minOffsetX = (hitBoxA.Width / 2) + (hitBoxB.Width / 2);
            float minOffsetY = (hitBoxA.Height / 2) + (hitBoxB.Height / 2);

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
