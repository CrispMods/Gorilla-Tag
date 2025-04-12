using System;
using GorillaGameModes;

namespace GorillaTagScripts.CustomMapSupport
{
	// Token: 0x020009F3 RID: 2547
	public class TagZone : CustomMapTrigger
	{
		// Token: 0x06003F9C RID: 16284 RVA: 0x00058C1A File Offset: 0x00056E1A
		public override void Trigger(double triggerTime = -1.0, bool originatedLocally = false, bool ignoreTriggerCount = false)
		{
			base.Trigger(triggerTime, originatedLocally, ignoreTriggerCount);
			if (originatedLocally)
			{
				GameMode.ReportHit();
			}
		}
	}
}
