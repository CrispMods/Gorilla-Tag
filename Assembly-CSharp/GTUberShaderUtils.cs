using System;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Rendering;

// Token: 0x020008DE RID: 2270
public static class GTUberShaderUtils
{
	// Token: 0x060036A8 RID: 13992 RVA: 0x0010255A File Offset: 0x0010075A
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void SetStencilComparison(this Material m, GTShaderStencilCompare cmp)
	{
		m.SetFloat(GTUberShaderUtils._StencilComparison, (float)cmp);
	}

	// Token: 0x060036A9 RID: 13993 RVA: 0x0010256E File Offset: 0x0010076E
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void SetStencilPassFrontOp(this Material m, GTShaderStencilOp op)
	{
		m.SetFloat(GTUberShaderUtils._StencilPassFront, (float)op);
	}

	// Token: 0x060036AA RID: 13994 RVA: 0x00102582 File Offset: 0x00100782
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void SetStencilReferenceValue(this Material m, int value)
	{
		m.SetFloat(GTUberShaderUtils._StencilReference, (float)value);
	}

	// Token: 0x060036AB RID: 13995 RVA: 0x00102598 File Offset: 0x00100798
	public static void SetVisibleToXRay(this Material m, bool visible, bool saveToDisk = false)
	{
		GTShaderStencilCompare cmp = visible ? GTShaderStencilCompare.Equal : GTShaderStencilCompare.NotEqual;
		GTShaderStencilOp op = visible ? GTShaderStencilOp.Replace : GTShaderStencilOp.Keep;
		m.SetStencilComparison(cmp);
		m.SetStencilPassFrontOp(op);
		m.SetStencilReferenceValue(7);
	}

	// Token: 0x060036AC RID: 13996 RVA: 0x001025CC File Offset: 0x001007CC
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

	// Token: 0x060036AD RID: 13997 RVA: 0x00102644 File Offset: 0x00100844
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

	// Token: 0x060036AE RID: 13998 RVA: 0x00102695 File Offset: 0x00100895
	[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
	private static void InitOnLoad()
	{
		GTUberShaderUtils.kUberShader = Shader.Find("GorillaTag/UberShader");
	}

	// Token: 0x0400393A RID: 14650
	private static Shader kUberShader;

	// Token: 0x0400393B RID: 14651
	private static readonly ShaderHashId _StencilComparison = "_StencilComparison";

	// Token: 0x0400393C RID: 14652
	private static readonly ShaderHashId _StencilPassFront = "_StencilPassFront";

	// Token: 0x0400393D RID: 14653
	private static readonly ShaderHashId _StencilReference = "_StencilReference";

	// Token: 0x0400393E RID: 14654
	private static readonly ShaderHashId _ColorMask_ = "_ColorMask_";

	// Token: 0x0400393F RID: 14655
	private static readonly ShaderHashId _ManualZWrite = "_ManualZWrite";

	// Token: 0x04003940 RID: 14656
	private static readonly ShaderHashId _ZWrite = "_ZWrite";

	// Token: 0x04003941 RID: 14657
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
