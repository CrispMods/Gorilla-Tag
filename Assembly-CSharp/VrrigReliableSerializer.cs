using System;
using Fusion;
using UnityEngine;

// Token: 0x02000545 RID: 1349
[NetworkBehaviourWeaved(0)]
internal class VrrigReliableSerializer : GorillaWrappedSerializer
{
	// Token: 0x060020ED RID: 8429 RVA: 0x000023F4 File Offset: 0x000005F4
	protected override void OnBeforeDespawn()
	{
	}

	// Token: 0x060020EE RID: 8430 RVA: 0x000023F4 File Offset: 0x000005F4
	protected override void OnFailedSpawn()
	{
	}

	// Token: 0x060020EF RID: 8431 RVA: 0x000A4A60 File Offset: 0x000A2C60
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

	// Token: 0x060020F0 RID: 8432 RVA: 0x000023F4 File Offset: 0x000005F4
	protected override void OnSuccesfullySpawned(PhotonMessageInfoWrapped info)
	{
	}

	// Token: 0x060020F2 RID: 8434 RVA: 0x000A2DD9 File Offset: 0x000A0FD9
	[WeaverGenerated]
	public override void CopyBackingFieldsToState(bool A_1)
	{
		base.CopyBackingFieldsToState(A_1);
	}

	// Token: 0x060020F3 RID: 8435 RVA: 0x000A2DE5 File Offset: 0x000A0FE5
	[WeaverGenerated]
	public override void CopyStateToBackingFields()
	{
		base.CopyStateToBackingFields();
	}
}
