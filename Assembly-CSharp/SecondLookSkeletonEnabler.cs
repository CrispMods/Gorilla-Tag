using System;
using UnityEngine;

// Token: 0x020000B4 RID: 180
public class SecondLookSkeletonEnabler : Tappable
{
	// Token: 0x0600049E RID: 1182 RVA: 0x0001BE33 File Offset: 0x0001A033
	private void Awake()
	{
		this.isTapped = false;
		this.skele = Object.FindFirstObjectByType<SecondLookSkeleton>();
		this.skele.spookyText = this.spookyText;
	}

	// Token: 0x0600049F RID: 1183 RVA: 0x0001BE58 File Offset: 0x0001A058
	public override void OnTapLocal(float tapStrength, float tapTime, PhotonMessageInfoWrapped info)
	{
		if (!this.isTapped)
		{
			base.OnTapLocal(tapStrength, tapTime, info);
			if (this.skele != null)
			{
				this.skele.tapped = true;
			}
			base.gameObject.SetActive(false);
			this.isTapped = true;
			this.playOnDisappear.GTPlay();
			this.particles.Play();
		}
	}

	// Token: 0x04000560 RID: 1376
	public bool isTapped;

	// Token: 0x04000561 RID: 1377
	public AudioSource playOnDisappear;

	// Token: 0x04000562 RID: 1378
	public ParticleSystem particles;

	// Token: 0x04000563 RID: 1379
	public GameObject spookyText;

	// Token: 0x04000564 RID: 1380
	private SecondLookSkeleton skele;
}
