using System;
using GorillaTag.CosmeticSystem;
using UnityEngine;

namespace GorillaTag.Cosmetics
{
	// Token: 0x02000C2E RID: 3118
	public class SquirtingFlowerBadgeCosmetic : MonoBehaviour, ISpawnable, IFingerFlexListener
	{
		// Token: 0x1700080F RID: 2063
		// (get) Token: 0x06004DD4 RID: 19924 RVA: 0x0017DAC2 File Offset: 0x0017BCC2
		// (set) Token: 0x06004DD5 RID: 19925 RVA: 0x0017DACA File Offset: 0x0017BCCA
		public VRRig MyRig { get; private set; }

		// Token: 0x17000810 RID: 2064
		// (get) Token: 0x06004DD6 RID: 19926 RVA: 0x0017DAD3 File Offset: 0x0017BCD3
		// (set) Token: 0x06004DD7 RID: 19927 RVA: 0x0017DADB File Offset: 0x0017BCDB
		public bool IsSpawned { get; set; }

		// Token: 0x17000811 RID: 2065
		// (get) Token: 0x06004DD8 RID: 19928 RVA: 0x0017DAE4 File Offset: 0x0017BCE4
		// (set) Token: 0x06004DD9 RID: 19929 RVA: 0x0017DAEC File Offset: 0x0017BCEC
		public ECosmeticSelectSide CosmeticSelectedSide { get; set; }

		// Token: 0x06004DDA RID: 19930 RVA: 0x0017DAF5 File Offset: 0x0017BCF5
		public void OnSpawn(VRRig rig)
		{
			this.MyRig = rig;
		}

		// Token: 0x06004DDB RID: 19931 RVA: 0x000023F4 File Offset: 0x000005F4
		public void OnDespawn()
		{
		}

		// Token: 0x06004DDC RID: 19932 RVA: 0x0017DAFE File Offset: 0x0017BCFE
		private void Update()
		{
			if (!this.restartTimer && Time.time - this.triggeredTime >= this.coolDownTimer)
			{
				this.restartTimer = true;
			}
		}

		// Token: 0x06004DDD RID: 19933 RVA: 0x0017DB24 File Offset: 0x0017BD24
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

		// Token: 0x06004DDE RID: 19934 RVA: 0x0017DBA8 File Offset: 0x0017BDA8
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

		// Token: 0x06004DDF RID: 19935 RVA: 0x0017DBD2 File Offset: 0x0017BDD2
		public void OnButtonReleased(bool isLeftHand, float value)
		{
			if (!this.FingerFlexValidation(isLeftHand))
			{
				return;
			}
			this.buttonReleased = true;
		}

		// Token: 0x06004DE0 RID: 19936 RVA: 0x000023F4 File Offset: 0x000005F4
		public void OnButtonPressStayed(bool isLeftHand, float value)
		{
		}

		// Token: 0x06004DE1 RID: 19937 RVA: 0x0017DBE5 File Offset: 0x0017BDE5
		public bool FingerFlexValidation(bool isLeftHand)
		{
			return (!this.leftHand || isLeftHand) && (this.leftHand || !isLeftHand);
		}

		// Token: 0x040050DB RID: 20699
		[SerializeField]
		private ParticleSystem particlesToPlay;

		// Token: 0x040050DC RID: 20700
		[SerializeField]
		private GameObject objectToEnable;

		// Token: 0x040050DD RID: 20701
		[SerializeField]
		private AudioClip audioToPlay;

		// Token: 0x040050DE RID: 20702
		[SerializeField]
		private AudioSource audioSource;

		// Token: 0x040050DF RID: 20703
		[SerializeField]
		private float coolDownTimer = 2f;

		// Token: 0x040050E0 RID: 20704
		[SerializeField]
		private bool leftHand;

		// Token: 0x040050E1 RID: 20705
		private float triggeredTime;

		// Token: 0x040050E2 RID: 20706
		private bool restartTimer;

		// Token: 0x040050E3 RID: 20707
		private bool buttonReleased = true;
	}
}
