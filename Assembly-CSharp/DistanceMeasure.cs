using System;
using UnityEngine;

// Token: 0x020006BD RID: 1725
public class DistanceMeasure : MonoBehaviour
{
	// Token: 0x06002AE8 RID: 10984 RVA: 0x0004CEAC File Offset: 0x0004B0AC
	private void Awake()
	{
		if (this.from == null)
		{
			this.from = base.transform;
		}
		if (this.to == null)
		{
			this.to = base.transform;
		}
	}

	// Token: 0x0400306C RID: 12396
	public Transform from;

	// Token: 0x0400306D RID: 12397
	public Transform to;
}
