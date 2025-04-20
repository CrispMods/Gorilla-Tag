using System;
using GorillaTag.CosmeticSystem;
using UnityEngine;

namespace GorillaTag.Cosmetics
{
	// Token: 0x02000C5E RID: 3166
	public class SquirtingFlowerBadgeCosmetic : MonoBehaviour, ISpawnable, IFingerFlexListener
	{
		// Token: 0x1700082D RID: 2093
		// (get) Token: 0x06004F2C RID: 20268 RVA: 0x00063AE3 File Offset: 0x00061CE3
		// (set) Token: 0x06004F2D RID: 20269 RVA: 0x00063AEB File Offset: 0x00061CEB
		public VRRig MyRig { get; private set; }

		// Token: 0x1700082E RID: 2094
		// (get) Token: 0x06004F2E RID: 20270 RVA: 0x00063AF4 File Offset: 0x00061CF4
		// (set) Token: 0x06004F2F RID: 20271 RVA: 0x00063AFC File Offset: 0x00061CFC
		public bool IsSpawned { get; set; }

		// Token: 0x1700082F RID: 2095
		// (get) Token: 0x06004F30 RID: 20272 RVA: 0x00063B05 File Offset: 0x00061D05
		// (set) Token: 0x06004F31 RID: 20273 RVA: 0x00063B0D File Offset: 0x00061D0D
		public ECosmeticSelectSide CosmeticSelectedSide { get; set; }

		// Token: 0x06004F32 RID: 20274 RVA: 0x00063B16 File Offset: 0x00061D16
		public void OnSpawn(VRRig rig)
		{
			this.MyRig = rig;
		}

		// Token: 0x06004F33 RID: 20275 RVA: 0x00030607 File Offset: 0x0002E807
		public void OnDespawn()
		{
		}

		// Token: 0x06004F34 RID: 20276 RVA: 0x00063B1F File Offset: 0x00061D1F
		private void Update()
		{
			if (!this.restartTimer && Time.time - this.triggeredTime >= this.coolDownTimer)
			{
				this.restartTimer = true;
			}
		}

		// Token: 0x06004F35 RID: 20277 RVA: 0x001B6158 File Offset: 0x001B4358
		private void OnPlayEffectLocal()
		{
			if (this.particlesToPlay != null)
			{
				this.particlesToPlay.Play();
			}
			if (this.objectToEnable != null)
			{
				this.objectToEnable.SetActive(true);
			}
			if (this.audioSource != null && this.audioToPlay != null)
			{
				this.audioSource.GTPlayOneShot(this.audioToPlay, 1f);
			}
			this.restartTimer = false;
			this.triggeredTime = Time.time;
		}

		// Token: 0x06004F36 RID: 20278 RVA: 0x00063B44 File Offset: 0x00061D44
		public void OnButtonPressed(bool isLeftHand, float value)
		{
			if (!this.FingerFlexValidation(isLeftHand))
			{
				return;
			}
			if (!this.restartTimer || !this.buttonReleased)
			{
				return;
			}
			this.OnPlayEffectLocal();
			this.buttonReleased = false;
		}

		// Token: 0x06004F37 RID: 20279 RVA: 0x00063B6E File Offset: 0x00061D6E
		public void OnButtonReleased(bool isLeftHand, float value)
		{
			if (!this.FingerFlexValidation(isLeftHand))
			{
				return;
			}
			this.buttonReleased = true;
		}

		// Token: 0x06004F38 RID: 20280 RVA: 0x00030607 File Offset: 0x0002E807
		public void OnButtonPressStayed(bool isLeftHand, float value)
		{
		}

		// Token: 0x06004F39 RID: 20281 RVA: 0x00063B81 File Offset: 0x00061D81
		public bool FingerFlexValidation(bool isLeftHand)
		{
			return (!this.leftHand || isLeftHand) && (this.leftHand || !isLeftHand);
		}

		// Token: 0x040051E1 RID: 20961
		[SerializeField]
		private ParticleSystem particlesToPlay;

		// Token: 0x040051E2 RID: 20962
		[SerializeField]
		private GameObject objectToEnable;

		// Token: 0x040051E3 RID: 20963
		[SerializeField]
		private AudioClip audioToPlay;

		// Token: 0x040051E4 RID: 20964
		[SerializeField]
		private AudioSource audioSource;

		// Token: 0x040051E5 RID: 20965
		[SerializeField]
		private float coolDownTimer = 2f;

		// Token: 0x040051E6 RID: 20966
		[SerializeField]
		private bool leftHand;

		// Token: 0x040051E7 RID: 20967
		private float triggeredTime;

		// Token: 0x040051E8 RID: 20968
		private bool restartTimer;

		// Token: 0x040051E9 RID: 20969
		private bool buttonReleased = true;
	}
}
