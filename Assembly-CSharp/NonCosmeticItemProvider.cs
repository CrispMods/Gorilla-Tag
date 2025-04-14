using System;
using GorillaTagScripts;
using Photon.Pun;
using UnityEngine;

// Token: 0x020002B1 RID: 689
public class NonCosmeticItemProvider : MonoBehaviour
{
	// Token: 0x060010B8 RID: 4280 RVA: 0x00050F44 File Offset: 0x0004F144
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

	// Token: 0x040012BA RID: 4794
	public GTZone zone;

	// Token: 0x040012BB RID: 4795
	[Tooltip("only for honeycomb")]
	public bool useCondition;

	// Token: 0x040012BC RID: 4796
	public int conditionThreshold;

	// Token: 0x040012BD RID: 4797
	public NonCosmeticItemProvider.ItemType itemType;

	// Token: 0x020002B2 RID: 690
	public enum ItemType
	{
		// Token: 0x040012BF RID: 4799
		honeycomb
	}
}
