using System;
using UnityEngine;
using UnityEngine.XR;
using Valve.VR;

// Token: 0x02000650 RID: 1616
public class GorillaThrowableController : MonoBehaviour
{
	// Token: 0x0600280A RID: 10250 RVA: 0x0004B3B5 File Offset: 0x000495B5
	protected void Awake()
	{
		this.gorillaThrowableLayerMask = LayerMask.GetMask(new string[]
		{
			"GorillaThrowable"
		});
	}

	// Token: 0x0600280B RID: 10251 RVA: 0x0010F6FC File Offset: 0x0010D8FC
	private void LateUpdate()
	{
		if (this.testCanGrab)
		{
			this.testCanGrab = false;
			this.CanGrabAnObject(this.rightHandController, out this.returnCollider);
			Debug.Log(this.returnCollider.gameObject, this.returnCollider.gameObject);
		}
		if (this.leftHandIsGrabbing)
		{
			if (this.CheckIfHandHasReleased(XRNode.LeftHand))
			{
				if (this.leftHandGrabbedObject != null)
				{
					this.leftHandGrabbedObject.ThrowThisThingo();
					this.leftHandGrabbedObject = null;
				}
				this.leftHandIsGrabbing = false;
			}
		}
		else if (this.CheckIfHandHasGrabbed(XRNode.LeftHand))
		{
			this.leftHandIsGrabbing = true;
			if (this.CanGrabAnObject(this.leftHandController, out this.returnCollider))
			{
				this.leftHandGrabbedObject = this.returnCollider.GetComponent<GorillaThrowable>();
				this.leftHandGrabbedObject.Grabbed(this.leftHandController);
			}
		}
		if (this.rightHandIsGrabbing)
		{
			if (this.CheckIfHandHasReleased(XRNode.RightHand))
			{
				if (this.rightHandGrabbedObject != null)
				{
					this.rightHandGrabbedObject.ThrowThisThingo();
					this.rightHandGrabbedObject = null;
				}
				this.rightHandIsGrabbing = false;
				return;
			}
		}
		else if (this.CheckIfHandHasGrabbed(XRNode.RightHand))
		{
			this.rightHandIsGrabbing = true;
			if (this.CanGrabAnObject(this.rightHandController, out this.returnCollider))
			{
				this.rightHandGrabbedObject = this.returnCollider.GetComponent<GorillaThrowable>();
				this.rightHandGrabbedObject.Grabbed(this.rightHandController);
			}
		}
	}

	// Token: 0x0600280C RID: 10252 RVA: 0x0010F848 File Offset: 0x0010DA48
	private bool CheckIfHandHasReleased(XRNode node)
	{
		this.inputDevice = InputDevices.GetDeviceAtXRNode(node);
		this.triggerValue = ((node == XRNode.LeftHand) ? SteamVR_Actions.gorillaTag_LeftTriggerFloat.GetAxis(SteamVR_Input_Sources.LeftHand) : SteamVR_Actions.gorillaTag_RightTriggerFloat.GetAxis(SteamVR_Input_Sources.RightHand));
		if (this.triggerValue < 0.75f)
		{
			this.triggerValue = ((node == XRNode.LeftHand) ? SteamVR_Actions.gorillaTag_LeftGripFloat.GetAxis(SteamVR_Input_Sources.LeftHand) : SteamVR_Actions.gorillaTag_RightGripFloat.GetAxis(SteamVR_Input_Sources.RightHand));
			if (this.triggerValue < 0.75f)
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x0600280D RID: 10253 RVA: 0x0010F8C4 File Offset: 0x0010DAC4
	private bool CheckIfHandHasGrabbed(XRNode node)
	{
		this.inputDevice = InputDevices.GetDeviceAtXRNode(node);
		this.triggerValue = ((node == XRNode.LeftHand) ? SteamVR_Actions.gorillaTag_LeftTriggerFloat.GetAxis(SteamVR_Input_Sources.LeftHand) : SteamVR_Actions.gorillaTag_RightTriggerFloat.GetAxis(SteamVR_Input_Sources.RightHand));
		if (this.triggerValue > 0.75f)
		{
			return true;
		}
		this.triggerValue = ((node == XRNode.LeftHand) ? SteamVR_Actions.gorillaTag_LeftGripFloat.GetAxis(SteamVR_Input_Sources.LeftHand) : SteamVR_Actions.gorillaTag_RightGripFloat.GetAxis(SteamVR_Input_Sources.RightHand));
		return this.triggerValue > 0.75f;
	}

	// Token: 0x0600280E RID: 10254 RVA: 0x0010F940 File Offset: 0x0010DB40
	private bool CanGrabAnObject(Transform handTransform, out Collider returnCollider)
	{
		this.magnitude = 100f;
		returnCollider = null;
		Debug.Log("trying:");
		if (Physics.OverlapSphereNonAlloc(handTransform.position, this.handRadius, this.colliders, this.gorillaThrowableLayerMask) > 0)
		{
			Debug.Log("found something!");
			this.minCollider = this.colliders[0];
			foreach (Collider collider in this.colliders)
			{
				if (collider != null)
				{
					Debug.Log("found this", collider);
					if ((collider.transform.position - handTransform.position).magnitude < this.magnitude)
					{
						this.minCollider = collider;
						this.magnitude = (collider.transform.position - handTransform.position).magnitude;
					}
				}
			}
			returnCollider = this.minCollider;
			return true;
		}
		return false;
	}

	// Token: 0x0600280F RID: 10255 RVA: 0x0004B3D0 File Offset: 0x000495D0
	public void GrabbableObjectHover(bool isLeft)
	{
		GorillaTagger.Instance.StartVibration(isLeft, this.hoverVibrationStrength, this.hoverVibrationDuration);
	}

	// Token: 0x04002D46 RID: 11590
	public Transform leftHandController;

	// Token: 0x04002D47 RID: 11591
	public Transform rightHandController;

	// Token: 0x04002D48 RID: 11592
	public bool leftHandIsGrabbing;

	// Token: 0x04002D49 RID: 11593
	public bool rightHandIsGrabbing;

	// Token: 0x04002D4A RID: 11594
	public GorillaThrowable leftHandGrabbedObject;

	// Token: 0x04002D4B RID: 11595
	public GorillaThrowable rightHandGrabbedObject;

	// Token: 0x04002D4C RID: 11596
	public float hoverVibrationStrength = 0.25f;

	// Token: 0x04002D4D RID: 11597
	public float hoverVibrationDuration = 0.05f;

	// Token: 0x04002D4E RID: 11598
	public float handRadius = 0.05f;

	// Token: 0x04002D4F RID: 11599
	private InputDevice rightDevice;

	// Token: 0x04002D50 RID: 11600
	private InputDevice leftDevice;

	// Token: 0x04002D51 RID: 11601
	private InputDevice inputDevice;

	// Token: 0x04002D52 RID: 11602
	private float triggerValue;

	// Token: 0x04002D53 RID: 11603
	private bool boolVar;

	// Token: 0x04002D54 RID: 11604
	private Collider[] colliders = new Collider[10];

	// Token: 0x04002D55 RID: 11605
	private Collider minCollider;

	// Token: 0x04002D56 RID: 11606
	private Collider returnCollider;

	// Token: 0x04002D57 RID: 11607
	private float magnitude;

	// Token: 0x04002D58 RID: 11608
	public bool testCanGrab;

	// Token: 0x04002D59 RID: 11609
	private int gorillaThrowableLayerMask;
}
