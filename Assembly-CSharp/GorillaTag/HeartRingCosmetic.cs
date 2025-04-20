using System;
using GorillaExtensions;
using UnityEngine;

namespace GorillaTag
{
	// Token: 0x02000BA2 RID: 2978
	[DefaultExecutionOrder(1250)]
	public class HeartRingCosmetic : MonoBehaviour
	{
		// Token: 0x06004B9C RID: 19356 RVA: 0x00061B85 File Offset: 0x0005FD85
		protected void Awake()
		{
			Application.quitting += delegate()
			{
				base.enabled = false;
			};
		}

		// Token: 0x06004B9D RID: 19357 RVA: 0x001A2364 File Offset: 0x001A0564
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

		// Token: 0x06004B9E RID: 19358 RVA: 0x001A246C File Offset: 0x001A066C
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

		// Token: 0x04004CD0 RID: 19664
		public GameObject effects;

		// Token: 0x04004CD1 RID: 19665
		[SerializeField]
		private bool isHauntedVoiceChanger;

		// Token: 0x04004CD2 RID: 19666
		[SerializeField]
		private float hauntedVoicePitch = 0.75f;

		// Token: 0x04004CD3 RID: 19667
		[AssignInCorePrefab]
		public float effectActivationRadius = 0.15f;

		// Token: 0x04004CD4 RID: 19668
		private readonly Vector3 headToMouthOffset = new Vector3(0f, 0.0208f, 0.171f);

		// Token: 0x04004CD5 RID: 19669
		private VRRig ownerRig;

		// Token: 0x04004CD6 RID: 19670
		private Transform ownerHead;

		// Token: 0x04004CD7 RID: 19671
		private ParticleSystem particleSystem;

		// Token: 0x04004CD8 RID: 19672
		private AudioSource audioSource;

		// Token: 0x04004CD9 RID: 19673
		private float maxEmissionRate;

		// Token: 0x04004CDA RID: 19674
		private float maxVolume;

		// Token: 0x04004CDB RID: 19675
		private const float emissionFadeTime = 0.1f;

		// Token: 0x04004CDC RID: 19676
		private const float volumeFadeTime = 2f;
	}
}
