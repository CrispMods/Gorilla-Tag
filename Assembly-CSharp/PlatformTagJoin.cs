using System;
using UnityEngine;

// Token: 0x020001D4 RID: 468
[CreateAssetMenu(fileName = "PlatformTagJoin", menuName = "ScriptableObjects/PlatformTagJoin", order = 0)]
public class PlatformTagJoin : ScriptableObject
{
	// Token: 0x06000AED RID: 2797 RVA: 0x0003B11E File Offset: 0x0003931E
	public override string ToString()
	{
		return this.PlatformTag;
	}

	// Token: 0x04000D54 RID: 3412
	public string PlatformTag = " ";
}
