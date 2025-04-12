using System;
using UnityEngine;

// Token: 0x020006DD RID: 1757
[Serializable]
public class XformNode
{
	// Token: 0x170004A0 RID: 1184
	// (get) Token: 0x06002BC9 RID: 11209 RVA: 0x0011D718 File Offset: 0x0011B918
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

	// Token: 0x170004A1 RID: 1185
	// (get) Token: 0x06002BCA RID: 11210 RVA: 0x0004CECB File Offset: 0x0004B0CB
	// (set) Token: 0x06002BCB RID: 11211 RVA: 0x0004CED8 File Offset: 0x0004B0D8
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

	// Token: 0x06002BCC RID: 11212 RVA: 0x0004CEE6 File Offset: 0x0004B0E6
	public Matrix4x4 LocalTRS()
	{
		return Matrix4x4.TRS(this.localPosition, Quaternion.identity, Vector3.one);
	}

	// Token: 0x040030F5 RID: 12533
	public Vector4 localPosition;

	// Token: 0x040030F6 RID: 12534
	public Transform parent;
}
