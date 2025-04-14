using System;
using System.Collections.Generic;
using UnityEngine;

namespace CjLib
{
	// Token: 0x02000C96 RID: 3222
	public class DebugUtil
	{
		// Token: 0x0600511E RID: 20766 RVA: 0x0018969C File Offset: 0x0018789C
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

		// Token: 0x0600511F RID: 20767 RVA: 0x00189755 File Offset: 0x00187955
		private static MaterialPropertyBlock GetMaterialPropertyBlock()
		{
			if (DebugUtil.s_materialProperties == null)
			{
				return DebugUtil.s_materialProperties = new MaterialPropertyBlock();
			}
			return DebugUtil.s_materialProperties;
		}

		// Token: 0x06005120 RID: 20768 RVA: 0x00189770 File Offset: 0x00187970
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

		// Token: 0x06005121 RID: 20769 RVA: 0x001897F8 File Offset: 0x001879F8
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

		// Token: 0x06005122 RID: 20770 RVA: 0x00189880 File Offset: 0x00187A80
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

		// Token: 0x06005123 RID: 20771 RVA: 0x00189908 File Offset: 0x00187B08
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

		// Token: 0x06005124 RID: 20772 RVA: 0x00189994 File Offset: 0x00187B94
		public static void DrawLocator(Vector3 position, Vector3 right, Vector3 up, Vector3 forward, Color rightColor, Color upColor, Color forwardColor, float size = 0.5f)
		{
			DebugUtil.DrawLine(position, position + right * size, rightColor, true);
			DebugUtil.DrawLine(position, position + up * size, upColor, true);
			DebugUtil.DrawLine(position, position + forward * size, forwardColor, true);
		}

		// Token: 0x06005125 RID: 20773 RVA: 0x001899E6 File Offset: 0x00187BE6
		public static void DrawLocator(Vector3 position, Vector3 right, Vector3 up, Vector3 forward, float size = 0.5f)
		{
			DebugUtil.DrawLocator(position, right, up, forward, Color.red, Color.green, Color.blue, size);
		}

		// Token: 0x06005126 RID: 20774 RVA: 0x00189A04 File Offset: 0x00187C04
		public static void DrawLocator(Vector3 position, Quaternion rotation, Color rightColor, Color upColor, Color forwardColor, float size = 0.5f)
		{
			Vector3 right = rotation * Vector3.right;
			Vector3 up = rotation * Vector3.up;
			Vector3 forward = rotation * Vector3.forward;
			DebugUtil.DrawLocator(position, right, up, forward, rightColor, upColor, forwardColor, size);
		}

		// Token: 0x06005127 RID: 20775 RVA: 0x00189A44 File Offset: 0x00187C44
		public static void DrawLocator(Vector3 position, Quaternion rotation, float size = 0.5f)
		{
			DebugUtil.DrawLocator(position, rotation, Color.red, Color.green, Color.blue, size);
		}

		// Token: 0x06005128 RID: 20776 RVA: 0x00189A60 File Offset: 0x00187C60
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

		// Token: 0x06005129 RID: 20777 RVA: 0x00189B40 File Offset: 0x00187D40
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

		// Token: 0x0600512A RID: 20778 RVA: 0x00189C14 File Offset: 0x00187E14
		public static void DrawRect2D(Vector3 center, float rotationDeg, Vector2 dimensions, Color color, bool depthTest = true, DebugUtil.Style style = DebugUtil.Style.Wireframe)
		{
			Quaternion rotation = Quaternion.AngleAxis(rotationDeg, Vector3.forward) * Quaternion.AngleAxis(90f, Vector3.right);
			DebugUtil.DrawRect(center, rotation, dimensions, color, depthTest, style);
		}

		// Token: 0x0600512B RID: 20779 RVA: 0x00189C50 File Offset: 0x00187E50
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

		// Token: 0x0600512C RID: 20780 RVA: 0x00189D08 File Offset: 0x00187F08
		public static void DrawCircle(Vector3 center, Vector3 normal, float radius, int numSegments, Color color, bool depthTest = true, DebugUtil.Style style = DebugUtil.Style.Wireframe)
		{
			Quaternion rotation = Quaternion.LookRotation(Vector3.Normalize(Vector3.Cross((Mathf.Abs(Vector3.Dot(normal, Vector3.up)) < 0.5f) ? Vector3.up : Vector3.forward, normal)), normal);
			DebugUtil.DrawCircle(center, rotation, radius, numSegments, color, depthTest, style);
		}

		// Token: 0x0600512D RID: 20781 RVA: 0x00189D59 File Offset: 0x00187F59
		public static void DrawCircle2D(Vector3 center, float radius, int numSegments, Color color, bool depthTest = true, DebugUtil.Style style = DebugUtil.Style.Wireframe)
		{
			DebugUtil.DrawCircle(center, Vector3.forward, radius, numSegments, color, depthTest, style);
		}

		// Token: 0x0600512E RID: 20782 RVA: 0x00189D70 File Offset: 0x00187F70
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

		// Token: 0x0600512F RID: 20783 RVA: 0x00189E38 File Offset: 0x00188038
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

		// Token: 0x06005130 RID: 20784 RVA: 0x00189EC0 File Offset: 0x001880C0
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

		// Token: 0x06005131 RID: 20785 RVA: 0x00189F86 File Offset: 0x00188186
		public static void DrawSphere(Vector3 center, float radius, int latSegments, int longSegments, Color color, bool depthTest = true, DebugUtil.Style style = DebugUtil.Style.Wireframe)
		{
			DebugUtil.DrawSphere(center, Quaternion.identity, radius, latSegments, longSegments, color, depthTest, style);
		}

		// Token: 0x06005132 RID: 20786 RVA: 0x00189F9C File Offset: 0x0018819C
		public static void DrawSphereTripleCircles(Vector3 center, Quaternion rotation, float radius, int numSegments, Color color, bool depthTest = true, DebugUtil.Style style = DebugUtil.Style.Wireframe)
		{
			Vector3 normal = rotation * Vector3.right;
			Vector3 normal2 = rotation * Vector3.up;
			Vector3 normal3 = rotation * Vector3.forward;
			DebugUtil.DrawCircle(center, normal, radius, numSegments, color, depthTest, style);
			DebugUtil.DrawCircle(center, normal2, radius, numSegments, color, depthTest, style);
			DebugUtil.DrawCircle(center, normal3, radius, numSegments, color, depthTest, style);
		}

		// Token: 0x06005133 RID: 20787 RVA: 0x00189FFA File Offset: 0x001881FA
		public static void DrawSphereTripleCircles(Vector3 center, float radius, int numSegments, Color color, bool depthTest = true, DebugUtil.Style style = DebugUtil.Style.Wireframe)
		{
			DebugUtil.DrawSphereTripleCircles(center, Quaternion.identity, radius, numSegments, color, depthTest, style);
		}

		// Token: 0x06005134 RID: 20788 RVA: 0x0018A010 File Offset: 0x00188210
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

		// Token: 0x06005135 RID: 20789 RVA: 0x0018A0EC File Offset: 0x001882EC
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

		// Token: 0x06005136 RID: 20790 RVA: 0x0018A178 File Offset: 0x00188378
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

		// Token: 0x06005137 RID: 20791 RVA: 0x0018A240 File Offset: 0x00188440
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

		// Token: 0x06005138 RID: 20792 RVA: 0x0018A30C File Offset: 0x0018850C
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

		// Token: 0x06005139 RID: 20793 RVA: 0x0018A380 File Offset: 0x00188580
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

		// Token: 0x0600513A RID: 20794 RVA: 0x0018A454 File Offset: 0x00188654
		public static void DrawArrow(Vector3 from, Vector3 to, float size, Color color, bool depthTest = true, DebugUtil.Style style = DebugUtil.Style.Wireframe)
		{
			DebugUtil.DrawArrow(from, to, 0.5f * size, size, 8, 0f, color, depthTest, style);
		}

		// Token: 0x0400537D RID: 21373
		private static float s_wireframeZBias = 0.0001f;

		// Token: 0x0400537E RID: 21374
		private const int kNormalFlag = 1;

		// Token: 0x0400537F RID: 21375
		private const int kCapShiftScaleFlag = 2;

		// Token: 0x04005380 RID: 21376
		private const int kDepthTestFlag = 4;

		// Token: 0x04005381 RID: 21377
		private static Dictionary<int, Material> s_materialPool;

		// Token: 0x04005382 RID: 21378
		private static MaterialPropertyBlock s_materialProperties;

		// Token: 0x02000C97 RID: 3223
		public enum Style
		{
			// Token: 0x04005384 RID: 21380
			Wireframe,
			// Token: 0x04005385 RID: 21381
			SolidColor,
			// Token: 0x04005386 RID: 21382
			FlatShaded,
			// Token: 0x04005387 RID: 21383
			SmoothShaded
		}
	}
}
