using System;
using UnityEngine;

// Token: 0x02000830 RID: 2096
public class BetterBaker : MonoBehaviour
{
	// Token: 0x0400368E RID: 13966
	public string bakeryLightmapDirectory;

	// Token: 0x0400368F RID: 13967
	public string dayNightLightmapsDirectory;

	// Token: 0x04003690 RID: 13968
	public GameObject[] allLights;

	// Token: 0x02000831 RID: 2097
	public struct LightMapMap
	{
		// Token: 0x04003691 RID: 13969
		public string timeOfDayName;

		// Token: 0x04003692 RID: 13970
		public GameObject lightObject;
	}
}
