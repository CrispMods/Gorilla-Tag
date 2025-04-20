using System;
using System.Runtime.InteropServices;
using Fusion;
using Fusion.CodeGen;
using UnityEngine;

// Token: 0x0200046A RID: 1130
[NetworkStructWeaved(31)]
[StructLayout(LayoutKind.Explicit, Size = 124)]
public struct PaintbrawlData : INetworkStruct
{
	// Token: 0x17000301 RID: 769
	// (get) Token: 0x06001BC0 RID: 7104 RVA: 0x00042FD4 File Offset: 0x000411D4
	[Networked]
	[Capacity(10)]
	public NetworkArray<int> playerLivesArray
	{
		get
		{
			return new NetworkArray<int>(Native.ReferenceToPointer<FixedStorage@10>(ref this._playerLivesArray), 10, ReaderWriter@System_Int32.GetInstance());
		}
	}

	// Token: 0x17000302 RID: 770
	// (get) Token: 0x06001BC1 RID: 7105 RVA: 0x00042FF0 File Offset: 0x000411F0
	[Networked]
	[Capacity(10)]
	public NetworkArray<int> playerActorNumberArray
	{
		get
		{
			return new NetworkArray<int>(Native.ReferenceToPointer<FixedStorage@10>(ref this._playerActorNumberArray), 10, ReaderWriter@System_Int32.GetInstance());
		}
	}

	// Token: 0x17000303 RID: 771
	// (get) Token: 0x06001BC2 RID: 7106 RVA: 0x0004300C File Offset: 0x0004120C
	[Networked]
	[Capacity(10)]
	public NetworkArray<GorillaPaintbrawlManager.PaintbrawlStatus> playerStatusArray
	{
		get
		{
			return new NetworkArray<GorillaPaintbrawlManager.PaintbrawlStatus>(Native.ReferenceToPointer<FixedStorage@10>(ref this._playerStatusArray), 10, ReaderWriter@GorillaPaintbrawlManager__PaintbrawlStatus.GetInstance());
		}
	}

	// Token: 0x04001EC2 RID: 7874
	[FieldOffset(0)]
	public GorillaPaintbrawlManager.PaintbrawlState currentPaintbrawlState;

	// Token: 0x04001EC3 RID: 7875
	[FixedBufferProperty(typeof(NetworkArray<int>), typeof(UnityArraySurrogate@ReaderWriter@System_Int32), 10, order = -2147483647)]
	[WeaverGenerated]
	[SerializeField]
	[FieldOffset(4)]
	private FixedStorage@10 _playerLivesArray;

	// Token: 0x04001EC4 RID: 7876
	[FixedBufferProperty(typeof(NetworkArray<int>), typeof(UnityArraySurrogate@ReaderWriter@System_Int32), 10, order = -2147483647)]
	[WeaverGenerated]
	[SerializeField]
	[FieldOffset(44)]
	private FixedStorage@10 _playerActorNumberArray;

	// Token: 0x04001EC5 RID: 7877
	[FixedBufferProperty(typeof(NetworkArray<GorillaPaintbrawlManager.PaintbrawlStatus>), typeof(UnityArraySurrogate@ReaderWriter@GorillaPaintbrawlManager__PaintbrawlStatus), 10, order = -2147483647)]
	[WeaverGenerated]
	[SerializeField]
	[FieldOffset(84)]
	private FixedStorage@10 _playerStatusArray;
}
