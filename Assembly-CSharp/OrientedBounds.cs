using System;
using UnityEngine;

// Token: 0x020006E1 RID: 1761
[Serializable]
public struct OrientedBounds
{
	// Token: 0x1700049B RID: 1179
	// (get) Token: 0x06002BA9 RID: 11177 RVA: 0x0004D8C8 File Offset: 0x0004BAC8
	public static OrientedBounds Empty { get; } = new OrientedBounds
	{
		size = Vector3.zero,
		center = Vector3.zero,
		rotation = Quaternion.identity
	};

	// Token: 0x1700049C RID: 1180
	// (get) Token: 0x06002BAA RID: 11178 RVA: 0x0004D8CF File Offset: 0x0004BACF
	public static OrientedBounds Identity { get; } = new OrientedBounds
	{
		size = Vector3.one,
		center = Vector3.zero,
		rotation = Quaternion.identity
	};

	// Token: 0x06002BAB RID: 11179 RVA: 0x0004D8D6 File Offset: 0x0004BAD6
	public Matrix4x4 TRS()
	{
		return Matrix4x4.TRS(this.center, this.rotation, this.size);
	}

	// Token: 0x04003132 RID: 12594
	public Vector3 size;

	// Token: 0x04003133 RID: 12595
	public Vector3 center;

	// Token: 0x04003134 RID: 12596
	public Quaternion rotation;
}
