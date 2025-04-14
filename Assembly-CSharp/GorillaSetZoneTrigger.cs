using System;
using UnityEngine;

// Token: 0x0200020F RID: 527
public class GorillaSetZoneTrigger : GorillaTriggerBox
{
	// Token: 0x06000C47 RID: 3143 RVA: 0x000418CB File Offset: 0x0003FACB
	public override void OnBoxTriggered()
	{
		ZoneManagement.SetActiveZones(this.zones);
	}

	// Token: 0x04000F8C RID: 3980
	[SerializeField]
	private GTZone[] zones;
}
