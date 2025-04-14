using System;
using UnityEngine;

// Token: 0x020006CC RID: 1740
[Serializable]
public struct OrientedBounds
{
	// Token: 0x1700048E RID: 1166
	// (get) Token: 0x06002B13 RID: 11027 RVA: 0x000D56BC File Offset: 0x000D38BC
	public static OrientedBounds Empty { get; } = new OrientedBounds
	{
		size = Vector3.zero,
		center = Vector3.zero,
		rotation = Quaternion.identity
	};

	// Token: 0x1700048F RID: 1167
	// (get) Token: 0x06002B14 RID: 11028 RVA: 0x000D56C3 File Offset: 0x000D38C3
	public static OrientedBounds Identity { get; } = new OrientedBounds
	{
		size = Vector3.one,
		center = Vector3.zero,
		rotation = Quaternion.identity
	};

	// Token: 0x06002B15 RID: 11029 RVA: 0x000D56CA File Offset: 0x000D38CA
	public Matrix4x4 TRS()
	{
		return Matrix4x4.TRS(this.center, this.rotation, this.size);
	}

	// Token: 0x04003095 RID: 12437
	public Vector3 size;

	// Token: 0x04003096 RID: 12438
	public Vector3 center;

	// Token: 0x04003097 RID: 12439
	public Quaternion rotation;
}
