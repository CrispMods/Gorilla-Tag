using System;
using NetSynchrony;
using UnityEngine;

// Token: 0x020008BB RID: 2235
public class ReportTargetHit : MonoBehaviour
{
	// Token: 0x0600360B RID: 13835 RVA: 0x000FFDB0 File Offset: 0x000FDFB0
	private void Start()
	{
		this.seekFreq = ReportTargetHit.rand.NextFloat(this.minseekFreq, this.maxseekFreq);
	}

	// Token: 0x0600360C RID: 13836 RVA: 0x000FFDCE File Offset: 0x000FDFCE
	private void OnEnable()
	{
		if (this.nsRand != null)
		{
			this.nsRand.Dispatch += this.NsRand_Dispatch;
		}
	}

	// Token: 0x0600360D RID: 13837 RVA: 0x000FFDF5 File Offset: 0x000FDFF5
	private void OnDisable()
	{
		if (this.nsRand != null)
		{
			this.nsRand.Dispatch -= this.NsRand_Dispatch;
		}
	}

	// Token: 0x0600360E RID: 13838 RVA: 0x000FFE1C File Offset: 0x000FE01C
	private void NsRand_Dispatch(RandomDispatcher randomDispatcher)
	{
		this.seek();
	}

	// Token: 0x0600360F RID: 13839 RVA: 0x000FFE24 File Offset: 0x000FE024
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

	// Token: 0x06003610 RID: 13840 RVA: 0x000FFE90 File Offset: 0x000FE090
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

	// Token: 0x04003839 RID: 14393
	private static SRand rand = new SRand("ReportForwardHit");

	// Token: 0x0400383A RID: 14394
	[SerializeField]
	private float minseekFreq = 3f;

	// Token: 0x0400383B RID: 14395
	[SerializeField]
	private float maxseekFreq = 6f;

	// Token: 0x0400383C RID: 14396
	[SerializeField]
	private Transform[] targets;

	// Token: 0x0400383D RID: 14397
	[SerializeField]
	private LightningDispatcherEvent colliderFound;

	// Token: 0x0400383E RID: 14398
	private float timeSinceSeek;

	// Token: 0x0400383F RID: 14399
	private float seekFreq;

	// Token: 0x04003840 RID: 14400
	[SerializeField]
	private RandomDispatcher nsRand;
}
