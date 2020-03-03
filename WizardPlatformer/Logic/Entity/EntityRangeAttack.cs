using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WizardPlatformer.Logic.Level;

namespace WizardPlatformer {
	public class EntityRangeAttack : Entity {
		private int TTL;
		private bool isStartAnimationEnd;

		public EntityRangeAttack(int TTL, int damage, float velocity, bool emulatePhysics, int heatBoxWidth, int heatBoxHeight, int heatBoxSpritePosX, int heatBoxSpritePosY, int posX, int posY, int roomSizeId, Level level, Vector2 directionVector) 
			: base(0, damage, velocity, emulatePhysics, heatBoxWidth, heatBoxHeight, heatBoxSpritePosX, heatBoxSpritePosY, posX, posY, roomSizeId, level) {

			this.TTL = TTL;
			this.isGravityOn = false;
			this.isRotatable = true;

			this.spriteRotation = Geometry.GetAngleBetweenVectors(this.EntityPosition, directionVector);

			this.drawDebugInfo = true;
			this.isStartAnimationEnd = false;
		}

		public override void LoadContent(ContentManager contentManager) {
			base.LoadContent(contentManager);

			this.sprite = contentManager.Load<Texture2D>("entity/ice_arrow_sprite");
			this.spriteSize = new Point(1, 5);
			this.spriteRotationOrigin = new Vector2(12, 12);
			this.UpdateSpritePosition();
		}

		public override void Update(GameTime gameTime) {
			base.Update(gameTime);

			TTL -= gameTime.ElapsedGameTime.Milliseconds;

			if (!isStartAnimationEnd) {
				Animator.Animate(0, 0, 3, false, frameTimeCounter, ref currentFrame);
				if (!currentFrame.Equals(new Point(0, 2))) {
					isStartAnimationEnd = true;
				}
			} else {
				Animator.Animate(1, 0, 2, true, frameTimeCounter, ref currentFrame);
			}
			
			if (Math.Cos(this.spriteRotation) > 0) {
				AccelerateRight(0.4f, false);
			}  else {
				AccelerateLeft(-0.4f, false);
			}

			if (TTL <= 0) {
				this.Die();
				this.level.DespawnEntity(this);
			}

			//this.spriteFlip = SpriteEffects.None;
		}

		protected override void DrawDebugInfo(SpriteBatch spriteBatch, GameTime gameTime) {
			base.DrawDebugInfo(spriteBatch, gameTime);

			if (drawDebugInfo) {
				spriteBatch.DrawString(debugFont, "Angle = " + Math.Cos(this.spriteRotation), heatBox.Location.ToVector2() + new Vector2(60, 0), Color.AntiqueWhite);
			}
		}

		protected override void HandleExtraTile(Tile tile) {
			base.HandleExtraTile(tile);

			if (tile.Collision == Tile.CollisionType.IMPASSABLE) {
				this.Die();
				this.level.DespawnEntity(this);
			}
		}
	}
}
