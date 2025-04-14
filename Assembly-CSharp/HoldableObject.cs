using System;
using UnityEngine;

// Token: 0x020003E3 RID: 995
public abstract class HoldableObject : MonoBehaviour, IHoldableObject
{
	// Token: 0x170002B2 RID: 690
	// (get) Token: 0x0600181D RID: 6173 RVA: 0x00002076 File Offset: 0x00000276
	public virtual bool TwoHanded
	{
		get
		{
			return false;
		}
	}

	// Token: 0x0600181E RID: 6174
	public abstract void OnHover(InteractionPoint pointHovered, GameObject hoveringHand);

	// Token: 0x0600181F RID: 6175
	public abstract void OnGrab(InteractionPoint pointGrabbed, GameObject grabbingHand);

	// Token: 0x06001820 RID: 6176
	public abstract void DropItemCleanup();

	// Token: 0x06001821 RID: 6177 RVA: 0x00075714 File Offset: 0x00073914
	public virtual bool OnRelease(DropZone zoneReleased, GameObject releasingHand)
	{
		return (EquipmentInteractor.instance.rightHandHeldEquipment != this || !(releasingHand != EquipmentInteractor.instance.rightHand)) && (EquipmentInteractor.instance.leftHandHeldEquipment != this || !(releasingHand != EquipmentInteractor.instance.leftHand));
	}

	// Token: 0x06001823 RID: 6179 RVA: 0x00012273 File Offset: 0x00010473
	GameObject IHoldableObject.get_gameObject()
	{
		return base.gameObject;
	}

	// Token: 0x06001824 RID: 6180 RVA: 0x0001227B File Offset: 0x0001047B
	string IHoldableObject.get_name()
	{
		return base.name;
	}

	// Token: 0x06001825 RID: 6181 RVA: 0x00012283 File Offset: 0x00010483
	void IHoldableObject.set_name(string value)
	{
		base.name = value;
	}
}
