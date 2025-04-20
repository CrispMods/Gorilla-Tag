using System;
using UnityEngine;

namespace GorillaNetworking
{
	// Token: 0x02000AE6 RID: 2790
	public class GorillaNetworkLobbyJoinTrigger : GorillaTriggerBox
	{
		// Token: 0x04004767 RID: 18279
		public GameObject[] makeSureThisIsDisabled;

		// Token: 0x04004768 RID: 18280
		public GameObject[] makeSureThisIsEnabled;

		// Token: 0x04004769 RID: 18281
		public string gameModeName;

		// Token: 0x0400476A RID: 18282
		public PhotonNetworkController photonNetworkController;

		// Token: 0x0400476B RID: 18283
		public string componentTypeToRemove;

		// Token: 0x0400476C RID: 18284
		public GameObject componentRemoveTarget;

		// Token: 0x0400476D RID: 18285
		public string componentTypeToAdd;

		// Token: 0x0400476E RID: 18286
		public GameObject componentAddTarget;

		// Token: 0x0400476F RID: 18287
		public GameObject gorillaParent;

		// Token: 0x04004770 RID: 18288
		public GameObject joinFailedBlock;
	}
}
