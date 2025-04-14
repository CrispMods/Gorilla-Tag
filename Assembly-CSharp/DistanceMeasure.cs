using System;
using UnityEngine;

// Token: 0x020006A8 RID: 1704
public class DistanceMeasure : MonoBehaviour
{
	// Token: 0x06002A52 RID: 10834 RVA: 0x000D332F File Offset: 0x000D152F
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

	// Token: 0x04002FCF RID: 12239
	public Transform from;

	// Token: 0x04002FD0 RID: 12240
	public Transform to;
}
