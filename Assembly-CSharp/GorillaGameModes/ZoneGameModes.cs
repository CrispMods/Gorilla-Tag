using System;

namespace GorillaGameModes
{
	// Token: 0x02000990 RID: 2448
	[Serializable]
	public struct ZoneGameModes
	{
		// Token: 0x04003CB2 RID: 15538
		public GTZone[] zone;

		// Token: 0x04003CB3 RID: 15539
		public GameModeType[] modes;

		// Token: 0x04003CB4 RID: 15540
		public GameModeType[] privateModes;
	}
}
