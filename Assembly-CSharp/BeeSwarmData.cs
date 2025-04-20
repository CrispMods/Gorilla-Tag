using System;
using System.Runtime.InteropServices;
using Fusion;

// Token: 0x020000E9 RID: 233
[NetworkStructWeaved(3)]
[StructLayout(LayoutKind.Explicit, Size = 12)]
public struct BeeSwarmData : INetworkStruct
{
	// Token: 0x17000071 RID: 113
	// (get) Token: 0x06000604 RID: 1540 RVA: 0x00034806 File Offset: 0x00032A06
	// (set) Token: 0x06000605 RID: 1541 RVA: 0x0003480E File Offset: 0x00032A0E
	public int TargetActorNumber { readonly get; set; }

	// Token: 0x17000072 RID: 114
	// (get) Token: 0x06000606 RID: 1542 RVA: 0x00034817 File Offset: 0x00032A17
	// (set) Token: 0x06000607 RID: 1543 RVA: 0x0003481F File Offset: 0x00032A1F
	public int CurrentState { readonly get; set; }

	// Token: 0x17000073 RID: 115
	// (get) Token: 0x06000608 RID: 1544 RVA: 0x00034828 File Offset: 0x00032A28
	// (set) Token: 0x06000609 RID: 1545 RVA: 0x00034830 File Offset: 0x00032A30
	public float CurrentSpeed { readonly get; set; }

	// Token: 0x0600060A RID: 1546 RVA: 0x00034839 File Offset: 0x00032A39
	public BeeSwarmData(int actorNr, int state, float speed)
	{
		this.TargetActorNumber = actorNr;
		this.CurrentState = state;
		this.CurrentSpeed = speed;
	}
}
