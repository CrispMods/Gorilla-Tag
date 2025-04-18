﻿using System;
using System.IO;
using UnityEngine;

// Token: 0x020004CC RID: 1228
[Serializable]
public struct SnapBounds
{
	// Token: 0x06001DBE RID: 7614 RVA: 0x00091726 File Offset: 0x0008F926
	public SnapBounds(Vector2Int min, Vector2Int max)
	{
		this.min = min;
		this.max = max;
	}

	// Token: 0x06001DBF RID: 7615 RVA: 0x00091736 File Offset: 0x0008F936
	public SnapBounds(int minX, int minY, int maxX, int maxY)
	{
		this.min = new Vector2Int(minX, minY);
		this.max = new Vector2Int(maxX, maxY);
	}

	// Token: 0x06001DC0 RID: 7616 RVA: 0x00091753 File Offset: 0x0008F953
	public void Clear()
	{
		this.min = new Vector2Int(int.MinValue, int.MinValue);
		this.max = new Vector2Int(int.MinValue, int.MinValue);
	}

	// Token: 0x06001DC1 RID: 7617 RVA: 0x00091780 File Offset: 0x0008F980
	public void Write(BinaryWriter writer)
	{
		writer.Write(this.min.x);
		writer.Write(this.min.y);
		writer.Write(this.max.x);
		writer.Write(this.max.y);
	}

	// Token: 0x06001DC2 RID: 7618 RVA: 0x000917D4 File Offset: 0x0008F9D4
	public void Read(BinaryReader reader)
	{
		this.min.x = reader.ReadInt32();
		this.min.y = reader.ReadInt32();
		this.max.x = reader.ReadInt32();
		this.max.y = reader.ReadInt32();
	}

	// Token: 0x040020D7 RID: 8407
	public Vector2Int min;

	// Token: 0x040020D8 RID: 8408
	public Vector2Int max;
}
