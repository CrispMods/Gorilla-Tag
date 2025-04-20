using System;
using UnityEngine;

// Token: 0x02000048 RID: 72
public class CrittersCageSettings : CrittersActorSettings
{
	// Token: 0x06000169 RID: 361 RVA: 0x0003145E File Offset: 0x0002F65E
	public override void UpdateActorSettings()
	{
		base.UpdateActorSettings();
		CrittersCage crittersCage = (CrittersCage)this.parentActor;
		crittersCage.cagePosition = this.cagePoint;
		crittersCage.grabPosition = this.grabPoint;
	}

	// Token: 0x040001C2 RID: 450
	public Transform cagePoint;

	// Token: 0x040001C3 RID: 451
	public Transform grabPoint;
}
