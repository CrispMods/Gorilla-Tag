using System;
using System.Runtime.InteropServices;
using Fusion;

// Token: 0x020000DF RID: 223
[NetworkStructWeaved(3)]
[StructLayout(LayoutKind.Explicit, Size = 12)]
public struct BeeSwarmData : INetworkStruct
{
	// Token: 0x1700006C RID: 108
	// (get) Token: 0x060005C3 RID: 1475 RVA: 0x00022058 File Offset: 0x00020258
	// (set) Token: 0x060005C4 RID: 1476 RVA: 0x00022060 File Offset: 0x00020260
	public int TargetActorNumber { readonly get; set; }

	// Token: 0x1700006D RID: 109
	// (get) Token: 0x060005C5 RID: 1477 RVA: 0x00022069 File Offset: 0x00020269
	// (set) Token: 0x060005C6 RID: 1478 RVA: 0x00022071 File Offset: 0x00020271
	public int CurrentState { readonly get; set; }

	// Token: 0x1700006E RID: 110
	// (get) Token: 0x060005C7 RID: 1479 RVA: 0x0002207A File Offset: 0x0002027A
	// (set) Token: 0x060005C8 RID: 1480 RVA: 0x00022082 File Offset: 0x00020282
	public float CurrentSpeed { readonly get; set; }

	// Token: 0x060005C9 RID: 1481 RVA: 0x0002208B File Offset: 0x0002028B
	public BeeSwarmData(int actorNr, int state, float speed)
	{
		this.TargetActorNumber = actorNr;
		this.CurrentState = state;
		this.CurrentSpeed = speed;
	}
}
