using System;
using System.Diagnostics;
using Cysharp.Text;
using Drawing;
using UnityEngine;

// Token: 0x020001B3 RID: 435
public static class GTDev
{
	// Token: 0x06000A38 RID: 2616 RVA: 0x00037A88 File Offset: 0x00035C88
	[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSplashScreen)]
	private static void InitializeOnLoad()
	{
		GTDev.FetchDevID();
	}

	// Token: 0x06000A39 RID: 2617 RVA: 0x000023F4 File Offset: 0x000005F4
	[HideInCallstack]
	public static void Log<T>(T msg, string channel = null)
	{
	}

	// Token: 0x06000A3A RID: 2618 RVA: 0x000023F4 File Offset: 0x000005F4
	[HideInCallstack]
	public static void Log<T>(T msg, Object context, string channel = null)
	{
	}

	// Token: 0x06000A3B RID: 2619 RVA: 0x000023F4 File Offset: 0x000005F4
	[HideInCallstack]
	public static void LogError<T>(T msg, string channel = null)
	{
	}

	// Token: 0x06000A3C RID: 2620 RVA: 0x000023F4 File Offset: 0x000005F4
	[HideInCallstack]
	public static void LogError<T>(T msg, Object context, string channel = null)
	{
	}

	// Token: 0x06000A3D RID: 2621 RVA: 0x000023F4 File Offset: 0x000005F4
	[HideInCallstack]
	public static void LogWarning<T>(T msg, string channel = null)
	{
	}

	// Token: 0x06000A3E RID: 2622 RVA: 0x000023F4 File Offset: 0x000005F4
	[HideInCallstack]
	public static void LogWarning<T>(T msg, Object context, string channel = null)
	{
	}

	// Token: 0x06000A3F RID: 2623 RVA: 0x000023F4 File Offset: 0x000005F4
	[HideInCallstack]
	public static void LogSilent<T>(T msg, string channel = null)
	{
	}

	// Token: 0x06000A40 RID: 2624 RVA: 0x000023F4 File Offset: 0x000005F4
	[HideInCallstack]
	public static void LogSilent<T>(T msg, Object context, string channel = null)
	{
	}

	// Token: 0x06000A41 RID: 2625 RVA: 0x000023F4 File Offset: 0x000005F4
	[HideInCallstack]
	[Conditional("UNITY_EDITOR")]
	public static void LogEditorOnly<T>(T msg, string channel = null)
	{
	}

	// Token: 0x06000A42 RID: 2626 RVA: 0x000023F4 File Offset: 0x000005F4
	[HideInCallstack]
	[Conditional("UNITY_EDITOR")]
	public static void LogEditorOnly<T>(T msg, Object context, string channel = null)
	{
	}

	// Token: 0x06000A43 RID: 2627 RVA: 0x000023F4 File Offset: 0x000005F4
	[HideInCallstack]
	[Conditional("UNITY_EDITOR")]
	public static void CallEditorOnly(Action call)
	{
	}

	// Token: 0x17000108 RID: 264
	// (get) Token: 0x06000A44 RID: 2628 RVA: 0x00037A90 File Offset: 0x00035C90
	public static int DevID
	{
		get
		{
			return GTDev.FetchDevID();
		}
	}

	// Token: 0x06000A45 RID: 2629 RVA: 0x00037A98 File Offset: 0x00035C98
	private static int FetchDevID()
	{
		if (GTDev.gHasDevID)
		{
			return GTDev.gDevID;
		}
		int i = StaticHash.Compute(SystemInfo.deviceUniqueIdentifier);
		int i2 = StaticHash.Compute(Environment.UserDomainName);
		int i3 = StaticHash.Compute(Environment.UserName);
		int i4 = StaticHash.Compute(Application.unityVersion);
		GTDev.gDevID = StaticHash.Compute(i, i2, i3, i4);
		GTDev.gHasDevID = true;
		return GTDev.gDevID;
	}

	// Token: 0x06000A46 RID: 2630 RVA: 0x000023F4 File Offset: 0x000005F4
	[HideInCallstack]
	[Conditional("_GTDEV_ON_")]
	private static void _Log<T>(Action<object, Object> log, Action<object> logNoCtx, T msg, Object ctx, string channel)
	{
	}

	// Token: 0x06000A47 RID: 2631 RVA: 0x00037AF5 File Offset: 0x00035CF5
	private static Mesh SphereMesh()
	{
		if (!GTDev.gSphereMesh)
		{
			GTDev.gSphereMesh = Resources.GetBuiltinResource<Mesh>("New-Sphere.fbx");
		}
		return GTDev.gSphereMesh;
	}

	// Token: 0x06000A48 RID: 2632 RVA: 0x00037B18 File Offset: 0x00035D18
	[Conditional("_GTDEV_ON_")]
	public unsafe static void Ping3D(this Collider col, Color color = default(Color), float duration = 8f)
	{
		if (color == default(Color))
		{
			color = GTDev.gDefaultColor;
		}
		if (color.a.Approx0(1E-06f))
		{
			return;
		}
		Matrix4x4 localToWorldMatrix = col.transform.localToWorldMatrix;
		SRand srand = new SRand(localToWorldMatrix.QuantizedId128().GetHashCode());
		color.r = srand.NextFloat();
		color.g = srand.NextFloat();
		color.b = srand.NextFloat();
		CommandBuilder commandBuilder = *Draw.ingame;
		using (commandBuilder.WithDuration(duration))
		{
			commandBuilder.PushMatrix(localToWorldMatrix);
			commandBuilder.PushLineWidth(2f, true);
			commandBuilder.PushColor(color);
			BoxCollider boxCollider = col as BoxCollider;
			if (boxCollider == null)
			{
				SphereCollider sphereCollider = col as SphereCollider;
				if (sphereCollider == null)
				{
					CapsuleCollider capsuleCollider = col as CapsuleCollider;
					if (capsuleCollider != null)
					{
						commandBuilder.WireCapsule(capsuleCollider.center, Vector3.up, capsuleCollider.height, capsuleCollider.radius, color);
					}
				}
				else
				{
					commandBuilder.WireSphere(sphereCollider.center, sphereCollider.radius, color);
				}
			}
			else
			{
				commandBuilder.WireBox(boxCollider.center, boxCollider.size);
			}
			commandBuilder.Label2D(Vector3.zero, col.name, 16f, LabelAlignment.Center);
			commandBuilder.PopColor();
			commandBuilder.PopLineWidth();
			commandBuilder.PopMatrix();
		}
	}

	// Token: 0x06000A49 RID: 2633 RVA: 0x00037CBC File Offset: 0x00035EBC
	[Conditional("_GTDEV_ON_")]
	public unsafe static void Ping3D(this Vector3 vec, Color color = default(Color), float duration = 8f)
	{
		if (color == default(Color))
		{
			color = GTDev.gDefaultColor;
		}
		else
		{
			color.a = GTDev.gDefaultColor.a;
		}
		string text = ZString.Format<float, float, float>("{{ X: {0:##0.0000}, Y: {1:##0.0000}, Z: {2:##0.0000} }}", vec.x, vec.y, vec.z);
		CommandBuilder commandBuilder = *Draw.ingame;
		using (commandBuilder.WithDuration(duration))
		{
			using (commandBuilder.WithLineWidth(2f, true))
			{
				commandBuilder.Cross(vec, 0.64f, color);
			}
			commandBuilder.Label2D(vec + Vector3.down * 0.64f, text, 16f, LabelAlignment.Center, color);
		}
	}

	// Token: 0x06000A4A RID: 2634 RVA: 0x00037DB0 File Offset: 0x00035FB0
	[Conditional("_GTDEV_ON_")]
	public unsafe static void Ping3D<T>(this T value, Vector3 position, Color color = default(Color), float duration = 8f)
	{
		if (color == default(Color))
		{
			color = GTDev.gDefaultColor;
		}
		string text = ZString.Concat<T>(value);
		CommandBuilder commandBuilder = *Draw.ingame;
		using (commandBuilder.WithDuration(duration))
		{
			commandBuilder.Label2D(position, text, 16f, LabelAlignment.Center, color);
		}
	}

	// Token: 0x04000C8B RID: 3211
	[OnEnterPlay_Set(0)]
	private static int gDevID;

	// Token: 0x04000C8C RID: 3212
	[OnEnterPlay_Set(false)]
	private static bool gHasDevID;

	// Token: 0x04000C8D RID: 3213
	private static readonly Color gDefaultColor = new Color(0f, 1f, 1f, 0.32f);

	// Token: 0x04000C8E RID: 3214
	private const string kFormatF = "{{ X: {0:##0.0000}, Y: {1:##0.0000}, Z: {2:##0.0000} }}";

	// Token: 0x04000C8F RID: 3215
	private const float kDuration = 8f;

	// Token: 0x04000C90 RID: 3216
	private static Mesh gSphereMesh;
}
