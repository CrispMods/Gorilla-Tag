using System;
using Drawing;
using Unity.Mathematics;
using UnityEngine;

// Token: 0x020006F0 RID: 1776
[ExecuteAlways]
public class Xform : MonoBehaviour
{
	// Token: 0x170004AB RID: 1195
	// (get) Token: 0x06002C51 RID: 11345 RVA: 0x0004E1A9 File Offset: 0x0004C3A9
	public float3 localExtents
	{
		get
		{
			return this.localScale * 0.5f;
		}
	}

	// Token: 0x06002C52 RID: 11346 RVA: 0x0004E1BB File Offset: 0x0004C3BB
	public Matrix4x4 LocalTRS()
	{
		return Matrix4x4.TRS(this.localPosition, this.localRotation, this.localScale);
	}

	// Token: 0x06002C53 RID: 11347 RVA: 0x0004E1DE File Offset: 0x0004C3DE
	public Matrix4x4 TRS()
	{
		if (this.parent.AsNull<Transform>() == null)
		{
			return this.LocalTRS();
		}
		return this.parent.localToWorldMatrix * this.LocalTRS();
	}

	// Token: 0x06002C54 RID: 11348 RVA: 0x001220AC File Offset: 0x001202AC
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

	// Token: 0x0400317F RID: 12671
	public Transform parent;

	// Token: 0x04003180 RID: 12672
	[Space]
	public Color displayColor = SRand.New().NextColor();

	// Token: 0x04003181 RID: 12673
	[Space]
	public float3 localPosition = float3.zero;

	// Token: 0x04003182 RID: 12674
	public float3 localScale = Vector3.one;

	// Token: 0x04003183 RID: 12675
	public Quaternion localRotation = quaternion.identity;

	// Token: 0x04003184 RID: 12676
	private static readonly float3 F3_ONE = 1f;

	// Token: 0x04003185 RID: 12677
	private static readonly float2 F2_ONE = 1f;

	// Token: 0x04003186 RID: 12678
	private static readonly float3 AXIS_ZB_FW = new float3(0f, 0f, 1f);

	// Token: 0x04003187 RID: 12679
	private static readonly float3 AXIS_YG_UP = new float3(0f, 1f, 0f);

	// Token: 0x04003188 RID: 12680
	private static readonly float3 AXIS_XR_RT = new float3(1f, 0f, 0f);

	// Token: 0x04003189 RID: 12681
	private static readonly Color CR = new Color(1f, 0f, 0f, 0.24f);

	// Token: 0x0400318A RID: 12682
	private static readonly Color CG = new Color(0f, 1f, 0f, 0.24f);

	// Token: 0x0400318B RID: 12683
	private static readonly Color CB = new Color(0f, 0f, 1f, 0.24f);
}
