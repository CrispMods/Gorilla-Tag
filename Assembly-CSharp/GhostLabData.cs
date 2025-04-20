using System;
using System.Runtime.InteropServices;
using Fusion;
using Fusion.CodeGen;
using UnityEngine;

// Token: 0x020000B8 RID: 184
[NetworkStructWeaved(11)]
[StructLayout(LayoutKind.Explicit, Size = 44)]
public struct GhostLabData : INetworkStruct
{
	// Token: 0x17000054 RID: 84
	// (get) Token: 0x0600049F RID: 1183 RVA: 0x0003372B File Offset: 0x0003192B
	// (set) Token: 0x060004A0 RID: 1184 RVA: 0x00033733 File Offset: 0x00031933
	public int DoorState { readonly get; set; }

	// Token: 0x17000055 RID: 85
	// (get) Token: 0x060004A1 RID: 1185 RVA: 0x0003373C File Offset: 0x0003193C
	[Networked]
	[Capacity(10)]
	public NetworkArray<NetworkBool> OpenDoors
	{
		get
		{
			return new NetworkArray<NetworkBool>(Native.ReferenceToPointer<FixedStorage@10>(ref this._OpenDoors), 10, ReaderWriter@Fusion_NetworkBool.GetInstance());
		}
	}

	// Token: 0x060004A2 RID: 1186 RVA: 0x0007DA48 File Offset: 0x0007BC48
	public GhostLabData(int state, bool[] openDoors)
	{
		this.DoorState = state;
		for (int i = 0; i < openDoors.Length; i++)
		{
			bool val = openDoors[i];
			this.OpenDoors.Set(i, val);
		}
	}

	// Token: 0x0400055A RID: 1370
	[FixedBufferProperty(typeof(NetworkArray<NetworkBool>), typeof(UnityArraySurrogate@ReaderWriter@Fusion_NetworkBool), 10, order = -2147483647)]
	[WeaverGenerated]
	[SerializeField]
	[FieldOffset(4)]
	private FixedStorage@10 _OpenDoors;
}
