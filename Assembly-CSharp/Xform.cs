using System;
using Drawing;
using Unity.Mathematics;
using UnityEngine;

// Token: 0x020006DB RID: 1755
[ExecuteAlways]
public class Xform : MonoBehaviour
{
	// Token: 0x1700049E RID: 1182
	// (get) Token: 0x06002BBB RID: 11195 RVA: 0x000D6E77 File Offset: 0x000D5077
	public float3 localExtents
	{
		get
		{
			return this.localScale * 0.5f;
		}
	}

	// Token: 0x06002BBC RID: 11196 RVA: 0x000D6E89 File Offset: 0x000D5089
	public Matrix4x4 LocalTRS()
	{
		return Matrix4x4.TRS(this.localPosition, this.localRotation, this.localScale);
	}

	// Token: 0x06002BBD RID: 11197 RVA: 0x000D6EAC File Offset: 0x000D50AC
	public Matrix4x4 TRS()
	{
		if (this.parent.AsNull<Transform>() == null)
		{
			return this.LocalTRS();
		}
		return this.parent.localToWorldMatrix * this.LocalTRS();
	}

	// Token: 0x06002BBE RID: 11198 RVA: 0x000D6EE0 File Offset: 0x000D50E0
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

	// Token: 0x040030E2 RID: 12514
	public Transform parent;

	// Token: 0x040030E3 RID: 12515
	[Space]
	public Color displayColor = SRand.New().NextColor();

	// Token: 0x040030E4 RID: 12516
	[Space]
	public float3 localPosition = float3.zero;

	// Token: 0x040030E5 RID: 12517
	public float3 localScale = Vector3.one;

	// Token: 0x040030E6 RID: 12518
	public Quaternion localRotation = quaternion.identity;

	// Token: 0x040030E7 RID: 12519
	private static readonly float3 F3_ONE = 1f;

	// Token: 0x040030E8 RID: 12520
	private static readonly float2 F2_ONE = 1f;

	// Token: 0x040030E9 RID: 12521
	private static readonly float3 AXIS_ZB_FW = new float3(0f, 0f, 1f);

	// Token: 0x040030EA RID: 12522
	private static readonly float3 AXIS_YG_UP = new float3(0f, 1f, 0f);

	// Token: 0x040030EB RID: 12523
	private static readonly float3 AXIS_XR_RT = new float3(1f, 0f, 0f);

	// Token: 0x040030EC RID: 12524
	private static readonly Color CR = new Color(1f, 0f, 0f, 0.24f);

	// Token: 0x040030ED RID: 12525
	private static readonly Color CG = new Color(0f, 1f, 0f, 0.24f);

	// Token: 0x040030EE RID: 12526
	private static readonly Color CB = new Color(0f, 0f, 1f, 0.24f);
}
