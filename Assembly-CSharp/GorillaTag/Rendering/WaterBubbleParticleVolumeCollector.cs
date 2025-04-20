using System;
using System.Collections.Generic;
using GorillaExtensions;
using GorillaLocomotion;
using GorillaLocomotion.Swimming;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace GorillaTag.Rendering
{
	// Token: 0x02000C32 RID: 3122
	public class WaterBubbleParticleVolumeCollector : MonoBehaviour
	{
		// Token: 0x06004E43 RID: 20035 RVA: 0x001AE7E0 File Offset: 0x001AC9E0
		protected void Awake()
		{
			List<WaterVolume> componentsInHierarchy = SceneManager.GetActiveScene().GetComponentsInHierarchy(true, 64);
			List<Collider> list = new List<Collider>(componentsInHierarchy.Count * 4);
			foreach (WaterVolume waterVolume in componentsInHierarchy)
			{
				if (!(waterVolume.Parameters != null) || waterVolume.Parameters.allowBubblesInVolume)
				{
					foreach (Collider collider in waterVolume.volumeColliders)
					{
						if (!(collider == null))
						{
							list.Add(collider);
						}
					}
				}
			}
			this.bubbleableVolumeColliders = list.ToArray();
			this.particleTriggerModules = new ParticleSystem.TriggerModule[this.particleSystems.Length];
			this.particleEmissionModules = new ParticleSystem.EmissionModule[this.particleSystems.Length];
			for (int i = 0; i < this.particleSystems.Length; i++)
			{
				this.particleTriggerModules[i] = this.particleSystems[i].trigger;
				this.particleEmissionModules[i] = this.particleSystems[i].emission;
			}
			for (int j = 0; j < this.particleSystems.Length; j++)
			{
				ParticleSystem.TriggerModule triggerModule = this.particleTriggerModules[j];
				for (int k = 0; k < list.Count; k++)
				{
					triggerModule.SetCollider(k, this.bubbleableVolumeColliders[k]);
				}
			}
			this.SetEmissionState(false);
		}

		// Token: 0x06004E44 RID: 20036 RVA: 0x001AE980 File Offset: 0x001ACB80
		protected void LateUpdate()
		{
			bool headInWater = GTPlayer.Instance.HeadInWater;
			if (headInWater && !this.emissionEnabled)
			{
				this.SetEmissionState(true);
				return;
			}
			if (!headInWater && this.emissionEnabled)
			{
				this.SetEmissionState(false);
			}
		}

		// Token: 0x06004E45 RID: 20037 RVA: 0x001AE9C0 File Offset: 0x001ACBC0
		private void SetEmissionState(bool setEnabled)
		{
			float rateOverTimeMultiplier = setEnabled ? 1f : 0f;
			for (int i = 0; i < this.particleEmissionModules.Length; i++)
			{
				this.particleEmissionModules[i].rateOverTimeMultiplier = rateOverTimeMultiplier;
			}
			this.emissionEnabled = setEnabled;
		}

		// Token: 0x04005038 RID: 20536
		public ParticleSystem[] particleSystems;

		// Token: 0x04005039 RID: 20537
		private ParticleSystem.TriggerModule[] particleTriggerModules;

		// Token: 0x0400503A RID: 20538
		private ParticleSystem.EmissionModule[] particleEmissionModules;

		// Token: 0x0400503B RID: 20539
		private Collider[] bubbleableVolumeColliders;

		// Token: 0x0400503C RID: 20540
		private bool emissionEnabled;
	}
}
