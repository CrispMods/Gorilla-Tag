﻿using System;
using UnityEngine;

// Token: 0x0200020F RID: 527
public class GorillaSetZoneTrigger : GorillaTriggerBox
{
	// Token: 0x06000C49 RID: 3145 RVA: 0x00037A4A File Offset: 0x00035C4A
	public override void OnBoxTriggered()
	{
		ZoneManagement.SetActiveZones(this.zones);
	}

	// Token: 0x04000F8D RID: 3981
	[SerializeField]
	private GTZone[] zones;
}
