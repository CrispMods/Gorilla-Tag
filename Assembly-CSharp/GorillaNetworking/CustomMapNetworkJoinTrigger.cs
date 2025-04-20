using System;

namespace GorillaNetworking
{
	// Token: 0x02000AE3 RID: 2787
	public class CustomMapNetworkJoinTrigger : GorillaNetworkJoinTrigger
	{
		// Token: 0x06004628 RID: 17960 RVA: 0x0018508C File Offset: 0x0018328C
		public override string GetFullDesiredGameModeString()
		{
			return this.networkZone + GorillaComputer.instance.currentQueue + CustomMapLoader.LoadedMapModId.ToString() + base.GetDesiredGameType();
		}
	}
}
