using System;
using TMPro;
using UnityEngine;

// Token: 0x02000077 RID: 119
public class MenagerieSlot : MonoBehaviour
{
	// Token: 0x06000318 RID: 792 RVA: 0x00032637 File Offset: 0x00030837
	private void Reset()
	{
		this.critterMountPoint = base.transform;
	}

	// Token: 0x040003B0 RID: 944
	public Transform critterMountPoint;

	// Token: 0x040003B1 RID: 945
	public TMP_Text label;

	// Token: 0x040003B2 RID: 946
	public MenagerieCritter critter;
}
