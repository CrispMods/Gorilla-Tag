using System;
using UnityEngine;

// Token: 0x02000257 RID: 599
[Serializable]
public struct SerializableVector2
{
	// Token: 0x06000DDD RID: 3549 RVA: 0x00038F38 File Offset: 0x00037138
	public SerializableVector2(float x, float y)
	{
		this.x = x;
		this.y = y;
	}

	// Token: 0x06000DDE RID: 3550 RVA: 0x00038F48 File Offset: 0x00037148
	public static implicit operator SerializableVector2(Vector2 v)
	{
		return new SerializableVector2(v.x, v.y);
	}

	// Token: 0x06000DDF RID: 3551 RVA: 0x00038F5B File Offset: 0x0003715B
	public static implicit operator Vector2(SerializableVector2 v)
	{
		return new Vector2(v.x, v.y);
	}

	// Token: 0x040010DF RID: 4319
	public float x;

	// Token: 0x040010E0 RID: 4320
	public float y;
}
