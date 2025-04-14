using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020008BC RID: 2236
public class RandomLocalColliders : MonoBehaviour
{
	// Token: 0x0600360A RID: 13834 RVA: 0x000FFFDB File Offset: 0x000FE1DB
	private void Start()
	{
		this.colliders = new List<Collider>();
		this.seekFreq = RandomLocalColliders.rand.NextFloat(this.minseekFreq, this.maxseekFreq);
	}

	// Token: 0x0600360B RID: 13835 RVA: 0x00100004 File Offset: 0x000FE204
	private void Update()
	{
		this.timeSinceSeek += Time.deltaTime;
		if (this.timeSinceSeek > this.seekFreq)
		{
			this.seek();
			this.timeSinceSeek = 0f;
			this.seekFreq = RandomLocalColliders.rand.NextFloat(this.minseekFreq, this.maxseekFreq);
		}
	}

	// Token: 0x0600360C RID: 13836 RVA: 0x00100060 File Offset: 0x000FE260
	private void seek()
	{
		float num = Mathf.Max(new float[]
		{
			base.transform.lossyScale.x,
			base.transform.lossyScale.y,
			base.transform.lossyScale.z
		});
		this.colliders.Clear();
		this.colliders.AddRange(Physics.OverlapSphere(base.transform.position, this.maxRadias * num));
		Collider[] array = Physics.OverlapSphere(base.transform.position, this.minRadias * num);
		for (int i = 0; i < array.Length; i++)
		{
			this.colliders.Remove(array[i]);
		}
		if (this.colliders.Count > 0 && this.colliderFound != null)
		{
			this.colliderFound.Invoke(base.transform.position, this.colliders[RandomLocalColliders.rand.NextInt(this.colliders.Count)].transform.position);
		}
	}

	// Token: 0x04003839 RID: 14393
	private static SRand rand = new SRand("RandomLocalColliders");

	// Token: 0x0400383A RID: 14394
	[SerializeField]
	private float minseekFreq = 3f;

	// Token: 0x0400383B RID: 14395
	[SerializeField]
	private float maxseekFreq = 6f;

	// Token: 0x0400383C RID: 14396
	[SerializeField]
	private float minRadias = 1f;

	// Token: 0x0400383D RID: 14397
	[SerializeField]
	private float maxRadias = 10f;

	// Token: 0x0400383E RID: 14398
	[SerializeField]
	private LightningDispatcherEvent colliderFound;

	// Token: 0x0400383F RID: 14399
	private List<Collider> colliders;

	// Token: 0x04003840 RID: 14400
	private float timeSinceSeek;

	// Token: 0x04003841 RID: 14401
	private float seekFreq;
}
