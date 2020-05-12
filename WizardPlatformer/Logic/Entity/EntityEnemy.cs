using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using WizardPlatformer.Logic.Level;

namespace WizardPlatformer {
	public abstract class EntityEnemy : EntityLiving {
		private List<Func<bool>> AITasks;

		private int currentTaskID;
		private bool taskComplete;

		public EntityEnemy(int health, int damage, float velocity, bool emulatePhysics, int hitBoxWidth, int hitBoxHeight, int hitBoxSpritePosX, int hitBoxSpritePosY, int posX, int posY, int roomSizeId, Level level) 
			: base(health, damage, velocity, emulatePhysics, hitBoxWidth, hitBoxHeight, hitBoxSpritePosX, hitBoxSpritePosY, posX, posY, roomSizeId, level) {

			this.AITasks = new List<Func<bool>>();
			this.currentTaskID = 0;
			this.taskComplete = true;
		}

		public override void Update(GameTime gameTime) {
			base.Update(gameTime);

			UpdateAITask();

			if(taskComplete) {
				SelectAITask();
			}
		}

		private void UpdateAITask() {
			taskComplete = AITasks[currentTaskID].Invoke();
		}

		private void SelectAITask() {
			currentTaskID = RandomManager.GetRandom().Next(AITasks.Count);
			taskComplete = false;
		}

		public void AddAITask(Func<bool> AITask) {
			if (!AITasks.Contains(AITask)) {
				AITasks.Add(AITask);
			}
		}
	}
}
