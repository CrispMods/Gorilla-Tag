using System;
using Photon.Pun;
using UnityEngine;

namespace GorillaNetworking
{
	// Token: 0x02000AE4 RID: 2788
	public class GorillaNetworkDisconnectTrigger : GorillaTriggerBox
	{
		// Token: 0x0600462A RID: 17962 RVA: 0x001850C4 File Offset: 0x001832C4
		public override void OnBoxTriggered()
		{
			base.OnBoxTriggered();
			if (this.makeSureThisIsEnabled != null)
			{
				this.makeSureThisIsEnabled.SetActive(true);
			}
			GameObject[] array = this.makeSureTheseAreEnabled;
			for (int i = 0; i < array.Length; i++)
			{
				array[i].SetActive(true);
			}
			if (PhotonNetwork.InRoom)
			{
				if (this.componentTypeToRemove != "" && this.componentTarget.GetComponent(this.componentTypeToRemove) != null)
				{
					UnityEngine.Object.Destroy(this.componentTarget.GetComponent(this.componentTypeToRemove));
				}
				PhotonNetwork.Disconnect();
				SkinnedMeshRenderer[] array2 = this.photonNetworkController.offlineVRRig;
				for (int i = 0; i < array2.Length; i++)
				{
					array2[i].enabled = true;
				}
				PhotonNetwork.ConnectUsingSettings();
			}
		}

		// Token: 0x04004753 RID: 18259
		public PhotonNetworkController photonNetworkController;

		// Token: 0x04004754 RID: 18260
		public GameObject offlineVRRig;

		// Token: 0x04004755 RID: 18261
		public GameObject makeSureThisIsEnabled;

		// Token: 0x04004756 RID: 18262
		public GameObject[] makeSureTheseAreEnabled;

		// Token: 0x04004757 RID: 18263
		public string componentTypeToRemove;

		// Token: 0x04004758 RID: 18264
		public GameObject componentTarget;
	}
}
