using System;
using UnityEngine;

// Token: 0x020005E5 RID: 1509
[Serializable]
public class SizeLayerMask
{
	// Token: 0x170003E0 RID: 992
	// (get) Token: 0x06002584 RID: 9604 RVA: 0x000B980C File Offset: 0x000B7A0C
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

	// Token: 0x040029B7 RID: 10679
	[SerializeField]
	private bool affectLayerA = true;

	// Token: 0x040029B8 RID: 10680
	[SerializeField]
	private bool affectLayerB = true;

	// Token: 0x040029B9 RID: 10681
	[SerializeField]
	private bool affectLayerC = true;

	// Token: 0x040029BA RID: 10682
	[SerializeField]
	private bool affectLayerD = true;
}
