using System;
using UnityEngine;

// Token: 0x020005E4 RID: 1508
[Serializable]
public class SizeLayerMask
{
	// Token: 0x170003DF RID: 991
	// (get) Token: 0x0600257C RID: 9596 RVA: 0x000B938C File Offset: 0x000B758C
	public int Mask
	{
		get
		{
			int num = 0;
			if (this.affectLayerA)
			{
				num |= 1;
			}
			if (this.affectLayerB)
			{
				num |= 2;
			}
			if (this.affectLayerC)
			{
				num |= 4;
			}
			if (this.affectLayerD)
			{
				num |= 8;
			}
			return num;
		}
	}

	// Token: 0x040029B1 RID: 10673
	[SerializeField]
	private bool affectLayerA = true;

	// Token: 0x040029B2 RID: 10674
	[SerializeField]
	private bool affectLayerB = true;

	// Token: 0x040029B3 RID: 10675
	[SerializeField]
	private bool affectLayerC = true;

	// Token: 0x040029B4 RID: 10676
	[SerializeField]
	private bool affectLayerD = true;
}
