﻿using System;
using GorillaExtensions;
using GorillaTag.CosmeticSystem;
using Photon.Pun;
using UnityEngine;
using UnityEngine.Serialization;

namespace GorillaTag.Cosmetics
{
	// Token: 0x02000C2D RID: 3117
	public class CosmeticEffectsOnPlayers : MonoBehaviour, ISpawnable
	{
		// Token: 0x06004DBE RID: 19902 RVA: 0x00061FC5 File Offset: 0x000601C5
		public void ApplyAllEffects()
		{
			this.ApplyAllEffectsByDistance(base.transform.position);
		}

		// Token: 0x06004DBF RID: 19903 RVA: 0x00061FD8 File Offset: 0x000601D8
		public void ApplyAllEffectsByDistance(Vector3 position)
		{
			this.ApplySkinByDistance(position);
			this.ApplyFPVEffectsByDistance(position);
		}

		// Token: 0x06004DC0 RID: 19904 RVA: 0x00061FE8 File Offset: 0x000601E8
		public void ApplyAllEffectsForRig(VRRig rig)
		{
			this.ApplySkinForRig(rig);
			this.ApplyFPVEffectsForRig(rig);
		}

		// Token: 0x06004DC1 RID: 19905 RVA: 0x001ADB70 File Offset: 0x001ABD70
		private void ApplyFPVEffectsByDistance(Vector3 position)
		{
			if (this.firstPersonViewEffects == null)
			{
				return;
			}
			CosmeticEffectsOnPlayers.CosmeticEffect cosmeticEffect = new CosmeticEffectsOnPlayers.CosmeticEffect
			{
				effectDuration = this.effectDurationOnOthers,
				effectStartedTime = Time.time,
				FPVEffect = this.firstPersonViewEffects
			};
			if (!PhotonNetwork.InRoom)
			{
				if ((GorillaTagger.Instance.offlineVRRig.transform.position - position).IsShorterThan(this.effectDistanceRadius))
				{
					cosmeticEffect.effectDuration = this.effectDurationOnOwner;
					GorillaTagger.Instance.offlineVRRig.SpawnFPVEffects(cosmeticEffect, true);
					return;
				}
			}
			else
			{
				foreach (VRRig vrrig in GorillaParent.instance.vrrigs)
				{
					if (this.myRig == vrrig)
					{
						cosmeticEffect.effectDuration = this.effectDurationOnOwner;
					}
					if ((vrrig.transform.position - position).IsShorterThan(this.effectDistanceRadius) && this.firstPersonViewEffects)
					{
						vrrig.SpawnFPVEffects(cosmeticEffect, true);
					}
				}
			}
		}

		// Token: 0x06004DC2 RID: 19906 RVA: 0x001ADC98 File Offset: 0x001ABE98
		private void ApplyFPVEffectsForRig(VRRig rig)
		{
			if (this.firstPersonViewEffects == null)
			{
				return;
			}
			CosmeticEffectsOnPlayers.CosmeticEffect cosmeticEffect = new CosmeticEffectsOnPlayers.CosmeticEffect
			{
				effectDuration = this.effectDurationOnOthers,
				effectStartedTime = Time.time,
				FPVEffect = this.firstPersonViewEffects
			};
			if (rig == this.myRig)
			{
				cosmeticEffect.effectDuration = this.effectDurationOnOwner;
			}
			rig.SpawnFPVEffects(cosmeticEffect, true);
		}

		// Token: 0x06004DC3 RID: 19907 RVA: 0x001ADD00 File Offset: 0x001ABF00
		private void ApplySkinByDistance(Vector3 position)
		{
			if (this.newSkin == null)
			{
				return;
			}
			CosmeticEffectsOnPlayers.CosmeticEffect cosmeticEffect = new CosmeticEffectsOnPlayers.CosmeticEffect
			{
				effectDuration = this.effectDurationOnOthers,
				effectStartedTime = Time.time,
				newSkin = this.newSkin
			};
			if (!PhotonNetwork.InRoom)
			{
				if ((GorillaTagger.Instance.offlineVRRig.transform.position - position).IsShorterThan(this.effectDistanceRadius))
				{
					cosmeticEffect.effectDuration = this.effectDurationOnOwner;
					GorillaTagger.Instance.offlineVRRig.SpawnSkinEffects(cosmeticEffect);
					return;
				}
			}
			else
			{
				foreach (VRRig vrrig in GorillaParent.instance.vrrigs)
				{
					if (this.myRig == vrrig)
					{
						cosmeticEffect.effectDuration = this.effectDurationOnOwner;
					}
					if ((vrrig.transform.position - position).IsShorterThan(this.effectDistanceRadius))
					{
						vrrig.SpawnSkinEffects(cosmeticEffect);
					}
				}
			}
		}

		// Token: 0x06004DC4 RID: 19908 RVA: 0x001ADE18 File Offset: 0x001AC018
		private void ApplySkinForRig(VRRig vrRig)
		{
			if (this.newSkin == null)
			{
				return;
			}
			CosmeticEffectsOnPlayers.CosmeticEffect cosmeticEffect = new CosmeticEffectsOnPlayers.CosmeticEffect
			{
				effectDuration = this.effectDurationOnOthers,
				effectStartedTime = Time.time,
				newSkin = this.newSkin
			};
			if (vrRig == this.myRig)
			{
				cosmeticEffect.effectDuration = this.effectDurationOnOwner;
			}
			vrRig.SpawnSkinEffects(cosmeticEffect);
		}

		// Token: 0x1700080E RID: 2062
		// (get) Token: 0x06004DC5 RID: 19909 RVA: 0x00061FF8 File Offset: 0x000601F8
		// (set) Token: 0x06004DC6 RID: 19910 RVA: 0x00062000 File Offset: 0x00060200
		public bool IsSpawned { get; set; }

		// Token: 0x1700080F RID: 2063
		// (get) Token: 0x06004DC7 RID: 19911 RVA: 0x00062009 File Offset: 0x00060209
		// (set) Token: 0x06004DC8 RID: 19912 RVA: 0x00062011 File Offset: 0x00060211
		public ECosmeticSelectSide CosmeticSelectedSide { get; set; }

		// Token: 0x06004DC9 RID: 19913 RVA: 0x0006201A File Offset: 0x0006021A
		public void OnSpawn(VRRig rig)
		{
			this.myRig = rig;
		}

		// Token: 0x06004DCA RID: 19914 RVA: 0x0002F75F File Offset: 0x0002D95F
		public void OnDespawn()
		{
		}

		// Token: 0x040050D0 RID: 20688
		[SerializeField]
		private float effectDurationOnOthers;

		// Token: 0x040050D1 RID: 20689
		[FormerlySerializedAs("effectDuration")]
		[SerializeField]
		private float effectDurationOnOwner;

		// Token: 0x040050D2 RID: 20690
		[SerializeField]
		private float effectDistanceRadius;

		// Token: 0x040050D3 RID: 20691
		[SerializeField]
		private GorillaSkin newSkin;

		// Token: 0x040050D4 RID: 20692
		[Tooltip("Spawn effects for the FPV - Use object pools")]
		[SerializeField]
		private GameObject firstPersonViewEffects;

		// Token: 0x040050D5 RID: 20693
		private VRRig myRig;

		// Token: 0x02000C2E RID: 3118
		public class CosmeticEffect
		{
			// Token: 0x040050D8 RID: 20696
			public float effectStartedTime;

			// Token: 0x040050D9 RID: 20697
			public float effectDuration;

			// Token: 0x040050DA RID: 20698
			public GorillaSkin newSkin;

			// Token: 0x040050DB RID: 20699
			public GameObject FPVEffect;
		}
	}
}
