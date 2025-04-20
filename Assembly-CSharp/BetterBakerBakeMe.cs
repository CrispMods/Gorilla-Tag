using System;
using System.Collections.Generic;
using GorillaTag.Rendering.Shaders;
using UnityEngine;

// Token: 0x0200084C RID: 2124
public class BetterBakerBakeMe : FlagForBaking
{
	// Token: 0x0400374F RID: 14159
	public GameObject[] stuffIncludingParentsToBake;

	// Token: 0x04003750 RID: 14160
	public GameObject getMatStuffFromHere;

	// Token: 0x04003751 RID: 14161
	public List<ShaderConfigData.ShaderConfig> allConfigs = new List<ShaderConfigData.ShaderConfig>();
}
