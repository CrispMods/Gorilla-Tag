using System;
using System.Runtime.CompilerServices;
using UnityEngine;

// Token: 0x020001A5 RID: 421
public static class GTVector3Extensions
{
	// Token: 0x06000A08 RID: 2568 RVA: 0x00036100 File Offset: 0x00034300
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Vector3 X_Z(this Vector3 vector)
	{
		return new Vector3(vector.x, 0f, vector.z);
	}
}
