using System;
using GorillaExtensions;
using GorillaTag.CosmeticSystem;
using Photon.Pun;
using UnityEngine;
using UnityEngine.Serialization;

namespace GorillaTag.Cosmetics
{
	// Token: 0x02000C58 RID: 3160
	public class CosmeticEffectsOnPlayers : MonoBehaviour, ISpawnable
	{
		// Token: 0x06004F03 RID: 20227 RVA: 0x00063986 File Offset: 0x00061B86
		public void ApplyAllEffects()
		{
			this.ApplyAllEffectsByDistance(base.transform.position);
		}

		// Token: 0x06004F04 RID: 20228 RVA: 0x00063999 File Offset: 0x00061B99
		public void ApplyAllEffectsByDistance(Vector3 position)
		{
			this.ApplySkinByDistance(position);
			this.ApplyFPVEffectsByDistance(position);
		}

		// Token: 0x06004F05 RID: 20229 RVA: 0x000639A9 File Offset: 0x00061BA9
		public void ApplyAllEffectsForRig(VRRig rig)
		{
			this.ApplySkinForRig(rig);
			this.ApplyFPVEffectsForRig(rig);
		}

		// Token: 0x06004F06 RID: 20230 RVA: 0x001B545C File Offset: 0x001B365C
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

		// Token: 0x06004F07 RID: 20231 RVA: 0x001B5584 File Offset: 0x001B3784
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

		// Token: 0x06004F08 RID: 20232 RVA: 0x001B55EC File Offset: 0x001B37EC
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

		// Token: 0x06004F09 RID: 20233 RVA: 0x001B5704 File Offset: 0x001B3904
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

		// Token: 0x1700082B RID: 2091
		// (get) Token: 0x06004F0A RID: 20234 RVA: 0x000639B9 File Offset: 0x00061BB9
		// (set) Token: 0x06004F0B RID: 20235 RVA: 0x000639C1 File Offset: 0x00061BC1
		public bool IsSpawned { get; set; }

		// Token: 0x1700082C RID: 2092
		// (get) Token: 0x06004F0C RID: 20236 RVA: 0x000639CA File Offset: 0x00061BCA
		// (set) Token: 0x06004F0D RID: 20237 RVA: 0x000639D2 File Offset: 0x00061BD2
		public ECosmeticSelectSide CosmeticSelectedSide { get; set; }

		// Token: 0x06004F0E RID: 20238 RVA: 0x000639DB File Offset: 0x00061BDB
		public void OnSpawn(VRRig rig)
		{
			this.myRig = rig;
		}

		// Token: 0x06004F0F RID: 20239 RVA: 0x00030607 File Offset: 0x0002E807
		public void OnDespawn()
		{
		}

		// Token: 0x040051B4 RID: 20916
		[SerializeField]
		private float effectDurationOnOthers;

		// Token: 0x040051B5 RID: 20917
		[FormerlySerializedAs("effectDuration")]
		[SerializeField]
		private float effectDurationOnOwner;

		// Token: 0x040051B6 RID: 20918
		[SerializeField]
		private float effectDistanceRadius;

		// Token: 0x040051B7 RID: 20919
		[SerializeField]
		private GorillaSkin newSkin;

		// Token: 0x040051B8 RID: 20920
		[Tooltip("Spawn effects for the FPV - Use object pools")]
		[SerializeField]
		private GameObject firstPersonViewEffects;

		// Token: 0x040051B9 RID: 20921
		private VRRig myRig;

		// Token: 0x02000C59 RID: 3161
		public class CosmeticEffect
		{
			// Token: 0x040051BC RID: 20924
			public float effectStartedTime;

			// Token: 0x040051BD RID: 20925
			public float effectDuration;

			// Token: 0x040051BE RID: 20926
			public GorillaSkin newSkin;

			// Token: 0x040051BF RID: 20927
			public GameObject FPVEffect;
		}
	}
}
