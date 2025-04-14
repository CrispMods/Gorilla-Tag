﻿using System;
using UnityEngine;

// Token: 0x020003E3 RID: 995
public abstract class HoldableObject : MonoBehaviour, IHoldableObject
{
	// Token: 0x170002B2 RID: 690
	// (get) Token: 0x06001820 RID: 6176 RVA: 0x00002076 File Offset: 0x00000276
	public virtual bool TwoHanded
	{
		get
		{
			return false;
		}
	}

	// Token: 0x06001821 RID: 6177
	public abstract void OnHover(InteractionPoint pointHovered, GameObject hoveringHand);

	// Token: 0x06001822 RID: 6178
	public abstract void OnGrab(InteractionPoint pointGrabbed, GameObject grabbingHand);

	// Token: 0x06001823 RID: 6179
	public abstract void DropItemCleanup();

	// Token: 0x06001824 RID: 6180 RVA: 0x00075A98 File Offset: 0x00073C98
	public virtual bool OnRelease(DropZone zoneReleased, GameObject releasingHand)
	{
		return (EquipmentInteractor.instance.rightHandHeldEquipment != this || !(releasingHand != EquipmentInteractor.instance.rightHand)) && (EquipmentInteractor.instance.leftHandHeldEquipment != this || !(releasingHand != EquipmentInteractor.instance.leftHand));
	}

	// Token: 0x06001826 RID: 6182 RVA: 0x00012597 File Offset: 0x00010797
	GameObject IHoldableObject.get_gameObject()
	{
		return base.gameObject;
	}

	// Token: 0x06001827 RID: 6183 RVA: 0x0001259F File Offset: 0x0001079F
	string IHoldableObject.get_name()
	{
		return base.name;
	}

	// Token: 0x06001828 RID: 6184 RVA: 0x000125A7 File Offset: 0x000107A7
	void IHoldableObject.set_name(string value)
	{
		base.name = value;
	}
}
