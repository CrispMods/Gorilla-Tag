using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000836 RID: 2102
public class BetterBakerPositionOverrides : MonoBehaviour
{
	// Token: 0x040036A8 RID: 13992
	public List<BetterBakerPositionOverrides.OverridePosition> overridePositions;

	// Token: 0x02000837 RID: 2103
	[Serializable]
	public struct OverridePosition
	{
		// Token: 0x040036A9 RID: 13993
		public GameObject go;

		// Token: 0x040036AA RID: 13994
		public Transform bakingTransform;

		// Token: 0x040036AB RID: 13995
		public Transform gameTransform;
	}
}
