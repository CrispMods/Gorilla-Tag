using System;
using System.Runtime.InteropServices;
using UnityEngine;

// Token: 0x020006C9 RID: 1737
[Serializable]
[StructLayout(LayoutKind.Explicit)]
public struct GlobalObjectRef
{
	// Token: 0x06002B1A RID: 11034 RVA: 0x0011FEA8 File Offset: 0x0011E0A8
	public static GlobalObjectRef ObjectToRefSlow(UnityEngine.Object target)
	{
		return default(GlobalObjectRef);
	}

	// Token: 0x06002B1B RID: 11035 RVA: 0x0003924B File Offset: 0x0003744B
	public static UnityEngine.Object RefToObjectSlow(GlobalObjectRef @ref)
	{
		return null;
	}

	// Token: 0x040030A7 RID: 12455
	[FieldOffset(0)]
	public ulong targetObjectId;

	// Token: 0x040030A8 RID: 12456
	[FieldOffset(8)]
	public ulong targetPrefabId;

	// Token: 0x040030A9 RID: 12457
	[FieldOffset(16)]
	public Guid assetGUID;

	// Token: 0x040030AA RID: 12458
	[HideInInspector]
	[FieldOffset(32)]
	public int identifierType;

	// Token: 0x040030AB RID: 12459
	[NonSerialized]
	[FieldOffset(32)]
	private GlobalObjectRefType refType;
}
