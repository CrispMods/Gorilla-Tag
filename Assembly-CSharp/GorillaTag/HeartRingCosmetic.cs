using System;
using GorillaExtensions;
using UnityEngine;

namespace GorillaTag
{
	// Token: 0x02000B75 RID: 2933
	[DefaultExecutionOrder(1250)]
	public class HeartRingCosmetic : MonoBehaviour
	{
		// Token: 0x06004A51 RID: 19025 RVA: 0x00168673 File Offset: 0x00166873
		protected void Awake()
		{
			Application.quitting += delegate()
			{
				base.enabled = false;
			};
		}

		// Token: 0x06004A52 RID: 19026 RVA: 0x00168688 File Offset: 0x00166888
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

		// Token: 0x06004A53 RID: 19027 RVA: 0x00168790 File Offset: 0x00166990
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

		// Token: 0x04004BDA RID: 19418
		public GameObject effects;

		// Token: 0x04004BDB RID: 19419
		[SerializeField]
		private bool isHauntedVoiceChanger;

		// Token: 0x04004BDC RID: 19420
		[SerializeField]
		private float hauntedVoicePitch = 0.75f;

		// Token: 0x04004BDD RID: 19421
		[AssignInCorePrefab]
		public float effectActivationRadius = 0.15f;

		// Token: 0x04004BDE RID: 19422
		private readonly Vector3 headToMouthOffset = new Vector3(0f, 0.0208f, 0.171f);

		// Token: 0x04004BDF RID: 19423
		private VRRig ownerRig;

		// Token: 0x04004BE0 RID: 19424
		private Transform ownerHead;

		// Token: 0x04004BE1 RID: 19425
		private ParticleSystem particleSystem;

		// Token: 0x04004BE2 RID: 19426
		private AudioSource audioSource;

		// Token: 0x04004BE3 RID: 19427
		private float maxEmissionRate;

		// Token: 0x04004BE4 RID: 19428
		private float maxVolume;

		// Token: 0x04004BE5 RID: 19429
		private const float emissionFadeTime = 0.1f;

		// Token: 0x04004BE6 RID: 19430
		private const float volumeFadeTime = 2f;
	}
}
