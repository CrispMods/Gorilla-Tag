using System;
using UnityEngine;

// Token: 0x020001D4 RID: 468
[CreateAssetMenu(fileName = "PlatformTagJoin", menuName = "ScriptableObjects/PlatformTagJoin", order = 0)]
public class PlatformTagJoin : ScriptableObject
{
	// Token: 0x06000AEF RID: 2799 RVA: 0x00036AC3 File Offset: 0x00034CC3
	public override string ToString()
	{
		return this.PlatformTag;
	}

	// Token: 0x04000D55 RID: 3413
	public string PlatformTag = " ";
}
