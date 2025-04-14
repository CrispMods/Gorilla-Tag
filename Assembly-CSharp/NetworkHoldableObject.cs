using System;
using Fusion;
using Photon.Pun;
using UnityEngine;

// Token: 0x020003EF RID: 1007
[NetworkBehaviourWeaved(0)]
public abstract class NetworkHoldableObject : NetworkComponent, IHoldableObject
{
	// Token: 0x170002B7 RID: 695
	// (get) Token: 0x06001899 RID: 6297 RVA: 0x00002076 File Offset: 0x00000276
	public virtual bool TwoHanded
	{
		get
		{
			return false;
		}
	}

	// Token: 0x0600189A RID: 6298
	public abstract void OnHover(InteractionPoint pointHovered, GameObject hoveringHand);

	// Token: 0x0600189B RID: 6299
	public abstract void OnGrab(InteractionPoint pointGrabbed, GameObject grabbingHand);

	// Token: 0x0600189C RID: 6300
	public abstract void DropItemCleanup();

	// Token: 0x0600189D RID: 6301 RVA: 0x0007793C File Offset: 0x00075B3C
	public virtual bool OnRelease(DropZone zoneReleased, GameObject releasingHand)
	{
		return (EquipmentInteractor.instance.rightHandHeldEquipment != this || !(releasingHand != EquipmentInteractor.instance.rightHand)) && (EquipmentInteractor.instance.leftHandHeldEquipment != this || !(releasingHand != EquipmentInteractor.instance.leftHand));
	}

	// Token: 0x0600189E RID: 6302 RVA: 0x000023F4 File Offset: 0x000005F4
	public override void ReadDataFusion()
	{
	}

	// Token: 0x0600189F RID: 6303 RVA: 0x000023F4 File Offset: 0x000005F4
	public override void WriteDataFusion()
	{
	}

	// Token: 0x060018A0 RID: 6304 RVA: 0x000023F4 File Offset: 0x000005F4
	protected override void ReadDataPUN(PhotonStream stream, PhotonMessageInfo info)
	{
	}

	// Token: 0x060018A1 RID: 6305 RVA: 0x000023F4 File Offset: 0x000005F4
	protected override void WriteDataPUN(PhotonStream stream, PhotonMessageInfo info)
	{
	}

	// Token: 0x060018A3 RID: 6307 RVA: 0x00012273 File Offset: 0x00010473
	GameObject IHoldableObject.get_gameObject()
	{
		return base.gameObject;
	}

	// Token: 0x060018A4 RID: 6308 RVA: 0x0001227B File Offset: 0x0001047B
	string IHoldableObject.get_name()
	{
		return base.name;
	}

	// Token: 0x060018A5 RID: 6309 RVA: 0x00012283 File Offset: 0x00010483
	void IHoldableObject.set_name(string value)
	{
		base.name = value;
	}

	// Token: 0x060018A6 RID: 6310 RVA: 0x00002655 File Offset: 0x00000855
	[WeaverGenerated]
	public override void CopyBackingFieldsToState(bool A_1)
	{
		base.CopyBackingFieldsToState(A_1);
	}

	// Token: 0x060018A7 RID: 6311 RVA: 0x00002661 File Offset: 0x00000861
	[WeaverGenerated]
	public override void CopyStateToBackingFields()
	{
		base.CopyStateToBackingFields();
	}
}
