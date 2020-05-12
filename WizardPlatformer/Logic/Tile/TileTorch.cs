using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WizardPlatformer {
	public class TileTorch : Tile {
		public TileTorch(Texture2D texture, Point spritePos, CollisionType collision, PassType pass, int hitBoxWidth, int hitBoxHeigth, int hitBoxPosX, int hitBoxPosY, int posX, int posY) 
			: base(texture, spritePos, collision, pass, hitBoxWidth, hitBoxHeigth, hitBoxPosX, hitBoxPosY, posX, posY) {

			this.spriteSize = new Point(3, 1);
			this.isAnimated = true;
		}

		public override void Update(GameTime gameTime) {
			base.Update(gameTime);

			Animator.Animate(0, 0, 3, true, this.frameTimeCounter, ref this.currentFrame);
		}

		public override void Draw(SpriteBatch spriteBatch, GameTime gameTime, float opacity = 1) {
			base.Draw(spriteBatch, gameTime, opacity);
		}
	}
}
