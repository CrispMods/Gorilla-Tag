using System;
using UnityEngine;
using UnityEngine.Serialization;

// Token: 0x020008F1 RID: 2289
public class ZoneDef : MonoBehaviour
{
	// Token: 0x17000599 RID: 1433
	// (get) Token: 0x060036EA RID: 14058 RVA: 0x00104700 File Offset: 0x00102900
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

	// Token: 0x1700059A RID: 1434
	// (get) Token: 0x060036EB RID: 14059 RVA: 0x00104730 File Offset: 0x00102930
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

	// Token: 0x040039F6 RID: 14838
	public GTZone zoneId;

	// Token: 0x040039F7 RID: 14839
	[FormerlySerializedAs("subZoneType")]
	[FormerlySerializedAs("subZone")]
	public GTSubZone subZoneId;

	// Token: 0x040039F8 RID: 14840
	public GroupJoinZoneA groupZone;

	// Token: 0x040039F9 RID: 14841
	public GroupJoinZoneB groupZoneB;

	// Token: 0x040039FA RID: 14842
	public GroupJoinZoneA excludeGroupZone;

	// Token: 0x040039FB RID: 14843
	public GroupJoinZoneB excludeGroupZoneB;

	// Token: 0x040039FC RID: 14844
	[Space]
	public bool trackEnter = true;

	// Token: 0x040039FD RID: 14845
	public bool trackExit;

	// Token: 0x040039FE RID: 14846
	public bool trackStay = true;

	// Token: 0x040039FF RID: 14847
	public int priority = 1;

	// Token: 0x04003A00 RID: 14848
	[Space]
	public BoxCollider[] colliders = new BoxCollider[0];

	// Token: 0x04003A01 RID: 14849
	[Space]
	public ZoneNode[] nodes = new ZoneNode[0];

	// Token: 0x04003A02 RID: 14850
	[Space]
	public Bounds bounds;

	// Token: 0x04003A03 RID: 14851
	[Space]
	public ZoneDef[] zoneOverlaps = new ZoneDef[0];
}
