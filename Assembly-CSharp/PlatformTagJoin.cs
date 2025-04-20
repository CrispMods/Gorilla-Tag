using System;
using UnityEngine;

// Token: 0x020001DF RID: 479
[CreateAssetMenu(fileName = "PlatformTagJoin", menuName = "ScriptableObjects/PlatformTagJoin", order = 0)]
public class PlatformTagJoin : ScriptableObject
{
	// Token: 0x06000B39 RID: 2873 RVA: 0x00037D83 File Offset: 0x00035F83
	public override string ToString()
	{
		return this.PlatformTag;
	}

	// Token: 0x04000D9A RID: 3482
	public string PlatformTag = " ";
}
