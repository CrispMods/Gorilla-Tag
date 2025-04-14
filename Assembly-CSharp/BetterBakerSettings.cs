using System;
using UnityEngine;

// Token: 0x02000835 RID: 2101
public class BetterBakerSettings : MonoBehaviour
{
	// Token: 0x0400369A RID: 13978
	[SerializeField]
	public GameObject[] lightMapMaps = new GameObject[9];

	// Token: 0x02000836 RID: 2102
	[Serializable]
	public struct LightMapMap
	{
		// Token: 0x0400369B RID: 13979
		[SerializeField]
		public string timeOfDayName;

		// Token: 0x0400369C RID: 13980
		[SerializeField]
		public GameObject sceneLightObject;
	}
}
