using System;
using Fusion;
using Photon.Pun;
using UnityEngine;

// Token: 0x020003FA RID: 1018
[NetworkBehaviourWeaved(0)]
public abstract class NetworkHoldableObject : NetworkComponent, IHoldableObject
{
	// Token: 0x170002BE RID: 702
	// (get) Token: 0x060018E6 RID: 6374 RVA: 0x00030498 File Offset: 0x0002E698
	public virtual bool TwoHanded
	{
		get
		{
			return false;
		}
	}

	// Token: 0x060018E7 RID: 6375
	public abstract void OnHover(InteractionPoint pointHovered, GameObject hoveringHand);

	// Token: 0x060018E8 RID: 6376
	public abstract void OnGrab(InteractionPoint pointGrabbed, GameObject grabbingHand);

	// Token: 0x060018E9 RID: 6377
	public abstract void DropItemCleanup();

	// Token: 0x060018EA RID: 6378 RVA: 0x000CBE70 File Offset: 0x000CA070
	public virtual bool OnRelease(DropZone zoneReleased, GameObject releasingHand)
	{
		return (EquipmentInteractor.instance.rightHandHeldEquipment != this || !(releasingHand != EquipmentInteractor.instance.rightHand)) && (EquipmentInteractor.instance.leftHandHeldEquipment != this || !(releasingHand != EquipmentInteractor.instance.leftHand));
	}

	// Token: 0x060018EB RID: 6379 RVA: 0x00030607 File Offset: 0x0002E807
	public override void ReadDataFusion()
	{
	}

	// Token: 0x060018EC RID: 6380 RVA: 0x00030607 File Offset: 0x0002E807
	public override void WriteDataFusion()
	{
	}

	// Token: 0x060018ED RID: 6381 RVA: 0x00030607 File Offset: 0x0002E807
	protected override void ReadDataPUN(PhotonStream stream, PhotonMessageInfo info)
	{
	}

	// Token: 0x060018EE RID: 6382 RVA: 0x00030607 File Offset: 0x0002E807
	protected override void WriteDataPUN(PhotonStream stream, PhotonMessageInfo info)
	{
	}

	// Token: 0x060018F0 RID: 6384 RVA: 0x00032616 File Offset: 0x00030816
	GameObject IHoldableObject.get_gameObject()
	{
		return base.gameObject;
	}

	// Token: 0x060018F1 RID: 6385 RVA: 0x0003261E File Offset: 0x0003081E
	string IHoldableObject.get_name()
	{
		return base.name;
	}

	// Token: 0x060018F2 RID: 6386 RVA: 0x00032626 File Offset: 0x00030826
	void IHoldableObject.set_name(string value)
	{
		base.name = value;
	}

	// Token: 0x060018F3 RID: 6387 RVA: 0x00030709 File Offset: 0x0002E909
	[WeaverGenerated]
	public override void CopyBackingFieldsToState(bool A_1)
	{
		base.CopyBackingFieldsToState(A_1);
	}

	// Token: 0x060018F4 RID: 6388 RVA: 0x00030715 File Offset: 0x0002E915
	[WeaverGenerated]
	public override void CopyStateToBackingFields()
	{
		base.CopyStateToBackingFields();
	}
}
