using System;
using System.Runtime.CompilerServices;
using UnityEngine;

// Token: 0x020001B0 RID: 432
public static class GTVector3Extensions
{
	// Token: 0x06000A52 RID: 2642 RVA: 0x000373C0 File Offset: 0x000355C0
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Vector3 X_Z(this Vector3 vector)
	{
		return new Vector3(vector.x, 0f, vector.z);
	}
}
