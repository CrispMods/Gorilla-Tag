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
	// Token: 0x02000C53 RID: 3155
	public class RCRemoteHoldable : TransferrableObject, ISnapTurnOverride
	{
		// Token: 0x17000826 RID: 2086
		// (get) Token: 0x06004ED8 RID: 20184 RVA: 0x000637EE File Offset: 0x000619EE
		public XRNode XRNode
		{
			get
			{
				return this.xrNode;
			}
		}

		// Token: 0x17000827 RID: 2087
		// (get) Token: 0x06004ED9 RID: 20185 RVA: 0x000637F6 File Offset: 0x000619F6
		public RCVehicle Vehicle
		{
			get
			{
				return this.targetVehicle;
			}
		}

		// Token: 0x06004EDA RID: 20186 RVA: 0x000637FE File Offset: 0x000619FE
		public bool TurnOverrideActive()
		{
			return base.gameObject.activeSelf && this.currentlyHeld && this.xrNode == XRNode.RightHand;
		}

		// Token: 0x06004EDB RID: 20187 RVA: 0x001B44E0 File Offset: 0x001B26E0
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

		// Token: 0x06004EDC RID: 20188 RVA: 0x001B4548 File Offset: 0x001B2748
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

		// Token: 0x06004EDD RID: 20189 RVA: 0x001B4638 File Offset: 0x001B2838
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

		// Token: 0x06004EDE RID: 20190 RVA: 0x001B46DC File Offset: 0x001B28DC
		protected override void OnDestroy()
		{
			base.OnDestroy();
			GorillaSnapTurn gorillaSnapTurn = (GorillaTagger.Instance != null) ? GorillaTagger.Instance.GetComponent<GorillaSnapTurn>() : null;
			if (gorillaSnapTurn != null)
			{
				gorillaSnapTurn.UnsetTurningOverride(this);
			}
		}

		// Token: 0x06004EDF RID: 20191 RVA: 0x001B471C File Offset: 0x001B291C
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

		// Token: 0x06004EE0 RID: 20192 RVA: 0x001B486C File Offset: 0x001B2A6C
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

		// Token: 0x06004EE1 RID: 20193 RVA: 0x001B48E4 File Offset: 0x001B2AE4
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

		// Token: 0x06004EE2 RID: 20194 RVA: 0x00063820 File Offset: 0x00061A20
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

		// Token: 0x06004EE3 RID: 20195 RVA: 0x00063847 File Offset: 0x00061A47
		public void WakeUpRemoteVehicle()
		{
			if (this.networkSync != null && this.targetVehicle.IsNotNull() && !this.targetVehicle.HasLocalAuthority)
			{
				this.targetVehicle.WakeUpRemote(this.networkSync);
			}
		}

		// Token: 0x06004EE4 RID: 20196 RVA: 0x001B4A44 File Offset: 0x001B2C44
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

		// Token: 0x06004EE5 RID: 20197 RVA: 0x001B4AD0 File Offset: 0x001B2CD0
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

		// Token: 0x0400517D RID: 20861
		[SerializeField]
		private Transform joystickTransform;

		// Token: 0x0400517E RID: 20862
		[SerializeField]
		private Transform triggerTransform;

		// Token: 0x0400517F RID: 20863
		[SerializeField]
		private Transform buttonTransform;

		// Token: 0x04005180 RID: 20864
		private RCVehicle targetVehicle;

		// Token: 0x04005181 RID: 20865
		private float joystickLeanDegrees = 30f;

		// Token: 0x04005182 RID: 20866
		private float triggerPullDegrees = 40f;

		// Token: 0x04005183 RID: 20867
		private float buttonPressDepth = 0.005f;

		// Token: 0x04005184 RID: 20868
		private Quaternion initialJoystickRotation;

		// Token: 0x04005185 RID: 20869
		private Quaternion initialTriggerRotation;

		// Token: 0x04005186 RID: 20870
		private Quaternion initialButtonRotation;

		// Token: 0x04005187 RID: 20871
		private Vector3 initialButtonPosition;

		// Token: 0x04005188 RID: 20872
		private bool currentlyHeld;

		// Token: 0x04005189 RID: 20873
		private XRNode xrNode;

		// Token: 0x0400518A RID: 20874
		private RCRemoteHoldable.RCInput currentInput;

		// Token: 0x0400518B RID: 20875
		[HideInInspector]
		public RCCosmeticNetworkSync networkSync;

		// Token: 0x0400518C RID: 20876
		private string networkSyncPrefabName = "RCCosmeticNetworkSync";

		// Token: 0x0400518D RID: 20877
		private RubberDuckEvents _events;

		// Token: 0x0400518E RID: 20878
		private object[] emptyArgs = new object[0];

		// Token: 0x02000C54 RID: 3156
		public struct RCInput
		{
			// Token: 0x0400518F RID: 20879
			public Vector2 joystick;

			// Token: 0x04005190 RID: 20880
			public float trigger;

			// Token: 0x04005191 RID: 20881
			public byte buttons;
		}
	}
}
