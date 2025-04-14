using System;
using System.Runtime.InteropServices;
using UnityEngine;

// Token: 0x020006B4 RID: 1716
[Serializable]
[StructLayout(LayoutKind.Explicit)]
public struct GlobalObjectRef
{
	// Token: 0x06002A84 RID: 10884 RVA: 0x000D3C04 File Offset: 0x000D1E04
	public static GlobalObjectRef ObjectToRefSlow(Object target)
	{
		return default(GlobalObjectRef);
	}

	// Token: 0x06002A85 RID: 10885 RVA: 0x00042E31 File Offset: 0x00041031
	public static Object RefToObjectSlow(GlobalObjectRef @ref)
	{
		return null;
	}

	// Token: 0x0400300A RID: 12298
	[FieldOffset(0)]
	public ulong targetObjectId;

	// Token: 0x0400300B RID: 12299
	[FieldOffset(8)]
	public ulong targetPrefabId;

	// Token: 0x0400300C RID: 12300
	[FieldOffset(16)]
	public Guid assetGUID;

	// Token: 0x0400300D RID: 12301
	[HideInInspector]
	[FieldOffset(32)]
	public int identifierType;

	// Token: 0x0400300E RID: 12302
	[NonSerialized]
	[FieldOffset(32)]
	private GlobalObjectRefType refType;
}
