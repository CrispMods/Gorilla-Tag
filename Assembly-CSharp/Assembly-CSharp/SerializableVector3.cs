using System;
using UnityEngine;

// Token: 0x02000258 RID: 600
[Serializable]
public struct SerializableVector3
{
	// Token: 0x06000DE0 RID: 3552 RVA: 0x00046B8B File Offset: 0x00044D8B
	public SerializableVector3(float x, float y, float z)
	{
		this.x = x;
		this.y = y;
		this.z = z;
	}

	// Token: 0x06000DE1 RID: 3553 RVA: 0x00046BA2 File Offset: 0x00044DA2
	public static implicit operator SerializableVector3(Vector3 v)
	{
		return new SerializableVector3(v.x, v.y, v.z);
	}

	// Token: 0x06000DE2 RID: 3554 RVA: 0x00046BBB File Offset: 0x00044DBB
	public static implicit operator Vector3(SerializableVector3 v)
	{
		return new Vector3(v.x, v.y, v.z);
	}

	// Token: 0x040010E1 RID: 4321
	public float x;

	// Token: 0x040010E2 RID: 4322
	public float y;

	// Token: 0x040010E3 RID: 4323
	public float z;
}
