using System;
using UnityEngine;

// Token: 0x020006B7 RID: 1719
public static class GradientHelper
{
	// Token: 0x06002A8E RID: 10894 RVA: 0x000D409C File Offset: 0x000D229C
	public static Gradient FromColor(Color color)
	{
		float a = color.a;
		Color col = color;
		col.a = 1f;
		return new Gradient
		{
			colorKeys = new GradientColorKey[]
			{
				new GradientColorKey(col, 1f)
			},
			alphaKeys = new GradientAlphaKey[]
			{
				new GradientAlphaKey(a, 1f)
			}
		};
	}
}
