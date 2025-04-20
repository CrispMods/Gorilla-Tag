using System;
using UnityEngine;

// Token: 0x020006F1 RID: 1777
[Serializable]
public class XformNode
{
	// Token: 0x170004AC RID: 1196
	// (get) Token: 0x06002C57 RID: 11351 RVA: 0x001222D0 File Offset: 0x001204D0
	public Vector4 worldPosition
	{
		get
		{
			if (!this.parent)
			{
				return this.localPosition;
			}
			Matrix4x4 localToWorldMatrix = this.parent.localToWorldMatrix;
			Vector4 result = this.localPosition;
			MatrixUtils.MultiplyXYZ3x4(ref localToWorldMatrix, ref result);
			return result;
		}
	}

	// Token: 0x170004AD RID: 1197
	// (get) Token: 0x06002C58 RID: 11352 RVA: 0x0004E210 File Offset: 0x0004C410
	// (set) Token: 0x06002C59 RID: 11353 RVA: 0x0004E21D File Offset: 0x0004C41D
	public float radius
	{
		get
		{
			return this.localPosition.w;
		}
		set
		{
			this.localPosition.w = value;
		}
	}

	// Token: 0x06002C5A RID: 11354 RVA: 0x0004E22B File Offset: 0x0004C42B
	public Matrix4x4 LocalTRS()
	{
		return Matrix4x4.TRS(this.localPosition, Quaternion.identity, Vector3.one);
	}

	// Token: 0x0400318C RID: 12684
	public Vector4 localPosition;

	// Token: 0x0400318D RID: 12685
	public Transform parent;
}
