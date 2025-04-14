using System;

namespace GorillaNetworking
{
	// Token: 0x02000AA1 RID: 2721
	public class ModIONetworkJoinTrigger : GorillaNetworkJoinTrigger
	{
		// Token: 0x060043FD RID: 17405 RVA: 0x001422EC File Offset: 0x001404EC
		public override string GetFullDesiredGameModeString()
		{
			return this.networkZone + GorillaComputer.instance.currentQueue + ModIOMapLoader.LoadedMapModId.ToString() + base.GetDesiredGameType();
		}
	}
}
