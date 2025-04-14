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
	// (get) Token: 0x06001706 RID: 5894 RVA: 0x00070AF5 File Offset: 0x0006ECF5
	// (set) Token: 0x06001707 RID: 5895 RVA: 0x00070AFD File Offset: 0x0006ECFD
	public long Header { readonly get; set; }

	// Token: 0x17000292 RID: 658
	// (get) Token: 0x06001708 RID: 5896 RVA: 0x00070B08 File Offset: 0x0006ED08
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
	// (get) Token: 0x06001709 RID: 5897 RVA: 0x00070B2B File Offset: 0x0006ED2B
	// (set) Token: 0x0600170A RID: 5898 RVA: 0x00070B33 File Offset: 0x0006ED33
	public int WearablesPackedState { readonly get; set; }

	// Token: 0x17000294 RID: 660
	// (get) Token: 0x0600170B RID: 5899 RVA: 0x00070B3C File Offset: 0x0006ED3C
	// (set) Token: 0x0600170C RID: 5900 RVA: 0x00070B44 File Offset: 0x0006ED44
	public int LThrowableProjectileIndex { readonly get; set; }

	// Token: 0x17000295 RID: 661
	// (get) Token: 0x0600170D RID: 5901 RVA: 0x00070B4D File Offset: 0x0006ED4D
	// (set) Token: 0x0600170E RID: 5902 RVA: 0x00070B55 File Offset: 0x0006ED55
	public int RThrowableProjectileIndex { readonly get; set; }

	// Token: 0x17000296 RID: 662
	// (get) Token: 0x0600170F RID: 5903 RVA: 0x00070B5E File Offset: 0x0006ED5E
	// (set) Token: 0x06001710 RID: 5904 RVA: 0x00070B66 File Offset: 0x0006ED66
	public int SizeLayerMask { readonly get; set; }

	// Token: 0x17000297 RID: 663
	// (get) Token: 0x06001711 RID: 5905 RVA: 0x00070B6F File Offset: 0x0006ED6F
	// (set) Token: 0x06001712 RID: 5906 RVA: 0x00070B77 File Offset: 0x0006ED77
	public int RandomThrowableIndex { readonly get; set; }

	// Token: 0x17000298 RID: 664
	// (get) Token: 0x06001713 RID: 5907 RVA: 0x00070B80 File Offset: 0x0006ED80
	// (set) Token: 0x06001714 RID: 5908 RVA: 0x00070B88 File Offset: 0x0006ED88
	public long PackedBeads { readonly get; set; }

	// Token: 0x17000299 RID: 665
	// (get) Token: 0x06001715 RID: 5909 RVA: 0x00070B91 File Offset: 0x0006ED91
	// (set) Token: 0x06001716 RID: 5910 RVA: 0x00070B99 File Offset: 0x0006ED99
	public long PackedBeadsMoreThan6 { readonly get; set; }

	// Token: 0x040019B9 RID: 6585
	[FixedBufferProperty(typeof(NetworkArray<long>), typeof(UnityArraySurrogate@ReaderWriter@System_Int64), 5, order = -2147483647)]
	[WeaverGenerated]
	[SerializeField]
	[FieldOffset(44)]
	private FixedStorage@10 _TransferrableStates;
}
