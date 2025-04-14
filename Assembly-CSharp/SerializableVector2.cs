using System;
using UnityEngine;

// Token: 0x02000257 RID: 599
[Serializable]
public struct SerializableVector2
{
	// Token: 0x06000DDB RID: 3547 RVA: 0x00046811 File Offset: 0x00044A11
	public SerializableVector2(float x, float y)
	{
		this.x = x;
		this.y = y;
	}

	// Token: 0x06000DDC RID: 3548 RVA: 0x00046821 File Offset: 0x00044A21
	public static implicit operator SerializableVector2(Vector2 v)
	{
		return new SerializableVector2(v.x, v.y);
	}

	// Token: 0x06000DDD RID: 3549 RVA: 0x00046834 File Offset: 0x00044A34
	public static implicit operator Vector2(SerializableVector2 v)
	{
		return new Vector2(v.x, v.y);
	}

	// Token: 0x040010DE RID: 4318
	public float x;

	// Token: 0x040010DF RID: 4319
	public float y;
}
