using System;

namespace GorillaNetworking
{
	// Token: 0x02000AA4 RID: 2724
	public class ModIONetworkJoinTrigger : GorillaNetworkJoinTrigger
	{
		// Token: 0x06004409 RID: 17417 RVA: 0x00179914 File Offset: 0x00177B14
		public override string GetFullDesiredGameModeString()
		{
			return this.networkZone + GorillaComputer.instance.currentQueue + ModIOMapLoader.LoadedMapModId.ToString() + base.GetDesiredGameType();
		}
	}
}
