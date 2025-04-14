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
	// (get) Token: 0x06001B9B RID: 7067 RVA: 0x0008749C File Offset: 0x0008569C
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
	// (get) Token: 0x06001B9C RID: 7068 RVA: 0x000874C3 File Offset: 0x000856C3
	// (set) Token: 0x06001B9D RID: 7069 RVA: 0x000874CB File Offset: 0x000856CB
	public int currentItID { readonly get; set; }

	// Token: 0x04001E84 RID: 7812
	[FieldOffset(4)]
	public NetworkBool isCurrentlyTag;

	// Token: 0x04001E85 RID: 7813
	[FixedBufferProperty(typeof(NetworkArray<int>), typeof(UnityArraySurrogate@ReaderWriter@System_Int32), 10, order = -2147483647)]
	[WeaverGenerated]
	[SerializeField]
	[FieldOffset(8)]
	private FixedStorage@10 _infectedPlayerList;
}
