using System;
using GorillaExtensions;
using UnityEngine;

namespace TagEffects
{
	// Token: 0x02000B57 RID: 2903
	public class HandEffectsTrigger : MonoBehaviour, IHandEffectsTrigger
	{
		// Token: 0x1700076D RID: 1901
		// (get) Token: 0x0600487E RID: 18558 RVA: 0x0005F352 File Offset: 0x0005D552
		public bool Static
		{
			get
			{
				return this.isStatic;
			}
		}

		// Token: 0x1700076E RID: 1902
		// (get) Token: 0x0600487F RID: 18559 RVA: 0x0018F770 File Offset: 0x0018D970
		public bool FingersDown
		{
			get
			{
				return !(this.rig == null) && ((this.rightHand && this.rig.IsMakingFistRight()) || (!this.rightHand && this.rig.IsMakingFistLeft()));
			}
		}

		// Token: 0x1700076F RID: 1903
		// (get) Token: 0x06004880 RID: 18560 RVA: 0x0018F7BC File Offset: 0x0018D9BC
		public bool FingersUp
		{
			get
			{
				return !(this.rig == null) && ((this.rightHand && this.rig.IsMakingFiveRight()) || (!this.rightHand && this.rig.IsMakingFiveLeft()));
			}
		}

		// Token: 0x17000770 RID: 1904
		// (get) Token: 0x06004881 RID: 18561 RVA: 0x0018F808 File Offset: 0x0018DA08
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

		// Token: 0x17000771 RID: 1905
		// (get) Token: 0x06004882 RID: 18562 RVA: 0x0005F35A File Offset: 0x0005D55A
		bool IHandEffectsTrigger.RightHand
		{
			get
			{
				return this.rightHand;
			}
		}

		// Token: 0x17000772 RID: 1906
		// (get) Token: 0x06004883 RID: 18563 RVA: 0x0005F362 File Offset: 0x0005D562
		public IHandEffectsTrigger.Mode EffectMode { get; }

		// Token: 0x17000773 RID: 1907
		// (get) Token: 0x06004884 RID: 18564 RVA: 0x00039243 File Offset: 0x00037443
		public Transform Transform
		{
			get
			{
				return base.transform;
			}
		}

		// Token: 0x17000774 RID: 1908
		// (get) Token: 0x06004885 RID: 18565 RVA: 0x0005F36A File Offset: 0x0005D56A
		public VRRig Rig
		{
			get
			{
				return this.rig;
			}
		}

		// Token: 0x17000775 RID: 1909
		// (get) Token: 0x06004886 RID: 18566 RVA: 0x0005F372 File Offset: 0x0005D572
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

		// Token: 0x06004887 RID: 18567 RVA: 0x0018F864 File Offset: 0x0018DA64
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

		// Token: 0x06004888 RID: 18568 RVA: 0x00039299 File Offset: 0x00037499
		private void OnEnable()
		{
			if (!HandEffectsTriggerRegistry.HasInstance)
			{
				HandEffectsTriggerRegistry.FindInstance();
			}
			HandEffectsTriggerRegistry.Instance.Register(this);
		}

		// Token: 0x06004889 RID: 18569 RVA: 0x000392B2 File Offset: 0x000374B2
		private void OnDisable()
		{
			HandEffectsTriggerRegistry.Instance.Unregister(this);
		}

		// Token: 0x0600488A RID: 18570 RVA: 0x0018F8BC File Offset: 0x0018DABC
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

		// Token: 0x0600488B RID: 18571 RVA: 0x0018FA28 File Offset: 0x0018DC28
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

		// Token: 0x0600488C RID: 18572 RVA: 0x0005F38F File Offset: 0x0005D58F
		public bool InTriggerZone(IHandEffectsTrigger t)
		{
			return (base.transform.position - t.Transform.position).IsShorterThan(this.triggerRadius * this.rig.scaleFactor);
		}

		// Token: 0x0600488D RID: 18573 RVA: 0x0005F3C3 File Offset: 0x0005D5C3
		private HandEffectsOverrideCosmetic.HandEffectType MapEnum(TagEffectsLibrary.EffectType oldEnum)
		{
			return HandEffectsTrigger.mappingArray[(int)oldEnum];
		}

		// Token: 0x040049CF RID: 18895
		[SerializeField]
		private float triggerRadius = 0.07f;

		// Token: 0x040049D0 RID: 18896
		[SerializeField]
		private bool rightHand;

		// Token: 0x040049D1 RID: 18897
		[SerializeField]
		private bool isStatic;

		// Token: 0x040049D2 RID: 18898
		private VRRig rig;

		// Token: 0x040049D3 RID: 18899
		public GorillaVelocityEstimator velocityEstimator;

		// Token: 0x040049D4 RID: 18900
		[SerializeField]
		private GameObject[] debugVisuals;

		// Token: 0x040049D6 RID: 18902
		private static HandEffectsOverrideCosmetic.HandEffectType[] mappingArray = new HandEffectsOverrideCosmetic.HandEffectType[]
		{
			HandEffectsOverrideCosmetic.HandEffectType.None,
			HandEffectsOverrideCosmetic.HandEffectType.None,
			HandEffectsOverrideCosmetic.HandEffectType.HighFive,
			HandEffectsOverrideCosmetic.HandEffectType.FistBump
		};
	}
}
