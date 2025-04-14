using System;
using GorillaTag;
using GorillaTag.CosmeticSystem;
using UnityEngine;

// Token: 0x0200043C RID: 1084
public class HypnoRing : MonoBehaviour, ISpawnable
{
	// Token: 0x170002E7 RID: 743
	// (get) Token: 0x06001AA9 RID: 6825 RVA: 0x00083489 File Offset: 0x00081689
	// (set) Token: 0x06001AAA RID: 6826 RVA: 0x00083491 File Offset: 0x00081691
	bool ISpawnable.IsSpawned { get; set; }

	// Token: 0x170002E8 RID: 744
	// (get) Token: 0x06001AAB RID: 6827 RVA: 0x0008349A File Offset: 0x0008169A
	// (set) Token: 0x06001AAC RID: 6828 RVA: 0x000834A2 File Offset: 0x000816A2
	ECosmeticSelectSide ISpawnable.CosmeticSelectedSide { get; set; }

	// Token: 0x06001AAD RID: 6829 RVA: 0x000023F4 File Offset: 0x000005F4
	void ISpawnable.OnDespawn()
	{
	}

	// Token: 0x06001AAE RID: 6830 RVA: 0x000834AB File Offset: 0x000816AB
	void ISpawnable.OnSpawn(VRRig rig)
	{
		this.myRig = rig;
	}

	// Token: 0x06001AAF RID: 6831 RVA: 0x000834B4 File Offset: 0x000816B4
	private void Update()
	{
		if ((this.attachedToLeftHand ? this.myRig.leftIndex.calcT : this.myRig.rightIndex.calcT) > 0.5f)
		{
			base.transform.localRotation *= Quaternion.AngleAxis(Time.deltaTime * this.rotationSpeed, Vector3.up);
			this.currentVolume = Mathf.MoveTowards(this.currentVolume, this.maxVolume, Time.deltaTime / this.fadeInDuration);
			this.audioSource.volume = this.currentVolume;
			if (!this.audioSource.isPlaying)
			{
				this.audioSource.GTPlay();
				return;
			}
		}
		else
		{
			this.currentVolume = Mathf.MoveTowards(this.currentVolume, 0f, Time.deltaTime / this.fadeOutDuration);
			if (this.audioSource.isPlaying)
			{
				if (this.currentVolume == 0f)
				{
					this.audioSource.GTStop();
					return;
				}
				this.audioSource.volume = this.currentVolume;
			}
		}
	}

	// Token: 0x04001D75 RID: 7541
	[SerializeField]
	private bool attachedToLeftHand;

	// Token: 0x04001D76 RID: 7542
	private VRRig myRig;

	// Token: 0x04001D77 RID: 7543
	[SerializeField]
	private float rotationSpeed;

	// Token: 0x04001D78 RID: 7544
	[SerializeField]
	private AudioSource audioSource;

	// Token: 0x04001D79 RID: 7545
	[SerializeField]
	private float maxVolume = 1f;

	// Token: 0x04001D7A RID: 7546
	[SerializeField]
	private float fadeInDuration;

	// Token: 0x04001D7B RID: 7547
	[SerializeField]
	private float fadeOutDuration;

	// Token: 0x04001D7E RID: 7550
	private float currentVolume;
}
