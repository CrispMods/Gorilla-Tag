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
	// Token: 0x02000C28 RID: 3112
	public class RCRemoteHoldable : TransferrableObject, ISnapTurnOverride
	{
		// Token: 0x17000809 RID: 2057
		// (get) Token: 0x06004D93 RID: 19859 RVA: 0x00061E2D File Offset: 0x0006002D
		public XRNode XRNode
		{
			get
			{
				return this.xrNode;
			}
		}

		// Token: 0x1700080A RID: 2058
		// (get) Token: 0x06004D94 RID: 19860 RVA: 0x00061E35 File Offset: 0x00060035
		public RCVehicle Vehicle
		{
			get
			{
				return this.targetVehicle;
			}
		}

		// Token: 0x06004D95 RID: 19861 RVA: 0x00061E3D File Offset: 0x0006003D
		public bool TurnOverrideActive()
		{
			return base.gameObject.activeSelf && this.currentlyHeld && this.xrNode == XRNode.RightHand;
		}

		// Token: 0x06004D96 RID: 19862 RVA: 0x001ACBF4 File Offset: 0x001AADF4
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

		// Token: 0x06004D97 RID: 19863 RVA: 0x001ACC5C File Offset: 0x001AAE5C
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

		// Token: 0x06004D98 RID: 19864 RVA: 0x001ACD4C File Offset: 0x001AAF4C
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

		// Token: 0x06004D99 RID: 19865 RVA: 0x001ACDF0 File Offset: 0x001AAFF0
		protected override void OnDestroy()
		{
			base.OnDestroy();
			GorillaSnapTurn gorillaSnapTurn = (GorillaTagger.Instance != null) ? GorillaTagger.Instance.GetComponent<GorillaSnapTurn>() : null;
			if (gorillaSnapTurn != null)
			{
				gorillaSnapTurn.UnsetTurningOverride(this);
			}
		}

		// Token: 0x06004D9A RID: 19866 RVA: 0x001ACE30 File Offset: 0x001AB030
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

		// Token: 0x06004D9B RID: 19867 RVA: 0x001ACF80 File Offset: 0x001AB180
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

		// Token: 0x06004D9C RID: 19868 RVA: 0x001ACFF8 File Offset: 0x001AB1F8
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

		// Token: 0x06004D9D RID: 19869 RVA: 0x00061E5F File Offset: 0x0006005F
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

		// Token: 0x06004D9E RID: 19870 RVA: 0x00061E86 File Offset: 0x00060086
		public void WakeUpRemoteVehicle()
		{
			if (this.networkSync != null && this.targetVehicle.IsNotNull() && !this.targetVehicle.HasLocalAuthority)
			{
				this.targetVehicle.WakeUpRemote(this.networkSync);
			}
		}

		// Token: 0x06004D9F RID: 19871 RVA: 0x001AD158 File Offset: 0x001AB358
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

		// Token: 0x06004DA0 RID: 19872 RVA: 0x001AD1E4 File Offset: 0x001AB3E4
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

		// Token: 0x04005099 RID: 20633
		[SerializeField]
		private Transform joystickTransform;

		// Token: 0x0400509A RID: 20634
		[SerializeField]
		private Transform triggerTransform;

		// Token: 0x0400509B RID: 20635
		[SerializeField]
		private Transform buttonTransform;

		// Token: 0x0400509C RID: 20636
		private RCVehicle targetVehicle;

		// Token: 0x0400509D RID: 20637
		private float joystickLeanDegrees = 30f;

		// Token: 0x0400509E RID: 20638
		private float triggerPullDegrees = 40f;

		// Token: 0x0400509F RID: 20639
		private float buttonPressDepth = 0.005f;

		// Token: 0x040050A0 RID: 20640
		private Quaternion initialJoystickRotation;

		// Token: 0x040050A1 RID: 20641
		private Quaternion initialTriggerRotation;

		// Token: 0x040050A2 RID: 20642
		private Quaternion initialButtonRotation;

		// Token: 0x040050A3 RID: 20643
		private Vector3 initialButtonPosition;

		// Token: 0x040050A4 RID: 20644
		private bool currentlyHeld;

		// Token: 0x040050A5 RID: 20645
		private XRNode xrNode;

		// Token: 0x040050A6 RID: 20646
		private RCRemoteHoldable.RCInput currentInput;

		// Token: 0x040050A7 RID: 20647
		[HideInInspector]
		public RCCosmeticNetworkSync networkSync;

		// Token: 0x040050A8 RID: 20648
		private string networkSyncPrefabName = "RCCosmeticNetworkSync";

		// Token: 0x040050A9 RID: 20649
		private RubberDuckEvents _events;

		// Token: 0x040050AA RID: 20650
		private object[] emptyArgs = new object[0];

		// Token: 0x02000C29 RID: 3113
		public struct RCInput
		{
			// Token: 0x040050AB RID: 20651
			public Vector2 joystick;

			// Token: 0x040050AC RID: 20652
			public float trigger;

			// Token: 0x040050AD RID: 20653
			public byte buttons;
		}
	}
}
