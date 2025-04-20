using System;
using UnityEngine;

// Token: 0x020006CB RID: 1739
public static class GradientHelper
{
	// Token: 0x06002B1C RID: 11036 RVA: 0x0011FEC0 File Offset: 0x0011E0C0
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
