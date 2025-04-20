using System;
using NetSynchrony;
using UnityEngine;

// Token: 0x020008D6 RID: 2262
public class ReportForwardHit : MonoBehaviour
{
	// Token: 0x060036CB RID: 14027 RVA: 0x0005427D File Offset: 0x0005247D
	private void Start()
	{
		this.seekFreq = ReportForwardHit.rand.NextFloat(this.minseekFreq, this.maxseekFreq);
	}

	// Token: 0x060036CC RID: 14028 RVA: 0x0005429B File Offset: 0x0005249B
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

	// Token: 0x060036CD RID: 14029 RVA: 0x000542D0 File Offset: 0x000524D0
	private void OnDisable()
	{
		if (this.nsRand != null)
		{
			this.nsRand.Dispatch -= this.NsRand_Dispatch;
		}
	}

	// Token: 0x060036CE RID: 14030 RVA: 0x000542F7 File Offset: 0x000524F7
	private void NsRand_Dispatch(RandomDispatcher randomDispatcher)
	{
		this.seek();
	}

	// Token: 0x060036CF RID: 14031 RVA: 0x0014568C File Offset: 0x0014388C
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

	// Token: 0x060036D0 RID: 14032 RVA: 0x001456F8 File Offset: 0x001438F8
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

	// Token: 0x040038F1 RID: 14577
	private static SRand rand = new SRand("ReportForwardHit");

	// Token: 0x040038F2 RID: 14578
	[SerializeField]
	private float minseekFreq = 3f;

	// Token: 0x040038F3 RID: 14579
	[SerializeField]
	private float maxseekFreq = 6f;

	// Token: 0x040038F4 RID: 14580
	[SerializeField]
	private float maxRadias = 10f;

	// Token: 0x040038F5 RID: 14581
	[SerializeField]
	private LightningDispatcherEvent colliderFound;

	// Token: 0x040038F6 RID: 14582
	[SerializeField]
	private RandomDispatcher nsRand;

	// Token: 0x040038F7 RID: 14583
	private float timeSinceSeek;

	// Token: 0x040038F8 RID: 14584
	private float seekFreq;

	// Token: 0x040038F9 RID: 14585
	[SerializeField]
	private bool seekOnEnable;
}
