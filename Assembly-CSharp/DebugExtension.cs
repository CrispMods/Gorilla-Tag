using System;
using System.Reflection;
using UnityEngine;

// Token: 0x0200007F RID: 127
public static class DebugExtension
{
	// Token: 0x06000335 RID: 821 RVA: 0x00076FDC File Offset: 0x000751DC
	public static void DebugPoint(Vector3 position, Color color, float scale = 1f, float duration = 0f, bool depthTest = true)
	{
		color = ((color == default(Color)) ? Color.white : color);
		Debug.DrawRay(position + Vector3.up * (scale * 0.5f), -Vector3.up * scale, color, duration, depthTest);
		Debug.DrawRay(position + Vector3.right * (scale * 0.5f), -Vector3.right * scale, color, duration, depthTest);
		Debug.DrawRay(position + Vector3.forward * (scale * 0.5f), -Vector3.forward * scale, color, duration, depthTest);
	}

	// Token: 0x06000336 RID: 822 RVA: 0x000327EA File Offset: 0x000309EA
	public static void DebugPoint(Vector3 position, float scale = 1f, float duration = 0f, bool depthTest = true)
	{
		DebugExtension.DebugPoint(position, Color.white, scale, duration, depthTest);
	}

	// Token: 0x06000337 RID: 823 RVA: 0x00077094 File Offset: 0x00075294
	public static void DebugBounds(Bounds bounds, Color color, float duration = 0f, bool depthTest = true)
	{
		Vector3 center = bounds.center;
		float x = bounds.extents.x;
		float y = bounds.extents.y;
		float z = bounds.extents.z;
		Vector3 start = center + new Vector3(x, y, z);
		Vector3 vector = center + new Vector3(x, y, -z);
		Vector3 vector2 = center + new Vector3(-x, y, z);
		Vector3 vector3 = center + new Vector3(-x, y, -z);
		Vector3 vector4 = center + new Vector3(x, -y, z);
		Vector3 end = center + new Vector3(x, -y, -z);
		Vector3 vector5 = center + new Vector3(-x, -y, z);
		Vector3 vector6 = center + new Vector3(-x, -y, -z);
		Debug.DrawLine(start, vector2, color, duration, depthTest);
		Debug.DrawLine(start, vector, color, duration, depthTest);
		Debug.DrawLine(vector2, vector3, color, duration, depthTest);
		Debug.DrawLine(vector, vector3, color, duration, depthTest);
		Debug.DrawLine(start, vector4, color, duration, depthTest);
		Debug.DrawLine(vector, end, color, duration, depthTest);
		Debug.DrawLine(vector2, vector5, color, duration, depthTest);
		Debug.DrawLine(vector3, vector6, color, duration, depthTest);
		Debug.DrawLine(vector4, vector5, color, duration, depthTest);
		Debug.DrawLine(vector4, end, color, duration, depthTest);
		Debug.DrawLine(vector5, vector6, color, duration, depthTest);
		Debug.DrawLine(vector6, end, color, duration, depthTest);
	}

	// Token: 0x06000338 RID: 824 RVA: 0x000327FA File Offset: 0x000309FA
	public static void DebugBounds(Bounds bounds, float duration = 0f, bool depthTest = true)
	{
		DebugExtension.DebugBounds(bounds, Color.white, duration, depthTest);
	}

	// Token: 0x06000339 RID: 825 RVA: 0x000771E8 File Offset: 0x000753E8
	public static void DebugLocalCube(Transform transform, Vector3 size, Color color, Vector3 center = default(Vector3), float duration = 0f, bool depthTest = true)
	{
		Vector3 vector = transform.TransformPoint(center + -size * 0.5f);
		Vector3 vector2 = transform.TransformPoint(center + new Vector3(size.x, -size.y, -size.z) * 0.5f);
		Vector3 vector3 = transform.TransformPoint(center + new Vector3(size.x, -size.y, size.z) * 0.5f);
		Vector3 vector4 = transform.TransformPoint(center + new Vector3(-size.x, -size.y, size.z) * 0.5f);
		Vector3 vector5 = transform.TransformPoint(center + new Vector3(-size.x, size.y, -size.z) * 0.5f);
		Vector3 vector6 = transform.TransformPoint(center + new Vector3(size.x, size.y, -size.z) * 0.5f);
		Vector3 vector7 = transform.TransformPoint(center + size * 0.5f);
		Vector3 vector8 = transform.TransformPoint(center + new Vector3(-size.x, size.y, size.z) * 0.5f);
		Debug.DrawLine(vector, vector2, color, duration, depthTest);
		Debug.DrawLine(vector2, vector3, color, duration, depthTest);
		Debug.DrawLine(vector3, vector4, color, duration, depthTest);
		Debug.DrawLine(vector4, vector, color, duration, depthTest);
		Debug.DrawLine(vector5, vector6, color, duration, depthTest);
		Debug.DrawLine(vector6, vector7, color, duration, depthTest);
		Debug.DrawLine(vector7, vector8, color, duration, depthTest);
		Debug.DrawLine(vector8, vector5, color, duration, depthTest);
		Debug.DrawLine(vector, vector5, color, duration, depthTest);
		Debug.DrawLine(vector2, vector6, color, duration, depthTest);
		Debug.DrawLine(vector3, vector7, color, duration, depthTest);
		Debug.DrawLine(vector4, vector8, color, duration, depthTest);
	}

	// Token: 0x0600033A RID: 826 RVA: 0x00032809 File Offset: 0x00030A09
	public static void DebugLocalCube(Transform transform, Vector3 size, Vector3 center = default(Vector3), float duration = 0f, bool depthTest = true)
	{
		DebugExtension.DebugLocalCube(transform, size, Color.white, center, duration, depthTest);
	}

	// Token: 0x0600033B RID: 827 RVA: 0x000773E8 File Offset: 0x000755E8
	public static void DebugLocalCube(Matrix4x4 space, Vector3 size, Color color, Vector3 center = default(Vector3), float duration = 0f, bool depthTest = true)
	{
		color = ((color == default(Color)) ? Color.white : color);
		Vector3 vector = space.MultiplyPoint3x4(center + -size * 0.5f);
		Vector3 vector2 = space.MultiplyPoint3x4(center + new Vector3(size.x, -size.y, -size.z) * 0.5f);
		Vector3 vector3 = space.MultiplyPoint3x4(center + new Vector3(size.x, -size.y, size.z) * 0.5f);
		Vector3 vector4 = space.MultiplyPoint3x4(center + new Vector3(-size.x, -size.y, size.z) * 0.5f);
		Vector3 vector5 = space.MultiplyPoint3x4(center + new Vector3(-size.x, size.y, -size.z) * 0.5f);
		Vector3 vector6 = space.MultiplyPoint3x4(center + new Vector3(size.x, size.y, -size.z) * 0.5f);
		Vector3 vector7 = space.MultiplyPoint3x4(center + size * 0.5f);
		Vector3 vector8 = space.MultiplyPoint3x4(center + new Vector3(-size.x, size.y, size.z) * 0.5f);
		Debug.DrawLine(vector, vector2, color, duration, depthTest);
		Debug.DrawLine(vector2, vector3, color, duration, depthTest);
		Debug.DrawLine(vector3, vector4, color, duration, depthTest);
		Debug.DrawLine(vector4, vector, color, duration, depthTest);
		Debug.DrawLine(vector5, vector6, color, duration, depthTest);
		Debug.DrawLine(vector6, vector7, color, duration, depthTest);
		Debug.DrawLine(vector7, vector8, color, duration, depthTest);
		Debug.DrawLine(vector8, vector5, color, duration, depthTest);
		Debug.DrawLine(vector, vector5, color, duration, depthTest);
		Debug.DrawLine(vector2, vector6, color, duration, depthTest);
		Debug.DrawLine(vector3, vector7, color, duration, depthTest);
		Debug.DrawLine(vector4, vector8, color, duration, depthTest);
	}

	// Token: 0x0600033C RID: 828 RVA: 0x0003281B File Offset: 0x00030A1B
	public static void DebugLocalCube(Matrix4x4 space, Vector3 size, Vector3 center = default(Vector3), float duration = 0f, bool depthTest = true)
	{
		DebugExtension.DebugLocalCube(space, size, Color.white, center, duration, depthTest);
	}

	// Token: 0x0600033D RID: 829 RVA: 0x0007760C File Offset: 0x0007580C
	public static void DebugCircle(Vector3 position, Vector3 up, Color color, float radius = 1f, float duration = 0f, bool depthTest = true)
	{
		Vector3 vector = up.normalized * radius;
		Vector3 vector2 = Vector3.Slerp(vector, -vector, 0.5f);
		Vector3 vector3 = Vector3.Cross(vector, vector2).normalized * radius;
		Matrix4x4 matrix4x = default(Matrix4x4);
		matrix4x[0] = vector3.x;
		matrix4x[1] = vector3.y;
		matrix4x[2] = vector3.z;
		matrix4x[4] = vector.x;
		matrix4x[5] = vector.y;
		matrix4x[6] = vector.z;
		matrix4x[8] = vector2.x;
		matrix4x[9] = vector2.y;
		matrix4x[10] = vector2.z;
		Vector3 start = position + matrix4x.MultiplyPoint3x4(new Vector3(Mathf.Cos(0f), 0f, Mathf.Sin(0f)));
		Vector3 vector4 = Vector3.zero;
		color = ((color == default(Color)) ? Color.white : color);
		for (int i = 0; i < 91; i++)
		{
			vector4.x = Mathf.Cos((float)(i * 4) * 0.017453292f);
			vector4.z = Mathf.Sin((float)(i * 4) * 0.017453292f);
			vector4.y = 0f;
			vector4 = position + matrix4x.MultiplyPoint3x4(vector4);
			Debug.DrawLine(start, vector4, color, duration, depthTest);
			start = vector4;
		}
	}

	// Token: 0x0600033E RID: 830 RVA: 0x0003282D File Offset: 0x00030A2D
	public static void DebugCircle(Vector3 position, Color color, float radius = 1f, float duration = 0f, bool depthTest = true)
	{
		DebugExtension.DebugCircle(position, Vector3.up, color, radius, duration, depthTest);
	}

	// Token: 0x0600033F RID: 831 RVA: 0x0003283F File Offset: 0x00030A3F
	public static void DebugCircle(Vector3 position, Vector3 up, float radius = 1f, float duration = 0f, bool depthTest = true)
	{
		DebugExtension.DebugCircle(position, up, Color.white, radius, duration, depthTest);
	}

	// Token: 0x06000340 RID: 832 RVA: 0x00032851 File Offset: 0x00030A51
	public static void DebugCircle(Vector3 position, float radius = 1f, float duration = 0f, bool depthTest = true)
	{
		DebugExtension.DebugCircle(position, Vector3.up, Color.white, radius, duration, depthTest);
	}

	// Token: 0x06000341 RID: 833 RVA: 0x00077798 File Offset: 0x00075998
	public static void DebugWireSphere(Vector3 position, Color color, float radius = 1f, float duration = 0f, bool depthTest = true)
	{
		float num = 10f;
		Vector3 start = new Vector3(position.x, position.y + radius * Mathf.Sin(0f), position.z + radius * Mathf.Cos(0f));
		Vector3 start2 = new Vector3(position.x + radius * Mathf.Cos(0f), position.y, position.z + radius * Mathf.Sin(0f));
		Vector3 start3 = new Vector3(position.x + radius * Mathf.Cos(0f), position.y + radius * Mathf.Sin(0f), position.z);
		for (int i = 1; i < 37; i++)
		{
			Vector3 vector = new Vector3(position.x, position.y + radius * Mathf.Sin(num * (float)i * 0.017453292f), position.z + radius * Mathf.Cos(num * (float)i * 0.017453292f));
			Vector3 vector2 = new Vector3(position.x + radius * Mathf.Cos(num * (float)i * 0.017453292f), position.y, position.z + radius * Mathf.Sin(num * (float)i * 0.017453292f));
			Vector3 vector3 = new Vector3(position.x + radius * Mathf.Cos(num * (float)i * 0.017453292f), position.y + radius * Mathf.Sin(num * (float)i * 0.017453292f), position.z);
			Debug.DrawLine(start, vector, color, duration, depthTest);
			Debug.DrawLine(start2, vector2, color, duration, depthTest);
			Debug.DrawLine(start3, vector3, color, duration, depthTest);
			start = vector;
			start2 = vector2;
			start3 = vector3;
		}
	}

	// Token: 0x06000342 RID: 834 RVA: 0x00032866 File Offset: 0x00030A66
	public static void DebugWireSphere(Vector3 position, float radius = 1f, float duration = 0f, bool depthTest = true)
	{
		DebugExtension.DebugWireSphere(position, Color.white, radius, duration, depthTest);
	}

	// Token: 0x06000343 RID: 835 RVA: 0x00077948 File Offset: 0x00075B48
	public static void DebugCylinder(Vector3 start, Vector3 end, Color color, float radius = 1f, float duration = 0f, bool depthTest = true)
	{
		Vector3 vector = (end - start).normalized * radius;
		Vector3 vector2 = Vector3.Slerp(vector, -vector, 0.5f);
		Vector3 b = Vector3.Cross(vector, vector2).normalized * radius;
		DebugExtension.DebugCircle(start, vector, color, radius, duration, depthTest);
		DebugExtension.DebugCircle(end, -vector, color, radius, duration, depthTest);
		DebugExtension.DebugCircle((start + end) * 0.5f, vector, color, radius, duration, depthTest);
		Debug.DrawLine(start + b, end + b, color, duration, depthTest);
		Debug.DrawLine(start - b, end - b, color, duration, depthTest);
		Debug.DrawLine(start + vector2, end + vector2, color, duration, depthTest);
		Debug.DrawLine(start - vector2, end - vector2, color, duration, depthTest);
		Debug.DrawLine(start - b, start + b, color, duration, depthTest);
		Debug.DrawLine(start - vector2, start + vector2, color, duration, depthTest);
		Debug.DrawLine(end - b, end + b, color, duration, depthTest);
		Debug.DrawLine(end - vector2, end + vector2, color, duration, depthTest);
	}

	// Token: 0x06000344 RID: 836 RVA: 0x00032876 File Offset: 0x00030A76
	public static void DebugCylinder(Vector3 start, Vector3 end, float radius = 1f, float duration = 0f, bool depthTest = true)
	{
		DebugExtension.DebugCylinder(start, end, Color.white, radius, duration, depthTest);
	}

	// Token: 0x06000345 RID: 837 RVA: 0x00077A90 File Offset: 0x00075C90
	public static void DebugCone(Vector3 position, Vector3 direction, Color color, float angle = 45f, float duration = 0f, bool depthTest = true)
	{
		float magnitude = direction.magnitude;
		Vector3 vector = direction;
		Vector3 vector2 = Vector3.Slerp(vector, -vector, 0.5f);
		Vector3 vector3 = Vector3.Cross(vector, vector2).normalized * magnitude;
		direction = direction.normalized;
		Vector3 direction2 = Vector3.Slerp(vector, vector2, angle / 90f);
		Plane plane = new Plane(-direction, position + vector);
		Ray ray = new Ray(position, direction2);
		float num;
		plane.Raycast(ray, out num);
		Debug.DrawRay(position, direction2.normalized * num, color);
		Debug.DrawRay(position, Vector3.Slerp(vector, -vector2, angle / 90f).normalized * num, color, duration, depthTest);
		Debug.DrawRay(position, Vector3.Slerp(vector, vector3, angle / 90f).normalized * num, color, duration, depthTest);
		Debug.DrawRay(position, Vector3.Slerp(vector, -vector3, angle / 90f).normalized * num, color, duration, depthTest);
		DebugExtension.DebugCircle(position + vector, direction, color, (vector - direction2.normalized * num).magnitude, duration, depthTest);
		DebugExtension.DebugCircle(position + vector * 0.5f, direction, color, (vector * 0.5f - direction2.normalized * (num * 0.5f)).magnitude, duration, depthTest);
	}

	// Token: 0x06000346 RID: 838 RVA: 0x00032888 File Offset: 0x00030A88
	public static void DebugCone(Vector3 position, Vector3 direction, float angle = 45f, float duration = 0f, bool depthTest = true)
	{
		DebugExtension.DebugCone(position, direction, Color.white, angle, duration, depthTest);
	}

	// Token: 0x06000347 RID: 839 RVA: 0x0003289A File Offset: 0x00030A9A
	public static void DebugCone(Vector3 position, Color color, float angle = 45f, float duration = 0f, bool depthTest = true)
	{
		DebugExtension.DebugCone(position, Vector3.up, color, angle, duration, depthTest);
	}

	// Token: 0x06000348 RID: 840 RVA: 0x000328AC File Offset: 0x00030AAC
	public static void DebugCone(Vector3 position, float angle = 45f, float duration = 0f, bool depthTest = true)
	{
		DebugExtension.DebugCone(position, Vector3.up, Color.white, angle, duration, depthTest);
	}

	// Token: 0x06000349 RID: 841 RVA: 0x000328C1 File Offset: 0x00030AC1
	public static void DebugArrow(Vector3 position, Vector3 direction, Color color, float duration = 0f, bool depthTest = true)
	{
		Debug.DrawRay(position, direction, color, duration, depthTest);
		DebugExtension.DebugCone(position + direction, -direction * 0.333f, color, 15f, duration, depthTest);
	}

	// Token: 0x0600034A RID: 842 RVA: 0x000328F3 File Offset: 0x00030AF3
	public static void DebugArrow(Vector3 position, Vector3 direction, float duration = 0f, bool depthTest = true)
	{
		DebugExtension.DebugArrow(position, direction, Color.white, duration, depthTest);
	}

	// Token: 0x0600034B RID: 843 RVA: 0x00077C28 File Offset: 0x00075E28
	public static void DebugCapsule(Vector3 start, Vector3 end, Color color, float radius = 1f, float duration = 0f, bool depthTest = true)
	{
		Vector3 vector = (end - start).normalized * radius;
		Vector3 vector2 = Vector3.Slerp(vector, -vector, 0.5f);
		Vector3 vector3 = Vector3.Cross(vector, vector2).normalized * radius;
		float magnitude = (start - end).magnitude;
		float d = Mathf.Max(0f, magnitude * 0.5f - radius);
		Vector3 vector4 = (end + start) * 0.5f;
		start = vector4 + (start - vector4).normalized * d;
		end = vector4 + (end - vector4).normalized * d;
		DebugExtension.DebugCircle(start, vector, color, radius, duration, depthTest);
		DebugExtension.DebugCircle(end, -vector, color, radius, duration, depthTest);
		Debug.DrawLine(start + vector3, end + vector3, color, duration, depthTest);
		Debug.DrawLine(start - vector3, end - vector3, color, duration, depthTest);
		Debug.DrawLine(start + vector2, end + vector2, color, duration, depthTest);
		Debug.DrawLine(start - vector2, end - vector2, color, duration, depthTest);
		for (int i = 1; i < 26; i++)
		{
			Debug.DrawLine(Vector3.Slerp(vector3, -vector, (float)i / 25f) + start, Vector3.Slerp(vector3, -vector, (float)(i - 1) / 25f) + start, color, duration, depthTest);
			Debug.DrawLine(Vector3.Slerp(-vector3, -vector, (float)i / 25f) + start, Vector3.Slerp(-vector3, -vector, (float)(i - 1) / 25f) + start, color, duration, depthTest);
			Debug.DrawLine(Vector3.Slerp(vector2, -vector, (float)i / 25f) + start, Vector3.Slerp(vector2, -vector, (float)(i - 1) / 25f) + start, color, duration, depthTest);
			Debug.DrawLine(Vector3.Slerp(-vector2, -vector, (float)i / 25f) + start, Vector3.Slerp(-vector2, -vector, (float)(i - 1) / 25f) + start, color, duration, depthTest);
			Debug.DrawLine(Vector3.Slerp(vector3, vector, (float)i / 25f) + end, Vector3.Slerp(vector3, vector, (float)(i - 1) / 25f) + end, color, duration, depthTest);
			Debug.DrawLine(Vector3.Slerp(-vector3, vector, (float)i / 25f) + end, Vector3.Slerp(-vector3, vector, (float)(i - 1) / 25f) + end, color, duration, depthTest);
			Debug.DrawLine(Vector3.Slerp(vector2, vector, (float)i / 25f) + end, Vector3.Slerp(vector2, vector, (float)(i - 1) / 25f) + end, color, duration, depthTest);
			Debug.DrawLine(Vector3.Slerp(-vector2, vector, (float)i / 25f) + end, Vector3.Slerp(-vector2, vector, (float)(i - 1) / 25f) + end, color, duration, depthTest);
		}
	}

	// Token: 0x0600034C RID: 844 RVA: 0x00032903 File Offset: 0x00030B03
	public static void DebugCapsule(Vector3 start, Vector3 end, float radius = 1f, float duration = 0f, bool depthTest = true)
	{
		DebugExtension.DebugCapsule(start, end, Color.white, radius, duration, depthTest);
	}

	// Token: 0x0600034D RID: 845 RVA: 0x00077F98 File Offset: 0x00076198
	public static void DrawPoint(Vector3 position, Color color, float scale = 1f)
	{
		Color color2 = Gizmos.color;
		Gizmos.color = color;
		Gizmos.DrawRay(position + Vector3.up * (scale * 0.5f), -Vector3.up * scale);
		Gizmos.DrawRay(position + Vector3.right * (scale * 0.5f), -Vector3.right * scale);
		Gizmos.DrawRay(position + Vector3.forward * (scale * 0.5f), -Vector3.forward * scale);
		Gizmos.color = color2;
	}

	// Token: 0x0600034E RID: 846 RVA: 0x00032915 File Offset: 0x00030B15
	public static void DrawPoint(Vector3 position, float scale = 1f)
	{
		DebugExtension.DrawPoint(position, Color.white, scale);
	}

	// Token: 0x0600034F RID: 847 RVA: 0x0007803C File Offset: 0x0007623C
	public static void DrawBounds(Bounds bounds, Color color)
	{
		Vector3 center = bounds.center;
		float x = bounds.extents.x;
		float y = bounds.extents.y;
		float z = bounds.extents.z;
		Vector3 from = center + new Vector3(x, y, z);
		Vector3 vector = center + new Vector3(x, y, -z);
		Vector3 vector2 = center + new Vector3(-x, y, z);
		Vector3 vector3 = center + new Vector3(-x, y, -z);
		Vector3 vector4 = center + new Vector3(x, -y, z);
		Vector3 to = center + new Vector3(x, -y, -z);
		Vector3 vector5 = center + new Vector3(-x, -y, z);
		Vector3 vector6 = center + new Vector3(-x, -y, -z);
		Color color2 = Gizmos.color;
		Gizmos.color = color;
		Gizmos.DrawLine(from, vector2);
		Gizmos.DrawLine(from, vector);
		Gizmos.DrawLine(vector2, vector3);
		Gizmos.DrawLine(vector, vector3);
		Gizmos.DrawLine(from, vector4);
		Gizmos.DrawLine(vector, to);
		Gizmos.DrawLine(vector2, vector5);
		Gizmos.DrawLine(vector3, vector6);
		Gizmos.DrawLine(vector4, vector5);
		Gizmos.DrawLine(vector4, to);
		Gizmos.DrawLine(vector5, vector6);
		Gizmos.DrawLine(vector6, to);
		Gizmos.color = color2;
	}

	// Token: 0x06000350 RID: 848 RVA: 0x00032923 File Offset: 0x00030B23
	public static void DrawBounds(Bounds bounds)
	{
		DebugExtension.DrawBounds(bounds, Color.white);
	}

	// Token: 0x06000351 RID: 849 RVA: 0x0007817C File Offset: 0x0007637C
	public static void DrawLocalCube(Transform transform, Vector3 size, Color color, Vector3 center = default(Vector3))
	{
		Color color2 = Gizmos.color;
		Gizmos.color = color;
		Vector3 vector = transform.TransformPoint(center + -size * 0.5f);
		Vector3 vector2 = transform.TransformPoint(center + new Vector3(size.x, -size.y, -size.z) * 0.5f);
		Vector3 vector3 = transform.TransformPoint(center + new Vector3(size.x, -size.y, size.z) * 0.5f);
		Vector3 vector4 = transform.TransformPoint(center + new Vector3(-size.x, -size.y, size.z) * 0.5f);
		Vector3 vector5 = transform.TransformPoint(center + new Vector3(-size.x, size.y, -size.z) * 0.5f);
		Vector3 vector6 = transform.TransformPoint(center + new Vector3(size.x, size.y, -size.z) * 0.5f);
		Vector3 vector7 = transform.TransformPoint(center + size * 0.5f);
		Vector3 vector8 = transform.TransformPoint(center + new Vector3(-size.x, size.y, size.z) * 0.5f);
		Gizmos.DrawLine(vector, vector2);
		Gizmos.DrawLine(vector2, vector3);
		Gizmos.DrawLine(vector3, vector4);
		Gizmos.DrawLine(vector4, vector);
		Gizmos.DrawLine(vector5, vector6);
		Gizmos.DrawLine(vector6, vector7);
		Gizmos.DrawLine(vector7, vector8);
		Gizmos.DrawLine(vector8, vector5);
		Gizmos.DrawLine(vector, vector5);
		Gizmos.DrawLine(vector2, vector6);
		Gizmos.DrawLine(vector3, vector7);
		Gizmos.DrawLine(vector4, vector8);
		Gizmos.color = color2;
	}

	// Token: 0x06000352 RID: 850 RVA: 0x00032930 File Offset: 0x00030B30
	public static void DrawLocalCube(Transform transform, Vector3 size, Vector3 center = default(Vector3))
	{
		DebugExtension.DrawLocalCube(transform, size, Color.white, center);
	}

	// Token: 0x06000353 RID: 851 RVA: 0x00078350 File Offset: 0x00076550
	public static void DrawLocalCube(Matrix4x4 space, Vector3 size, Color color, Vector3 center = default(Vector3))
	{
		Color color2 = Gizmos.color;
		Gizmos.color = color;
		Vector3 vector = space.MultiplyPoint3x4(center + -size * 0.5f);
		Vector3 vector2 = space.MultiplyPoint3x4(center + new Vector3(size.x, -size.y, -size.z) * 0.5f);
		Vector3 vector3 = space.MultiplyPoint3x4(center + new Vector3(size.x, -size.y, size.z) * 0.5f);
		Vector3 vector4 = space.MultiplyPoint3x4(center + new Vector3(-size.x, -size.y, size.z) * 0.5f);
		Vector3 vector5 = space.MultiplyPoint3x4(center + new Vector3(-size.x, size.y, -size.z) * 0.5f);
		Vector3 vector6 = space.MultiplyPoint3x4(center + new Vector3(size.x, size.y, -size.z) * 0.5f);
		Vector3 vector7 = space.MultiplyPoint3x4(center + size * 0.5f);
		Vector3 vector8 = space.MultiplyPoint3x4(center + new Vector3(-size.x, size.y, size.z) * 0.5f);
		Gizmos.DrawLine(vector, vector2);
		Gizmos.DrawLine(vector2, vector3);
		Gizmos.DrawLine(vector3, vector4);
		Gizmos.DrawLine(vector4, vector);
		Gizmos.DrawLine(vector5, vector6);
		Gizmos.DrawLine(vector6, vector7);
		Gizmos.DrawLine(vector7, vector8);
		Gizmos.DrawLine(vector8, vector5);
		Gizmos.DrawLine(vector, vector5);
		Gizmos.DrawLine(vector2, vector6);
		Gizmos.DrawLine(vector3, vector7);
		Gizmos.DrawLine(vector4, vector8);
		Gizmos.color = color2;
	}

	// Token: 0x06000354 RID: 852 RVA: 0x0003293F File Offset: 0x00030B3F
	public static void DrawLocalCube(Matrix4x4 space, Vector3 size, Vector3 center = default(Vector3))
	{
		DebugExtension.DrawLocalCube(space, size, Color.white, center);
	}

	// Token: 0x06000355 RID: 853 RVA: 0x0007852C File Offset: 0x0007672C
	public static void DrawCircle(Vector3 position, Vector3 up, Color color, float radius = 1f)
	{
		up = ((up == Vector3.zero) ? Vector3.up : up).normalized * radius;
		Vector3 vector = Vector3.Slerp(up, -up, 0.5f);
		Vector3 vector2 = Vector3.Cross(up, vector).normalized * radius;
		Matrix4x4 matrix4x = default(Matrix4x4);
		matrix4x[0] = vector2.x;
		matrix4x[1] = vector2.y;
		matrix4x[2] = vector2.z;
		matrix4x[4] = up.x;
		matrix4x[5] = up.y;
		matrix4x[6] = up.z;
		matrix4x[8] = vector.x;
		matrix4x[9] = vector.y;
		matrix4x[10] = vector.z;
		Vector3 from = position + matrix4x.MultiplyPoint3x4(new Vector3(Mathf.Cos(0f), 0f, Mathf.Sin(0f)));
		Vector3 vector3 = Vector3.zero;
		Color color2 = Gizmos.color;
		Gizmos.color = ((color == default(Color)) ? Color.white : color);
		for (int i = 0; i < 91; i++)
		{
			vector3.x = Mathf.Cos((float)(i * 4) * 0.017453292f);
			vector3.z = Mathf.Sin((float)(i * 4) * 0.017453292f);
			vector3.y = 0f;
			vector3 = position + matrix4x.MultiplyPoint3x4(vector3);
			Gizmos.DrawLine(from, vector3);
			from = vector3;
		}
		Gizmos.color = color2;
	}

	// Token: 0x06000356 RID: 854 RVA: 0x0003294E File Offset: 0x00030B4E
	public static void DrawCircle(Vector3 position, Color color, float radius = 1f)
	{
		DebugExtension.DrawCircle(position, Vector3.up, color, radius);
	}

	// Token: 0x06000357 RID: 855 RVA: 0x0003295D File Offset: 0x00030B5D
	public static void DrawCircle(Vector3 position, Vector3 up, float radius = 1f)
	{
		DebugExtension.DrawCircle(position, position, Color.white, radius);
	}

	// Token: 0x06000358 RID: 856 RVA: 0x0003296C File Offset: 0x00030B6C
	public static void DrawCircle(Vector3 position, float radius = 1f)
	{
		DebugExtension.DrawCircle(position, Vector3.up, Color.white, radius);
	}

	// Token: 0x06000359 RID: 857 RVA: 0x000786D8 File Offset: 0x000768D8
	public static void DrawCylinder(Vector3 start, Vector3 end, Color color, float radius = 1f)
	{
		Vector3 vector = (end - start).normalized * radius;
		Vector3 vector2 = Vector3.Slerp(vector, -vector, 0.5f);
		Vector3 b = Vector3.Cross(vector, vector2).normalized * radius;
		DebugExtension.DrawCircle(start, vector, color, radius);
		DebugExtension.DrawCircle(end, -vector, color, radius);
		DebugExtension.DrawCircle((start + end) * 0.5f, vector, color, radius);
		Color color2 = Gizmos.color;
		Gizmos.color = color;
		Gizmos.DrawLine(start + b, end + b);
		Gizmos.DrawLine(start - b, end - b);
		Gizmos.DrawLine(start + vector2, end + vector2);
		Gizmos.DrawLine(start - vector2, end - vector2);
		Gizmos.DrawLine(start - b, start + b);
		Gizmos.DrawLine(start - vector2, start + vector2);
		Gizmos.DrawLine(end - b, end + b);
		Gizmos.DrawLine(end - vector2, end + vector2);
		Gizmos.color = color2;
	}

	// Token: 0x0600035A RID: 858 RVA: 0x0003297F File Offset: 0x00030B7F
	public static void DrawCylinder(Vector3 start, Vector3 end, float radius = 1f)
	{
		DebugExtension.DrawCylinder(start, end, Color.white, radius);
	}

	// Token: 0x0600035B RID: 859 RVA: 0x000787FC File Offset: 0x000769FC
	public static void DrawCone(Vector3 position, Vector3 direction, Color color, float angle = 45f)
	{
		float magnitude = direction.magnitude;
		Vector3 vector = direction;
		Vector3 vector2 = Vector3.Slerp(vector, -vector, 0.5f);
		Vector3 vector3 = Vector3.Cross(vector, vector2).normalized * magnitude;
		direction = direction.normalized;
		Vector3 direction2 = Vector3.Slerp(vector, vector2, angle / 90f);
		Plane plane = new Plane(-direction, position + vector);
		Ray ray = new Ray(position, direction2);
		float num;
		plane.Raycast(ray, out num);
		Color color2 = Gizmos.color;
		Gizmos.color = color;
		Gizmos.DrawRay(position, direction2.normalized * num);
		Gizmos.DrawRay(position, Vector3.Slerp(vector, -vector2, angle / 90f).normalized * num);
		Gizmos.DrawRay(position, Vector3.Slerp(vector, vector3, angle / 90f).normalized * num);
		Gizmos.DrawRay(position, Vector3.Slerp(vector, -vector3, angle / 90f).normalized * num);
		DebugExtension.DrawCircle(position + vector, direction, color, (vector - direction2.normalized * num).magnitude);
		DebugExtension.DrawCircle(position + vector * 0.5f, direction, color, (vector * 0.5f - direction2.normalized * (num * 0.5f)).magnitude);
		Gizmos.color = color2;
	}

	// Token: 0x0600035C RID: 860 RVA: 0x0003298E File Offset: 0x00030B8E
	public static void DrawCone(Vector3 position, Vector3 direction, float angle = 45f)
	{
		DebugExtension.DrawCone(position, direction, Color.white, angle);
	}

	// Token: 0x0600035D RID: 861 RVA: 0x0003299D File Offset: 0x00030B9D
	public static void DrawCone(Vector3 position, Color color, float angle = 45f)
	{
		DebugExtension.DrawCone(position, Vector3.up, color, angle);
	}

	// Token: 0x0600035E RID: 862 RVA: 0x000329AC File Offset: 0x00030BAC
	public static void DrawCone(Vector3 position, float angle = 45f)
	{
		DebugExtension.DrawCone(position, Vector3.up, Color.white, angle);
	}

	// Token: 0x0600035F RID: 863 RVA: 0x000329BF File Offset: 0x00030BBF
	public static void DrawArrow(Vector3 position, Vector3 direction, Color color)
	{
		Color color2 = Gizmos.color;
		Gizmos.color = color;
		Gizmos.DrawRay(position, direction);
		DebugExtension.DrawCone(position + direction, -direction * 0.333f, color, 15f);
		Gizmos.color = color2;
	}

	// Token: 0x06000360 RID: 864 RVA: 0x000329FA File Offset: 0x00030BFA
	public static void DrawArrow(Vector3 position, Vector3 direction)
	{
		DebugExtension.DrawArrow(position, direction, Color.white);
	}

	// Token: 0x06000361 RID: 865 RVA: 0x0007898C File Offset: 0x00076B8C
	public static void DrawCapsule(Vector3 start, Vector3 end, Color color, float radius = 1f)
	{
		Vector3 vector = (end - start).normalized * radius;
		Vector3 vector2 = Vector3.Slerp(vector, -vector, 0.5f);
		Vector3 vector3 = Vector3.Cross(vector, vector2).normalized * radius;
		Color color2 = Gizmos.color;
		Gizmos.color = color;
		float magnitude = (start - end).magnitude;
		float d = Mathf.Max(0f, magnitude * 0.5f - radius);
		Vector3 vector4 = (end + start) * 0.5f;
		start = vector4 + (start - vector4).normalized * d;
		end = vector4 + (end - vector4).normalized * d;
		DebugExtension.DrawCircle(start, vector, color, radius);
		DebugExtension.DrawCircle(end, -vector, color, radius);
		Gizmos.DrawLine(start + vector3, end + vector3);
		Gizmos.DrawLine(start - vector3, end - vector3);
		Gizmos.DrawLine(start + vector2, end + vector2);
		Gizmos.DrawLine(start - vector2, end - vector2);
		for (int i = 1; i < 26; i++)
		{
			Gizmos.DrawLine(Vector3.Slerp(vector3, -vector, (float)i / 25f) + start, Vector3.Slerp(vector3, -vector, (float)(i - 1) / 25f) + start);
			Gizmos.DrawLine(Vector3.Slerp(-vector3, -vector, (float)i / 25f) + start, Vector3.Slerp(-vector3, -vector, (float)(i - 1) / 25f) + start);
			Gizmos.DrawLine(Vector3.Slerp(vector2, -vector, (float)i / 25f) + start, Vector3.Slerp(vector2, -vector, (float)(i - 1) / 25f) + start);
			Gizmos.DrawLine(Vector3.Slerp(-vector2, -vector, (float)i / 25f) + start, Vector3.Slerp(-vector2, -vector, (float)(i - 1) / 25f) + start);
			Gizmos.DrawLine(Vector3.Slerp(vector3, vector, (float)i / 25f) + end, Vector3.Slerp(vector3, vector, (float)(i - 1) / 25f) + end);
			Gizmos.DrawLine(Vector3.Slerp(-vector3, vector, (float)i / 25f) + end, Vector3.Slerp(-vector3, vector, (float)(i - 1) / 25f) + end);
			Gizmos.DrawLine(Vector3.Slerp(vector2, vector, (float)i / 25f) + end, Vector3.Slerp(vector2, vector, (float)(i - 1) / 25f) + end);
			Gizmos.DrawLine(Vector3.Slerp(-vector2, vector, (float)i / 25f) + end, Vector3.Slerp(-vector2, vector, (float)(i - 1) / 25f) + end);
		}
		Gizmos.color = color2;
	}

	// Token: 0x06000362 RID: 866 RVA: 0x00032A08 File Offset: 0x00030C08
	public static void DrawCapsule(Vector3 start, Vector3 end, float radius = 1f)
	{
		DebugExtension.DrawCapsule(start, end, Color.white, radius);
	}

	// Token: 0x06000363 RID: 867 RVA: 0x00078CCC File Offset: 0x00076ECC
	public static string MethodsOfObject(object obj, bool includeInfo = false)
	{
		string text = "";
		MethodInfo[] methods = obj.GetType().GetMethods();
		for (int i = 0; i < methods.Length; i++)
		{
			if (includeInfo)
			{
				string str = text;
				MethodInfo methodInfo = methods[i];
				text = str + ((methodInfo != null) ? methodInfo.ToString() : null) + "\n";
			}
			else
			{
				text = text + methods[i].Name + "\n";
			}
		}
		return text;
	}

	// Token: 0x06000364 RID: 868 RVA: 0x00078D30 File Offset: 0x00076F30
	public static string MethodsOfType(Type type, bool includeInfo = false)
	{
		string text = "";
		MethodInfo[] methods = type.GetMethods();
		for (int i = 0; i < methods.Length; i++)
		{
			if (includeInfo)
			{
				string str = text;
				MethodInfo methodInfo = methods[i];
				text = str + ((methodInfo != null) ? methodInfo.ToString() : null) + "\n";
			}
			else
			{
				text = text + methods[i].Name + "\n";
			}
		}
		return text;
	}
}
