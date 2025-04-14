using System;
using System.Collections.Generic;
using UnityEngine;

namespace CjLib
{
	// Token: 0x02000C99 RID: 3225
	public class DebugUtil
	{
		// Token: 0x0600512A RID: 20778 RVA: 0x00189C64 File Offset: 0x00187E64
		private static Material GetMaterial(DebugUtil.Style style, bool depthTest, bool capShiftScale)
		{
			int num = 0;
			if (style - DebugUtil.Style.FlatShaded <= 1)
			{
				num |= 1;
			}
			if (capShiftScale)
			{
				num |= 2;
			}
			if (depthTest)
			{
				num |= 4;
			}
			if (DebugUtil.s_materialPool == null)
			{
				DebugUtil.s_materialPool = new Dictionary<int, Material>();
			}
			Material material;
			if (!DebugUtil.s_materialPool.TryGetValue(num, out material) || material == null)
			{
				if (material == null)
				{
					DebugUtil.s_materialPool.Remove(num);
				}
				Shader shader = Shader.Find(depthTest ? "CjLib/Primitive" : "CjLib/PrimitiveNoZTest");
				if (shader == null)
				{
					return null;
				}
				material = new Material(shader);
				if ((num & 1) != 0)
				{
					material.EnableKeyword("NORMAL_ON");
				}
				if ((num & 2) != 0)
				{
					material.EnableKeyword("CAP_SHIFT_SCALE");
				}
				DebugUtil.s_materialPool.Add(num, material);
			}
			return material;
		}

		// Token: 0x0600512B RID: 20779 RVA: 0x00189D1D File Offset: 0x00187F1D
		private static MaterialPropertyBlock GetMaterialPropertyBlock()
		{
			if (DebugUtil.s_materialProperties == null)
			{
				return DebugUtil.s_materialProperties = new MaterialPropertyBlock();
			}
			return DebugUtil.s_materialProperties;
		}

		// Token: 0x0600512C RID: 20780 RVA: 0x00189D38 File Offset: 0x00187F38
		public static void DrawLine(Vector3 v0, Vector3 v1, Color color, bool depthTest = true)
		{
			Mesh mesh = PrimitiveMeshFactory.Line(v0, v1);
			if (mesh == null)
			{
				return;
			}
			Material material = DebugUtil.GetMaterial(DebugUtil.Style.Wireframe, depthTest, false);
			MaterialPropertyBlock materialPropertyBlock = DebugUtil.GetMaterialPropertyBlock();
			materialPropertyBlock.SetColor("_Color", color);
			materialPropertyBlock.SetVector("_Dimensions", new Vector4(1f, 1f, 1f, 0f));
			materialPropertyBlock.SetFloat("_ZBias", DebugUtil.s_wireframeZBias);
			Graphics.DrawMesh(mesh, Vector3.zero, Quaternion.identity, material, 0, null, 0, materialPropertyBlock, false, false, false);
		}

		// Token: 0x0600512D RID: 20781 RVA: 0x00189DC0 File Offset: 0x00187FC0
		public static void DrawLines(Vector3[] aVert, Color color, bool depthTest = true)
		{
			Mesh mesh = PrimitiveMeshFactory.Lines(aVert);
			if (mesh == null)
			{
				return;
			}
			Material material = DebugUtil.GetMaterial(DebugUtil.Style.Wireframe, depthTest, false);
			MaterialPropertyBlock materialPropertyBlock = DebugUtil.GetMaterialPropertyBlock();
			materialPropertyBlock.SetColor("_Color", color);
			materialPropertyBlock.SetVector("_Dimensions", new Vector4(1f, 1f, 1f, 0f));
			materialPropertyBlock.SetFloat("_ZBias", DebugUtil.s_wireframeZBias);
			Graphics.DrawMesh(mesh, Vector3.zero, Quaternion.identity, material, 0, null, 0, materialPropertyBlock, false, false, false);
		}

		// Token: 0x0600512E RID: 20782 RVA: 0x00189E48 File Offset: 0x00188048
		public static void DrawLineStrip(Vector3[] aVert, Color color, bool depthTest = true)
		{
			Mesh mesh = PrimitiveMeshFactory.LineStrip(aVert);
			if (mesh == null)
			{
				return;
			}
			Material material = DebugUtil.GetMaterial(DebugUtil.Style.Wireframe, depthTest, false);
			MaterialPropertyBlock materialPropertyBlock = DebugUtil.GetMaterialPropertyBlock();
			materialPropertyBlock.SetColor("_Color", color);
			materialPropertyBlock.SetVector("_Dimensions", new Vector4(1f, 1f, 1f, 0f));
			materialPropertyBlock.SetFloat("_ZBias", DebugUtil.s_wireframeZBias);
			Graphics.DrawMesh(mesh, Vector3.zero, Quaternion.identity, material, 0, null, 0, materialPropertyBlock, false, false, false);
		}

		// Token: 0x0600512F RID: 20783 RVA: 0x00189ED0 File Offset: 0x001880D0
		public static void DrawArc(Vector3 center, Vector3 from, Vector3 normal, float angle, float radius, int numSegments, Color color, bool depthTest = true)
		{
			if (numSegments <= 0)
			{
				return;
			}
			from.Normalize();
			from *= radius;
			Vector3[] array = new Vector3[numSegments + 1];
			array[0] = center + from;
			float num = 1f / (float)numSegments;
			Quaternion rotation = QuaternionUtil.AxisAngle(normal, angle * num);
			Vector3 vector = rotation * from;
			for (int i = 1; i <= numSegments; i++)
			{
				array[i] = center + vector;
				vector = rotation * vector;
			}
			DebugUtil.DrawLineStrip(array, color, depthTest);
		}

		// Token: 0x06005130 RID: 20784 RVA: 0x00189F5C File Offset: 0x0018815C
		public static void DrawLocator(Vector3 position, Vector3 right, Vector3 up, Vector3 forward, Color rightColor, Color upColor, Color forwardColor, float size = 0.5f)
		{
			DebugUtil.DrawLine(position, position + right * size, rightColor, true);
			DebugUtil.DrawLine(position, position + up * size, upColor, true);
			DebugUtil.DrawLine(position, position + forward * size, forwardColor, true);
		}

		// Token: 0x06005131 RID: 20785 RVA: 0x00189FAE File Offset: 0x001881AE
		public static void DrawLocator(Vector3 position, Vector3 right, Vector3 up, Vector3 forward, float size = 0.5f)
		{
			DebugUtil.DrawLocator(position, right, up, forward, Color.red, Color.green, Color.blue, size);
		}

		// Token: 0x06005132 RID: 20786 RVA: 0x00189FCC File Offset: 0x001881CC
		public static void DrawLocator(Vector3 position, Quaternion rotation, Color rightColor, Color upColor, Color forwardColor, float size = 0.5f)
		{
			Vector3 right = rotation * Vector3.right;
			Vector3 up = rotation * Vector3.up;
			Vector3 forward = rotation * Vector3.forward;
			DebugUtil.DrawLocator(position, right, up, forward, rightColor, upColor, forwardColor, size);
		}

		// Token: 0x06005133 RID: 20787 RVA: 0x0018A00C File Offset: 0x0018820C
		public static void DrawLocator(Vector3 position, Quaternion rotation, float size = 0.5f)
		{
			DebugUtil.DrawLocator(position, rotation, Color.red, Color.green, Color.blue, size);
		}

		// Token: 0x06005134 RID: 20788 RVA: 0x0018A028 File Offset: 0x00188228
		public static void DrawBox(Vector3 center, Quaternion rotation, Vector3 dimensions, Color color, bool depthTest = true, DebugUtil.Style style = DebugUtil.Style.Wireframe)
		{
			if (dimensions.x < MathUtil.Epsilon || dimensions.y < MathUtil.Epsilon || dimensions.z < MathUtil.Epsilon)
			{
				return;
			}
			Mesh mesh = null;
			switch (style)
			{
			case DebugUtil.Style.Wireframe:
				mesh = PrimitiveMeshFactory.BoxWireframe();
				break;
			case DebugUtil.Style.SolidColor:
				mesh = PrimitiveMeshFactory.BoxSolidColor();
				break;
			case DebugUtil.Style.FlatShaded:
			case DebugUtil.Style.SmoothShaded:
				mesh = PrimitiveMeshFactory.BoxFlatShaded();
				break;
			}
			if (mesh == null)
			{
				return;
			}
			Material material = DebugUtil.GetMaterial(style, depthTest, false);
			MaterialPropertyBlock materialPropertyBlock = DebugUtil.GetMaterialPropertyBlock();
			materialPropertyBlock.SetColor("_Color", color);
			materialPropertyBlock.SetVector("_Dimensions", new Vector4(dimensions.x, dimensions.y, dimensions.z, 0f));
			materialPropertyBlock.SetFloat("_ZBias", (style == DebugUtil.Style.Wireframe) ? DebugUtil.s_wireframeZBias : 0f);
			Graphics.DrawMesh(mesh, center, rotation, material, 0, null, 0, materialPropertyBlock, false, false, false);
		}

		// Token: 0x06005135 RID: 20789 RVA: 0x0018A108 File Offset: 0x00188308
		public static void DrawRect(Vector3 center, Quaternion rotation, Vector2 dimensions, Color color, bool depthTest = true, DebugUtil.Style style = DebugUtil.Style.Wireframe)
		{
			if (dimensions.x < MathUtil.Epsilon || dimensions.y < MathUtil.Epsilon)
			{
				return;
			}
			Mesh mesh = null;
			switch (style)
			{
			case DebugUtil.Style.Wireframe:
				mesh = PrimitiveMeshFactory.RectWireframe();
				break;
			case DebugUtil.Style.SolidColor:
				mesh = PrimitiveMeshFactory.RectSolidColor();
				break;
			case DebugUtil.Style.FlatShaded:
			case DebugUtil.Style.SmoothShaded:
				mesh = PrimitiveMeshFactory.RectFlatShaded();
				break;
			}
			if (mesh == null)
			{
				return;
			}
			Material material = DebugUtil.GetMaterial(style, depthTest, false);
			MaterialPropertyBlock materialPropertyBlock = DebugUtil.GetMaterialPropertyBlock();
			materialPropertyBlock.SetColor("_Color", color);
			materialPropertyBlock.SetVector("_Dimensions", new Vector4(dimensions.x, 1f, dimensions.y, 0f));
			materialPropertyBlock.SetFloat("_ZBias", (style == DebugUtil.Style.Wireframe) ? DebugUtil.s_wireframeZBias : 0f);
			Graphics.DrawMesh(mesh, center, rotation, material, 0, null, 0, materialPropertyBlock, false, false, false);
		}

		// Token: 0x06005136 RID: 20790 RVA: 0x0018A1DC File Offset: 0x001883DC
		public static void DrawRect2D(Vector3 center, float rotationDeg, Vector2 dimensions, Color color, bool depthTest = true, DebugUtil.Style style = DebugUtil.Style.Wireframe)
		{
			Quaternion rotation = Quaternion.AngleAxis(rotationDeg, Vector3.forward) * Quaternion.AngleAxis(90f, Vector3.right);
			DebugUtil.DrawRect(center, rotation, dimensions, color, depthTest, style);
		}

		// Token: 0x06005137 RID: 20791 RVA: 0x0018A218 File Offset: 0x00188418
		public static void DrawCircle(Vector3 center, Quaternion rotation, float radius, int numSegments, Color color, bool depthTest = true, DebugUtil.Style style = DebugUtil.Style.Wireframe)
		{
			if (radius < MathUtil.Epsilon)
			{
				return;
			}
			Mesh mesh = null;
			switch (style)
			{
			case DebugUtil.Style.Wireframe:
				mesh = PrimitiveMeshFactory.CircleWireframe(numSegments);
				break;
			case DebugUtil.Style.SolidColor:
				mesh = PrimitiveMeshFactory.CircleSolidColor(numSegments);
				break;
			case DebugUtil.Style.FlatShaded:
			case DebugUtil.Style.SmoothShaded:
				mesh = PrimitiveMeshFactory.CircleFlatShaded(numSegments);
				break;
			}
			if (mesh == null)
			{
				return;
			}
			Material material = DebugUtil.GetMaterial(style, depthTest, false);
			MaterialPropertyBlock materialPropertyBlock = DebugUtil.GetMaterialPropertyBlock();
			materialPropertyBlock.SetColor("_Color", color);
			materialPropertyBlock.SetVector("_Dimensions", new Vector4(radius, radius, radius, 0f));
			materialPropertyBlock.SetFloat("_ZBias", (style == DebugUtil.Style.Wireframe) ? DebugUtil.s_wireframeZBias : 0f);
			Graphics.DrawMesh(mesh, center, rotation, material, 0, null, 0, materialPropertyBlock, false, false, false);
		}

		// Token: 0x06005138 RID: 20792 RVA: 0x0018A2D0 File Offset: 0x001884D0
		public static void DrawCircle(Vector3 center, Vector3 normal, float radius, int numSegments, Color color, bool depthTest = true, DebugUtil.Style style = DebugUtil.Style.Wireframe)
		{
			Quaternion rotation = Quaternion.LookRotation(Vector3.Normalize(Vector3.Cross((Mathf.Abs(Vector3.Dot(normal, Vector3.up)) < 0.5f) ? Vector3.up : Vector3.forward, normal)), normal);
			DebugUtil.DrawCircle(center, rotation, radius, numSegments, color, depthTest, style);
		}

		// Token: 0x06005139 RID: 20793 RVA: 0x0018A321 File Offset: 0x00188521
		public static void DrawCircle2D(Vector3 center, float radius, int numSegments, Color color, bool depthTest = true, DebugUtil.Style style = DebugUtil.Style.Wireframe)
		{
			DebugUtil.DrawCircle(center, Vector3.forward, radius, numSegments, color, depthTest, style);
		}

		// Token: 0x0600513A RID: 20794 RVA: 0x0018A338 File Offset: 0x00188538
		public static void DrawCylinder(Vector3 center, Quaternion rotation, float height, float radius, int numSegments, Color color, bool depthTest = true, DebugUtil.Style style = DebugUtil.Style.Wireframe)
		{
			if (height < MathUtil.Epsilon || radius < MathUtil.Epsilon)
			{
				return;
			}
			Mesh mesh = null;
			switch (style)
			{
			case DebugUtil.Style.Wireframe:
				mesh = PrimitiveMeshFactory.CylinderWireframe(numSegments);
				break;
			case DebugUtil.Style.SolidColor:
				mesh = PrimitiveMeshFactory.CylinderSolidColor(numSegments);
				break;
			case DebugUtil.Style.FlatShaded:
				mesh = PrimitiveMeshFactory.CylinderFlatShaded(numSegments);
				break;
			case DebugUtil.Style.SmoothShaded:
				mesh = PrimitiveMeshFactory.CylinderSmoothShaded(numSegments);
				break;
			}
			if (mesh == null)
			{
				return;
			}
			Material material = DebugUtil.GetMaterial(style, depthTest, true);
			MaterialPropertyBlock materialPropertyBlock = DebugUtil.GetMaterialPropertyBlock();
			materialPropertyBlock.SetColor("_Color", color);
			materialPropertyBlock.SetVector("_Dimensions", new Vector4(radius, radius, radius, height));
			materialPropertyBlock.SetFloat("_ZBias", (style == DebugUtil.Style.Wireframe) ? DebugUtil.s_wireframeZBias : 0f);
			Graphics.DrawMesh(mesh, center, rotation, material, 0, null, 0, materialPropertyBlock, false, false, false);
		}

		// Token: 0x0600513B RID: 20795 RVA: 0x0018A400 File Offset: 0x00188600
		public static void DrawCylinder(Vector3 point0, Vector3 point1, float radius, int numSegments, Color color, bool depthTest = true, DebugUtil.Style style = DebugUtil.Style.Wireframe)
		{
			Vector3 vector = point1 - point0;
			float magnitude = vector.magnitude;
			if (magnitude < MathUtil.Epsilon)
			{
				return;
			}
			vector.Normalize();
			Vector3 center = 0.5f * (point0 + point1);
			Quaternion rotation = Quaternion.LookRotation(Vector3.Normalize(Vector3.Cross((Mathf.Abs(Vector3.Dot(vector.normalized, Vector3.up)) < 0.5f) ? Vector3.up : Vector3.forward, vector)), vector);
			DebugUtil.DrawCylinder(center, rotation, magnitude, radius, numSegments, color, depthTest, style);
		}

		// Token: 0x0600513C RID: 20796 RVA: 0x0018A488 File Offset: 0x00188688
		public static void DrawSphere(Vector3 center, Quaternion rotation, float radius, int latSegments, int longSegments, Color color, bool depthTest = true, DebugUtil.Style style = DebugUtil.Style.Wireframe)
		{
			if (radius < MathUtil.Epsilon)
			{
				return;
			}
			Mesh mesh = null;
			switch (style)
			{
			case DebugUtil.Style.Wireframe:
				mesh = PrimitiveMeshFactory.SphereWireframe(latSegments, longSegments);
				break;
			case DebugUtil.Style.SolidColor:
				mesh = PrimitiveMeshFactory.SphereSolidColor(latSegments, longSegments);
				break;
			case DebugUtil.Style.FlatShaded:
				mesh = PrimitiveMeshFactory.SphereFlatShaded(latSegments, longSegments);
				break;
			case DebugUtil.Style.SmoothShaded:
				mesh = PrimitiveMeshFactory.SphereSmoothShaded(latSegments, longSegments);
				break;
			}
			if (mesh == null)
			{
				return;
			}
			Material material = DebugUtil.GetMaterial(style, depthTest, false);
			MaterialPropertyBlock materialPropertyBlock = DebugUtil.GetMaterialPropertyBlock();
			materialPropertyBlock.SetColor("_Color", color);
			materialPropertyBlock.SetVector("_Dimensions", new Vector4(radius, radius, radius, 0f));
			materialPropertyBlock.SetFloat("_ZBias", (style == DebugUtil.Style.Wireframe) ? DebugUtil.s_wireframeZBias : 0f);
			Graphics.DrawMesh(mesh, center, rotation, material, 0, null, 0, materialPropertyBlock, false, false, false);
		}

		// Token: 0x0600513D RID: 20797 RVA: 0x0018A54E File Offset: 0x0018874E
		public static void DrawSphere(Vector3 center, float radius, int latSegments, int longSegments, Color color, bool depthTest = true, DebugUtil.Style style = DebugUtil.Style.Wireframe)
		{
			DebugUtil.DrawSphere(center, Quaternion.identity, radius, latSegments, longSegments, color, depthTest, style);
		}

		// Token: 0x0600513E RID: 20798 RVA: 0x0018A564 File Offset: 0x00188764
		public static void DrawSphereTripleCircles(Vector3 center, Quaternion rotation, float radius, int numSegments, Color color, bool depthTest = true, DebugUtil.Style style = DebugUtil.Style.Wireframe)
		{
			Vector3 normal = rotation * Vector3.right;
			Vector3 normal2 = rotation * Vector3.up;
			Vector3 normal3 = rotation * Vector3.forward;
			DebugUtil.DrawCircle(center, normal, radius, numSegments, color, depthTest, style);
			DebugUtil.DrawCircle(center, normal2, radius, numSegments, color, depthTest, style);
			DebugUtil.DrawCircle(center, normal3, radius, numSegments, color, depthTest, style);
		}

		// Token: 0x0600513F RID: 20799 RVA: 0x0018A5C2 File Offset: 0x001887C2
		public static void DrawSphereTripleCircles(Vector3 center, float radius, int numSegments, Color color, bool depthTest = true, DebugUtil.Style style = DebugUtil.Style.Wireframe)
		{
			DebugUtil.DrawSphereTripleCircles(center, Quaternion.identity, radius, numSegments, color, depthTest, style);
		}

		// Token: 0x06005140 RID: 20800 RVA: 0x0018A5D8 File Offset: 0x001887D8
		public static void DrawCapsule(Vector3 center, Quaternion rotation, float height, float radius, int latSegmentsPerCap, int longSegmentsPerCap, Color color, bool depthTest = true, DebugUtil.Style style = DebugUtil.Style.Wireframe)
		{
			if (height < MathUtil.Epsilon || radius < MathUtil.Epsilon)
			{
				return;
			}
			Mesh mesh = null;
			switch (style)
			{
			case DebugUtil.Style.Wireframe:
				mesh = PrimitiveMeshFactory.CapsuleWireframe(latSegmentsPerCap, longSegmentsPerCap, true, true, false);
				break;
			case DebugUtil.Style.SolidColor:
				mesh = PrimitiveMeshFactory.CapsuleSolidColor(latSegmentsPerCap, longSegmentsPerCap, true, true, false);
				break;
			case DebugUtil.Style.FlatShaded:
				mesh = PrimitiveMeshFactory.CapsuleFlatShaded(latSegmentsPerCap, longSegmentsPerCap, true, true, false);
				break;
			case DebugUtil.Style.SmoothShaded:
				mesh = PrimitiveMeshFactory.CapsuleSmoothShaded(latSegmentsPerCap, longSegmentsPerCap, true, true, false);
				break;
			}
			if (mesh == null)
			{
				return;
			}
			Material material = DebugUtil.GetMaterial(style, depthTest, true);
			MaterialPropertyBlock materialPropertyBlock = DebugUtil.GetMaterialPropertyBlock();
			materialPropertyBlock.SetColor("_Color", color);
			materialPropertyBlock.SetVector("_Dimensions", new Vector4(radius, radius, radius, height));
			materialPropertyBlock.SetFloat("_ZBias", (style == DebugUtil.Style.Wireframe) ? DebugUtil.s_wireframeZBias : 0f);
			Graphics.DrawMesh(mesh, center, rotation, material, 0, null, 0, materialPropertyBlock, false, false, false);
		}

		// Token: 0x06005141 RID: 20801 RVA: 0x0018A6B4 File Offset: 0x001888B4
		public static void DrawCapsule(Vector3 point0, Vector3 point1, float radius, int latSegmentsPerCap, int longSegmentsPerCap, Color color, bool depthTest = true, DebugUtil.Style style = DebugUtil.Style.Wireframe)
		{
			Vector3 vector = point1 - point0;
			float magnitude = vector.magnitude;
			if (magnitude < MathUtil.Epsilon)
			{
				return;
			}
			vector.Normalize();
			Vector3 center = 0.5f * (point0 + point1);
			Quaternion rotation = Quaternion.LookRotation(Vector3.Normalize(Vector3.Cross((Mathf.Abs(Vector3.Dot(vector.normalized, Vector3.up)) < 0.5f) ? Vector3.up : Vector3.forward, vector)), vector);
			DebugUtil.DrawCapsule(center, rotation, magnitude, radius, latSegmentsPerCap, longSegmentsPerCap, color, depthTest, style);
		}

		// Token: 0x06005142 RID: 20802 RVA: 0x0018A740 File Offset: 0x00188940
		public static void DrawCapsule2D(Vector3 center, float rotationDeg, float height, float radius, int capSegments, Color color, bool depthTest = true, DebugUtil.Style style = DebugUtil.Style.Wireframe)
		{
			if (height < MathUtil.Epsilon || radius < MathUtil.Epsilon)
			{
				return;
			}
			Mesh mesh = null;
			switch (style)
			{
			case DebugUtil.Style.Wireframe:
				mesh = PrimitiveMeshFactory.Capsule2DWireframe(capSegments);
				break;
			case DebugUtil.Style.SolidColor:
				mesh = PrimitiveMeshFactory.Capsule2DSolidColor(capSegments);
				break;
			case DebugUtil.Style.FlatShaded:
			case DebugUtil.Style.SmoothShaded:
				mesh = PrimitiveMeshFactory.Capsule2DFlatShaded(capSegments);
				break;
			}
			if (mesh == null)
			{
				return;
			}
			Material material = DebugUtil.GetMaterial(style, depthTest, true);
			MaterialPropertyBlock materialPropertyBlock = DebugUtil.GetMaterialPropertyBlock();
			materialPropertyBlock.SetColor("_Color", color);
			materialPropertyBlock.SetVector("_Dimensions", new Vector4(radius, radius, radius, height));
			materialPropertyBlock.SetFloat("_ZBias", (style == DebugUtil.Style.Wireframe) ? DebugUtil.s_wireframeZBias : 0f);
			Graphics.DrawMesh(mesh, center, Quaternion.AngleAxis(rotationDeg, Vector3.forward), material, 0, null, 0, materialPropertyBlock, false, false, false);
		}

		// Token: 0x06005143 RID: 20803 RVA: 0x0018A808 File Offset: 0x00188A08
		public static void DrawCone(Vector3 baseCenter, Quaternion rotation, float height, float radius, int numSegments, Color color, bool depthTest = true, DebugUtil.Style style = DebugUtil.Style.Wireframe)
		{
			if (height < MathUtil.Epsilon || radius < MathUtil.Epsilon)
			{
				return;
			}
			Mesh mesh = null;
			switch (style)
			{
			case DebugUtil.Style.Wireframe:
				mesh = PrimitiveMeshFactory.ConeWireframe(numSegments);
				break;
			case DebugUtil.Style.SolidColor:
				mesh = PrimitiveMeshFactory.ConeSolidColor(numSegments);
				break;
			case DebugUtil.Style.FlatShaded:
				mesh = PrimitiveMeshFactory.ConeFlatShaded(numSegments);
				break;
			case DebugUtil.Style.SmoothShaded:
				mesh = PrimitiveMeshFactory.ConeSmoothShaded(numSegments);
				break;
			}
			if (mesh == null)
			{
				return;
			}
			Material material = DebugUtil.GetMaterial(style, depthTest, false);
			MaterialPropertyBlock materialPropertyBlock = DebugUtil.GetMaterialPropertyBlock();
			materialPropertyBlock.SetColor("_Color", color);
			materialPropertyBlock.SetVector("_Dimensions", new Vector4(radius, height, radius, 0f));
			materialPropertyBlock.SetFloat("_ZBias", (style == DebugUtil.Style.Wireframe) ? DebugUtil.s_wireframeZBias : 0f);
			Graphics.DrawMesh(mesh, baseCenter, rotation, material, 0, null, 0, materialPropertyBlock, false, false, false);
		}

		// Token: 0x06005144 RID: 20804 RVA: 0x0018A8D4 File Offset: 0x00188AD4
		public static void DrawCone(Vector3 baseCenter, Vector3 top, float radius, int numSegments, Color color, bool depthTest = true, DebugUtil.Style style = DebugUtil.Style.Wireframe)
		{
			Vector3 vector = top - baseCenter;
			float magnitude = vector.magnitude;
			if (magnitude < MathUtil.Epsilon)
			{
				return;
			}
			vector.Normalize();
			Quaternion rotation = Quaternion.LookRotation(Vector3.Normalize(Vector3.Cross((Mathf.Abs(Vector3.Dot(vector, Vector3.up)) < 0.5f) ? Vector3.up : Vector3.forward, vector)), vector);
			DebugUtil.DrawCone(baseCenter, rotation, magnitude, radius, numSegments, color, depthTest, style);
		}

		// Token: 0x06005145 RID: 20805 RVA: 0x0018A948 File Offset: 0x00188B48
		public static void DrawArrow(Vector3 from, Vector3 to, float coneRadius, float coneHeight, int numSegments, float stemThickness, Color color, bool depthTest = true, DebugUtil.Style style = DebugUtil.Style.Wireframe)
		{
			Vector3 vector = to - from;
			float magnitude = vector.magnitude;
			if (magnitude < MathUtil.Epsilon)
			{
				return;
			}
			vector.Normalize();
			Quaternion rotation = Quaternion.LookRotation(Vector3.Normalize(Vector3.Cross((Mathf.Abs(Vector3.Dot(vector, Vector3.up)) < 0.5f) ? Vector3.up : Vector3.forward, vector)), vector);
			DebugUtil.DrawCone(to - coneHeight * vector, rotation, coneHeight, coneRadius, numSegments, color, depthTest, style);
			if (stemThickness <= 0f)
			{
				if (style == DebugUtil.Style.Wireframe)
				{
					to -= coneHeight * vector;
				}
				DebugUtil.DrawLine(from, to, color, depthTest);
				return;
			}
			if (coneHeight < magnitude)
			{
				to -= coneHeight * vector;
				DebugUtil.DrawCylinder(from, to, 0.5f * stemThickness, numSegments, color, depthTest, style);
			}
		}

		// Token: 0x06005146 RID: 20806 RVA: 0x0018AA1C File Offset: 0x00188C1C
		public static void DrawArrow(Vector3 from, Vector3 to, float size, Color color, bool depthTest = true, DebugUtil.Style style = DebugUtil.Style.Wireframe)
		{
			DebugUtil.DrawArrow(from, to, 0.5f * size, size, 8, 0f, color, depthTest, style);
		}

		// Token: 0x0400538F RID: 21391
		private static float s_wireframeZBias = 0.0001f;

		// Token: 0x04005390 RID: 21392
		private const int kNormalFlag = 1;

		// Token: 0x04005391 RID: 21393
		private const int kCapShiftScaleFlag = 2;

		// Token: 0x04005392 RID: 21394
		private const int kDepthTestFlag = 4;

		// Token: 0x04005393 RID: 21395
		private static Dictionary<int, Material> s_materialPool;

		// Token: 0x04005394 RID: 21396
		private static MaterialPropertyBlock s_materialProperties;

		// Token: 0x02000C9A RID: 3226
		public enum Style
		{
			// Token: 0x04005396 RID: 21398
			Wireframe,
			// Token: 0x04005397 RID: 21399
			SolidColor,
			// Token: 0x04005398 RID: 21400
			FlatShaded,
			// Token: 0x04005399 RID: 21401
			SmoothShaded
		}
	}
}
