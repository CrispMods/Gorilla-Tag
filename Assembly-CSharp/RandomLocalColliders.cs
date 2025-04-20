using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020008D5 RID: 2261
public class RandomLocalColliders : MonoBehaviour
{
	// Token: 0x060036C6 RID: 14022 RVA: 0x0005420F File Offset: 0x0005240F
	private void Start()
	{
		this.colliders = new List<Collider>();
		this.seekFreq = RandomLocalColliders.rand.NextFloat(this.minseekFreq, this.maxseekFreq);
	}

	// Token: 0x060036C7 RID: 14023 RVA: 0x00145524 File Offset: 0x00143724
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

	// Token: 0x060036C8 RID: 14024 RVA: 0x00145580 File Offset: 0x00143780
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

	// Token: 0x040038E8 RID: 14568
	private static SRand rand = new SRand("RandomLocalColliders");

	// Token: 0x040038E9 RID: 14569
	[SerializeField]
	private float minseekFreq = 3f;

	// Token: 0x040038EA RID: 14570
	[SerializeField]
	private float maxseekFreq = 6f;

	// Token: 0x040038EB RID: 14571
	[SerializeField]
	private float minRadias = 1f;

	// Token: 0x040038EC RID: 14572
	[SerializeField]
	private float maxRadias = 10f;

	// Token: 0x040038ED RID: 14573
	[SerializeField]
	private LightningDispatcherEvent colliderFound;

	// Token: 0x040038EE RID: 14574
	private List<Collider> colliders;

	// Token: 0x040038EF RID: 14575
	private float timeSinceSeek;

	// Token: 0x040038F0 RID: 14576
	private float seekFreq;
}
