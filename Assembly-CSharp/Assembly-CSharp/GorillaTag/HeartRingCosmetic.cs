using System;
using GorillaExtensions;
using UnityEngine;

namespace GorillaTag
{
	// Token: 0x02000B78 RID: 2936
	[DefaultExecutionOrder(1250)]
	public class HeartRingCosmetic : MonoBehaviour
	{
		// Token: 0x06004A5D RID: 19037 RVA: 0x00168C3B File Offset: 0x00166E3B
		protected void Awake()
		{
			Application.quitting += delegate()
			{
				base.enabled = false;
			};
		}

		// Token: 0x06004A5E RID: 19038 RVA: 0x00168C50 File Offset: 0x00166E50
		protected void OnEnable()
		{
			this.particleSystem = this.effects.GetComponentInChildren<ParticleSystem>(true);
			this.audioSource = this.effects.GetComponentInChildren<AudioSource>(true);
			this.ownerRig = base.GetComponentInParent<VRRig>();
			bool flag = this.ownerRig != null && this.ownerRig.head != null && this.ownerRig.head.rigTarget != null;
			base.enabled = flag;
			this.effects.SetActive(flag);
			if (!flag)
			{
				Debug.LogError("Disabling HeartRingCosmetic. Could not find owner head. Scene path: " + base.transform.GetPath(), this);
				return;
			}
			this.ownerHead = ((this.ownerRig != null) ? this.ownerRig.head.rigTarget.transform : base.transform);
			this.maxEmissionRate = this.particleSystem.emission.rateOverTime.constant;
			this.maxVolume = this.audioSource.volume;
		}

		// Token: 0x06004A5F RID: 19039 RVA: 0x00168D58 File Offset: 0x00166F58
		protected void LateUpdate()
		{
			Transform transform = base.transform;
			Vector3 position = transform.position;
			float x = transform.lossyScale.x;
			float num = this.effectActivationRadius * this.effectActivationRadius * x * x;
			bool flag = (this.ownerHead.TransformPoint(this.headToMouthOffset) - position).sqrMagnitude < num;
			ParticleSystem.EmissionModule emission = this.particleSystem.emission;
			emission.rateOverTime = Mathf.Lerp(emission.rateOverTime.constant, flag ? this.maxEmissionRate : 0f, Time.deltaTime / 0.1f);
			this.audioSource.volume = Mathf.Lerp(this.audioSource.volume, flag ? this.maxVolume : 0f, Time.deltaTime / 2f);
			this.ownerRig.UsingHauntedRing = (this.isHauntedVoiceChanger && flag);
			if (this.ownerRig.UsingHauntedRing)
			{
				this.ownerRig.HauntedRingVoicePitch = this.hauntedVoicePitch;
			}
		}

		// Token: 0x04004BEC RID: 19436
		public GameObject effects;

		// Token: 0x04004BED RID: 19437
		[SerializeField]
		private bool isHauntedVoiceChanger;

		// Token: 0x04004BEE RID: 19438
		[SerializeField]
		private float hauntedVoicePitch = 0.75f;

		// Token: 0x04004BEF RID: 19439
		[AssignInCorePrefab]
		public float effectActivationRadius = 0.15f;

		// Token: 0x04004BF0 RID: 19440
		private readonly Vector3 headToMouthOffset = new Vector3(0f, 0.0208f, 0.171f);

		// Token: 0x04004BF1 RID: 19441
		private VRRig ownerRig;

		// Token: 0x04004BF2 RID: 19442
		private Transform ownerHead;

		// Token: 0x04004BF3 RID: 19443
		private ParticleSystem particleSystem;

		// Token: 0x04004BF4 RID: 19444
		private AudioSource audioSource;

		// Token: 0x04004BF5 RID: 19445
		private float maxEmissionRate;

		// Token: 0x04004BF6 RID: 19446
		private float maxVolume;

		// Token: 0x04004BF7 RID: 19447
		private const float emissionFadeTime = 0.1f;

		// Token: 0x04004BF8 RID: 19448
		private const float volumeFadeTime = 2f;
	}
}
