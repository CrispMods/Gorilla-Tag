using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Fusion;
using Fusion.CodeGen;
using UnityEngine;

// Token: 0x02000531 RID: 1329
[NetworkStructWeaved(337)]
[StructLayout(LayoutKind.Explicit, Size = 1348)]
public struct FlockingData : INetworkStruct
{
	// Token: 0x17000342 RID: 834
	// (get) Token: 0x06002041 RID: 8257 RVA: 0x00045F08 File Offset: 0x00044108
	// (set) Token: 0x06002042 RID: 8258 RVA: 0x00045F10 File Offset: 0x00044110
	public int count { readonly get; set; }

	// Token: 0x17000343 RID: 835
	// (get) Token: 0x06002043 RID: 8259 RVA: 0x00045F19 File Offset: 0x00044119
	[Networked]
	[Capacity(30)]
	public NetworkLinkedList<Vector3> Positions
	{
		get
		{
			return new NetworkLinkedList<Vector3>(Native.ReferenceToPointer<FixedStorage@153>(ref this._Positions), 30, ReaderWriter@UnityEngine_Vector3.GetInstance());
		}
	}

	// Token: 0x17000344 RID: 836
	// (get) Token: 0x06002044 RID: 8260 RVA: 0x00045F35 File Offset: 0x00044135
	[Networked]
	[Capacity(30)]
	public NetworkLinkedList<Quaternion> Rotations
	{
		get
		{
			return new NetworkLinkedList<Quaternion>(Native.ReferenceToPointer<FixedStorage@183>(ref this._Rotations), 30, ReaderWriter@UnityEngine_Quaternion.GetInstance());
		}
	}

	// Token: 0x06002045 RID: 8261 RVA: 0x000F2708 File Offset: 0x000F0908
	public FlockingData(List<Flocking> items)
	{
		this.count = items.Count;
		foreach (Flocking flocking in items)
		{
			this.Positions.Add(flocking.pos);
			this.Rotations.Add(flocking.rot);
		}
	}

	// Token: 0x04002446 RID: 9286
	[FixedBufferProperty(typeof(NetworkLinkedList<Vector3>), typeof(UnityLinkedListSurrogate@ReaderWriter@UnityEngine_Vector3), 30, order = -2147483647)]
	[WeaverGenerated]
	[SerializeField]
	[FieldOffset(4)]
	private FixedStorage@153 _Positions;

	// Token: 0x04002447 RID: 9287
	[FixedBufferProperty(typeof(NetworkLinkedList<Quaternion>), typeof(UnityLinkedListSurrogate@ReaderWriter@UnityEngine_Quaternion), 30, order = -2147483647)]
	[WeaverGenerated]
	[SerializeField]
	[FieldOffset(616)]
	private FixedStorage@183 _Rotations;
}
