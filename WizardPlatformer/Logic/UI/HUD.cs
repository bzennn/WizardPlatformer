using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace WizardPlatformer.Logic.UI {
	class HUD {
		private EntityPlayer player;

		private Vector2 hudPosition;
		private int scaleFactor;

		private Texture2D sprite;
		private SpriteFont font;
		private SpriteFont smallFont;

		private Rectangle heart;
		private Rectangle heartHalf;
		private Rectangle heartDead;
		private Rectangle heartEmpty;

		private Rectangle coin;
		private string coinInfo;

		private Rectangle manaFrame;
		private Rectangle manaBar;
		private int manaBarWidth;

		private Rectangle staminaFrame;
		private Rectangle staminaBar;
		private int staminaBarWidth;

		private Vector2[] heartsPositions;
		private Vector2[] emptyHeartPositions;
		private Vector2 heartDeadPosition;
		private Vector2 coinPosition;
		private Vector2 coinInfoPosition;
		private Vector2 manaBarPosition;
		private Vector2 staminaBarPosition;

		public HUD(EntityPlayer player) {
			this.scaleFactor = (int)Display.DrawScale.X;
			this.player = player;
			this.heartsPositions = new Vector2[player.MaxHealth / 2];
			this.emptyHeartPositions = new Vector2[player.MaxHealth / 2];
			this.coinInfo = "";
		}

		public void LoadContent(ContentManager contentManager) {
			sprite = contentManager.Load<Texture2D>("gui/hud_sprite");
			font = contentManager.Load<SpriteFont>("font/russo_one_32");
			smallFont = contentManager.Load<SpriteFont>("font/russo_one_20");

			heart = new Rectangle(0, 24, 13, 12);
			heartHalf = new Rectangle(13, 24, 9, 12);
			heartDead = new Rectangle(32, 24, 8, 12);
			heartEmpty = new Rectangle(40, 24, 13, 12);
			coin = new Rectangle(22, 24, 10, 10);

			manaFrame = new Rectangle(0, 0, 96, 6);
			manaBar = new Rectangle(0, 6, 96, 6);
			manaBarWidth = 96;

			staminaFrame = new Rectangle(0, 12, 48, 6);
			staminaBar = new Rectangle(0, 18, 48, 6);
			staminaBarWidth = 48;
		}

		public void Update(GameTime gameTime) {
			UpdateHudPosition();
			UpdateDeadHeart();
			UpdateHearts();
			UpdateCoins();
			UpdateManaBar();
			UpdateStaminaBar();
		}

		private void UpdateHudPosition() {
			this.hudPosition = Display.GetZeroScreenPositionOnLevel() + new Vector2(20, 20);
		}

		private void UpdateHearts() {
			int health = player.Health;

			if (player.MaxHealth / 2 > heartsPositions.Length) {
				this.heartsPositions = new Vector2[player.MaxHealth / 2];
				this.emptyHeartPositions = new Vector2[player.MaxHealth / 2];
			}

			for (int i = 0; i < emptyHeartPositions.Length; i++) {
				emptyHeartPositions[i] = new Vector2(hudPosition.X + i * (heartEmpty.Width * scaleFactor + scaleFactor), hudPosition.Y);
			}

			for (int i = 0; i < health / 2; i++) {
				heartsPositions[i] = new Vector2(hudPosition.X + i * (heart.Width * scaleFactor + scaleFactor), hudPosition.Y);
			}

			if (health % 2 != 0) {
				heartsPositions[health / 2] = new Vector2(hudPosition.X + (health / 2) * (heart.Width * scaleFactor + scaleFactor), hudPosition.Y);
			}
		}

		private void UpdateDeadHeart() {
			heartDeadPosition = new Vector2(hudPosition.X, hudPosition.Y);
		}

		private void UpdateCoins() {
			coinPosition = new Vector2(hudPosition.X, hudPosition.Y + (heart.Height * scaleFactor + scaleFactor));
			coinInfoPosition = new Vector2(coinPosition.X + (coin.Width * scaleFactor + scaleFactor), coinPosition.Y + scaleFactor);
			coinInfo = "X " + player.Coins;
		}

		private void UpdateManaBar() {
			manaBarPosition = new Vector2(hudPosition.X, coinPosition.Y + coin.Height * scaleFactor + scaleFactor * 2);

			int manaPercent = player.Mana * 100 / player.MaxMana;
			manaBar.Width = manaPercent * manaBarWidth / 100;
		}

		private void UpdateStaminaBar() {
			staminaBarPosition = new Vector2(hudPosition.X, manaBarPosition.Y + manaBar.Height * scaleFactor + scaleFactor);

			int staminaPercent = player.Stamina * 100 / player.MaxStamina;
			staminaBar.Width = staminaPercent * staminaBarWidth / 100;
		}

		public void Draw(SpriteBatch spriteBatch, GameTime gameTime) {
			DrawHearts(spriteBatch, gameTime);
			DrawCoins(spriteBatch, gameTime);
			DrawManaBar(spriteBatch, gameTime);
			DrawStaminaBar(spriteBatch, gameTime);
			DrawLevelInfo(spriteBatch, gameTime);
		}

		private void DrawHearts(SpriteBatch spriteBatch, GameTime gameTime) {
			for (int i = 0; i < heartsPositions.Length; i++) {
				DrawHudPart(spriteBatch, heartEmpty, emptyHeartPositions[i]);
			}

			if (!player.IsAlive) {
				DrawHudPart(spriteBatch, heartDead, heartDeadPosition);
			}

			int j;
			for (j = 0; j < player.Health / 2; j++) {
				if (heartsPositions[j] != null) {
					DrawHudPart(spriteBatch, heart, heartsPositions[j]);
				}
			}

			if (player.Health % 2 != 0) {
				DrawHudPart(spriteBatch, heartHalf, heartsPositions[j]);
			}
		}

		private void DrawCoins(SpriteBatch spriteBatch, GameTime gameTime) {
			DrawHudPart(spriteBatch, coin, coinPosition);
			spriteBatch.DrawString(smallFont, coinInfo, coinInfoPosition, Color.AntiqueWhite);
		}

		private void DrawManaBar(SpriteBatch spriteBatch, GameTime gameTime) {
			DrawHudPart(spriteBatch, manaFrame, manaBarPosition);
			DrawHudPart(spriteBatch, manaBar, manaBarPosition);
		}

		private void DrawStaminaBar(SpriteBatch spriteBatch, GameTime gameTime) {
			DrawHudPart(spriteBatch, staminaFrame, staminaBarPosition);
			DrawHudPart(spriteBatch, staminaBar, staminaBarPosition);
		}

		private void DrawHudPart(SpriteBatch spriteBatch, Rectangle hudPart, Vector2 position) {
			spriteBatch.Draw(
				sprite,
				position,
				hudPart,
				Color.White,
				0.0f,
				Vector2.Zero,
				Display.DrawScale,
				SpriteEffects.None,
				0.5f
				);
		}

		private void DrawLevelInfo(SpriteBatch spriteBatch, GameTime gameTime) {
			Vector2 zeroPosition = Display.GetZeroScreenPositionOnLevel();
			Vector2 levelInfoPosition = new Vector2(zeroPosition.X + Display.BaseResolution.X - 200, zeroPosition.Y + 20);
			spriteBatch.DrawString(font, "Level " + (player.LevelId + 1), levelInfoPosition, Color.White);
		}
	}
}
