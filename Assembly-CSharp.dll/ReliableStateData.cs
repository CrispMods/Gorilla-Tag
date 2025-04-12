using System;
using System.Runtime.InteropServices;
using Fusion;
using Fusion.CodeGen;
using UnityEngine;

// Token: 0x020003BB RID: 955
[NetworkStructWeaved(21)]
[Serializable]
[StructLayout(LayoutKind.Explicit, Size = 84)]
public struct ReliableStateData : INetworkStruct
{
	// Token: 0x17000291 RID: 657
	// (get) Token: 0x06001709 RID: 5897 RVA: 0x0003EA62 File Offset: 0x0003CC62
	// (set) Token: 0x0600170A RID: 5898 RVA: 0x0003EA6A File Offset: 0x0003CC6A
	public long Header { readonly get; set; }

	// Token: 0x17000292 RID: 658
	// (get) Token: 0x0600170B RID: 5899 RVA: 0x0003EA73 File Offset: 0x0003CC73
	[Networked]
	[Capacity(5)]
	public NetworkArray<long> TransferrableStates
	{
		get
		{
			return new NetworkArray<long>(Native.ReferenceToPointer<FixedStorage@10>(ref this._TransferrableStates), 5, ReaderWriter@System_Int64.GetInstance());
		}
	}

	// Token: 0x17000293 RID: 659
	// (get) Token: 0x0600170C RID: 5900 RVA: 0x0003EA8B File Offset: 0x0003CC8B
	// (set) Token: 0x0600170D RID: 5901 RVA: 0x0003EA93 File Offset: 0x0003CC93
	public int WearablesPackedState { readonly get; set; }

	// Token: 0x17000294 RID: 660
	// (get) Token: 0x0600170E RID: 5902 RVA: 0x0003EA9C File Offset: 0x0003CC9C
	// (set) Token: 0x0600170F RID: 5903 RVA: 0x0003EAA4 File Offset: 0x0003CCA4
	public int LThrowableProjectileIndex { readonly get; set; }

	// Token: 0x17000295 RID: 661
	// (get) Token: 0x06001710 RID: 5904 RVA: 0x0003EAAD File Offset: 0x0003CCAD
	// (set) Token: 0x06001711 RID: 5905 RVA: 0x0003EAB5 File Offset: 0x0003CCB5
	public int RThrowableProjectileIndex { readonly get; set; }

	// Token: 0x17000296 RID: 662
	// (get) Token: 0x06001712 RID: 5906 RVA: 0x0003EABE File Offset: 0x0003CCBE
	// (set) Token: 0x06001713 RID: 5907 RVA: 0x0003EAC6 File Offset: 0x0003CCC6
	public int SizeLayerMask { readonly get; set; }

	// Token: 0x17000297 RID: 663
	// (get) Token: 0x06001714 RID: 5908 RVA: 0x0003EACF File Offset: 0x0003CCCF
	// (set) Token: 0x06001715 RID: 5909 RVA: 0x0003EAD7 File Offset: 0x0003CCD7
	public int RandomThrowableIndex { readonly get; set; }

	// Token: 0x17000298 RID: 664
	// (get) Token: 0x06001716 RID: 5910 RVA: 0x0003EAE0 File Offset: 0x0003CCE0
	// (set) Token: 0x06001717 RID: 5911 RVA: 0x0003EAE8 File Offset: 0x0003CCE8
	public long PackedBeads { readonly get; set; }

	// Token: 0x17000299 RID: 665
	// (get) Token: 0x06001718 RID: 5912 RVA: 0x0003EAF1 File Offset: 0x0003CCF1
	// (set) Token: 0x06001719 RID: 5913 RVA: 0x0003EAF9 File Offset: 0x0003CCF9
	public long PackedBeadsMoreThan6 { readonly get; set; }

	// Token: 0x040019BA RID: 6586
	[FixedBufferProperty(typeof(NetworkArray<long>), typeof(UnityArraySurrogate@ReaderWriter@System_Int64), 5, order = -2147483647)]
	[WeaverGenerated]
	[SerializeField]
	[FieldOffset(44)]
	private FixedStorage@10 _TransferrableStates;
}
