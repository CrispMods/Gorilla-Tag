using System;
using UnityEngine;

// Token: 0x0200021A RID: 538
public class GorillaSetZoneTrigger : GorillaTriggerBox
{
	// Token: 0x06000C92 RID: 3218 RVA: 0x00038D0A File Offset: 0x00036F0A
	public override void OnBoxTriggered()
	{
		ZoneManagement.SetActiveZones(this.zones);
	}

	// Token: 0x04000FD2 RID: 4050
	[SerializeField]
	private GTZone[] zones;
}
