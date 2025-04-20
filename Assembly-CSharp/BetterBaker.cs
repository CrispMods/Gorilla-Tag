using System;
using UnityEngine;

// Token: 0x0200084A RID: 2122
public class BetterBaker : MonoBehaviour
{
	// Token: 0x0400374A RID: 14154
	public string bakeryLightmapDirectory;

	// Token: 0x0400374B RID: 14155
	public string dayNightLightmapsDirectory;

	// Token: 0x0400374C RID: 14156
	public GameObject[] allLights;

	// Token: 0x0200084B RID: 2123
	public struct LightMapMap
	{
		// Token: 0x0400374D RID: 14157
		public string timeOfDayName;

		// Token: 0x0400374E RID: 14158
		public GameObject lightObject;
	}
}
