using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WizardPlatformer {
	public class TileCheckpointFire : Tile {
		public TileCheckpointFire(Texture2D texture, Point spritePos, CollisionType collision, PassType pass, int hitBoxWidth, int hitBoxHeigth, int hitBoxPosX, int hitBoxPosY, int posX, int posY) 
			: base(texture, spritePos, collision, pass, hitBoxWidth, hitBoxHeigth, hitBoxPosX, hitBoxPosY, posX, posY) {
		}
	}
}
