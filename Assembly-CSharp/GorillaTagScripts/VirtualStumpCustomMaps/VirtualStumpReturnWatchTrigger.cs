using System;
using GorillaLocomotion;
using GorillaNetworking;
using UnityEngine;

namespace GorillaTagScripts.VirtualStumpCustomMaps
{
	// Token: 0x020009FD RID: 2557
	public class VirtualStumpReturnWatchTrigger : MonoBehaviour
	{
		// Token: 0x06003FE9 RID: 16361 RVA: 0x00059C03 File Offset: 0x00057E03
		public void OnTriggerEnter(Collider other)
		{
			if (other == GTPlayer.Instance.headCollider)
			{
				VRRig.LocalRig.EnableVStumpReturnWatch(false);
			}
		}

		// Token: 0x06003FEA RID: 16362 RVA: 0x00059C22 File Offset: 0x00057E22
		public void OnTriggerExit(Collider other)
		{
			if (other == GTPlayer.Instance.headCollider && GorillaComputer.instance.IsPlayerInVirtualStump())
			{
				VRRig.LocalRig.EnableVStumpReturnWatch(true);
			}
		}
	}
}
