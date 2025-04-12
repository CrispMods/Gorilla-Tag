using System;
using GorillaTagScripts;
using Photon.Pun;
using UnityEngine;

// Token: 0x020002B1 RID: 689
public class NonCosmeticItemProvider : MonoBehaviour
{
	// Token: 0x060010BB RID: 4283 RVA: 0x000A9DDC File Offset: 0x000A7FDC
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

	// Token: 0x040012BB RID: 4795
	public GTZone zone;

	// Token: 0x040012BC RID: 4796
	[Tooltip("only for honeycomb")]
	public bool useCondition;

	// Token: 0x040012BD RID: 4797
	public int conditionThreshold;

	// Token: 0x040012BE RID: 4798
	public NonCosmeticItemProvider.ItemType itemType;

	// Token: 0x020002B2 RID: 690
	public enum ItemType
	{
		// Token: 0x040012C0 RID: 4800
		honeycomb
	}
}
