using System;
using UnityEngine;

// Token: 0x020001D4 RID: 468
[CreateAssetMenu(fileName = "PlatformTagJoin", menuName = "ScriptableObjects/PlatformTagJoin", order = 0)]
public class PlatformTagJoin : ScriptableObject
{
	// Token: 0x06000AEF RID: 2799 RVA: 0x0003B442 File Offset: 0x00039642
	public override string ToString()
	{
		return this.PlatformTag;
	}

	// Token: 0x04000D55 RID: 3413
	public string PlatformTag = " ";
}
