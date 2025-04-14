using System;
using TMPro;
using UnityEngine;

// Token: 0x02000071 RID: 113
public class MenagerieSlot : MonoBehaviour
{
	// Token: 0x060002E9 RID: 745 RVA: 0x0001231E File Offset: 0x0001051E
	private void Reset()
	{
		this.critterMountPoint = base.transform;
	}

	// Token: 0x0400037E RID: 894
	public Transform critterMountPoint;

	// Token: 0x0400037F RID: 895
	public TMP_Text label;

	// Token: 0x04000380 RID: 896
	public MenagerieCritter critter;
}
