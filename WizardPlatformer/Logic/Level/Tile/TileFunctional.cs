using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WizardPlatformer {
	public class TileFunctional : Tile {
		private FunctionType type;

		public enum FunctionType {
			EXIT,
			ENTRANCE,
			DEADLY,
			TRIGGER
		}

		public TileFunctional(Texture2D texture, Point spritePos, CollisionType collision, PassType pass, FunctionType type, int heatBoxWidth, int heatBoxHeigth, int heatBoxPosX, int heatBoxPosY, int posX, int posY) 
			: base(texture, spritePos, collision, pass, heatBoxWidth, heatBoxHeigth, heatBoxPosX, heatBoxPosY, posX, posY) {

			this.type = type;
		}

		public override void Update(GameTime gameTime) { }

		public override void Draw(SpriteBatch spriteBatch, GameTime gameTime, float opacity = 1.0f) {
			base.Draw(spriteBatch, gameTime, opacity);
		}

		public FunctionType Type {
			get { return type; }
		}
	}
}
