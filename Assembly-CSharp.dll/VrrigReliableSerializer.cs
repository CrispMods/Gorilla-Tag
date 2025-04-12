using System;
using Fusion;
using UnityEngine;

// Token: 0x02000546 RID: 1350
[NetworkBehaviourWeaved(0)]
internal class VrrigReliableSerializer : GorillaWrappedSerializer
{
	// Token: 0x060020F5 RID: 8437 RVA: 0x0002F75F File Offset: 0x0002D95F
	protected override void OnBeforeDespawn()
	{
	}

	// Token: 0x060020F6 RID: 8438 RVA: 0x0002F75F File Offset: 0x0002D95F
	protected override void OnFailedSpawn()
	{
	}

	// Token: 0x060020F7 RID: 8439 RVA: 0x000F29AC File Offset: 0x000F0BAC
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

	// Token: 0x060020F8 RID: 8440 RVA: 0x0002F75F File Offset: 0x0002D95F
	protected override void OnSuccesfullySpawned(PhotonMessageInfoWrapped info)
	{
	}

	// Token: 0x060020FA RID: 8442 RVA: 0x00045124 File Offset: 0x00043324
	[WeaverGenerated]
	public override void CopyBackingFieldsToState(bool A_1)
	{
		base.CopyBackingFieldsToState(A_1);
	}

	// Token: 0x060020FB RID: 8443 RVA: 0x00045130 File Offset: 0x00043330
	[WeaverGenerated]
	public override void CopyStateToBackingFields()
	{
		base.CopyStateToBackingFields();
	}
}
