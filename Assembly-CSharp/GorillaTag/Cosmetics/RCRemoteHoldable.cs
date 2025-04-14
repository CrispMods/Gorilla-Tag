using System;
using System.Collections.Generic;
using GorillaExtensions;
using GorillaNetworking;
using Photon.Pun;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

namespace GorillaTag.Cosmetics
{
	// Token: 0x02000C25 RID: 3109
	public class RCRemoteHoldable : TransferrableObject, ISnapTurnOverride
	{
		// Token: 0x17000808 RID: 2056
		// (get) Token: 0x06004D87 RID: 19847 RVA: 0x0017BDF0 File Offset: 0x00179FF0
		public XRNode XRNode
		{
			get
			{
				return this.xrNode;
			}
		}

		// Token: 0x17000809 RID: 2057
		// (get) Token: 0x06004D88 RID: 19848 RVA: 0x0017BDF8 File Offset: 0x00179FF8
		public RCVehicle Vehicle
		{
			get
			{
				return this.targetVehicle;
			}
		}

		// Token: 0x06004D89 RID: 19849 RVA: 0x0017BE00 File Offset: 0x0017A000
		public bool TurnOverrideActive()
		{
			return base.gameObject.activeSelf && this.currentlyHeld && this.xrNode == XRNode.RightHand;
		}

		// Token: 0x06004D8A RID: 19850 RVA: 0x0017BE24 File Offset: 0x0017A024
		protected override void Awake()
		{
			base.Awake();
			this.initialJoystickRotation = this.joystickTransform.localRotation;
			this.initialTriggerRotation = this.triggerTransform.localRotation;
			if (this.buttonTransform != null)
			{
				this.initialButtonRotation = this.buttonTransform.localRotation;
				this.initialButtonPosition = this.buttonTransform.localPosition;
			}
		}

		// Token: 0x06004D8B RID: 19851 RVA: 0x0017BE8C File Offset: 0x0017A08C
		internal override void OnEnable()
		{
			base.OnEnable();
			if (!this._TryFindRemoteVehicle())
			{
				base.gameObject.SetActive(false);
				return;
			}
			if (this._events.IsNotNull() || base.gameObject.TryGetComponent<RubberDuckEvents>(out this._events))
			{
				this._events = base.gameObject.GetOrAddComponent<RubberDuckEvents>();
				NetPlayer netPlayer = (base.myOnlineRig != null) ? base.myOnlineRig.creator : ((base.myRig != null) ? ((base.myRig.creator != null) ? base.myRig.creator : NetworkSystem.Instance.LocalPlayer) : null);
				if (netPlayer != null)
				{
					this._events.Init(netPlayer);
				}
				else
				{
					Debug.LogError("Failed to get a reference to the Photon Player needed to hook up the cosmetic event");
				}
				this._events.Activate += this.OnStartConnectionEvent;
			}
			this.WakeUpRemoteVehicle();
		}

		// Token: 0x06004D8C RID: 19852 RVA: 0x0017BF7C File Offset: 0x0017A17C
		internal override void OnDisable()
		{
			base.OnDisable();
			GorillaSnapTurn gorillaSnapTurn = (GorillaTagger.Instance != null) ? GorillaTagger.Instance.GetComponent<GorillaSnapTurn>() : null;
			if (gorillaSnapTurn != null)
			{
				gorillaSnapTurn.UnsetTurningOverride(this);
			}
			if (this.networkSync != null && this.networkSync.photonView.IsMine)
			{
				PhotonNetwork.Destroy(this.networkSync.gameObject);
				this.networkSync = null;
			}
			if (this._events.IsNotNull())
			{
				this._events.Activate -= this.OnStartConnectionEvent;
			}
		}

		// Token: 0x06004D8D RID: 19853 RVA: 0x0017C020 File Offset: 0x0017A220
		protected override void OnDestroy()
		{
			base.OnDestroy();
			GorillaSnapTurn gorillaSnapTurn = (GorillaTagger.Instance != null) ? GorillaTagger.Instance.GetComponent<GorillaSnapTurn>() : null;
			if (gorillaSnapTurn != null)
			{
				gorillaSnapTurn.UnsetTurningOverride(this);
			}
		}

		// Token: 0x06004D8E RID: 19854 RVA: 0x0017C060 File Offset: 0x0017A260
		public override void OnGrab(InteractionPoint pointGrabbed, GameObject grabbingHand)
		{
			base.OnGrab(pointGrabbed, grabbingHand);
			if (PhotonNetwork.InRoom && this.networkSync != null && this.networkSync.photonView.Owner == null)
			{
				PhotonNetwork.Destroy(this.networkSync.gameObject);
				this.networkSync = null;
			}
			if (this.networkSync == null && PhotonNetwork.InRoom)
			{
				object[] data = new object[]
				{
					this.myIndex
				};
				GameObject gameObject = PhotonNetwork.Instantiate(this.networkSyncPrefabName, Vector3.zero, Quaternion.identity, 0, data);
				this.networkSync = ((gameObject != null) ? gameObject.GetComponent<RCCosmeticNetworkSync>() : null);
			}
			this.currentlyHeld = true;
			bool flag = grabbingHand == EquipmentInteractor.instance.rightHand;
			this.xrNode = (flag ? XRNode.RightHand : XRNode.LeftHand);
			GorillaSnapTurn component = GorillaTagger.Instance.GetComponent<GorillaSnapTurn>();
			if (flag)
			{
				component.SetTurningOverride(this);
			}
			else
			{
				component.UnsetTurningOverride(this);
			}
			if (this.targetVehicle != null)
			{
				this.targetVehicle.StartConnection(this, this.networkSync);
			}
			if (PhotonNetwork.InRoom && this._events != null && this._events.Activate != null)
			{
				this._events.Activate.RaiseOthers(this.emptyArgs);
			}
		}

		// Token: 0x06004D8F RID: 19855 RVA: 0x0017C1B0 File Offset: 0x0017A3B0
		public override bool OnRelease(DropZone zoneReleased, GameObject releasingHand)
		{
			if (!base.OnRelease(zoneReleased, releasingHand))
			{
				return false;
			}
			this.currentlyHeld = false;
			this.currentInput = default(RCRemoteHoldable.RCInput);
			if (this.targetVehicle != null)
			{
				this.targetVehicle.EndConnection();
			}
			this.joystickTransform.localRotation = this.initialJoystickRotation;
			this.triggerTransform.localRotation = this.initialTriggerRotation;
			GorillaTagger.Instance.GetComponent<GorillaSnapTurn>().UnsetTurningOverride(this);
			return true;
		}

		// Token: 0x06004D90 RID: 19856 RVA: 0x0017C228 File Offset: 0x0017A428
		private void Update()
		{
			if (this.currentlyHeld)
			{
				this.currentInput.joystick = ControllerInputPoller.Primary2DAxis(this.xrNode);
				this.currentInput.trigger = ControllerInputPoller.TriggerFloat(this.xrNode);
				this.currentInput.buttons = (ControllerInputPoller.PrimaryButtonPress(this.xrNode) ? 1 : 0);
				if (this.targetVehicle != null)
				{
					this.targetVehicle.ApplyRemoteControlInput(this.currentInput);
				}
				this.joystickTransform.localRotation = this.initialJoystickRotation * Quaternion.Euler(this.joystickLeanDegrees * this.currentInput.joystick.y, 0f, -this.joystickLeanDegrees * this.currentInput.joystick.x);
				this.triggerTransform.localRotation = this.initialTriggerRotation * Quaternion.Euler(this.triggerPullDegrees * this.currentInput.trigger, 0f, 0f);
				if (this.buttonTransform != null)
				{
					this.buttonTransform.localPosition = this.initialButtonPosition + this.initialButtonRotation * new Vector3(0f, 0f, -this.buttonPressDepth * (float)((this.currentInput.buttons > 0) ? 1 : 0));
				}
			}
		}

		// Token: 0x06004D91 RID: 19857 RVA: 0x0017C387 File Offset: 0x0017A587
		public void OnStartConnectionEvent(int sender, int target, object[] args, PhotonMessageInfoWrapped info)
		{
			if (sender != target)
			{
				return;
			}
			if (info.senderID != this.ownerRig.creator.ActorNumber)
			{
				return;
			}
			this.WakeUpRemoteVehicle();
		}

		// Token: 0x06004D92 RID: 19858 RVA: 0x0017C3AE File Offset: 0x0017A5AE
		public void WakeUpRemoteVehicle()
		{
			if (this.networkSync != null && this.targetVehicle.IsNotNull() && !this.targetVehicle.HasLocalAuthority)
			{
				this.targetVehicle.WakeUpRemote(this.networkSync);
			}
		}

		// Token: 0x06004D93 RID: 19859 RVA: 0x0017C3EC File Offset: 0x0017A5EC
		private bool _TryFindRemoteVehicle()
		{
			if (this.targetVehicle != null)
			{
				return true;
			}
			VRRig componentInParent = base.GetComponentInParent<VRRig>(true);
			if (componentInParent.IsNull())
			{
				Debug.LogError("RCRemoteHoldable: unable to find parent vrrig");
				return false;
			}
			CosmeticItemInstance cosmeticItemInstance = componentInParent.cosmeticsObjectRegistry.Cosmetic(base.name);
			int instanceID = base.gameObject.GetInstanceID();
			return this._TryFindRemoteVehicle_InCosmeticInstanceArray(instanceID, cosmeticItemInstance.objects) || this._TryFindRemoteVehicle_InCosmeticInstanceArray(instanceID, cosmeticItemInstance.leftObjects) || this._TryFindRemoteVehicle_InCosmeticInstanceArray(instanceID, cosmeticItemInstance.rightObjects);
		}

		// Token: 0x06004D94 RID: 19860 RVA: 0x0017C478 File Offset: 0x0017A678
		private bool _TryFindRemoteVehicle_InCosmeticInstanceArray(int thisGobjInstId, List<GameObject> gameObjects)
		{
			foreach (GameObject gameObject in gameObjects)
			{
				if (gameObject.GetInstanceID() != thisGobjInstId)
				{
					this.targetVehicle = gameObject.GetComponentInChildren<RCVehicle>(true);
					if (this.targetVehicle != null)
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x04005087 RID: 20615
		[SerializeField]
		private Transform joystickTransform;

		// Token: 0x04005088 RID: 20616
		[SerializeField]
		private Transform triggerTransform;

		// Token: 0x04005089 RID: 20617
		[SerializeField]
		private Transform buttonTransform;

		// Token: 0x0400508A RID: 20618
		private RCVehicle targetVehicle;

		// Token: 0x0400508B RID: 20619
		private float joystickLeanDegrees = 30f;

		// Token: 0x0400508C RID: 20620
		private float triggerPullDegrees = 40f;

		// Token: 0x0400508D RID: 20621
		private float buttonPressDepth = 0.005f;

		// Token: 0x0400508E RID: 20622
		private Quaternion initialJoystickRotation;

		// Token: 0x0400508F RID: 20623
		private Quaternion initialTriggerRotation;

		// Token: 0x04005090 RID: 20624
		private Quaternion initialButtonRotation;

		// Token: 0x04005091 RID: 20625
		private Vector3 initialButtonPosition;

		// Token: 0x04005092 RID: 20626
		private bool currentlyHeld;

		// Token: 0x04005093 RID: 20627
		private XRNode xrNode;

		// Token: 0x04005094 RID: 20628
		private RCRemoteHoldable.RCInput currentInput;

		// Token: 0x04005095 RID: 20629
		[HideInInspector]
		public RCCosmeticNetworkSync networkSync;

		// Token: 0x04005096 RID: 20630
		private string networkSyncPrefabName = "RCCosmeticNetworkSync";

		// Token: 0x04005097 RID: 20631
		private RubberDuckEvents _events;

		// Token: 0x04005098 RID: 20632
		private object[] emptyArgs = new object[0];

		// Token: 0x02000C26 RID: 3110
		public struct RCInput
		{
			// Token: 0x04005099 RID: 20633
			public Vector2 joystick;

			// Token: 0x0400509A RID: 20634
			public float trigger;

			// Token: 0x0400509B RID: 20635
			public byte buttons;
		}
	}
}
