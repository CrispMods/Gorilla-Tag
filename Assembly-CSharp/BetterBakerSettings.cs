using System;
using UnityEngine;

// Token: 0x0200084F RID: 2127
public class BetterBakerSettings : MonoBehaviour
{
	// Token: 0x04003756 RID: 14166
	[SerializeField]
	public GameObject[] lightMapMaps = new GameObject[9];

	// Token: 0x02000850 RID: 2128
	[Serializable]
	public struct LightMapMap
	{
		// Token: 0x04003757 RID: 14167
		[SerializeField]
		public string timeOfDayName;

		// Token: 0x04003758 RID: 14168
		[SerializeField]
		public GameObject sceneLightObject;
	}
}
