using System;
using TMPro;
using UnityEngine;

// Token: 0x02000071 RID: 113
public class MenagerieSlot : MonoBehaviour
{
	// Token: 0x060002EB RID: 747 RVA: 0x00012642 File Offset: 0x00010842
	private void Reset()
	{
		this.critterMountPoint = base.transform;
	}

	// Token: 0x0400037F RID: 895
	public Transform critterMountPoint;

	// Token: 0x04000380 RID: 896
	public TMP_Text label;

	// Token: 0x04000381 RID: 897
	public MenagerieCritter critter;
}
