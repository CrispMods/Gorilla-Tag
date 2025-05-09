﻿using System;
using UnityEngine;

// Token: 0x020006CD RID: 1741
[Serializable]
public struct OrientedBounds
{
	// Token: 0x1700048F RID: 1167
	// (get) Token: 0x06002B1B RID: 11035 RVA: 0x0004C583 File Offset: 0x0004A783
	public static OrientedBounds Empty { get; } = new OrientedBounds
	{
		size = Vector3.zero,
		center = Vector3.zero,
		rotation = Quaternion.identity
	};

	// Token: 0x17000490 RID: 1168
	// (get) Token: 0x06002B1C RID: 11036 RVA: 0x0004C58A File Offset: 0x0004A78A
	public static OrientedBounds Identity { get; } = new OrientedBounds
	{
		size = Vector3.one,
		center = Vector3.zero,
		rotation = Quaternion.identity
	};

	// Token: 0x06002B1D RID: 11037 RVA: 0x0004C591 File Offset: 0x0004A791
	public Matrix4x4 TRS()
	{
		return Matrix4x4.TRS(this.center, this.rotation, this.size);
	}

	// Token: 0x0400309B RID: 12443
	public Vector3 size;

	// Token: 0x0400309C RID: 12444
	public Vector3 center;

	// Token: 0x0400309D RID: 12445
	public Quaternion rotation;
}
