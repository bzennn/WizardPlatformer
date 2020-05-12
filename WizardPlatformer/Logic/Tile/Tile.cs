using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace WizardPlatformer {
	public class Tile {
		public enum CollisionType {
			PASSABLE,
			IMPASSABLE,
			PLATFORM
		}

		public enum PassType {
			REGULAR,
			HOSTILE
		}

		private Rectangle hitBox;
		private Vector2 hitBoxOffset;
		public Vector2 tilePosition;

		protected Point currentFrame;
		protected Point spriteSize;
		protected int frameTimeCounter;
		protected int frameUpdateMillis;
		protected bool isAnimated;

		protected Texture2D sprite;
		protected Vector2 spritePosition;
		private Vector2 spriteOffset;
		private Rectangle spriteOnMap;
		protected Rectangle frameRectangle;

		private int tileSideSize;
		private int scaleFactor;

		private CollisionType collision;
		private PassType pass;
		
		private bool isCollapsed;

		protected bool drawDebugInfo;

		public Tile(Texture2D texture, Point spritePos, CollisionType collision, PassType pass, int hitBoxWidth, int hitBoxHeigth, int hitBoxPosX, int hitBoxPosY, int posX, int posY) {
			this.scaleFactor = (int)Display.DrawScale.X;
			this.tileSideSize = Display.CalcTileSideSize;

			this.hitBox = new Rectangle(posX, posY, hitBoxWidth * scaleFactor, hitBoxHeigth * scaleFactor);
			this.hitBoxOffset = Vector2.Zero;
			this.tilePosition = new Vector2(posX, posY);

			this.currentFrame = new Point(0, 0);
			this.spriteSize = new Point(1, 1);
			this.frameTimeCounter = 0;
			this.frameUpdateMillis = 150;
			this.isAnimated = false;

			this.sprite = texture;
			this.spritePosition = new Vector2(posX - hitBoxPosX, posY - hitBoxPosY);
			this.spriteOffset = new Vector2(hitBoxPosX, hitBoxPosY);
			this.spriteOnMap = new Rectangle(spritePos.X * tileSideSize, spritePos.Y * tileSideSize, tileSideSize, tileSideSize);
			this.frameRectangle = new Rectangle(spriteOnMap.X + currentFrame.X * tileSideSize, spriteOnMap.Y + currentFrame.Y * tileSideSize, tileSideSize, tileSideSize);

			this.collision = collision;
			this.pass = pass;

			this.isCollapsed = false;

			this.drawDebugInfo = false;
		}

		public virtual void Update(GameTime gameTime) {
			if (isAnimated) {
				UpdateFrameCounter(gameTime);
			}
		}

		public virtual void Draw(SpriteBatch spriteBatch, GameTime gameTime, float opacity = 1.0f) {
			spriteBatch.Draw(
				sprite,
				spritePosition,
				frameRectangle,
				Color.White * opacity,
				0.0f,
				Vector2.Zero,
				Display.DrawScale,
				SpriteEffects.None,
				0.5f);

			DrawDebugInfo(spriteBatch, gameTime);
		}

		private void DrawDebugInfo(SpriteBatch spriteBatch, GameTime gameTime) {
			if (drawDebugInfo) {
				spriteBatch.Draw(
				sprite,
				tilePosition,
				new Rectangle(11 * tileSideSize, 19 * tileSideSize, hitBox.Width / scaleFactor, hitBox.Height / scaleFactor),
				Color.White,
				0.0f,
				Vector2.Zero,
				Display.DrawScale,
				SpriteEffects.None,
				0.5f
				);
			}
		}

		private void UpdateFrameCounter(GameTime gameTime) {
			frameTimeCounter += gameTime.ElapsedGameTime.Milliseconds;
			if (frameTimeCounter > frameUpdateMillis) {
				frameTimeCounter = 0;
				UpdateFramePosition();
			}
		}

		private void UpdateFramePosition() {
			this.frameRectangle = new Rectangle(spriteOnMap.X + currentFrame.X * tileSideSize, spriteOnMap.Y + currentFrame.Y * tileSideSize, tileSideSize, tileSideSize);
		}

		public void Collapse() {
			isCollapsed = true;
		}

		public Vector2 TilePosition {
			get { return tilePosition; }
			set {
				tilePosition = value;
				hitBox.X = (int)tilePosition.X - (int)hitBoxOffset.X;
				hitBox.Y = (int)tilePosition.Y - (int)hitBoxOffset.Y;
				spritePosition = tilePosition - spriteOffset;
			}
		}

		public CollisionType Collision {
			get { return collision; }
			set { collision = value; }
		}

		public PassType Pass {
			get { return pass; }
		}

		public Rectangle HitBox {
			get { return hitBox; }
			set { hitBox = value; }
		}

		public Vector2 HitBoxOffset {
			get { return hitBoxOffset; }
			set { hitBoxOffset = value; }
		}
		
		public bool DebugInfo {
			get { return drawDebugInfo; }
			set { drawDebugInfo = value; }
		}
		
		public bool IsCollapsed {
			get { return isCollapsed; }
		}
	}
}
