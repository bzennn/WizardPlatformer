using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WizardPlatformer {
	public class TileLiquid : Tile {
		public enum LiquidType {
			WATER,
			LAVA
		}

		private LiquidType liquidType;

		public TileLiquid(Texture2D texture, Point spritePos, CollisionType collision, PassType pass, LiquidType liquidType, int heatBoxWidth, int heatBoxHeigth, int heatBoxPosX, int heatBoxPosY, int posX, int posY) 
			: base(texture, spritePos, collision, pass, heatBoxWidth, heatBoxHeigth, heatBoxPosX, heatBoxPosY, posX, posY) {

			this.liquidType = liquidType;
		}


		public LiquidType Liquid {
			get { return liquidType; }
		}
	}
}
