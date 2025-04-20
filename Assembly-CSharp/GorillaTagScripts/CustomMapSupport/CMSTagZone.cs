using System;
using GorillaGameModes;

namespace GorillaTagScripts.CustomMapSupport
{
	// Token: 0x02000A05 RID: 2565
	public class CMSTagZone : CMSTrigger
	{
		// Token: 0x06004020 RID: 16416 RVA: 0x00059EB7 File Offset: 0x000580B7
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
