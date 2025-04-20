using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

// Token: 0x0200012F RID: 303
[JsonConverter(typeof(StringEnumConverter))]
[Serializable]
public enum QuestType
{
	// Token: 0x04000972 RID: 2418
	none,
	// Token: 0x04000973 RID: 2419
	gameModeObjective,
	// Token: 0x04000974 RID: 2420
	gameModeRound,
	// Token: 0x04000975 RID: 2421
	grabObject,
	// Token: 0x04000976 RID: 2422
	dropObject,
	// Token: 0x04000977 RID: 2423
	eatObject,
	// Token: 0x04000978 RID: 2424
	tapObject,
	// Token: 0x04000979 RID: 2425
	launchedProjectile,
	// Token: 0x0400097A RID: 2426
	moveDistance,
	// Token: 0x0400097B RID: 2427
	swimDistance,
	// Token: 0x0400097C RID: 2428
	triggerHandEffect,
	// Token: 0x0400097D RID: 2429
	enterLocation,
	// Token: 0x0400097E RID: 2430
	misc,
	// Token: 0x0400097F RID: 2431
	critter
}
