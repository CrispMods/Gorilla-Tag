using System;
using UnityEngine;
using UnityEngine.Serialization;

// Token: 0x020008EE RID: 2286
public class ZoneDef : MonoBehaviour
{
	// Token: 0x17000598 RID: 1432
	// (get) Token: 0x060036DE RID: 14046 RVA: 0x00104138 File Offset: 0x00102338
	public GroupJoinZoneAB groupZoneAB
	{
		get
		{
			return new GroupJoinZoneAB
			{
				a = this.groupZone,
				b = this.groupZoneB
			};
		}
	}

	// Token: 0x17000599 RID: 1433
	// (get) Token: 0x060036DF RID: 14047 RVA: 0x00104168 File Offset: 0x00102368
	public GroupJoinZoneAB excludeGroupZoneAB
	{
		get
		{
			return new GroupJoinZoneAB
			{
				a = this.excludeGroupZone,
				b = this.excludeGroupZoneB
			};
		}
	}

	// Token: 0x040039E4 RID: 14820
	public GTZone zoneId;

	// Token: 0x040039E5 RID: 14821
	[FormerlySerializedAs("subZoneType")]
	[FormerlySerializedAs("subZone")]
	public GTSubZone subZoneId;

	// Token: 0x040039E6 RID: 14822
	public GroupJoinZoneA groupZone;

	// Token: 0x040039E7 RID: 14823
	public GroupJoinZoneB groupZoneB;

	// Token: 0x040039E8 RID: 14824
	public GroupJoinZoneA excludeGroupZone;

	// Token: 0x040039E9 RID: 14825
	public GroupJoinZoneB excludeGroupZoneB;

	// Token: 0x040039EA RID: 14826
	[Space]
	public bool trackEnter = true;

	// Token: 0x040039EB RID: 14827
	public bool trackExit;

	// Token: 0x040039EC RID: 14828
	public bool trackStay = true;

	// Token: 0x040039ED RID: 14829
	public int priority = 1;

	// Token: 0x040039EE RID: 14830
	[Space]
	public BoxCollider[] colliders = new BoxCollider[0];

	// Token: 0x040039EF RID: 14831
	[Space]
	public ZoneNode[] nodes = new ZoneNode[0];

	// Token: 0x040039F0 RID: 14832
	[Space]
	public Bounds bounds;

	// Token: 0x040039F1 RID: 14833
	[Space]
	public ZoneDef[] zoneOverlaps = new ZoneDef[0];
}
