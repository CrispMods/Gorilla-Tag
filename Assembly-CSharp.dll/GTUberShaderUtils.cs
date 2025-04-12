using System;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Rendering;

// Token: 0x020008E1 RID: 2273
public static class GTUberShaderUtils
{
	// Token: 0x060036B4 RID: 14004 RVA: 0x000534CA File Offset: 0x000516CA
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void SetStencilComparison(this Material m, GTShaderStencilCompare cmp)
	{
		m.SetFloat(GTUberShaderUtils._StencilComparison, (float)cmp);
	}

	// Token: 0x060036B5 RID: 14005 RVA: 0x000534DE File Offset: 0x000516DE
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void SetStencilPassFrontOp(this Material m, GTShaderStencilOp op)
	{
		m.SetFloat(GTUberShaderUtils._StencilPassFront, (float)op);
	}

	// Token: 0x060036B6 RID: 14006 RVA: 0x000534F2 File Offset: 0x000516F2
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void SetStencilReferenceValue(this Material m, int value)
	{
		m.SetFloat(GTUberShaderUtils._StencilReference, (float)value);
	}

	// Token: 0x060036B7 RID: 14007 RVA: 0x001422CC File Offset: 0x001404CC
	public static void SetVisibleToXRay(this Material m, bool visible, bool saveToDisk = false)
	{
		GTShaderStencilCompare cmp = visible ? GTShaderStencilCompare.Equal : GTShaderStencilCompare.NotEqual;
		GTShaderStencilOp op = visible ? GTShaderStencilOp.Replace : GTShaderStencilOp.Keep;
		m.SetStencilComparison(cmp);
		m.SetStencilPassFrontOp(op);
		m.SetStencilReferenceValue(7);
	}

	// Token: 0x060036B8 RID: 14008 RVA: 0x00142300 File Offset: 0x00140500
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

	// Token: 0x060036B9 RID: 14009 RVA: 0x00142378 File Offset: 0x00140578
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

	// Token: 0x060036BA RID: 14010 RVA: 0x00053506 File Offset: 0x00051706
	[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
	private static void InitOnLoad()
	{
		GTUberShaderUtils.kUberShader = Shader.Find("GorillaTag/UberShader");
	}

	// Token: 0x0400394C RID: 14668
	private static Shader kUberShader;

	// Token: 0x0400394D RID: 14669
	private static readonly ShaderHashId _StencilComparison = "_StencilComparison";

	// Token: 0x0400394E RID: 14670
	private static readonly ShaderHashId _StencilPassFront = "_StencilPassFront";

	// Token: 0x0400394F RID: 14671
	private static readonly ShaderHashId _StencilReference = "_StencilReference";

	// Token: 0x04003950 RID: 14672
	private static readonly ShaderHashId _ColorMask_ = "_ColorMask_";

	// Token: 0x04003951 RID: 14673
	private static readonly ShaderHashId _ManualZWrite = "_ManualZWrite";

	// Token: 0x04003952 RID: 14674
	private static readonly ShaderHashId _ZWrite = "_ZWrite";

	// Token: 0x04003953 RID: 14675
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
