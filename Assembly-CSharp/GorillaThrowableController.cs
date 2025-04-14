using System;
using UnityEngine;
using UnityEngine.XR;
using Valve.VR;

// Token: 0x02000671 RID: 1649
public class GorillaThrowableController : MonoBehaviour
{
	// Token: 0x060028DF RID: 10463 RVA: 0x000C8BB9 File Offset: 0x000C6DB9
	protected void Awake()
	{
		this.gorillaThrowableLayerMask = LayerMask.GetMask(new string[]
		{
			"GorillaThrowable"
		});
	}

	// Token: 0x060028E0 RID: 10464 RVA: 0x000C8BD4 File Offset: 0x000C6DD4
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

	// Token: 0x060028E1 RID: 10465 RVA: 0x000C8D20 File Offset: 0x000C6F20
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

	// Token: 0x060028E2 RID: 10466 RVA: 0x000C8D9C File Offset: 0x000C6F9C
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

	// Token: 0x060028E3 RID: 10467 RVA: 0x000C8E18 File Offset: 0x000C7018
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

	// Token: 0x060028E4 RID: 10468 RVA: 0x000C8F01 File Offset: 0x000C7101
	public void GrabbableObjectHover(bool isLeft)
	{
		GorillaTagger.Instance.StartVibration(isLeft, this.hoverVibrationStrength, this.hoverVibrationDuration);
	}

	// Token: 0x04002DE0 RID: 11744
	public Transform leftHandController;

	// Token: 0x04002DE1 RID: 11745
	public Transform rightHandController;

	// Token: 0x04002DE2 RID: 11746
	public bool leftHandIsGrabbing;

	// Token: 0x04002DE3 RID: 11747
	public bool rightHandIsGrabbing;

	// Token: 0x04002DE4 RID: 11748
	public GorillaThrowable leftHandGrabbedObject;

	// Token: 0x04002DE5 RID: 11749
	public GorillaThrowable rightHandGrabbedObject;

	// Token: 0x04002DE6 RID: 11750
	public float hoverVibrationStrength = 0.25f;

	// Token: 0x04002DE7 RID: 11751
	public float hoverVibrationDuration = 0.05f;

	// Token: 0x04002DE8 RID: 11752
	public float handRadius = 0.05f;

	// Token: 0x04002DE9 RID: 11753
	private InputDevice rightDevice;

	// Token: 0x04002DEA RID: 11754
	private InputDevice leftDevice;

	// Token: 0x04002DEB RID: 11755
	private InputDevice inputDevice;

	// Token: 0x04002DEC RID: 11756
	private float triggerValue;

	// Token: 0x04002DED RID: 11757
	private bool boolVar;

	// Token: 0x04002DEE RID: 11758
	private Collider[] colliders = new Collider[10];

	// Token: 0x04002DEF RID: 11759
	private Collider minCollider;

	// Token: 0x04002DF0 RID: 11760
	private Collider returnCollider;

	// Token: 0x04002DF1 RID: 11761
	private float magnitude;

	// Token: 0x04002DF2 RID: 11762
	public bool testCanGrab;

	// Token: 0x04002DF3 RID: 11763
	private int gorillaThrowableLayerMask;
}
