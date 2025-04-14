using System;
using System.Collections.Generic;
using GorillaTag.Rendering.Shaders;
using UnityEngine;

// Token: 0x02000835 RID: 2101
public class BetterBakerBakeMe : FlagForBaking
{
	// Token: 0x040036A5 RID: 13989
	public GameObject[] stuffIncludingParentsToBake;

	// Token: 0x040036A6 RID: 13990
	public GameObject getMatStuffFromHere;

	// Token: 0x040036A7 RID: 13991
	public List<ShaderConfigData.ShaderConfig> allConfigs = new List<ShaderConfigData.ShaderConfig>();
}
