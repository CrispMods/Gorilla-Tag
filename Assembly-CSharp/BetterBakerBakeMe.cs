using System;
using System.Collections.Generic;
using GorillaTag.Rendering.Shaders;
using UnityEngine;

// Token: 0x02000832 RID: 2098
public class BetterBakerBakeMe : FlagForBaking
{
	// Token: 0x04003693 RID: 13971
	public GameObject[] stuffIncludingParentsToBake;

	// Token: 0x04003694 RID: 13972
	public GameObject getMatStuffFromHere;

	// Token: 0x04003695 RID: 13973
	public List<ShaderConfigData.ShaderConfig> allConfigs = new List<ShaderConfigData.ShaderConfig>();
}
