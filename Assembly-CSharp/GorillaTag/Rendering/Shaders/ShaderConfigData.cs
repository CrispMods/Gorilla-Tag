﻿using System;
using UnityEngine;

namespace GorillaTag.Rendering.Shaders
{
	// Token: 0x02000C3F RID: 3135
	public class ShaderConfigData
	{
		// Token: 0x06004E90 RID: 20112 RVA: 0x001B0104 File Offset: 0x001AE304
		public static ShaderConfigData.MatPropInt[] convertInts(string[] names, int[] vals)
		{
			ShaderConfigData.MatPropInt[] array = new ShaderConfigData.MatPropInt[names.Length];
			for (int i = 0; i < array.Length; i++)
			{
				array[i] = new ShaderConfigData.MatPropInt
				{
					intName = names[i],
					intVal = vals[i]
				};
			}
			return array;
		}

		// Token: 0x06004E91 RID: 20113 RVA: 0x001B0150 File Offset: 0x001AE350
		public static ShaderConfigData.MatPropFloat[] convertFloats(string[] names, float[] vals)
		{
			ShaderConfigData.MatPropFloat[] array = new ShaderConfigData.MatPropFloat[names.Length];
			for (int i = 0; i < array.Length; i++)
			{
				array[i] = new ShaderConfigData.MatPropFloat
				{
					floatName = names[i],
					floatVal = vals[i]
				};
			}
			return array;
		}

		// Token: 0x06004E92 RID: 20114 RVA: 0x001B019C File Offset: 0x001AE39C
		public static ShaderConfigData.MatPropMatrix[] convertMatrices(string[] names, Matrix4x4[] vals)
		{
			ShaderConfigData.MatPropMatrix[] array = new ShaderConfigData.MatPropMatrix[names.Length];
			for (int i = 0; i < array.Length; i++)
			{
				array[i] = new ShaderConfigData.MatPropMatrix
				{
					matrixName = names[i],
					matrixVal = vals[i]
				};
			}
			return array;
		}

		// Token: 0x06004E93 RID: 20115 RVA: 0x001B01EC File Offset: 0x001AE3EC
		public static ShaderConfigData.MatPropVector[] convertVectors(string[] names, Vector4[] vals)
		{
			ShaderConfigData.MatPropVector[] array = new ShaderConfigData.MatPropVector[names.Length];
			for (int i = 0; i < array.Length; i++)
			{
				array[i] = new ShaderConfigData.MatPropVector
				{
					vectorName = names[i],
					vectorVal = vals[i]
				};
			}
			return array;
		}

		// Token: 0x06004E94 RID: 20116 RVA: 0x001B023C File Offset: 0x001AE43C
		public static ShaderConfigData.MatPropTexture[] convertTextures(string[] names, Texture[] vals)
		{
			ShaderConfigData.MatPropTexture[] array = new ShaderConfigData.MatPropTexture[names.Length];
			for (int i = 0; i < array.Length; i++)
			{
				array[i] = new ShaderConfigData.MatPropTexture
				{
					textureName = names[i],
					textureVal = vals[i]
				};
			}
			return array;
		}

		// Token: 0x06004E95 RID: 20117 RVA: 0x001B0288 File Offset: 0x001AE488
		public static string GetShaderPropertiesStringFromMaterial(Material mat, bool excludeMainTexData)
		{
			string text = "";
			string[] propertyNames = mat.GetPropertyNames(MaterialPropertyType.Int);
			int[] array = new int[propertyNames.Length];
			for (int i = 0; i < propertyNames.Length; i++)
			{
				array[i] = mat.GetInteger(propertyNames[i]);
				text += array[i].ToString();
			}
			propertyNames = mat.GetPropertyNames(MaterialPropertyType.Float);
			float[] array2 = new float[propertyNames.Length];
			for (int j = 0; j < propertyNames.Length; j++)
			{
				if (excludeMainTexData || !propertyNames[j].Contains("_BaseMap"))
				{
					array2[j] = mat.GetFloat(propertyNames[j]);
					text += array2[j].ToString();
				}
			}
			propertyNames = mat.GetPropertyNames(MaterialPropertyType.Matrix);
			Matrix4x4[] array3 = new Matrix4x4[propertyNames.Length];
			for (int k = 0; k < propertyNames.Length; k++)
			{
				array3[k] = mat.GetMatrix(propertyNames[k]);
				text += array3[k].ToString();
			}
			propertyNames = mat.GetPropertyNames(MaterialPropertyType.Vector);
			Vector4[] array4 = new Vector4[propertyNames.Length];
			for (int l = 0; l < propertyNames.Length; l++)
			{
				if (excludeMainTexData || !propertyNames[l].Contains("_BaseMap"))
				{
					array4[l] = mat.GetVector(propertyNames[l]);
					text += array4[l].ToString();
				}
			}
			propertyNames = mat.GetPropertyNames(MaterialPropertyType.Texture);
			Texture[] array5 = new Texture[propertyNames.Length];
			for (int m = 0; m < propertyNames.Length; m++)
			{
				if (!propertyNames[m].Contains("_BaseMap"))
				{
					array5[m] = mat.GetTexture(propertyNames[m]);
					if (array5[m] != null)
					{
						text += array5[m].ToString();
					}
				}
			}
			return text;
		}

		// Token: 0x06004E96 RID: 20118 RVA: 0x001B0454 File Offset: 0x001AE654
		public static ShaderConfigData.ShaderConfig GetConfigDataFromMaterial(Material mat, bool includeMainTexData)
		{
			string[] propertyNames = mat.GetPropertyNames(MaterialPropertyType.Int);
			string[] array = propertyNames;
			int[] array2 = new int[array.Length];
			bool flag = mat.IsKeywordEnabled("_WATER_EFFECT");
			bool flag2 = mat.IsKeywordEnabled("_MAINTEX_ROTATE");
			bool flag3 = mat.IsKeywordEnabled("_UV_WAVE_WARP");
			bool flag4 = mat.IsKeywordEnabled("_EMISSION_USE_UV_WAVE_WARP");
			bool flag5 = flag3 || flag4;
			bool flag6 = mat.IsKeywordEnabled("_LIQUID_CONTAINER");
			bool flag7 = mat.IsKeywordEnabled("_LIQUID_VOLUME") && !flag6;
			bool flag8 = mat.IsKeywordEnabled("_CRYSTAL_EFFECT");
			bool flag9 = mat.IsKeywordEnabled("_EMISSION") || flag8;
			bool flag10 = mat.IsKeywordEnabled("_REFLECTIONS");
			mat.IsKeywordEnabled("_REFLECTIONS_MATCAP");
			bool flag11 = mat.IsKeywordEnabled("_UV_SHIFT");
			for (int i = 0; i < array.Length; i++)
			{
				array2[i] = mat.GetInteger(propertyNames[i]);
				if (!flag11 && (propertyNames[i] == "_UvShiftSteps" || propertyNames[i] == "_UvShiftOffset"))
				{
					array2[i] = 0;
				}
			}
			propertyNames = mat.GetPropertyNames(MaterialPropertyType.Float);
			string[] array3 = propertyNames;
			float[] array4 = new float[array3.Length];
			for (int j = 0; j < propertyNames.Length; j++)
			{
				if (includeMainTexData || !propertyNames[j].Contains("_BaseMap"))
				{
					array4[j] = mat.GetFloat(propertyNames[j]);
				}
				if ((!flag && propertyNames[j] == "_HeightBasedWaterEffect") || (!flag2 && propertyNames[j] == "_RotateSpeed") || (!flag5 && (propertyNames[j] == "_WaveAmplitude" || propertyNames[j] == "_WaveFrequency" || propertyNames[j] == "_WaveScale")) || (!flag7 && (propertyNames[j] == "_LiquidFill" || propertyNames[j] == "_LiquidSwayX" || propertyNames[j] == "_LiquidSwayY")) || (!flag8 && propertyNames[j] == "_CrystalPower") || (!flag9 && propertyNames[j].StartsWith("_Emission")) || (!flag10 && (propertyNames[j] == "_ReflectOpacity" || propertyNames[j] == "_ReflectExposure" || propertyNames[j] == "_ReflectRotate")) || (!flag11 && propertyNames[j] == "_UvShiftRate"))
				{
					array4[j] = 0f;
				}
			}
			propertyNames = mat.GetPropertyNames(MaterialPropertyType.Matrix);
			string[] array5 = propertyNames;
			Matrix4x4[] array6 = new Matrix4x4[array5.Length];
			for (int k = 0; k < propertyNames.Length; k++)
			{
				array6[k] = mat.GetMatrix(propertyNames[k]);
			}
			propertyNames = mat.GetPropertyNames(MaterialPropertyType.Vector);
			string[] array7 = propertyNames;
			Vector4[] array8 = new Vector4[array7.Length];
			for (int l = 0; l < propertyNames.Length; l++)
			{
				if (includeMainTexData || !propertyNames[l].Contains("_BaseMap"))
				{
					array8[l] = mat.GetVector(propertyNames[l]);
				}
				if ((!flag7 && (propertyNames[l] == "_LiquidFillNormal" || propertyNames[l] == "_LiquidSurfaceColor")) || (!flag6 && (propertyNames[l] == "_LiquidPlanePosition" || propertyNames[l] == "_LiquidPlaneNormal")) || (!flag8 && propertyNames[l] == "_CrystalRimColor") || (!flag9 && propertyNames[l].StartsWith("_Emission")) || (!flag10 && (propertyNames[l] == "_ReflectTint" || propertyNames[l] == "_ReflectOffset" || propertyNames[l] == "_ReflectScale")))
				{
					array8[l] = Vector4.zero;
				}
			}
			propertyNames = mat.GetPropertyNames(MaterialPropertyType.Texture);
			string[] array9 = propertyNames;
			Texture[] array10 = new Texture[array9.Length];
			for (int m = 0; m < propertyNames.Length; m++)
			{
				if (!propertyNames[m].Contains("_BaseMap"))
				{
					array10[m] = mat.GetTexture(propertyNames[m]);
				}
			}
			return new ShaderConfigData.ShaderConfig(mat.shader.name, mat, array, array2, array3, array4, array5, array6, array7, array8, array9, array10);
		}

		// Token: 0x02000C40 RID: 3136
		[Serializable]
		public struct ShaderConfig
		{
			// Token: 0x06004E98 RID: 20120 RVA: 0x001B087C File Offset: 0x001AEA7C
			public ShaderConfig(string shadName, Material fMat, string[] intNames, int[] intVals, string[] floatNames, float[] floatVals, string[] matrixNames, Matrix4x4[] matrixVals, string[] vectorNames, Vector4[] vectorVals, string[] textureNames, Texture[] textureVals)
			{
				this.shaderName = shadName;
				this.firstMat = fMat;
				this.ints = ShaderConfigData.convertInts(intNames, intVals);
				this.floats = ShaderConfigData.convertFloats(floatNames, floatVals);
				this.matrices = ShaderConfigData.convertMatrices(matrixNames, matrixVals);
				this.vectors = ShaderConfigData.convertVectors(vectorNames, vectorVals);
				this.textures = ShaderConfigData.convertTextures(textureNames, textureVals);
			}

			// Token: 0x040050A0 RID: 20640
			public string shaderName;

			// Token: 0x040050A1 RID: 20641
			public Material firstMat;

			// Token: 0x040050A2 RID: 20642
			public ShaderConfigData.MatPropInt[] ints;

			// Token: 0x040050A3 RID: 20643
			public ShaderConfigData.MatPropFloat[] floats;

			// Token: 0x040050A4 RID: 20644
			public ShaderConfigData.MatPropMatrix[] matrices;

			// Token: 0x040050A5 RID: 20645
			public ShaderConfigData.MatPropVector[] vectors;

			// Token: 0x040050A6 RID: 20646
			public ShaderConfigData.MatPropTexture[] textures;
		}

		// Token: 0x02000C41 RID: 3137
		[Serializable]
		public struct MatPropInt
		{
			// Token: 0x040050A7 RID: 20647
			public string intName;

			// Token: 0x040050A8 RID: 20648
			public int intVal;
		}

		// Token: 0x02000C42 RID: 3138
		[Serializable]
		public struct MatPropFloat
		{
			// Token: 0x040050A9 RID: 20649
			public string floatName;

			// Token: 0x040050AA RID: 20650
			public float floatVal;
		}

		// Token: 0x02000C43 RID: 3139
		[Serializable]
		public struct MatPropMatrix
		{
			// Token: 0x040050AB RID: 20651
			public string matrixName;

			// Token: 0x040050AC RID: 20652
			public Matrix4x4 matrixVal;
		}

		// Token: 0x02000C44 RID: 3140
		[Serializable]
		public struct MatPropVector
		{
			// Token: 0x040050AD RID: 20653
			public string vectorName;

			// Token: 0x040050AE RID: 20654
			public Vector4 vectorVal;
		}

		// Token: 0x02000C45 RID: 3141
		[Serializable]
		public struct MatPropTexture
		{
			// Token: 0x040050AF RID: 20655
			public string textureName;

			// Token: 0x040050B0 RID: 20656
			public Texture textureVal;
		}

		// Token: 0x02000C46 RID: 3142
		[Serializable]
		public struct RenderersForShaderWithSameProperties
		{
			// Token: 0x040050B1 RID: 20657
			public MeshRenderer[] renderers;
		}
	}
}
