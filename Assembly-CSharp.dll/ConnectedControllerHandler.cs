using System;
using System.Collections;
using System.Collections.Generic;
using GorillaLocomotion;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

// Token: 0x02000502 RID: 1282
internal class ConnectedControllerHandler : MonoBehaviour
{
	// Token: 0x17000325 RID: 805
	// (get) Token: 0x06001F13 RID: 7955 RVA: 0x00043FD3 File Offset: 0x000421D3
	// (set) Token: 0x06001F14 RID: 7956 RVA: 0x00043FDA File Offset: 0x000421DA
	public static ConnectedControllerHandler Instance { get; private set; }

	// Token: 0x17000326 RID: 806
	// (get) Token: 0x06001F15 RID: 7957 RVA: 0x00043FE2 File Offset: 0x000421E2
	public bool RightValid
	{
		get
		{
			return this.rightValid;
		}
	}

	// Token: 0x17000327 RID: 807
	// (get) Token: 0x06001F16 RID: 7958 RVA: 0x00043FEA File Offset: 0x000421EA
	public bool LeftValid
	{
		get
		{
			return this.leftValid;
		}
	}

	// Token: 0x06001F17 RID: 7959 RVA: 0x000EC514 File Offset: 0x000EA714
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

	// Token: 0x06001F18 RID: 7960 RVA: 0x000EC63C File Offset: 0x000EA83C
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

	// Token: 0x06001F19 RID: 7961 RVA: 0x00043FF2 File Offset: 0x000421F2
	private void OnEnable()
	{
		base.StartCoroutine(this.ControllerValidator());
	}

	// Token: 0x06001F1A RID: 7962 RVA: 0x00044001 File Offset: 0x00042201
	private void OnDisable()
	{
		base.StopCoroutine(this.ControllerValidator());
	}

	// Token: 0x06001F1B RID: 7963 RVA: 0x000EC6C4 File Offset: 0x000EA8C4
	private void OnDestroy()
	{
		if (ConnectedControllerHandler.Instance != null && ConnectedControllerHandler.Instance == this)
		{
			ConnectedControllerHandler.Instance = null;
		}
		InputDevices.deviceConnected -= this.DeviceConnected;
		InputDevices.deviceDisconnected -= this.DeviceDisconnected;
	}

	// Token: 0x06001F1C RID: 7964 RVA: 0x0004400F File Offset: 0x0004220F
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

	// Token: 0x06001F1D RID: 7965 RVA: 0x00044037 File Offset: 0x00042237
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

	// Token: 0x06001F1E RID: 7966 RVA: 0x00044046 File Offset: 0x00042246
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

	// Token: 0x06001F1F RID: 7967 RVA: 0x00044084 File Offset: 0x00042284
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

	// Token: 0x06001F20 RID: 7968 RVA: 0x000EC714 File Offset: 0x000EA914
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

	// Token: 0x06001F21 RID: 7969 RVA: 0x000EC7B4 File Offset: 0x000EA9B4
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

	// Token: 0x06001F22 RID: 7970 RVA: 0x000EC818 File Offset: 0x000EAA18
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

	// Token: 0x040022D7 RID: 8919
	[SerializeField]
	private HandTransformFollowOffest rightHandFollower;

	// Token: 0x040022D8 RID: 8920
	[SerializeField]
	private HandTransformFollowOffest leftHandFollower;

	// Token: 0x040022D9 RID: 8921
	[SerializeField]
	private XRController rightXRController;

	// Token: 0x040022DA RID: 8922
	[SerializeField]
	private XRController leftXRController;

	// Token: 0x040022DB RID: 8923
	[SerializeField]
	private GorillaSnapTurn snapTurnController;

	// Token: 0x040022DC RID: 8924
	private List<XRController> rightControllerList;

	// Token: 0x040022DD RID: 8925
	private List<XRController> leftcontrollerList;

	// Token: 0x040022DE RID: 8926
	private const InputDeviceCharacteristics rightCharecteristics = InputDeviceCharacteristics.Controller | InputDeviceCharacteristics.Right;

	// Token: 0x040022DF RID: 8927
	private const InputDeviceCharacteristics leftCharecteristics = InputDeviceCharacteristics.Controller | InputDeviceCharacteristics.Left;

	// Token: 0x040022E0 RID: 8928
	private bool rightControllerValid = true;

	// Token: 0x040022E1 RID: 8929
	private bool leftControllerValid = true;

	// Token: 0x040022E2 RID: 8930
	[SerializeField]
	private bool rightValid = true;

	// Token: 0x040022E3 RID: 8931
	[SerializeField]
	private bool leftValid = true;

	// Token: 0x040022E4 RID: 8932
	[SerializeField]
	private Vector3 lastRightPos;

	// Token: 0x040022E5 RID: 8933
	[SerializeField]
	private Vector3 lastLeftPos;

	// Token: 0x040022E6 RID: 8934
	private Vector3 tempRightPos;

	// Token: 0x040022E7 RID: 8935
	private Vector3 tempLeftPos;

	// Token: 0x040022E8 RID: 8936
	private bool updateControllers;

	// Token: 0x040022E9 RID: 8937
	private GTPlayer playerHandler;

	// Token: 0x040022EA RID: 8938
	[Tooltip("The rate at which controllers are checked to be moving, if they not moving, overrides and enables one hand mode")]
	[SerializeField]
	private float overridePollRate = 15f;

	// Token: 0x040022EB RID: 8939
	[SerializeField]
	private bool overrideEnabled;

	// Token: 0x040022EC RID: 8940
	[SerializeField]
	private OverrideControllers overrideController;
}
