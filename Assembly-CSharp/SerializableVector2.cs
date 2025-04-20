using System;
using UnityEngine;

// Token: 0x02000262 RID: 610
[Serializable]
public struct SerializableVector2
{
	// Token: 0x06000E26 RID: 3622 RVA: 0x0003A1F8 File Offset: 0x000383F8
	public SerializableVector2(float x, float y)
	{
		this.x = x;
		this.y = y;
	}

	// Token: 0x06000E27 RID: 3623 RVA: 0x0003A208 File Offset: 0x00038408
	public static implicit operator SerializableVector2(Vector2 v)
	{
		return new SerializableVector2(v.x, v.y);
	}

	// Token: 0x06000E28 RID: 3624 RVA: 0x0003A21B File Offset: 0x0003841B
	public static implicit operator Vector2(SerializableVector2 v)
	{
		return new Vector2(v.x, v.y);
	}

	// Token: 0x04001124 RID: 4388
	public float x;

	// Token: 0x04001125 RID: 4389
	public float y;
}
