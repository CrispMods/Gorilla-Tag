using System;
using UnityEngine;
using UnityEngine.Events;

// Token: 0x020003A4 RID: 932
public class TransferrableObjectHoldablePart : HoldableObject
{
	// Token: 0x060015C5 RID: 5573 RVA: 0x000C0EC4 File Offset: 0x000BF0C4
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

	// Token: 0x060015C6 RID: 5574 RVA: 0x00030607 File Offset: 0x0002E807
	protected virtual void UpdateHeld(VRRig rig, bool isHeldLeftHand)
	{
	}

	// Token: 0x060015C7 RID: 5575 RVA: 0x00030607 File Offset: 0x0002E807
	public override void OnHover(InteractionPoint pointHovered, GameObject hoveringHand)
	{
	}

	// Token: 0x060015C8 RID: 5576 RVA: 0x000C0F90 File Offset: 0x000BF190
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

	// Token: 0x060015C9 RID: 5577 RVA: 0x0003EAD7 File Offset: 0x0003CCD7
	public override void DropItemCleanup()
	{
		this.isHeld = false;
		this.isHeldLeftHand = false;
		this.transferrableParentObject.itemState &= ~this.heldBit;
	}

	// Token: 0x060015CA RID: 5578 RVA: 0x000C0FF8 File Offset: 0x000BF1F8
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

	// Token: 0x0400180B RID: 6155
	[SerializeField]
	protected TransferrableObject transferrableParentObject;

	// Token: 0x0400180C RID: 6156
	[SerializeField]
	private TransferrableObject.ItemStates heldBit = TransferrableObject.ItemStates.Part0Held;

	// Token: 0x0400180D RID: 6157
	private bool isHeld;

	// Token: 0x0400180E RID: 6158
	protected bool isHeldLeftHand;

	// Token: 0x0400180F RID: 6159
	public UnityEvent onGrab;

	// Token: 0x04001810 RID: 6160
	public UnityEvent onRelease;

	// Token: 0x04001811 RID: 6161
	public UnityEvent onDrop;
}
