using System;
using UnityEngine;

// Token: 0x020000B4 RID: 180
public class SecondLookSkeletonEnabler : Tappable
{
	// Token: 0x060004A0 RID: 1184 RVA: 0x0001C157 File Offset: 0x0001A357
	private void Awake()
	{
		this.isTapped = false;
		this.skele = Object.FindFirstObjectByType<SecondLookSkeleton>();
		this.skele.spookyText = this.spookyText;
	}

	// Token: 0x060004A1 RID: 1185 RVA: 0x0001C17C File Offset: 0x0001A37C
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

	// Token: 0x04000561 RID: 1377
	public bool isTapped;

	// Token: 0x04000562 RID: 1378
	public AudioSource playOnDisappear;

	// Token: 0x04000563 RID: 1379
	public ParticleSystem particles;

	// Token: 0x04000564 RID: 1380
	public GameObject spookyText;

	// Token: 0x04000565 RID: 1381
	private SecondLookSkeleton skele;
}
