using System;
using NetSynchrony;
using UnityEngine;

// Token: 0x020008BD RID: 2237
public class ReportForwardHit : MonoBehaviour
{
	// Token: 0x0600360F RID: 13839 RVA: 0x00052D60 File Offset: 0x00050F60
	private void Start()
	{
		this.seekFreq = ReportForwardHit.rand.NextFloat(this.minseekFreq, this.maxseekFreq);
	}

	// Token: 0x06003610 RID: 13840 RVA: 0x00052D7E File Offset: 0x00050F7E
	private void OnEnable()
	{
		if (this.seekOnEnable)
		{
			this.seek();
		}
		if (this.nsRand != null)
		{
			this.nsRand.Dispatch += this.NsRand_Dispatch;
		}
	}

	// Token: 0x06003611 RID: 13841 RVA: 0x00052DB3 File Offset: 0x00050FB3
	private void OnDisable()
	{
		if (this.nsRand != null)
		{
			this.nsRand.Dispatch -= this.NsRand_Dispatch;
		}
	}

	// Token: 0x06003612 RID: 13842 RVA: 0x00052DDA File Offset: 0x00050FDA
	private void NsRand_Dispatch(RandomDispatcher randomDispatcher)
	{
		this.seek();
	}

	// Token: 0x06003613 RID: 13843 RVA: 0x001400CC File Offset: 0x0013E2CC
	private void Update()
	{
		if (this.nsRand != null)
		{
			return;
		}
		this.timeSinceSeek += Time.deltaTime;
		if (this.timeSinceSeek > this.seekFreq)
		{
			this.seek();
			this.timeSinceSeek = 0f;
			this.seekFreq = ReportForwardHit.rand.NextFloat(this.minseekFreq, this.maxseekFreq);
		}
	}

	// Token: 0x06003614 RID: 13844 RVA: 0x00140138 File Offset: 0x0013E338
	private void seek()
	{
		float num = Mathf.Max(new float[]
		{
			base.transform.lossyScale.x,
			base.transform.lossyScale.y,
			base.transform.lossyScale.z
		});
		RaycastHit raycastHit;
		if (Physics.Raycast(base.transform.position, base.transform.forward, out raycastHit, this.maxRadias * num) && this.colliderFound != null)
		{
			this.colliderFound.Invoke(base.transform.position, raycastHit.point);
		}
	}

	// Token: 0x04003842 RID: 14402
	private static SRand rand = new SRand("ReportForwardHit");

	// Token: 0x04003843 RID: 14403
	[SerializeField]
	private float minseekFreq = 3f;

	// Token: 0x04003844 RID: 14404
	[SerializeField]
	private float maxseekFreq = 6f;

	// Token: 0x04003845 RID: 14405
	[SerializeField]
	private float maxRadias = 10f;

	// Token: 0x04003846 RID: 14406
	[SerializeField]
	private LightningDispatcherEvent colliderFound;

	// Token: 0x04003847 RID: 14407
	[SerializeField]
	private RandomDispatcher nsRand;

	// Token: 0x04003848 RID: 14408
	private float timeSinceSeek;

	// Token: 0x04003849 RID: 14409
	private float seekFreq;

	// Token: 0x0400384A RID: 14410
	[SerializeField]
	private bool seekOnEnable;
}
