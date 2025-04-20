using System;
using GorillaLocomotion;
using UnityEngine;

// Token: 0x020004C0 RID: 1216
public class BuilderSizeLayerChanger : MonoBehaviour
{
	// Token: 0x17000319 RID: 793
	// (get) Token: 0x06001D7A RID: 7546 RVA: 0x000E112C File Offset: 0x000DF32C
	public int SizeLayerMask
	{
		get
		{
			int num = 0;
			if (this.affectLayerA)
			{
				num |= 1;
			}
			if (this.affectLayerB)
			{
				num |= 2;
			}
			if (this.affectLayerC)
			{
				num |= 4;
			}
			if (this.affectLayerD)
			{
				num |= 8;
			}
			return num;
		}
	}

	// Token: 0x06001D7B RID: 7547 RVA: 0x0004429A File Offset: 0x0004249A
	private void Awake()
	{
		this.minScale = Mathf.Max(this.minScale, 0.01f);
	}

	// Token: 0x06001D7C RID: 7548 RVA: 0x000E116C File Offset: 0x000DF36C
	public void OnTriggerEnter(Collider other)
	{
		if (other != GTPlayer.Instance.bodyCollider)
		{
			return;
		}
		VRRig offlineVRRig = GorillaTagger.Instance.offlineVRRig;
		if (offlineVRRig == null)
		{
			return;
		}
		if (this.applyOnTriggerEnter)
		{
			if (offlineVRRig.sizeManager.currentSizeLayerMaskValue != this.SizeLayerMask && this.fxForLayerChange != null)
			{
				ObjectPools.instance.Instantiate(this.fxForLayerChange, offlineVRRig.transform.position);
			}
			offlineVRRig.sizeManager.currentSizeLayerMaskValue = this.SizeLayerMask;
		}
	}

	// Token: 0x06001D7D RID: 7549 RVA: 0x000E11F8 File Offset: 0x000DF3F8
	public void OnTriggerExit(Collider other)
	{
		if (other != GTPlayer.Instance.bodyCollider)
		{
			return;
		}
		VRRig offlineVRRig = GorillaTagger.Instance.offlineVRRig;
		if (offlineVRRig == null)
		{
			return;
		}
		if (this.applyOnTriggerExit)
		{
			if (offlineVRRig.sizeManager.currentSizeLayerMaskValue != this.SizeLayerMask && this.fxForLayerChange != null)
			{
				ObjectPools.instance.Instantiate(this.fxForLayerChange, offlineVRRig.transform.position);
			}
			offlineVRRig.sizeManager.currentSizeLayerMaskValue = this.SizeLayerMask;
		}
	}

	// Token: 0x04002075 RID: 8309
	public float maxScale;

	// Token: 0x04002076 RID: 8310
	public float minScale;

	// Token: 0x04002077 RID: 8311
	public bool isAssurance;

	// Token: 0x04002078 RID: 8312
	public bool affectLayerA = true;

	// Token: 0x04002079 RID: 8313
	public bool affectLayerB = true;

	// Token: 0x0400207A RID: 8314
	public bool affectLayerC = true;

	// Token: 0x0400207B RID: 8315
	public bool affectLayerD = true;

	// Token: 0x0400207C RID: 8316
	[SerializeField]
	private bool applyOnTriggerEnter = true;

	// Token: 0x0400207D RID: 8317
	[SerializeField]
	private bool applyOnTriggerExit;

	// Token: 0x0400207E RID: 8318
	[SerializeField]
	private GameObject fxForLayerChange;
}
