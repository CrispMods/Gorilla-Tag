using System;
using UnityEngine;

// Token: 0x02000263 RID: 611
[Serializable]
public struct SerializableVector3
{
	// Token: 0x06000E29 RID: 3625 RVA: 0x0003A22E File Offset: 0x0003842E
	public SerializableVector3(float x, float y, float z)
	{
		this.x = x;
		this.y = y;
		this.z = z;
	}

	// Token: 0x06000E2A RID: 3626 RVA: 0x0003A245 File Offset: 0x00038445
	public static implicit operator SerializableVector3(Vector3 v)
	{
		return new SerializableVector3(v.x, v.y, v.z);
	}

	// Token: 0x06000E2B RID: 3627 RVA: 0x0003A25E File Offset: 0x0003845E
	public static implicit operator Vector3(SerializableVector3 v)
	{
		return new Vector3(v.x, v.y, v.z);
	}

	// Token: 0x04001126 RID: 4390
	public float x;

	// Token: 0x04001127 RID: 4391
	public float y;

	// Token: 0x04001128 RID: 4392
	public float z;
}
