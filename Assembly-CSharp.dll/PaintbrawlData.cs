using System;
using System.Runtime.InteropServices;
using Fusion;
using Fusion.CodeGen;
using UnityEngine;

// Token: 0x0200045E RID: 1118
[NetworkStructWeaved(31)]
[StructLayout(LayoutKind.Explicit, Size = 124)]
public struct PaintbrawlData : INetworkStruct
{
	// Token: 0x170002FA RID: 762
	// (get) Token: 0x06001B6F RID: 7023 RVA: 0x00041C9B File Offset: 0x0003FE9B
	[Networked]
	[Capacity(10)]
	public NetworkArray<int> playerLivesArray
	{
		get
		{
			return new NetworkArray<int>(Native.ReferenceToPointer<FixedStorage@10>(ref this._playerLivesArray), 10, ReaderWriter@System_Int32.GetInstance());
		}
	}

	// Token: 0x170002FB RID: 763
	// (get) Token: 0x06001B70 RID: 7024 RVA: 0x00041CB7 File Offset: 0x0003FEB7
	[Networked]
	[Capacity(10)]
	public NetworkArray<int> playerActorNumberArray
	{
		get
		{
			return new NetworkArray<int>(Native.ReferenceToPointer<FixedStorage@10>(ref this._playerActorNumberArray), 10, ReaderWriter@System_Int32.GetInstance());
		}
	}

	// Token: 0x170002FC RID: 764
	// (get) Token: 0x06001B71 RID: 7025 RVA: 0x00041CD3 File Offset: 0x0003FED3
	[Networked]
	[Capacity(10)]
	public NetworkArray<GorillaPaintbrawlManager.PaintbrawlStatus> playerStatusArray
	{
		get
		{
			return new NetworkArray<GorillaPaintbrawlManager.PaintbrawlStatus>(Native.ReferenceToPointer<FixedStorage@10>(ref this._playerStatusArray), 10, ReaderWriter@GorillaPaintbrawlManager__PaintbrawlStatus.GetInstance());
		}
	}

	// Token: 0x04001E74 RID: 7796
	[FieldOffset(0)]
	public GorillaPaintbrawlManager.PaintbrawlState currentPaintbrawlState;

	// Token: 0x04001E75 RID: 7797
	[FixedBufferProperty(typeof(NetworkArray<int>), typeof(UnityArraySurrogate@ReaderWriter@System_Int32), 10, order = -2147483647)]
	[WeaverGenerated]
	[SerializeField]
	[FieldOffset(4)]
	private FixedStorage@10 _playerLivesArray;

	// Token: 0x04001E76 RID: 7798
	[FixedBufferProperty(typeof(NetworkArray<int>), typeof(UnityArraySurrogate@ReaderWriter@System_Int32), 10, order = -2147483647)]
	[WeaverGenerated]
	[SerializeField]
	[FieldOffset(44)]
	private FixedStorage@10 _playerActorNumberArray;

	// Token: 0x04001E77 RID: 7799
	[FixedBufferProperty(typeof(NetworkArray<GorillaPaintbrawlManager.PaintbrawlStatus>), typeof(UnityArraySurrogate@ReaderWriter@GorillaPaintbrawlManager__PaintbrawlStatus), 10, order = -2147483647)]
	[WeaverGenerated]
	[SerializeField]
	[FieldOffset(84)]
	private FixedStorage@10 _playerStatusArray;
}
