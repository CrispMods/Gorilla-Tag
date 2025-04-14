using System;
using System.Runtime.InteropServices;
using Fusion;
using Fusion.CodeGen;
using UnityEngine;

// Token: 0x02000467 RID: 1127
[NetworkStructWeaved(12)]
[StructLayout(LayoutKind.Explicit, Size = 48)]
public struct TagData : INetworkStruct
{
	// Token: 0x17000306 RID: 774
	// (get) Token: 0x06001B9E RID: 7070 RVA: 0x00087820 File Offset: 0x00085A20
	[Networked]
	[Capacity(10)]
	public NetworkArray<int> infectedPlayerList
	{
		get
		{
			return new NetworkArray<int>(Native.ReferenceToPointer<FixedStorage@10>(ref this._infectedPlayerList), 10, ReaderWriter@System_Int32.GetInstance());
		}
	}

	// Token: 0x17000307 RID: 775
	// (get) Token: 0x06001B9F RID: 7071 RVA: 0x00087847 File Offset: 0x00085A47
	// (set) Token: 0x06001BA0 RID: 7072 RVA: 0x0008784F File Offset: 0x00085A4F
	public int currentItID { readonly get; set; }

	// Token: 0x04001E85 RID: 7813
	[FieldOffset(4)]
	public NetworkBool isCurrentlyTag;

	// Token: 0x04001E86 RID: 7814
	[FixedBufferProperty(typeof(NetworkArray<int>), typeof(UnityArraySurrogate@ReaderWriter@System_Int32), 10, order = -2147483647)]
	[WeaverGenerated]
	[SerializeField]
	[FieldOffset(8)]
	private FixedStorage@10 _infectedPlayerList;
}
