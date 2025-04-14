using System;
using Fusion;
using Photon.Pun;
using UnityEngine;

// Token: 0x020003EF RID: 1007
[NetworkBehaviourWeaved(0)]
public abstract class NetworkHoldableObject : NetworkComponent, IHoldableObject
{
	// Token: 0x170002B7 RID: 695
	// (get) Token: 0x0600189C RID: 6300 RVA: 0x00002076 File Offset: 0x00000276
	public virtual bool TwoHanded
	{
		get
		{
			return false;
		}
	}

	// Token: 0x0600189D RID: 6301
	public abstract void OnHover(InteractionPoint pointHovered, GameObject hoveringHand);

	// Token: 0x0600189E RID: 6302
	public abstract void OnGrab(InteractionPoint pointGrabbed, GameObject grabbingHand);

	// Token: 0x0600189F RID: 6303
	public abstract void DropItemCleanup();

	// Token: 0x060018A0 RID: 6304 RVA: 0x00077CC0 File Offset: 0x00075EC0
	public virtual bool OnRelease(DropZone zoneReleased, GameObject releasingHand)
	{
		return (EquipmentInteractor.instance.rightHandHeldEquipment != this || !(releasingHand != EquipmentInteractor.instance.rightHand)) && (EquipmentInteractor.instance.leftHandHeldEquipment != this || !(releasingHand != EquipmentInteractor.instance.leftHand));
	}

	// Token: 0x060018A1 RID: 6305 RVA: 0x000023F4 File Offset: 0x000005F4
	public override void ReadDataFusion()
	{
	}

	// Token: 0x060018A2 RID: 6306 RVA: 0x000023F4 File Offset: 0x000005F4
	public override void WriteDataFusion()
	{
	}

	// Token: 0x060018A3 RID: 6307 RVA: 0x000023F4 File Offset: 0x000005F4
	protected override void ReadDataPUN(PhotonStream stream, PhotonMessageInfo info)
	{
	}

	// Token: 0x060018A4 RID: 6308 RVA: 0x000023F4 File Offset: 0x000005F4
	protected override void WriteDataPUN(PhotonStream stream, PhotonMessageInfo info)
	{
	}

	// Token: 0x060018A6 RID: 6310 RVA: 0x00012597 File Offset: 0x00010797
	GameObject IHoldableObject.get_gameObject()
	{
		return base.gameObject;
	}

	// Token: 0x060018A7 RID: 6311 RVA: 0x0001259F File Offset: 0x0001079F
	string IHoldableObject.get_name()
	{
		return base.name;
	}

	// Token: 0x060018A8 RID: 6312 RVA: 0x000125A7 File Offset: 0x000107A7
	void IHoldableObject.set_name(string value)
	{
		base.name = value;
	}

	// Token: 0x060018A9 RID: 6313 RVA: 0x00002655 File Offset: 0x00000855
	[WeaverGenerated]
	public override void CopyBackingFieldsToState(bool A_1)
	{
		base.CopyBackingFieldsToState(A_1);
	}

	// Token: 0x060018AA RID: 6314 RVA: 0x00002661 File Offset: 0x00000861
	[WeaverGenerated]
	public override void CopyStateToBackingFields()
	{
		base.CopyStateToBackingFields();
	}
}
