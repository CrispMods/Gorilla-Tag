using System;
using UnityEngine;

namespace GorillaNetworking
{
	// Token: 0x02000ABC RID: 2748
	public class GorillaNetworkLobbyJoinTrigger : GorillaTriggerBox
	{
		// Token: 0x04004682 RID: 18050
		public GameObject[] makeSureThisIsDisabled;

		// Token: 0x04004683 RID: 18051
		public GameObject[] makeSureThisIsEnabled;

		// Token: 0x04004684 RID: 18052
		public string gameModeName;

		// Token: 0x04004685 RID: 18053
		public PhotonNetworkController photonNetworkController;

		// Token: 0x04004686 RID: 18054
		public string componentTypeToRemove;

		// Token: 0x04004687 RID: 18055
		public GameObject componentRemoveTarget;

		// Token: 0x04004688 RID: 18056
		public string componentTypeToAdd;

		// Token: 0x04004689 RID: 18057
		public GameObject componentAddTarget;

		// Token: 0x0400468A RID: 18058
		public GameObject gorillaParent;

		// Token: 0x0400468B RID: 18059
		public GameObject joinFailedBlock;
	}
}
