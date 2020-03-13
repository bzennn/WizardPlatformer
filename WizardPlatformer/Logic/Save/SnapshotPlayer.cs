using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using WizardPlatformer.Logic.Level;
using System;

namespace WizardPlatformer.Logic.Save {
	public class SnapshotPlayer {
		Vector2 playerPosition;

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

		public SnapshotPlayer(Vector2 playerPosition, int health, int maxHealth, int damage, int mana, int maxMana, int manaRegenSpeed, int stamina, int maxStamina, int staminaRegenSpeed, int coins, int rangeAttackCost, int meleeAttackCost) {
			this.playerPosition = playerPosition;
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

		public Vector2 PlayerPosition {
			get { return playerPosition; }
		}

		public int Health {
			get { return health; } 
		}

		public int MaxHealth {
			get { return maxHealth; }
		}

		public int Damage {
			get { return damage; }
		}

		public int Mana {
			get { return mana; }
		}

		public int MaxMana {
			get { return maxMana; }
		}

		public int ManaRegenSpeed {
			get { return manaRegenSpeed; }
		}

		public int Stamina {
			get { return stamina; }
		}

		public int MaxStamina {
			get { return maxStamina; }
		}

		public int StaminaRegenSpeed {
			get { return staminaRegenSpeed; }
		}

		public int Coins {
			get { return coins; }
		}

		public int RangeAttackCost {
			get { return rangeAttackCost; }
		}

		public int MeleeAttackCost {
			get { return meleeAttackCost; }
		}
	}
}
