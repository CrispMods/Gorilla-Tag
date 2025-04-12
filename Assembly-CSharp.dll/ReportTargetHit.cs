using System;
using NetSynchrony;
using UnityEngine;

// Token: 0x020008BE RID: 2238
public class ReportTargetHit : MonoBehaviour
{
	// Token: 0x06003617 RID: 13847 RVA: 0x00052E1C File Offset: 0x0005101C
	private void Start()
	{
		this.seekFreq = ReportTargetHit.rand.NextFloat(this.minseekFreq, this.maxseekFreq);
	}

	// Token: 0x06003618 RID: 13848 RVA: 0x00052E3A File Offset: 0x0005103A
	private void OnEnable()
	{
		if (this.nsRand != null)
		{
			this.nsRand.Dispatch += this.NsRand_Dispatch;
		}
	}

	// Token: 0x06003619 RID: 13849 RVA: 0x00052E61 File Offset: 0x00051061
	private void OnDisable()
	{
		if (this.nsRand != null)
		{
			this.nsRand.Dispatch -= this.NsRand_Dispatch;
		}
	}

	// Token: 0x0600361A RID: 13850 RVA: 0x00052E88 File Offset: 0x00051088
	private void NsRand_Dispatch(RandomDispatcher randomDispatcher)
	{
		this.seek();
	}

	// Token: 0x0600361B RID: 13851 RVA: 0x001401D8 File Offset: 0x0013E3D8
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

	// Token: 0x0600361C RID: 13852 RVA: 0x00140244 File Offset: 0x0013E444
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

	// Token: 0x0400384B RID: 14411
	private static SRand rand = new SRand("ReportForwardHit");

	// Token: 0x0400384C RID: 14412
	[SerializeField]
	private float minseekFreq = 3f;

	// Token: 0x0400384D RID: 14413
	[SerializeField]
	private float maxseekFreq = 6f;

	// Token: 0x0400384E RID: 14414
	[SerializeField]
	private Transform[] targets;

	// Token: 0x0400384F RID: 14415
	[SerializeField]
	private LightningDispatcherEvent colliderFound;

	// Token: 0x04003850 RID: 14416
	private float timeSinceSeek;

	// Token: 0x04003851 RID: 14417
	private float seekFreq;

	// Token: 0x04003852 RID: 14418
	[SerializeField]
	private RandomDispatcher nsRand;
}
