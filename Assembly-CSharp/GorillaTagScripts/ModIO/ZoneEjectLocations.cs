using System;
using UnityEngine;

namespace GorillaTagScripts.ModIO
{
	// Token: 0x02000A0C RID: 2572
	[Serializable]
	public class ZoneEjectLocations
	{
		// Token: 0x04004131 RID: 16689
		public GTZone ejectZone = GTZone.none;

		// Token: 0x04004132 RID: 16690
		public GameObject[] ejectLocations;
	}
}
