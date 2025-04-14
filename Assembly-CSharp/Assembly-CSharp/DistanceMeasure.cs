using System;
using UnityEngine;

// Token: 0x020006A9 RID: 1705
public class DistanceMeasure : MonoBehaviour
{
	// Token: 0x06002A5A RID: 10842 RVA: 0x000D37AF File Offset: 0x000D19AF
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

	// Token: 0x04002FD5 RID: 12245
	public Transform from;

	// Token: 0x04002FD6 RID: 12246
	public Transform to;
}
