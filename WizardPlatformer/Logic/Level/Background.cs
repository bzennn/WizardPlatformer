using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WizardPlatformer.Logic.Save;

namespace WizardPlatformer.Logic.Level {
	class Background {
		private Texture2D[] background;
		private Vector2[] backgroundPositions;

		private int tileSideSize;
		private int halfScreenWidth;

		private int roomWidth;
		private int roomHeigth;

		private Vector2 currentPositionOffset;

		private float offsetL1;
		private float offsetL2;
		private float offsetL3;

		private float offsetR1;
		private float offsetR2;
		private float offsetR3;

		private float velocityCoefficientX;
		private float velocityCoefficientY;

		private float velocityCoefficientMaxX;
		private float velocityCoefficientMaxY;

		private float cloudsOffset;
		private float cloudsVelocity;
		private bool isCloudsOn;

		private int size;
		private string backgroundId;

		public Background(int roomWidth, int roomHeigth) {
			this.background = new Texture2D[7];
			this.backgroundPositions = new Vector2[7];

			this.tileSideSize = Display.TileSideSize;
			this.halfScreenWidth = (int)Display.BaseResolution.X / 2;

			this.roomWidth = roomWidth;
			this.roomHeigth = roomHeigth;

			this.currentPositionOffset = Vector2.Zero;

			this.offsetL1 = 0;
			this.offsetL2 = 0;
			this.offsetL3 = 0;

			this.offsetR1 = 0;
			this.offsetR2 = 0;
			this.offsetR3 = 0;

			this.velocityCoefficientX = 0.0f;
			this.velocityCoefficientY = 0.0f;

			this.velocityCoefficientMaxX = 1.0f;
			this.velocityCoefficientMaxY = 0.1f;

			this.cloudsOffset = 0.0f;
			this.cloudsVelocity = 0.25f;
			this.isCloudsOn = false;
		}

		public void LoadContent(ContentManager contentManager, string backgroundId) {
			background[0] = contentManager.Load<Texture2D>("background/back_" + backgroundId[0] + "/sky_" + backgroundId[1]);

			if (backgroundId[2].Equals('1')) {
				background[1] = contentManager.Load<Texture2D>("background/back_" + backgroundId[0] + "/sun");
			} else if (backgroundId[2].Equals('2')) {
				background[1] = contentManager.Load<Texture2D>("background/back_" + backgroundId[0] + "/moon");
			}
			
			background[2] = contentManager.Load<Texture2D>("background/back_" + backgroundId[0] + "/size_" + backgroundId[4] + "/sky_mountains_" + backgroundId[1]);
			background[3] = contentManager.Load<Texture2D>("background/back_" + backgroundId[0] + "/size_" + backgroundId[4] + "/l_0");
			
			if (backgroundId[3].Equals('1')) {
				isCloudsOn = true;
				background[4] = contentManager.Load<Texture2D>("background/back_" + backgroundId[0] + "/clouds");
			}
			
			background[5] = contentManager.Load<Texture2D>("background/back_" + backgroundId[0] + "/size_" + backgroundId[4] + "/l_1");
			background[6] = contentManager.Load<Texture2D>("background/back_" + backgroundId[0] + "/size_" + backgroundId[4] + "/l_2");	

			if (roomWidth <= 32) {
				velocityCoefficientMaxX = 0.05f;
			}

			if (roomHeigth > 18) {
				velocityCoefficientMaxY = 0.3f;
			}

			size = int.Parse(backgroundId[4].ToString());
			this.backgroundId = backgroundId;
		}

		public void Update(GameTime gameTime, Vector2 playerPosition) {
			bool hasTransitionL = playerPosition.X <= halfScreenWidth && roomWidth > 32;
			bool hasTransitionR = playerPosition.X >= roomWidth * tileSideSize - halfScreenWidth && roomWidth > 32;
			bool hasTransition = !hasTransitionL && !hasTransitionR;

			velocityCoefficientX = velocityCoefficientMaxX;
			velocityCoefficientY = velocityCoefficientMaxY;

			if (!hasTransition) {
				if (hasTransitionL) {
					offsetL1 = GetTransitionOffset(0.1f, velocityCoefficientX, playerPosition.X);
					offsetL2 = GetTransitionOffset(0.2f, velocityCoefficientX, playerPosition.X);
					offsetL3 = GetTransitionOffset(0.3f, velocityCoefficientX, playerPosition.X);
					velocityCoefficientX = 0.0f;
				}

				if (hasTransitionR) {
					velocityCoefficientX = 0.0f;
				}
			}

			for (int i = 0; i < background.Length; i++) {
				if (background[i] != null) {
					backgroundPositions[i] = Display.GetZeroScreenPositionOnLevel();
					switch (i) {
						case 0: // sky
							break;
						case 1: // sun/moon
							break;
						case 2: // far mountains
							break;
						case 3: // mountains
							currentPositionOffset = GetPositionOffset(0.1f, velocityCoefficientX, velocityCoefficientY, playerPosition, hasTransition, hasTransitionL, hasTransitionR, offsetL1, offsetR1);
							backgroundPositions[i] -= currentPositionOffset;
							if (hasTransition) {
								offsetR1 = currentPositionOffset.X;
							}
							break;
						case 4: // clouds
							if (isCloudsOn) {
								cloudsOffset += cloudsVelocity;
								if (cloudsOffset >= halfScreenWidth * 2) {
									cloudsOffset = 0;
								}
								backgroundPositions[i] -= new Vector2(cloudsOffset, 0);
							}
							break;
						case 5: // far forest
							currentPositionOffset = GetPositionOffset(0.2f, velocityCoefficientX, velocityCoefficientY, playerPosition, hasTransition, hasTransitionL, hasTransitionR, offsetL2, offsetR2);
							backgroundPositions[i] -= currentPositionOffset;
							if (hasTransition) {
								offsetR2 = currentPositionOffset.X;
							}
							break;
						case 6: // forest
							currentPositionOffset = GetPositionOffset(0.3f, velocityCoefficientX, velocityCoefficientY, playerPosition, hasTransition, hasTransitionL, hasTransitionR, offsetL3, offsetR3);
							backgroundPositions[i] -= currentPositionOffset;
							if (hasTransition) {
								offsetR3 = currentPositionOffset.X;
							}
							break;
						default:
							break;
					}
				}
			}
		}

		public void Draw(SpriteBatch spriteBatch, GameTime gameTime) {
			for (int i = 0; i < background.Length; i++) {
				Vector2 drawPosition = backgroundPositions[i];
				if (i != 0 && i != 1 && i != 4) {
					if (roomHeigth > 36) {
						drawPosition = new Vector2(backgroundPositions[i].X, backgroundPositions[i].Y - size * (background.Length - i) * 100);
					} else {
						drawPosition = new Vector2(backgroundPositions[i].X, backgroundPositions[i].Y - size * 400);
					}
				}
				
				if (background[i] != null) {
					spriteBatch.Draw(
					background[i],
					drawPosition,
					null,
					Color.White,
					0.0f,
					Vector2.Zero,
					Display.DrawScale,
					SpriteEffects.None,
					0.0f);
				}
			}
		}

		private Vector2 GetPositionOffset(float baseVelocity, float velocityCoefficientX, float velocityCoefficientY, Vector2 playerPosition, bool hasTransition, bool hasTransitionL, bool hasTransitionR, float offsetL, float offsetR) {
			Vector2 positionOffset = Vector2.Zero;

			if (hasTransition) {
				positionOffset = GetBackgroundOffset(baseVelocity, velocityCoefficientX, velocityCoefficientY, playerPosition, hasTransitionL, offsetL);
			 } else {
				if (hasTransitionL) {
					positionOffset = GetBackgroundOffset(baseVelocity, velocityCoefficientX, velocityCoefficientY, playerPosition, false, 0.0f);
				} else if (hasTransitionR) {
					positionOffset = new Vector2(offsetR, playerPosition.Y * baseVelocity * velocityCoefficientY);
				}
			}

			return positionOffset;
		}

		private float GetTransitionOffset(float baseVelocity, float velocityCoefficient, float playerPosition) {
			return Math.Abs(playerPosition * baseVelocity * velocityCoefficient);
		}

		private Vector2 GetBackgroundOffset(float baseVelocity, float velocityCoefficientX, float velocityCoefficientY, Vector2 playerPosition, bool hasTransition, float transitionOffset) {
			return new Vector2(playerPosition.X * baseVelocity * velocityCoefficientX - (hasTransition ? 0 : transitionOffset),
							playerPosition.Y * baseVelocity * velocityCoefficientY);
		}

		public SnapshotBackground GetSnapshot() {
			return new SnapshotBackground(
				currentPositionOffset.X,
				currentPositionOffset.Y,
				offsetL1,
				offsetL2,
				offsetL3,
				offsetR1,
				offsetR2,
				offsetR3,
				velocityCoefficientX,
				velocityCoefficientY,
				backgroundId);
		}

		public void RestoreSnapshot(SnapshotBackground snapshot) {
			this.currentPositionOffset = new Vector2(snapshot.CurrentPositionOffsetX, snapshot.CurrentPositionOffsetY);
			this.offsetL1 = snapshot.OffsetL1;
			this.offsetL2 = snapshot.OffsetL2;
			this.offsetL3 = snapshot.OffsetL3;
			this.offsetR1 = snapshot.OffsetR1;
			this.offsetR2 = snapshot.OffsetR2;
			this.offsetR3 = snapshot.OffsetR3;
			this.velocityCoefficientX = snapshot.VelocityCoefficientX;
			this.velocityCoefficientY = snapshot.VelocityCoefficientY;
		}
	}
}
