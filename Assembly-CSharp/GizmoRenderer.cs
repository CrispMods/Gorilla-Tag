using System;
using Drawing;
using Unity.Mathematics;
using UnityEngine;

// Token: 0x020006AF RID: 1711
public class GizmoRenderer : MonoBehaviour
{
	// Token: 0x06002A74 RID: 10868 RVA: 0x000D3734 File Offset: 0x000D1934
	private void Update()
	{
		this.RenderGizmos();
	}

	// Token: 0x06002A75 RID: 10869 RVA: 0x000D373C File Offset: 0x000D193C
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

	// Token: 0x06002A76 RID: 10870 RVA: 0x000D3828 File Offset: 0x000D1A28
	private static void RenderPlaneWire(CommandBuilder draw, GizmoRenderer.GizmoInfo gizmo)
	{
		draw.WirePlane(gizmo.center, gizmo.rotation, gizmo.size.xz, gizmo.color);
	}

	// Token: 0x06002A77 RID: 10871 RVA: 0x000D384E File Offset: 0x000D1A4E
	private static void RenderPlaneSolid(CommandBuilder draw, GizmoRenderer.GizmoInfo gizmo)
	{
		draw.SolidPlane(gizmo.center, gizmo.rotation, gizmo.size.xz, gizmo.color);
	}

	// Token: 0x06002A78 RID: 10872 RVA: 0x000D3874 File Offset: 0x000D1A74
	private static void RenderGridWire(CommandBuilder draw, GizmoRenderer.GizmoInfo gizmo)
	{
		draw.WireGrid(gizmo.center, gizmo.rotation, gizmo.gridCells, gizmo.size.xz, gizmo.color);
	}

	// Token: 0x06002A79 RID: 10873 RVA: 0x000D38A0 File Offset: 0x000D1AA0
	private static void RenderBoxWire(CommandBuilder draw, GizmoRenderer.GizmoInfo gizmo)
	{
		draw.WireBox(gizmo.center, gizmo.rotation, gizmo.size, gizmo.color);
	}

	// Token: 0x06002A7A RID: 10874 RVA: 0x000D38C1 File Offset: 0x000D1AC1
	private static void RenderBoxSolid(CommandBuilder draw, GizmoRenderer.GizmoInfo gizmo)
	{
		draw.SolidBox(gizmo.center, gizmo.rotation, gizmo.size, gizmo.color);
	}

	// Token: 0x06002A7B RID: 10875 RVA: 0x000D38E2 File Offset: 0x000D1AE2
	private static void RenderSphereWire(CommandBuilder draw, GizmoRenderer.GizmoInfo gizmo)
	{
		draw.WireSphere(gizmo.center, gizmo.radius * 0.5f, gizmo.color);
	}

	// Token: 0x06002A7C RID: 10876 RVA: 0x000D3904 File Offset: 0x000D1B04
	private static void RenderSphereSolid(CommandBuilder draw, GizmoRenderer.GizmoInfo gizmo)
	{
		Matrix4x4 matrix = Matrix4x4.TRS(gizmo.center, quaternion.identity, new float3(gizmo.radius));
		using (draw.WithMatrix(matrix))
		{
			draw.SolidMesh(GizmoRenderer.gSphereMesh, gizmo.color);
		}
	}

	// Token: 0x06002A7D RID: 10877 RVA: 0x000D3978 File Offset: 0x000D1B78
	private static void RenderLabel3D(CommandBuilder draw, GizmoRenderer.GizmoInfo gizmo)
	{
		draw.Label3D(gizmo.center, gizmo.rotation, gizmo.text, gizmo.textSize * 0.1f, GizmoRenderer.gLabelAligns[(int)gizmo.textAlign], gizmo.color);
	}

	// Token: 0x06002A7E RID: 10878 RVA: 0x000D39B5 File Offset: 0x000D1BB5
	private static void RenderLabel2D(CommandBuilder draw, GizmoRenderer.GizmoInfo gizmo)
	{
		draw.Label2D(gizmo.center, gizmo.text, gizmo.textSize * gizmo.textPPU, GizmoRenderer.gLabelAligns[(int)gizmo.textAlign], gizmo.color);
	}

	// Token: 0x06002A7F RID: 10879 RVA: 0x000D39EF File Offset: 0x000D1BEF
	[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
	private static void InitializeOnLoad()
	{
		GizmoRenderer.gSphereMesh = Resources.GetBuiltinResource<Mesh>("New-Sphere.fbx");
	}

	// Token: 0x06002A80 RID: 10880 RVA: 0x000D3A00 File Offset: 0x000D1C00
	private static Color GetRandomColor()
	{
		Color result = Color.HSVToRGB((float)(DateTime.UtcNow.Ticks % 65536L) / 65535f, 1f, 1f, true);
		result.a = 1f;
		return result;
	}

	// Token: 0x04002FDD RID: 12253
	public GizmoRenderer.RenderMode renderMode = GizmoRenderer.RenderMode.Always;

	// Token: 0x04002FDE RID: 12254
	public bool includeInBuild;

	// Token: 0x04002FDF RID: 12255
	public GizmoRenderer.GizmoInfo[] gizmos = new GizmoRenderer.GizmoInfo[0];

	// Token: 0x04002FE0 RID: 12256
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

	// Token: 0x04002FE1 RID: 12257
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

	// Token: 0x04002FE2 RID: 12258
	private static Mesh gSphereMesh;

	// Token: 0x020006B0 RID: 1712
	[Serializable]
	public class GizmoInfo
	{
		// Token: 0x04002FE3 RID: 12259
		public bool render = true;

		// Token: 0x04002FE4 RID: 12260
		public GizmoRenderer.GizmoType type;

		// Token: 0x04002FE5 RID: 12261
		public Color color = GizmoRenderer.GetRandomColor();

		// Token: 0x04002FE6 RID: 12262
		public uint lineWidth = 1U;

		// Token: 0x04002FE7 RID: 12263
		[Space]
		public Transform target;

		// Token: 0x04002FE8 RID: 12264
		[Space]
		public float3 center = float3.zero;

		// Token: 0x04002FE9 RID: 12265
		public float3 size = Vector3.one;

		// Token: 0x04002FEA RID: 12266
		public float radius = 1f;

		// Token: 0x04002FEB RID: 12267
		public quaternion rotation = quaternion.identity;

		// Token: 0x04002FEC RID: 12268
		[Space]
		public string text = string.Empty;

		// Token: 0x04002FED RID: 12269
		public float textSize = 4f;

		// Token: 0x04002FEE RID: 12270
		public GizmoRenderer.TextAlign textAlign;

		// Token: 0x04002FEF RID: 12271
		public uint textPPU = 24U;

		// Token: 0x04002FF0 RID: 12272
		[Space]
		public int2 gridCells = new int2(4);
	}

	// Token: 0x020006B1 RID: 1713
	[Flags]
	public enum RenderMode : uint
	{
		// Token: 0x04002FF2 RID: 12274
		Never = 0U,
		// Token: 0x04002FF3 RID: 12275
		InEditor = 1U,
		// Token: 0x04002FF4 RID: 12276
		InBuild = 2U,
		// Token: 0x04002FF5 RID: 12277
		Always = 3U
	}

	// Token: 0x020006B2 RID: 1714
	public enum GizmoType : uint
	{
		// Token: 0x04002FF7 RID: 12279
		BoxWire,
		// Token: 0x04002FF8 RID: 12280
		BoxSolid,
		// Token: 0x04002FF9 RID: 12281
		SphereWire,
		// Token: 0x04002FFA RID: 12282
		SphereSolid,
		// Token: 0x04002FFB RID: 12283
		Label3D,
		// Token: 0x04002FFC RID: 12284
		Label2D,
		// Token: 0x04002FFD RID: 12285
		GridWire,
		// Token: 0x04002FFE RID: 12286
		PlaneSolid,
		// Token: 0x04002FFF RID: 12287
		PlaneWire
	}

	// Token: 0x020006B3 RID: 1715
	public enum TextAlign : uint
	{
		// Token: 0x04003001 RID: 12289
		Center,
		// Token: 0x04003002 RID: 12290
		MiddleRight,
		// Token: 0x04003003 RID: 12291
		MiddleLeft,
		// Token: 0x04003004 RID: 12292
		BottomCenter,
		// Token: 0x04003005 RID: 12293
		BottomRight,
		// Token: 0x04003006 RID: 12294
		BottomLeft,
		// Token: 0x04003007 RID: 12295
		TopRight,
		// Token: 0x04003008 RID: 12296
		TopLeft,
		// Token: 0x04003009 RID: 12297
		TopCenter
	}
}
