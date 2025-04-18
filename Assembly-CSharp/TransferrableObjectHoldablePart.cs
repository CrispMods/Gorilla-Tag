﻿using System;
using UnityEngine;
using UnityEngine.Events;

// Token: 0x02000399 RID: 921
public class TransferrableObjectHoldablePart : HoldableObject
{
	// Token: 0x06001579 RID: 5497 RVA: 0x000688F4 File Offset: 0x00066AF4
	private void Update()
	{
		VRRig rig;
		if (!this.transferrableParentObject.IsLocalObject())
		{
			rig = this.transferrableParentObject.myOnlineRig;
			this.isHeld = ((this.transferrableParentObject.itemState & this.heldBit) > (TransferrableObject.ItemStates)0);
			TransferrableObject.PositionState currentState = this.transferrableParentObject.currentState;
			if (currentState == TransferrableObject.PositionState.OnRightArm || currentState == TransferrableObject.PositionState.InRightHand)
			{
				this.isHeldLeftHand = this.isHeld;
			}
			else
			{
				this.isHeldLeftHand = false;
			}
		}
		else
		{
			rig = VRRig.LocalRig;
		}
		if (this.isHeld)
		{
			if (this.transferrableParentObject.InHand())
			{
				this.UpdateHeld(rig, this.isHeldLeftHand);
				return;
			}
			if (this.transferrableParentObject.IsLocalObject())
			{
				this.OnRelease(null, this.isHeldLeftHand ? EquipmentInteractor.instance.leftHand : EquipmentInteractor.instance.rightHand);
			}
		}
	}

	// Token: 0x0600157A RID: 5498 RVA: 0x000023F4 File Offset: 0x000005F4
	protected virtual void UpdateHeld(VRRig rig, bool isHeldLeftHand)
	{
	}

	// Token: 0x0600157B RID: 5499 RVA: 0x000023F4 File Offset: 0x000005F4
	public override void OnHover(InteractionPoint pointHovered, GameObject hoveringHand)
	{
	}

	// Token: 0x0600157C RID: 5500 RVA: 0x000689C0 File Offset: 0x00066BC0
	public override void OnGrab(InteractionPoint pointGrabbed, GameObject grabbingHand)
	{
		this.isHeld = true;
		this.isHeldLeftHand = (grabbingHand == EquipmentInteractor.instance.leftHand);
		this.transferrableParentObject.itemState |= this.heldBit;
		EquipmentInteractor.instance.UpdateHandEquipment(this, this.isHeldLeftHand);
		UnityEvent unityEvent = this.onGrab;
		if (unityEvent == null)
		{
			return;
		}
		unityEvent.Invoke();
	}

	// Token: 0x0600157D RID: 5501 RVA: 0x00068A27 File Offset: 0x00066C27
	public override void DropItemCleanup()
	{
		this.isHeld = false;
		this.isHeldLeftHand = false;
		this.transferrableParentObject.itemState &= ~this.heldBit;
	}

	// Token: 0x0600157E RID: 5502 RVA: 0x00068A50 File Offset: 0x00066C50
	public override bool OnRelease(DropZone zoneReleased, GameObject releasingHand)
	{
		if (EquipmentInteractor.instance.rightHandHeldEquipment == this && releasingHand != EquipmentInteractor.instance.rightHand)
		{
			return false;
		}
		if (EquipmentInteractor.instance.leftHandHeldEquipment == this && releasingHand != EquipmentInteractor.instance.leftHand)
		{
			return false;
		}
		EquipmentInteractor.instance.UpdateHandEquipment(null, this.isHeldLeftHand);
		this.isHeld = false;
		this.isHeldLeftHand = false;
		this.transferrableParentObject.itemState &= ~this.heldBit;
		UnityEvent unityEvent = this.onRelease;
		if (unityEvent != null)
		{
			unityEvent.Invoke();
		}
		return true;
	}

	// Token: 0x040017C4 RID: 6084
	[SerializeField]
	protected TransferrableObject transferrableParentObject;

	// Token: 0x040017C5 RID: 6085
	[SerializeField]
	private TransferrableObject.ItemStates heldBit = TransferrableObject.ItemStates.Part0Held;

	// Token: 0x040017C6 RID: 6086
	private bool isHeld;

	// Token: 0x040017C7 RID: 6087
	protected bool isHeldLeftHand;

	// Token: 0x040017C8 RID: 6088
	public UnityEvent onGrab;

	// Token: 0x040017C9 RID: 6089
	public UnityEvent onRelease;

	// Token: 0x040017CA RID: 6090
	public UnityEvent onDrop;
}
