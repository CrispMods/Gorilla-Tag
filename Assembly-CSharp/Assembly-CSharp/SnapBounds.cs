using System;
using System.IO;
using UnityEngine;

// Token: 0x020004CC RID: 1228
[Serializable]
public struct SnapBounds
{
	// Token: 0x06001DC1 RID: 7617 RVA: 0x00091AAA File Offset: 0x0008FCAA
	public SnapBounds(Vector2Int min, Vector2Int max)
	{
		this.min = min;
		this.max = max;
	}

	// Token: 0x06001DC2 RID: 7618 RVA: 0x00091ABA File Offset: 0x0008FCBA
	public SnapBounds(int minX, int minY, int maxX, int maxY)
	{
		this.min = new Vector2Int(minX, minY);
		this.max = new Vector2Int(maxX, maxY);
	}

	// Token: 0x06001DC3 RID: 7619 RVA: 0x00091AD7 File Offset: 0x0008FCD7
	public void Clear()
	{
		this.min = new Vector2Int(int.MinValue, int.MinValue);
		this.max = new Vector2Int(int.MinValue, int.MinValue);
	}

	// Token: 0x06001DC4 RID: 7620 RVA: 0x00091B04 File Offset: 0x0008FD04
	public void Write(BinaryWriter writer)
	{
		writer.Write(this.min.x);
		writer.Write(this.min.y);
		writer.Write(this.max.x);
		writer.Write(this.max.y);
	}

	// Token: 0x06001DC5 RID: 7621 RVA: 0x00091B58 File Offset: 0x0008FD58
	public void Read(BinaryReader reader)
	{
		this.min.x = reader.ReadInt32();
		this.min.y = reader.ReadInt32();
		this.max.x = reader.ReadInt32();
		this.max.y = reader.ReadInt32();
	}

	// Token: 0x040020D8 RID: 8408
	public Vector2Int min;

	// Token: 0x040020D9 RID: 8409
	public Vector2Int max;
}
