using System;
using Photon.Pun;
using UnityEngine;

namespace GorillaNetworking
{
	// Token: 0x02000AB7 RID: 2743
	public class GorillaNetworkDisconnectTrigger : GorillaTriggerBox
	{
		// Token: 0x060044E5 RID: 17637 RVA: 0x00147604 File Offset: 0x00145804
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
					Object.Destroy(this.componentTarget.GetComponent(this.componentTypeToRemove));
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

		// Token: 0x0400465C RID: 18012
		public PhotonNetworkController photonNetworkController;

		// Token: 0x0400465D RID: 18013
		public GameObject offlineVRRig;

		// Token: 0x0400465E RID: 18014
		public GameObject makeSureThisIsEnabled;

		// Token: 0x0400465F RID: 18015
		public GameObject[] makeSureTheseAreEnabled;

		// Token: 0x04004660 RID: 18016
		public string componentTypeToRemove;

		// Token: 0x04004661 RID: 18017
		public GameObject componentTarget;
	}
}
