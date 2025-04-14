using System;

// Token: 0x020001E5 RID: 485
public struct UberShaderMatUsedProps
{
	// Token: 0x06000B5C RID: 2908 RVA: 0x0003CF00 File Offset: 0x0003B100
	private static void _g_Macro_DECLARE_ATLASABLE_TEX2D(in GTUberShader_MaterialKeywordStates kw, ref int tex, ref int tex_Atlas)
	{
		tex += ((!kw._USE_TEX_ARRAY_ATLAS) ? 1 : 0);
		tex_Atlas += (kw._USE_TEX_ARRAY_ATLAS ? 1 : 0);
	}

	// Token: 0x06000B5D RID: 2909 RVA: 0x0003CF00 File Offset: 0x0003B100
	private static void _g_Macro_DECLARE_ATLASABLE_SAMPLER(in GTUberShader_MaterialKeywordStates kw, ref int sampler, ref int sampler_Atlas)
	{
		sampler += ((!kw._USE_TEX_ARRAY_ATLAS) ? 1 : 0);
		sampler_Atlas += (kw._USE_TEX_ARRAY_ATLAS ? 1 : 0);
	}

	// Token: 0x06000B5E RID: 2910 RVA: 0x0003CF24 File Offset: 0x0003B124
	private static void _g_Macro_SAMPLE_ATLASABLE_TEX2D(in GTUberShader_MaterialKeywordStates kw, ref int tex, ref int tex_Atlas, ref int tex_AtlasSlice, ref int sampler, ref int sampler_Atlas, ref int coord2, ref int mipBias)
	{
		tex += ((!kw._USE_TEX_ARRAY_ATLAS) ? 1 : 0);
		tex_Atlas += (kw._USE_TEX_ARRAY_ATLAS ? 1 : 0);
		tex_AtlasSlice += (kw._USE_TEX_ARRAY_ATLAS ? 1 : 0);
		sampler += ((!kw._USE_TEX_ARRAY_ATLAS) ? 1 : 0);
		sampler_Atlas += (kw._USE_TEX_ARRAY_ATLAS ? 1 : 0);
		mipBias++;
		coord2++;
	}

	// Token: 0x06000B5F RID: 2911 RVA: 0x0003CF00 File Offset: 0x0003B100
	private static void _g_Macro_SAMPLE_ATLASABLE_TEX2D_LOD(in GTUberShader_MaterialKeywordStates kw, ref int texName, ref int texName_Atlas)
	{
		texName += ((!kw._USE_TEX_ARRAY_ATLAS) ? 1 : 0);
		texName_Atlas += (kw._USE_TEX_ARRAY_ATLAS ? 1 : 0);
	}

	// Token: 0x06000B60 RID: 2912 RVA: 0x0003CF9A File Offset: 0x0003B19A
	private static void _g_Macro_SAMPLE_ATLASABLE_TEX2D_LOD(in GTUberShader_MaterialKeywordStates kw, ref int texName, ref int texName_Atlas, ref int sampler, ref int coord2, ref int lod)
	{
		texName += ((!kw._USE_TEX_ARRAY_ATLAS) ? 1 : 0);
		texName_Atlas += (kw._USE_TEX_ARRAY_ATLAS ? 1 : 0);
		sampler++;
		coord2++;
		lod++;
	}
}
