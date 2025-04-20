using System;
using UnityEngine;

// Token: 0x020008FB RID: 2299
public struct GTUberShader_MaterialKeywordStates
{
	// Token: 0x06003778 RID: 14200 RVA: 0x00147A0C File Offset: 0x00145C0C
	public GTUberShader_MaterialKeywordStates(Material mat)
	{
		this.material = mat;
		this.STEREO_INSTANCING_ON = mat.IsKeywordEnabled("STEREO_INSTANCING_ON");
		this.UNITY_SINGLE_PASS_STEREO = mat.IsKeywordEnabled("UNITY_SINGLE_PASS_STEREO");
		this.STEREO_MULTIVIEW_ON = mat.IsKeywordEnabled("STEREO_MULTIVIEW_ON");
		this.STEREO_CUBEMAP_RENDER_ON = mat.IsKeywordEnabled("STEREO_CUBEMAP_RENDER_ON");
		this._GLOBAL_ZONE_LIQUID_TYPE__WATER = mat.IsKeywordEnabled("_GLOBAL_ZONE_LIQUID_TYPE__WATER");
		this._GLOBAL_ZONE_LIQUID_TYPE__LAVA = mat.IsKeywordEnabled("_GLOBAL_ZONE_LIQUID_TYPE__LAVA");
		this._ZONE_LIQUID_SHAPE__CYLINDER = mat.IsKeywordEnabled("_ZONE_LIQUID_SHAPE__CYLINDER");
		this._USE_TEXTURE = mat.IsKeywordEnabled("_USE_TEXTURE");
		this.USE_TEXTURE__AS_MASK = mat.IsKeywordEnabled("USE_TEXTURE__AS_MASK");
		this._UV_SOURCE__UV0 = mat.IsKeywordEnabled("_UV_SOURCE__UV0");
		this._UV_SOURCE__WORLD_PLANAR_Y = mat.IsKeywordEnabled("_UV_SOURCE__WORLD_PLANAR_Y");
		this._USE_VERTEX_COLOR = mat.IsKeywordEnabled("_USE_VERTEX_COLOR");
		this._USE_WEATHER_MAP = mat.IsKeywordEnabled("_USE_WEATHER_MAP");
		this._ALPHA_DETAIL_MAP = mat.IsKeywordEnabled("_ALPHA_DETAIL_MAP");
		this._HALF_LAMBERT_TERM = mat.IsKeywordEnabled("_HALF_LAMBERT_TERM");
		this._WATER_EFFECT = mat.IsKeywordEnabled("_WATER_EFFECT");
		this._HEIGHT_BASED_WATER_EFFECT = mat.IsKeywordEnabled("_HEIGHT_BASED_WATER_EFFECT");
		this._WATER_CAUSTICS = mat.IsKeywordEnabled("_WATER_CAUSTICS");
		this._ALPHATEST_ON = mat.IsKeywordEnabled("_ALPHATEST_ON");
		this._MAINTEX_ROTATE = mat.IsKeywordEnabled("_MAINTEX_ROTATE");
		this._UV_WAVE_WARP = mat.IsKeywordEnabled("_UV_WAVE_WARP");
		this._LIQUID_VOLUME = mat.IsKeywordEnabled("_LIQUID_VOLUME");
		this._LIQUID_CONTAINER = mat.IsKeywordEnabled("_LIQUID_CONTAINER");
		this._GT_RIM_LIGHT = mat.IsKeywordEnabled("_GT_RIM_LIGHT");
		this._GT_RIM_LIGHT_FLAT = mat.IsKeywordEnabled("_GT_RIM_LIGHT_FLAT");
		this._GT_RIM_LIGHT_USE_ALPHA = mat.IsKeywordEnabled("_GT_RIM_LIGHT_USE_ALPHA");
		this._SPECULAR_HIGHLIGHT = mat.IsKeywordEnabled("_SPECULAR_HIGHLIGHT");
		this._EMISSION = mat.IsKeywordEnabled("_EMISSION");
		this._EMISSION_USE_UV_WAVE_WARP = mat.IsKeywordEnabled("_EMISSION_USE_UV_WAVE_WARP");
		this._USE_DEFORM_MAP = mat.IsKeywordEnabled("_USE_DEFORM_MAP");
		this._USE_DAY_NIGHT_LIGHTMAP = mat.IsKeywordEnabled("_USE_DAY_NIGHT_LIGHTMAP");
		this._USE_TEX_ARRAY_ATLAS = mat.IsKeywordEnabled("_USE_TEX_ARRAY_ATLAS");
		this._GT_BASE_MAP_ATLAS_SLICE_SOURCE__PROPERTY = mat.IsKeywordEnabled("_GT_BASE_MAP_ATLAS_SLICE_SOURCE__PROPERTY");
		this._GT_BASE_MAP_ATLAS_SLICE_SOURCE__UV1_Z = mat.IsKeywordEnabled("_GT_BASE_MAP_ATLAS_SLICE_SOURCE__UV1_Z");
		this._CRYSTAL_EFFECT = mat.IsKeywordEnabled("_CRYSTAL_EFFECT");
		this._EYECOMP = mat.IsKeywordEnabled("_EYECOMP");
		this._MOUTHCOMP = mat.IsKeywordEnabled("_MOUTHCOMP");
		this._ALPHA_BLUE_LIVE_ON = mat.IsKeywordEnabled("_ALPHA_BLUE_LIVE_ON");
		this._GRID_EFFECT = mat.IsKeywordEnabled("_GRID_EFFECT");
		this._REFLECTIONS = mat.IsKeywordEnabled("_REFLECTIONS");
		this._REFLECTIONS_BOX_PROJECT = mat.IsKeywordEnabled("_REFLECTIONS_BOX_PROJECT");
		this._REFLECTIONS_MATCAP = mat.IsKeywordEnabled("_REFLECTIONS_MATCAP");
		this._REFLECTIONS_MATCAP_PERSP_AWARE = mat.IsKeywordEnabled("_REFLECTIONS_MATCAP_PERSP_AWARE");
		this._REFLECTIONS_ALBEDO_TINT = mat.IsKeywordEnabled("_REFLECTIONS_ALBEDO_TINT");
		this._REFLECTIONS_USE_NORMAL_TEX = mat.IsKeywordEnabled("_REFLECTIONS_USE_NORMAL_TEX");
		this._VERTEX_ROTATE = mat.IsKeywordEnabled("_VERTEX_ROTATE");
		this._VERTEX_ANIM_FLAP = mat.IsKeywordEnabled("_VERTEX_ANIM_FLAP");
		this._VERTEX_ANIM_WAVE = mat.IsKeywordEnabled("_VERTEX_ANIM_WAVE");
		this._VERTEX_ANIM_WAVE_DEBUG = mat.IsKeywordEnabled("_VERTEX_ANIM_WAVE_DEBUG");
		this._GRADIENT_MAP_ON = mat.IsKeywordEnabled("_GRADIENT_MAP_ON");
		this._PARALLAX = mat.IsKeywordEnabled("_PARALLAX");
		this._PARALLAX_AA = mat.IsKeywordEnabled("_PARALLAX_AA");
		this._PARALLAX_PLANAR = mat.IsKeywordEnabled("_PARALLAX_PLANAR");
		this._MASK_MAP_ON = mat.IsKeywordEnabled("_MASK_MAP_ON");
		this._FX_LAVA_LAMP = mat.IsKeywordEnabled("_FX_LAVA_LAMP");
		this._INNER_GLOW = mat.IsKeywordEnabled("_INNER_GLOW");
		this._STEALTH_EFFECT = mat.IsKeywordEnabled("_STEALTH_EFFECT");
		this._UV_SHIFT = mat.IsKeywordEnabled("_UV_SHIFT");
		this._TEXEL_SNAP_UVS = mat.IsKeywordEnabled("_TEXEL_SNAP_UVS");
		this._UNITY_EDIT_MODE = mat.IsKeywordEnabled("_UNITY_EDIT_MODE");
		this._GT_EDITOR_TIME = mat.IsKeywordEnabled("_GT_EDITOR_TIME");
		this._DEBUG_PAWN_DATA = mat.IsKeywordEnabled("_DEBUG_PAWN_DATA");
		this._COLOR_GRADE_PROTANOMALY = mat.IsKeywordEnabled("_COLOR_GRADE_PROTANOMALY");
		this._COLOR_GRADE_PROTANOPIA = mat.IsKeywordEnabled("_COLOR_GRADE_PROTANOPIA");
		this._COLOR_GRADE_DEUTERANOMALY = mat.IsKeywordEnabled("_COLOR_GRADE_DEUTERANOMALY");
		this._COLOR_GRADE_DEUTERANOPIA = mat.IsKeywordEnabled("_COLOR_GRADE_DEUTERANOPIA");
		this._COLOR_GRADE_TRITANOMALY = mat.IsKeywordEnabled("_COLOR_GRADE_TRITANOMALY");
		this._COLOR_GRADE_TRITANOPIA = mat.IsKeywordEnabled("_COLOR_GRADE_TRITANOPIA");
		this._COLOR_GRADE_ACHROMATOMALY = mat.IsKeywordEnabled("_COLOR_GRADE_ACHROMATOMALY");
		this._COLOR_GRADE_ACHROMATOPSIA = mat.IsKeywordEnabled("_COLOR_GRADE_ACHROMATOPSIA");
		this.LIGHTMAP_ON = mat.IsKeywordEnabled("LIGHTMAP_ON");
		this.DIRLIGHTMAP_COMBINED = mat.IsKeywordEnabled("DIRLIGHTMAP_COMBINED");
		this.INSTANCING_ON = mat.IsKeywordEnabled("INSTANCING_ON");
	}

	// Token: 0x06003779 RID: 14201 RVA: 0x00147EFC File Offset: 0x001460FC
	public void Refresh()
	{
		Material material = this.material;
		this.STEREO_INSTANCING_ON = material.IsKeywordEnabled("STEREO_INSTANCING_ON");
		this.UNITY_SINGLE_PASS_STEREO = material.IsKeywordEnabled("UNITY_SINGLE_PASS_STEREO");
		this.STEREO_MULTIVIEW_ON = material.IsKeywordEnabled("STEREO_MULTIVIEW_ON");
		this.STEREO_CUBEMAP_RENDER_ON = material.IsKeywordEnabled("STEREO_CUBEMAP_RENDER_ON");
		this._GLOBAL_ZONE_LIQUID_TYPE__WATER = material.IsKeywordEnabled("_GLOBAL_ZONE_LIQUID_TYPE__WATER");
		this._GLOBAL_ZONE_LIQUID_TYPE__LAVA = material.IsKeywordEnabled("_GLOBAL_ZONE_LIQUID_TYPE__LAVA");
		this._ZONE_LIQUID_SHAPE__CYLINDER = material.IsKeywordEnabled("_ZONE_LIQUID_SHAPE__CYLINDER");
		this._USE_TEXTURE = material.IsKeywordEnabled("_USE_TEXTURE");
		this.USE_TEXTURE__AS_MASK = material.IsKeywordEnabled("USE_TEXTURE__AS_MASK");
		this._UV_SOURCE__UV0 = material.IsKeywordEnabled("_UV_SOURCE__UV0");
		this._UV_SOURCE__WORLD_PLANAR_Y = material.IsKeywordEnabled("_UV_SOURCE__WORLD_PLANAR_Y");
		this._USE_VERTEX_COLOR = material.IsKeywordEnabled("_USE_VERTEX_COLOR");
		this._USE_WEATHER_MAP = material.IsKeywordEnabled("_USE_WEATHER_MAP");
		this._ALPHA_DETAIL_MAP = material.IsKeywordEnabled("_ALPHA_DETAIL_MAP");
		this._HALF_LAMBERT_TERM = material.IsKeywordEnabled("_HALF_LAMBERT_TERM");
		this._WATER_EFFECT = material.IsKeywordEnabled("_WATER_EFFECT");
		this._HEIGHT_BASED_WATER_EFFECT = material.IsKeywordEnabled("_HEIGHT_BASED_WATER_EFFECT");
		this._WATER_CAUSTICS = material.IsKeywordEnabled("_WATER_CAUSTICS");
		this._ALPHATEST_ON = material.IsKeywordEnabled("_ALPHATEST_ON");
		this._MAINTEX_ROTATE = material.IsKeywordEnabled("_MAINTEX_ROTATE");
		this._UV_WAVE_WARP = material.IsKeywordEnabled("_UV_WAVE_WARP");
		this._LIQUID_VOLUME = material.IsKeywordEnabled("_LIQUID_VOLUME");
		this._LIQUID_CONTAINER = material.IsKeywordEnabled("_LIQUID_CONTAINER");
		this._GT_RIM_LIGHT = material.IsKeywordEnabled("_GT_RIM_LIGHT");
		this._GT_RIM_LIGHT_FLAT = material.IsKeywordEnabled("_GT_RIM_LIGHT_FLAT");
		this._GT_RIM_LIGHT_USE_ALPHA = material.IsKeywordEnabled("_GT_RIM_LIGHT_USE_ALPHA");
		this._SPECULAR_HIGHLIGHT = material.IsKeywordEnabled("_SPECULAR_HIGHLIGHT");
		this._EMISSION = material.IsKeywordEnabled("_EMISSION");
		this._EMISSION_USE_UV_WAVE_WARP = material.IsKeywordEnabled("_EMISSION_USE_UV_WAVE_WARP");
		this._USE_DEFORM_MAP = material.IsKeywordEnabled("_USE_DEFORM_MAP");
		this._USE_DAY_NIGHT_LIGHTMAP = material.IsKeywordEnabled("_USE_DAY_NIGHT_LIGHTMAP");
		this._USE_TEX_ARRAY_ATLAS = material.IsKeywordEnabled("_USE_TEX_ARRAY_ATLAS");
		this._GT_BASE_MAP_ATLAS_SLICE_SOURCE__PROPERTY = material.IsKeywordEnabled("_GT_BASE_MAP_ATLAS_SLICE_SOURCE__PROPERTY");
		this._GT_BASE_MAP_ATLAS_SLICE_SOURCE__UV1_Z = material.IsKeywordEnabled("_GT_BASE_MAP_ATLAS_SLICE_SOURCE__UV1_Z");
		this._CRYSTAL_EFFECT = material.IsKeywordEnabled("_CRYSTAL_EFFECT");
		this._EYECOMP = material.IsKeywordEnabled("_EYECOMP");
		this._MOUTHCOMP = material.IsKeywordEnabled("_MOUTHCOMP");
		this._ALPHA_BLUE_LIVE_ON = material.IsKeywordEnabled("_ALPHA_BLUE_LIVE_ON");
		this._GRID_EFFECT = material.IsKeywordEnabled("_GRID_EFFECT");
		this._REFLECTIONS = material.IsKeywordEnabled("_REFLECTIONS");
		this._REFLECTIONS_BOX_PROJECT = material.IsKeywordEnabled("_REFLECTIONS_BOX_PROJECT");
		this._REFLECTIONS_MATCAP = material.IsKeywordEnabled("_REFLECTIONS_MATCAP");
		this._REFLECTIONS_MATCAP_PERSP_AWARE = material.IsKeywordEnabled("_REFLECTIONS_MATCAP_PERSP_AWARE");
		this._REFLECTIONS_ALBEDO_TINT = material.IsKeywordEnabled("_REFLECTIONS_ALBEDO_TINT");
		this._REFLECTIONS_USE_NORMAL_TEX = material.IsKeywordEnabled("_REFLECTIONS_USE_NORMAL_TEX");
		this._VERTEX_ROTATE = material.IsKeywordEnabled("_VERTEX_ROTATE");
		this._VERTEX_ANIM_FLAP = material.IsKeywordEnabled("_VERTEX_ANIM_FLAP");
		this._VERTEX_ANIM_WAVE = material.IsKeywordEnabled("_VERTEX_ANIM_WAVE");
		this._VERTEX_ANIM_WAVE_DEBUG = material.IsKeywordEnabled("_VERTEX_ANIM_WAVE_DEBUG");
		this._GRADIENT_MAP_ON = material.IsKeywordEnabled("_GRADIENT_MAP_ON");
		this._PARALLAX = material.IsKeywordEnabled("_PARALLAX");
		this._PARALLAX_AA = material.IsKeywordEnabled("_PARALLAX_AA");
		this._PARALLAX_PLANAR = material.IsKeywordEnabled("_PARALLAX_PLANAR");
		this._MASK_MAP_ON = material.IsKeywordEnabled("_MASK_MAP_ON");
		this._FX_LAVA_LAMP = material.IsKeywordEnabled("_FX_LAVA_LAMP");
		this._INNER_GLOW = material.IsKeywordEnabled("_INNER_GLOW");
		this._STEALTH_EFFECT = material.IsKeywordEnabled("_STEALTH_EFFECT");
		this._UV_SHIFT = material.IsKeywordEnabled("_UV_SHIFT");
		this._TEXEL_SNAP_UVS = material.IsKeywordEnabled("_TEXEL_SNAP_UVS");
		this._UNITY_EDIT_MODE = material.IsKeywordEnabled("_UNITY_EDIT_MODE");
		this._GT_EDITOR_TIME = material.IsKeywordEnabled("_GT_EDITOR_TIME");
		this._DEBUG_PAWN_DATA = material.IsKeywordEnabled("_DEBUG_PAWN_DATA");
		this._COLOR_GRADE_PROTANOMALY = material.IsKeywordEnabled("_COLOR_GRADE_PROTANOMALY");
		this._COLOR_GRADE_PROTANOPIA = material.IsKeywordEnabled("_COLOR_GRADE_PROTANOPIA");
		this._COLOR_GRADE_DEUTERANOMALY = material.IsKeywordEnabled("_COLOR_GRADE_DEUTERANOMALY");
		this._COLOR_GRADE_DEUTERANOPIA = material.IsKeywordEnabled("_COLOR_GRADE_DEUTERANOPIA");
		this._COLOR_GRADE_TRITANOMALY = material.IsKeywordEnabled("_COLOR_GRADE_TRITANOMALY");
		this._COLOR_GRADE_TRITANOPIA = material.IsKeywordEnabled("_COLOR_GRADE_TRITANOPIA");
		this._COLOR_GRADE_ACHROMATOMALY = material.IsKeywordEnabled("_COLOR_GRADE_ACHROMATOMALY");
		this._COLOR_GRADE_ACHROMATOPSIA = material.IsKeywordEnabled("_COLOR_GRADE_ACHROMATOPSIA");
		this.LIGHTMAP_ON = material.IsKeywordEnabled("LIGHTMAP_ON");
		this.DIRLIGHTMAP_COMBINED = material.IsKeywordEnabled("DIRLIGHTMAP_COMBINED");
		this.INSTANCING_ON = material.IsKeywordEnabled("INSTANCING_ON");
	}

	// Token: 0x04003A03 RID: 14851
	public Material material;

	// Token: 0x04003A04 RID: 14852
	public bool STEREO_INSTANCING_ON;

	// Token: 0x04003A05 RID: 14853
	public bool UNITY_SINGLE_PASS_STEREO;

	// Token: 0x04003A06 RID: 14854
	public bool STEREO_MULTIVIEW_ON;

	// Token: 0x04003A07 RID: 14855
	public bool STEREO_CUBEMAP_RENDER_ON;

	// Token: 0x04003A08 RID: 14856
	public bool _GLOBAL_ZONE_LIQUID_TYPE__WATER;

	// Token: 0x04003A09 RID: 14857
	public bool _GLOBAL_ZONE_LIQUID_TYPE__LAVA;

	// Token: 0x04003A0A RID: 14858
	public bool _ZONE_LIQUID_SHAPE__CYLINDER;

	// Token: 0x04003A0B RID: 14859
	public bool _USE_TEXTURE;

	// Token: 0x04003A0C RID: 14860
	public bool USE_TEXTURE__AS_MASK;

	// Token: 0x04003A0D RID: 14861
	public bool _UV_SOURCE__UV0;

	// Token: 0x04003A0E RID: 14862
	public bool _UV_SOURCE__WORLD_PLANAR_Y;

	// Token: 0x04003A0F RID: 14863
	public bool _USE_VERTEX_COLOR;

	// Token: 0x04003A10 RID: 14864
	public bool _USE_WEATHER_MAP;

	// Token: 0x04003A11 RID: 14865
	public bool _ALPHA_DETAIL_MAP;

	// Token: 0x04003A12 RID: 14866
	public bool _HALF_LAMBERT_TERM;

	// Token: 0x04003A13 RID: 14867
	public bool _WATER_EFFECT;

	// Token: 0x04003A14 RID: 14868
	public bool _HEIGHT_BASED_WATER_EFFECT;

	// Token: 0x04003A15 RID: 14869
	public bool _WATER_CAUSTICS;

	// Token: 0x04003A16 RID: 14870
	public bool _ALPHATEST_ON;

	// Token: 0x04003A17 RID: 14871
	public bool _MAINTEX_ROTATE;

	// Token: 0x04003A18 RID: 14872
	public bool _UV_WAVE_WARP;

	// Token: 0x04003A19 RID: 14873
	public bool _LIQUID_VOLUME;

	// Token: 0x04003A1A RID: 14874
	public bool _LIQUID_CONTAINER;

	// Token: 0x04003A1B RID: 14875
	public bool _GT_RIM_LIGHT;

	// Token: 0x04003A1C RID: 14876
	public bool _GT_RIM_LIGHT_FLAT;

	// Token: 0x04003A1D RID: 14877
	public bool _GT_RIM_LIGHT_USE_ALPHA;

	// Token: 0x04003A1E RID: 14878
	public bool _SPECULAR_HIGHLIGHT;

	// Token: 0x04003A1F RID: 14879
	public bool _EMISSION;

	// Token: 0x04003A20 RID: 14880
	public bool _EMISSION_USE_UV_WAVE_WARP;

	// Token: 0x04003A21 RID: 14881
	public bool _USE_DEFORM_MAP;

	// Token: 0x04003A22 RID: 14882
	public bool _USE_DAY_NIGHT_LIGHTMAP;

	// Token: 0x04003A23 RID: 14883
	public bool _USE_TEX_ARRAY_ATLAS;

	// Token: 0x04003A24 RID: 14884
	public bool _GT_BASE_MAP_ATLAS_SLICE_SOURCE__PROPERTY;

	// Token: 0x04003A25 RID: 14885
	public bool _GT_BASE_MAP_ATLAS_SLICE_SOURCE__UV1_Z;

	// Token: 0x04003A26 RID: 14886
	public bool _CRYSTAL_EFFECT;

	// Token: 0x04003A27 RID: 14887
	public bool _EYECOMP;

	// Token: 0x04003A28 RID: 14888
	public bool _MOUTHCOMP;

	// Token: 0x04003A29 RID: 14889
	public bool _ALPHA_BLUE_LIVE_ON;

	// Token: 0x04003A2A RID: 14890
	public bool _GRID_EFFECT;

	// Token: 0x04003A2B RID: 14891
	public bool _REFLECTIONS;

	// Token: 0x04003A2C RID: 14892
	public bool _REFLECTIONS_BOX_PROJECT;

	// Token: 0x04003A2D RID: 14893
	public bool _REFLECTIONS_MATCAP;

	// Token: 0x04003A2E RID: 14894
	public bool _REFLECTIONS_MATCAP_PERSP_AWARE;

	// Token: 0x04003A2F RID: 14895
	public bool _REFLECTIONS_ALBEDO_TINT;

	// Token: 0x04003A30 RID: 14896
	public bool _REFLECTIONS_USE_NORMAL_TEX;

	// Token: 0x04003A31 RID: 14897
	public bool _VERTEX_ROTATE;

	// Token: 0x04003A32 RID: 14898
	public bool _VERTEX_ANIM_FLAP;

	// Token: 0x04003A33 RID: 14899
	public bool _VERTEX_ANIM_WAVE;

	// Token: 0x04003A34 RID: 14900
	public bool _VERTEX_ANIM_WAVE_DEBUG;

	// Token: 0x04003A35 RID: 14901
	public bool _GRADIENT_MAP_ON;

	// Token: 0x04003A36 RID: 14902
	public bool _PARALLAX;

	// Token: 0x04003A37 RID: 14903
	public bool _PARALLAX_AA;

	// Token: 0x04003A38 RID: 14904
	public bool _PARALLAX_PLANAR;

	// Token: 0x04003A39 RID: 14905
	public bool _MASK_MAP_ON;

	// Token: 0x04003A3A RID: 14906
	public bool _FX_LAVA_LAMP;

	// Token: 0x04003A3B RID: 14907
	public bool _INNER_GLOW;

	// Token: 0x04003A3C RID: 14908
	public bool _STEALTH_EFFECT;

	// Token: 0x04003A3D RID: 14909
	public bool _UV_SHIFT;

	// Token: 0x04003A3E RID: 14910
	public bool _TEXEL_SNAP_UVS;

	// Token: 0x04003A3F RID: 14911
	public bool _UNITY_EDIT_MODE;

	// Token: 0x04003A40 RID: 14912
	public bool _GT_EDITOR_TIME;

	// Token: 0x04003A41 RID: 14913
	public bool _DEBUG_PAWN_DATA;

	// Token: 0x04003A42 RID: 14914
	public bool _COLOR_GRADE_PROTANOMALY;

	// Token: 0x04003A43 RID: 14915
	public bool _COLOR_GRADE_PROTANOPIA;

	// Token: 0x04003A44 RID: 14916
	public bool _COLOR_GRADE_DEUTERANOMALY;

	// Token: 0x04003A45 RID: 14917
	public bool _COLOR_GRADE_DEUTERANOPIA;

	// Token: 0x04003A46 RID: 14918
	public bool _COLOR_GRADE_TRITANOMALY;

	// Token: 0x04003A47 RID: 14919
	public bool _COLOR_GRADE_TRITANOPIA;

	// Token: 0x04003A48 RID: 14920
	public bool _COLOR_GRADE_ACHROMATOMALY;

	// Token: 0x04003A49 RID: 14921
	public bool _COLOR_GRADE_ACHROMATOPSIA;

	// Token: 0x04003A4A RID: 14922
	public bool LIGHTMAP_ON;

	// Token: 0x04003A4B RID: 14923
	public bool DIRLIGHTMAP_COMBINED;

	// Token: 0x04003A4C RID: 14924
	public bool INSTANCING_ON;
}
