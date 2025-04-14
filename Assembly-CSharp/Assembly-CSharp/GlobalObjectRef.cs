using System;
using System.Runtime.InteropServices;
using UnityEngine;

// Token: 0x020006B5 RID: 1717
[Serializable]
[StructLayout(LayoutKind.Explicit)]
public struct GlobalObjectRef
{
	// Token: 0x06002A8C RID: 10892 RVA: 0x000D4084 File Offset: 0x000D2284
	public static GlobalObjectRef ObjectToRefSlow(Object target)
	{
		return default(GlobalObjectRef);
	}

	// Token: 0x06002A8D RID: 10893 RVA: 0x00043175 File Offset: 0x00041375
	public static Object RefToObjectSlow(GlobalObjectRef @ref)
	{
		return null;
	}

	// Token: 0x04003010 RID: 12304
	[FieldOffset(0)]
	public ulong targetObjectId;

	// Token: 0x04003011 RID: 12305
	[FieldOffset(8)]
	public ulong targetPrefabId;

	// Token: 0x04003012 RID: 12306
	[FieldOffset(16)]
	public Guid assetGUID;

	// Token: 0x04003013 RID: 12307
	[HideInInspector]
	[FieldOffset(32)]
	public int identifierType;

	// Token: 0x04003014 RID: 12308
	[NonSerialized]
	[FieldOffset(32)]
	private GlobalObjectRefType refType;
}
