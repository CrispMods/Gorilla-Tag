using System;
using UnityEngine;

// Token: 0x02000833 RID: 2099
public class BetterBaker : MonoBehaviour
{
	// Token: 0x040036A0 RID: 13984
	public string bakeryLightmapDirectory;

	// Token: 0x040036A1 RID: 13985
	public string dayNightLightmapsDirectory;

	// Token: 0x040036A2 RID: 13986
	public GameObject[] allLights;

	// Token: 0x02000834 RID: 2100
	public struct LightMapMap
	{
		// Token: 0x040036A3 RID: 13987
		public string timeOfDayName;

		// Token: 0x040036A4 RID: 13988
		public GameObject lightObject;
	}
}
