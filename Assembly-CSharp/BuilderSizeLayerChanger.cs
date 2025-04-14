using System;
using GorillaLocomotion;
using UnityEngine;

// Token: 0x020004B3 RID: 1203
public class BuilderSizeLayerChanger : MonoBehaviour
{
	// Token: 0x17000312 RID: 786
	// (get) Token: 0x06001D21 RID: 7457 RVA: 0x0008DE44 File Offset: 0x0008C044
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

	// Token: 0x06001D22 RID: 7458 RVA: 0x0008DE84 File Offset: 0x0008C084
	private void Awake()
	{
		this.minScale = Mathf.Max(this.minScale, 0.01f);
	}

	// Token: 0x06001D23 RID: 7459 RVA: 0x0008DE9C File Offset: 0x0008C09C
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

	// Token: 0x06001D24 RID: 7460 RVA: 0x0008DF28 File Offset: 0x0008C128
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

	// Token: 0x04002022 RID: 8226
	public float maxScale;

	// Token: 0x04002023 RID: 8227
	public float minScale;

	// Token: 0x04002024 RID: 8228
	public bool isAssurance;

	// Token: 0x04002025 RID: 8229
	public bool affectLayerA = true;

	// Token: 0x04002026 RID: 8230
	public bool affectLayerB = true;

	// Token: 0x04002027 RID: 8231
	public bool affectLayerC = true;

	// Token: 0x04002028 RID: 8232
	public bool affectLayerD = true;

	// Token: 0x04002029 RID: 8233
	[SerializeField]
	private bool applyOnTriggerEnter = true;

	// Token: 0x0400202A RID: 8234
	[SerializeField]
	private bool applyOnTriggerExit;

	// Token: 0x0400202B RID: 8235
	[SerializeField]
	private GameObject fxForLayerChange;
}
