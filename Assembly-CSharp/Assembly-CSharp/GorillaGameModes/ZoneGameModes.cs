using System;

namespace GorillaGameModes
{
	// Token: 0x0200096D RID: 2413
	[Serializable]
	public struct ZoneGameModes
	{
		// Token: 0x04003BEA RID: 15338
		public GTZone[] zone;

		// Token: 0x04003BEB RID: 15339
		public GameModeType[] modes;

		// Token: 0x04003BEC RID: 15340
		public GameModeType[] privateModes;
	}
}
