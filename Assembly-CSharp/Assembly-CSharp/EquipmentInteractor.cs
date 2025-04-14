using System;
using System.Collections.Generic;
using GorillaLocomotion.Climbing;
using UnityEngine;
using UnityEngine.XR;

// Token: 0x02000376 RID: 886
public class EquipmentInteractor : MonoBehaviour
{
	// Token: 0x17000246 RID: 582
	// (get) Token: 0x060014A4 RID: 5284 RVA: 0x000657CF File Offset: 0x000639CF
	public GorillaHandClimber BodyClimber
	{
		get
		{
			return this.bodyClimber;
		}
	}

	// Token: 0x17000247 RID: 583
	// (get) Token: 0x060014A5 RID: 5285 RVA: 0x000657D7 File Offset: 0x000639D7
	public GorillaHandClimber LeftClimber
	{
		get
		{
			return this.leftClimber;
		}
	}

	// Token: 0x17000248 RID: 584
	// (get) Token: 0x060014A6 RID: 5286 RVA: 0x000657DF File Offset: 0x000639DF
	public GorillaHandClimber RightClimber
	{
		get
		{
			return this.rightClimber;
		}
	}

	// Token: 0x060014A7 RID: 5287 RVA: 0x000657E8 File Offset: 0x000639E8
	private void Awake()
	{
		if (EquipmentInteractor.instance == null)
		{
			EquipmentInteractor.instance = this;
			EquipmentInteractor.hasInstance = true;
		}
		else if (EquipmentInteractor.instance != this)
		{
			Object.Destroy(base.gameObject);
		}
		this.autoGrabLeft = true;
		this.autoGrabRight = true;
	}

	// Token: 0x060014A8 RID: 5288 RVA: 0x0006583C File Offset: 0x00063A3C
	private void OnDestroy()
	{
		if (EquipmentInteractor.instance == this)
		{
			EquipmentInteractor.hasInstance = false;
			EquipmentInteractor.instance = null;
		}
	}

	// Token: 0x060014A9 RID: 5289 RVA: 0x0006585B File Offset: 0x00063A5B
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

	// Token: 0x060014AA RID: 5290 RVA: 0x0006589A File Offset: 0x00063A9A
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

	// Token: 0x060014AB RID: 5291 RVA: 0x000658D9 File Offset: 0x00063AD9
	public void ForceStopClimbing()
	{
		this.bodyClimber.ForceStopClimbing(false, false);
		this.leftClimber.ForceStopClimbing(false, false);
		this.rightClimber.ForceStopClimbing(false, false);
	}

	// Token: 0x060014AC RID: 5292 RVA: 0x00065902 File Offset: 0x00063B02
	public bool GetIsHolding(XRNode node)
	{
		if (node == XRNode.LeftHand)
		{
			return this.leftHandHeldEquipment != null;
		}
		return this.rightHandHeldEquipment != null;
	}

	// Token: 0x060014AD RID: 5293 RVA: 0x0006591C File Offset: 0x00063B1C
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

	// Token: 0x060014AE RID: 5294 RVA: 0x00065968 File Offset: 0x00063B68
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

	// Token: 0x060014AF RID: 5295 RVA: 0x00065AF8 File Offset: 0x00063CF8
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

	// Token: 0x060014B0 RID: 5296 RVA: 0x00065D50 File Offset: 0x00063F50
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

	// Token: 0x060014B1 RID: 5297 RVA: 0x00065DDC File Offset: 0x00063FDC
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

	// Token: 0x060014B2 RID: 5298 RVA: 0x00065E35 File Offset: 0x00064035
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

	// Token: 0x060014B3 RID: 5299 RVA: 0x00065E58 File Offset: 0x00064058
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

	// Token: 0x040016DC RID: 5852
	[OnEnterPlay_SetNull]
	public static volatile EquipmentInteractor instance;

	// Token: 0x040016DD RID: 5853
	[OnEnterPlay_Set(false)]
	public static bool hasInstance;

	// Token: 0x040016DE RID: 5854
	public IHoldableObject leftHandHeldEquipment;

	// Token: 0x040016DF RID: 5855
	public IHoldableObject rightHandHeldEquipment;

	// Token: 0x040016E0 RID: 5856
	public BuilderPieceInteractor builderPieceInteractor;

	// Token: 0x040016E1 RID: 5857
	public GameObject rightHand;

	// Token: 0x040016E2 RID: 5858
	public GameObject leftHand;

	// Token: 0x040016E3 RID: 5859
	public InputDevice leftHandDevice;

	// Token: 0x040016E4 RID: 5860
	public InputDevice rightHandDevice;

	// Token: 0x040016E5 RID: 5861
	public List<InteractionPoint> overlapInteractionPointsLeft = new List<InteractionPoint>();

	// Token: 0x040016E6 RID: 5862
	public List<InteractionPoint> overlapInteractionPointsRight = new List<InteractionPoint>();

	// Token: 0x040016E7 RID: 5863
	public float grabRadius;

	// Token: 0x040016E8 RID: 5864
	public float grabThreshold = 0.7f;

	// Token: 0x040016E9 RID: 5865
	public float grabHysteresis = 0.05f;

	// Token: 0x040016EA RID: 5866
	public bool wasLeftGrabPressed;

	// Token: 0x040016EB RID: 5867
	public bool wasRightGrabPressed;

	// Token: 0x040016EC RID: 5868
	public bool isLeftGrabbing;

	// Token: 0x040016ED RID: 5869
	public bool isRightGrabbing;

	// Token: 0x040016EE RID: 5870
	public bool justReleased;

	// Token: 0x040016EF RID: 5871
	public bool justGrabbed;

	// Token: 0x040016F0 RID: 5872
	public bool disableLeftGrab;

	// Token: 0x040016F1 RID: 5873
	public bool disableRightGrab;

	// Token: 0x040016F2 RID: 5874
	public bool autoGrabLeft;

	// Token: 0x040016F3 RID: 5875
	public bool autoGrabRight;

	// Token: 0x040016F4 RID: 5876
	private float grabValue;

	// Token: 0x040016F5 RID: 5877
	private float tempValue;

	// Token: 0x040016F6 RID: 5878
	private DropZone tempZone;

	// Token: 0x040016F7 RID: 5879
	private bool iteratingInteractionPoints;

	// Token: 0x040016F8 RID: 5880
	private List<InteractionPoint> interactionPointsToRemove = new List<InteractionPoint>();

	// Token: 0x040016F9 RID: 5881
	[SerializeField]
	private GorillaHandClimber bodyClimber;

	// Token: 0x040016FA RID: 5882
	[SerializeField]
	private GorillaHandClimber leftClimber;

	// Token: 0x040016FB RID: 5883
	[SerializeField]
	private GorillaHandClimber rightClimber;
}
