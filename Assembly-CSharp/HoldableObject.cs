using System;
using UnityEngine;

// Token: 0x020003EE RID: 1006
public abstract class HoldableObject : MonoBehaviour, IHoldableObject
{
	// Token: 0x170002B9 RID: 697
	// (get) Token: 0x0600186A RID: 6250 RVA: 0x00030498 File Offset: 0x0002E698
	public virtual bool TwoHanded
	{
		get
		{
			return false;
		}
	}

	// Token: 0x0600186B RID: 6251
	public abstract void OnHover(InteractionPoint pointHovered, GameObject hoveringHand);

	// Token: 0x0600186C RID: 6252
	public abstract void OnGrab(InteractionPoint pointGrabbed, GameObject grabbingHand);

	// Token: 0x0600186D RID: 6253
	public abstract void DropItemCleanup();

	// Token: 0x0600186E RID: 6254 RVA: 0x000CBE70 File Offset: 0x000CA070
	public virtual bool OnRelease(DropZone zoneReleased, GameObject releasingHand)
	{
		return (EquipmentInteractor.instance.rightHandHeldEquipment != this || !(releasingHand != EquipmentInteractor.instance.rightHand)) && (EquipmentInteractor.instance.leftHandHeldEquipment != this || !(releasingHand != EquipmentInteractor.instance.leftHand));
	}

	// Token: 0x06001870 RID: 6256 RVA: 0x00032616 File Offset: 0x00030816
	GameObject IHoldableObject.get_gameObject()
	{
		return base.gameObject;
	}

	// Token: 0x06001871 RID: 6257 RVA: 0x0003261E File Offset: 0x0003081E
	string IHoldableObject.get_name()
	{
		return base.name;
	}

	// Token: 0x06001872 RID: 6258 RVA: 0x00032626 File Offset: 0x00030826
	void IHoldableObject.set_name(string value)
	{
		base.name = value;
	}
}
