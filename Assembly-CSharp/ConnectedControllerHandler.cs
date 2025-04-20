using System;
using System.Collections;
using System.Collections.Generic;
using GorillaLocomotion;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

// Token: 0x0200050F RID: 1295
internal class ConnectedControllerHandler : MonoBehaviour
{
	// Token: 0x1700032C RID: 812
	// (get) Token: 0x06001F69 RID: 8041 RVA: 0x00045372 File Offset: 0x00043572
	// (set) Token: 0x06001F6A RID: 8042 RVA: 0x00045379 File Offset: 0x00043579
	public static ConnectedControllerHandler Instance { get; private set; }

	// Token: 0x1700032D RID: 813
	// (get) Token: 0x06001F6B RID: 8043 RVA: 0x00045381 File Offset: 0x00043581
	public bool RightValid
	{
		get
		{
			return this.rightValid;
		}
	}

	// Token: 0x1700032E RID: 814
	// (get) Token: 0x06001F6C RID: 8044 RVA: 0x00045389 File Offset: 0x00043589
	public bool LeftValid
	{
		get
		{
			return this.leftValid;
		}
	}

	// Token: 0x06001F6D RID: 8045 RVA: 0x000EF250 File Offset: 0x000ED450
	private void Awake()
	{
		if (ConnectedControllerHandler.Instance != null && ConnectedControllerHandler.Instance != this)
		{
			UnityEngine.Object.Destroy(this);
			return;
		}
		ConnectedControllerHandler.Instance = this;
		if (this.leftHandFollower == null || this.rightHandFollower == null || this.rightXRController == null || this.leftXRController == null || this.snapTurnController == null)
		{
			base.enabled = false;
			return;
		}
		this.rightControllerList = new List<XRController>();
		this.leftcontrollerList = new List<XRController>();
		this.rightControllerList.Add(this.rightXRController);
		this.leftcontrollerList.Add(this.leftXRController);
		InputDevice deviceAtXRNode = InputDevices.GetDeviceAtXRNode(XRNode.RightHand);
		InputDevice deviceAtXRNode2 = InputDevices.GetDeviceAtXRNode(XRNode.LeftHand);
		Debug.Log(string.Format("right controller? {0}", (deviceAtXRNode.characteristics & (InputDeviceCharacteristics.Controller | InputDeviceCharacteristics.Right)) == (InputDeviceCharacteristics.Controller | InputDeviceCharacteristics.Right)));
		this.rightControllerValid = deviceAtXRNode.isValid;
		this.leftControllerValid = deviceAtXRNode2.isValid;
		InputDevices.deviceConnected += this.DeviceConnected;
		InputDevices.deviceDisconnected += this.DeviceDisconnected;
		this.UpdateControllerStates();
	}

	// Token: 0x06001F6E RID: 8046 RVA: 0x000EF378 File Offset: 0x000ED578
	private void Start()
	{
		if (this.leftHandFollower == null || this.rightHandFollower == null || this.leftXRController == null || this.rightXRController == null || this.snapTurnController == null)
		{
			return;
		}
		this.playerHandler = GTPlayer.Instance;
		this.rightHandFollower.followTransform = GorillaTagger.Instance.offlineVRRig.transform;
		this.leftHandFollower.followTransform = GorillaTagger.Instance.offlineVRRig.transform;
	}

	// Token: 0x06001F6F RID: 8047 RVA: 0x00045391 File Offset: 0x00043591
	private void OnEnable()
	{
		base.StartCoroutine(this.ControllerValidator());
	}

	// Token: 0x06001F70 RID: 8048 RVA: 0x000453A0 File Offset: 0x000435A0
	private void OnDisable()
	{
		base.StopCoroutine(this.ControllerValidator());
	}

	// Token: 0x06001F71 RID: 8049 RVA: 0x000EF400 File Offset: 0x000ED600
	private void OnDestroy()
	{
		if (ConnectedControllerHandler.Instance != null && ConnectedControllerHandler.Instance == this)
		{
			ConnectedControllerHandler.Instance = null;
		}
		InputDevices.deviceConnected -= this.DeviceConnected;
		InputDevices.deviceDisconnected -= this.DeviceDisconnected;
	}

	// Token: 0x06001F72 RID: 8050 RVA: 0x000453AE File Offset: 0x000435AE
	private void LateUpdate()
	{
		if (!this.rightValid)
		{
			this.rightHandFollower.UpdatePositionRotation();
		}
		if (!this.leftValid)
		{
			this.leftHandFollower.UpdatePositionRotation();
		}
	}

	// Token: 0x06001F73 RID: 8051 RVA: 0x000453D6 File Offset: 0x000435D6
	private IEnumerator ControllerValidator()
	{
		yield return null;
		this.lastRightPos = ControllerInputPoller.DevicePosition(XRNode.RightHand);
		this.lastLeftPos = ControllerInputPoller.DevicePosition(XRNode.LeftHand);
		for (;;)
		{
			yield return new WaitForSeconds(this.overridePollRate);
			this.updateControllers = false;
			if (!this.playerHandler.inOverlay)
			{
				if (this.rightControllerValid)
				{
					this.tempRightPos = ControllerInputPoller.DevicePosition(XRNode.RightHand);
					if (this.tempRightPos == this.lastRightPos)
					{
						if ((this.overrideController & OverrideControllers.RightController) != OverrideControllers.RightController)
						{
							this.overrideController |= OverrideControllers.RightController;
							this.updateControllers = true;
						}
					}
					else if ((this.overrideController & OverrideControllers.RightController) == OverrideControllers.RightController)
					{
						this.overrideController &= ~OverrideControllers.RightController;
						this.updateControllers = true;
					}
					this.lastRightPos = this.tempRightPos;
				}
				if (this.leftControllerValid)
				{
					this.tempLeftPos = ControllerInputPoller.DevicePosition(XRNode.LeftHand);
					if (this.tempLeftPos == this.lastLeftPos)
					{
						if ((this.overrideController & OverrideControllers.LeftController) != OverrideControllers.LeftController)
						{
							this.overrideController |= OverrideControllers.LeftController;
							this.updateControllers = true;
						}
					}
					else if ((this.overrideController & OverrideControllers.LeftController) == OverrideControllers.LeftController)
					{
						this.overrideController &= ~OverrideControllers.LeftController;
						this.updateControllers = true;
					}
					this.lastLeftPos = this.tempLeftPos;
				}
				if (this.updateControllers)
				{
					this.overrideEnabled = (this.overrideController > OverrideControllers.None);
					this.UpdateControllerStates();
				}
			}
		}
		yield break;
	}

	// Token: 0x06001F74 RID: 8052 RVA: 0x000453E5 File Offset: 0x000435E5
	private void DeviceDisconnected(InputDevice device)
	{
		if ((device.characteristics & (InputDeviceCharacteristics.Controller | InputDeviceCharacteristics.Right)) == (InputDeviceCharacteristics.Controller | InputDeviceCharacteristics.Right))
		{
			this.rightControllerValid = false;
		}
		if ((device.characteristics & (InputDeviceCharacteristics.Controller | InputDeviceCharacteristics.Left)) == (InputDeviceCharacteristics.Controller | InputDeviceCharacteristics.Left))
		{
			this.leftControllerValid = false;
		}
		this.UpdateControllerStates();
	}

	// Token: 0x06001F75 RID: 8053 RVA: 0x00045423 File Offset: 0x00043623
	private void DeviceConnected(InputDevice device)
	{
		if ((device.characteristics & (InputDeviceCharacteristics.Controller | InputDeviceCharacteristics.Right)) == (InputDeviceCharacteristics.Controller | InputDeviceCharacteristics.Right))
		{
			this.rightControllerValid = true;
		}
		if ((device.characteristics & (InputDeviceCharacteristics.Controller | InputDeviceCharacteristics.Left)) == (InputDeviceCharacteristics.Controller | InputDeviceCharacteristics.Left))
		{
			this.leftControllerValid = true;
		}
		this.UpdateControllerStates();
	}

	// Token: 0x06001F76 RID: 8054 RVA: 0x000EF450 File Offset: 0x000ED650
	private void UpdateControllerStates()
	{
		if (this.overrideEnabled && this.overrideController != OverrideControllers.None)
		{
			this.rightValid = (this.rightControllerValid && (this.overrideController & OverrideControllers.RightController) != OverrideControllers.RightController);
			this.leftValid = (this.leftControllerValid && (this.overrideController & OverrideControllers.LeftController) != OverrideControllers.LeftController);
		}
		else
		{
			this.rightValid = this.rightControllerValid;
			this.leftValid = this.leftControllerValid;
		}
		this.rightXRController.enabled = this.rightValid;
		this.leftXRController.enabled = this.leftValid;
		this.AssignSnapturnController();
	}

	// Token: 0x06001F77 RID: 8055 RVA: 0x000EF4F0 File Offset: 0x000ED6F0
	private void AssignSnapturnController()
	{
		if (!this.leftValid && this.rightValid)
		{
			this.snapTurnController.controllers = this.rightControllerList;
			return;
		}
		if (!this.rightValid && this.leftValid)
		{
			this.snapTurnController.controllers = this.leftcontrollerList;
			return;
		}
		this.snapTurnController.controllers = this.rightControllerList;
	}

	// Token: 0x06001F78 RID: 8056 RVA: 0x000EF554 File Offset: 0x000ED754
	public bool GetValidForXRNode(XRNode controllerNode)
	{
		bool result;
		if (controllerNode != XRNode.LeftHand)
		{
			result = (controllerNode != XRNode.RightHand || this.rightValid);
		}
		else
		{
			result = this.leftValid;
		}
		return result;
	}

	// Token: 0x04002329 RID: 9001
	[SerializeField]
	private HandTransformFollowOffest rightHandFollower;

	// Token: 0x0400232A RID: 9002
	[SerializeField]
	private HandTransformFollowOffest leftHandFollower;

	// Token: 0x0400232B RID: 9003
	[SerializeField]
	private XRController rightXRController;

	// Token: 0x0400232C RID: 9004
	[SerializeField]
	private XRController leftXRController;

	// Token: 0x0400232D RID: 9005
	[SerializeField]
	private GorillaSnapTurn snapTurnController;

	// Token: 0x0400232E RID: 9006
	private List<XRController> rightControllerList;

	// Token: 0x0400232F RID: 9007
	private List<XRController> leftcontrollerList;

	// Token: 0x04002330 RID: 9008
	private const InputDeviceCharacteristics rightCharecteristics = InputDeviceCharacteristics.Controller | InputDeviceCharacteristics.Right;

	// Token: 0x04002331 RID: 9009
	private const InputDeviceCharacteristics leftCharecteristics = InputDeviceCharacteristics.Controller | InputDeviceCharacteristics.Left;

	// Token: 0x04002332 RID: 9010
	private bool rightControllerValid = true;

	// Token: 0x04002333 RID: 9011
	private bool leftControllerValid = true;

	// Token: 0x04002334 RID: 9012
	[SerializeField]
	private bool rightValid = true;

	// Token: 0x04002335 RID: 9013
	[SerializeField]
	private bool leftValid = true;

	// Token: 0x04002336 RID: 9014
	[SerializeField]
	private Vector3 lastRightPos;

	// Token: 0x04002337 RID: 9015
	[SerializeField]
	private Vector3 lastLeftPos;

	// Token: 0x04002338 RID: 9016
	private Vector3 tempRightPos;

	// Token: 0x04002339 RID: 9017
	private Vector3 tempLeftPos;

	// Token: 0x0400233A RID: 9018
	private bool updateControllers;

	// Token: 0x0400233B RID: 9019
	private GTPlayer playerHandler;

	// Token: 0x0400233C RID: 9020
	[Tooltip("The rate at which controllers are checked to be moving, if they not moving, overrides and enables one hand mode")]
	[SerializeField]
	private float overridePollRate = 15f;

	// Token: 0x0400233D RID: 9021
	[SerializeField]
	private bool overrideEnabled;

	// Token: 0x0400233E RID: 9022
	[SerializeField]
	private OverrideControllers overrideController;
}
