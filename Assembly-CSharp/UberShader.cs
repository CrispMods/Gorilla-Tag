using System;
using UnityEngine;
using UnityEngine.Rendering;

// Token: 0x0200064F RID: 1615
public static class UberShader
{
	// Token: 0x17000437 RID: 1079
	// (get) Token: 0x06002808 RID: 10248 RVA: 0x000C4059 File Offset: 0x000C2259
	public static Material ReferenceMaterial
	{
		get
		{
			UberShader.InitDependencies();
			return UberShader.kReferenceMaterial;
		}
	}

	// Token: 0x17000438 RID: 1080
	// (get) Token: 0x06002809 RID: 10249 RVA: 0x000C4065 File Offset: 0x000C2265
	public static Shader ReferenceShader
	{
		get
		{
			UberShader.InitDependencies();
			return UberShader.kReferenceShader;
		}
	}

	// Token: 0x17000439 RID: 1081
	// (get) Token: 0x0600280A RID: 10250 RVA: 0x000C4071 File Offset: 0x000C2271
	public static Material ReferenceMaterialNonSRP
	{
		get
		{
			UberShader.InitDependencies();
			return UberShader.kReferenceMaterialNonSRP;
		}
	}

	// Token: 0x1700043A RID: 1082
	// (get) Token: 0x0600280B RID: 10251 RVA: 0x000C407D File Offset: 0x000C227D
	public static Shader ReferenceShaderNonSRP
	{
		get
		{
			UberShader.InitDependencies();
			return UberShader.kReferenceShaderNonSRP;
		}
	}

	// Token: 0x1700043B RID: 1083
	// (get) Token: 0x0600280C RID: 10252 RVA: 0x000C4089 File Offset: 0x000C2289
	public static UberShaderProperty[] AllProperties
	{
		get
		{
			UberShader.InitDependencies();
			return UberShader.kProperties;
		}
	}

	// Token: 0x0600280D RID: 10253 RVA: 0x000C4098 File Offset: 0x000C2298
	public static bool IsAnimated(Material m)
	{
		if (m == null)
		{
			return false;
		}
		if ((double)UberShader.UvShiftToggle.GetValue<float>(m) <= 0.5)
		{
			return false;
		}
		Vector2 value = UberShader.UvShiftRate.GetValue<Vector2>(m);
		return value.x > 0f || value.y > 0f;
	}

	// Token: 0x0600280E RID: 10254 RVA: 0x000C40F3 File Offset: 0x000C22F3
	private static UberShaderProperty GetProperty(int i)
	{
		UberShader.InitDependencies();
		return UberShader.kProperties[i];
	}

	// Token: 0x0600280F RID: 10255 RVA: 0x000C40F3 File Offset: 0x000C22F3
	private static UberShaderProperty GetProperty(int i, string expectedName)
	{
		UberShader.InitDependencies();
		return UberShader.kProperties[i];
	}

	// Token: 0x06002810 RID: 10256 RVA: 0x000C4104 File Offset: 0x000C2304
	private static void InitDependencies()
	{
		if (UberShader.gInitialized)
		{
			return;
		}
		UberShader.kReferenceShader = Shader.Find("GorillaTag/UberShader");
		UberShader.kReferenceMaterial = new Material(UberShader.kReferenceShader);
		UberShader.kReferenceShaderNonSRP = Shader.Find("GorillaTag/UberShaderNonSRP");
		UberShader.kReferenceMaterialNonSRP = new Material(UberShader.kReferenceShaderNonSRP);
		UberShader.kProperties = UberShader.EnumerateAllProperties(UberShader.kReferenceShader);
		UberShader.gInitialized = true;
	}

	// Token: 0x06002811 RID: 10257 RVA: 0x000C4065 File Offset: 0x000C2265
	public static Shader GetShader()
	{
		UberShader.InitDependencies();
		return UberShader.kReferenceShader;
	}

	// Token: 0x06002812 RID: 10258 RVA: 0x000C416C File Offset: 0x000C236C
	private static UberShaderProperty[] EnumerateAllProperties(Shader uberShader)
	{
		int propertyCount = uberShader.GetPropertyCount();
		UberShaderProperty[] array = new UberShaderProperty[propertyCount];
		for (int i = 0; i < propertyCount; i++)
		{
			UberShaderProperty uberShaderProperty = new UberShaderProperty
			{
				index = i,
				flags = uberShader.GetPropertyFlags(i),
				type = uberShader.GetPropertyType(i),
				nameID = uberShader.GetPropertyNameId(i),
				name = uberShader.GetPropertyName(i),
				attributes = uberShader.GetPropertyAttributes(i)
			};
			if (uberShaderProperty.type == ShaderPropertyType.Range)
			{
				uberShaderProperty.rangeLimits = uberShader.GetPropertyRangeLimits(uberShaderProperty.index);
			}
			string[] attributes = uberShaderProperty.attributes;
			if (attributes != null && attributes.Length != 0)
			{
				foreach (string text in attributes)
				{
					if (!string.IsNullOrWhiteSpace(text))
					{
						bool flag = text.StartsWith("Toggle(");
						uberShaderProperty.isKeywordToggle = flag;
						if (flag)
						{
							string keyword = text.Split('(', StringSplitOptions.RemoveEmptyEntries)[1].RemoveEnd(")", StringComparison.InvariantCulture);
							uberShaderProperty.keyword = keyword;
						}
					}
				}
			}
			array[i] = uberShaderProperty;
		}
		return array;
	}

	// Token: 0x04002BF0 RID: 11248
	private static Shader kReferenceShader;

	// Token: 0x04002BF1 RID: 11249
	private static Material kReferenceMaterial;

	// Token: 0x04002BF2 RID: 11250
	private static Shader kReferenceShaderNonSRP;

	// Token: 0x04002BF3 RID: 11251
	private static Material kReferenceMaterialNonSRP;

	// Token: 0x04002BF4 RID: 11252
	private static UberShaderProperty[] kProperties;

	// Token: 0x04002BF5 RID: 11253
	private static bool gInitialized = false;

	// Token: 0x04002BF6 RID: 11254
	public static UberShaderProperty TransparencyMode = UberShader.GetProperty(0);

	// Token: 0x04002BF7 RID: 11255
	public static UberShaderProperty Cutoff = UberShader.GetProperty(1);

	// Token: 0x04002BF8 RID: 11256
	public static UberShaderProperty ColorSource = UberShader.GetProperty(2);

	// Token: 0x04002BF9 RID: 11257
	public static UberShaderProperty BaseColor = UberShader.GetProperty(3);

	// Token: 0x04002BFA RID: 11258
	public static UberShaderProperty GChannelColor = UberShader.GetProperty(4);

	// Token: 0x04002BFB RID: 11259
	public static UberShaderProperty BChannelColor = UberShader.GetProperty(5);

	// Token: 0x04002BFC RID: 11260
	public static UberShaderProperty AChannelColor = UberShader.GetProperty(6);

	// Token: 0x04002BFD RID: 11261
	public static UberShaderProperty BaseMap = UberShader.GetProperty(7);

	// Token: 0x04002BFE RID: 11262
	public static UberShaderProperty BaseMap_WH = UberShader.GetProperty(8);

	// Token: 0x04002BFF RID: 11263
	public static UberShaderProperty TexelSnapToggle = UberShader.GetProperty(9);

	// Token: 0x04002C00 RID: 11264
	public static UberShaderProperty TexelSnap_Factor = UberShader.GetProperty(10);

	// Token: 0x04002C01 RID: 11265
	public static UberShaderProperty UVSource = UberShader.GetProperty(11);

	// Token: 0x04002C02 RID: 11266
	public static UberShaderProperty AlphaDetailToggle = UberShader.GetProperty(12);

	// Token: 0x04002C03 RID: 11267
	public static UberShaderProperty AlphaDetail_ST = UberShader.GetProperty(13);

	// Token: 0x04002C04 RID: 11268
	public static UberShaderProperty AlphaDetail_Opacity = UberShader.GetProperty(14);

	// Token: 0x04002C05 RID: 11269
	public static UberShaderProperty AlphaDetail_WorldSpace = UberShader.GetProperty(15);

	// Token: 0x04002C06 RID: 11270
	public static UberShaderProperty MaskMapToggle = UberShader.GetProperty(16);

	// Token: 0x04002C07 RID: 11271
	public static UberShaderProperty MaskMap = UberShader.GetProperty(17);

	// Token: 0x04002C08 RID: 11272
	public static UberShaderProperty MaskMap_WH = UberShader.GetProperty(18);

	// Token: 0x04002C09 RID: 11273
	public static UberShaderProperty LavaLampToggle = UberShader.GetProperty(19);

	// Token: 0x04002C0A RID: 11274
	public static UberShaderProperty GradientMapToggle = UberShader.GetProperty(20);

	// Token: 0x04002C0B RID: 11275
	public static UberShaderProperty GradientMap = UberShader.GetProperty(21);

	// Token: 0x04002C0C RID: 11276
	public static UberShaderProperty DoTextureRotation = UberShader.GetProperty(22);

	// Token: 0x04002C0D RID: 11277
	public static UberShaderProperty RotateAngle = UberShader.GetProperty(23);

	// Token: 0x04002C0E RID: 11278
	public static UberShaderProperty RotateAnim = UberShader.GetProperty(24);

	// Token: 0x04002C0F RID: 11279
	public static UberShaderProperty UseWaveWarp = UberShader.GetProperty(25);

	// Token: 0x04002C10 RID: 11280
	public static UberShaderProperty WaveAmplitude = UberShader.GetProperty(26);

	// Token: 0x04002C11 RID: 11281
	public static UberShaderProperty WaveFrequency = UberShader.GetProperty(27);

	// Token: 0x04002C12 RID: 11282
	public static UberShaderProperty WaveScale = UberShader.GetProperty(28);

	// Token: 0x04002C13 RID: 11283
	public static UberShaderProperty WaveTimeScale = UberShader.GetProperty(29);

	// Token: 0x04002C14 RID: 11284
	public static UberShaderProperty UseWeatherMap = UberShader.GetProperty(30);

	// Token: 0x04002C15 RID: 11285
	public static UberShaderProperty WeatherMap = UberShader.GetProperty(31);

	// Token: 0x04002C16 RID: 11286
	public static UberShaderProperty WeatherMapDissolveEdgeSize = UberShader.GetProperty(32);

	// Token: 0x04002C17 RID: 11287
	public static UberShaderProperty ReflectToggle = UberShader.GetProperty(33);

	// Token: 0x04002C18 RID: 11288
	public static UberShaderProperty ReflectBoxProjectToggle = UberShader.GetProperty(34);

	// Token: 0x04002C19 RID: 11289
	public static UberShaderProperty ReflectBoxCubePos = UberShader.GetProperty(35);

	// Token: 0x04002C1A RID: 11290
	public static UberShaderProperty ReflectBoxSize = UberShader.GetProperty(36);

	// Token: 0x04002C1B RID: 11291
	public static UberShaderProperty ReflectBoxRotation = UberShader.GetProperty(37);

	// Token: 0x04002C1C RID: 11292
	public static UberShaderProperty ReflectMatcapToggle = UberShader.GetProperty(38);

	// Token: 0x04002C1D RID: 11293
	public static UberShaderProperty ReflectMatcapPerspToggle = UberShader.GetProperty(39);

	// Token: 0x04002C1E RID: 11294
	public static UberShaderProperty ReflectNormalToggle = UberShader.GetProperty(40);

	// Token: 0x04002C1F RID: 11295
	public static UberShaderProperty ReflectTex = UberShader.GetProperty(41);

	// Token: 0x04002C20 RID: 11296
	public static UberShaderProperty ReflectNormalTex = UberShader.GetProperty(42);

	// Token: 0x04002C21 RID: 11297
	public static UberShaderProperty ReflectAlbedoTint = UberShader.GetProperty(43);

	// Token: 0x04002C22 RID: 11298
	public static UberShaderProperty ReflectTint = UberShader.GetProperty(44);

	// Token: 0x04002C23 RID: 11299
	public static UberShaderProperty ReflectOpacity = UberShader.GetProperty(45);

	// Token: 0x04002C24 RID: 11300
	public static UberShaderProperty ReflectExposure = UberShader.GetProperty(46);

	// Token: 0x04002C25 RID: 11301
	public static UberShaderProperty ReflectOffset = UberShader.GetProperty(47);

	// Token: 0x04002C26 RID: 11302
	public static UberShaderProperty ReflectScale = UberShader.GetProperty(48);

	// Token: 0x04002C27 RID: 11303
	public static UberShaderProperty ReflectRotate = UberShader.GetProperty(49);

	// Token: 0x04002C28 RID: 11304
	public static UberShaderProperty HalfLambertToggle = UberShader.GetProperty(50);

	// Token: 0x04002C29 RID: 11305
	public static UberShaderProperty ZFightOffset = UberShader.GetProperty(51);

	// Token: 0x04002C2A RID: 11306
	public static UberShaderProperty ParallaxPlanarToggle = UberShader.GetProperty(52);

	// Token: 0x04002C2B RID: 11307
	public static UberShaderProperty ParallaxToggle = UberShader.GetProperty(53);

	// Token: 0x04002C2C RID: 11308
	public static UberShaderProperty ParallaxAAToggle = UberShader.GetProperty(54);

	// Token: 0x04002C2D RID: 11309
	public static UberShaderProperty ParallaxAABias = UberShader.GetProperty(55);

	// Token: 0x04002C2E RID: 11310
	public static UberShaderProperty DepthMap = UberShader.GetProperty(56);

	// Token: 0x04002C2F RID: 11311
	public static UberShaderProperty ParallaxAmplitude = UberShader.GetProperty(57);

	// Token: 0x04002C30 RID: 11312
	public static UberShaderProperty ParallaxSamplesMinMax = UberShader.GetProperty(58);

	// Token: 0x04002C31 RID: 11313
	public static UberShaderProperty UvShiftToggle = UberShader.GetProperty(59);

	// Token: 0x04002C32 RID: 11314
	public static UberShaderProperty UvShiftSteps = UberShader.GetProperty(60);

	// Token: 0x04002C33 RID: 11315
	public static UberShaderProperty UvShiftRate = UberShader.GetProperty(61);

	// Token: 0x04002C34 RID: 11316
	public static UberShaderProperty UvShiftOffset = UberShader.GetProperty(62);

	// Token: 0x04002C35 RID: 11317
	public static UberShaderProperty UseGridEffect = UberShader.GetProperty(63);

	// Token: 0x04002C36 RID: 11318
	public static UberShaderProperty UseCrystalEffect = UberShader.GetProperty(64);

	// Token: 0x04002C37 RID: 11319
	public static UberShaderProperty CrystalPower = UberShader.GetProperty(65);

	// Token: 0x04002C38 RID: 11320
	public static UberShaderProperty CrystalRimColor = UberShader.GetProperty(66);

	// Token: 0x04002C39 RID: 11321
	public static UberShaderProperty LiquidVolume = UberShader.GetProperty(67);

	// Token: 0x04002C3A RID: 11322
	public static UberShaderProperty LiquidFill = UberShader.GetProperty(68);

	// Token: 0x04002C3B RID: 11323
	public static UberShaderProperty LiquidFillNormal = UberShader.GetProperty(69);

	// Token: 0x04002C3C RID: 11324
	public static UberShaderProperty LiquidSurfaceColor = UberShader.GetProperty(70);

	// Token: 0x04002C3D RID: 11325
	public static UberShaderProperty LiquidSwayX = UberShader.GetProperty(71);

	// Token: 0x04002C3E RID: 11326
	public static UberShaderProperty LiquidSwayY = UberShader.GetProperty(72);

	// Token: 0x04002C3F RID: 11327
	public static UberShaderProperty LiquidContainer = UberShader.GetProperty(73);

	// Token: 0x04002C40 RID: 11328
	public static UberShaderProperty LiquidPlanePosition = UberShader.GetProperty(74);

	// Token: 0x04002C41 RID: 11329
	public static UberShaderProperty LiquidPlaneNormal = UberShader.GetProperty(75);

	// Token: 0x04002C42 RID: 11330
	public static UberShaderProperty VertexFlapToggle = UberShader.GetProperty(76);

	// Token: 0x04002C43 RID: 11331
	public static UberShaderProperty VertexFlapAxis = UberShader.GetProperty(77);

	// Token: 0x04002C44 RID: 11332
	public static UberShaderProperty VertexFlapDegreesMinMax = UberShader.GetProperty(78);

	// Token: 0x04002C45 RID: 11333
	public static UberShaderProperty VertexFlapSpeed = UberShader.GetProperty(79);

	// Token: 0x04002C46 RID: 11334
	public static UberShaderProperty VertexFlapPhaseOffset = UberShader.GetProperty(80);

	// Token: 0x04002C47 RID: 11335
	public static UberShaderProperty VertexWaveToggle = UberShader.GetProperty(81);

	// Token: 0x04002C48 RID: 11336
	public static UberShaderProperty VertexWaveDebug = UberShader.GetProperty(82);

	// Token: 0x04002C49 RID: 11337
	public static UberShaderProperty VertexWaveEnd = UberShader.GetProperty(83);

	// Token: 0x04002C4A RID: 11338
	public static UberShaderProperty VertexWaveParams = UberShader.GetProperty(84);

	// Token: 0x04002C4B RID: 11339
	public static UberShaderProperty VertexWaveFalloff = UberShader.GetProperty(85);

	// Token: 0x04002C4C RID: 11340
	public static UberShaderProperty VertexWaveSphereMask = UberShader.GetProperty(86);

	// Token: 0x04002C4D RID: 11341
	public static UberShaderProperty VertexWavePhaseOffset = UberShader.GetProperty(87);

	// Token: 0x04002C4E RID: 11342
	public static UberShaderProperty VertexWaveAxes = UberShader.GetProperty(88);

	// Token: 0x04002C4F RID: 11343
	public static UberShaderProperty VertexRotateToggle = UberShader.GetProperty(89);

	// Token: 0x04002C50 RID: 11344
	public static UberShaderProperty VertexRotateAngles = UberShader.GetProperty(90);

	// Token: 0x04002C51 RID: 11345
	public static UberShaderProperty VertexRotateAnim = UberShader.GetProperty(91);

	// Token: 0x04002C52 RID: 11346
	public static UberShaderProperty VertexLightToggle = UberShader.GetProperty(92);

	// Token: 0x04002C53 RID: 11347
	public static UberShaderProperty InnerGlowOn = UberShader.GetProperty(93);

	// Token: 0x04002C54 RID: 11348
	public static UberShaderProperty InnerGlowColor = UberShader.GetProperty(94);

	// Token: 0x04002C55 RID: 11349
	public static UberShaderProperty InnerGlowParams = UberShader.GetProperty(95);

	// Token: 0x04002C56 RID: 11350
	public static UberShaderProperty InnerGlowTap = UberShader.GetProperty(96);

	// Token: 0x04002C57 RID: 11351
	public static UberShaderProperty InnerGlowSine = UberShader.GetProperty(97);

	// Token: 0x04002C58 RID: 11352
	public static UberShaderProperty InnerGlowSinePeriod = UberShader.GetProperty(98);

	// Token: 0x04002C59 RID: 11353
	public static UberShaderProperty InnerGlowSinePhaseShift = UberShader.GetProperty(99);

	// Token: 0x04002C5A RID: 11354
	public static UberShaderProperty StealthEffectOn = UberShader.GetProperty(100);

	// Token: 0x04002C5B RID: 11355
	public static UberShaderProperty UseEyeTracking = UberShader.GetProperty(101);

	// Token: 0x04002C5C RID: 11356
	public static UberShaderProperty EyeTileOffsetUV = UberShader.GetProperty(102);

	// Token: 0x04002C5D RID: 11357
	public static UberShaderProperty EyeOverrideUV = UberShader.GetProperty(103);

	// Token: 0x04002C5E RID: 11358
	public static UberShaderProperty EyeOverrideUVTransform = UberShader.GetProperty(104);

	// Token: 0x04002C5F RID: 11359
	public static UberShaderProperty UseMouthFlap = UberShader.GetProperty(105);

	// Token: 0x04002C60 RID: 11360
	public static UberShaderProperty MouthMap = UberShader.GetProperty(106);

	// Token: 0x04002C61 RID: 11361
	public static UberShaderProperty MouthMap_Atlas = UberShader.GetProperty(107);

	// Token: 0x04002C62 RID: 11362
	public static UberShaderProperty MouthMap_AtlasSlice = UberShader.GetProperty(108);

	// Token: 0x04002C63 RID: 11363
	public static UberShaderProperty UseVertexColor = UberShader.GetProperty(109);

	// Token: 0x04002C64 RID: 11364
	public static UberShaderProperty WaterEffect = UberShader.GetProperty(110);

	// Token: 0x04002C65 RID: 11365
	public static UberShaderProperty HeightBasedWaterEffect = UberShader.GetProperty(111);

	// Token: 0x04002C66 RID: 11366
	public static UberShaderProperty UseDayNightLightmap = UberShader.GetProperty(112);

	// Token: 0x04002C67 RID: 11367
	public static UberShaderProperty UseSpecular = UberShader.GetProperty(113);

	// Token: 0x04002C68 RID: 11368
	public static UberShaderProperty UseSpecularAlphaChannel = UberShader.GetProperty(114);

	// Token: 0x04002C69 RID: 11369
	public static UberShaderProperty Smoothness = UberShader.GetProperty(115);

	// Token: 0x04002C6A RID: 11370
	public static UberShaderProperty UseSpecHighlight = UberShader.GetProperty(116);

	// Token: 0x04002C6B RID: 11371
	public static UberShaderProperty SpecularDir = UberShader.GetProperty(117);

	// Token: 0x04002C6C RID: 11372
	public static UberShaderProperty SpecularPowerIntensity = UberShader.GetProperty(118);

	// Token: 0x04002C6D RID: 11373
	public static UberShaderProperty SpecularColor = UberShader.GetProperty(119);

	// Token: 0x04002C6E RID: 11374
	public static UberShaderProperty SpecularUseDiffuseColor = UberShader.GetProperty(120);

	// Token: 0x04002C6F RID: 11375
	public static UberShaderProperty EmissionToggle = UberShader.GetProperty(121);

	// Token: 0x04002C70 RID: 11376
	public static UberShaderProperty EmissionColor = UberShader.GetProperty(122);

	// Token: 0x04002C71 RID: 11377
	public static UberShaderProperty EmissionMap = UberShader.GetProperty(123);

	// Token: 0x04002C72 RID: 11378
	public static UberShaderProperty EmissionMaskByBaseMapAlpha = UberShader.GetProperty(124);

	// Token: 0x04002C73 RID: 11379
	public static UberShaderProperty EmissionUVScrollSpeed = UberShader.GetProperty(125);

	// Token: 0x04002C74 RID: 11380
	public static UberShaderProperty EmissionDissolveProgress = UberShader.GetProperty(126);

	// Token: 0x04002C75 RID: 11381
	public static UberShaderProperty EmissionDissolveAnimation = UberShader.GetProperty(127);

	// Token: 0x04002C76 RID: 11382
	public static UberShaderProperty EmissionDissolveEdgeSize = UberShader.GetProperty(128);

	// Token: 0x04002C77 RID: 11383
	public static UberShaderProperty EmissionUseUVWaveWarp = UberShader.GetProperty(129);

	// Token: 0x04002C78 RID: 11384
	public static UberShaderProperty GreyZoneException = UberShader.GetProperty(130);

	// Token: 0x04002C79 RID: 11385
	public static UberShaderProperty Cull = UberShader.GetProperty(131);

	// Token: 0x04002C7A RID: 11386
	public static UberShaderProperty StencilReference = UberShader.GetProperty(132);

	// Token: 0x04002C7B RID: 11387
	public static UberShaderProperty StencilComparison = UberShader.GetProperty(133);

	// Token: 0x04002C7C RID: 11388
	public static UberShaderProperty StencilPassFront = UberShader.GetProperty(134);

	// Token: 0x04002C7D RID: 11389
	public static UberShaderProperty USE_DEFORM_MAP = UberShader.GetProperty(135);

	// Token: 0x04002C7E RID: 11390
	public static UberShaderProperty DeformMap = UberShader.GetProperty(136);

	// Token: 0x04002C7F RID: 11391
	public static UberShaderProperty DeformMapIntensity = UberShader.GetProperty(137);

	// Token: 0x04002C80 RID: 11392
	public static UberShaderProperty DeformMapMaskByVertColorRAmount = UberShader.GetProperty(138);

	// Token: 0x04002C81 RID: 11393
	public static UberShaderProperty DeformMapScrollSpeed = UberShader.GetProperty(139);

	// Token: 0x04002C82 RID: 11394
	public static UberShaderProperty DeformMapUV0Influence = UberShader.GetProperty(140);

	// Token: 0x04002C83 RID: 11395
	public static UberShaderProperty DeformMapObjectSpaceOffsetsU = UberShader.GetProperty(141);

	// Token: 0x04002C84 RID: 11396
	public static UberShaderProperty DeformMapObjectSpaceOffsetsV = UberShader.GetProperty(142);

	// Token: 0x04002C85 RID: 11397
	public static UberShaderProperty DeformMapWorldSpaceOffsetsU = UberShader.GetProperty(143);

	// Token: 0x04002C86 RID: 11398
	public static UberShaderProperty DeformMapWorldSpaceOffsetsV = UberShader.GetProperty(144);

	// Token: 0x04002C87 RID: 11399
	public static UberShaderProperty RotateOnYAxisBySinTime = UberShader.GetProperty(145);

	// Token: 0x04002C88 RID: 11400
	public static UberShaderProperty USE_TEX_ARRAY_ATLAS = UberShader.GetProperty(146);

	// Token: 0x04002C89 RID: 11401
	public static UberShaderProperty BaseMap_Atlas = UberShader.GetProperty(147);

	// Token: 0x04002C8A RID: 11402
	public static UberShaderProperty BaseMap_AtlasSlice = UberShader.GetProperty(148);

	// Token: 0x04002C8B RID: 11403
	public static UberShaderProperty EmissionMap_Atlas = UberShader.GetProperty(149);

	// Token: 0x04002C8C RID: 11404
	public static UberShaderProperty EmissionMap_AtlasSlice = UberShader.GetProperty(150);

	// Token: 0x04002C8D RID: 11405
	public static UberShaderProperty DeformMap_Atlas = UberShader.GetProperty(151);

	// Token: 0x04002C8E RID: 11406
	public static UberShaderProperty DeformMap_AtlasSlice = UberShader.GetProperty(152);

	// Token: 0x04002C8F RID: 11407
	public static UberShaderProperty DEBUG_PAWN_DATA = UberShader.GetProperty(153);

	// Token: 0x04002C90 RID: 11408
	public static UberShaderProperty SrcBlend = UberShader.GetProperty(154);

	// Token: 0x04002C91 RID: 11409
	public static UberShaderProperty DstBlend = UberShader.GetProperty(155);

	// Token: 0x04002C92 RID: 11410
	public static UberShaderProperty SrcBlendAlpha = UberShader.GetProperty(156);

	// Token: 0x04002C93 RID: 11411
	public static UberShaderProperty DstBlendAlpha = UberShader.GetProperty(157);

	// Token: 0x04002C94 RID: 11412
	public static UberShaderProperty ZWrite = UberShader.GetProperty(158);

	// Token: 0x04002C95 RID: 11413
	public static UberShaderProperty AlphaToMask = UberShader.GetProperty(159);

	// Token: 0x04002C96 RID: 11414
	public static UberShaderProperty Color = UberShader.GetProperty(160);

	// Token: 0x04002C97 RID: 11415
	public static UberShaderProperty Surface = UberShader.GetProperty(161);

	// Token: 0x04002C98 RID: 11416
	public static UberShaderProperty Metallic = UberShader.GetProperty(162);

	// Token: 0x04002C99 RID: 11417
	public static UberShaderProperty SpecColor = UberShader.GetProperty(163);

	// Token: 0x04002C9A RID: 11418
	public static UberShaderProperty DayNightLightmapArray = UberShader.GetProperty(164);

	// Token: 0x04002C9B RID: 11419
	public static UberShaderProperty DayNightLightmapArray_AtlasSlice = UberShader.GetProperty(165);

	// Token: 0x04002C9C RID: 11420
	public static UberShaderProperty SingleLightmap = UberShader.GetProperty(166);
}
