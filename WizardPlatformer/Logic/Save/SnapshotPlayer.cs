using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using WizardPlatformer.Logic.Level;
using System;

namespace WizardPlatformer.Logic.Save {
	[Serializable]
	public class SnapshotPlayer {
		private float playerPositionX;
		private float playerPositionY;

		private int health;
		private int maxHealth;

		private int damage;

		private int mana;
		private int maxMana;
		private int manaRegenSpeed;

		private int stamina;
		private int maxStamina;
		private int staminaRegenSpeed;

		private int coins;

		private int rangeAttackCost;
		private int meleeAttackCost;

		//public SnapshotPlayer() { }

		public SnapshotPlayer(float playerPositionX, float playerPositionY, int health, int maxHealth, int damage, int mana, int maxMana, int manaRegenSpeed, int stamina, int maxStamina, int staminaRegenSpeed, int coins, int rangeAttackCost, int meleeAttackCost) {
			this.playerPositionX = playerPositionX;
			this.playerPositionY = playerPositionY;
			this.health = health;
			this.maxHealth = maxHealth;
			this.damage = damage;			
			this.mana = mana;
			this.maxMana = maxMana;
			this.manaRegenSpeed = manaRegenSpeed;
			this.stamina = stamina;
			this.maxStamina = maxStamina;
			this.staminaRegenSpeed = staminaRegenSpeed;
			this.coins = coins;
			this.rangeAttackCost = rangeAttackCost;
			this.meleeAttackCost = meleeAttackCost;
		}

		public float PlayerPositionX {
			get { return playerPositionX; }
			//set { this.playerPosition = value; }
		}

		public float PlayerPositionY {
			get { return playerPositionY; }
			//set { this.playerPosition = value; }
		}

		public int Health {
			get { return health; }
			//set { this.health = value; }
		}

		public int MaxHealth {
			get { return maxHealth; }
			//set { this.maxHealth = value; }
		}

		public int Damage {
			get { return damage; }
			//set { this.damage = value; }
		}

		public int Mana {
			get { return mana; }
			//set { this.mana = value; }
		}

		public int MaxMana {
			get { return maxMana; }
			//set { this.maxMana = value; }
		}

		public int ManaRegenSpeed {
			get { return manaRegenSpeed; }
			//set { this.manaRegenSpeed = value; }
		}

		public int Stamina {
			get { return stamina; }
			//set { this.stamina = value; }
		}

		public int MaxStamina {
			get { return maxStamina; }
			//set { this.maxStamina = value; }
		}

		public int StaminaRegenSpeed {
			get { return staminaRegenSpeed; }
			//set { this.staminaRegenSpeed = value; }
		}

		public int Coins {
			get { return coins; }
			//set { this.coins = value; }
		}

		public int RangeAttackCost {
			get { return rangeAttackCost; }
			//set { this.rangeAttackCost = value; }
		}

		public int MeleeAttackCost {
			get { return meleeAttackCost; }
			//set { this.meleeAttackCost = value; }
		}
	}
}
