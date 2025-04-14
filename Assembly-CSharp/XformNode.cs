using System;
using UnityEngine;

// Token: 0x020006DC RID: 1756
[Serializable]
public class XformNode
{
	// Token: 0x1700049F RID: 1183
	// (get) Token: 0x06002BC1 RID: 11201 RVA: 0x000D7104 File Offset: 0x000D5304
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

	// Token: 0x170004A0 RID: 1184
	// (get) Token: 0x06002BC2 RID: 11202 RVA: 0x000D7142 File Offset: 0x000D5342
	// (set) Token: 0x06002BC3 RID: 11203 RVA: 0x000D714F File Offset: 0x000D534F
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

	// Token: 0x06002BC4 RID: 11204 RVA: 0x000D715D File Offset: 0x000D535D
	public Matrix4x4 LocalTRS()
	{
		return Matrix4x4.TRS(this.localPosition, Quaternion.identity, Vector3.one);
	}

	// Token: 0x040030EF RID: 12527
	public Vector4 localPosition;

	// Token: 0x040030F0 RID: 12528
	public Transform parent;
}
