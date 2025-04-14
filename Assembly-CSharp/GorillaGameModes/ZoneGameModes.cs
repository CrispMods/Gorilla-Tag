using System;

namespace GorillaGameModes
{
	// Token: 0x0200096A RID: 2410
	[Serializable]
	public struct ZoneGameModes
	{
		// Token: 0x04003BD8 RID: 15320
		public GTZone[] zone;

		// Token: 0x04003BD9 RID: 15321
		public GameModeType[] modes;

		// Token: 0x04003BDA RID: 15322
		public GameModeType[] privateModes;
	}
}
