using System;
using UnityEngine;

// Token: 0x020000BE RID: 190
public class SecondLookSkeletonEnabler : Tappable
{
	// Token: 0x060004DA RID: 1242 RVA: 0x00033A12 File Offset: 0x00031C12
	private void Awake()
	{
		this.isTapped = false;
		this.skele = UnityEngine.Object.FindFirstObjectByType<SecondLookSkeleton>();
		this.skele.spookyText = this.spookyText;
	}

	// Token: 0x060004DB RID: 1243 RVA: 0x0007F4B0 File Offset: 0x0007D6B0
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

	// Token: 0x040005A0 RID: 1440
	public bool isTapped;

	// Token: 0x040005A1 RID: 1441
	public AudioSource playOnDisappear;

	// Token: 0x040005A2 RID: 1442
	public ParticleSystem particles;

	// Token: 0x040005A3 RID: 1443
	public GameObject spookyText;

	// Token: 0x040005A4 RID: 1444
	private SecondLookSkeleton skele;
}
