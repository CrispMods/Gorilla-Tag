using System;
using UnityEngine;

// Token: 0x02000838 RID: 2104
public class BetterBakerSettings : MonoBehaviour
{
	// Token: 0x040036AC RID: 13996
	[SerializeField]
	public GameObject[] lightMapMaps = new GameObject[9];

	// Token: 0x02000839 RID: 2105
	[Serializable]
	public struct LightMapMap
	{
		// Token: 0x040036AD RID: 13997
		[SerializeField]
		public string timeOfDayName;

		// Token: 0x040036AE RID: 13998
		[SerializeField]
		public GameObject sceneLightObject;
	}
}
