using System;
using UnityEngine;

// Token: 0x020005F2 RID: 1522
[Serializable]
public class SizeLayerMask
{
	// Token: 0x170003E7 RID: 999
	// (get) Token: 0x060025DE RID: 9694 RVA: 0x0010712C File Offset: 0x0010532C
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

	// Token: 0x04002A10 RID: 10768
	[SerializeField]
	private bool affectLayerA = true;

	// Token: 0x04002A11 RID: 10769
	[SerializeField]
	private bool affectLayerB = true;

	// Token: 0x04002A12 RID: 10770
	[SerializeField]
	private bool affectLayerC = true;

	// Token: 0x04002A13 RID: 10771
	[SerializeField]
	private bool affectLayerD = true;
}
