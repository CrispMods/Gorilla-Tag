using System;
using GorillaExtensions;
using GT_CustomMapSupportRuntime;
using UnityEngine;

namespace GorillaTag.Rendering
{
	// Token: 0x02000C3A RID: 3130
	public class ZoneShaderSettings : MonoBehaviour, ITickSystemPost
	{
		// Token: 0x1700081D RID: 2077
		// (get) Token: 0x06004E66 RID: 20070 RVA: 0x0006346F File Offset: 0x0006166F
		// (set) Token: 0x06004E67 RID: 20071 RVA: 0x00063476 File Offset: 0x00061676
		[DebugReadout]
		public static ZoneShaderSettings defaultsInstance { get; private set; }

		// Token: 0x1700081E RID: 2078
		// (get) Token: 0x06004E68 RID: 20072 RVA: 0x0006347E File Offset: 0x0006167E
		// (set) Token: 0x06004E69 RID: 20073 RVA: 0x00063485 File Offset: 0x00061685
		public static bool hasDefaultsInstance { get; private set; }

		// Token: 0x1700081F RID: 2079
		// (get) Token: 0x06004E6A RID: 20074 RVA: 0x0006348D File Offset: 0x0006168D
		// (set) Token: 0x06004E6B RID: 20075 RVA: 0x00063494 File Offset: 0x00061694
		[DebugReadout]
		public static ZoneShaderSettings activeInstance { get; private set; }

		// Token: 0x17000820 RID: 2080
		// (get) Token: 0x06004E6C RID: 20076 RVA: 0x0006349C File Offset: 0x0006169C
		// (set) Token: 0x06004E6D RID: 20077 RVA: 0x000634A3 File Offset: 0x000616A3
		public static bool hasActiveInstance { get; private set; }

		// Token: 0x17000821 RID: 2081
		// (get) Token: 0x06004E6E RID: 20078 RVA: 0x000634AB File Offset: 0x000616AB
		public bool isActiveInstance
		{
			get
			{
				return ZoneShaderSettings.activeInstance == this;
			}
		}

		// Token: 0x17000822 RID: 2082
		// (get) Token: 0x06004E6F RID: 20079 RVA: 0x000634B8 File Offset: 0x000616B8
		[DebugReadout]
		private float GroundFogDepthFadeSq
		{
			get
			{
				return 1f / Mathf.Max(1E-05f, this._groundFogDepthFadeSize * this._groundFogDepthFadeSize);
			}
		}

		// Token: 0x17000823 RID: 2083
		// (get) Token: 0x06004E70 RID: 20080 RVA: 0x000634D7 File Offset: 0x000616D7
		[DebugReadout]
		private float GroundFogHeightFade
		{
			get
			{
				return 1f / Mathf.Max(1E-05f, this._groundFogHeightFadeSize);
			}
		}

		// Token: 0x06004E71 RID: 20081 RVA: 0x001AF034 File Offset: 0x001AD234
		public void SetZoneLiquidTypeKeywordEnum(ZoneShaderSettings.EZoneLiquidType liquidType)
		{
			if (liquidType == ZoneShaderSettings.EZoneLiquidType.None)
			{
				Shader.EnableKeyword("_GLOBAL_ZONE_LIQUID_TYPE__NONE");
			}
			else
			{
				Shader.DisableKeyword("_GLOBAL_ZONE_LIQUID_TYPE__NONE");
			}
			if (liquidType == ZoneShaderSettings.EZoneLiquidType.Water)
			{
				Shader.EnableKeyword("_GLOBAL_ZONE_LIQUID_TYPE__WATER");
			}
			else
			{
				Shader.DisableKeyword("_GLOBAL_ZONE_LIQUID_TYPE__WATER");
			}
			if (liquidType == ZoneShaderSettings.EZoneLiquidType.Lava)
			{
				Shader.EnableKeyword("_GLOBAL_ZONE_LIQUID_TYPE__LAVA");
				return;
			}
			Shader.DisableKeyword("_GLOBAL_ZONE_LIQUID_TYPE__LAVA");
		}

		// Token: 0x06004E72 RID: 20082 RVA: 0x000634EF File Offset: 0x000616EF
		public void SetZoneLiquidShapeKeywordEnum(ZoneShaderSettings.ELiquidShape shape)
		{
			if (shape == ZoneShaderSettings.ELiquidShape.Plane)
			{
				Shader.EnableKeyword("_ZONE_LIQUID_SHAPE__PLANE");
			}
			else
			{
				Shader.DisableKeyword("_ZONE_LIQUID_SHAPE__PLANE");
			}
			if (shape == ZoneShaderSettings.ELiquidShape.Cylinder)
			{
				Shader.EnableKeyword("_ZONE_LIQUID_SHAPE__CYLINDER");
				return;
			}
			Shader.DisableKeyword("_ZONE_LIQUID_SHAPE__CYLINDER");
		}

		// Token: 0x17000824 RID: 2084
		// (get) Token: 0x06004E73 RID: 20083 RVA: 0x00063523 File Offset: 0x00061723
		// (set) Token: 0x06004E74 RID: 20084 RVA: 0x0006352A File Offset: 0x0006172A
		public static int shaderParam_ZoneLiquidPosRadiusSq { get; private set; } = Shader.PropertyToID("_ZoneLiquidPosRadiusSq");

		// Token: 0x06004E75 RID: 20085 RVA: 0x00063532 File Offset: 0x00061732
		public static float GetWaterY()
		{
			return ZoneShaderSettings.activeInstance.mainWaterSurfacePlane.position.y;
		}

		// Token: 0x06004E76 RID: 20086 RVA: 0x001AF090 File Offset: 0x001AD290
		protected void Awake()
		{
			this.hasMainWaterSurfacePlane = (this.mainWaterSurfacePlane != null && (this.mainWaterSurfacePlane_overrideMode == ZoneShaderSettings.EOverrideMode.ApplyNewValue || this.isDefaultValues));
			this.hasDynamicWaterSurfacePlane = (this.hasMainWaterSurfacePlane && !this.mainWaterSurfacePlane.gameObject.isStatic);
			this.hasLiquidBottomTransform = (this.liquidBottomTransform != null && (this.liquidBottomTransform_overrideMode == ZoneShaderSettings.EOverrideMode.ApplyNewValue || this.isDefaultValues));
			this.CheckDefaultsInstance();
			if (this._activateOnAwake)
			{
				this.BecomeActiveInstance(false);
			}
		}

		// Token: 0x06004E77 RID: 20087 RVA: 0x00063548 File Offset: 0x00061748
		protected void OnEnable()
		{
			if (this.hasDynamicWaterSurfacePlane)
			{
				TickSystem<object>.AddPostTickCallback(this);
			}
		}

		// Token: 0x06004E78 RID: 20088 RVA: 0x0004A56E File Offset: 0x0004876E
		protected void OnDisable()
		{
			TickSystem<object>.RemovePostTickCallback(this);
		}

		// Token: 0x06004E79 RID: 20089 RVA: 0x00063558 File Offset: 0x00061758
		protected void OnDestroy()
		{
			if (ZoneShaderSettings.defaultsInstance == this)
			{
				ZoneShaderSettings.hasDefaultsInstance = false;
			}
			if (ZoneShaderSettings.activeInstance == this)
			{
				ZoneShaderSettings.hasActiveInstance = false;
			}
		}

		// Token: 0x17000825 RID: 2085
		// (get) Token: 0x06004E7A RID: 20090 RVA: 0x00063580 File Offset: 0x00061780
		// (set) Token: 0x06004E7B RID: 20091 RVA: 0x00063588 File Offset: 0x00061788
		bool ITickSystemPost.PostTickRunning { get; set; }

		// Token: 0x06004E7C RID: 20092 RVA: 0x00063591 File Offset: 0x00061791
		void ITickSystemPost.PostTick()
		{
			if (ZoneShaderSettings.activeInstance == this && Application.isPlaying && !ApplicationQuittingState.IsQuitting)
			{
				this.UpdateMainPlaneShaderProperty();
			}
		}

		// Token: 0x06004E7D RID: 20093 RVA: 0x001AF128 File Offset: 0x001AD328
		private void UpdateMainPlaneShaderProperty()
		{
			Transform transform = null;
			bool flag = false;
			if (this.hasMainWaterSurfacePlane && (this.mainWaterSurfacePlane_overrideMode == ZoneShaderSettings.EOverrideMode.ApplyNewValue || this.isDefaultValues))
			{
				flag = true;
				transform = this.mainWaterSurfacePlane;
			}
			else if (this.mainWaterSurfacePlane_overrideMode == ZoneShaderSettings.EOverrideMode.ApplyDefaultValue && ZoneShaderSettings.hasDefaultsInstance && ZoneShaderSettings.defaultsInstance.hasMainWaterSurfacePlane)
			{
				flag = true;
				transform = ZoneShaderSettings.defaultsInstance.mainWaterSurfacePlane;
			}
			if (!flag)
			{
				return;
			}
			Vector3 position = transform.position;
			Vector3 up = transform.up;
			float w = -Vector3.Dot(up, position);
			Shader.SetGlobalVector(this.shaderParam_GlobalMainWaterSurfacePlane, new Vector4(up.x, up.y, up.z, w));
			ZoneShaderSettings.ELiquidShape eliquidShape;
			if (this.liquidShape_overrideMode == ZoneShaderSettings.EOverrideMode.ApplyNewValue || this.isDefaultValues)
			{
				eliquidShape = this.liquidShape;
			}
			else if (this.liquidShape_overrideMode == ZoneShaderSettings.EOverrideMode.ApplyDefaultValue && ZoneShaderSettings.hasDefaultsInstance)
			{
				eliquidShape = ZoneShaderSettings.defaultsInstance.liquidShape;
			}
			else
			{
				eliquidShape = ZoneShaderSettings.liquidShape_previousValue;
			}
			ZoneShaderSettings.liquidShape_previousValue = eliquidShape;
			float y;
			if ((this.liquidBottomTransform_overrideMode == ZoneShaderSettings.EOverrideMode.ApplyNewValue || this.isDefaultValues) && this.hasLiquidBottomTransform)
			{
				y = this.liquidBottomTransform.position.y;
			}
			else if (this.liquidBottomTransform_overrideMode == ZoneShaderSettings.EOverrideMode.ApplyDefaultValue && ZoneShaderSettings.hasDefaultsInstance && ZoneShaderSettings.defaultsInstance.hasLiquidBottomTransform)
			{
				y = ZoneShaderSettings.defaultsInstance.liquidBottomTransform.position.y;
			}
			else
			{
				y = this.liquidBottomPosY_previousValue;
			}
			float num;
			if (this.liquidShapeRadius_overrideMode == ZoneShaderSettings.EOverrideMode.ApplyNewValue || this.isDefaultValues)
			{
				num = this.liquidShapeRadius;
			}
			else if (this.liquidShape_overrideMode == ZoneShaderSettings.EOverrideMode.ApplyDefaultValue && ZoneShaderSettings.hasDefaultsInstance)
			{
				num = ZoneShaderSettings.defaultsInstance.liquidShapeRadius;
			}
			else
			{
				num = ZoneShaderSettings.liquidShapeRadius_previousValue;
			}
			if (eliquidShape == ZoneShaderSettings.ELiquidShape.Cylinder)
			{
				Shader.SetGlobalVector(ZoneShaderSettings.shaderParam_ZoneLiquidPosRadiusSq, new Vector4(position.x, y, position.z, num * num));
				ZoneShaderSettings.liquidShapeRadius_previousValue = num;
			}
		}

		// Token: 0x06004E7E RID: 20094 RVA: 0x001AF2E4 File Offset: 0x001AD4E4
		private void CheckDefaultsInstance()
		{
			if (!this.isDefaultValues)
			{
				return;
			}
			if (ZoneShaderSettings.hasDefaultsInstance && ZoneShaderSettings.defaultsInstance != null && ZoneShaderSettings.defaultsInstance != this)
			{
				string path = ZoneShaderSettings.defaultsInstance.transform.GetPath();
				Debug.LogError(string.Concat(new string[]
				{
					"ZoneShaderSettings: Destroying conflicting defaults instance.\n- keeping: \"",
					path,
					"\"\n- destroying (this): \"",
					base.transform.GetPath(),
					"\""
				}), this);
				UnityEngine.Object.Destroy(base.gameObject);
				return;
			}
			ZoneShaderSettings.defaultsInstance = this;
			ZoneShaderSettings.hasDefaultsInstance = true;
			this.BecomeActiveInstance(false);
		}

		// Token: 0x06004E7F RID: 20095 RVA: 0x000635B4 File Offset: 0x000617B4
		public void BecomeActiveInstance(bool force = false)
		{
			if (ZoneShaderSettings.activeInstance == this && !force)
			{
				return;
			}
			this.ApplyValues();
			ZoneShaderSettings.activeInstance = this;
			ZoneShaderSettings.hasActiveInstance = true;
		}

		// Token: 0x06004E80 RID: 20096 RVA: 0x000635D9 File Offset: 0x000617D9
		public static void ActivateDefaultSettings()
		{
			if (ZoneShaderSettings.hasDefaultsInstance)
			{
				ZoneShaderSettings.defaultsInstance.BecomeActiveInstance(false);
			}
		}

		// Token: 0x06004E81 RID: 20097 RVA: 0x001AF388 File Offset: 0x001AD588
		public void SetGroundFogValue(Color fogColor, float fogDepthFade, float fogHeight, float fogHeightFade)
		{
			this.groundFogColor_overrideMode = ZoneShaderSettings.EOverrideMode.ApplyNewValue;
			this.groundFogColor = fogColor;
			this.groundFogDepthFade_overrideMode = ZoneShaderSettings.EOverrideMode.ApplyNewValue;
			this._groundFogDepthFadeSize = fogDepthFade;
			this.groundFogHeight_overrideMode = ZoneShaderSettings.EOverrideMode.ApplyNewValue;
			this.groundFogHeight = fogHeight;
			this.groundFogHeightFade_overrideMode = ZoneShaderSettings.EOverrideMode.ApplyNewValue;
			this._groundFogHeightFadeSize = fogHeightFade;
			this.BecomeActiveInstance(true);
		}

		// Token: 0x06004E82 RID: 20098 RVA: 0x001AF3D8 File Offset: 0x001AD5D8
		private void ApplyValues()
		{
			if (!ZoneShaderSettings.hasDefaultsInstance || ApplicationQuittingState.IsQuitting)
			{
				return;
			}
			this.ApplyColor(ZoneShaderSettings.groundFogColor_shaderProp, this.groundFogColor_overrideMode, this.groundFogColor, ZoneShaderSettings.defaultsInstance.groundFogColor);
			this.ApplyFloat(ZoneShaderSettings.groundFogDepthFadeSq_shaderProp, this.groundFogDepthFade_overrideMode, this.GroundFogDepthFadeSq, ZoneShaderSettings.defaultsInstance.GroundFogDepthFadeSq);
			this.ApplyFloat(ZoneShaderSettings.groundFogHeight_shaderProp, this.groundFogHeight_overrideMode, this.groundFogHeight, ZoneShaderSettings.defaultsInstance.groundFogHeight);
			this.ApplyFloat(ZoneShaderSettings.groundFogHeightFade_shaderProp, this.groundFogHeightFade_overrideMode, this.GroundFogHeightFade, ZoneShaderSettings.defaultsInstance.GroundFogHeightFade);
			if (this.zoneLiquidType_overrideMode != ZoneShaderSettings.EOverrideMode.LeaveUnchanged)
			{
				ZoneShaderSettings.EZoneLiquidType ezoneLiquidType = (this.zoneLiquidType_overrideMode == ZoneShaderSettings.EOverrideMode.ApplyNewValue) ? this.zoneLiquidType : ZoneShaderSettings.defaultsInstance.zoneLiquidType;
				if (ezoneLiquidType != ZoneShaderSettings.liquidType_previousValue || !ZoneShaderSettings.isInitialized)
				{
					this.SetZoneLiquidTypeKeywordEnum(ezoneLiquidType);
					ZoneShaderSettings.liquidType_previousValue = ezoneLiquidType;
				}
			}
			if (this.liquidShape_overrideMode != ZoneShaderSettings.EOverrideMode.LeaveUnchanged)
			{
				ZoneShaderSettings.ELiquidShape eliquidShape = (this.liquidShape_overrideMode == ZoneShaderSettings.EOverrideMode.ApplyNewValue) ? this.liquidShape : ZoneShaderSettings.defaultsInstance.liquidShape;
				if (eliquidShape != ZoneShaderSettings.liquidShape_previousValue || !ZoneShaderSettings.isInitialized)
				{
					this.SetZoneLiquidShapeKeywordEnum(eliquidShape);
					ZoneShaderSettings.liquidShape_previousValue = eliquidShape;
				}
			}
			this.ApplyFloat(ZoneShaderSettings.shaderParam_GlobalZoneLiquidUVScale, this.zoneLiquidUVScale_overrideMode, this.zoneLiquidUVScale, ZoneShaderSettings.defaultsInstance.zoneLiquidUVScale);
			this.ApplyColor(ZoneShaderSettings.shaderParam_GlobalWaterTintColor, this.underwaterTintColor_overrideMode, this.underwaterTintColor, ZoneShaderSettings.defaultsInstance.underwaterTintColor);
			this.ApplyColor(ZoneShaderSettings.shaderParam_GlobalUnderwaterFogColor, this.underwaterFogColor_overrideMode, this.underwaterFogColor, ZoneShaderSettings.defaultsInstance.underwaterFogColor);
			this.ApplyVector(ZoneShaderSettings.shaderParam_GlobalUnderwaterFogParams, this.underwaterFogParams_overrideMode, this.underwaterFogParams, ZoneShaderSettings.defaultsInstance.underwaterFogParams);
			this.ApplyVector(ZoneShaderSettings.shaderParam_GlobalUnderwaterCausticsParams, this.underwaterCausticsParams_overrideMode, this.underwaterCausticsParams, ZoneShaderSettings.defaultsInstance.underwaterCausticsParams);
			this.ApplyTexture(ZoneShaderSettings.shaderParam_GlobalUnderwaterCausticsTex, this.underwaterCausticsTexture_overrideMode, this.underwaterCausticsTexture, ZoneShaderSettings.defaultsInstance.underwaterCausticsTexture);
			this.ApplyVector(ZoneShaderSettings.shaderParam_GlobalUnderwaterEffectsDistanceToSurfaceFade, this.underwaterEffectsDistanceToSurfaceFade_overrideMode, this.underwaterEffectsDistanceToSurfaceFade, ZoneShaderSettings.defaultsInstance.underwaterEffectsDistanceToSurfaceFade);
			this.ApplyTexture(ZoneShaderSettings.shaderParam_GlobalLiquidResidueTex, this.liquidResidueTex_overrideMode, this.liquidResidueTex, ZoneShaderSettings.defaultsInstance.liquidResidueTex);
			this.ApplyFloat(ZoneShaderSettings.shaderParam_ZoneWeatherMapDissolveProgress, this.zoneWeatherMapDissolveProgress_overrideMode, this.zoneWeatherMapDissolveProgress, ZoneShaderSettings.defaultsInstance.zoneWeatherMapDissolveProgress);
			this.UpdateMainPlaneShaderProperty();
			ZoneShaderSettings.isInitialized = true;
		}

		// Token: 0x06004E83 RID: 20099 RVA: 0x000635ED File Offset: 0x000617ED
		private void ApplyColor(int shaderProp, ZoneShaderSettings.EOverrideMode overrideMode, Color value, Color defaultValue)
		{
			if (overrideMode == ZoneShaderSettings.EOverrideMode.ApplyNewValue || this.isDefaultValues)
			{
				Shader.SetGlobalColor(shaderProp, value.linear);
				return;
			}
			if (overrideMode == ZoneShaderSettings.EOverrideMode.ApplyDefaultValue)
			{
				Shader.SetGlobalColor(shaderProp, defaultValue.linear);
			}
		}

		// Token: 0x06004E84 RID: 20100 RVA: 0x0006361A File Offset: 0x0006181A
		private void ApplyFloat(int shaderProp, ZoneShaderSettings.EOverrideMode overrideMode, float value, float defaultValue)
		{
			if (overrideMode == ZoneShaderSettings.EOverrideMode.ApplyNewValue || this.isDefaultValues)
			{
				Shader.SetGlobalFloat(shaderProp, value);
				return;
			}
			if (overrideMode == ZoneShaderSettings.EOverrideMode.ApplyDefaultValue)
			{
				Shader.SetGlobalFloat(shaderProp, defaultValue);
			}
		}

		// Token: 0x06004E85 RID: 20101 RVA: 0x0006363C File Offset: 0x0006183C
		private void ApplyVector(int shaderProp, ZoneShaderSettings.EOverrideMode overrideMode, Vector2 value, Vector2 defaultValue)
		{
			if (overrideMode == ZoneShaderSettings.EOverrideMode.ApplyNewValue || this.isDefaultValues)
			{
				Shader.SetGlobalVector(shaderProp, value);
				return;
			}
			if (overrideMode == ZoneShaderSettings.EOverrideMode.ApplyDefaultValue)
			{
				Shader.SetGlobalVector(shaderProp, defaultValue);
			}
		}

		// Token: 0x06004E86 RID: 20102 RVA: 0x00063668 File Offset: 0x00061868
		private void ApplyVector(int shaderProp, ZoneShaderSettings.EOverrideMode overrideMode, Vector3 value, Vector3 defaultValue)
		{
			if (overrideMode == ZoneShaderSettings.EOverrideMode.ApplyNewValue || this.isDefaultValues)
			{
				Shader.SetGlobalVector(shaderProp, value);
				return;
			}
			if (overrideMode == ZoneShaderSettings.EOverrideMode.ApplyDefaultValue)
			{
				Shader.SetGlobalVector(shaderProp, defaultValue);
			}
		}

		// Token: 0x06004E87 RID: 20103 RVA: 0x00063694 File Offset: 0x00061894
		private void ApplyVector(int shaderProp, ZoneShaderSettings.EOverrideMode overrideMode, Vector4 value, Vector4 defaultValue)
		{
			if (overrideMode == ZoneShaderSettings.EOverrideMode.ApplyNewValue || this.isDefaultValues)
			{
				Shader.SetGlobalVector(shaderProp, value);
				return;
			}
			if (overrideMode == ZoneShaderSettings.EOverrideMode.ApplyDefaultValue)
			{
				Shader.SetGlobalVector(shaderProp, defaultValue);
			}
		}

		// Token: 0x06004E88 RID: 20104 RVA: 0x000636B6 File Offset: 0x000618B6
		private void ApplyTexture(int shaderProp, ZoneShaderSettings.EOverrideMode overrideMode, Texture2D value, Texture2D defaultValue)
		{
			if (overrideMode == ZoneShaderSettings.EOverrideMode.ApplyNewValue || this.isDefaultValues)
			{
				Shader.SetGlobalTexture(shaderProp, value);
				return;
			}
			if (overrideMode == ZoneShaderSettings.EOverrideMode.ApplyDefaultValue)
			{
				Shader.SetGlobalTexture(shaderProp, defaultValue);
			}
		}

		// Token: 0x06004E89 RID: 20105 RVA: 0x001AF630 File Offset: 0x001AD830
		public void CopySettings(CMSZoneShaderSettings cmsZoneShaderSettings, bool rerunAwake = false, bool becomeActive = false)
		{
			this._activateOnAwake = cmsZoneShaderSettings.activateOnLoad;
			if (cmsZoneShaderSettings.applyGroundFog)
			{
				this.groundFogColor_overrideMode = (ZoneShaderSettings.EOverrideMode)cmsZoneShaderSettings.GetGroundFogColorOverrideMode();
				this.groundFogColor = cmsZoneShaderSettings.groundFogColor;
				this.groundFogHeight_overrideMode = (ZoneShaderSettings.EOverrideMode)cmsZoneShaderSettings.GetGroundFogHeightOverrideMode();
				if (cmsZoneShaderSettings.groundFogHeightPlane.IsNotNull())
				{
					this.groundFogHeight = cmsZoneShaderSettings.groundFogHeightPlane.position.y;
				}
				else
				{
					this.groundFogHeight = cmsZoneShaderSettings.groundFogHeight;
				}
				this.groundFogHeightFade_overrideMode = (ZoneShaderSettings.EOverrideMode)cmsZoneShaderSettings.GetGroundFogHeightFadeOverrideMode();
				this._groundFogHeightFadeSize = cmsZoneShaderSettings.groundFogHeightFadeSize;
				this.groundFogDepthFade_overrideMode = (ZoneShaderSettings.EOverrideMode)cmsZoneShaderSettings.GetGroundFogDepthFadeOverrideMode();
				this._groundFogDepthFadeSize = cmsZoneShaderSettings.groundFogDepthFadeSize;
			}
			else
			{
				this.groundFogColor_overrideMode = ZoneShaderSettings.EOverrideMode.ApplyNewValue;
				this.groundFogColor = new Color(0f, 0f, 0f, 0f);
				this.groundFogHeight = -9999f;
			}
			if (cmsZoneShaderSettings.applyLiquidEffects)
			{
				this.zoneLiquidType_overrideMode = (ZoneShaderSettings.EOverrideMode)cmsZoneShaderSettings.GetZoneLiquidTypeOverrideMode();
				this.zoneLiquidType = (ZoneShaderSettings.EZoneLiquidType)cmsZoneShaderSettings.GetZoneLiquidType();
				this.liquidShape_overrideMode = (ZoneShaderSettings.EOverrideMode)cmsZoneShaderSettings.GetLiquidShapeOverrideMode();
				this.liquidShape = (ZoneShaderSettings.ELiquidShape)cmsZoneShaderSettings.GetZoneLiquidShape();
				this.liquidShapeRadius_overrideMode = (ZoneShaderSettings.EOverrideMode)cmsZoneShaderSettings.GetLiquidShapeRadiusOverrideMode();
				this.liquidShapeRadius = cmsZoneShaderSettings.liquidShapeRadius;
				this.liquidBottomTransform_overrideMode = (ZoneShaderSettings.EOverrideMode)cmsZoneShaderSettings.GetLiquidBottomTransformOverrideMode();
				this.liquidBottomTransform = cmsZoneShaderSettings.liquidBottomTransform;
				this.zoneLiquidUVScale_overrideMode = (ZoneShaderSettings.EOverrideMode)cmsZoneShaderSettings.GetZoneLiquidUVScaleOverrideMode();
				this.zoneLiquidUVScale = cmsZoneShaderSettings.zoneLiquidUVScale;
				this.underwaterTintColor_overrideMode = (ZoneShaderSettings.EOverrideMode)cmsZoneShaderSettings.GetUnderwaterTintColorOverrideMode();
				this.underwaterTintColor = cmsZoneShaderSettings.underwaterTintColor;
				this.underwaterFogColor_overrideMode = (ZoneShaderSettings.EOverrideMode)cmsZoneShaderSettings.GetUnderwaterFogColorOverrideMode();
				this.underwaterFogColor = cmsZoneShaderSettings.underwaterFogColor;
				this.underwaterFogParams_overrideMode = (ZoneShaderSettings.EOverrideMode)cmsZoneShaderSettings.GetUnderwaterFogParamsOverrideMode();
				this.underwaterFogParams = cmsZoneShaderSettings.underwaterFogParams;
				this.underwaterCausticsParams_overrideMode = (ZoneShaderSettings.EOverrideMode)cmsZoneShaderSettings.GetUnderwaterCausticsParamsOverrideMode();
				this.underwaterCausticsParams = cmsZoneShaderSettings.underwaterCausticsParams;
				this.underwaterCausticsTexture_overrideMode = (ZoneShaderSettings.EOverrideMode)cmsZoneShaderSettings.GetUnderwaterCausticsTextureOverrideMode();
				this.underwaterCausticsTexture = cmsZoneShaderSettings.underwaterCausticsTexture;
				this.underwaterEffectsDistanceToSurfaceFade_overrideMode = (ZoneShaderSettings.EOverrideMode)cmsZoneShaderSettings.GetUnderwaterEffectsDistanceToSurfaceFadeOverrideMode();
				this.underwaterEffectsDistanceToSurfaceFade = cmsZoneShaderSettings.underwaterEffectsDistanceToSurfaceFade;
				this.liquidResidueTex_overrideMode = (ZoneShaderSettings.EOverrideMode)cmsZoneShaderSettings.GetLiquidResidueTextureOverrideMode();
				this.liquidResidueTex = cmsZoneShaderSettings.liquidResidueTex;
				this.mainWaterSurfacePlane_overrideMode = (ZoneShaderSettings.EOverrideMode)cmsZoneShaderSettings.GetMainWaterSurfacePlaneOverrideMode();
				this.mainWaterSurfacePlane = cmsZoneShaderSettings.mainWaterSurfacePlane;
			}
			else
			{
				this.underwaterTintColor_overrideMode = ZoneShaderSettings.EOverrideMode.ApplyNewValue;
				this.underwaterTintColor = new Color(0f, 0f, 0f, 0f);
				this.underwaterFogColor_overrideMode = ZoneShaderSettings.EOverrideMode.ApplyNewValue;
				this.underwaterFogColor = new Color(0f, 0f, 0f, 0f);
				this.mainWaterSurfacePlane_overrideMode = ZoneShaderSettings.EOverrideMode.ApplyNewValue;
				Transform transform = base.gameObject.transform.Find("DummyWaterPlane");
				GameObject gameObject;
				if (transform != null)
				{
					gameObject = transform.gameObject;
				}
				else
				{
					gameObject = new GameObject("DummyWaterPlane");
					gameObject.transform.SetParent(base.gameObject.transform);
					gameObject.transform.rotation = Quaternion.identity;
					gameObject.transform.position = new Vector3(0f, -9999f, 0f);
				}
				this.mainWaterSurfacePlane = gameObject.transform;
			}
			this.zoneWeatherMapDissolveProgress_overrideMode = ZoneShaderSettings.EOverrideMode.LeaveUnchanged;
			if (rerunAwake)
			{
				this.Awake();
			}
			if (becomeActive)
			{
				this.BecomeActiveInstance(true);
			}
		}

		// Token: 0x06004E8A RID: 20106 RVA: 0x001AF948 File Offset: 0x001ADB48
		public void CopySettings(ZoneShaderSettings zoneShaderSettings, bool rerunAwake = false, bool becomeActive = false)
		{
			this.groundFogColor_overrideMode = zoneShaderSettings.groundFogColor_overrideMode;
			this.groundFogColor = zoneShaderSettings.groundFogColor;
			this.groundFogHeight_overrideMode = zoneShaderSettings.groundFogHeight_overrideMode;
			this.groundFogHeight = zoneShaderSettings.groundFogHeight;
			this.groundFogHeightFade_overrideMode = zoneShaderSettings.groundFogHeightFade_overrideMode;
			this._groundFogHeightFadeSize = zoneShaderSettings._groundFogHeightFadeSize;
			this.groundFogDepthFade_overrideMode = zoneShaderSettings.groundFogDepthFade_overrideMode;
			this._groundFogDepthFadeSize = zoneShaderSettings._groundFogDepthFadeSize;
			this.zoneLiquidType_overrideMode = zoneShaderSettings.zoneLiquidType_overrideMode;
			this.zoneLiquidType = zoneShaderSettings.zoneLiquidType;
			this.liquidShape_overrideMode = zoneShaderSettings.liquidShape_overrideMode;
			this.liquidShape = zoneShaderSettings.liquidShape;
			this.liquidShapeRadius_overrideMode = zoneShaderSettings.liquidShapeRadius_overrideMode;
			this.liquidShapeRadius = zoneShaderSettings.liquidShapeRadius;
			this.liquidBottomTransform_overrideMode = zoneShaderSettings.liquidBottomTransform_overrideMode;
			this.liquidBottomTransform = zoneShaderSettings.liquidBottomTransform;
			this.zoneLiquidUVScale_overrideMode = zoneShaderSettings.zoneLiquidUVScale_overrideMode;
			this.zoneLiquidUVScale = zoneShaderSettings.zoneLiquidUVScale;
			this.underwaterTintColor_overrideMode = zoneShaderSettings.underwaterTintColor_overrideMode;
			this.underwaterTintColor = zoneShaderSettings.underwaterTintColor;
			this.underwaterFogColor_overrideMode = zoneShaderSettings.underwaterFogColor_overrideMode;
			this.underwaterFogColor = zoneShaderSettings.underwaterFogColor;
			this.underwaterFogParams_overrideMode = zoneShaderSettings.underwaterFogParams_overrideMode;
			this.underwaterFogParams = zoneShaderSettings.underwaterFogParams;
			this.underwaterCausticsParams_overrideMode = zoneShaderSettings.underwaterCausticsParams_overrideMode;
			this.underwaterCausticsParams = zoneShaderSettings.underwaterCausticsParams;
			this.underwaterCausticsTexture_overrideMode = zoneShaderSettings.underwaterCausticsTexture_overrideMode;
			this.underwaterCausticsTexture = zoneShaderSettings.underwaterCausticsTexture;
			this.underwaterEffectsDistanceToSurfaceFade_overrideMode = zoneShaderSettings.underwaterEffectsDistanceToSurfaceFade_overrideMode;
			this.underwaterEffectsDistanceToSurfaceFade = zoneShaderSettings.underwaterEffectsDistanceToSurfaceFade;
			this.liquidResidueTex_overrideMode = zoneShaderSettings.liquidResidueTex_overrideMode;
			this.liquidResidueTex = zoneShaderSettings.liquidResidueTex;
			this.mainWaterSurfacePlane_overrideMode = zoneShaderSettings.mainWaterSurfacePlane_overrideMode;
			this.mainWaterSurfacePlane = zoneShaderSettings.mainWaterSurfacePlane;
			this.zoneWeatherMapDissolveProgress_overrideMode = zoneShaderSettings.zoneWeatherMapDissolveProgress_overrideMode;
			this.zoneWeatherMapDissolveProgress = zoneShaderSettings.zoneWeatherMapDissolveProgress;
			if (rerunAwake)
			{
				this.Awake();
			}
			if (becomeActive)
			{
				this.BecomeActiveInstance(true);
			}
		}

		// Token: 0x06004E8B RID: 20107 RVA: 0x001AFB18 File Offset: 0x001ADD18
		public void ReplaceDefaultValues(ZoneShaderSettings defaultZoneShaderSettings, bool rerunAwake = false)
		{
			if (this.groundFogColor_overrideMode == ZoneShaderSettings.EOverrideMode.ApplyDefaultValue)
			{
				this.groundFogColor_overrideMode = ZoneShaderSettings.EOverrideMode.ApplyNewValue;
				this.groundFogColor = defaultZoneShaderSettings.groundFogColor;
			}
			if (this.groundFogHeight_overrideMode == ZoneShaderSettings.EOverrideMode.ApplyDefaultValue)
			{
				this.groundFogHeight_overrideMode = ZoneShaderSettings.EOverrideMode.ApplyNewValue;
				this.groundFogHeight = defaultZoneShaderSettings.groundFogHeight;
			}
			if (this.groundFogHeightFade_overrideMode == ZoneShaderSettings.EOverrideMode.ApplyDefaultValue)
			{
				this.groundFogHeightFade_overrideMode = ZoneShaderSettings.EOverrideMode.ApplyNewValue;
				this._groundFogHeightFadeSize = defaultZoneShaderSettings._groundFogHeightFadeSize;
			}
			if (this.groundFogDepthFade_overrideMode == ZoneShaderSettings.EOverrideMode.ApplyDefaultValue)
			{
				this.groundFogDepthFade_overrideMode = ZoneShaderSettings.EOverrideMode.ApplyNewValue;
				this._groundFogDepthFadeSize = defaultZoneShaderSettings._groundFogDepthFadeSize;
			}
			if (this.zoneLiquidType_overrideMode == ZoneShaderSettings.EOverrideMode.ApplyDefaultValue)
			{
				this.zoneLiquidType_overrideMode = ZoneShaderSettings.EOverrideMode.ApplyNewValue;
				this.zoneLiquidType = defaultZoneShaderSettings.zoneLiquidType;
			}
			if (this.liquidShape_overrideMode == ZoneShaderSettings.EOverrideMode.ApplyDefaultValue)
			{
				this.liquidShape_overrideMode = ZoneShaderSettings.EOverrideMode.ApplyNewValue;
				this.liquidShape = defaultZoneShaderSettings.liquidShape;
			}
			if (this.liquidShapeRadius_overrideMode == ZoneShaderSettings.EOverrideMode.ApplyDefaultValue)
			{
				this.liquidShapeRadius_overrideMode = ZoneShaderSettings.EOverrideMode.ApplyNewValue;
				this.liquidShapeRadius = defaultZoneShaderSettings.liquidShapeRadius;
			}
			if (this.liquidBottomTransform_overrideMode == ZoneShaderSettings.EOverrideMode.ApplyDefaultValue)
			{
				this.liquidBottomTransform_overrideMode = ZoneShaderSettings.EOverrideMode.ApplyNewValue;
				this.liquidBottomTransform = defaultZoneShaderSettings.liquidBottomTransform;
			}
			if (this.zoneLiquidUVScale_overrideMode == ZoneShaderSettings.EOverrideMode.ApplyDefaultValue)
			{
				this.zoneLiquidUVScale_overrideMode = ZoneShaderSettings.EOverrideMode.ApplyNewValue;
				this.zoneLiquidUVScale = defaultZoneShaderSettings.zoneLiquidUVScale;
			}
			if (this.underwaterTintColor_overrideMode == ZoneShaderSettings.EOverrideMode.ApplyDefaultValue)
			{
				this.underwaterTintColor_overrideMode = ZoneShaderSettings.EOverrideMode.ApplyNewValue;
				this.underwaterTintColor = defaultZoneShaderSettings.underwaterTintColor;
			}
			if (this.underwaterFogColor_overrideMode == ZoneShaderSettings.EOverrideMode.ApplyDefaultValue)
			{
				this.underwaterFogColor_overrideMode = ZoneShaderSettings.EOverrideMode.ApplyNewValue;
				this.underwaterFogColor = defaultZoneShaderSettings.underwaterFogColor;
			}
			if (this.underwaterFogParams_overrideMode == ZoneShaderSettings.EOverrideMode.ApplyDefaultValue)
			{
				this.underwaterFogParams_overrideMode = ZoneShaderSettings.EOverrideMode.ApplyNewValue;
				this.underwaterFogParams = defaultZoneShaderSettings.underwaterFogParams;
			}
			if (this.underwaterCausticsParams_overrideMode == ZoneShaderSettings.EOverrideMode.ApplyDefaultValue)
			{
				this.underwaterCausticsParams_overrideMode = ZoneShaderSettings.EOverrideMode.ApplyNewValue;
				this.underwaterCausticsParams = defaultZoneShaderSettings.underwaterCausticsParams;
			}
			if (this.underwaterCausticsTexture_overrideMode == ZoneShaderSettings.EOverrideMode.ApplyDefaultValue)
			{
				this.underwaterCausticsTexture_overrideMode = ZoneShaderSettings.EOverrideMode.ApplyNewValue;
				this.underwaterCausticsTexture = defaultZoneShaderSettings.underwaterCausticsTexture;
			}
			if (this.underwaterEffectsDistanceToSurfaceFade_overrideMode == ZoneShaderSettings.EOverrideMode.ApplyDefaultValue)
			{
				this.underwaterEffectsDistanceToSurfaceFade_overrideMode = ZoneShaderSettings.EOverrideMode.ApplyNewValue;
				this.underwaterEffectsDistanceToSurfaceFade = defaultZoneShaderSettings.underwaterEffectsDistanceToSurfaceFade;
			}
			if (this.liquidResidueTex_overrideMode == ZoneShaderSettings.EOverrideMode.ApplyDefaultValue)
			{
				this.liquidResidueTex_overrideMode = ZoneShaderSettings.EOverrideMode.ApplyNewValue;
				this.liquidResidueTex = defaultZoneShaderSettings.liquidResidueTex;
			}
			if (this.mainWaterSurfacePlane_overrideMode == ZoneShaderSettings.EOverrideMode.ApplyDefaultValue)
			{
				this.mainWaterSurfacePlane_overrideMode = ZoneShaderSettings.EOverrideMode.ApplyNewValue;
				this.mainWaterSurfacePlane = defaultZoneShaderSettings.mainWaterSurfacePlane;
			}
			if (rerunAwake)
			{
				this.Awake();
			}
		}

		// Token: 0x06004E8C RID: 20108 RVA: 0x001AFD0C File Offset: 0x001ADF0C
		public void ReplaceDefaultValues(CMSZoneShaderSettings.CMSZoneShaderProperties defaultZoneShaderProperties, bool rerunAwake = false)
		{
			if (this.groundFogColor_overrideMode == ZoneShaderSettings.EOverrideMode.ApplyDefaultValue)
			{
				this.groundFogColor_overrideMode = ZoneShaderSettings.EOverrideMode.ApplyNewValue;
				this.groundFogColor = defaultZoneShaderProperties.groundFogColor;
			}
			if (this.groundFogHeight_overrideMode == ZoneShaderSettings.EOverrideMode.ApplyDefaultValue)
			{
				this.groundFogHeight_overrideMode = ZoneShaderSettings.EOverrideMode.ApplyNewValue;
				this.groundFogHeight = defaultZoneShaderProperties.groundFogHeight;
			}
			if (this.groundFogHeightFade_overrideMode == ZoneShaderSettings.EOverrideMode.ApplyDefaultValue)
			{
				this.groundFogHeightFade_overrideMode = ZoneShaderSettings.EOverrideMode.ApplyNewValue;
				this._groundFogHeightFadeSize = defaultZoneShaderProperties.groundFogHeightFadeSize;
			}
			if (this.groundFogDepthFade_overrideMode == ZoneShaderSettings.EOverrideMode.ApplyDefaultValue)
			{
				this.groundFogDepthFade_overrideMode = ZoneShaderSettings.EOverrideMode.ApplyNewValue;
				this._groundFogDepthFadeSize = defaultZoneShaderProperties.groundFogDepthFadeSize;
			}
			if (this.zoneLiquidType_overrideMode == ZoneShaderSettings.EOverrideMode.ApplyDefaultValue)
			{
				this.zoneLiquidType_overrideMode = ZoneShaderSettings.EOverrideMode.ApplyNewValue;
				this.zoneLiquidType = (ZoneShaderSettings.EZoneLiquidType)defaultZoneShaderProperties.zoneLiquidType;
			}
			if (this.liquidShape_overrideMode == ZoneShaderSettings.EOverrideMode.ApplyDefaultValue)
			{
				this.liquidShape_overrideMode = ZoneShaderSettings.EOverrideMode.ApplyNewValue;
				this.liquidShape = (ZoneShaderSettings.ELiquidShape)defaultZoneShaderProperties.liquidShape;
			}
			if (this.liquidShapeRadius_overrideMode == ZoneShaderSettings.EOverrideMode.ApplyDefaultValue)
			{
				this.liquidShapeRadius_overrideMode = ZoneShaderSettings.EOverrideMode.ApplyNewValue;
				this.liquidShapeRadius = defaultZoneShaderProperties.liquidShapeRadius;
			}
			if (this.liquidBottomTransform_overrideMode == ZoneShaderSettings.EOverrideMode.ApplyDefaultValue)
			{
				this.liquidBottomTransform_overrideMode = ZoneShaderSettings.EOverrideMode.ApplyNewValue;
				this.liquidBottomTransform = defaultZoneShaderProperties.liquidBottomTransform;
			}
			if (this.zoneLiquidUVScale_overrideMode == ZoneShaderSettings.EOverrideMode.ApplyDefaultValue)
			{
				this.zoneLiquidUVScale_overrideMode = ZoneShaderSettings.EOverrideMode.ApplyNewValue;
				this.zoneLiquidUVScale = defaultZoneShaderProperties.zoneLiquidUVScale;
			}
			if (this.underwaterTintColor_overrideMode == ZoneShaderSettings.EOverrideMode.ApplyDefaultValue)
			{
				this.underwaterTintColor_overrideMode = ZoneShaderSettings.EOverrideMode.ApplyNewValue;
				this.underwaterTintColor = defaultZoneShaderProperties.underwaterTintColor;
			}
			if (this.underwaterFogColor_overrideMode == ZoneShaderSettings.EOverrideMode.ApplyDefaultValue)
			{
				this.underwaterFogColor_overrideMode = ZoneShaderSettings.EOverrideMode.ApplyNewValue;
				this.underwaterFogColor = defaultZoneShaderProperties.underwaterFogColor;
			}
			if (this.underwaterFogParams_overrideMode == ZoneShaderSettings.EOverrideMode.ApplyDefaultValue)
			{
				this.underwaterFogParams_overrideMode = ZoneShaderSettings.EOverrideMode.ApplyNewValue;
				this.underwaterFogParams = defaultZoneShaderProperties.underwaterFogParams;
			}
			if (this.underwaterCausticsParams_overrideMode == ZoneShaderSettings.EOverrideMode.ApplyDefaultValue)
			{
				this.underwaterCausticsParams_overrideMode = ZoneShaderSettings.EOverrideMode.ApplyNewValue;
				this.underwaterCausticsParams = defaultZoneShaderProperties.underwaterCausticsParams;
			}
			if (this.underwaterCausticsTexture_overrideMode == ZoneShaderSettings.EOverrideMode.ApplyDefaultValue)
			{
				this.underwaterCausticsTexture_overrideMode = ZoneShaderSettings.EOverrideMode.ApplyNewValue;
				this.underwaterCausticsTexture = defaultZoneShaderProperties.underwaterCausticsTexture;
			}
			if (this.underwaterEffectsDistanceToSurfaceFade_overrideMode == ZoneShaderSettings.EOverrideMode.ApplyDefaultValue)
			{
				this.underwaterEffectsDistanceToSurfaceFade_overrideMode = ZoneShaderSettings.EOverrideMode.ApplyNewValue;
				this.underwaterEffectsDistanceToSurfaceFade = defaultZoneShaderProperties.underwaterEffectsDistanceToSurfaceFade;
			}
			if (this.liquidResidueTex_overrideMode == ZoneShaderSettings.EOverrideMode.ApplyDefaultValue)
			{
				this.liquidResidueTex_overrideMode = ZoneShaderSettings.EOverrideMode.ApplyNewValue;
				this.liquidResidueTex = defaultZoneShaderProperties.liquidResidueTex;
			}
			if (this.mainWaterSurfacePlane_overrideMode == ZoneShaderSettings.EOverrideMode.ApplyDefaultValue)
			{
				this.mainWaterSurfacePlane_overrideMode = ZoneShaderSettings.EOverrideMode.ApplyNewValue;
				this.mainWaterSurfacePlane = defaultZoneShaderProperties.mainWaterSurfacePlane;
			}
			if (rerunAwake)
			{
				this.Awake();
			}
		}

		// Token: 0x04005051 RID: 20561
		[OnEnterPlay_Set(false)]
		private static bool isInitialized;

		// Token: 0x04005056 RID: 20566
		[Tooltip("Set this to true for cases like it is the first ZoneShaderSettings that should be activated when entering a scene.")]
		[SerializeField]
		private bool _activateOnAwake;

		// Token: 0x04005057 RID: 20567
		[Tooltip("These values will be used as the default global values that will be fallen back to when not in a zone and that the other scripts will reference.")]
		public bool isDefaultValues;

		// Token: 0x04005058 RID: 20568
		private static readonly int groundFogColor_shaderProp = Shader.PropertyToID("_ZoneGroundFogColor");

		// Token: 0x04005059 RID: 20569
		[SerializeField]
		private ZoneShaderSettings.EOverrideMode groundFogColor_overrideMode;

		// Token: 0x0400505A RID: 20570
		[SerializeField]
		private Color groundFogColor = new Color(0.7f, 0.9f, 1f, 1f);

		// Token: 0x0400505B RID: 20571
		private static readonly int groundFogDepthFadeSq_shaderProp = Shader.PropertyToID("_ZoneGroundFogDepthFadeSq");

		// Token: 0x0400505C RID: 20572
		[SerializeField]
		private ZoneShaderSettings.EOverrideMode groundFogDepthFade_overrideMode;

		// Token: 0x0400505D RID: 20573
		[SerializeField]
		private float _groundFogDepthFadeSize = 20f;

		// Token: 0x0400505E RID: 20574
		private static readonly int groundFogHeight_shaderProp = Shader.PropertyToID("_ZoneGroundFogHeight");

		// Token: 0x0400505F RID: 20575
		[SerializeField]
		private ZoneShaderSettings.EOverrideMode groundFogHeight_overrideMode;

		// Token: 0x04005060 RID: 20576
		[SerializeField]
		private float groundFogHeight = 7.45f;

		// Token: 0x04005061 RID: 20577
		private static readonly int groundFogHeightFade_shaderProp = Shader.PropertyToID("_ZoneGroundFogHeightFade");

		// Token: 0x04005062 RID: 20578
		[SerializeField]
		private ZoneShaderSettings.EOverrideMode groundFogHeightFade_overrideMode;

		// Token: 0x04005063 RID: 20579
		[SerializeField]
		private float _groundFogHeightFadeSize = 20f;

		// Token: 0x04005064 RID: 20580
		[SerializeField]
		private ZoneShaderSettings.EOverrideMode zoneLiquidType_overrideMode;

		// Token: 0x04005065 RID: 20581
		[SerializeField]
		private ZoneShaderSettings.EZoneLiquidType zoneLiquidType = ZoneShaderSettings.EZoneLiquidType.Water;

		// Token: 0x04005066 RID: 20582
		[OnEnterPlay_Set(ZoneShaderSettings.EZoneLiquidType.None)]
		private static ZoneShaderSettings.EZoneLiquidType liquidType_previousValue = ZoneShaderSettings.EZoneLiquidType.None;

		// Token: 0x04005067 RID: 20583
		[OnEnterPlay_Set(false)]
		private static bool didEverSetLiquidShape;

		// Token: 0x04005068 RID: 20584
		[SerializeField]
		private ZoneShaderSettings.EOverrideMode liquidShape_overrideMode;

		// Token: 0x04005069 RID: 20585
		[SerializeField]
		private ZoneShaderSettings.ELiquidShape liquidShape;

		// Token: 0x0400506A RID: 20586
		[OnEnterPlay_Set(ZoneShaderSettings.ELiquidShape.Plane)]
		private static ZoneShaderSettings.ELiquidShape liquidShape_previousValue = ZoneShaderSettings.ELiquidShape.Plane;

		// Token: 0x0400506C RID: 20588
		[SerializeField]
		private ZoneShaderSettings.EOverrideMode liquidShapeRadius_overrideMode;

		// Token: 0x0400506D RID: 20589
		[Tooltip("Fog params are: start, distance (end - start), unused, unused")]
		[SerializeField]
		private float liquidShapeRadius = 1f;

		// Token: 0x0400506E RID: 20590
		[OnEnterPlay_Set(1f)]
		private static float liquidShapeRadius_previousValue;

		// Token: 0x0400506F RID: 20591
		private bool hasLiquidBottomTransform;

		// Token: 0x04005070 RID: 20592
		[SerializeField]
		private ZoneShaderSettings.EOverrideMode liquidBottomTransform_overrideMode;

		// Token: 0x04005071 RID: 20593
		[Tooltip("TODO: remove this when there is a way to precalculate the nearest triangle plane per vertex so it will work better for rivers.")]
		[SerializeField]
		private Transform liquidBottomTransform;

		// Token: 0x04005072 RID: 20594
		private float liquidBottomPosY_previousValue;

		// Token: 0x04005073 RID: 20595
		private static readonly int shaderParam_GlobalZoneLiquidUVScale = Shader.PropertyToID("_GlobalZoneLiquidUVScale");

		// Token: 0x04005074 RID: 20596
		[SerializeField]
		private ZoneShaderSettings.EOverrideMode zoneLiquidUVScale_overrideMode;

		// Token: 0x04005075 RID: 20597
		[Tooltip("Fog params are: start, distance (end - start), unused, unused")]
		[SerializeField]
		private float zoneLiquidUVScale = 1f;

		// Token: 0x04005076 RID: 20598
		private static readonly int shaderParam_GlobalWaterTintColor = Shader.PropertyToID("_GlobalWaterTintColor");

		// Token: 0x04005077 RID: 20599
		[SerializeField]
		private ZoneShaderSettings.EOverrideMode underwaterTintColor_overrideMode;

		// Token: 0x04005078 RID: 20600
		[SerializeField]
		private Color underwaterTintColor = new Color(0.3f, 0.65f, 1f, 0.2f);

		// Token: 0x04005079 RID: 20601
		private static readonly int shaderParam_GlobalUnderwaterFogColor = Shader.PropertyToID("_GlobalUnderwaterFogColor");

		// Token: 0x0400507A RID: 20602
		[SerializeField]
		private ZoneShaderSettings.EOverrideMode underwaterFogColor_overrideMode;

		// Token: 0x0400507B RID: 20603
		[SerializeField]
		private Color underwaterFogColor = new Color(0.12f, 0.41f, 0.77f);

		// Token: 0x0400507C RID: 20604
		private static readonly int shaderParam_GlobalUnderwaterFogParams = Shader.PropertyToID("_GlobalUnderwaterFogParams");

		// Token: 0x0400507D RID: 20605
		[SerializeField]
		private ZoneShaderSettings.EOverrideMode underwaterFogParams_overrideMode;

		// Token: 0x0400507E RID: 20606
		[Tooltip("Fog params are: start, distance (end - start), unused, unused")]
		[SerializeField]
		private Vector4 underwaterFogParams = new Vector4(-5f, 40f, 0f, 0f);

		// Token: 0x0400507F RID: 20607
		private static readonly int shaderParam_GlobalUnderwaterCausticsParams = Shader.PropertyToID("_GlobalUnderwaterCausticsParams");

		// Token: 0x04005080 RID: 20608
		[SerializeField]
		private ZoneShaderSettings.EOverrideMode underwaterCausticsParams_overrideMode;

		// Token: 0x04005081 RID: 20609
		[Tooltip("Caustics params are: speed1, scale, alpha, unused")]
		[SerializeField]
		private Vector4 underwaterCausticsParams = new Vector4(0.075f, 0.075f, 1f, 0f);

		// Token: 0x04005082 RID: 20610
		private static readonly int shaderParam_GlobalUnderwaterCausticsTex = Shader.PropertyToID("_GlobalUnderwaterCausticsTex");

		// Token: 0x04005083 RID: 20611
		[SerializeField]
		private ZoneShaderSettings.EOverrideMode underwaterCausticsTexture_overrideMode;

		// Token: 0x04005084 RID: 20612
		[SerializeField]
		private Texture2D underwaterCausticsTexture;

		// Token: 0x04005085 RID: 20613
		private static readonly int shaderParam_GlobalUnderwaterEffectsDistanceToSurfaceFade = Shader.PropertyToID("_GlobalUnderwaterEffectsDistanceToSurfaceFade");

		// Token: 0x04005086 RID: 20614
		[SerializeField]
		private ZoneShaderSettings.EOverrideMode underwaterEffectsDistanceToSurfaceFade_overrideMode;

		// Token: 0x04005087 RID: 20615
		[SerializeField]
		private Vector2 underwaterEffectsDistanceToSurfaceFade = new Vector2(0.0001f, 50f);

		// Token: 0x04005088 RID: 20616
		private const string kEdTooltip_liquidResidueTex = "This is used for things like the charred surface effect when lava burns static geo.";

		// Token: 0x04005089 RID: 20617
		private static readonly int shaderParam_GlobalLiquidResidueTex = Shader.PropertyToID("_GlobalLiquidResidueTex");

		// Token: 0x0400508A RID: 20618
		[SerializeField]
		[Tooltip("This is used for things like the charred surface effect when lava burns static geo.")]
		private ZoneShaderSettings.EOverrideMode liquidResidueTex_overrideMode;

		// Token: 0x0400508B RID: 20619
		[SerializeField]
		[Tooltip("This is used for things like the charred surface effect when lava burns static geo.")]
		private Texture2D liquidResidueTex;

		// Token: 0x0400508C RID: 20620
		private readonly int shaderParam_GlobalMainWaterSurfacePlane = Shader.PropertyToID("_GlobalMainWaterSurfacePlane");

		// Token: 0x0400508D RID: 20621
		private bool hasMainWaterSurfacePlane;

		// Token: 0x0400508E RID: 20622
		private bool hasDynamicWaterSurfacePlane;

		// Token: 0x0400508F RID: 20623
		[SerializeField]
		private ZoneShaderSettings.EOverrideMode mainWaterSurfacePlane_overrideMode;

		// Token: 0x04005090 RID: 20624
		[Tooltip("TODO: remove this when there is a way to precalculate the nearest triangle plane per vertex so it will work better for rivers.")]
		[SerializeField]
		private Transform mainWaterSurfacePlane;

		// Token: 0x04005091 RID: 20625
		private static readonly int shaderParam_ZoneWeatherMapDissolveProgress = Shader.PropertyToID("_ZoneWeatherMapDissolveProgress");

		// Token: 0x04005092 RID: 20626
		[SerializeField]
		private ZoneShaderSettings.EOverrideMode zoneWeatherMapDissolveProgress_overrideMode;

		// Token: 0x04005093 RID: 20627
		[Tooltip("Fog params are: start, distance (end - start), unused, unused")]
		[Range(0f, 1f)]
		[SerializeField]
		private float zoneWeatherMapDissolveProgress = 1f;

		// Token: 0x02000C3B RID: 3131
		public enum EOverrideMode
		{
			// Token: 0x04005096 RID: 20630
			LeaveUnchanged,
			// Token: 0x04005097 RID: 20631
			ApplyNewValue,
			// Token: 0x04005098 RID: 20632
			ApplyDefaultValue
		}

		// Token: 0x02000C3C RID: 3132
		public enum EZoneLiquidType
		{
			// Token: 0x0400509A RID: 20634
			None,
			// Token: 0x0400509B RID: 20635
			Water,
			// Token: 0x0400509C RID: 20636
			Lava
		}

		// Token: 0x02000C3D RID: 3133
		public enum ELiquidShape
		{
			// Token: 0x0400509E RID: 20638
			Plane,
			// Token: 0x0400509F RID: 20639
			Cylinder
		}
	}
}
