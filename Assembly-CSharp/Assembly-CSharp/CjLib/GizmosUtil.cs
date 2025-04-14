using System;
using UnityEngine;

namespace CjLib
{
	// Token: 0x02000C9B RID: 3227
	public class GizmosUtil
	{
		// Token: 0x06005149 RID: 20809 RVA: 0x0018AA4F File Offset: 0x00188C4F
		public static void DrawLine(Vector3 v0, Vector3 v1, Color color)
		{
			Gizmos.color = color;
			Gizmos.DrawLine(v0, v1);
		}

		// Token: 0x0600514A RID: 20810 RVA: 0x0018AA60 File Offset: 0x00188C60
		public static void DrawLines(Vector3[] aVert, Color color)
		{
			Gizmos.color = color;
			for (int i = 0; i < aVert.Length; i += 2)
			{
				Gizmos.DrawLine(aVert[i], aVert[i + 1]);
			}
		}

		// Token: 0x0600514B RID: 20811 RVA: 0x0018AA98 File Offset: 0x00188C98
		public static void DrawLineStrip(Vector3[] aVert, Color color)
		{
			Gizmos.color = color;
			for (int i = 0; i < aVert.Length; i++)
			{
				Gizmos.DrawLine(aVert[i], aVert[i + 1]);
			}
		}

		// Token: 0x0600514C RID: 20812 RVA: 0x0018AAD0 File Offset: 0x00188CD0
		public static void DrawBox(Vector3 center, Quaternion rotation, Vector3 dimensions, Color color, GizmosUtil.Style style = GizmosUtil.Style.FlatShaded)
		{
			if (dimensions.x < MathUtil.Epsilon || dimensions.y < MathUtil.Epsilon || dimensions.z < MathUtil.Epsilon)
			{
				return;
			}
			Mesh mesh = null;
			if (style != GizmosUtil.Style.Wireframe)
			{
				if (style - GizmosUtil.Style.FlatShaded <= 1)
				{
					mesh = PrimitiveMeshFactory.BoxFlatShaded();
				}
			}
			else
			{
				mesh = PrimitiveMeshFactory.BoxWireframe();
			}
			if (mesh == null)
			{
				return;
			}
			Gizmos.color = color;
			if (style == GizmosUtil.Style.Wireframe)
			{
				Gizmos.DrawWireMesh(mesh, center, rotation, dimensions);
				return;
			}
			Gizmos.DrawMesh(mesh, center, rotation, dimensions);
		}

		// Token: 0x0600514D RID: 20813 RVA: 0x0018AB4C File Offset: 0x00188D4C
		public static void DrawCylinder(Vector3 center, Quaternion rotation, float height, float radius, int numSegments, Color color, GizmosUtil.Style style = GizmosUtil.Style.SmoothShaded)
		{
			if (height < MathUtil.Epsilon || radius < MathUtil.Epsilon)
			{
				return;
			}
			Mesh mesh = null;
			switch (style)
			{
			case GizmosUtil.Style.Wireframe:
				mesh = PrimitiveMeshFactory.CylinderWireframe(numSegments);
				break;
			case GizmosUtil.Style.FlatShaded:
				mesh = PrimitiveMeshFactory.CylinderFlatShaded(numSegments);
				break;
			case GizmosUtil.Style.SmoothShaded:
				mesh = PrimitiveMeshFactory.CylinderSmoothShaded(numSegments);
				break;
			}
			if (mesh == null)
			{
				return;
			}
			Gizmos.color = color;
			if (style == GizmosUtil.Style.Wireframe)
			{
				Gizmos.DrawWireMesh(mesh, center, rotation, new Vector3(radius, height, radius));
				return;
			}
			Gizmos.DrawMesh(mesh, center, rotation, new Vector3(radius, height, radius));
		}

		// Token: 0x0600514E RID: 20814 RVA: 0x0018ABD4 File Offset: 0x00188DD4
		public static void DrawCylinder(Vector3 point0, Vector3 point1, float radius, int numSegments, Color color, GizmosUtil.Style style = GizmosUtil.Style.SmoothShaded)
		{
			Vector3 vector = point1 - point0;
			float magnitude = vector.magnitude;
			if (magnitude < MathUtil.Epsilon)
			{
				return;
			}
			vector.Normalize();
			Vector3 center = 0.5f * (point0 + point1);
			Quaternion rotation = Quaternion.LookRotation(Vector3.Normalize(Vector3.Cross((Vector3.Dot(vector.normalized, Vector3.up) < 0.5f) ? Vector3.up : Vector3.forward, vector)), vector);
			GizmosUtil.DrawCylinder(center, rotation, magnitude, radius, numSegments, color, style);
		}

		// Token: 0x0600514F RID: 20815 RVA: 0x0018AC58 File Offset: 0x00188E58
		public static void DrawSphere(Vector3 center, Quaternion rotation, float radius, int latSegments, int longSegments, Color color, GizmosUtil.Style style = GizmosUtil.Style.SmoothShaded)
		{
			if (radius < MathUtil.Epsilon)
			{
				return;
			}
			Mesh mesh = null;
			switch (style)
			{
			case GizmosUtil.Style.Wireframe:
				mesh = PrimitiveMeshFactory.SphereWireframe(latSegments, longSegments);
				break;
			case GizmosUtil.Style.FlatShaded:
				mesh = PrimitiveMeshFactory.SphereFlatShaded(latSegments, longSegments);
				break;
			case GizmosUtil.Style.SmoothShaded:
				mesh = PrimitiveMeshFactory.SphereSmoothShaded(latSegments, longSegments);
				break;
			}
			if (mesh == null)
			{
				return;
			}
			Gizmos.color = color;
			if (style == GizmosUtil.Style.Wireframe)
			{
				Gizmos.DrawWireMesh(mesh, center, rotation, new Vector3(radius, radius, radius));
				return;
			}
			Gizmos.DrawMesh(mesh, center, rotation, new Vector3(radius, radius, radius));
		}

		// Token: 0x06005150 RID: 20816 RVA: 0x0018ACDA File Offset: 0x00188EDA
		public static void DrawSphere(Vector3 center, float radius, int latSegments, int longSegments, Color color, GizmosUtil.Style style = GizmosUtil.Style.SmoothShaded)
		{
			GizmosUtil.DrawSphere(center, Quaternion.identity, radius, latSegments, longSegments, color, style);
		}

		// Token: 0x06005151 RID: 20817 RVA: 0x0018ACF0 File Offset: 0x00188EF0
		public static void DrawCapsule(Vector3 center, Quaternion rotation, float height, float radius, int latSegmentsPerCap, int longSegmentsPerCap, Color color, GizmosUtil.Style style = GizmosUtil.Style.SmoothShaded)
		{
			if (height < MathUtil.Epsilon || radius < MathUtil.Epsilon)
			{
				return;
			}
			Mesh mesh = null;
			Mesh mesh2 = null;
			switch (style)
			{
			case GizmosUtil.Style.Wireframe:
				mesh = PrimitiveMeshFactory.CapsuleWireframe(latSegmentsPerCap, longSegmentsPerCap, true, true, false);
				mesh2 = PrimitiveMeshFactory.CapsuleWireframe(latSegmentsPerCap, longSegmentsPerCap, false, false, true);
				break;
			case GizmosUtil.Style.FlatShaded:
				mesh = PrimitiveMeshFactory.CapsuleFlatShaded(latSegmentsPerCap, longSegmentsPerCap, true, true, false);
				mesh2 = PrimitiveMeshFactory.CapsuleFlatShaded(latSegmentsPerCap, longSegmentsPerCap, false, false, true);
				break;
			case GizmosUtil.Style.SmoothShaded:
				mesh = PrimitiveMeshFactory.CapsuleSmoothShaded(latSegmentsPerCap, longSegmentsPerCap, true, true, false);
				mesh2 = PrimitiveMeshFactory.CapsuleSmoothShaded(latSegmentsPerCap, longSegmentsPerCap, false, false, true);
				break;
			}
			if (mesh == null || mesh2 == null)
			{
				return;
			}
			Vector3 vector = rotation * Vector3.up;
			Vector3 b = 0.5f * (height - radius) * vector;
			Vector3 position = center + b;
			Vector3 position2 = center - b;
			Quaternion rotation2 = Quaternion.AngleAxis(180f, vector) * rotation;
			Gizmos.color = color;
			if (style == GizmosUtil.Style.Wireframe)
			{
				Gizmos.DrawWireMesh(mesh, position, rotation, new Vector3(radius, radius, radius));
				Gizmos.DrawWireMesh(mesh, position2, rotation2, new Vector3(-radius, -radius, radius));
				Gizmos.DrawWireMesh(mesh2, center, rotation, new Vector3(radius, height, radius));
				return;
			}
			Gizmos.DrawMesh(mesh, position, rotation, new Vector3(radius, radius, radius));
			Gizmos.DrawMesh(mesh, position2, rotation2, new Vector3(-radius, -radius, radius));
			Gizmos.DrawMesh(mesh2, center, rotation, new Vector3(radius, height, radius));
		}

		// Token: 0x06005152 RID: 20818 RVA: 0x0018AE44 File Offset: 0x00189044
		public static void DrawCapsule(Vector3 point0, Vector3 point1, float radius, int latSegmentsPerCap, int longSegmentsPerCap, Color color, GizmosUtil.Style style = GizmosUtil.Style.SmoothShaded)
		{
			Vector3 vector = point1 - point0;
			float magnitude = vector.magnitude;
			if (magnitude < MathUtil.Epsilon)
			{
				return;
			}
			vector.Normalize();
			Vector3 center = 0.5f * (point0 + point1);
			Quaternion rotation = Quaternion.LookRotation(Vector3.Normalize(Vector3.Cross((Vector3.Dot(vector.normalized, Vector3.up) < 0.5f) ? Vector3.up : Vector3.forward, vector)), vector);
			GizmosUtil.DrawCapsule(center, rotation, magnitude, radius, latSegmentsPerCap, longSegmentsPerCap, color, style);
		}

		// Token: 0x06005153 RID: 20819 RVA: 0x0018AEC8 File Offset: 0x001890C8
		public static void DrawCone(Vector3 baseCenter, Quaternion rotation, float height, float radius, int numSegments, Color color, GizmosUtil.Style style = GizmosUtil.Style.FlatShaded)
		{
			if (height < MathUtil.Epsilon || radius < MathUtil.Epsilon)
			{
				return;
			}
			Mesh mesh = null;
			switch (style)
			{
			case GizmosUtil.Style.Wireframe:
				mesh = PrimitiveMeshFactory.ConeWireframe(numSegments);
				break;
			case GizmosUtil.Style.FlatShaded:
				mesh = PrimitiveMeshFactory.ConeFlatShaded(numSegments);
				break;
			case GizmosUtil.Style.SmoothShaded:
				mesh = PrimitiveMeshFactory.ConeSmoothShaded(numSegments);
				break;
			}
			if (mesh == null)
			{
				return;
			}
			Gizmos.color = color;
			if (style == GizmosUtil.Style.Wireframe)
			{
				Gizmos.DrawWireMesh(mesh, baseCenter, rotation, new Vector3(radius, height, radius));
				return;
			}
			Gizmos.DrawMesh(mesh, baseCenter, rotation, new Vector3(radius, height, radius));
		}

		// Token: 0x06005154 RID: 20820 RVA: 0x0018AF50 File Offset: 0x00189150
		public static void DrawCone(Vector3 baseCenter, Vector3 top, float radius, int numSegments, Color color, GizmosUtil.Style style = GizmosUtil.Style.FlatShaded)
		{
			Vector3 vector = top - baseCenter;
			float magnitude = vector.magnitude;
			if (magnitude < MathUtil.Epsilon)
			{
				return;
			}
			vector.Normalize();
			Quaternion rotation = Quaternion.LookRotation(Vector3.Normalize(Vector3.Cross((Vector3.Dot(vector, Vector3.up) < 0.5f) ? Vector3.up : Vector3.forward, vector)), vector);
			GizmosUtil.DrawCone(baseCenter, rotation, magnitude, radius, numSegments, color, style);
		}

		// Token: 0x06005155 RID: 20821 RVA: 0x0018AFBC File Offset: 0x001891BC
		public static void DrawArrow(Vector3 from, Vector3 to, float coneRadius, float coneHeight, int numSegments, float stemThickness, Color color, GizmosUtil.Style style = GizmosUtil.Style.FlatShaded)
		{
			Vector3 vector = to - from;
			float magnitude = vector.magnitude;
			if (magnitude < MathUtil.Epsilon)
			{
				return;
			}
			vector.Normalize();
			Quaternion rotation = Quaternion.LookRotation(Vector3.Normalize(Vector3.Cross((Vector3.Dot(vector, Vector3.up) < 0.5f) ? Vector3.up : Vector3.forward, vector)), vector);
			GizmosUtil.DrawCone(to - coneHeight * vector, rotation, coneHeight, coneRadius, numSegments, color, style);
			if (stemThickness <= 0f)
			{
				if (style != GizmosUtil.Style.Wireframe)
				{
					to -= coneHeight * vector;
				}
				GizmosUtil.DrawLine(from, to, color);
				return;
			}
			if (coneHeight < magnitude)
			{
				to -= coneHeight * vector;
				GizmosUtil.DrawCylinder(from, to, 0.5f * stemThickness, numSegments, color, style);
			}
		}

		// Token: 0x06005156 RID: 20822 RVA: 0x0018B082 File Offset: 0x00189282
		public static void DrawArrow(Vector3 from, Vector3 to, float size, Color color, GizmosUtil.Style style = GizmosUtil.Style.FlatShaded)
		{
			GizmosUtil.DrawArrow(from, to, 0.5f * size, size, 8, 0f, color, style);
		}

		// Token: 0x02000C9C RID: 3228
		public enum Style
		{
			// Token: 0x0400539B RID: 21403
			Wireframe,
			// Token: 0x0400539C RID: 21404
			FlatShaded,
			// Token: 0x0400539D RID: 21405
			SmoothShaded
		}
	}
}
