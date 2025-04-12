using System;
using UnityEngine;

// Token: 0x02000043 RID: 67
public class CrittersCageSettings : CrittersActorSettings
{
	// Token: 0x06000151 RID: 337 RVA: 0x00030469 File Offset: 0x0002E669
	public override void UpdateActorSettings()
	{
		base.UpdateActorSettings();
		CrittersCage crittersCage = (CrittersCage)this.parentActor;
		crittersCage.cagePosition = this.cagePoint;
		crittersCage.grabPosition = this.grabPoint;
	}

	// Token: 0x0400019D RID: 413
	public Transform cagePoint;

	// Token: 0x0400019E RID: 414
	public Transform grabPoint;
}
