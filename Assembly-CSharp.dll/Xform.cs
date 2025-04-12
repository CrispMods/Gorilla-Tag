using System;
using Drawing;
using Unity.Mathematics;
using UnityEngine;

// Token: 0x020006DC RID: 1756
[ExecuteAlways]
public class Xform : MonoBehaviour
{
	// Token: 0x1700049F RID: 1183
	// (get) Token: 0x06002BC3 RID: 11203 RVA: 0x0004CE64 File Offset: 0x0004B064
	public float3 localExtents
	{
		get
		{
			return this.localScale * 0.5f;
		}
	}

	// Token: 0x06002BC4 RID: 11204 RVA: 0x0004CE76 File Offset: 0x0004B076
	public Matrix4x4 LocalTRS()
	{
		return Matrix4x4.TRS(this.localPosition, this.localRotation, this.localScale);
	}

	// Token: 0x06002BC5 RID: 11205 RVA: 0x0004CE99 File Offset: 0x0004B099
	public Matrix4x4 TRS()
	{
		if (this.parent.AsNull<Transform>() == null)
		{
			return this.LocalTRS();
		}
		return this.parent.localToWorldMatrix * this.LocalTRS();
	}

	// Token: 0x06002BC6 RID: 11206 RVA: 0x0011D4F4 File Offset: 0x0011B6F4
	private unsafe void Update()
	{
		Matrix4x4 matrix = this.TRS();
		CommandBuilder commandBuilder = *Draw.ingame;
		using (commandBuilder.WithMatrix(matrix))
		{
			using (commandBuilder.WithLineWidth(2f, true))
			{
				commandBuilder.PlaneWithNormal(Xform.AXIS_XR_RT * 0.5f, Xform.AXIS_XR_RT, Xform.F2_ONE, Xform.CR);
				commandBuilder.PlaneWithNormal(Xform.AXIS_YG_UP * 0.5f, Xform.AXIS_YG_UP, Xform.F2_ONE, Xform.CG);
				commandBuilder.PlaneWithNormal(Xform.AXIS_ZB_FW * 0.5f, Xform.AXIS_ZB_FW, Xform.F2_ONE, Xform.CB);
				commandBuilder.WireBox(float3.zero, quaternion.identity, 1f, this.displayColor);
			}
		}
	}

	// Token: 0x040030E8 RID: 12520
	public Transform parent;

	// Token: 0x040030E9 RID: 12521
	[Space]
	public Color displayColor = SRand.New().NextColor();

	// Token: 0x040030EA RID: 12522
	[Space]
	public float3 localPosition = float3.zero;

	// Token: 0x040030EB RID: 12523
	public float3 localScale = Vector3.one;

	// Token: 0x040030EC RID: 12524
	public Quaternion localRotation = quaternion.identity;

	// Token: 0x040030ED RID: 12525
	private static readonly float3 F3_ONE = 1f;

	// Token: 0x040030EE RID: 12526
	private static readonly float2 F2_ONE = 1f;

	// Token: 0x040030EF RID: 12527
	private static readonly float3 AXIS_ZB_FW = new float3(0f, 0f, 1f);

	// Token: 0x040030F0 RID: 12528
	private static readonly float3 AXIS_YG_UP = new float3(0f, 1f, 0f);

	// Token: 0x040030F1 RID: 12529
	private static readonly float3 AXIS_XR_RT = new float3(1f, 0f, 0f);

	// Token: 0x040030F2 RID: 12530
	private static readonly Color CR = new Color(1f, 0f, 0f, 0.24f);

	// Token: 0x040030F3 RID: 12531
	private static readonly Color CG = new Color(0f, 1f, 0f, 0.24f);

	// Token: 0x040030F4 RID: 12532
	private static readonly Color CB = new Color(0f, 0f, 1f, 0.24f);
}
