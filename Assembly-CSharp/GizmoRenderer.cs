using System;
using Drawing;
using Unity.Mathematics;
using UnityEngine;

// Token: 0x020006C4 RID: 1732
public class GizmoRenderer : MonoBehaviour
{
	// Token: 0x06002B0A RID: 11018 RVA: 0x0004CFF3 File Offset: 0x0004B1F3
	private void Update()
	{
		this.RenderGizmos();
	}

	// Token: 0x06002B0B RID: 11019 RVA: 0x0011FB60 File Offset: 0x0011DD60
	private unsafe void RenderGizmos()
	{
		if (this.renderMode == GizmoRenderer.RenderMode.Never)
		{
			return;
		}
		if (this.gizmos == null)
		{
			return;
		}
		int num = this.gizmos.Length;
		if (num == 0)
		{
			return;
		}
		CommandBuilder arg = *Draw.ingame;
		Transform transform = base.transform;
		for (int i = 0; i < num; i++)
		{
			GizmoRenderer.GizmoInfo gizmoInfo = this.gizmos[i];
			if (gizmoInfo.render)
			{
				Transform transform2 = gizmoInfo.target ? gizmoInfo.target : transform;
				using (arg.InLocalSpace(transform2))
				{
					using (arg.WithLineWidth(gizmoInfo.lineWidth, false))
					{
						GizmoRenderer.gRenderFuncs[(int)gizmoInfo.type](arg, gizmoInfo);
					}
				}
			}
		}
	}

	// Token: 0x06002B0C RID: 11020 RVA: 0x0004CFFB File Offset: 0x0004B1FB
	private static void RenderPlaneWire(CommandBuilder draw, GizmoRenderer.GizmoInfo gizmo)
	{
		draw.WirePlane(gizmo.center, gizmo.rotation, gizmo.size.xz, gizmo.color);
	}

	// Token: 0x06002B0D RID: 11021 RVA: 0x0004D021 File Offset: 0x0004B221
	private static void RenderPlaneSolid(CommandBuilder draw, GizmoRenderer.GizmoInfo gizmo)
	{
		draw.SolidPlane(gizmo.center, gizmo.rotation, gizmo.size.xz, gizmo.color);
	}

	// Token: 0x06002B0E RID: 11022 RVA: 0x0004D047 File Offset: 0x0004B247
	private static void RenderGridWire(CommandBuilder draw, GizmoRenderer.GizmoInfo gizmo)
	{
		draw.WireGrid(gizmo.center, gizmo.rotation, gizmo.gridCells, gizmo.size.xz, gizmo.color);
	}

	// Token: 0x06002B0F RID: 11023 RVA: 0x0004D073 File Offset: 0x0004B273
	private static void RenderBoxWire(CommandBuilder draw, GizmoRenderer.GizmoInfo gizmo)
	{
		draw.WireBox(gizmo.center, gizmo.rotation, gizmo.size, gizmo.color);
	}

	// Token: 0x06002B10 RID: 11024 RVA: 0x0004D094 File Offset: 0x0004B294
	private static void RenderBoxSolid(CommandBuilder draw, GizmoRenderer.GizmoInfo gizmo)
	{
		draw.SolidBox(gizmo.center, gizmo.rotation, gizmo.size, gizmo.color);
	}

	// Token: 0x06002B11 RID: 11025 RVA: 0x0004D0B5 File Offset: 0x0004B2B5
	private static void RenderSphereWire(CommandBuilder draw, GizmoRenderer.GizmoInfo gizmo)
	{
		draw.WireSphere(gizmo.center, gizmo.radius * 0.5f, gizmo.color);
	}

	// Token: 0x06002B12 RID: 11026 RVA: 0x0011FC4C File Offset: 0x0011DE4C
	private static void RenderSphereSolid(CommandBuilder draw, GizmoRenderer.GizmoInfo gizmo)
	{
		Matrix4x4 matrix = Matrix4x4.TRS(gizmo.center, quaternion.identity, new float3(gizmo.radius));
		using (draw.WithMatrix(matrix))
		{
			draw.SolidMesh(GizmoRenderer.gSphereMesh, gizmo.color);
		}
	}

	// Token: 0x06002B13 RID: 11027 RVA: 0x0004D0D6 File Offset: 0x0004B2D6
	private static void RenderLabel3D(CommandBuilder draw, GizmoRenderer.GizmoInfo gizmo)
	{
		draw.Label3D(gizmo.center, gizmo.rotation, gizmo.text, gizmo.textSize * 0.1f, GizmoRenderer.gLabelAligns[(int)gizmo.textAlign], gizmo.color);
	}

	// Token: 0x06002B14 RID: 11028 RVA: 0x0004D113 File Offset: 0x0004B313
	private static void RenderLabel2D(CommandBuilder draw, GizmoRenderer.GizmoInfo gizmo)
	{
		draw.Label2D(gizmo.center, gizmo.text, gizmo.textSize * gizmo.textPPU, GizmoRenderer.gLabelAligns[(int)gizmo.textAlign], gizmo.color);
	}

	// Token: 0x06002B15 RID: 11029 RVA: 0x0004D14D File Offset: 0x0004B34D
	[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
	private static void InitializeOnLoad()
	{
		GizmoRenderer.gSphereMesh = Resources.GetBuiltinResource<Mesh>("New-Sphere.fbx");
	}

	// Token: 0x06002B16 RID: 11030 RVA: 0x0011FCC0 File Offset: 0x0011DEC0
	private static Color GetRandomColor()
	{
		Color result = Color.HSVToRGB((float)(DateTime.UtcNow.Ticks % 65536L) / 65535f, 1f, 1f, true);
		result.a = 1f;
		return result;
	}

	// Token: 0x0400307A RID: 12410
	public GizmoRenderer.RenderMode renderMode = GizmoRenderer.RenderMode.Always;

	// Token: 0x0400307B RID: 12411
	public bool includeInBuild;

	// Token: 0x0400307C RID: 12412
	public GizmoRenderer.GizmoInfo[] gizmos = new GizmoRenderer.GizmoInfo[0];

	// Token: 0x0400307D RID: 12413
	private static readonly Action<CommandBuilder, GizmoRenderer.GizmoInfo>[] gRenderFuncs = new Action<CommandBuilder, GizmoRenderer.GizmoInfo>[]
	{
		new Action<CommandBuilder, GizmoRenderer.GizmoInfo>(GizmoRenderer.RenderBoxWire),
		new Action<CommandBuilder, GizmoRenderer.GizmoInfo>(GizmoRenderer.RenderBoxSolid),
		new Action<CommandBuilder, GizmoRenderer.GizmoInfo>(GizmoRenderer.RenderSphereWire),
		new Action<CommandBuilder, GizmoRenderer.GizmoInfo>(GizmoRenderer.RenderSphereSolid),
		new Action<CommandBuilder, GizmoRenderer.GizmoInfo>(GizmoRenderer.RenderLabel3D),
		new Action<CommandBuilder, GizmoRenderer.GizmoInfo>(GizmoRenderer.RenderLabel2D),
		new Action<CommandBuilder, GizmoRenderer.GizmoInfo>(GizmoRenderer.RenderGridWire),
		new Action<CommandBuilder, GizmoRenderer.GizmoInfo>(GizmoRenderer.RenderPlaneSolid),
		new Action<CommandBuilder, GizmoRenderer.GizmoInfo>(GizmoRenderer.RenderPlaneWire)
	};

	// Token: 0x0400307E RID: 12414
	private static readonly LabelAlignment[] gLabelAligns = new LabelAlignment[]
	{
		LabelAlignment.Center,
		LabelAlignment.MiddleRight,
		LabelAlignment.MiddleLeft,
		LabelAlignment.BottomCenter,
		LabelAlignment.BottomRight,
		LabelAlignment.BottomLeft,
		LabelAlignment.TopRight,
		LabelAlignment.TopLeft,
		LabelAlignment.TopCenter
	};

	// Token: 0x0400307F RID: 12415
	private static Mesh gSphereMesh;

	// Token: 0x020006C5 RID: 1733
	[Serializable]
	public class GizmoInfo
	{
		// Token: 0x04003080 RID: 12416
		public bool render = true;

		// Token: 0x04003081 RID: 12417
		public GizmoRenderer.GizmoType type;

		// Token: 0x04003082 RID: 12418
		public Color color = GizmoRenderer.GetRandomColor();

		// Token: 0x04003083 RID: 12419
		public uint lineWidth = 1U;

		// Token: 0x04003084 RID: 12420
		[Space]
		public Transform target;

		// Token: 0x04003085 RID: 12421
		[Space]
		public float3 center = float3.zero;

		// Token: 0x04003086 RID: 12422
		public float3 size = Vector3.one;

		// Token: 0x04003087 RID: 12423
		public float radius = 1f;

		// Token: 0x04003088 RID: 12424
		public quaternion rotation = quaternion.identity;

		// Token: 0x04003089 RID: 12425
		[Space]
		public string text = string.Empty;

		// Token: 0x0400308A RID: 12426
		public float textSize = 4f;

		// Token: 0x0400308B RID: 12427
		public GizmoRenderer.TextAlign textAlign;

		// Token: 0x0400308C RID: 12428
		public uint textPPU = 24U;

		// Token: 0x0400308D RID: 12429
		[Space]
		public int2 gridCells = new int2(4);
	}

	// Token: 0x020006C6 RID: 1734
	[Flags]
	public enum RenderMode : uint
	{
		// Token: 0x0400308F RID: 12431
		Never = 0U,
		// Token: 0x04003090 RID: 12432
		InEditor = 1U,
		// Token: 0x04003091 RID: 12433
		InBuild = 2U,
		// Token: 0x04003092 RID: 12434
		Always = 3U
	}

	// Token: 0x020006C7 RID: 1735
	public enum GizmoType : uint
	{
		// Token: 0x04003094 RID: 12436
		BoxWire,
		// Token: 0x04003095 RID: 12437
		BoxSolid,
		// Token: 0x04003096 RID: 12438
		SphereWire,
		// Token: 0x04003097 RID: 12439
		SphereSolid,
		// Token: 0x04003098 RID: 12440
		Label3D,
		// Token: 0x04003099 RID: 12441
		Label2D,
		// Token: 0x0400309A RID: 12442
		GridWire,
		// Token: 0x0400309B RID: 12443
		PlaneSolid,
		// Token: 0x0400309C RID: 12444
		PlaneWire
	}

	// Token: 0x020006C8 RID: 1736
	public enum TextAlign : uint
	{
		// Token: 0x0400309E RID: 12446
		Center,
		// Token: 0x0400309F RID: 12447
		MiddleRight,
		// Token: 0x040030A0 RID: 12448
		MiddleLeft,
		// Token: 0x040030A1 RID: 12449
		BottomCenter,
		// Token: 0x040030A2 RID: 12450
		BottomRight,
		// Token: 0x040030A3 RID: 12451
		BottomLeft,
		// Token: 0x040030A4 RID: 12452
		TopRight,
		// Token: 0x040030A5 RID: 12453
		TopLeft,
		// Token: 0x040030A6 RID: 12454
		TopCenter
	}
}
