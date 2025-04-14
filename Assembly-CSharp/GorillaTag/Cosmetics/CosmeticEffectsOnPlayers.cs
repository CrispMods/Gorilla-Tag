using System;
using GorillaExtensions;
using GorillaTag.CosmeticSystem;
using Photon.Pun;
using UnityEngine;
using UnityEngine.Serialization;

namespace GorillaTag.Cosmetics
{
	// Token: 0x02000C2A RID: 3114
	public class CosmeticEffectsOnPlayers : MonoBehaviour, ISpawnable
	{
		// Token: 0x06004DB2 RID: 19890 RVA: 0x0017CF0B File Offset: 0x0017B10B
		public void ApplyAllEffects()
		{
			this.ApplyAllEffectsByDistance(base.transform.position);
		}

		// Token: 0x06004DB3 RID: 19891 RVA: 0x0017CF1E File Offset: 0x0017B11E
		public void ApplyAllEffectsByDistance(Vector3 position)
		{
			this.ApplySkinByDistance(position);
			this.ApplyFPVEffectsByDistance(position);
		}

		// Token: 0x06004DB4 RID: 19892 RVA: 0x0017CF2E File Offset: 0x0017B12E
		public void ApplyAllEffectsForRig(VRRig rig)
		{
			this.ApplySkinForRig(rig);
			this.ApplyFPVEffectsForRig(rig);
		}

		// Token: 0x06004DB5 RID: 19893 RVA: 0x0017CF40 File Offset: 0x0017B140
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

		// Token: 0x06004DB6 RID: 19894 RVA: 0x0017D068 File Offset: 0x0017B268
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

		// Token: 0x06004DB7 RID: 19895 RVA: 0x0017D0D0 File Offset: 0x0017B2D0
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

		// Token: 0x06004DB8 RID: 19896 RVA: 0x0017D1E8 File Offset: 0x0017B3E8
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

		// Token: 0x1700080D RID: 2061
		// (get) Token: 0x06004DB9 RID: 19897 RVA: 0x0017D24E File Offset: 0x0017B44E
		// (set) Token: 0x06004DBA RID: 19898 RVA: 0x0017D256 File Offset: 0x0017B456
		public bool IsSpawned { get; set; }

		// Token: 0x1700080E RID: 2062
		// (get) Token: 0x06004DBB RID: 19899 RVA: 0x0017D25F File Offset: 0x0017B45F
		// (set) Token: 0x06004DBC RID: 19900 RVA: 0x0017D267 File Offset: 0x0017B467
		public ECosmeticSelectSide CosmeticSelectedSide { get; set; }

		// Token: 0x06004DBD RID: 19901 RVA: 0x0017D270 File Offset: 0x0017B470
		public void OnSpawn(VRRig rig)
		{
			this.myRig = rig;
		}

		// Token: 0x06004DBE RID: 19902 RVA: 0x000023F4 File Offset: 0x000005F4
		public void OnDespawn()
		{
		}

		// Token: 0x040050BE RID: 20670
		[SerializeField]
		private float effectDurationOnOthers;

		// Token: 0x040050BF RID: 20671
		[FormerlySerializedAs("effectDuration")]
		[SerializeField]
		private float effectDurationOnOwner;

		// Token: 0x040050C0 RID: 20672
		[SerializeField]
		private float effectDistanceRadius;

		// Token: 0x040050C1 RID: 20673
		[SerializeField]
		private GorillaSkin newSkin;

		// Token: 0x040050C2 RID: 20674
		[Tooltip("Spawn effects for the FPV - Use object pools")]
		[SerializeField]
		private GameObject firstPersonViewEffects;

		// Token: 0x040050C3 RID: 20675
		private VRRig myRig;

		// Token: 0x02000C2B RID: 3115
		public class CosmeticEffect
		{
			// Token: 0x040050C6 RID: 20678
			public float effectStartedTime;

			// Token: 0x040050C7 RID: 20679
			public float effectDuration;

			// Token: 0x040050C8 RID: 20680
			public GorillaSkin newSkin;

			// Token: 0x040050C9 RID: 20681
			public GameObject FPVEffect;
		}
	}
}
