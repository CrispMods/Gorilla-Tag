using System;
using NetSynchrony;
using UnityEngine;

// Token: 0x020008D7 RID: 2263
public class ReportTargetHit : MonoBehaviour
{
	// Token: 0x060036D3 RID: 14035 RVA: 0x00054339 File Offset: 0x00052539
	private void Start()
	{
		this.seekFreq = ReportTargetHit.rand.NextFloat(this.minseekFreq, this.maxseekFreq);
	}

	// Token: 0x060036D4 RID: 14036 RVA: 0x00054357 File Offset: 0x00052557
	private void OnEnable()
	{
		if (this.nsRand != null)
		{
			this.nsRand.Dispatch += this.NsRand_Dispatch;
		}
	}

	// Token: 0x060036D5 RID: 14037 RVA: 0x0005437E File Offset: 0x0005257E
	private void OnDisable()
	{
		if (this.nsRand != null)
		{
			this.nsRand.Dispatch -= this.NsRand_Dispatch;
		}
	}

	// Token: 0x060036D6 RID: 14038 RVA: 0x000543A5 File Offset: 0x000525A5
	private void NsRand_Dispatch(RandomDispatcher randomDispatcher)
	{
		this.seek();
	}

	// Token: 0x060036D7 RID: 14039 RVA: 0x00145798 File Offset: 0x00143998
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
			this.seekFreq = ReportTargetHit.rand.NextFloat(this.minseekFreq, this.maxseekFreq);
		}
	}

	// Token: 0x060036D8 RID: 14040 RVA: 0x00145804 File Offset: 0x00143A04
	private void seek()
	{
		if (this.targets.Length != 0)
		{
			Vector3 direction = this.targets[ReportTargetHit.rand.NextInt(this.targets.Length)].position - base.transform.position;
			RaycastHit raycastHit;
			if (Physics.Raycast(base.transform.position, direction, out raycastHit) && this.colliderFound != null)
			{
				this.colliderFound.Invoke(base.transform.position, raycastHit.point);
			}
		}
	}

	// Token: 0x040038FA RID: 14586
	private static SRand rand = new SRand("ReportForwardHit");

	// Token: 0x040038FB RID: 14587
	[SerializeField]
	private float minseekFreq = 3f;

	// Token: 0x040038FC RID: 14588
	[SerializeField]
	private float maxseekFreq = 6f;

	// Token: 0x040038FD RID: 14589
	[SerializeField]
	private Transform[] targets;

	// Token: 0x040038FE RID: 14590
	[SerializeField]
	private LightningDispatcherEvent colliderFound;

	// Token: 0x040038FF RID: 14591
	private float timeSinceSeek;

	// Token: 0x04003900 RID: 14592
	private float seekFreq;

	// Token: 0x04003901 RID: 14593
	[SerializeField]
	private RandomDispatcher nsRand;
}
