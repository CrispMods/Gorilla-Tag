using System;
using UnityEngine;

// Token: 0x02000306 RID: 774
public static class VectorUtil
{
	// Token: 0x06001283 RID: 4739 RVA: 0x0003CB4E File Offset: 0x0003AD4E
	public static Vector4 ToVector(this Rect rect)
	{
		return new Vector4(rect.x, rect.y, rect.width, rect.height);
	}
}
