using System;
using UnityEngine;

// Token: 0x02000257 RID: 599
[Serializable]
public struct SerializableVector2
{
	// Token: 0x06000DDD RID: 3549 RVA: 0x00046B55 File Offset: 0x00044D55
	public SerializableVector2(float x, float y)
	{
		this.x = x;
		this.y = y;
	}

	// Token: 0x06000DDE RID: 3550 RVA: 0x00046B65 File Offset: 0x00044D65
	public static implicit operator SerializableVector2(Vector2 v)
	{
		return new SerializableVector2(v.x, v.y);
	}

	// Token: 0x06000DDF RID: 3551 RVA: 0x00046B78 File Offset: 0x00044D78
	public static implicit operator Vector2(SerializableVector2 v)
	{
		return new Vector2(v.x, v.y);
	}

	// Token: 0x040010DF RID: 4319
	public float x;

	// Token: 0x040010E0 RID: 4320
	public float y;
}
