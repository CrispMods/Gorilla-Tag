using System;
using Photon.Pun;
using UnityEngine;

namespace GorillaNetworking
{
	// Token: 0x02000ABA RID: 2746
	public class GorillaNetworkDisconnectTrigger : GorillaTriggerBox
	{
		// Token: 0x060044F1 RID: 17649 RVA: 0x00147BCC File Offset: 0x00145DCC
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

		// Token: 0x0400466E RID: 18030
		public PhotonNetworkController photonNetworkController;

		// Token: 0x0400466F RID: 18031
		public GameObject offlineVRRig;

		// Token: 0x04004670 RID: 18032
		public GameObject makeSureThisIsEnabled;

		// Token: 0x04004671 RID: 18033
		public GameObject[] makeSureTheseAreEnabled;

		// Token: 0x04004672 RID: 18034
		public string componentTypeToRemove;

		// Token: 0x04004673 RID: 18035
		public GameObject componentTarget;
	}
}
