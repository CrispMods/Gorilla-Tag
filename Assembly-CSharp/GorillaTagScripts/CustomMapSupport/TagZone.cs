using System;
using GorillaGameModes;

namespace GorillaTagScripts.CustomMapSupport
{
	// Token: 0x020009F0 RID: 2544
	public class TagZone : CustomMapTrigger
	{
		// Token: 0x06003F90 RID: 16272 RVA: 0x0012D59D File Offset: 0x0012B79D
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
