using System;
using UnityEngine;

// Token: 0x02000490 RID: 1168
public class AddCollidersToParticleSystemTriggers : MonoBehaviour
{
	// Token: 0x06001C4C RID: 7244 RVA: 0x000DBF28 File Offset: 0x000DA128
	private void Update()
	{
		this.count = 0;
		while (this.count < 6)
		{
			this.index++;
			if (this.index >= this.collidersToAdd.Length)
			{
				if (BetterDayNightManager.instance.collidersToAddToWeatherSystems.Count >= this.index - this.collidersToAdd.Length)
				{
					this.index = 0;
				}
				else
				{
					this.particleSystemToUpdate.trigger.SetCollider(this.count, BetterDayNightManager.instance.collidersToAddToWeatherSystems[this.index - this.collidersToAdd.Length]);
				}
			}
			if (this.index < this.collidersToAdd.Length)
			{
				this.particleSystemToUpdate.trigger.SetCollider(this.count, this.collidersToAdd[this.index]);
			}
			this.count++;
		}
	}

	// Token: 0x04001F53 RID: 8019
	public Collider[] collidersToAdd;

	// Token: 0x04001F54 RID: 8020
	public ParticleSystem particleSystemToUpdate;

	// Token: 0x04001F55 RID: 8021
	private int count;

	// Token: 0x04001F56 RID: 8022
	private int index;
}
