using System;
using UnityEngine;

// Token: 0x02000258 RID: 600
[Serializable]
public struct SerializableVector3
{
	// Token: 0x06000DDE RID: 3550 RVA: 0x00046847 File Offset: 0x00044A47
	public SerializableVector3(float x, float y, float z)
	{
		this.x = x;
		this.y = y;
		this.z = z;
	}

	// Token: 0x06000DDF RID: 3551 RVA: 0x0004685E File Offset: 0x00044A5E
	public static implicit operator SerializableVector3(Vector3 v)
	{
		return new SerializableVector3(v.x, v.y, v.z);
	}

	// Token: 0x06000DE0 RID: 3552 RVA: 0x00046877 File Offset: 0x00044A77
	public static implicit operator Vector3(SerializableVector3 v)
	{
		return new Vector3(v.x, v.y, v.z);
	}

	// Token: 0x040010E0 RID: 4320
	public float x;

	// Token: 0x040010E1 RID: 4321
	public float y;

	// Token: 0x040010E2 RID: 4322
	public float z;
}
