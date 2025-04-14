using System;
using System.Runtime.InteropServices;
using Fusion;
using Fusion.CodeGen;
using UnityEngine;

// Token: 0x020000AE RID: 174
[NetworkStructWeaved(11)]
[StructLayout(LayoutKind.Explicit, Size = 44)]
public struct GhostLabData : INetworkStruct
{
	// Token: 0x1700004F RID: 79
	// (get) Token: 0x06000463 RID: 1123 RVA: 0x0001A0D9 File Offset: 0x000182D9
	// (set) Token: 0x06000464 RID: 1124 RVA: 0x0001A0E1 File Offset: 0x000182E1
	public int DoorState { readonly get; set; }

	// Token: 0x17000050 RID: 80
	// (get) Token: 0x06000465 RID: 1125 RVA: 0x0001A0EC File Offset: 0x000182EC
	[Networked]
	[Capacity(10)]
	public NetworkArray<NetworkBool> OpenDoors
	{
		get
		{
			return new NetworkArray<NetworkBool>(Native.ReferenceToPointer<FixedStorage@10>(ref this._OpenDoors), 10, ReaderWriter@Fusion_NetworkBool.GetInstance());
		}
	}

	// Token: 0x06000466 RID: 1126 RVA: 0x0001A114 File Offset: 0x00018314
	public GhostLabData(int state, bool[] openDoors)
	{
		this.DoorState = state;
		for (int i = 0; i < openDoors.Length; i++)
		{
			bool val = openDoors[i];
			this.OpenDoors.Set(i, val);
		}
	}

	// Token: 0x0400051A RID: 1306
	[FixedBufferProperty(typeof(NetworkArray<NetworkBool>), typeof(UnityArraySurrogate@ReaderWriter@Fusion_NetworkBool), 10, order = -2147483647)]
	[WeaverGenerated]
	[SerializeField]
	[FieldOffset(4)]
	private FixedStorage@10 _OpenDoors;
}
