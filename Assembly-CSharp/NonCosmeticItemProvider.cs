using System;
using GorillaTagScripts;
using Photon.Pun;
using UnityEngine;

// Token: 0x020002BC RID: 700
public class NonCosmeticItemProvider : MonoBehaviour
{
	// Token: 0x06001104 RID: 4356 RVA: 0x000AC674 File Offset: 0x000AA874
	private void OnTriggerEnter(Collider other)
	{
		GorillaTriggerColliderHandIndicator component = other.GetComponent<GorillaTriggerColliderHandIndicator>();
		if (component != null)
		{
			int healthyFlowersInZoneCount = FlowersManager.Instance.GetHealthyFlowersInZoneCount(this.zone);
			if (this.useCondition && this.itemType == NonCosmeticItemProvider.ItemType.honeycomb && (healthyFlowersInZoneCount < this.conditionThreshold || healthyFlowersInZoneCount < 0))
			{
				return;
			}
			GorillaGameManager.instance.FindPlayerVRRig(NetworkSystem.Instance.LocalPlayer).netView.SendRPC("EnableNonCosmeticHandItemRPC", RpcTarget.All, new object[]
			{
				true,
				component.isLeftHand
			});
		}
	}

	// Token: 0x04001302 RID: 4866
	public GTZone zone;

	// Token: 0x04001303 RID: 4867
	[Tooltip("only for honeycomb")]
	public bool useCondition;

	// Token: 0x04001304 RID: 4868
	public int conditionThreshold;

	// Token: 0x04001305 RID: 4869
	public NonCosmeticItemProvider.ItemType itemType;

	// Token: 0x020002BD RID: 701
	public enum ItemType
	{
		// Token: 0x04001307 RID: 4871
		honeycomb
	}
}
