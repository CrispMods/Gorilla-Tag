using System;
using GorillaExtensions;
using UnityEngine;

namespace GorillaTag.Rendering
{
	// Token: 0x02000C0F RID: 3087
	public class ZoneShaderSettings : MonoBehaviour, ITickSystemPost
	{
		// Token: 0x17000800 RID: 2048
		// (get) Token: 0x06004D26 RID: 19750 RVA: 0x00061AAE File Offset: 0x0005FCAE
		// (set) Token: 0x06004D27 RID: 19751 RVA: 0x00061AB5 File Offset: 0x0005FCB5
		[DebugReadout]
		public static ZoneShaderSettings defaultsInstance { get; private set; }

		// Token: 0x17000801 RID: 2049
		// (get) Token: 0x06004D28 RID: 19752 RVA: 0x00061ABD File Offset: 0x0005FCBD
		// (set) Token: 0x06004D29 RID: 19753 RVA: 0x00061AC4 File Offset: 0x0005FCC4
		public static bool hasDefaultsInstance { get; private set; }

		// Token: 0x17000802 RID: 2050
		// (get) Token: 0x06004D2A RID: 19754 RVA: 0x00061ACC File Offset: 0x0005FCCC
		// (set) Token: 0x06004D2B RID: 19755 RVA: 0x00061AD3 File Offset: 0x0005FCD3
		[DebugReadout]
		public static ZoneShaderSettings activeInstance { get; private set; }

		// Token: 0x17000803 RID: 2051
		// (get) Token: 0x06004D2C RID: 19756 RVA: 0x00061ADB File Offset: 0x0005FCDB
		// (set) Token: 0x06004D2D RID: 19757 RVA: 0x00061AE2 File Offset: 0x0005FCE2
		public static bool hasActiveInstance { get; private set; }

		// Token: 0x17000804 RID: 2052
		// (get) Token: 0x06004D2E RID: 19758 RVA: 0x00061AEA File Offset: 0x0005FCEA
		public bool isActiveInstance
		{
			get
			{
				return ZoneShaderSettings.activeInstance == this;
			}
		}

		// Token: 0x17000805 RID: 2053
		// (get) Token: 0x06004D2F RID: 19759 RVA: 0x00061AF7 File Offset: 0x0005FCF7
		[DebugReadout]
		private float GroundFogDepthFadeSq
		{
			get
			{
				return 1f / Mathf.Max(1E-05f, this._groundFogDepthFadeSize * this._groundFogDepthFadeSize);
			}
		}

		// Token: 0x17000806 RID: 2054
		// (get) Token: 0x06004D30 RID: 19760 RVA: 0x00061B16 File Offset: 0x0005FD16
		[DebugReadout]
		private float GroundFogHeightFade
		{
			get
			{
				return 1f / Mathf.Max(1E-05f, this._groundFogHeightFadeSize);
			}
		}

		// Token: 0x06004D31 RID: 19761 RVA: 0x001A8068 File Offset: 0x001A6268
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

		// Token: 0x06004D32 RID: 19762 RVA: 0x00061B2E File Offset: 0x0005FD2E
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

		// Token: 0x17000807 RID: 2055
		// (get) Token: 0x06004D33 RID: 19763 RVA: 0x00061B62 File Offset: 0x0005FD62
		// (set) Token: 0x06004D34 RID: 19764 RVA: 0x00061B69 File Offset: 0x0005FD69
		public static int shaderParam_ZoneLiquidPosRadiusSq { get; private set; } = Shader.PropertyToID("_ZoneLiquidPosRadiusSq");

		// Token: 0x06004D35 RID: 19765 RVA: 0x00061B71 File Offset: 0x0005FD71
		public static float GetWaterY()
		{
			return ZoneShaderSettings.activeInstance.mainWaterSurfacePlane.position.y;
		}

		// Token: 0x06004D36 RID: 19766 RVA: 0x001A80C4 File Offset: 0x001A62C4
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

		// Token: 0x06004D37 RID: 19767 RVA: 0x00061B87 File Offset: 0x0005FD87
		protected void OnEnable()
		{
			if (this.hasDynamicWaterSurfacePlane)
			{
				TickSystem<object>.AddPostTickCallback(this);
			}
		}

		// Token: 0x06004D38 RID: 19768 RVA: 0x00049FD9 File Offset: 0x000481D9
		protected void OnDisable()
		{
			TickSystem<object>.RemovePostTickCallback(this);
		}

		// Token: 0x06004D39 RID: 19769 RVA: 0x00061B97 File Offset: 0x0005FD97
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

		// Token: 0x17000808 RID: 2056
		// (get) Token: 0x06004D3A RID: 19770 RVA: 0x00061BBF File Offset: 0x0005FDBF
		// (set) Token: 0x06004D3B RID: 19771 RVA: 0x00061BC7 File Offset: 0x0005FDC7
		bool ITickSystemPost.PostTickRunning { get; set; }

		// Token: 0x06004D3C RID: 19772 RVA: 0x00061BD0 File Offset: 0x0005FDD0
		void ITickSystemPost.PostTick()
		{
			if (ZoneShaderSettings.activeInstance == this && Application.isPlaying && !ApplicationQuittingState.IsQuitting)
			{
				this.UpdateMainPlaneShaderProperty();
			}
		}

		// Token: 0x06004D3D RID: 19773 RVA: 0x001A815C File Offset: 0x001A635C
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

		// Token: 0x06004D3E RID: 19774 RVA: 0x001A8318 File Offset: 0x001A6518
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

		// Token: 0x06004D3F RID: 19775 RVA: 0x00061BF3 File Offset: 0x0005FDF3
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

		// Token: 0x06004D40 RID: 19776 RVA: 0x00061C18 File Offset: 0x0005FE18
		public static void ActivateDefaultSettings()
		{
			if (ZoneShaderSettings.hasDefaultsInstance)
			{
				ZoneShaderSettings.defaultsInstance.BecomeActiveInstance(false);
			}
		}

		// Token: 0x06004D41 RID: 19777 RVA: 0x001A83BC File Offset: 0x001A65BC
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

		// Token: 0x06004D42 RID: 19778 RVA: 0x00061C2C File Offset: 0x0005FE2C
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

		// Token: 0x06004D43 RID: 19779 RVA: 0x00061C59 File Offset: 0x0005FE59
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

		// Token: 0x06004D44 RID: 19780 RVA: 0x00061C7B File Offset: 0x0005FE7B
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

		// Token: 0x06004D45 RID: 19781 RVA: 0x00061CA7 File Offset: 0x0005FEA7
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

		// Token: 0x06004D46 RID: 19782 RVA: 0x00061CD3 File Offset: 0x0005FED3
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

		// Token: 0x06004D47 RID: 19783 RVA: 0x00061CF5 File Offset: 0x0005FEF5
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

		// Token: 0x04004F6D RID: 20333
		[OnEnterPlay_Set(false)]
		private static bool isInitialized;

		// Token: 0x04004F72 RID: 20338
		[Tooltip("Set this to true for cases like it is the first ZoneShaderSettings that should be activated when entering a scene.")]
		[SerializeField]
		private bool _activateOnAwake;

		// Token: 0x04004F73 RID: 20339
		[Tooltip("These values will be used as the default global values that will be fallen back to when not in a zone and that the other scripts will reference.")]
		public bool isDefaultValues;

		// Token: 0x04004F74 RID: 20340
		private static readonly int groundFogColor_shaderProp = Shader.PropertyToID("_ZoneGroundFogColor");

		// Token: 0x04004F75 RID: 20341
		[SerializeField]
		private ZoneShaderSettings.EOverrideMode groundFogColor_overrideMode;

		// Token: 0x04004F76 RID: 20342
		[SerializeField]
		private Color groundFogColor = new Color(0.7f, 0.9f, 1f, 1f);

		// Token: 0x04004F77 RID: 20343
		private static readonly int groundFogDepthFadeSq_shaderProp = Shader.PropertyToID("_ZoneGroundFogDepthFadeSq");

		// Token: 0x04004F78 RID: 20344
		[SerializeField]
		private ZoneShaderSettings.EOverrideMode groundFogDepthFade_overrideMode;

		// Token: 0x04004F79 RID: 20345
		[SerializeField]
		private float _groundFogDepthFadeSize = 20f;

		// Token: 0x04004F7A RID: 20346
		private static readonly int groundFogHeight_shaderProp = Shader.PropertyToID("_ZoneGroundFogHeight");

		// Token: 0x04004F7B RID: 20347
		[SerializeField]
		private ZoneShaderSettings.EOverrideMode groundFogHeight_overrideMode;

		// Token: 0x04004F7C RID: 20348
		[SerializeField]
		private float groundFogHeight = 7.45f;

		// Token: 0x04004F7D RID: 20349
		private static readonly int groundFogHeightFade_shaderProp = Shader.PropertyToID("_ZoneGroundFogHeightFade");

		// Token: 0x04004F7E RID: 20350
		[SerializeField]
		private ZoneShaderSettings.EOverrideMode groundFogHeightFade_overrideMode;

		// Token: 0x04004F7F RID: 20351
		[SerializeField]
		private float _groundFogHeightFadeSize = 20f;

		// Token: 0x04004F80 RID: 20352
		[SerializeField]
		private ZoneShaderSettings.EOverrideMode zoneLiquidType_overrideMode;

		// Token: 0x04004F81 RID: 20353
		[SerializeField]
		private ZoneShaderSettings.EZoneLiquidType zoneLiquidType = ZoneShaderSettings.EZoneLiquidType.Water;

		// Token: 0x04004F82 RID: 20354
		[OnEnterPlay_Set(0)]
		private static ZoneShaderSettings.EZoneLiquidType liquidType_previousValue = ZoneShaderSettings.EZoneLiquidType.None;

		// Token: 0x04004F83 RID: 20355
		[OnEnterPlay_Set(false)]
		private static bool didEverSetLiquidShape;

		// Token: 0x04004F84 RID: 20356
		[SerializeField]
		private ZoneShaderSettings.EOverrideMode liquidShape_overrideMode;

		// Token: 0x04004F85 RID: 20357
		[SerializeField]
		private ZoneShaderSettings.ELiquidShape liquidShape;

		// Token: 0x04004F86 RID: 20358
		[OnEnterPlay_Set(0)]
		private static ZoneShaderSettings.ELiquidShape liquidShape_previousValue = ZoneShaderSettings.ELiquidShape.Plane;

		// Token: 0x04004F88 RID: 20360
		[SerializeField]
		private ZoneShaderSettings.EOverrideMode liquidShapeRadius_overrideMode;

		// Token: 0x04004F89 RID: 20361
		[Tooltip("Fog params are: start, distance (end - start), unused, unused")]
		[SerializeField]
		private float liquidShapeRadius = 1f;

		// Token: 0x04004F8A RID: 20362
		[OnEnterPlay_Set(1f)]
		private static float liquidShapeRadius_previousValue;

		// Token: 0x04004F8B RID: 20363
		private bool hasLiquidBottomTransform;

		// Token: 0x04004F8C RID: 20364
		[SerializeField]
		private ZoneShaderSettings.EOverrideMode liquidBottomTransform_overrideMode;

		// Token: 0x04004F8D RID: 20365
		[Tooltip("TODO: remove this when there is a way to precalculate the nearest triangle plane per vertex so it will work better for rivers.")]
		[SerializeField]
		private Transform liquidBottomTransform;

		// Token: 0x04004F8E RID: 20366
		private float liquidBottomPosY_previousValue;

		// Token: 0x04004F8F RID: 20367
		private static readonly int shaderParam_GlobalZoneLiquidUVScale = Shader.PropertyToID("_GlobalZoneLiquidUVScale");

		// Token: 0x04004F90 RID: 20368
		[SerializeField]
		private ZoneShaderSettings.EOverrideMode zoneLiquidUVScale_overrideMode;

		// Token: 0x04004F91 RID: 20369
		[Tooltip("Fog params are: start, distance (end - start), unused, unused")]
		[SerializeField]
		private float zoneLiquidUVScale = 1f;

		// Token: 0x04004F92 RID: 20370
		private static readonly int shaderParam_GlobalWaterTintColor = Shader.PropertyToID("_GlobalWaterTintColor");

		// Token: 0x04004F93 RID: 20371
		[SerializeField]
		private ZoneShaderSettings.EOverrideMode underwaterTintColor_overrideMode;

		// Token: 0x04004F94 RID: 20372
		[SerializeField]
		private Color underwaterTintColor = new Color(0.3f, 0.65f, 1f, 0.2f);

		// Token: 0x04004F95 RID: 20373
		private static readonly int shaderParam_GlobalUnderwaterFogColor = Shader.PropertyToID("_GlobalUnderwaterFogColor");

		// Token: 0x04004F96 RID: 20374
		[SerializeField]
		private ZoneShaderSettings.EOverrideMode underwaterFogColor_overrideMode;

		// Token: 0x04004F97 RID: 20375
		[SerializeField]
		private Color underwaterFogColor = new Color(0.12f, 0.41f, 0.77f);

		// Token: 0x04004F98 RID: 20376
		private static readonly int shaderParam_GlobalUnderwaterFogParams = Shader.PropertyToID("_GlobalUnderwaterFogParams");

		// Token: 0x04004F99 RID: 20377
		[SerializeField]
		private ZoneShaderSettings.EOverrideMode underwaterFogParams_overrideMode;

		// Token: 0x04004F9A RID: 20378
		[Tooltip("Fog params are: start, distance (end - start), unused, unused")]
		[SerializeField]
		private Vector4 underwaterFogParams = new Vector4(-5f, 40f, 0f, 0f);

		// Token: 0x04004F9B RID: 20379
		private static readonly int shaderParam_GlobalUnderwaterCausticsParams = Shader.PropertyToID("_GlobalUnderwaterCausticsParams");

		// Token: 0x04004F9C RID: 20380
		[SerializeField]
		private ZoneShaderSettings.EOverrideMode underwaterCausticsParams_overrideMode;

		// Token: 0x04004F9D RID: 20381
		[Tooltip("Caustics params are: speed1, scale, alpha, unused")]
		[SerializeField]
		private Vector4 underwaterCausticsParams = new Vector4(0.075f, 0.075f, 1f, 0f);

		// Token: 0x04004F9E RID: 20382
		private static readonly int shaderParam_GlobalUnderwaterCausticsTex = Shader.PropertyToID("_GlobalUnderwaterCausticsTex");

		// Token: 0x04004F9F RID: 20383
		[SerializeField]
		private ZoneShaderSettings.EOverrideMode underwaterCausticsTexture_overrideMode;

		// Token: 0x04004FA0 RID: 20384
		[SerializeField]
		private Texture2D underwaterCausticsTexture;

		// Token: 0x04004FA1 RID: 20385
		private static readonly int shaderParam_GlobalUnderwaterEffectsDistanceToSurfaceFade = Shader.PropertyToID("_GlobalUnderwaterEffectsDistanceToSurfaceFade");

		// Token: 0x04004FA2 RID: 20386
		[SerializeField]
		private ZoneShaderSettings.EOverrideMode underwaterEffectsDistanceToSurfaceFade_overrideMode;

		// Token: 0x04004FA3 RID: 20387
		[SerializeField]
		private Vector2 underwaterEffectsDistanceToSurfaceFade = new Vector2(0.0001f, 50f);

		// Token: 0x04004FA4 RID: 20388
		private const string kEdTooltip_liquidResidueTex = "This is used for things like the charred surface effect when lava burns static geo.";

		// Token: 0x04004FA5 RID: 20389
		private static readonly int shaderParam_GlobalLiquidResidueTex = Shader.PropertyToID("_GlobalLiquidResidueTex");

		// Token: 0x04004FA6 RID: 20390
		[SerializeField]
		[Tooltip("This is used for things like the charred surface effect when lava burns static geo.")]
		private ZoneShaderSettings.EOverrideMode liquidResidueTex_overrideMode;

		// Token: 0x04004FA7 RID: 20391
		[SerializeField]
		[Tooltip("This is used for things like the charred surface effect when lava burns static geo.")]
		private Texture2D liquidResidueTex;

		// Token: 0x04004FA8 RID: 20392
		private readonly int shaderParam_GlobalMainWaterSurfacePlane = Shader.PropertyToID("_GlobalMainWaterSurfacePlane");

		// Token: 0x04004FA9 RID: 20393
		private bool hasMainWaterSurfacePlane;

		// Token: 0x04004FAA RID: 20394
		private bool hasDynamicWaterSurfacePlane;

		// Token: 0x04004FAB RID: 20395
		[SerializeField]
		private ZoneShaderSettings.EOverrideMode mainWaterSurfacePlane_overrideMode;

		// Token: 0x04004FAC RID: 20396
		[Tooltip("TODO: remove this when there is a way to precalculate the nearest triangle plane per vertex so it will work better for rivers.")]
		[SerializeField]
		private Transform mainWaterSurfacePlane;

		// Token: 0x04004FAD RID: 20397
		private static readonly int shaderParam_ZoneWeatherMapDissolveProgress = Shader.PropertyToID("_ZoneWeatherMapDissolveProgress");

		// Token: 0x04004FAE RID: 20398
		[SerializeField]
		private ZoneShaderSettings.EOverrideMode zoneWeatherMapDissolveProgress_overrideMode;

		// Token: 0x04004FAF RID: 20399
		[Tooltip("Fog params are: start, distance (end - start), unused, unused")]
		[Range(0f, 1f)]
		[SerializeField]
		private float zoneWeatherMapDissolveProgress = 1f;

		// Token: 0x02000C10 RID: 3088
		public enum EOverrideMode
		{
			// Token: 0x04004FB2 RID: 20402
			LeaveUnchanged,
			// Token: 0x04004FB3 RID: 20403
			ApplyNewValue,
			// Token: 0x04004FB4 RID: 20404
			ApplyDefaultValue
		}

		// Token: 0x02000C11 RID: 3089
		public enum EZoneLiquidType
		{
			// Token: 0x04004FB6 RID: 20406
			None,
			// Token: 0x04004FB7 RID: 20407
			Water,
			// Token: 0x04004FB8 RID: 20408
			Lava
		}

		// Token: 0x02000C12 RID: 3090
		public enum ELiquidShape
		{
			// Token: 0x04004FBA RID: 20410
			Plane,
			// Token: 0x04004FBB RID: 20411
			Cylinder
		}
	}
}
