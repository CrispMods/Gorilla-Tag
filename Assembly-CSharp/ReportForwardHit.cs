using System;
using NetSynchrony;
using UnityEngine;

// Token: 0x020008BA RID: 2234
public class ReportForwardHit : MonoBehaviour
{
	// Token: 0x06003603 RID: 13827 RVA: 0x000FFBE9 File Offset: 0x000FDDE9
	private void Start()
	{
		this.seekFreq = ReportForwardHit.rand.NextFloat(this.minseekFreq, this.maxseekFreq);
	}

	// Token: 0x06003604 RID: 13828 RVA: 0x000FFC07 File Offset: 0x000FDE07
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

	// Token: 0x06003605 RID: 13829 RVA: 0x000FFC3C File Offset: 0x000FDE3C
	private void OnDisable()
	{
		if (this.nsRand != null)
		{
			this.nsRand.Dispatch -= this.NsRand_Dispatch;
		}
	}

	// Token: 0x06003606 RID: 13830 RVA: 0x000FFC63 File Offset: 0x000FDE63
	private void NsRand_Dispatch(RandomDispatcher randomDispatcher)
	{
		this.seek();
	}

	// Token: 0x06003607 RID: 13831 RVA: 0x000FFC6C File Offset: 0x000FDE6C
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

	// Token: 0x06003608 RID: 13832 RVA: 0x000FFCD8 File Offset: 0x000FDED8
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

	// Token: 0x04003830 RID: 14384
	private static SRand rand = new SRand("ReportForwardHit");

	// Token: 0x04003831 RID: 14385
	[SerializeField]
	private float minseekFreq = 3f;

	// Token: 0x04003832 RID: 14386
	[SerializeField]
	private float maxseekFreq = 6f;

	// Token: 0x04003833 RID: 14387
	[SerializeField]
	private float maxRadias = 10f;

	// Token: 0x04003834 RID: 14388
	[SerializeField]
	private LightningDispatcherEvent colliderFound;

	// Token: 0x04003835 RID: 14389
	[SerializeField]
	private RandomDispatcher nsRand;

	// Token: 0x04003836 RID: 14390
	private float timeSinceSeek;

	// Token: 0x04003837 RID: 14391
	private float seekFreq;

	// Token: 0x04003838 RID: 14392
	[SerializeField]
	private bool seekOnEnable;
}
