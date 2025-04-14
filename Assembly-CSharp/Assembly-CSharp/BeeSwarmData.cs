using System;
using System.Runtime.InteropServices;
using Fusion;

// Token: 0x020000DF RID: 223
[NetworkStructWeaved(3)]
[StructLayout(LayoutKind.Explicit, Size = 12)]
public struct BeeSwarmData : INetworkStruct
{
	// Token: 0x1700006C RID: 108
	// (get) Token: 0x060005C5 RID: 1477 RVA: 0x0002237C File Offset: 0x0002057C
	// (set) Token: 0x060005C6 RID: 1478 RVA: 0x00022384 File Offset: 0x00020584
	public int TargetActorNumber { readonly get; set; }

	// Token: 0x1700006D RID: 109
	// (get) Token: 0x060005C7 RID: 1479 RVA: 0x0002238D File Offset: 0x0002058D
	// (set) Token: 0x060005C8 RID: 1480 RVA: 0x00022395 File Offset: 0x00020595
	public int CurrentState { readonly get; set; }

	// Token: 0x1700006E RID: 110
	// (get) Token: 0x060005C9 RID: 1481 RVA: 0x0002239E File Offset: 0x0002059E
	// (set) Token: 0x060005CA RID: 1482 RVA: 0x000223A6 File Offset: 0x000205A6
	public float CurrentSpeed { readonly get; set; }

	// Token: 0x060005CB RID: 1483 RVA: 0x000223AF File Offset: 0x000205AF
	public BeeSwarmData(int actorNr, int state, float speed)
	{
		this.TargetActorNumber = actorNr;
		this.CurrentState = state;
		this.CurrentSpeed = speed;
	}
}
