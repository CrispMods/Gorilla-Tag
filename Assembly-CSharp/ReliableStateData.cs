using System;
using System.Runtime.InteropServices;
using Fusion;
using Fusion.CodeGen;
using UnityEngine;

// Token: 0x020003C6 RID: 966
[NetworkStructWeaved(21)]
[Serializable]
[StructLayout(LayoutKind.Explicit, Size = 84)]
public struct ReliableStateData : INetworkStruct
{
	// Token: 0x17000298 RID: 664
	// (get) Token: 0x06001753 RID: 5971 RVA: 0x0003FD4C File Offset: 0x0003DF4C
	// (set) Token: 0x06001754 RID: 5972 RVA: 0x0003FD54 File Offset: 0x0003DF54
	public long Header { readonly get; set; }

	// Token: 0x17000299 RID: 665
	// (get) Token: 0x06001755 RID: 5973 RVA: 0x0003FD5D File Offset: 0x0003DF5D
	[Networked]
	[Capacity(5)]
	public NetworkArray<long> TransferrableStates
	{
		get
		{
			return new NetworkArray<long>(Native.ReferenceToPointer<FixedStorage@10>(ref this._TransferrableStates), 5, ReaderWriter@System_Int64.GetInstance());
		}
	}

	// Token: 0x1700029A RID: 666
	// (get) Token: 0x06001756 RID: 5974 RVA: 0x0003FD75 File Offset: 0x0003DF75
	// (set) Token: 0x06001757 RID: 5975 RVA: 0x0003FD7D File Offset: 0x0003DF7D
	public int WearablesPackedState { readonly get; set; }

	// Token: 0x1700029B RID: 667
	// (get) Token: 0x06001758 RID: 5976 RVA: 0x0003FD86 File Offset: 0x0003DF86
	// (set) Token: 0x06001759 RID: 5977 RVA: 0x0003FD8E File Offset: 0x0003DF8E
	public int LThrowableProjectileIndex { readonly get; set; }

	// Token: 0x1700029C RID: 668
	// (get) Token: 0x0600175A RID: 5978 RVA: 0x0003FD97 File Offset: 0x0003DF97
	// (set) Token: 0x0600175B RID: 5979 RVA: 0x0003FD9F File Offset: 0x0003DF9F
	public int RThrowableProjectileIndex { readonly get; set; }

	// Token: 0x1700029D RID: 669
	// (get) Token: 0x0600175C RID: 5980 RVA: 0x0003FDA8 File Offset: 0x0003DFA8
	// (set) Token: 0x0600175D RID: 5981 RVA: 0x0003FDB0 File Offset: 0x0003DFB0
	public int SizeLayerMask { readonly get; set; }

	// Token: 0x1700029E RID: 670
	// (get) Token: 0x0600175E RID: 5982 RVA: 0x0003FDB9 File Offset: 0x0003DFB9
	// (set) Token: 0x0600175F RID: 5983 RVA: 0x0003FDC1 File Offset: 0x0003DFC1
	public int RandomThrowableIndex { readonly get; set; }

	// Token: 0x1700029F RID: 671
	// (get) Token: 0x06001760 RID: 5984 RVA: 0x0003FDCA File Offset: 0x0003DFCA
	// (set) Token: 0x06001761 RID: 5985 RVA: 0x0003FDD2 File Offset: 0x0003DFD2
	public long PackedBeads { readonly get; set; }

	// Token: 0x170002A0 RID: 672
	// (get) Token: 0x06001762 RID: 5986 RVA: 0x0003FDDB File Offset: 0x0003DFDB
	// (set) Token: 0x06001763 RID: 5987 RVA: 0x0003FDE3 File Offset: 0x0003DFE3
	public long PackedBeadsMoreThan6 { readonly get; set; }

	// Token: 0x04001A02 RID: 6658
	[FixedBufferProperty(typeof(NetworkArray<long>), typeof(UnityArraySurrogate@ReaderWriter@System_Int64), 5, order = -2147483647)]
	[WeaverGenerated]
	[SerializeField]
	[FieldOffset(44)]
	private FixedStorage@10 _TransferrableStates;
}
