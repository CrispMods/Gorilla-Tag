using System;
using UnityEngine;

namespace GorillaNetworking
{
	// Token: 0x02000AB9 RID: 2745
	public class GorillaNetworkLobbyJoinTrigger : GorillaTriggerBox
	{
		// Token: 0x04004670 RID: 18032
		public GameObject[] makeSureThisIsDisabled;

		// Token: 0x04004671 RID: 18033
		public GameObject[] makeSureThisIsEnabled;

		// Token: 0x04004672 RID: 18034
		public string gameModeName;

		// Token: 0x04004673 RID: 18035
		public PhotonNetworkController photonNetworkController;

		// Token: 0x04004674 RID: 18036
		public string componentTypeToRemove;

		// Token: 0x04004675 RID: 18037
		public GameObject componentRemoveTarget;

		// Token: 0x04004676 RID: 18038
		public string componentTypeToAdd;

		// Token: 0x04004677 RID: 18039
		public GameObject componentAddTarget;

		// Token: 0x04004678 RID: 18040
		public GameObject gorillaParent;

		// Token: 0x04004679 RID: 18041
		public GameObject joinFailedBlock;
	}
}
