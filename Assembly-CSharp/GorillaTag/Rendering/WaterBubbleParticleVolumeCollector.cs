using System;
using System.Collections.Generic;
using GorillaExtensions;
using GorillaLocomotion;
using GorillaLocomotion.Swimming;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace GorillaTag.Rendering
{
	// Token: 0x02000C04 RID: 3076
	public class WaterBubbleParticleVolumeCollector : MonoBehaviour
	{
		// Token: 0x06004CF7 RID: 19703 RVA: 0x00176478 File Offset: 0x00174678
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

		// Token: 0x06004CF8 RID: 19704 RVA: 0x00176618 File Offset: 0x00174818
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

		// Token: 0x06004CF9 RID: 19705 RVA: 0x00176658 File Offset: 0x00174858
		private void SetEmissionState(bool setEnabled)
		{
			float rateOverTimeMultiplier = setEnabled ? 1f : 0f;
			for (int i = 0; i < this.particleEmissionModules.Length; i++)
			{
				this.particleEmissionModules[i].rateOverTimeMultiplier = rateOverTimeMultiplier;
			}
			this.emissionEnabled = setEnabled;
		}

		// Token: 0x04004F42 RID: 20290
		public ParticleSystem[] particleSystems;

		// Token: 0x04004F43 RID: 20291
		private ParticleSystem.TriggerModule[] particleTriggerModules;

		// Token: 0x04004F44 RID: 20292
		private ParticleSystem.EmissionModule[] particleEmissionModules;

		// Token: 0x04004F45 RID: 20293
		private Collider[] bubbleableVolumeColliders;

		// Token: 0x04004F46 RID: 20294
		private bool emissionEnabled;
	}
}
