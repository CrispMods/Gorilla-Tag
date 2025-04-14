using System;
using GorillaTag;
using GorillaTag.CosmeticSystem;
using UnityEngine;

// Token: 0x0200043C RID: 1084
public class HypnoRing : MonoBehaviour, ISpawnable
{
	// Token: 0x170002E7 RID: 743
	// (get) Token: 0x06001AAC RID: 6828 RVA: 0x0008380D File Offset: 0x00081A0D
	// (set) Token: 0x06001AAD RID: 6829 RVA: 0x00083815 File Offset: 0x00081A15
	bool ISpawnable.IsSpawned { get; set; }

	// Token: 0x170002E8 RID: 744
	// (get) Token: 0x06001AAE RID: 6830 RVA: 0x0008381E File Offset: 0x00081A1E
	// (set) Token: 0x06001AAF RID: 6831 RVA: 0x00083826 File Offset: 0x00081A26
	ECosmeticSelectSide ISpawnable.CosmeticSelectedSide { get; set; }

	// Token: 0x06001AB0 RID: 6832 RVA: 0x000023F4 File Offset: 0x000005F4
	void ISpawnable.OnDespawn()
	{
	}

	// Token: 0x06001AB1 RID: 6833 RVA: 0x0008382F File Offset: 0x00081A2F
	void ISpawnable.OnSpawn(VRRig rig)
	{
		this.myRig = rig;
	}

	// Token: 0x06001AB2 RID: 6834 RVA: 0x00083838 File Offset: 0x00081A38
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

	// Token: 0x04001D76 RID: 7542
	[SerializeField]
	private bool attachedToLeftHand;

	// Token: 0x04001D77 RID: 7543
	private VRRig myRig;

	// Token: 0x04001D78 RID: 7544
	[SerializeField]
	private float rotationSpeed;

	// Token: 0x04001D79 RID: 7545
	[SerializeField]
	private AudioSource audioSource;

	// Token: 0x04001D7A RID: 7546
	[SerializeField]
	private float maxVolume = 1f;

	// Token: 0x04001D7B RID: 7547
	[SerializeField]
	private float fadeInDuration;

	// Token: 0x04001D7C RID: 7548
	[SerializeField]
	private float fadeOutDuration;

	// Token: 0x04001D7F RID: 7551
	private float currentVolume;
}
