﻿using System;
using GorillaTag.CosmeticSystem;
using UnityEngine;

namespace GorillaTag.Cosmetics
{
	// Token: 0x02000C31 RID: 3121
	public class SquirtingFlowerBadgeCosmetic : MonoBehaviour, ISpawnable, IFingerFlexListener
	{
		// Token: 0x17000810 RID: 2064
		// (get) Token: 0x06004DE0 RID: 19936 RVA: 0x000620F9 File Offset: 0x000602F9
		// (set) Token: 0x06004DE1 RID: 19937 RVA: 0x00062101 File Offset: 0x00060301
		public VRRig MyRig { get; private set; }

		// Token: 0x17000811 RID: 2065
		// (get) Token: 0x06004DE2 RID: 19938 RVA: 0x0006210A File Offset: 0x0006030A
		// (set) Token: 0x06004DE3 RID: 19939 RVA: 0x00062112 File Offset: 0x00060312
		public bool IsSpawned { get; set; }

		// Token: 0x17000812 RID: 2066
		// (get) Token: 0x06004DE4 RID: 19940 RVA: 0x0006211B File Offset: 0x0006031B
		// (set) Token: 0x06004DE5 RID: 19941 RVA: 0x00062123 File Offset: 0x00060323
		public ECosmeticSelectSide CosmeticSelectedSide { get; set; }

		// Token: 0x06004DE6 RID: 19942 RVA: 0x0006212C File Offset: 0x0006032C
		public void OnSpawn(VRRig rig)
		{
			this.MyRig = rig;
		}

		// Token: 0x06004DE7 RID: 19943 RVA: 0x0002F75F File Offset: 0x0002D95F
		public void OnDespawn()
		{
		}

		// Token: 0x06004DE8 RID: 19944 RVA: 0x00062135 File Offset: 0x00060335
		private void Update()
		{
			if (!this.restartTimer && Time.time - this.triggeredTime >= this.coolDownTimer)
			{
				this.restartTimer = true;
			}
		}

		// Token: 0x06004DE9 RID: 19945 RVA: 0x001AE5F0 File Offset: 0x001AC7F0
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

		// Token: 0x06004DEA RID: 19946 RVA: 0x0006215A File Offset: 0x0006035A
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

		// Token: 0x06004DEB RID: 19947 RVA: 0x00062184 File Offset: 0x00060384
		public void OnButtonReleased(bool isLeftHand, float value)
		{
			if (!this.FingerFlexValidation(isLeftHand))
			{
				return;
			}
			this.buttonReleased = true;
		}

		// Token: 0x06004DEC RID: 19948 RVA: 0x0002F75F File Offset: 0x0002D95F
		public void OnButtonPressStayed(bool isLeftHand, float value)
		{
		}

		// Token: 0x06004DED RID: 19949 RVA: 0x00062197 File Offset: 0x00060397
		public bool FingerFlexValidation(bool isLeftHand)
		{
			return (!this.leftHand || isLeftHand) && (this.leftHand || !isLeftHand);
		}

		// Token: 0x040050ED RID: 20717
		[SerializeField]
		private ParticleSystem particlesToPlay;

		// Token: 0x040050EE RID: 20718
		[SerializeField]
		private GameObject objectToEnable;

		// Token: 0x040050EF RID: 20719
		[SerializeField]
		private AudioClip audioToPlay;

		// Token: 0x040050F0 RID: 20720
		[SerializeField]
		private AudioSource audioSource;

		// Token: 0x040050F1 RID: 20721
		[SerializeField]
		private float coolDownTimer = 2f;

		// Token: 0x040050F2 RID: 20722
		[SerializeField]
		private bool leftHand;

		// Token: 0x040050F3 RID: 20723
		private float triggeredTime;

		// Token: 0x040050F4 RID: 20724
		private bool restartTimer;

		// Token: 0x040050F5 RID: 20725
		private bool buttonReleased = true;
	}
}
