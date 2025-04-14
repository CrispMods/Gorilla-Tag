using System;
using UnityEngine;

// Token: 0x020006B6 RID: 1718
public static class GradientHelper
{
	// Token: 0x06002A86 RID: 10886 RVA: 0x000D3C1C File Offset: 0x000D1E1C
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
