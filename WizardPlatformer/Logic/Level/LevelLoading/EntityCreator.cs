using System.Collections.Generic;
using WizardPlatformer.Logic.Level;

namespace WizardPlatformer.Logic.Level.LevelLoading {
	public class EntityCreator {
		private Dictionary<int, string> entityIdMap;
		private Level level;

		private int scaleFactor;

		public EntityCreator(Dictionary<int, string> entityIdMap, Level level) {
			this.scaleFactor = (int)Display.DrawScale.X;
			this.entityIdMap = entityIdMap;
			this.level = level;
		}

		public Entity CreateEntity(int entityID, int entityPosX, int entityPosY, int sourceEntityId = 0) {
			switch (entityIdMap[entityID]) {
				default:
					return null;
				case "player":
					return new EntityPlayer(10, 10, 50, 100, 0, 50, 1, 5.0f, 0, true, 8, 20, 8 * scaleFactor, 4 * scaleFactor, entityPosX, entityPosY, level.RoomSizeId, level);
				case "range_ice_arrow_attack":
					return new EntityRangeAttack(3000, sourceEntityId, level.GetEntity(sourceEntityId).Damage, 7.0f, true, 4, 4, 11 * scaleFactor, 10 * scaleFactor, entityPosX, entityPosY, level.RoomSizeId, level, InputManager.GetInstance().GetMousePosition());
				case "melee_melee_attack":
					return new EntityMeleeAttack(450, sourceEntityId, (int)(level.GetEntity(sourceEntityId).Damage * 1.5f), 7.0f, true, 4, 4, 11 * scaleFactor, 10 * scaleFactor, entityPosX, entityPosY, level.RoomSizeId, level, InputManager.GetInstance().GetMousePosition());
				case "collectable_coin":
					return new EntityCollectable(10000, TileCollectable.CollectableType.COIN, 7.0f, true, 8, 8, 8 * scaleFactor, 8 * scaleFactor, entityPosX, entityPosY, level.RoomSizeId, level);
				case "collectable_mana_crystal":
					return new EntityCollectable(10000, TileCollectable.CollectableType.MANA_CRYSTAL, 7.0f, true, 6, 6, 9 * scaleFactor, 8 * scaleFactor, entityPosX, entityPosY, level.RoomSizeId, level);
				case "collectable_stamina_crystal":
					return new EntityCollectable(10000, TileCollectable.CollectableType.STAMINA_CRYSTAL, 7.0f, true, 6, 8, 9 * scaleFactor, 7 * scaleFactor, entityPosX, entityPosY, level.RoomSizeId, level);
				case "collectable_health_crystal":
					return new EntityCollectable(10000, TileCollectable.CollectableType.HEALTH_CRYSTAL, 7.0f, true, 6, 8, 9 * scaleFactor, 7 * scaleFactor, entityPosX, entityPosY, level.RoomSizeId, level);
				case "collectable_mana_upgrade":
					return new EntityCollectable(10000, TileCollectable.CollectableType.MANA_UPGRADE, 7.0f, true, 10, 10, 7 * scaleFactor, 7 * scaleFactor, entityPosX, entityPosY, level.RoomSizeId, level);
				case "collectable_stamina_upgrade":
					return new EntityCollectable(10000, TileCollectable.CollectableType.STAMINA_UPGRADE, 7.0f, true, 6, 10, 9 * scaleFactor, 7 * scaleFactor, entityPosX, entityPosY, level.RoomSizeId, level);
				case "collectable_health_upgrade":
					return new EntityCollectable(10000, TileCollectable.CollectableType.HEART, 7.0f, true, 7, 10, 8 * scaleFactor, 7 * scaleFactor, entityPosX, entityPosY, level.RoomSizeId, level);
				case "collectable_damage_upgrade":
					return new EntityCollectable(10000, TileCollectable.CollectableType.DAMAGE_UPGRADE, 7.0f, true, 12, 12, 6 * scaleFactor, 6 * scaleFactor, entityPosX, entityPosY, level.RoomSizeId, level);
			}
		}

		public void AddLevel(Level level) {
			this.level = level;
		}
	}
}
