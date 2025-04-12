using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Fusion;
using Fusion.CodeGen;
using UnityEngine;

// Token: 0x02000524 RID: 1316
[NetworkStructWeaved(337)]
[StructLayout(LayoutKind.Explicit, Size = 1348)]
public struct FlockingData : INetworkStruct
{
	// Token: 0x1700033B RID: 827
	// (get) Token: 0x06001FEB RID: 8171 RVA: 0x00044B69 File Offset: 0x00042D69
	// (set) Token: 0x06001FEC RID: 8172 RVA: 0x00044B71 File Offset: 0x00042D71
	public int count { readonly get; set; }

	// Token: 0x1700033C RID: 828
	// (get) Token: 0x06001FED RID: 8173 RVA: 0x00044B7A File Offset: 0x00042D7A
	[Networked]
	[Capacity(30)]
	public NetworkLinkedList<Vector3> Positions
	{
		get
		{
			return new NetworkLinkedList<Vector3>(Native.ReferenceToPointer<FixedStorage@153>(ref this._Positions), 30, ReaderWriter@UnityEngine_Vector3.GetInstance());
		}
	}

	// Token: 0x1700033D RID: 829
	// (get) Token: 0x06001FEE RID: 8174 RVA: 0x00044B96 File Offset: 0x00042D96
	[Networked]
	[Capacity(30)]
	public NetworkLinkedList<Quaternion> Rotations
	{
		get
		{
			return new NetworkLinkedList<Quaternion>(Native.ReferenceToPointer<FixedStorage@183>(ref this._Rotations), 30, ReaderWriter@UnityEngine_Quaternion.GetInstance());
		}
	}

	// Token: 0x06001FEF RID: 8175 RVA: 0x000EF984 File Offset: 0x000EDB84
	public FlockingData(List<Flocking> items)
	{
		this.count = items.Count;
		foreach (Flocking flocking in items)
		{
			this.Positions.Add(flocking.pos);
			this.Rotations.Add(flocking.rot);
		}
	}

	// Token: 0x040023F4 RID: 9204
	[FixedBufferProperty(typeof(NetworkLinkedList<Vector3>), typeof(UnityLinkedListSurrogate@ReaderWriter@UnityEngine_Vector3), 30, order = -2147483647)]
	[WeaverGenerated]
	[SerializeField]
	[FieldOffset(4)]
	private FixedStorage@153 _Positions;

	// Token: 0x040023F5 RID: 9205
	[FixedBufferProperty(typeof(NetworkLinkedList<Quaternion>), typeof(UnityLinkedListSurrogate@ReaderWriter@UnityEngine_Quaternion), 30, order = -2147483647)]
	[WeaverGenerated]
	[SerializeField]
	[FieldOffset(616)]
	private FixedStorage@183 _Rotations;
}
