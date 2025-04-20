using System;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Rendering;

// Token: 0x020008FA RID: 2298
public static class GTUberShaderUtils
{
	// Token: 0x06003770 RID: 14192 RVA: 0x000549E7 File Offset: 0x00052BE7
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void SetStencilComparison(this Material m, GTShaderStencilCompare cmp)
	{
		m.SetFloat(GTUberShaderUtils._StencilComparison, (float)cmp);
	}

	// Token: 0x06003771 RID: 14193 RVA: 0x000549FB File Offset: 0x00052BFB
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void SetStencilPassFrontOp(this Material m, GTShaderStencilOp op)
	{
		m.SetFloat(GTUberShaderUtils._StencilPassFront, (float)op);
	}

	// Token: 0x06003772 RID: 14194 RVA: 0x00054A0F File Offset: 0x00052C0F
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void SetStencilReferenceValue(this Material m, int value)
	{
		m.SetFloat(GTUberShaderUtils._StencilReference, (float)value);
	}

	// Token: 0x06003773 RID: 14195 RVA: 0x0014788C File Offset: 0x00145A8C
	public static void SetVisibleToXRay(this Material m, bool visible, bool saveToDisk = false)
	{
		GTShaderStencilCompare cmp = visible ? GTShaderStencilCompare.Equal : GTShaderStencilCompare.NotEqual;
		GTShaderStencilOp op = visible ? GTShaderStencilOp.Replace : GTShaderStencilOp.Keep;
		m.SetStencilComparison(cmp);
		m.SetStencilPassFrontOp(op);
		m.SetStencilReferenceValue(7);
	}

	// Token: 0x06003774 RID: 14196 RVA: 0x001478C0 File Offset: 0x00145AC0
	public static void SetRevealsXRay(this Material m, bool reveals, bool changeQueue = true, bool saveToDisk = false)
	{
		m.SetFloat(GTUberShaderUtils._ZWrite, (float)(reveals ? 0 : 1));
		m.SetFloat(GTUberShaderUtils._ColorMask_, (float)(reveals ? 0 : 14));
		m.SetStencilComparison(GTShaderStencilCompare.Disabled);
		m.SetStencilPassFrontOp(reveals ? GTShaderStencilOp.Replace : GTShaderStencilOp.Keep);
		m.SetStencilReferenceValue(reveals ? 7 : 0);
		if (changeQueue)
		{
			int renderQueue = m.renderQueue;
			m.renderQueue = renderQueue + (reveals ? -1 : 1);
		}
	}

	// Token: 0x06003775 RID: 14197 RVA: 0x00147938 File Offset: 0x00145B38
	public static int GetNearestRenderQueue(this Material m, out RenderQueue queue)
	{
		int renderQueue = m.renderQueue;
		int num = -1;
		int num2 = int.MaxValue;
		for (int i = 0; i < GTUberShaderUtils.kRenderQueueInts.Length; i++)
		{
			int num3 = GTUberShaderUtils.kRenderQueueInts[i];
			int num4 = Math.Abs(num3 - renderQueue);
			if (num2 > num4)
			{
				num = num3;
				num2 = num4;
			}
		}
		queue = (RenderQueue)num;
		return num;
	}

	// Token: 0x06003776 RID: 14198 RVA: 0x00054A23 File Offset: 0x00052C23
	[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
	private static void InitOnLoad()
	{
		GTUberShaderUtils.kUberShader = Shader.Find("GorillaTag/UberShader");
	}

	// Token: 0x040039FB RID: 14843
	private static Shader kUberShader;

	// Token: 0x040039FC RID: 14844
	private static readonly ShaderHashId _StencilComparison = "_StencilComparison";

	// Token: 0x040039FD RID: 14845
	private static readonly ShaderHashId _StencilPassFront = "_StencilPassFront";

	// Token: 0x040039FE RID: 14846
	private static readonly ShaderHashId _StencilReference = "_StencilReference";

	// Token: 0x040039FF RID: 14847
	private static readonly ShaderHashId _ColorMask_ = "_ColorMask_";

	// Token: 0x04003A00 RID: 14848
	private static readonly ShaderHashId _ManualZWrite = "_ManualZWrite";

	// Token: 0x04003A01 RID: 14849
	private static readonly ShaderHashId _ZWrite = "_ZWrite";

	// Token: 0x04003A02 RID: 14850
	private static readonly int[] kRenderQueueInts = new int[]
	{
		1000,
		2000,
		2450,
		2500,
		3000,
		4000
	};
}
