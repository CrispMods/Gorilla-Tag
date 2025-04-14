using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

// Token: 0x02000125 RID: 293
[JsonConverter(typeof(StringEnumConverter))]
[Serializable]
public enum QuestType
{
	// Token: 0x04000930 RID: 2352
	none,
	// Token: 0x04000931 RID: 2353
	gameModeObjective,
	// Token: 0x04000932 RID: 2354
	gameModeRound,
	// Token: 0x04000933 RID: 2355
	grabObject,
	// Token: 0x04000934 RID: 2356
	dropObject,
	// Token: 0x04000935 RID: 2357
	eatObject,
	// Token: 0x04000936 RID: 2358
	tapObject,
	// Token: 0x04000937 RID: 2359
	launchedProjectile,
	// Token: 0x04000938 RID: 2360
	moveDistance,
	// Token: 0x04000939 RID: 2361
	swimDistance,
	// Token: 0x0400093A RID: 2362
	triggerHandEffect,
	// Token: 0x0400093B RID: 2363
	enterLocation,
	// Token: 0x0400093C RID: 2364
	misc
}
