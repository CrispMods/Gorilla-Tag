using System;
using Fusion;
using UnityEngine;

// Token: 0x02000553 RID: 1363
[NetworkBehaviourWeaved(0)]
internal class VrrigReliableSerializer : GorillaWrappedSerializer
{
	// Token: 0x0600214B RID: 8523 RVA: 0x00030607 File Offset: 0x0002E807
	protected override void OnBeforeDespawn()
	{
	}

	// Token: 0x0600214C RID: 8524 RVA: 0x00030607 File Offset: 0x0002E807
	protected override void OnFailedSpawn()
	{
	}

	// Token: 0x0600214D RID: 8525 RVA: 0x000F5728 File Offset: 0x000F3928
	protected override bool OnSpawnSetupCheck(PhotonMessageInfoWrapped wrappedInfo, out GameObject outTargetObject, out Type outTargetType)
	{
		outTargetObject = null;
		outTargetType = null;
		if (wrappedInfo.punInfo.Sender != wrappedInfo.punInfo.photonView.Owner || wrappedInfo.punInfo.photonView.IsRoomView)
		{
			return false;
		}
		RigContainer rigContainer;
		if (VRRigCache.Instance.TryGetVrrig(wrappedInfo.Sender, out rigContainer))
		{
			outTargetObject = rigContainer.gameObject;
			outTargetType = typeof(VRRigReliableState);
			return true;
		}
		return false;
	}

	// Token: 0x0600214E RID: 8526 RVA: 0x00030607 File Offset: 0x0002E807
	protected override void OnSuccesfullySpawned(PhotonMessageInfoWrapped info)
	{
	}

	// Token: 0x06002150 RID: 8528 RVA: 0x000464C3 File Offset: 0x000446C3
	[WeaverGenerated]
	public override void CopyBackingFieldsToState(bool A_1)
	{
		base.CopyBackingFieldsToState(A_1);
	}

	// Token: 0x06002151 RID: 8529 RVA: 0x000464CF File Offset: 0x000446CF
	[WeaverGenerated]
	public override void CopyStateToBackingFields()
	{
		base.CopyStateToBackingFields();
	}
}
