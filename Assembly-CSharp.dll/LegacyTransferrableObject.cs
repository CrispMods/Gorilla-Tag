﻿using System;
using Photon.Pun;
using UnityEngine;
using UnityEngine.XR;

// Token: 0x020003E5 RID: 997
public class LegacyTransferrableObject : HoldableObject
{
	// Token: 0x06001831 RID: 6193 RVA: 0x0003F653 File Offset: 0x0003D853
	protected void Awake()
	{
		this.latched = false;
		this.initOffset = base.transform.localPosition;
		this.initRotation = base.transform.localRotation;
	}

	// Token: 0x06001832 RID: 6194 RVA: 0x000C96A0 File Offset: 0x000C78A0
	protected virtual void Start()
	{
		RoomSystem.JoinedRoomEvent = (Action)Delegate.Combine(RoomSystem.JoinedRoomEvent, new Action(this.OnJoinedRoom));
		RoomSystem.LeftRoomEvent = (Action)Delegate.Combine(RoomSystem.LeftRoomEvent, new Action(this.OnLeftRoom));
		RoomSystem.PlayerJoinedEvent = (Action<NetPlayer>)Delegate.Combine(RoomSystem.PlayerJoinedEvent, new Action<NetPlayer>(this.OnPlayerLeftRoom));
	}

	// Token: 0x06001833 RID: 6195 RVA: 0x000C9710 File Offset: 0x000C7910
	public void OnEnable()
	{
		if (this.myRig == null && this.myOnlineRig != null && this.myOnlineRig.netView != null && this.myOnlineRig.netView.IsMine)
		{
			base.gameObject.SetActive(false);
			return;
		}
		if (this.myRig == null && this.myOnlineRig == null)
		{
			base.gameObject.SetActive(false);
			return;
		}
		this.objectIndex = this.targetDock.ReturnTransferrableItemIndex(this.myIndex);
		if (this.myRig != null && this.myRig.isOfflineVRRig)
		{
			if (this.currentState == TransferrableObject.PositionState.OnLeftArm)
			{
				this.storedZone = BodyDockPositions.DropPositions.LeftArm;
			}
			else if (this.currentState == TransferrableObject.PositionState.OnRightArm)
			{
				this.storedZone = BodyDockPositions.DropPositions.RightArm;
			}
			else if (this.currentState == TransferrableObject.PositionState.OnLeftShoulder)
			{
				this.storedZone = BodyDockPositions.DropPositions.LeftBack;
			}
			else if (this.currentState == TransferrableObject.PositionState.OnRightShoulder)
			{
				this.storedZone = BodyDockPositions.DropPositions.RightBack;
			}
			else
			{
				this.storedZone = BodyDockPositions.DropPositions.Chest;
			}
		}
		if (this.objectIndex == -1)
		{
			base.gameObject.SetActive(false);
			return;
		}
		if (this.currentState == TransferrableObject.PositionState.OnLeftArm && this.flipOnXForLeftArm)
		{
			Transform transform = this.GetAnchor(this.currentState);
			transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
		}
		this.initState = this.currentState;
		this.enabledOnFrame = Time.frameCount;
		this.SpawnShareableObject();
	}

	// Token: 0x06001834 RID: 6196 RVA: 0x0003F67E File Offset: 0x0003D87E
	public void OnDisable()
	{
		this.enabledOnFrame = -1;
	}

	// Token: 0x06001835 RID: 6197 RVA: 0x000C9898 File Offset: 0x000C7A98
	private void SpawnShareableObject()
	{
		if (!PhotonNetwork.InRoom)
		{
			return;
		}
		if (!this.canDrop && !this.shareable)
		{
			return;
		}
		if (this.worldShareableInstance != null)
		{
			return;
		}
		object[] data = new object[]
		{
			this.myIndex,
			PhotonNetwork.LocalPlayer
		};
		this.worldShareableInstance = PhotonNetwork.Instantiate("Objects/equipment/WorldShareableItem", base.transform.position, base.transform.rotation, 0, data);
		if (this.myRig != null && this.worldShareableInstance != null)
		{
			this.OnWorldShareableItemSpawn();
		}
	}

	// Token: 0x06001836 RID: 6198 RVA: 0x0003F687 File Offset: 0x0003D887
	public void OnJoinedRoom()
	{
		Debug.Log("Here");
		this.SpawnShareableObject();
	}

	// Token: 0x06001837 RID: 6199 RVA: 0x0003F699 File Offset: 0x0003D899
	public void OnLeftRoom()
	{
		if (this.worldShareableInstance != null)
		{
			PhotonNetwork.Destroy(this.worldShareableInstance);
		}
		this.OnWorldShareableItemDeallocated(NetworkSystem.Instance.LocalPlayer);
	}

	// Token: 0x06001838 RID: 6200 RVA: 0x0003F6C4 File Offset: 0x0003D8C4
	public void OnPlayerLeftRoom(NetPlayer otherPlayer)
	{
		this.OnWorldShareableItemDeallocated(otherPlayer);
	}

	// Token: 0x06001839 RID: 6201 RVA: 0x0003F6CD File Offset: 0x0003D8CD
	public void SetWorldShareableItem(GameObject item)
	{
		this.worldShareableInstance = item;
		this.OnWorldShareableItemSpawn();
	}

	// Token: 0x0600183A RID: 6202 RVA: 0x0002F75F File Offset: 0x0002D95F
	protected virtual void OnWorldShareableItemSpawn()
	{
	}

	// Token: 0x0600183B RID: 6203 RVA: 0x0002F75F File Offset: 0x0002D95F
	protected virtual void OnWorldShareableItemDeallocated(NetPlayer player)
	{
	}

	// Token: 0x0600183C RID: 6204 RVA: 0x000C9934 File Offset: 0x000C7B34
	public virtual void LateUpdate()
	{
		if (this.interactor == null)
		{
			this.interactor = EquipmentInteractor.instance;
		}
		if (this.IsMyItem())
		{
			this.LateUpdateLocal();
		}
		else
		{
			this.LateUpdateReplicated();
		}
		this.LateUpdateShared();
		this.previousState = this.currentState;
	}

	// Token: 0x0600183D RID: 6205 RVA: 0x0003F6DC File Offset: 0x0003D8DC
	protected Transform DefaultAnchor()
	{
		if (!(this.anchor == null))
		{
			return this.anchor;
		}
		return base.transform;
	}

	// Token: 0x0600183E RID: 6206 RVA: 0x0003F6F9 File Offset: 0x0003D8F9
	private Transform GetAnchor(TransferrableObject.PositionState pos)
	{
		if (this.grabAnchor == null)
		{
			return this.DefaultAnchor();
		}
		if (this.InHand())
		{
			return this.grabAnchor;
		}
		return this.DefaultAnchor();
	}

	// Token: 0x0600183F RID: 6207 RVA: 0x000C9984 File Offset: 0x000C7B84
	protected bool Attached()
	{
		bool flag = this.InHand() && this.detatchOnGrab;
		return !this.Dropped() && !flag;
	}

	// Token: 0x06001840 RID: 6208 RVA: 0x000C99B4 File Offset: 0x000C7BB4
	private void UpdateFollowXform()
	{
		if (this.targetRig == null)
		{
			return;
		}
		if (this.targetDock == null)
		{
			this.targetDock = this.targetRig.GetComponent<BodyDockPositions>();
		}
		if (this.anchorOverrides == null)
		{
			this.anchorOverrides = this.targetRig.GetComponent<VRRigAnchorOverrides>();
		}
		Transform transform = this.GetAnchor(this.currentState);
		Transform transform2 = transform;
		TransferrableObject.PositionState positionState = this.currentState;
		if (positionState <= TransferrableObject.PositionState.InRightHand)
		{
			switch (positionState)
			{
			case TransferrableObject.PositionState.OnLeftArm:
				transform2 = this.anchorOverrides.AnchorOverride(this.currentState, this.targetDock.leftArmTransform);
				break;
			case TransferrableObject.PositionState.OnRightArm:
				transform2 = this.anchorOverrides.AnchorOverride(this.currentState, this.targetDock.rightArmTransform);
				break;
			case TransferrableObject.PositionState.OnLeftArm | TransferrableObject.PositionState.OnRightArm:
				break;
			case TransferrableObject.PositionState.InLeftHand:
				transform2 = this.anchorOverrides.AnchorOverride(this.currentState, this.targetDock.leftHandTransform);
				break;
			default:
				if (positionState == TransferrableObject.PositionState.InRightHand)
				{
					transform2 = this.anchorOverrides.AnchorOverride(this.currentState, this.targetDock.rightHandTransform);
				}
				break;
			}
		}
		else if (positionState != TransferrableObject.PositionState.OnChest)
		{
			if (positionState != TransferrableObject.PositionState.OnLeftShoulder)
			{
				if (positionState == TransferrableObject.PositionState.OnRightShoulder)
				{
					transform2 = this.anchorOverrides.AnchorOverride(this.currentState, this.targetDock.rightBackTransform);
				}
			}
			else
			{
				transform2 = this.anchorOverrides.AnchorOverride(this.currentState, this.targetDock.leftBackTransform);
			}
		}
		else
		{
			transform2 = this.anchorOverrides.AnchorOverride(this.currentState, this.targetDock.chestTransform);
		}
		LegacyTransferrableObject.InterpolateState interpolateState = this.interpState;
		if (interpolateState != LegacyTransferrableObject.InterpolateState.None)
		{
			if (interpolateState != LegacyTransferrableObject.InterpolateState.Interpolating)
			{
				return;
			}
			float t = Mathf.Clamp((this.interpTime - this.interpDt) / this.interpTime, 0f, 1f);
			transform.transform.position = Vector3.Lerp(this.interpStartPos, transform2.transform.position, t);
			transform.transform.rotation = Quaternion.Slerp(this.interpStartRot, transform2.transform.rotation, t);
			this.interpDt -= Time.deltaTime;
			if (this.interpDt <= 0f)
			{
				transform.parent = transform2;
				this.interpState = LegacyTransferrableObject.InterpolateState.None;
				transform.localPosition = Vector3.zero;
				transform.localRotation = Quaternion.identity;
				transform.localScale = Vector3.one;
				if (this.flipOnXForLeftHand && this.currentState == TransferrableObject.PositionState.InLeftHand)
				{
					transform.localScale = new Vector3(-1f, 1f, 1f);
				}
				if (this.flipOnYForLeftHand && this.currentState == TransferrableObject.PositionState.InLeftHand)
				{
					transform.localScale = new Vector3(1f, -1f, 1f);
				}
			}
		}
		else if (transform2 != transform.parent)
		{
			if (Time.frameCount == this.enabledOnFrame)
			{
				transform.parent = transform2;
				transform.localPosition = Vector3.zero;
				transform.localRotation = Quaternion.identity;
				return;
			}
			this.interpState = LegacyTransferrableObject.InterpolateState.Interpolating;
			this.interpDt = this.interpTime;
			this.interpStartPos = transform.transform.position;
			this.interpStartRot = transform.transform.rotation;
			return;
		}
	}

	// Token: 0x06001841 RID: 6209 RVA: 0x0003F725 File Offset: 0x0003D925
	public void DropItem()
	{
		base.transform.parent = null;
	}

	// Token: 0x06001842 RID: 6210 RVA: 0x000C9CDC File Offset: 0x000C7EDC
	protected virtual void LateUpdateShared()
	{
		this.disableItem = true;
		for (int i = 0; i < this.targetRig.ActiveTransferrableObjectIndexLength(); i++)
		{
			if (this.targetRig.ActiveTransferrableObjectIndex(i) == this.myIndex)
			{
				this.disableItem = false;
				break;
			}
		}
		if (this.disableItem)
		{
			base.gameObject.SetActive(false);
			return;
		}
		if (this.previousState != this.currentState && this.detatchOnGrab && this.InHand())
		{
			base.transform.parent = null;
		}
		if (this.currentState != TransferrableObject.PositionState.Dropped)
		{
			this.UpdateFollowXform();
			return;
		}
		if (this.canDrop)
		{
			this.DropItem();
		}
	}

	// Token: 0x06001843 RID: 6211 RVA: 0x000C9D84 File Offset: 0x000C7F84
	protected void ResetXf()
	{
		if (this.canDrop)
		{
			Transform transform = this.DefaultAnchor();
			if (base.transform != transform && base.transform.parent != transform)
			{
				base.transform.parent = transform;
			}
			base.transform.localPosition = this.initOffset;
			base.transform.localRotation = this.initRotation;
		}
	}

	// Token: 0x06001844 RID: 6212 RVA: 0x0003F733 File Offset: 0x0003D933
	protected void ReDock()
	{
		if (this.IsMyItem())
		{
			this.currentState = this.initState;
		}
		this.ResetXf();
	}

	// Token: 0x06001845 RID: 6213 RVA: 0x000C9DF0 File Offset: 0x000C7FF0
	private void HandleLocalInput()
	{
		GameObject[] array;
		if (!this.InHand())
		{
			array = this.gameObjectsActiveOnlyWhileHeld;
			for (int i = 0; i < array.Length; i++)
			{
				array[i].SetActive(false);
			}
			return;
		}
		array = this.gameObjectsActiveOnlyWhileHeld;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].SetActive(true);
		}
		XRNode node = (this.currentState == TransferrableObject.PositionState.InLeftHand) ? XRNode.LeftHand : XRNode.RightHand;
		this.indexTrigger = ControllerInputPoller.TriggerFloat(node);
		bool flag = !this.latched && this.indexTrigger >= this.myThreshold;
		bool flag2 = this.latched && this.indexTrigger < this.myThreshold - this.hysterisis;
		if (flag || this.testActivate)
		{
			this.testActivate = false;
			if (this.CanActivate())
			{
				this.OnActivate();
				return;
			}
		}
		else if (flag2 || this.testDeactivate)
		{
			this.testDeactivate = false;
			if (this.CanDeactivate())
			{
				this.OnDeactivate();
			}
		}
	}

	// Token: 0x06001846 RID: 6214 RVA: 0x000C9EDC File Offset: 0x000C80DC
	protected virtual void LateUpdateLocal()
	{
		this.wasHover = this.isHover;
		this.isHover = false;
		if (PhotonNetwork.InRoom)
		{
			this.myRig.SetTransferrablePosStates(this.objectIndex, this.currentState);
			this.myRig.SetTransferrableItemStates(this.objectIndex, this.itemState);
		}
		this.targetRig = this.myRig;
		this.HandleLocalInput();
	}

	// Token: 0x06001847 RID: 6215 RVA: 0x000C9F44 File Offset: 0x000C8144
	protected virtual void LateUpdateReplicated()
	{
		this.currentState = this.myOnlineRig.TransferrablePosStates(this.objectIndex);
		if (this.currentState == TransferrableObject.PositionState.Dropped && !this.canDrop && !this.shareable)
		{
			if (this.previousState == TransferrableObject.PositionState.None)
			{
				base.gameObject.SetActive(false);
			}
			this.currentState = this.previousState;
		}
		this.itemState = this.myOnlineRig.TransferrableItemStates(this.objectIndex);
		this.targetRig = this.myOnlineRig;
		if (this.myOnlineRig != null)
		{
			bool flag = true;
			for (int i = 0; i < this.myOnlineRig.ActiveTransferrableObjectIndexLength(); i++)
			{
				if (this.myOnlineRig.ActiveTransferrableObjectIndex(i) == this.myIndex)
				{
					flag = false;
					GameObject[] array = this.gameObjectsActiveOnlyWhileHeld;
					for (int j = 0; j < array.Length; j++)
					{
						array[j].SetActive(this.InHand());
					}
				}
			}
			if (flag)
			{
				base.gameObject.SetActive(false);
			}
		}
	}

	// Token: 0x06001848 RID: 6216 RVA: 0x0003F74F File Offset: 0x0003D94F
	public virtual void ResetToDefaultState()
	{
		this.canAutoGrabLeft = true;
		this.canAutoGrabRight = true;
		this.wasHover = false;
		this.isHover = false;
		this.ResetXf();
	}

	// Token: 0x06001849 RID: 6217 RVA: 0x000CA038 File Offset: 0x000C8238
	public override void OnGrab(InteractionPoint pointGrabbed, GameObject grabbingHand)
	{
		if (!this.IsMyItem())
		{
			return;
		}
		if (!(grabbingHand == this.interactor.leftHand) || this.currentState == TransferrableObject.PositionState.OnLeftArm)
		{
			if (grabbingHand == this.interactor.rightHand && this.currentState != TransferrableObject.PositionState.OnRightArm)
			{
				if (this.currentState == TransferrableObject.PositionState.InLeftHand && this.disableStealing)
				{
					return;
				}
				this.canAutoGrabRight = false;
				this.currentState = TransferrableObject.PositionState.InRightHand;
				EquipmentInteractor.instance.UpdateHandEquipment(this, false);
				GorillaTagger.Instance.StartVibration(false, GorillaTagger.Instance.tapHapticStrength / 8f, GorillaTagger.Instance.tapHapticDuration * 0.5f);
			}
			return;
		}
		if (this.currentState == TransferrableObject.PositionState.InRightHand && this.disableStealing)
		{
			return;
		}
		this.canAutoGrabLeft = false;
		this.currentState = TransferrableObject.PositionState.InLeftHand;
		EquipmentInteractor.instance.UpdateHandEquipment(this, true);
		GorillaTagger.Instance.StartVibration(true, GorillaTagger.Instance.tapHapticStrength / 8f, GorillaTagger.Instance.tapHapticDuration * 0.5f);
	}

	// Token: 0x0600184A RID: 6218 RVA: 0x000CA140 File Offset: 0x000C8340
	public override bool OnRelease(DropZone zoneReleased, GameObject releasingHand)
	{
		if (!this.IsMyItem())
		{
			return false;
		}
		if (!this.CanDeactivate())
		{
			return false;
		}
		if (this.IsHeld() && ((releasingHand == EquipmentInteractor.instance.rightHand && EquipmentInteractor.instance.rightHandHeldEquipment != null && this == (LegacyTransferrableObject)EquipmentInteractor.instance.rightHandHeldEquipment) || (releasingHand == EquipmentInteractor.instance.leftHand && EquipmentInteractor.instance.leftHandHeldEquipment != null && this == (LegacyTransferrableObject)EquipmentInteractor.instance.leftHandHeldEquipment)))
		{
			if (releasingHand == EquipmentInteractor.instance.leftHand)
			{
				this.canAutoGrabLeft = true;
			}
			else
			{
				this.canAutoGrabRight = true;
			}
			if (zoneReleased != null)
			{
				bool flag = this.currentState == TransferrableObject.PositionState.InLeftHand && zoneReleased.dropPosition == BodyDockPositions.DropPositions.LeftArm;
				bool flag2 = this.currentState == TransferrableObject.PositionState.InRightHand && zoneReleased.dropPosition == BodyDockPositions.DropPositions.RightArm;
				if (flag || flag2)
				{
					return false;
				}
				if (this.targetDock.DropZoneStorageUsed(zoneReleased.dropPosition) == -1 && zoneReleased.forBodyDock == this.targetDock && (zoneReleased.dropPosition & this.dockPositions) != BodyDockPositions.DropPositions.None)
				{
					this.storedZone = zoneReleased.dropPosition;
				}
			}
			this.DropItemCleanup();
			EquipmentInteractor.instance.UpdateHandEquipment(null, releasingHand == EquipmentInteractor.instance.leftHand);
			return true;
		}
		return false;
	}

	// Token: 0x0600184B RID: 6219 RVA: 0x000CA2B4 File Offset: 0x000C84B4
	public override void DropItemCleanup()
	{
		if (this.canDrop)
		{
			this.currentState = TransferrableObject.PositionState.Dropped;
			return;
		}
		BodyDockPositions.DropPositions dropPositions = this.storedZone;
		switch (dropPositions)
		{
		case BodyDockPositions.DropPositions.LeftArm:
			this.currentState = TransferrableObject.PositionState.OnLeftArm;
			return;
		case BodyDockPositions.DropPositions.RightArm:
			this.currentState = TransferrableObject.PositionState.OnRightArm;
			return;
		case BodyDockPositions.DropPositions.LeftArm | BodyDockPositions.DropPositions.RightArm:
			break;
		case BodyDockPositions.DropPositions.Chest:
			this.currentState = TransferrableObject.PositionState.OnChest;
			return;
		default:
			if (dropPositions == BodyDockPositions.DropPositions.LeftBack)
			{
				this.currentState = TransferrableObject.PositionState.OnLeftShoulder;
				return;
			}
			if (dropPositions != BodyDockPositions.DropPositions.RightBack)
			{
				return;
			}
			this.currentState = TransferrableObject.PositionState.OnRightShoulder;
			break;
		}
	}

	// Token: 0x0600184C RID: 6220 RVA: 0x000CA328 File Offset: 0x000C8528
	public override void OnHover(InteractionPoint pointHovered, GameObject hoveringHand)
	{
		if (!this.IsMyItem())
		{
			return;
		}
		if (!this.wasHover)
		{
			GorillaTagger.Instance.StartVibration(hoveringHand == EquipmentInteractor.instance.leftHand, GorillaTagger.Instance.tapHapticStrength / 8f, GorillaTagger.Instance.tapHapticDuration * 0.5f);
		}
		this.isHover = true;
	}

	// Token: 0x0600184D RID: 6221 RVA: 0x000CA38C File Offset: 0x000C858C
	protected void ActivateItemFX(float hapticStrength, float hapticDuration, int soundIndex, float soundVolume)
	{
		bool flag = this.currentState == TransferrableObject.PositionState.InLeftHand;
		VRRig vrrig = this.targetRig;
		if ((vrrig != null) ? vrrig.netView : null)
		{
			this.targetRig.rigSerializer.RPC_PlayHandTap(soundIndex, flag, 0.1f, default(PhotonMessageInfo));
		}
		this.myRig.PlayHandTapLocal(soundIndex, flag, soundVolume);
		GorillaTagger.Instance.StartVibration(flag, hapticStrength, hapticDuration);
	}

	// Token: 0x0600184E RID: 6222 RVA: 0x0002F75F File Offset: 0x0002D95F
	public virtual void PlayNote(int note, float volume)
	{
	}

	// Token: 0x0600184F RID: 6223 RVA: 0x0003F773 File Offset: 0x0003D973
	public virtual bool AutoGrabTrue(bool leftGrabbingHand)
	{
		if (!leftGrabbingHand)
		{
			return this.canAutoGrabRight;
		}
		return this.canAutoGrabLeft;
	}

	// Token: 0x06001850 RID: 6224 RVA: 0x00038586 File Offset: 0x00036786
	public virtual bool CanActivate()
	{
		return true;
	}

	// Token: 0x06001851 RID: 6225 RVA: 0x00038586 File Offset: 0x00036786
	public virtual bool CanDeactivate()
	{
		return true;
	}

	// Token: 0x06001852 RID: 6226 RVA: 0x0003F785 File Offset: 0x0003D985
	public virtual void OnActivate()
	{
		this.latched = true;
	}

	// Token: 0x06001853 RID: 6227 RVA: 0x0003F78E File Offset: 0x0003D98E
	public virtual void OnDeactivate()
	{
		this.latched = false;
	}

	// Token: 0x06001854 RID: 6228 RVA: 0x0003F797 File Offset: 0x0003D997
	public virtual bool IsMyItem()
	{
		return this.myRig != null && this.myRig.isOfflineVRRig;
	}

	// Token: 0x06001855 RID: 6229 RVA: 0x000CA3F8 File Offset: 0x000C85F8
	protected virtual bool IsHeld()
	{
		return (EquipmentInteractor.instance.leftHandHeldEquipment != null && (LegacyTransferrableObject)EquipmentInteractor.instance.leftHandHeldEquipment == this) || (EquipmentInteractor.instance.rightHandHeldEquipment != null && (LegacyTransferrableObject)EquipmentInteractor.instance.rightHandHeldEquipment == this);
	}

	// Token: 0x06001856 RID: 6230 RVA: 0x0003F7B4 File Offset: 0x0003D9B4
	public bool InHand()
	{
		return this.currentState == TransferrableObject.PositionState.InLeftHand || this.currentState == TransferrableObject.PositionState.InRightHand;
	}

	// Token: 0x06001857 RID: 6231 RVA: 0x0003F7CA File Offset: 0x0003D9CA
	public bool Dropped()
	{
		return this.currentState == TransferrableObject.PositionState.Dropped;
	}

	// Token: 0x06001858 RID: 6232 RVA: 0x0003F7D9 File Offset: 0x0003D9D9
	public bool InLeftHand()
	{
		return this.currentState == TransferrableObject.PositionState.InLeftHand;
	}

	// Token: 0x06001859 RID: 6233 RVA: 0x0003F7E4 File Offset: 0x0003D9E4
	public bool InRightHand()
	{
		return this.currentState == TransferrableObject.PositionState.InRightHand;
	}

	// Token: 0x0600185A RID: 6234 RVA: 0x0003F7EF File Offset: 0x0003D9EF
	public bool OnChest()
	{
		return this.currentState == TransferrableObject.PositionState.OnChest;
	}

	// Token: 0x0600185B RID: 6235 RVA: 0x0003F7FB File Offset: 0x0003D9FB
	public bool OnShoulder()
	{
		return this.currentState == TransferrableObject.PositionState.OnLeftShoulder || this.currentState == TransferrableObject.PositionState.OnRightShoulder;
	}

	// Token: 0x0600185C RID: 6236 RVA: 0x0003F813 File Offset: 0x0003DA13
	protected NetPlayer OwningPlayer()
	{
		if (this.myRig == null)
		{
			return this.myOnlineRig.netView.Owner;
		}
		return NetworkSystem.Instance.LocalPlayer;
	}

	// Token: 0x04001ACF RID: 6863
	protected EquipmentInteractor interactor;

	// Token: 0x04001AD0 RID: 6864
	public VRRig myRig;

	// Token: 0x04001AD1 RID: 6865
	public VRRig myOnlineRig;

	// Token: 0x04001AD2 RID: 6866
	public bool latched;

	// Token: 0x04001AD3 RID: 6867
	private float indexTrigger;

	// Token: 0x04001AD4 RID: 6868
	public bool testActivate;

	// Token: 0x04001AD5 RID: 6869
	public bool testDeactivate;

	// Token: 0x04001AD6 RID: 6870
	public float myThreshold = 0.8f;

	// Token: 0x04001AD7 RID: 6871
	public float hysterisis = 0.05f;

	// Token: 0x04001AD8 RID: 6872
	public bool flipOnXForLeftHand;

	// Token: 0x04001AD9 RID: 6873
	public bool flipOnYForLeftHand;

	// Token: 0x04001ADA RID: 6874
	public bool flipOnXForLeftArm;

	// Token: 0x04001ADB RID: 6875
	public bool disableStealing;

	// Token: 0x04001ADC RID: 6876
	private TransferrableObject.PositionState initState;

	// Token: 0x04001ADD RID: 6877
	public TransferrableObject.ItemStates itemState;

	// Token: 0x04001ADE RID: 6878
	public BodyDockPositions.DropPositions storedZone;

	// Token: 0x04001ADF RID: 6879
	protected TransferrableObject.PositionState previousState;

	// Token: 0x04001AE0 RID: 6880
	public TransferrableObject.PositionState currentState;

	// Token: 0x04001AE1 RID: 6881
	public BodyDockPositions.DropPositions dockPositions;

	// Token: 0x04001AE2 RID: 6882
	public VRRig targetRig;

	// Token: 0x04001AE3 RID: 6883
	public BodyDockPositions targetDock;

	// Token: 0x04001AE4 RID: 6884
	private VRRigAnchorOverrides anchorOverrides;

	// Token: 0x04001AE5 RID: 6885
	public bool canAutoGrabLeft;

	// Token: 0x04001AE6 RID: 6886
	public bool canAutoGrabRight;

	// Token: 0x04001AE7 RID: 6887
	public int objectIndex;

	// Token: 0x04001AE8 RID: 6888
	[Tooltip("In Holdables.prefab, assign to the parent of this transform.\nExample: 'Holdables/YellowHandBootsRight' is the anchor of 'Holdables/YellowHandBootsRight/YELLOW HAND BOOTS'")]
	public Transform anchor;

	// Token: 0x04001AE9 RID: 6889
	[Tooltip("In Holdables.prefab, assign to the Collider to grab this object")]
	public InteractionPoint gripInteractor;

	// Token: 0x04001AEA RID: 6890
	[Tooltip("(Optional) Use this to override the transform used when the object is in the hand.\nExample: 'GHOST BALLOON' uses child 'grabPtAnchor' which is the end of the balloon's string.")]
	public Transform grabAnchor;

	// Token: 0x04001AEB RID: 6891
	public int myIndex;

	// Token: 0x04001AEC RID: 6892
	[Tooltip("(Optional)")]
	public GameObject[] gameObjectsActiveOnlyWhileHeld;

	// Token: 0x04001AED RID: 6893
	protected GameObject worldShareableInstance;

	// Token: 0x04001AEE RID: 6894
	private float interpTime = 0.1f;

	// Token: 0x04001AEF RID: 6895
	private float interpDt;

	// Token: 0x04001AF0 RID: 6896
	private Vector3 interpStartPos;

	// Token: 0x04001AF1 RID: 6897
	private Quaternion interpStartRot;

	// Token: 0x04001AF2 RID: 6898
	protected int enabledOnFrame = -1;

	// Token: 0x04001AF3 RID: 6899
	private Vector3 initOffset;

	// Token: 0x04001AF4 RID: 6900
	private Quaternion initRotation;

	// Token: 0x04001AF5 RID: 6901
	public bool canDrop;

	// Token: 0x04001AF6 RID: 6902
	public bool shareable;

	// Token: 0x04001AF7 RID: 6903
	public bool detatchOnGrab;

	// Token: 0x04001AF8 RID: 6904
	private bool wasHover;

	// Token: 0x04001AF9 RID: 6905
	private bool isHover;

	// Token: 0x04001AFA RID: 6906
	private bool disableItem;

	// Token: 0x04001AFB RID: 6907
	public const int kPositionStateCount = 8;

	// Token: 0x04001AFC RID: 6908
	public LegacyTransferrableObject.InterpolateState interpState;

	// Token: 0x020003E6 RID: 998
	public enum InterpolateState
	{
		// Token: 0x04001AFE RID: 6910
		None,
		// Token: 0x04001AFF RID: 6911
		Interpolating
	}
}
