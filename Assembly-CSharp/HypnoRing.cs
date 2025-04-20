using System;
using GorillaTag;
using GorillaTag.CosmeticSystem;
using UnityEngine;

// Token: 0x02000448 RID: 1096
public class HypnoRing : MonoBehaviour, ISpawnable
{
	// Token: 0x170002EE RID: 750
	// (get) Token: 0x06001AFD RID: 6909 RVA: 0x0004256E File Offset: 0x0004076E
	// (set) Token: 0x06001AFE RID: 6910 RVA: 0x00042576 File Offset: 0x00040776
	bool ISpawnable.IsSpawned { get; set; }

	// Token: 0x170002EF RID: 751
	// (get) Token: 0x06001AFF RID: 6911 RVA: 0x0004257F File Offset: 0x0004077F
	// (set) Token: 0x06001B00 RID: 6912 RVA: 0x00042587 File Offset: 0x00040787
	ECosmeticSelectSide ISpawnable.CosmeticSelectedSide { get; set; }

	// Token: 0x06001B01 RID: 6913 RVA: 0x00030607 File Offset: 0x0002E807
	void ISpawnable.OnDespawn()
	{
	}

	// Token: 0x06001B02 RID: 6914 RVA: 0x00042590 File Offset: 0x00040790
	void ISpawnable.OnSpawn(VRRig rig)
	{
		this.myRig = rig;
	}

	// Token: 0x06001B03 RID: 6915 RVA: 0x000D83F0 File Offset: 0x000D65F0
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

	// Token: 0x04001DC4 RID: 7620
	[SerializeField]
	private bool attachedToLeftHand;

	// Token: 0x04001DC5 RID: 7621
	private VRRig myRig;

	// Token: 0x04001DC6 RID: 7622
	[SerializeField]
	private float rotationSpeed;

	// Token: 0x04001DC7 RID: 7623
	[SerializeField]
	private AudioSource audioSource;

	// Token: 0x04001DC8 RID: 7624
	[SerializeField]
	private float maxVolume = 1f;

	// Token: 0x04001DC9 RID: 7625
	[SerializeField]
	private float fadeInDuration;

	// Token: 0x04001DCA RID: 7626
	[SerializeField]
	private float fadeOutDuration;

	// Token: 0x04001DCD RID: 7629
	private float currentVolume;
}
