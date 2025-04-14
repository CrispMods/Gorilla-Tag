using System;
using UnityEngine;

// Token: 0x02000484 RID: 1156
public class AddCollidersToParticleSystemTriggers : MonoBehaviour
{
	// Token: 0x06001BFB RID: 7163 RVA: 0x000885FC File Offset: 0x000867FC
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

	// Token: 0x04001F05 RID: 7941
	public Collider[] collidersToAdd;

	// Token: 0x04001F06 RID: 7942
	public ParticleSystem particleSystemToUpdate;

	// Token: 0x04001F07 RID: 7943
	private int count;

	// Token: 0x04001F08 RID: 7944
	private int index;
}
