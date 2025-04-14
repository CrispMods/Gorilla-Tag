using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000833 RID: 2099
public class BetterBakerPositionOverrides : MonoBehaviour
{
	// Token: 0x04003696 RID: 13974
	public List<BetterBakerPositionOverrides.OverridePosition> overridePositions;

	// Token: 0x02000834 RID: 2100
	[Serializable]
	public struct OverridePosition
	{
		// Token: 0x04003697 RID: 13975
		public GameObject go;

		// Token: 0x04003698 RID: 13976
		public Transform bakingTransform;

		// Token: 0x04003699 RID: 13977
		public Transform gameTransform;
	}
}
