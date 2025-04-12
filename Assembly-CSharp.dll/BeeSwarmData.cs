using System;
using System.Runtime.InteropServices;
using Fusion;

// Token: 0x020000DF RID: 223
[NetworkStructWeaved(3)]
[StructLayout(LayoutKind.Explicit, Size = 12)]
public struct BeeSwarmData : INetworkStruct
{
	// Token: 0x1700006C RID: 108
	// (get) Token: 0x060005C5 RID: 1477 RVA: 0x000335A2 File Offset: 0x000317A2
	// (set) Token: 0x060005C6 RID: 1478 RVA: 0x000335AA File Offset: 0x000317AA
	public int TargetActorNumber { readonly get; set; }

	// Token: 0x1700006D RID: 109
	// (get) Token: 0x060005C7 RID: 1479 RVA: 0x000335B3 File Offset: 0x000317B3
	// (set) Token: 0x060005C8 RID: 1480 RVA: 0x000335BB File Offset: 0x000317BB
	public int CurrentState { readonly get; set; }

	// Token: 0x1700006E RID: 110
	// (get) Token: 0x060005C9 RID: 1481 RVA: 0x000335C4 File Offset: 0x000317C4
	// (set) Token: 0x060005CA RID: 1482 RVA: 0x000335CC File Offset: 0x000317CC
	public float CurrentSpeed { readonly get; set; }

	// Token: 0x060005CB RID: 1483 RVA: 0x000335D5 File Offset: 0x000317D5
	public BeeSwarmData(int actorNr, int state, float speed)
	{
		this.TargetActorNumber = actorNr;
		this.CurrentState = state;
		this.CurrentSpeed = speed;
	}
}
