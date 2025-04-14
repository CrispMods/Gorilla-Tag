using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020008B9 RID: 2233
public class RandomLocalColliders : MonoBehaviour
{
	// Token: 0x060035FE RID: 13822 RVA: 0x000FFA13 File Offset: 0x000FDC13
	private void Start()
	{
		this.colliders = new List<Collider>();
		this.seekFreq = RandomLocalColliders.rand.NextFloat(this.minseekFreq, this.maxseekFreq);
	}

	// Token: 0x060035FF RID: 13823 RVA: 0x000FFA3C File Offset: 0x000FDC3C
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

	// Token: 0x06003600 RID: 13824 RVA: 0x000FFA98 File Offset: 0x000FDC98
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

	// Token: 0x04003827 RID: 14375
	private static SRand rand = new SRand("RandomLocalColliders");

	// Token: 0x04003828 RID: 14376
	[SerializeField]
	private float minseekFreq = 3f;

	// Token: 0x04003829 RID: 14377
	[SerializeField]
	private float maxseekFreq = 6f;

	// Token: 0x0400382A RID: 14378
	[SerializeField]
	private float minRadias = 1f;

	// Token: 0x0400382B RID: 14379
	[SerializeField]
	private float maxRadias = 10f;

	// Token: 0x0400382C RID: 14380
	[SerializeField]
	private LightningDispatcherEvent colliderFound;

	// Token: 0x0400382D RID: 14381
	private List<Collider> colliders;

	// Token: 0x0400382E RID: 14382
	private float timeSinceSeek;

	// Token: 0x0400382F RID: 14383
	private float seekFreq;
}
