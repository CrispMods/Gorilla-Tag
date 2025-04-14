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
	// (get) Token: 0x06000465 RID: 1125 RVA: 0x0001A3FD File Offset: 0x000185FD
	// (set) Token: 0x06000466 RID: 1126 RVA: 0x0001A405 File Offset: 0x00018605
	public int DoorState { readonly get; set; }

	// Token: 0x17000050 RID: 80
	// (get) Token: 0x06000467 RID: 1127 RVA: 0x0001A410 File Offset: 0x00018610
	[Networked]
	[Capacity(10)]
	public NetworkArray<NetworkBool> OpenDoors
	{
		get
		{
			return new NetworkArray<NetworkBool>(Native.ReferenceToPointer<FixedStorage@10>(ref this._OpenDoors), 10, ReaderWriter@Fusion_NetworkBool.GetInstance());
		}
	}

	// Token: 0x06000468 RID: 1128 RVA: 0x0001A438 File Offset: 0x00018638
	public GhostLabData(int state, bool[] openDoors)
	{
		this.DoorState = state;
		for (int i = 0; i < openDoors.Length; i++)
		{
			bool val = openDoors[i];
			this.OpenDoors.Set(i, val);
		}
	}

	// Token: 0x0400051B RID: 1307
	[FixedBufferProperty(typeof(NetworkArray<NetworkBool>), typeof(UnityArraySurrogate@ReaderWriter@Fusion_NetworkBool), 10, order = -2147483647)]
	[WeaverGenerated]
	[SerializeField]
	[FieldOffset(4)]
	private FixedStorage@10 _OpenDoors;
}
