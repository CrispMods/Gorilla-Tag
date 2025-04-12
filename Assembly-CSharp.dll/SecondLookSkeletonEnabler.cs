using System;
using UnityEngine;

// Token: 0x020000B4 RID: 180
public class SecondLookSkeletonEnabler : Tappable
{
	// Token: 0x060004A0 RID: 1184 RVA: 0x0003280B File Offset: 0x00030A0B
	private void Awake()
	{
		this.isTapped = false;
		this.skele = UnityEngine.Object.FindFirstObjectByType<SecondLookSkeleton>();
		this.skele.spookyText = this.spookyText;
	}

	// Token: 0x060004A1 RID: 1185 RVA: 0x0007CC54 File Offset: 0x0007AE54
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
