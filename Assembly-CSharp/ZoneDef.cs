using System;
using UnityEngine;
using UnityEngine.Serialization;

// Token: 0x0200090A RID: 2314
public class ZoneDef : MonoBehaviour
{
	// Token: 0x170005A9 RID: 1449
	// (get) Token: 0x060037A6 RID: 14246 RVA: 0x00149218 File Offset: 0x00147418
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

	// Token: 0x170005AA RID: 1450
	// (get) Token: 0x060037A7 RID: 14247 RVA: 0x00149248 File Offset: 0x00147448
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

	// Token: 0x04003AA5 RID: 15013
	public GTZone zoneId;

	// Token: 0x04003AA6 RID: 15014
	[FormerlySerializedAs("subZoneType")]
	[FormerlySerializedAs("subZone")]
	public GTSubZone subZoneId;

	// Token: 0x04003AA7 RID: 15015
	public GroupJoinZoneA groupZone;

	// Token: 0x04003AA8 RID: 15016
	public GroupJoinZoneB groupZoneB;

	// Token: 0x04003AA9 RID: 15017
	public GroupJoinZoneA excludeGroupZone;

	// Token: 0x04003AAA RID: 15018
	public GroupJoinZoneB excludeGroupZoneB;

	// Token: 0x04003AAB RID: 15019
	[Space]
	public bool trackEnter = true;

	// Token: 0x04003AAC RID: 15020
	public bool trackExit;

	// Token: 0x04003AAD RID: 15021
	public bool trackStay = true;

	// Token: 0x04003AAE RID: 15022
	public int priority = 1;

	// Token: 0x04003AAF RID: 15023
	[Space]
	public BoxCollider[] colliders = new BoxCollider[0];

	// Token: 0x04003AB0 RID: 15024
	[Space]
	public ZoneNode[] nodes = new ZoneNode[0];

	// Token: 0x04003AB1 RID: 15025
	[Space]
	public Bounds bounds;

	// Token: 0x04003AB2 RID: 15026
	[Space]
	public ZoneDef[] zoneOverlaps = new ZoneDef[0];
}
