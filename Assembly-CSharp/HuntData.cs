using System;
using System.Runtime.InteropServices;
using Fusion;
using Fusion.CodeGen;
using UnityEngine;

// Token: 0x02000471 RID: 1137
[NetworkStructWeaved(23)]
[StructLayout(LayoutKind.Explicit, Size = 92)]
public struct HuntData : INetworkStruct
{
	// Token: 0x17000309 RID: 777
	// (get) Token: 0x06001BE6 RID: 7142 RVA: 0x000431AE File Offset: 0x000413AE
	[Networked]
	[Capacity(10)]
	public NetworkArray<int> currentHuntedArray
	{
		get
		{
			return new NetworkArray<int>(Native.ReferenceToPointer<FixedStorage@10>(ref this._currentHuntedArray), 10, ReaderWriter@System_Int32.GetInstance());
		}
	}

	// Token: 0x1700030A RID: 778
	// (get) Token: 0x06001BE7 RID: 7143 RVA: 0x000431CA File Offset: 0x000413CA
	[Networked]
	[Capacity(10)]
	public NetworkArray<int> currentTargetArray
	{
		get
		{
			return new NetworkArray<int>(Native.ReferenceToPointer<FixedStorage@10>(ref this._currentTargetArray), 10, ReaderWriter@System_Int32.GetInstance());
		}
	}

	// Token: 0x04001ECC RID: 7884
	[FieldOffset(0)]
	public NetworkBool huntStarted;

	// Token: 0x04001ECD RID: 7885
	[FieldOffset(4)]
	public NetworkBool waitingToStartNextHuntGame;

	// Token: 0x04001ECE RID: 7886
	[FieldOffset(8)]
	public int countDownTime;

	// Token: 0x04001ECF RID: 7887
	[FixedBufferProperty(typeof(NetworkArray<int>), typeof(UnityArraySurrogate@ReaderWriter@System_Int32), 10, order = -2147483647)]
	[WeaverGenerated]
	[SerializeField]
	[FieldOffset(12)]
	private FixedStorage@10 _currentHuntedArray;

	// Token: 0x04001ED0 RID: 7888
	[FixedBufferProperty(typeof(NetworkArray<int>), typeof(UnityArraySurrogate@ReaderWriter@System_Int32), 10, order = -2147483647)]
	[WeaverGenerated]
	[SerializeField]
	[FieldOffset(52)]
	private FixedStorage@10 _currentTargetArray;
}
