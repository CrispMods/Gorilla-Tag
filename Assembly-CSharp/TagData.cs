using System;
using System.Runtime.InteropServices;
using Fusion;
using Fusion.CodeGen;
using UnityEngine;

// Token: 0x02000473 RID: 1139
[NetworkStructWeaved(12)]
[StructLayout(LayoutKind.Explicit, Size = 48)]
public struct TagData : INetworkStruct
{
	// Token: 0x1700030D RID: 781
	// (get) Token: 0x06001BEF RID: 7151 RVA: 0x00043282 File Offset: 0x00041482
	[Networked]
	[Capacity(10)]
	public NetworkArray<int> infectedPlayerList
	{
		get
		{
			return new NetworkArray<int>(Native.ReferenceToPointer<FixedStorage@10>(ref this._infectedPlayerList), 10, ReaderWriter@System_Int32.GetInstance());
		}
	}

	// Token: 0x1700030E RID: 782
	// (get) Token: 0x06001BF0 RID: 7152 RVA: 0x0004329E File Offset: 0x0004149E
	// (set) Token: 0x06001BF1 RID: 7153 RVA: 0x000432A6 File Offset: 0x000414A6
	public int currentItID { readonly get; set; }

	// Token: 0x04001ED3 RID: 7891
	[FieldOffset(4)]
	public NetworkBool isCurrentlyTag;

	// Token: 0x04001ED4 RID: 7892
	[FixedBufferProperty(typeof(NetworkArray<int>), typeof(UnityArraySurrogate@ReaderWriter@System_Int32), 10, order = -2147483647)]
	[WeaverGenerated]
	[SerializeField]
	[FieldOffset(8)]
	private FixedStorage@10 _infectedPlayerList;
}
