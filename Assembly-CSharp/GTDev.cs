using System;
using System.Diagnostics;
using Cysharp.Text;
using Drawing;
using UnityEngine;

// Token: 0x020001BE RID: 446
public static class GTDev
{
	// Token: 0x06000A84 RID: 2692 RVA: 0x00037673 File Offset: 0x00035873
	[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSplashScreen)]
	private static void InitializeOnLoad()
	{
		GTDev.FetchDevID();
	}

	// Token: 0x06000A85 RID: 2693 RVA: 0x00030607 File Offset: 0x0002E807
	[HideInCallstack]
	public static void Log<T>(T msg, string channel = null)
	{
	}

	// Token: 0x06000A86 RID: 2694 RVA: 0x00030607 File Offset: 0x0002E807
	[HideInCallstack]
	public static void Log<T>(T msg, UnityEngine.Object context, string channel = null)
	{
	}

	// Token: 0x06000A87 RID: 2695 RVA: 0x00030607 File Offset: 0x0002E807
	[HideInCallstack]
	public static void LogError<T>(T msg, string channel = null)
	{
	}

	// Token: 0x06000A88 RID: 2696 RVA: 0x00030607 File Offset: 0x0002E807
	[HideInCallstack]
	public static void LogError<T>(T msg, UnityEngine.Object context, string channel = null)
	{
	}

	// Token: 0x06000A89 RID: 2697 RVA: 0x00030607 File Offset: 0x0002E807
	[HideInCallstack]
	public static void LogWarning<T>(T msg, string channel = null)
	{
	}

	// Token: 0x06000A8A RID: 2698 RVA: 0x00030607 File Offset: 0x0002E807
	[HideInCallstack]
	public static void LogWarning<T>(T msg, UnityEngine.Object context, string channel = null)
	{
	}

	// Token: 0x06000A8B RID: 2699 RVA: 0x00030607 File Offset: 0x0002E807
	[HideInCallstack]
	public static void LogSilent<T>(T msg, string channel = null)
	{
	}

	// Token: 0x06000A8C RID: 2700 RVA: 0x00030607 File Offset: 0x0002E807
	[HideInCallstack]
	public static void LogSilent<T>(T msg, UnityEngine.Object context, string channel = null)
	{
	}

	// Token: 0x06000A8D RID: 2701 RVA: 0x00030607 File Offset: 0x0002E807
	[HideInCallstack]
	[Conditional("UNITY_EDITOR")]
	public static void LogEditorOnly<T>(T msg, string channel = null)
	{
	}

	// Token: 0x06000A8E RID: 2702 RVA: 0x00030607 File Offset: 0x0002E807
	[HideInCallstack]
	[Conditional("UNITY_EDITOR")]
	public static void LogEditorOnly<T>(T msg, UnityEngine.Object context, string channel = null)
	{
	}

	// Token: 0x06000A8F RID: 2703 RVA: 0x00030607 File Offset: 0x0002E807
	[HideInCallstack]
	[Conditional("UNITY_EDITOR")]
	public static void CallEditorOnly(Action call)
	{
	}

	// Token: 0x1700010F RID: 271
	// (get) Token: 0x06000A90 RID: 2704 RVA: 0x0003767B File Offset: 0x0003587B
	public static int DevID
	{
		get
		{
			return GTDev.FetchDevID();
		}
	}

	// Token: 0x06000A91 RID: 2705 RVA: 0x000975E4 File Offset: 0x000957E4
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

	// Token: 0x06000A92 RID: 2706 RVA: 0x00030607 File Offset: 0x0002E807
	[HideInCallstack]
	[Conditional("_GTDEV_ON_")]
	private static void _Log<T>(Action<object, UnityEngine.Object> log, Action<object> logNoCtx, T msg, UnityEngine.Object ctx, string channel)
	{
	}

	// Token: 0x06000A93 RID: 2707 RVA: 0x00037682 File Offset: 0x00035882
	private static Mesh SphereMesh()
	{
		if (!GTDev.gSphereMesh)
		{
			GTDev.gSphereMesh = Resources.GetBuiltinResource<Mesh>("New-Sphere.fbx");
		}
		return GTDev.gSphereMesh;
	}

	// Token: 0x06000A94 RID: 2708 RVA: 0x00097644 File Offset: 0x00095844
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

	// Token: 0x06000A95 RID: 2709 RVA: 0x000977E8 File Offset: 0x000959E8
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

	// Token: 0x06000A96 RID: 2710 RVA: 0x000978DC File Offset: 0x00095ADC
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

	// Token: 0x04000CD1 RID: 3281
	[OnEnterPlay_Set(0)]
	private static int gDevID;

	// Token: 0x04000CD2 RID: 3282
	[OnEnterPlay_Set(false)]
	private static bool gHasDevID;

	// Token: 0x04000CD3 RID: 3283
	private static readonly Color gDefaultColor = new Color(0f, 1f, 1f, 0.32f);

	// Token: 0x04000CD4 RID: 3284
	private const string kFormatF = "{{ X: {0:##0.0000}, Y: {1:##0.0000}, Z: {2:##0.0000} }}";

	// Token: 0x04000CD5 RID: 3285
	private const float kDuration = 8f;

	// Token: 0x04000CD6 RID: 3286
	private static Mesh gSphereMesh;
}
