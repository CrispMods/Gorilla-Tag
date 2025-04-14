using System;
using UnityEngine;

// Token: 0x02000872 RID: 2162
public class PredicatableRandomRotation : MonoBehaviour
{
	// Token: 0x06003442 RID: 13378 RVA: 0x000F8E6C File Offset: 0x000F706C
	private void Start()
	{
		if (this.source == null)
		{
			this.source = base.transform;
		}
	}

	// Token: 0x06003443 RID: 13379 RVA: 0x000F8E88 File Offset: 0x000F7088
	private void Update()
	{
		float d = (this.source.position.x * this.source.position.x + this.source.position.y * this.source.position.y + this.source.position.z * this.source.position.z) % 1f;
		base.transform.Rotate(this.rot * d);
	}

	// Token: 0x0400372F RID: 14127
	[SerializeField]
	private Vector3 rot = Vector3.zero;

	// Token: 0x04003730 RID: 14128
	[SerializeField]
	private Transform source;
}
