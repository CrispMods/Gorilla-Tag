using System;
using GorillaExtensions;
using UnityEngine;

namespace TagEffects
{
	// Token: 0x02000B2A RID: 2858
	public class HandEffectsTrigger : MonoBehaviour, IHandEffectsTrigger
	{
		// Token: 0x17000751 RID: 1873
		// (get) Token: 0x06004735 RID: 18229 RVA: 0x001532BF File Offset: 0x001514BF
		public bool Static
		{
			get
			{
				return this.isStatic;
			}
		}

		// Token: 0x17000752 RID: 1874
		// (get) Token: 0x06004736 RID: 18230 RVA: 0x001532C8 File Offset: 0x001514C8
		public bool FingersDown
		{
			get
			{
				return !(this.rig == null) && ((this.rightHand && this.rig.IsMakingFistRight()) || (!this.rightHand && this.rig.IsMakingFistLeft()));
			}
		}

		// Token: 0x17000753 RID: 1875
		// (get) Token: 0x06004737 RID: 18231 RVA: 0x00153314 File Offset: 0x00151514
		public bool FingersUp
		{
			get
			{
				return !(this.rig == null) && ((this.rightHand && this.rig.IsMakingFiveRight()) || (!this.rightHand && this.rig.IsMakingFiveLeft()));
			}
		}

		// Token: 0x17000754 RID: 1876
		// (get) Token: 0x06004738 RID: 18232 RVA: 0x00153360 File Offset: 0x00151560
		public Vector3 Velocity
		{
			get
			{
				if (this.velocityEstimator != null && this.rig != null && this.rig.scaleFactor > 0.001f)
				{
					return this.velocityEstimator.linearVelocity / this.rig.scaleFactor;
				}
				return Vector3.zero;
			}
		}

		// Token: 0x17000755 RID: 1877
		// (get) Token: 0x06004739 RID: 18233 RVA: 0x001533BC File Offset: 0x001515BC
		bool IHandEffectsTrigger.RightHand
		{
			get
			{
				return this.rightHand;
			}
		}

		// Token: 0x17000756 RID: 1878
		// (get) Token: 0x0600473A RID: 18234 RVA: 0x001533C4 File Offset: 0x001515C4
		public IHandEffectsTrigger.Mode EffectMode { get; }

		// Token: 0x17000757 RID: 1879
		// (get) Token: 0x0600473B RID: 18235 RVA: 0x00042E29 File Offset: 0x00041029
		public Transform Transform
		{
			get
			{
				return base.transform;
			}
		}

		// Token: 0x17000758 RID: 1880
		// (get) Token: 0x0600473C RID: 18236 RVA: 0x001533CC File Offset: 0x001515CC
		public VRRig Rig
		{
			get
			{
				return this.rig;
			}
		}

		// Token: 0x17000759 RID: 1881
		// (get) Token: 0x0600473D RID: 18237 RVA: 0x001533D4 File Offset: 0x001515D4
		public TagEffectPack CosmeticEffectPack
		{
			get
			{
				if (this.rig == null)
				{
					return null;
				}
				return this.rig.CosmeticEffectPack;
			}
		}

		// Token: 0x0600473E RID: 18238 RVA: 0x001533F4 File Offset: 0x001515F4
		private void Awake()
		{
			this.rig = base.GetComponentInParent<VRRig>();
			if (this.velocityEstimator == null)
			{
				this.velocityEstimator = base.GetComponentInParent<GorillaVelocityEstimator>();
			}
			for (int i = 0; i < this.debugVisuals.Length; i++)
			{
				this.debugVisuals[i].SetActive(TagEffectsLibrary.DebugMode);
			}
		}

		// Token: 0x0600473F RID: 18239 RVA: 0x00042E7F File Offset: 0x0004107F
		private void OnEnable()
		{
			if (!HandEffectsTriggerRegistry.HasInstance)
			{
				HandEffectsTriggerRegistry.FindInstance();
			}
			HandEffectsTriggerRegistry.Instance.Register(this);
		}

		// Token: 0x06004740 RID: 18240 RVA: 0x00042E98 File Offset: 0x00041098
		private void OnDisable()
		{
			HandEffectsTriggerRegistry.Instance.Unregister(this);
		}

		// Token: 0x06004741 RID: 18241 RVA: 0x0015344C File Offset: 0x0015164C
		public void OnTriggerEntered(IHandEffectsTrigger other)
		{
			if (this.rig == other.Rig)
			{
				return;
			}
			if (this.FingersDown && other.FingersDown && (other.Static || (Vector3.Dot(Vector3.Dot(this.Velocity, base.transform.up) * base.transform.up - Vector3.Dot(other.Velocity, other.Transform.up) * other.Transform.up, -other.Transform.up) > TagEffectsLibrary.FistBumpSpeedThreshold && Vector3.Dot(base.transform.up, other.Transform.up) < -0.01f)))
			{
				this.PlayHandEffects(TagEffectsLibrary.EffectType.FIST_BUMP, other);
			}
			if (this.FingersUp && other.FingersUp && (other.Static || Mathf.Abs(Vector3.Dot(Vector3.Dot(this.Velocity, base.transform.right) * base.transform.right - Vector3.Dot(other.Velocity, other.Transform.right) * other.Transform.right, other.Transform.right)) > TagEffectsLibrary.HighFiveSpeedThreshold))
			{
				this.PlayHandEffects(TagEffectsLibrary.EffectType.HIGH_FIVE, other);
			}
		}

		// Token: 0x06004742 RID: 18242 RVA: 0x001535B8 File Offset: 0x001517B8
		private void PlayHandEffects(TagEffectsLibrary.EffectType effectType, IHandEffectsTrigger other)
		{
			bool flag = false;
			if (this.rig.isOfflineVRRig)
			{
				PlayerGameEvents.TriggerHandEffect(effectType.ToString());
			}
			HandEffectsOverrideCosmetic handEffectsOverrideCosmetic = null;
			HandEffectsOverrideCosmetic handEffectsOverrideCosmetic2 = null;
			foreach (HandEffectsOverrideCosmetic handEffectsOverrideCosmetic3 in (this.rightHand ? this.rig.CosmeticHandEffectsOverride_Right : this.rig.CosmeticHandEffectsOverride_Left))
			{
				if (handEffectsOverrideCosmetic3.handEffectType == this.MapEnum(effectType))
				{
					handEffectsOverrideCosmetic2 = handEffectsOverrideCosmetic3;
					break;
				}
			}
			if (this.rig != null && this.rig.isOfflineVRRig && GorillaTagger.Instance != null)
			{
				if (other.Rig)
				{
					foreach (HandEffectsOverrideCosmetic handEffectsOverrideCosmetic4 in ((other.Rig.CosmeticHandEffectsOverride_Right != null) ? other.Rig.CosmeticHandEffectsOverride_Right : other.Rig.CosmeticHandEffectsOverride_Left))
					{
						if (handEffectsOverrideCosmetic4.handEffectType == this.MapEnum(effectType))
						{
							handEffectsOverrideCosmetic = handEffectsOverrideCosmetic4;
							break;
						}
					}
					if (handEffectsOverrideCosmetic && handEffectsOverrideCosmetic.handEffectType == this.MapEnum(effectType) && ((!handEffectsOverrideCosmetic.isLeftHand && other.RightHand) || (handEffectsOverrideCosmetic.isLeftHand && !other.RightHand)))
					{
						if (handEffectsOverrideCosmetic.thirdPerson.playHaptics)
						{
							GorillaTagger.Instance.StartVibration(!this.rightHand, handEffectsOverrideCosmetic.thirdPerson.hapticStrength, handEffectsOverrideCosmetic.thirdPerson.hapticDuration);
						}
						TagEffectsLibrary.placeEffects(handEffectsOverrideCosmetic.thirdPerson.effectVFX, base.transform, this.rig.scaleFactor, false, handEffectsOverrideCosmetic.thirdPerson.parentEffect, base.transform.rotation);
						flag = true;
					}
				}
				if (handEffectsOverrideCosmetic2 && handEffectsOverrideCosmetic2.handEffectType == this.MapEnum(effectType) && ((handEffectsOverrideCosmetic2.isLeftHand && !this.rightHand) || (!handEffectsOverrideCosmetic2.isLeftHand && this.rightHand)))
				{
					if (handEffectsOverrideCosmetic2.firstPerson.playHaptics)
					{
						GorillaTagger.Instance.StartVibration(!this.rightHand, handEffectsOverrideCosmetic2.firstPerson.hapticStrength, handEffectsOverrideCosmetic2.firstPerson.hapticDuration);
					}
					TagEffectsLibrary.placeEffects(handEffectsOverrideCosmetic2.firstPerson.effectVFX, other.Transform, this.rig.scaleFactor, false, handEffectsOverrideCosmetic2.firstPerson.parentEffect, other.Transform.rotation);
					flag = true;
				}
				if (!flag)
				{
					GorillaTagger.Instance.StartVibration(!this.rightHand, GorillaTagger.Instance.tapHapticStrength, GorillaTagger.Instance.tapHapticDuration);
					TagEffectsLibrary.PlayEffect(base.transform, !this.rightHand, this.rig.scaleFactor, effectType, this.CosmeticEffectPack, other.CosmeticEffectPack, base.transform.rotation);
				}
			}
		}

		// Token: 0x06004743 RID: 18243 RVA: 0x001538D4 File Offset: 0x00151AD4
		public bool InTriggerZone(IHandEffectsTrigger t)
		{
			return (base.transform.position - t.Transform.position).IsShorterThan(this.triggerRadius * this.rig.scaleFactor);
		}

		// Token: 0x06004744 RID: 18244 RVA: 0x00153908 File Offset: 0x00151B08
		private HandEffectsOverrideCosmetic.HandEffectType MapEnum(TagEffectsLibrary.EffectType oldEnum)
		{
			return HandEffectsTrigger.mappingArray[(int)oldEnum];
		}

		// Token: 0x040048DA RID: 18650
		[SerializeField]
		private float triggerRadius = 0.07f;

		// Token: 0x040048DB RID: 18651
		[SerializeField]
		private bool rightHand;

		// Token: 0x040048DC RID: 18652
		[SerializeField]
		private bool isStatic;

		// Token: 0x040048DD RID: 18653
		private VRRig rig;

		// Token: 0x040048DE RID: 18654
		public GorillaVelocityEstimator velocityEstimator;

		// Token: 0x040048DF RID: 18655
		[SerializeField]
		private GameObject[] debugVisuals;

		// Token: 0x040048E1 RID: 18657
		private static HandEffectsOverrideCosmetic.HandEffectType[] mappingArray = new HandEffectsOverrideCosmetic.HandEffectType[]
		{
			HandEffectsOverrideCosmetic.HandEffectType.None,
			HandEffectsOverrideCosmetic.HandEffectType.None,
			HandEffectsOverrideCosmetic.HandEffectType.HighFive,
			HandEffectsOverrideCosmetic.HandEffectType.FistBump
		};
	}
}
