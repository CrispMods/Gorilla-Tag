using System;
using GorillaExtensions;
using UnityEngine;

namespace GorillaTag.Rendering
{
	// Token: 0x02000C0C RID: 3084
	public class ZoneShaderSettings : MonoBehaviour, ITickSystemPost
	{
		// Token: 0x170007FF RID: 2047
		// (get) Token: 0x06004D1A RID: 19738 RVA: 0x00176DC7 File Offset: 0x00174FC7
		// (set) Token: 0x06004D1B RID: 19739 RVA: 0x00176DCE File Offset: 0x00174FCE
		[DebugReadout]
		public static ZoneShaderSettings defaultsInstance { get; private set; }

		// Token: 0x17000800 RID: 2048
		// (get) Token: 0x06004D1C RID: 19740 RVA: 0x00176DD6 File Offset: 0x00174FD6
		// (set) Token: 0x06004D1D RID: 19741 RVA: 0x00176DDD File Offset: 0x00174FDD
		public static bool hasDefaultsInstance { get; private set; }

		// Token: 0x17000801 RID: 2049
		// (get) Token: 0x06004D1E RID: 19742 RVA: 0x00176DE5 File Offset: 0x00174FE5
		// (set) Token: 0x06004D1F RID: 19743 RVA: 0x00176DEC File Offset: 0x00174FEC
		[DebugReadout]
		public static ZoneShaderSettings activeInstance { get; private set; }

		// Token: 0x17000802 RID: 2050
		// (get) Token: 0x06004D20 RID: 19744 RVA: 0x00176DF4 File Offset: 0x00174FF4
		// (set) Token: 0x06004D21 RID: 19745 RVA: 0x00176DFB File Offset: 0x00174FFB
		public static bool hasActiveInstance { get; private set; }

		// Token: 0x17000803 RID: 2051
		// (get) Token: 0x06004D22 RID: 19746 RVA: 0x00176E03 File Offset: 0x00175003
		public bool isActiveInstance
		{
			get
			{
				return ZoneShaderSettings.activeInstance == this;
			}
		}

		// Token: 0x17000804 RID: 2052
		// (get) Token: 0x06004D23 RID: 19747 RVA: 0x00176E10 File Offset: 0x00175010
		[DebugReadout]
		private float GroundFogDepthFadeSq
		{
			get
			{
				return 1f / Mathf.Max(1E-05f, this._groundFogDepthFadeSize * this._groundFogDepthFadeSize);
			}
		}

		// Token: 0x17000805 RID: 2053
		// (get) Token: 0x06004D24 RID: 19748 RVA: 0x00176E2F File Offset: 0x0017502F
		[DebugReadout]
		private float GroundFogHeightFade
		{
			get
			{
				return 1f / Mathf.Max(1E-05f, this._groundFogHeightFadeSize);
			}
		}

		// Token: 0x06004D25 RID: 19749 RVA: 0x00176E48 File Offset: 0x00175048
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

		// Token: 0x06004D26 RID: 19750 RVA: 0x00176EA1 File Offset: 0x001750A1
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

		// Token: 0x17000806 RID: 2054
		// (get) Token: 0x06004D27 RID: 19751 RVA: 0x00176ED5 File Offset: 0x001750D5
		// (set) Token: 0x06004D28 RID: 19752 RVA: 0x00176EDC File Offset: 0x001750DC
		public static int shaderParam_ZoneLiquidPosRadiusSq { get; private set; } = Shader.PropertyToID("_ZoneLiquidPosRadiusSq");

		// Token: 0x06004D29 RID: 19753 RVA: 0x00176EE4 File Offset: 0x001750E4
		public static float GetWaterY()
		{
			return ZoneShaderSettings.activeInstance.mainWaterSurfacePlane.position.y;
		}

		// Token: 0x06004D2A RID: 19754 RVA: 0x00176EFC File Offset: 0x001750FC
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

		// Token: 0x06004D2B RID: 19755 RVA: 0x00176F94 File Offset: 0x00175194
		protected void OnEnable()
		{
			if (this.hasDynamicWaterSurfacePlane)
			{
				TickSystem<object>.AddPostTickCallback(this);
			}
		}

		// Token: 0x06004D2C RID: 19756 RVA: 0x000C141F File Offset: 0x000BF61F
		protected void OnDisable()
		{
			TickSystem<object>.RemovePostTickCallback(this);
		}

		// Token: 0x06004D2D RID: 19757 RVA: 0x00176FA4 File Offset: 0x001751A4
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

		// Token: 0x17000807 RID: 2055
		// (get) Token: 0x06004D2E RID: 19758 RVA: 0x00176FCC File Offset: 0x001751CC
		// (set) Token: 0x06004D2F RID: 19759 RVA: 0x00176FD4 File Offset: 0x001751D4
		bool ITickSystemPost.PostTickRunning { get; set; }

		// Token: 0x06004D30 RID: 19760 RVA: 0x00176FDD File Offset: 0x001751DD
		void ITickSystemPost.PostTick()
		{
			if (ZoneShaderSettings.activeInstance == this && Application.isPlaying && !ApplicationQuittingState.IsQuitting)
			{
				this.UpdateMainPlaneShaderProperty();
			}
		}

		// Token: 0x06004D31 RID: 19761 RVA: 0x00177000 File Offset: 0x00175200
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

		// Token: 0x06004D32 RID: 19762 RVA: 0x001771BC File Offset: 0x001753BC
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
				Object.Destroy(base.gameObject);
				return;
			}
			ZoneShaderSettings.defaultsInstance = this;
			ZoneShaderSettings.hasDefaultsInstance = true;
			this.BecomeActiveInstance(false);
		}

		// Token: 0x06004D33 RID: 19763 RVA: 0x0017725D File Offset: 0x0017545D
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

		// Token: 0x06004D34 RID: 19764 RVA: 0x00177282 File Offset: 0x00175482
		public static void ActivateDefaultSettings()
		{
			if (ZoneShaderSettings.hasDefaultsInstance)
			{
				ZoneShaderSettings.defaultsInstance.BecomeActiveInstance(false);
			}
		}

		// Token: 0x06004D35 RID: 19765 RVA: 0x00177298 File Offset: 0x00175498
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

		// Token: 0x06004D36 RID: 19766 RVA: 0x001774ED File Offset: 0x001756ED
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

		// Token: 0x06004D37 RID: 19767 RVA: 0x0017751A File Offset: 0x0017571A
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

		// Token: 0x06004D38 RID: 19768 RVA: 0x0017753C File Offset: 0x0017573C
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

		// Token: 0x06004D39 RID: 19769 RVA: 0x00177568 File Offset: 0x00175768
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

		// Token: 0x06004D3A RID: 19770 RVA: 0x00177594 File Offset: 0x00175794
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

		// Token: 0x06004D3B RID: 19771 RVA: 0x001775B6 File Offset: 0x001757B6
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

		// Token: 0x04004F5B RID: 20315
		[OnEnterPlay_Set(false)]
		private static bool isInitialized;

		// Token: 0x04004F60 RID: 20320
		[Tooltip("Set this to true for cases like it is the first ZoneShaderSettings that should be activated when entering a scene.")]
		[SerializeField]
		private bool _activateOnAwake;

		// Token: 0x04004F61 RID: 20321
		[Tooltip("These values will be used as the default global values that will be fallen back to when not in a zone and that the other scripts will reference.")]
		public bool isDefaultValues;

		// Token: 0x04004F62 RID: 20322
		private static readonly int groundFogColor_shaderProp = Shader.PropertyToID("_ZoneGroundFogColor");

		// Token: 0x04004F63 RID: 20323
		[SerializeField]
		private ZoneShaderSettings.EOverrideMode groundFogColor_overrideMode;

		// Token: 0x04004F64 RID: 20324
		[SerializeField]
		private Color groundFogColor = new Color(0.7f, 0.9f, 1f, 1f);

		// Token: 0x04004F65 RID: 20325
		private static readonly int groundFogDepthFadeSq_shaderProp = Shader.PropertyToID("_ZoneGroundFogDepthFadeSq");

		// Token: 0x04004F66 RID: 20326
		[SerializeField]
		private ZoneShaderSettings.EOverrideMode groundFogDepthFade_overrideMode;

		// Token: 0x04004F67 RID: 20327
		[SerializeField]
		private float _groundFogDepthFadeSize = 20f;

		// Token: 0x04004F68 RID: 20328
		private static readonly int groundFogHeight_shaderProp = Shader.PropertyToID("_ZoneGroundFogHeight");

		// Token: 0x04004F69 RID: 20329
		[SerializeField]
		private ZoneShaderSettings.EOverrideMode groundFogHeight_overrideMode;

		// Token: 0x04004F6A RID: 20330
		[SerializeField]
		private float groundFogHeight = 7.45f;

		// Token: 0x04004F6B RID: 20331
		private static readonly int groundFogHeightFade_shaderProp = Shader.PropertyToID("_ZoneGroundFogHeightFade");

		// Token: 0x04004F6C RID: 20332
		[SerializeField]
		private ZoneShaderSettings.EOverrideMode groundFogHeightFade_overrideMode;

		// Token: 0x04004F6D RID: 20333
		[SerializeField]
		private float _groundFogHeightFadeSize = 20f;

		// Token: 0x04004F6E RID: 20334
		[SerializeField]
		private ZoneShaderSettings.EOverrideMode zoneLiquidType_overrideMode;

		// Token: 0x04004F6F RID: 20335
		[SerializeField]
		private ZoneShaderSettings.EZoneLiquidType zoneLiquidType = ZoneShaderSettings.EZoneLiquidType.Water;

		// Token: 0x04004F70 RID: 20336
		[OnEnterPlay_Set(ZoneShaderSettings.EZoneLiquidType.None)]
		private static ZoneShaderSettings.EZoneLiquidType liquidType_previousValue = ZoneShaderSettings.EZoneLiquidType.None;

		// Token: 0x04004F71 RID: 20337
		[OnEnterPlay_Set(false)]
		private static bool didEverSetLiquidShape;

		// Token: 0x04004F72 RID: 20338
		[SerializeField]
		private ZoneShaderSettings.EOverrideMode liquidShape_overrideMode;

		// Token: 0x04004F73 RID: 20339
		[SerializeField]
		private ZoneShaderSettings.ELiquidShape liquidShape;

		// Token: 0x04004F74 RID: 20340
		[OnEnterPlay_Set(ZoneShaderSettings.ELiquidShape.Plane)]
		private static ZoneShaderSettings.ELiquidShape liquidShape_previousValue = ZoneShaderSettings.ELiquidShape.Plane;

		// Token: 0x04004F76 RID: 20342
		[SerializeField]
		private ZoneShaderSettings.EOverrideMode liquidShapeRadius_overrideMode;

		// Token: 0x04004F77 RID: 20343
		[Tooltip("Fog params are: start, distance (end - start), unused, unused")]
		[SerializeField]
		private float liquidShapeRadius = 1f;

		// Token: 0x04004F78 RID: 20344
		[OnEnterPlay_Set(1f)]
		private static float liquidShapeRadius_previousValue;

		// Token: 0x04004F79 RID: 20345
		private bool hasLiquidBottomTransform;

		// Token: 0x04004F7A RID: 20346
		[SerializeField]
		private ZoneShaderSettings.EOverrideMode liquidBottomTransform_overrideMode;

		// Token: 0x04004F7B RID: 20347
		[Tooltip("TODO: remove this when there is a way to precalculate the nearest triangle plane per vertex so it will work better for rivers.")]
		[SerializeField]
		private Transform liquidBottomTransform;

		// Token: 0x04004F7C RID: 20348
		private float liquidBottomPosY_previousValue;

		// Token: 0x04004F7D RID: 20349
		private static readonly int shaderParam_GlobalZoneLiquidUVScale = Shader.PropertyToID("_GlobalZoneLiquidUVScale");

		// Token: 0x04004F7E RID: 20350
		[SerializeField]
		private ZoneShaderSettings.EOverrideMode zoneLiquidUVScale_overrideMode;

		// Token: 0x04004F7F RID: 20351
		[Tooltip("Fog params are: start, distance (end - start), unused, unused")]
		[SerializeField]
		private float zoneLiquidUVScale = 1f;

		// Token: 0x04004F80 RID: 20352
		private static readonly int shaderParam_GlobalWaterTintColor = Shader.PropertyToID("_GlobalWaterTintColor");

		// Token: 0x04004F81 RID: 20353
		[SerializeField]
		private ZoneShaderSettings.EOverrideMode underwaterTintColor_overrideMode;

		// Token: 0x04004F82 RID: 20354
		[SerializeField]
		private Color underwaterTintColor = new Color(0.3f, 0.65f, 1f, 0.2f);

		// Token: 0x04004F83 RID: 20355
		private static readonly int shaderParam_GlobalUnderwaterFogColor = Shader.PropertyToID("_GlobalUnderwaterFogColor");

		// Token: 0x04004F84 RID: 20356
		[SerializeField]
		private ZoneShaderSettings.EOverrideMode underwaterFogColor_overrideMode;

		// Token: 0x04004F85 RID: 20357
		[SerializeField]
		private Color underwaterFogColor = new Color(0.12f, 0.41f, 0.77f);

		// Token: 0x04004F86 RID: 20358
		private static readonly int shaderParam_GlobalUnderwaterFogParams = Shader.PropertyToID("_GlobalUnderwaterFogParams");

		// Token: 0x04004F87 RID: 20359
		[SerializeField]
		private ZoneShaderSettings.EOverrideMode underwaterFogParams_overrideMode;

		// Token: 0x04004F88 RID: 20360
		[Tooltip("Fog params are: start, distance (end - start), unused, unused")]
		[SerializeField]
		private Vector4 underwaterFogParams = new Vector4(-5f, 40f, 0f, 0f);

		// Token: 0x04004F89 RID: 20361
		private static readonly int shaderParam_GlobalUnderwaterCausticsParams = Shader.PropertyToID("_GlobalUnderwaterCausticsParams");

		// Token: 0x04004F8A RID: 20362
		[SerializeField]
		private ZoneShaderSettings.EOverrideMode underwaterCausticsParams_overrideMode;

		// Token: 0x04004F8B RID: 20363
		[Tooltip("Caustics params are: speed1, scale, alpha, unused")]
		[SerializeField]
		private Vector4 underwaterCausticsParams = new Vector4(0.075f, 0.075f, 1f, 0f);

		// Token: 0x04004F8C RID: 20364
		private static readonly int shaderParam_GlobalUnderwaterCausticsTex = Shader.PropertyToID("_GlobalUnderwaterCausticsTex");

		// Token: 0x04004F8D RID: 20365
		[SerializeField]
		private ZoneShaderSettings.EOverrideMode underwaterCausticsTexture_overrideMode;

		// Token: 0x04004F8E RID: 20366
		[SerializeField]
		private Texture2D underwaterCausticsTexture;

		// Token: 0x04004F8F RID: 20367
		private static readonly int shaderParam_GlobalUnderwaterEffectsDistanceToSurfaceFade = Shader.PropertyToID("_GlobalUnderwaterEffectsDistanceToSurfaceFade");

		// Token: 0x04004F90 RID: 20368
		[SerializeField]
		private ZoneShaderSettings.EOverrideMode underwaterEffectsDistanceToSurfaceFade_overrideMode;

		// Token: 0x04004F91 RID: 20369
		[SerializeField]
		private Vector2 underwaterEffectsDistanceToSurfaceFade = new Vector2(0.0001f, 50f);

		// Token: 0x04004F92 RID: 20370
		private const string kEdTooltip_liquidResidueTex = "This is used for things like the charred surface effect when lava burns static geo.";

		// Token: 0x04004F93 RID: 20371
		private static readonly int shaderParam_GlobalLiquidResidueTex = Shader.PropertyToID("_GlobalLiquidResidueTex");

		// Token: 0x04004F94 RID: 20372
		[SerializeField]
		[Tooltip("This is used for things like the charred surface effect when lava burns static geo.")]
		private ZoneShaderSettings.EOverrideMode liquidResidueTex_overrideMode;

		// Token: 0x04004F95 RID: 20373
		[SerializeField]
		[Tooltip("This is used for things like the charred surface effect when lava burns static geo.")]
		private Texture2D liquidResidueTex;

		// Token: 0x04004F96 RID: 20374
		private readonly int shaderParam_GlobalMainWaterSurfacePlane = Shader.PropertyToID("_GlobalMainWaterSurfacePlane");

		// Token: 0x04004F97 RID: 20375
		private bool hasMainWaterSurfacePlane;

		// Token: 0x04004F98 RID: 20376
		private bool hasDynamicWaterSurfacePlane;

		// Token: 0x04004F99 RID: 20377
		[SerializeField]
		private ZoneShaderSettings.EOverrideMode mainWaterSurfacePlane_overrideMode;

		// Token: 0x04004F9A RID: 20378
		[Tooltip("TODO: remove this when there is a way to precalculate the nearest triangle plane per vertex so it will work better for rivers.")]
		[SerializeField]
		private Transform mainWaterSurfacePlane;

		// Token: 0x04004F9B RID: 20379
		private static readonly int shaderParam_ZoneWeatherMapDissolveProgress = Shader.PropertyToID("_ZoneWeatherMapDissolveProgress");

		// Token: 0x04004F9C RID: 20380
		[SerializeField]
		private ZoneShaderSettings.EOverrideMode zoneWeatherMapDissolveProgress_overrideMode;

		// Token: 0x04004F9D RID: 20381
		[Tooltip("Fog params are: start, distance (end - start), unused, unused")]
		[Range(0f, 1f)]
		[SerializeField]
		private float zoneWeatherMapDissolveProgress = 1f;

		// Token: 0x02000C0D RID: 3085
		public enum EOverrideMode
		{
			// Token: 0x04004FA0 RID: 20384
			LeaveUnchanged,
			// Token: 0x04004FA1 RID: 20385
			ApplyNewValue,
			// Token: 0x04004FA2 RID: 20386
			ApplyDefaultValue
		}

		// Token: 0x02000C0E RID: 3086
		public enum EZoneLiquidType
		{
			// Token: 0x04004FA4 RID: 20388
			None,
			// Token: 0x04004FA5 RID: 20389
			Water,
			// Token: 0x04004FA6 RID: 20390
			Lava
		}

		// Token: 0x02000C0F RID: 3087
		public enum ELiquidShape
		{
			// Token: 0x04004FA8 RID: 20392
			Plane,
			// Token: 0x04004FA9 RID: 20393
			Cylinder
		}
	}
}
