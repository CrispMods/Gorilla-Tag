using System;
using UnityEngine;
using UnityEngine.Rendering;

// Token: 0x0200062E RID: 1582
public static class UberShader
{
	// Token: 0x17000415 RID: 1045
	// (get) Token: 0x06002733 RID: 10035 RVA: 0x0004AC5D File Offset: 0x00048E5D
	public static Material ReferenceMaterial
	{
		get
		{
			UberShader.InitDependencies();
			return UberShader.kReferenceMaterial;
		}
	}

	// Token: 0x17000416 RID: 1046
	// (get) Token: 0x06002734 RID: 10036 RVA: 0x0004AC69 File Offset: 0x00048E69
	public static Shader ReferenceShader
	{
		get
		{
			UberShader.InitDependencies();
			return UberShader.kReferenceShader;
		}
	}

	// Token: 0x17000417 RID: 1047
	// (get) Token: 0x06002735 RID: 10037 RVA: 0x0004AC75 File Offset: 0x00048E75
	public static Material ReferenceMaterialNonSRP
	{
		get
		{
			UberShader.InitDependencies();
			return UberShader.kReferenceMaterialNonSRP;
		}
	}

	// Token: 0x17000418 RID: 1048
	// (get) Token: 0x06002736 RID: 10038 RVA: 0x0004AC81 File Offset: 0x00048E81
	public static Shader ReferenceShaderNonSRP
	{
		get
		{
			UberShader.InitDependencies();
			return UberShader.kReferenceShaderNonSRP;
		}
	}

	// Token: 0x17000419 RID: 1049
	// (get) Token: 0x06002737 RID: 10039 RVA: 0x0004AC8D File Offset: 0x00048E8D
	public static UberShaderProperty[] AllProperties
	{
		get
		{
			UberShader.InitDependencies();
			return UberShader.kProperties;
		}
	}

	// Token: 0x06002738 RID: 10040 RVA: 0x0010B2EC File Offset: 0x001094EC
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

	// Token: 0x06002739 RID: 10041 RVA: 0x0004AC99 File Offset: 0x00048E99
	private static UberShaderProperty GetProperty(int i)
	{
		UberShader.InitDependencies();
		return UberShader.kProperties[i];
	}

	// Token: 0x0600273A RID: 10042 RVA: 0x0004AC99 File Offset: 0x00048E99
	private static UberShaderProperty GetProperty(int i, string expectedName)
	{
		UberShader.InitDependencies();
		return UberShader.kProperties[i];
	}

	// Token: 0x0600273B RID: 10043 RVA: 0x0010B348 File Offset: 0x00109548
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

	// Token: 0x0600273C RID: 10044 RVA: 0x0004AC69 File Offset: 0x00048E69
	public static Shader GetShader()
	{
		UberShader.InitDependencies();
		return UberShader.kReferenceShader;
	}

	// Token: 0x0600273D RID: 10045 RVA: 0x0010B3B0 File Offset: 0x001095B0
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

	// Token: 0x04002B56 RID: 11094
	private static Shader kReferenceShader;

	// Token: 0x04002B57 RID: 11095
	private static Material kReferenceMaterial;

	// Token: 0x04002B58 RID: 11096
	private static Shader kReferenceShaderNonSRP;

	// Token: 0x04002B59 RID: 11097
	private static Material kReferenceMaterialNonSRP;

	// Token: 0x04002B5A RID: 11098
	private static UberShaderProperty[] kProperties;

	// Token: 0x04002B5B RID: 11099
	private static bool gInitialized = false;

	// Token: 0x04002B5C RID: 11100
	public static UberShaderProperty TransparencyMode = UberShader.GetProperty(0);

	// Token: 0x04002B5D RID: 11101
	public static UberShaderProperty Cutoff = UberShader.GetProperty(1);

	// Token: 0x04002B5E RID: 11102
	public static UberShaderProperty ColorSource = UberShader.GetProperty(2);

	// Token: 0x04002B5F RID: 11103
	public static UberShaderProperty BaseColor = UberShader.GetProperty(3);

	// Token: 0x04002B60 RID: 11104
	public static UberShaderProperty GChannelColor = UberShader.GetProperty(4);

	// Token: 0x04002B61 RID: 11105
	public static UberShaderProperty BChannelColor = UberShader.GetProperty(5);

	// Token: 0x04002B62 RID: 11106
	public static UberShaderProperty AChannelColor = UberShader.GetProperty(6);

	// Token: 0x04002B63 RID: 11107
	public static UberShaderProperty BaseMap = UberShader.GetProperty(7);

	// Token: 0x04002B64 RID: 11108
	public static UberShaderProperty BaseMap_WH = UberShader.GetProperty(8);

	// Token: 0x04002B65 RID: 11109
	public static UberShaderProperty TexelSnapToggle = UberShader.GetProperty(9);

	// Token: 0x04002B66 RID: 11110
	public static UberShaderProperty TexelSnap_Factor = UberShader.GetProperty(10);

	// Token: 0x04002B67 RID: 11111
	public static UberShaderProperty UVSource = UberShader.GetProperty(11);

	// Token: 0x04002B68 RID: 11112
	public static UberShaderProperty AlphaDetailToggle = UberShader.GetProperty(12);

	// Token: 0x04002B69 RID: 11113
	public static UberShaderProperty AlphaDetail_ST = UberShader.GetProperty(13);

	// Token: 0x04002B6A RID: 11114
	public static UberShaderProperty AlphaDetail_Opacity = UberShader.GetProperty(14);

	// Token: 0x04002B6B RID: 11115
	public static UberShaderProperty AlphaDetail_WorldSpace = UberShader.GetProperty(15);

	// Token: 0x04002B6C RID: 11116
	public static UberShaderProperty MaskMapToggle = UberShader.GetProperty(16);

	// Token: 0x04002B6D RID: 11117
	public static UberShaderProperty MaskMap = UberShader.GetProperty(17);

	// Token: 0x04002B6E RID: 11118
	public static UberShaderProperty MaskMap_WH = UberShader.GetProperty(18);

	// Token: 0x04002B6F RID: 11119
	public static UberShaderProperty LavaLampToggle = UberShader.GetProperty(19);

	// Token: 0x04002B70 RID: 11120
	public static UberShaderProperty GradientMapToggle = UberShader.GetProperty(20);

	// Token: 0x04002B71 RID: 11121
	public static UberShaderProperty GradientMap = UberShader.GetProperty(21);

	// Token: 0x04002B72 RID: 11122
	public static UberShaderProperty DoTextureRotation = UberShader.GetProperty(22);

	// Token: 0x04002B73 RID: 11123
	public static UberShaderProperty RotateAngle = UberShader.GetProperty(23);

	// Token: 0x04002B74 RID: 11124
	public static UberShaderProperty RotateAnim = UberShader.GetProperty(24);

	// Token: 0x04002B75 RID: 11125
	public static UberShaderProperty UseWaveWarp = UberShader.GetProperty(25);

	// Token: 0x04002B76 RID: 11126
	public static UberShaderProperty WaveAmplitude = UberShader.GetProperty(26);

	// Token: 0x04002B77 RID: 11127
	public static UberShaderProperty WaveFrequency = UberShader.GetProperty(27);

	// Token: 0x04002B78 RID: 11128
	public static UberShaderProperty WaveScale = UberShader.GetProperty(28);

	// Token: 0x04002B79 RID: 11129
	public static UberShaderProperty WaveTimeScale = UberShader.GetProperty(29);

	// Token: 0x04002B7A RID: 11130
	public static UberShaderProperty UseWeatherMap = UberShader.GetProperty(30);

	// Token: 0x04002B7B RID: 11131
	public static UberShaderProperty WeatherMap = UberShader.GetProperty(31);

	// Token: 0x04002B7C RID: 11132
	public static UberShaderProperty WeatherMapDissolveEdgeSize = UberShader.GetProperty(32);

	// Token: 0x04002B7D RID: 11133
	public static UberShaderProperty ReflectToggle = UberShader.GetProperty(33);

	// Token: 0x04002B7E RID: 11134
	public static UberShaderProperty ReflectBoxProjectToggle = UberShader.GetProperty(34);

	// Token: 0x04002B7F RID: 11135
	public static UberShaderProperty ReflectBoxCubePos = UberShader.GetProperty(35);

	// Token: 0x04002B80 RID: 11136
	public static UberShaderProperty ReflectBoxSize = UberShader.GetProperty(36);

	// Token: 0x04002B81 RID: 11137
	public static UberShaderProperty ReflectBoxRotation = UberShader.GetProperty(37);

	// Token: 0x04002B82 RID: 11138
	public static UberShaderProperty ReflectMatcapToggle = UberShader.GetProperty(38);

	// Token: 0x04002B83 RID: 11139
	public static UberShaderProperty ReflectMatcapPerspToggle = UberShader.GetProperty(39);

	// Token: 0x04002B84 RID: 11140
	public static UberShaderProperty ReflectNormalToggle = UberShader.GetProperty(40);

	// Token: 0x04002B85 RID: 11141
	public static UberShaderProperty ReflectTex = UberShader.GetProperty(41);

	// Token: 0x04002B86 RID: 11142
	public static UberShaderProperty ReflectNormalTex = UberShader.GetProperty(42);

	// Token: 0x04002B87 RID: 11143
	public static UberShaderProperty ReflectAlbedoTint = UberShader.GetProperty(43);

	// Token: 0x04002B88 RID: 11144
	public static UberShaderProperty ReflectTint = UberShader.GetProperty(44);

	// Token: 0x04002B89 RID: 11145
	public static UberShaderProperty ReflectOpacity = UberShader.GetProperty(45);

	// Token: 0x04002B8A RID: 11146
	public static UberShaderProperty ReflectExposure = UberShader.GetProperty(46);

	// Token: 0x04002B8B RID: 11147
	public static UberShaderProperty ReflectOffset = UberShader.GetProperty(47);

	// Token: 0x04002B8C RID: 11148
	public static UberShaderProperty ReflectScale = UberShader.GetProperty(48);

	// Token: 0x04002B8D RID: 11149
	public static UberShaderProperty ReflectRotate = UberShader.GetProperty(49);

	// Token: 0x04002B8E RID: 11150
	public static UberShaderProperty HalfLambertToggle = UberShader.GetProperty(50);

	// Token: 0x04002B8F RID: 11151
	public static UberShaderProperty ZFightOffset = UberShader.GetProperty(51);

	// Token: 0x04002B90 RID: 11152
	public static UberShaderProperty ParallaxPlanarToggle = UberShader.GetProperty(52);

	// Token: 0x04002B91 RID: 11153
	public static UberShaderProperty ParallaxToggle = UberShader.GetProperty(53);

	// Token: 0x04002B92 RID: 11154
	public static UberShaderProperty ParallaxAAToggle = UberShader.GetProperty(54);

	// Token: 0x04002B93 RID: 11155
	public static UberShaderProperty ParallaxAABias = UberShader.GetProperty(55);

	// Token: 0x04002B94 RID: 11156
	public static UberShaderProperty DepthMap = UberShader.GetProperty(56);

	// Token: 0x04002B95 RID: 11157
	public static UberShaderProperty ParallaxAmplitude = UberShader.GetProperty(57);

	// Token: 0x04002B96 RID: 11158
	public static UberShaderProperty ParallaxSamplesMinMax = UberShader.GetProperty(58);

	// Token: 0x04002B97 RID: 11159
	public static UberShaderProperty UvShiftToggle = UberShader.GetProperty(59);

	// Token: 0x04002B98 RID: 11160
	public static UberShaderProperty UvShiftSteps = UberShader.GetProperty(60);

	// Token: 0x04002B99 RID: 11161
	public static UberShaderProperty UvShiftRate = UberShader.GetProperty(61);

	// Token: 0x04002B9A RID: 11162
	public static UberShaderProperty UvShiftOffset = UberShader.GetProperty(62);

	// Token: 0x04002B9B RID: 11163
	public static UberShaderProperty UseGridEffect = UberShader.GetProperty(63);

	// Token: 0x04002B9C RID: 11164
	public static UberShaderProperty UseCrystalEffect = UberShader.GetProperty(64);

	// Token: 0x04002B9D RID: 11165
	public static UberShaderProperty CrystalPower = UberShader.GetProperty(65);

	// Token: 0x04002B9E RID: 11166
	public static UberShaderProperty CrystalRimColor = UberShader.GetProperty(66);

	// Token: 0x04002B9F RID: 11167
	public static UberShaderProperty LiquidVolume = UberShader.GetProperty(67);

	// Token: 0x04002BA0 RID: 11168
	public static UberShaderProperty LiquidFill = UberShader.GetProperty(68);

	// Token: 0x04002BA1 RID: 11169
	public static UberShaderProperty LiquidFillNormal = UberShader.GetProperty(69);

	// Token: 0x04002BA2 RID: 11170
	public static UberShaderProperty LiquidSurfaceColor = UberShader.GetProperty(70);

	// Token: 0x04002BA3 RID: 11171
	public static UberShaderProperty LiquidSwayX = UberShader.GetProperty(71);

	// Token: 0x04002BA4 RID: 11172
	public static UberShaderProperty LiquidSwayY = UberShader.GetProperty(72);

	// Token: 0x04002BA5 RID: 11173
	public static UberShaderProperty LiquidContainer = UberShader.GetProperty(73);

	// Token: 0x04002BA6 RID: 11174
	public static UberShaderProperty LiquidPlanePosition = UberShader.GetProperty(74);

	// Token: 0x04002BA7 RID: 11175
	public static UberShaderProperty LiquidPlaneNormal = UberShader.GetProperty(75);

	// Token: 0x04002BA8 RID: 11176
	public static UberShaderProperty VertexFlapToggle = UberShader.GetProperty(76);

	// Token: 0x04002BA9 RID: 11177
	public static UberShaderProperty VertexFlapAxis = UberShader.GetProperty(77);

	// Token: 0x04002BAA RID: 11178
	public static UberShaderProperty VertexFlapDegreesMinMax = UberShader.GetProperty(78);

	// Token: 0x04002BAB RID: 11179
	public static UberShaderProperty VertexFlapSpeed = UberShader.GetProperty(79);

	// Token: 0x04002BAC RID: 11180
	public static UberShaderProperty VertexFlapPhaseOffset = UberShader.GetProperty(80);

	// Token: 0x04002BAD RID: 11181
	public static UberShaderProperty VertexWaveToggle = UberShader.GetProperty(81);

	// Token: 0x04002BAE RID: 11182
	public static UberShaderProperty VertexWaveDebug = UberShader.GetProperty(82);

	// Token: 0x04002BAF RID: 11183
	public static UberShaderProperty VertexWaveEnd = UberShader.GetProperty(83);

	// Token: 0x04002BB0 RID: 11184
	public static UberShaderProperty VertexWaveParams = UberShader.GetProperty(84);

	// Token: 0x04002BB1 RID: 11185
	public static UberShaderProperty VertexWaveFalloff = UberShader.GetProperty(85);

	// Token: 0x04002BB2 RID: 11186
	public static UberShaderProperty VertexWaveSphereMask = UberShader.GetProperty(86);

	// Token: 0x04002BB3 RID: 11187
	public static UberShaderProperty VertexWavePhaseOffset = UberShader.GetProperty(87);

	// Token: 0x04002BB4 RID: 11188
	public static UberShaderProperty VertexWaveAxes = UberShader.GetProperty(88);

	// Token: 0x04002BB5 RID: 11189
	public static UberShaderProperty VertexRotateToggle = UberShader.GetProperty(89);

	// Token: 0x04002BB6 RID: 11190
	public static UberShaderProperty VertexRotateAngles = UberShader.GetProperty(90);

	// Token: 0x04002BB7 RID: 11191
	public static UberShaderProperty VertexRotateAnim = UberShader.GetProperty(91);

	// Token: 0x04002BB8 RID: 11192
	public static UberShaderProperty VertexLightToggle = UberShader.GetProperty(92);

	// Token: 0x04002BB9 RID: 11193
	public static UberShaderProperty InnerGlowOn = UberShader.GetProperty(93);

	// Token: 0x04002BBA RID: 11194
	public static UberShaderProperty InnerGlowColor = UberShader.GetProperty(94);

	// Token: 0x04002BBB RID: 11195
	public static UberShaderProperty InnerGlowParams = UberShader.GetProperty(95);

	// Token: 0x04002BBC RID: 11196
	public static UberShaderProperty InnerGlowTap = UberShader.GetProperty(96);

	// Token: 0x04002BBD RID: 11197
	public static UberShaderProperty InnerGlowSine = UberShader.GetProperty(97);

	// Token: 0x04002BBE RID: 11198
	public static UberShaderProperty InnerGlowSinePeriod = UberShader.GetProperty(98);

	// Token: 0x04002BBF RID: 11199
	public static UberShaderProperty InnerGlowSinePhaseShift = UberShader.GetProperty(99);

	// Token: 0x04002BC0 RID: 11200
	public static UberShaderProperty StealthEffectOn = UberShader.GetProperty(100);

	// Token: 0x04002BC1 RID: 11201
	public static UberShaderProperty UseEyeTracking = UberShader.GetProperty(101);

	// Token: 0x04002BC2 RID: 11202
	public static UberShaderProperty EyeTileOffsetUV = UberShader.GetProperty(102);

	// Token: 0x04002BC3 RID: 11203
	public static UberShaderProperty EyeOverrideUV = UberShader.GetProperty(103);

	// Token: 0x04002BC4 RID: 11204
	public static UberShaderProperty EyeOverrideUVTransform = UberShader.GetProperty(104);

	// Token: 0x04002BC5 RID: 11205
	public static UberShaderProperty UseMouthFlap = UberShader.GetProperty(105);

	// Token: 0x04002BC6 RID: 11206
	public static UberShaderProperty MouthMap = UberShader.GetProperty(106);

	// Token: 0x04002BC7 RID: 11207
	public static UberShaderProperty MouthMap_Atlas = UberShader.GetProperty(107);

	// Token: 0x04002BC8 RID: 11208
	public static UberShaderProperty MouthMap_AtlasSlice = UberShader.GetProperty(108);

	// Token: 0x04002BC9 RID: 11209
	public static UberShaderProperty UseVertexColor = UberShader.GetProperty(109);

	// Token: 0x04002BCA RID: 11210
	public static UberShaderProperty WaterEffect = UberShader.GetProperty(110);

	// Token: 0x04002BCB RID: 11211
	public static UberShaderProperty HeightBasedWaterEffect = UberShader.GetProperty(111);

	// Token: 0x04002BCC RID: 11212
	public static UberShaderProperty UseDayNightLightmap = UberShader.GetProperty(112);

	// Token: 0x04002BCD RID: 11213
	public static UberShaderProperty UseSpecular = UberShader.GetProperty(113);

	// Token: 0x04002BCE RID: 11214
	public static UberShaderProperty UseSpecularAlphaChannel = UberShader.GetProperty(114);

	// Token: 0x04002BCF RID: 11215
	public static UberShaderProperty Smoothness = UberShader.GetProperty(115);

	// Token: 0x04002BD0 RID: 11216
	public static UberShaderProperty UseSpecHighlight = UberShader.GetProperty(116);

	// Token: 0x04002BD1 RID: 11217
	public static UberShaderProperty SpecularDir = UberShader.GetProperty(117);

	// Token: 0x04002BD2 RID: 11218
	public static UberShaderProperty SpecularPowerIntensity = UberShader.GetProperty(118);

	// Token: 0x04002BD3 RID: 11219
	public static UberShaderProperty SpecularColor = UberShader.GetProperty(119);

	// Token: 0x04002BD4 RID: 11220
	public static UberShaderProperty SpecularUseDiffuseColor = UberShader.GetProperty(120);

	// Token: 0x04002BD5 RID: 11221
	public static UberShaderProperty EmissionToggle = UberShader.GetProperty(121);

	// Token: 0x04002BD6 RID: 11222
	public static UberShaderProperty EmissionColor = UberShader.GetProperty(122);

	// Token: 0x04002BD7 RID: 11223
	public static UberShaderProperty EmissionMap = UberShader.GetProperty(123);

	// Token: 0x04002BD8 RID: 11224
	public static UberShaderProperty EmissionMaskByBaseMapAlpha = UberShader.GetProperty(124);

	// Token: 0x04002BD9 RID: 11225
	public static UberShaderProperty EmissionUVScrollSpeed = UberShader.GetProperty(125);

	// Token: 0x04002BDA RID: 11226
	public static UberShaderProperty EmissionDissolveProgress = UberShader.GetProperty(126);

	// Token: 0x04002BDB RID: 11227
	public static UberShaderProperty EmissionDissolveAnimation = UberShader.GetProperty(127);

	// Token: 0x04002BDC RID: 11228
	public static UberShaderProperty EmissionDissolveEdgeSize = UberShader.GetProperty(128);

	// Token: 0x04002BDD RID: 11229
	public static UberShaderProperty EmissionUseUVWaveWarp = UberShader.GetProperty(129);

	// Token: 0x04002BDE RID: 11230
	public static UberShaderProperty GreyZoneException = UberShader.GetProperty(130);

	// Token: 0x04002BDF RID: 11231
	public static UberShaderProperty Cull = UberShader.GetProperty(131);

	// Token: 0x04002BE0 RID: 11232
	public static UberShaderProperty StencilReference = UberShader.GetProperty(132);

	// Token: 0x04002BE1 RID: 11233
	public static UberShaderProperty StencilComparison = UberShader.GetProperty(133);

	// Token: 0x04002BE2 RID: 11234
	public static UberShaderProperty StencilPassFront = UberShader.GetProperty(134);

	// Token: 0x04002BE3 RID: 11235
	public static UberShaderProperty USE_DEFORM_MAP = UberShader.GetProperty(135);

	// Token: 0x04002BE4 RID: 11236
	public static UberShaderProperty DeformMap = UberShader.GetProperty(136);

	// Token: 0x04002BE5 RID: 11237
	public static UberShaderProperty DeformMapIntensity = UberShader.GetProperty(137);

	// Token: 0x04002BE6 RID: 11238
	public static UberShaderProperty DeformMapMaskByVertColorRAmount = UberShader.GetProperty(138);

	// Token: 0x04002BE7 RID: 11239
	public static UberShaderProperty DeformMapScrollSpeed = UberShader.GetProperty(139);

	// Token: 0x04002BE8 RID: 11240
	public static UberShaderProperty DeformMapUV0Influence = UberShader.GetProperty(140);

	// Token: 0x04002BE9 RID: 11241
	public static UberShaderProperty DeformMapObjectSpaceOffsetsU = UberShader.GetProperty(141);

	// Token: 0x04002BEA RID: 11242
	public static UberShaderProperty DeformMapObjectSpaceOffsetsV = UberShader.GetProperty(142);

	// Token: 0x04002BEB RID: 11243
	public static UberShaderProperty DeformMapWorldSpaceOffsetsU = UberShader.GetProperty(143);

	// Token: 0x04002BEC RID: 11244
	public static UberShaderProperty DeformMapWorldSpaceOffsetsV = UberShader.GetProperty(144);

	// Token: 0x04002BED RID: 11245
	public static UberShaderProperty RotateOnYAxisBySinTime = UberShader.GetProperty(145);

	// Token: 0x04002BEE RID: 11246
	public static UberShaderProperty USE_TEX_ARRAY_ATLAS = UberShader.GetProperty(146);

	// Token: 0x04002BEF RID: 11247
	public static UberShaderProperty BaseMap_Atlas = UberShader.GetProperty(147);

	// Token: 0x04002BF0 RID: 11248
	public static UberShaderProperty BaseMap_AtlasSlice = UberShader.GetProperty(148);

	// Token: 0x04002BF1 RID: 11249
	public static UberShaderProperty EmissionMap_Atlas = UberShader.GetProperty(149);

	// Token: 0x04002BF2 RID: 11250
	public static UberShaderProperty EmissionMap_AtlasSlice = UberShader.GetProperty(150);

	// Token: 0x04002BF3 RID: 11251
	public static UberShaderProperty DeformMap_Atlas = UberShader.GetProperty(151);

	// Token: 0x04002BF4 RID: 11252
	public static UberShaderProperty DeformMap_AtlasSlice = UberShader.GetProperty(152);

	// Token: 0x04002BF5 RID: 11253
	public static UberShaderProperty DEBUG_PAWN_DATA = UberShader.GetProperty(153);

	// Token: 0x04002BF6 RID: 11254
	public static UberShaderProperty SrcBlend = UberShader.GetProperty(154);

	// Token: 0x04002BF7 RID: 11255
	public static UberShaderProperty DstBlend = UberShader.GetProperty(155);

	// Token: 0x04002BF8 RID: 11256
	public static UberShaderProperty SrcBlendAlpha = UberShader.GetProperty(156);

	// Token: 0x04002BF9 RID: 11257
	public static UberShaderProperty DstBlendAlpha = UberShader.GetProperty(157);

	// Token: 0x04002BFA RID: 11258
	public static UberShaderProperty ZWrite = UberShader.GetProperty(158);

	// Token: 0x04002BFB RID: 11259
	public static UberShaderProperty AlphaToMask = UberShader.GetProperty(159);

	// Token: 0x04002BFC RID: 11260
	public static UberShaderProperty Color = UberShader.GetProperty(160);

	// Token: 0x04002BFD RID: 11261
	public static UberShaderProperty Surface = UberShader.GetProperty(161);

	// Token: 0x04002BFE RID: 11262
	public static UberShaderProperty Metallic = UberShader.GetProperty(162);

	// Token: 0x04002BFF RID: 11263
	public static UberShaderProperty SpecColor = UberShader.GetProperty(163);

	// Token: 0x04002C00 RID: 11264
	public static UberShaderProperty DayNightLightmapArray = UberShader.GetProperty(164);

	// Token: 0x04002C01 RID: 11265
	public static UberShaderProperty DayNightLightmapArray_AtlasSlice = UberShader.GetProperty(165);

	// Token: 0x04002C02 RID: 11266
	public static UberShaderProperty SingleLightmap = UberShader.GetProperty(166);
}
