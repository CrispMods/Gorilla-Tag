using System;
using System.IO;
using UnityEngine;

// Token: 0x020004D9 RID: 1241
[Serializable]
public struct SnapBounds
{
	// Token: 0x06001E17 RID: 7703 RVA: 0x0004487D File Offset: 0x00042A7D
	public SnapBounds(Vector2Int min, Vector2Int max)
	{
		this.min = min;
		this.max = max;
	}

	// Token: 0x06001E18 RID: 7704 RVA: 0x0004488D File Offset: 0x00042A8D
	public SnapBounds(int minX, int minY, int maxX, int maxY)
	{
		this.min = new Vector2Int(minX, minY);
		this.max = new Vector2Int(maxX, maxY);
	}

	// Token: 0x06001E19 RID: 7705 RVA: 0x000448AA File Offset: 0x00042AAA
	public void Clear()
	{
		this.min = new Vector2Int(int.MinValue, int.MinValue);
		this.max = new Vector2Int(int.MinValue, int.MinValue);
	}

	// Token: 0x06001E1A RID: 7706 RVA: 0x000E4420 File Offset: 0x000E2620
	public void Write(BinaryWriter writer)
	{
		writer.Write(this.min.x);
		writer.Write(this.min.y);
		writer.Write(this.max.x);
		writer.Write(this.max.y);
	}

	// Token: 0x06001E1B RID: 7707 RVA: 0x000E4474 File Offset: 0x000E2674
	public void Read(BinaryReader reader)
	{
		this.min.x = reader.ReadInt32();
		this.min.y = reader.ReadInt32();
		this.max.x = reader.ReadInt32();
		this.max.y = reader.ReadInt32();
	}

	// Token: 0x0400212A RID: 8490
	public Vector2Int min;

	// Token: 0x0400212B RID: 8491
	public Vector2Int max;
}
