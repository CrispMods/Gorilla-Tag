using System;
using System.Collections.Generic;
using GorillaLocomotion.Climbing;
using UnityEngine;
using UnityEngine.XR;

// Token: 0x02000381 RID: 897
public class EquipmentInteractor : MonoBehaviour
{
	// Token: 0x1700024D RID: 589
	// (get) Token: 0x060014ED RID: 5357 RVA: 0x0003E191 File Offset: 0x0003C391
	public GorillaHandClimber BodyClimber
	{
		get
		{
			return this.bodyClimber;
		}
	}

	// Token: 0x1700024E RID: 590
	// (get) Token: 0x060014EE RID: 5358 RVA: 0x0003E199 File Offset: 0x0003C399
	public GorillaHandClimber LeftClimber
	{
		get
		{
			return this.leftClimber;
		}
	}

	// Token: 0x1700024F RID: 591
	// (get) Token: 0x060014EF RID: 5359 RVA: 0x0003E1A1 File Offset: 0x0003C3A1
	public GorillaHandClimber RightClimber
	{
		get
		{
			return this.rightClimber;
		}
	}

	// Token: 0x060014F0 RID: 5360 RVA: 0x000BE358 File Offset: 0x000BC558
	private void Awake()
	{
		if (EquipmentInteractor.instance == null)
		{
			EquipmentInteractor.instance = this;
			EquipmentInteractor.hasInstance = true;
		}
		else if (EquipmentInteractor.instance != this)
		{
			UnityEngine.Object.Destroy(base.gameObject);
		}
		this.autoGrabLeft = true;
		this.autoGrabRight = true;
	}

	// Token: 0x060014F1 RID: 5361 RVA: 0x0003E1A9 File Offset: 0x0003C3A9
	private void OnDestroy()
	{
		if (EquipmentInteractor.instance == this)
		{
			EquipmentInteractor.hasInstance = false;
			EquipmentInteractor.instance = null;
		}
	}

	// Token: 0x060014F2 RID: 5362 RVA: 0x0003E1C8 File Offset: 0x0003C3C8
	public void ReleaseRightHand()
	{
		if (this.rightHandHeldEquipment != null)
		{
			this.rightHandHeldEquipment.OnRelease(null, this.rightHand);
		}
		if (this.leftHandHeldEquipment != null)
		{
			this.leftHandHeldEquipment.OnRelease(null, this.rightHand);
		}
		this.autoGrabRight = true;
	}

	// Token: 0x060014F3 RID: 5363 RVA: 0x0003E207 File Offset: 0x0003C407
	public void ReleaseLeftHand()
	{
		if (this.rightHandHeldEquipment != null)
		{
			this.rightHandHeldEquipment.OnRelease(null, this.leftHand);
		}
		if (this.leftHandHeldEquipment != null)
		{
			this.leftHandHeldEquipment.OnRelease(null, this.leftHand);
		}
		this.autoGrabLeft = true;
	}

	// Token: 0x060014F4 RID: 5364 RVA: 0x0003E246 File Offset: 0x0003C446
	public void ForceStopClimbing()
	{
		this.bodyClimber.ForceStopClimbing(false, false);
		this.leftClimber.ForceStopClimbing(false, false);
		this.rightClimber.ForceStopClimbing(false, false);
	}

	// Token: 0x060014F5 RID: 5365 RVA: 0x0003E26F File Offset: 0x0003C46F
	public bool GetIsHolding(XRNode node)
	{
		if (node == XRNode.LeftHand)
		{
			return this.leftHandHeldEquipment != null;
		}
		return this.rightHandHeldEquipment != null;
	}

	// Token: 0x060014F6 RID: 5366 RVA: 0x000BE3AC File Offset: 0x000BC5AC
	public void InteractionPointDisabled(InteractionPoint interactionPoint)
	{
		if (this.iteratingInteractionPoints)
		{
			this.interactionPointsToRemove.Add(interactionPoint);
			return;
		}
		if (this.overlapInteractionPointsLeft != null)
		{
			this.overlapInteractionPointsLeft.Remove(interactionPoint);
		}
		if (this.overlapInteractionPointsRight != null)
		{
			this.overlapInteractionPointsRight.Remove(interactionPoint);
		}
	}

	// Token: 0x060014F7 RID: 5367 RVA: 0x000BE3F8 File Offset: 0x000BC5F8
	private void LateUpdate()
	{
		if (ApplicationQuittingState.IsQuitting)
		{
			return;
		}
		this.CheckInputValue(true);
		this.isLeftGrabbing = ((this.wasLeftGrabPressed && this.grabValue > this.grabThreshold - this.grabHysteresis) || (!this.wasLeftGrabPressed && this.grabValue > this.grabThreshold + this.grabHysteresis));
		if (this.leftClimber && this.leftClimber.isClimbing)
		{
			this.isLeftGrabbing = false;
		}
		this.CheckInputValue(false);
		this.isRightGrabbing = ((this.wasRightGrabPressed && this.grabValue > this.grabThreshold - this.grabHysteresis) || (!this.wasRightGrabPressed && this.grabValue > this.grabThreshold + this.grabHysteresis));
		if (this.rightClimber && this.rightClimber.isClimbing)
		{
			this.isRightGrabbing = false;
		}
		BuilderPiece pieceInHand = this.builderPieceInteractor.heldPiece[0];
		BuilderPiece pieceInHand2 = this.builderPieceInteractor.heldPiece[1];
		this.FireHandInteractions(this.leftHand, true, pieceInHand);
		this.FireHandInteractions(this.rightHand, false, pieceInHand2);
		if (!this.isRightGrabbing && this.wasRightGrabPressed)
		{
			this.ReleaseRightHand();
		}
		if (!this.isLeftGrabbing && this.wasLeftGrabPressed)
		{
			this.ReleaseLeftHand();
		}
		this.builderPieceInteractor.OnLateUpdate();
		GamePlayerLocal.instance.OnUpdateInteract();
		this.wasLeftGrabPressed = this.isLeftGrabbing;
		this.wasRightGrabPressed = this.isRightGrabbing;
	}

	// Token: 0x060014F8 RID: 5368 RVA: 0x000BE588 File Offset: 0x000BC788
	private void FireHandInteractions(GameObject interactingHand, bool isLeftHand, BuilderPiece pieceInHand)
	{
		if (isLeftHand)
		{
			this.justGrabbed = ((this.isLeftGrabbing && !this.wasLeftGrabPressed) || (this.isLeftGrabbing && this.autoGrabLeft));
			this.justReleased = (this.leftHandHeldEquipment != null && !this.isLeftGrabbing && this.wasLeftGrabPressed);
		}
		else
		{
			this.justGrabbed = ((this.isRightGrabbing && !this.wasRightGrabPressed) || (this.isRightGrabbing && this.autoGrabRight));
			this.justReleased = (this.rightHandHeldEquipment != null && !this.isRightGrabbing && this.wasRightGrabPressed);
		}
		List<InteractionPoint> list = isLeftHand ? this.overlapInteractionPointsLeft : this.overlapInteractionPointsRight;
		bool flag = isLeftHand ? (this.leftHandHeldEquipment != null) : (this.rightHandHeldEquipment != null);
		bool flag2 = pieceInHand != null;
		bool flag3 = isLeftHand ? this.disableLeftGrab : this.disableRightGrab;
		bool flag4 = !flag && !flag2 && !flag3;
		this.iteratingInteractionPoints = true;
		foreach (InteractionPoint interactionPoint in list)
		{
			if (flag4 && interactionPoint != null)
			{
				if (this.justGrabbed)
				{
					interactionPoint.Holdable.OnGrab(interactionPoint, interactingHand);
				}
				else
				{
					interactionPoint.Holdable.OnHover(interactionPoint, interactingHand);
				}
			}
			if (this.justReleased)
			{
				this.tempZone = interactionPoint.GetComponent<DropZone>();
				if (this.tempZone != null)
				{
					if (interactingHand == this.leftHand)
					{
						if (this.leftHandHeldEquipment != null)
						{
							this.leftHandHeldEquipment.OnRelease(this.tempZone, interactingHand);
						}
					}
					else if (this.rightHandHeldEquipment != null)
					{
						this.rightHandHeldEquipment.OnRelease(this.tempZone, interactingHand);
					}
				}
			}
		}
		this.iteratingInteractionPoints = false;
		foreach (InteractionPoint item in this.interactionPointsToRemove)
		{
			if (this.overlapInteractionPointsLeft != null)
			{
				this.overlapInteractionPointsLeft.Remove(item);
			}
			if (this.overlapInteractionPointsRight != null)
			{
				this.overlapInteractionPointsRight.Remove(item);
			}
		}
		this.interactionPointsToRemove.Clear();
	}

	// Token: 0x060014F9 RID: 5369 RVA: 0x000BE7E0 File Offset: 0x000BC9E0
	public void UpdateHandEquipment(IHoldableObject newEquipment, bool forLeftHand)
	{
		if (forLeftHand)
		{
			if (newEquipment != null && newEquipment == this.rightHandHeldEquipment && !newEquipment.TwoHanded)
			{
				this.rightHandHeldEquipment = null;
			}
			if (this.leftHandHeldEquipment != null)
			{
				this.leftHandHeldEquipment.DropItemCleanup();
			}
			this.leftHandHeldEquipment = newEquipment;
			this.autoGrabLeft = false;
			return;
		}
		if (newEquipment != null && newEquipment == this.leftHandHeldEquipment && !newEquipment.TwoHanded)
		{
			this.leftHandHeldEquipment = null;
		}
		if (this.rightHandHeldEquipment != null)
		{
			this.rightHandHeldEquipment.DropItemCleanup();
		}
		this.rightHandHeldEquipment = newEquipment;
		this.autoGrabRight = false;
	}

	// Token: 0x060014FA RID: 5370 RVA: 0x000BE86C File Offset: 0x000BCA6C
	public void CheckInputValue(bool isLeftHand)
	{
		if (isLeftHand)
		{
			this.grabValue = ControllerInputPoller.GripFloat(XRNode.LeftHand);
			this.tempValue = ControllerInputPoller.TriggerFloat(XRNode.LeftHand);
		}
		else
		{
			this.grabValue = ControllerInputPoller.GripFloat(XRNode.RightHand);
			this.tempValue = ControllerInputPoller.TriggerFloat(XRNode.RightHand);
		}
		this.grabValue = Mathf.Max(this.grabValue, this.tempValue);
	}

	// Token: 0x060014FB RID: 5371 RVA: 0x0003E288 File Offset: 0x0003C488
	public void ForceDropEquipment(IHoldableObject equipment)
	{
		if (this.rightHandHeldEquipment == equipment)
		{
			this.rightHandHeldEquipment = null;
		}
		if (this.leftHandHeldEquipment == equipment)
		{
			this.leftHandHeldEquipment = null;
		}
	}

	// Token: 0x060014FC RID: 5372 RVA: 0x000BE8C8 File Offset: 0x000BCAC8
	public void ForceDropManipulatableObject(HoldableObject manipulatableObject)
	{
		if ((HoldableObject)this.rightHandHeldEquipment == manipulatableObject)
		{
			this.rightHandHeldEquipment.OnRelease(null, this.rightHand);
			this.rightHandHeldEquipment = null;
			this.autoGrabRight = false;
		}
		if ((HoldableObject)this.leftHandHeldEquipment == manipulatableObject)
		{
			this.leftHandHeldEquipment.OnRelease(null, this.leftHand);
			this.leftHandHeldEquipment = null;
			this.autoGrabLeft = false;
		}
	}

	// Token: 0x04001723 RID: 5923
	[OnEnterPlay_SetNull]
	public static volatile EquipmentInteractor instance;

	// Token: 0x04001724 RID: 5924
	[OnEnterPlay_Set(false)]
	public static bool hasInstance;

	// Token: 0x04001725 RID: 5925
	public IHoldableObject leftHandHeldEquipment;

	// Token: 0x04001726 RID: 5926
	public IHoldableObject rightHandHeldEquipment;

	// Token: 0x04001727 RID: 5927
	public BuilderPieceInteractor builderPieceInteractor;

	// Token: 0x04001728 RID: 5928
	public GameObject rightHand;

	// Token: 0x04001729 RID: 5929
	public GameObject leftHand;

	// Token: 0x0400172A RID: 5930
	public InputDevice leftHandDevice;

	// Token: 0x0400172B RID: 5931
	public InputDevice rightHandDevice;

	// Token: 0x0400172C RID: 5932
	public List<InteractionPoint> overlapInteractionPointsLeft = new List<InteractionPoint>();

	// Token: 0x0400172D RID: 5933
	public List<InteractionPoint> overlapInteractionPointsRight = new List<InteractionPoint>();

	// Token: 0x0400172E RID: 5934
	public float grabRadius;

	// Token: 0x0400172F RID: 5935
	public float grabThreshold = 0.7f;

	// Token: 0x04001730 RID: 5936
	public float grabHysteresis = 0.05f;

	// Token: 0x04001731 RID: 5937
	public bool wasLeftGrabPressed;

	// Token: 0x04001732 RID: 5938
	public bool wasRightGrabPressed;

	// Token: 0x04001733 RID: 5939
	public bool isLeftGrabbing;

	// Token: 0x04001734 RID: 5940
	public bool isRightGrabbing;

	// Token: 0x04001735 RID: 5941
	public bool justReleased;

	// Token: 0x04001736 RID: 5942
	public bool justGrabbed;

	// Token: 0x04001737 RID: 5943
	public bool disableLeftGrab;

	// Token: 0x04001738 RID: 5944
	public bool disableRightGrab;

	// Token: 0x04001739 RID: 5945
	public bool autoGrabLeft;

	// Token: 0x0400173A RID: 5946
	public bool autoGrabRight;

	// Token: 0x0400173B RID: 5947
	private float grabValue;

	// Token: 0x0400173C RID: 5948
	private float tempValue;

	// Token: 0x0400173D RID: 5949
	private DropZone tempZone;

	// Token: 0x0400173E RID: 5950
	private bool iteratingInteractionPoints;

	// Token: 0x0400173F RID: 5951
	private List<InteractionPoint> interactionPointsToRemove = new List<InteractionPoint>();

	// Token: 0x04001740 RID: 5952
	[SerializeField]
	private GorillaHandClimber bodyClimber;

	// Token: 0x04001741 RID: 5953
	[SerializeField]
	private GorillaHandClimber leftClimber;

	// Token: 0x04001742 RID: 5954
	[SerializeField]
	private GorillaHandClimber rightClimber;
}
