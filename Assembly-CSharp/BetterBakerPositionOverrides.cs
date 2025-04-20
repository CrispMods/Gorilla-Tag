using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200084D RID: 2125
public class BetterBakerPositionOverrides : MonoBehaviour
{
	// Token: 0x04003752 RID: 14162
	public List<BetterBakerPositionOverrides.OverridePosition> overridePositions;

	// Token: 0x0200084E RID: 2126
	[Serializable]
	public struct OverridePosition
	{
		// Token: 0x04003753 RID: 14163
		public GameObject go;

		// Token: 0x04003754 RID: 14164
		public Transform bakingTransform;

		// Token: 0x04003755 RID: 14165
		public Transform gameTransform;
	}
}
