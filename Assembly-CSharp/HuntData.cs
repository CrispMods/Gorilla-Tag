using System;
using System.Runtime.InteropServices;
using Fusion;
using Fusion.CodeGen;
using UnityEngine;

// Token: 0x02000465 RID: 1125
[NetworkStructWeaved(23)]
[StructLayout(LayoutKind.Explicit, Size = 92)]
public struct HuntData : INetworkStruct
{
	// Token: 0x17000302 RID: 770
	// (get) Token: 0x06001B92 RID: 7058 RVA: 0x000873B0 File Offset: 0x000855B0
	[Networked]
	[Capacity(10)]
	public NetworkArray<int> currentHuntedArray
	{
		get
		{
			return new NetworkArray<int>(Native.ReferenceToPointer<FixedStorage@10>(ref this._currentHuntedArray), 10, ReaderWriter@System_Int32.GetInstance());
		}
	}

	// Token: 0x17000303 RID: 771
	// (get) Token: 0x06001B93 RID: 7059 RVA: 0x000873D8 File Offset: 0x000855D8
	[Networked]
	[Capacity(10)]
	public NetworkArray<int> currentTargetArray
	{
		get
		{
			return new NetworkArray<int>(Native.ReferenceToPointer<FixedStorage@10>(ref this._currentTargetArray), 10, ReaderWriter@System_Int32.GetInstance());
		}
	}

	// Token: 0x04001E7D RID: 7805
	[FieldOffset(0)]
	public NetworkBool huntStarted;

	// Token: 0x04001E7E RID: 7806
	[FieldOffset(4)]
	public NetworkBool waitingToStartNextHuntGame;

	// Token: 0x04001E7F RID: 7807
	[FieldOffset(8)]
	public int countDownTime;

	// Token: 0x04001E80 RID: 7808
	[FixedBufferProperty(typeof(NetworkArray<int>), typeof(UnityArraySurrogate@ReaderWriter@System_Int32), 10, order = -2147483647)]
	[WeaverGenerated]
	[SerializeField]
	[FieldOffset(12)]
	private FixedStorage@10 _currentHuntedArray;

	// Token: 0x04001E81 RID: 7809
	[FixedBufferProperty(typeof(NetworkArray<int>), typeof(UnityArraySurrogate@ReaderWriter@System_Int32), 10, order = -2147483647)]
	[WeaverGenerated]
	[SerializeField]
	[FieldOffset(52)]
	private FixedStorage@10 _currentTargetArray;
}
