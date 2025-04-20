using System;
using Photon.Pun;
using UnityEngine;
using UnityEngine.XR;

// Token: 0x020003F0 RID: 1008
public class LegacyTransferrableObject : HoldableObject
{
	// Token: 0x0600187B RID: 6267 RVA: 0x0004093D File Offset: 0x0003EB3D
	protected void Awake()
	{
		this.latched = false;
		this.initOffset = base.transform.localPosition;
		this.initRotation = base.transform.localRotation;
	}

	// Token: 0x0600187C RID: 6268 RVA: 0x000CBEC8 File Offset: 0x000CA0C8
	protected virtual void Start()
	{
		RoomSystem.JoinedRoomEvent = (Action)Delegate.Combine(RoomSystem.JoinedRoomEvent, new Action(this.OnJoinedRoom));
		RoomSystem.LeftRoomEvent = (Action)Delegate.Combine(RoomSystem.LeftRoomEvent, new Action(this.OnLeftRoom));
		RoomSystem.PlayerJoinedEvent = (Action<NetPlayer>)Delegate.Combine(RoomSystem.PlayerJoinedEvent, new Action<NetPlayer>(this.OnPlayerLeftRoom));
	}

	// Token: 0x0600187D RID: 6269 RVA: 0x000CBF38 File Offset: 0x000CA138
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

	// Token: 0x0600187E RID: 6270 RVA: 0x00040968 File Offset: 0x0003EB68
	public void OnDisable()
	{
		this.enabledOnFrame = -1;
	}

	// Token: 0x0600187F RID: 6271 RVA: 0x000CC0C0 File Offset: 0x000CA2C0
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

	// Token: 0x06001880 RID: 6272 RVA: 0x00040971 File Offset: 0x0003EB71
	public void OnJoinedRoom()
	{
		Debug.Log("Here");
		this.SpawnShareableObject();
	}

	// Token: 0x06001881 RID: 6273 RVA: 0x00040983 File Offset: 0x0003EB83
	public void OnLeftRoom()
	{
		if (this.worldShareableInstance != null)
		{
			PhotonNetwork.Destroy(this.worldShareableInstance);
		}
		this.OnWorldShareableItemDeallocated(NetworkSystem.Instance.LocalPlayer);
	}

	// Token: 0x06001882 RID: 6274 RVA: 0x000409AE File Offset: 0x0003EBAE
	public void OnPlayerLeftRoom(NetPlayer otherPlayer)
	{
		this.OnWorldShareableItemDeallocated(otherPlayer);
	}

	// Token: 0x06001883 RID: 6275 RVA: 0x000409B7 File Offset: 0x0003EBB7
	public void SetWorldShareableItem(GameObject item)
	{
		this.worldShareableInstance = item;
		this.OnWorldShareableItemSpawn();
	}

	// Token: 0x06001884 RID: 6276 RVA: 0x00030607 File Offset: 0x0002E807
	protected virtual void OnWorldShareableItemSpawn()
	{
	}

	// Token: 0x06001885 RID: 6277 RVA: 0x00030607 File Offset: 0x0002E807
	protected virtual void OnWorldShareableItemDeallocated(NetPlayer player)
	{
	}

	// Token: 0x06001886 RID: 6278 RVA: 0x000CC15C File Offset: 0x000CA35C
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

	// Token: 0x06001887 RID: 6279 RVA: 0x000409C6 File Offset: 0x0003EBC6
	protected Transform DefaultAnchor()
	{
		if (!(this.anchor == null))
		{
			return this.anchor;
		}
		return base.transform;
	}

	// Token: 0x06001888 RID: 6280 RVA: 0x000409E3 File Offset: 0x0003EBE3
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

	// Token: 0x06001889 RID: 6281 RVA: 0x000CC1AC File Offset: 0x000CA3AC
	protected bool Attached()
	{
		bool flag = this.InHand() && this.detatchOnGrab;
		return !this.Dropped() && !flag;
	}

	// Token: 0x0600188A RID: 6282 RVA: 0x000CC1DC File Offset: 0x000CA3DC
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

	// Token: 0x0600188B RID: 6283 RVA: 0x00040A0F File Offset: 0x0003EC0F
	public void DropItem()
	{
		base.transform.parent = null;
	}

	// Token: 0x0600188C RID: 6284 RVA: 0x000CC504 File Offset: 0x000CA704
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

	// Token: 0x0600188D RID: 6285 RVA: 0x000CC5AC File Offset: 0x000CA7AC
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

	// Token: 0x0600188E RID: 6286 RVA: 0x00040A1D File Offset: 0x0003EC1D
	protected void ReDock()
	{
		if (this.IsMyItem())
		{
			this.currentState = this.initState;
		}
		this.ResetXf();
	}

	// Token: 0x0600188F RID: 6287 RVA: 0x000CC618 File Offset: 0x000CA818
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

	// Token: 0x06001890 RID: 6288 RVA: 0x000CC704 File Offset: 0x000CA904
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

	// Token: 0x06001891 RID: 6289 RVA: 0x000CC76C File Offset: 0x000CA96C
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

	// Token: 0x06001892 RID: 6290 RVA: 0x00040A39 File Offset: 0x0003EC39
	public virtual void ResetToDefaultState()
	{
		this.canAutoGrabLeft = true;
		this.canAutoGrabRight = true;
		this.wasHover = false;
		this.isHover = false;
		this.ResetXf();
	}

	// Token: 0x06001893 RID: 6291 RVA: 0x000CC860 File Offset: 0x000CAA60
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

	// Token: 0x06001894 RID: 6292 RVA: 0x000CC968 File Offset: 0x000CAB68
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

	// Token: 0x06001895 RID: 6293 RVA: 0x000CCADC File Offset: 0x000CACDC
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

	// Token: 0x06001896 RID: 6294 RVA: 0x000CCB50 File Offset: 0x000CAD50
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

	// Token: 0x06001897 RID: 6295 RVA: 0x000CCBB4 File Offset: 0x000CADB4
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

	// Token: 0x06001898 RID: 6296 RVA: 0x00030607 File Offset: 0x0002E807
	public virtual void PlayNote(int note, float volume)
	{
	}

	// Token: 0x06001899 RID: 6297 RVA: 0x00040A5D File Offset: 0x0003EC5D
	public virtual bool AutoGrabTrue(bool leftGrabbingHand)
	{
		if (!leftGrabbingHand)
		{
			return this.canAutoGrabRight;
		}
		return this.canAutoGrabLeft;
	}

	// Token: 0x0600189A RID: 6298 RVA: 0x00039846 File Offset: 0x00037A46
	public virtual bool CanActivate()
	{
		return true;
	}

	// Token: 0x0600189B RID: 6299 RVA: 0x00039846 File Offset: 0x00037A46
	public virtual bool CanDeactivate()
	{
		return true;
	}

	// Token: 0x0600189C RID: 6300 RVA: 0x00040A6F File Offset: 0x0003EC6F
	public virtual void OnActivate()
	{
		this.latched = true;
	}

	// Token: 0x0600189D RID: 6301 RVA: 0x00040A78 File Offset: 0x0003EC78
	public virtual void OnDeactivate()
	{
		this.latched = false;
	}

	// Token: 0x0600189E RID: 6302 RVA: 0x00040A81 File Offset: 0x0003EC81
	public virtual bool IsMyItem()
	{
		return this.myRig != null && this.myRig.isOfflineVRRig;
	}

	// Token: 0x0600189F RID: 6303 RVA: 0x000CCC20 File Offset: 0x000CAE20
	protected virtual bool IsHeld()
	{
		return (EquipmentInteractor.instance.leftHandHeldEquipment != null && (LegacyTransferrableObject)EquipmentInteractor.instance.leftHandHeldEquipment == this) || (EquipmentInteractor.instance.rightHandHeldEquipment != null && (LegacyTransferrableObject)EquipmentInteractor.instance.rightHandHeldEquipment == this);
	}

	// Token: 0x060018A0 RID: 6304 RVA: 0x00040A9E File Offset: 0x0003EC9E
	public bool InHand()
	{
		return this.currentState == TransferrableObject.PositionState.InLeftHand || this.currentState == TransferrableObject.PositionState.InRightHand;
	}

	// Token: 0x060018A1 RID: 6305 RVA: 0x00040AB4 File Offset: 0x0003ECB4
	public bool Dropped()
	{
		return this.currentState == TransferrableObject.PositionState.Dropped;
	}

	// Token: 0x060018A2 RID: 6306 RVA: 0x00040AC3 File Offset: 0x0003ECC3
	public bool InLeftHand()
	{
		return this.currentState == TransferrableObject.PositionState.InLeftHand;
	}

	// Token: 0x060018A3 RID: 6307 RVA: 0x00040ACE File Offset: 0x0003ECCE
	public bool InRightHand()
	{
		return this.currentState == TransferrableObject.PositionState.InRightHand;
	}

	// Token: 0x060018A4 RID: 6308 RVA: 0x00040AD9 File Offset: 0x0003ECD9
	public bool OnChest()
	{
		return this.currentState == TransferrableObject.PositionState.OnChest;
	}

	// Token: 0x060018A5 RID: 6309 RVA: 0x00040AE5 File Offset: 0x0003ECE5
	public bool OnShoulder()
	{
		return this.currentState == TransferrableObject.PositionState.OnLeftShoulder || this.currentState == TransferrableObject.PositionState.OnRightShoulder;
	}

	// Token: 0x060018A6 RID: 6310 RVA: 0x00040AFD File Offset: 0x0003ECFD
	protected NetPlayer OwningPlayer()
	{
		if (this.myRig == null)
		{
			return this.myOnlineRig.netView.Owner;
		}
		return NetworkSystem.Instance.LocalPlayer;
	}

	// Token: 0x04001B17 RID: 6935
	protected EquipmentInteractor interactor;

	// Token: 0x04001B18 RID: 6936
	public VRRig myRig;

	// Token: 0x04001B19 RID: 6937
	public VRRig myOnlineRig;

	// Token: 0x04001B1A RID: 6938
	public bool latched;

	// Token: 0x04001B1B RID: 6939
	private float indexTrigger;

	// Token: 0x04001B1C RID: 6940
	public bool testActivate;

	// Token: 0x04001B1D RID: 6941
	public bool testDeactivate;

	// Token: 0x04001B1E RID: 6942
	public float myThreshold = 0.8f;

	// Token: 0x04001B1F RID: 6943
	public float hysterisis = 0.05f;

	// Token: 0x04001B20 RID: 6944
	public bool flipOnXForLeftHand;

	// Token: 0x04001B21 RID: 6945
	public bool flipOnYForLeftHand;

	// Token: 0x04001B22 RID: 6946
	public bool flipOnXForLeftArm;

	// Token: 0x04001B23 RID: 6947
	public bool disableStealing;

	// Token: 0x04001B24 RID: 6948
	private TransferrableObject.PositionState initState;

	// Token: 0x04001B25 RID: 6949
	public TransferrableObject.ItemStates itemState;

	// Token: 0x04001B26 RID: 6950
	public BodyDockPositions.DropPositions storedZone;

	// Token: 0x04001B27 RID: 6951
	protected TransferrableObject.PositionState previousState;

	// Token: 0x04001B28 RID: 6952
	public TransferrableObject.PositionState currentState;

	// Token: 0x04001B29 RID: 6953
	public BodyDockPositions.DropPositions dockPositions;

	// Token: 0x04001B2A RID: 6954
	public VRRig targetRig;

	// Token: 0x04001B2B RID: 6955
	public BodyDockPositions targetDock;

	// Token: 0x04001B2C RID: 6956
	private VRRigAnchorOverrides anchorOverrides;

	// Token: 0x04001B2D RID: 6957
	public bool canAutoGrabLeft;

	// Token: 0x04001B2E RID: 6958
	public bool canAutoGrabRight;

	// Token: 0x04001B2F RID: 6959
	public int objectIndex;

	// Token: 0x04001B30 RID: 6960
	[Tooltip("In Holdables.prefab, assign to the parent of this transform.\nExample: 'Holdables/YellowHandBootsRight' is the anchor of 'Holdables/YellowHandBootsRight/YELLOW HAND BOOTS'")]
	public Transform anchor;

	// Token: 0x04001B31 RID: 6961
	[Tooltip("In Holdables.prefab, assign to the Collider to grab this object")]
	public InteractionPoint gripInteractor;

	// Token: 0x04001B32 RID: 6962
	[Tooltip("(Optional) Use this to override the transform used when the object is in the hand.\nExample: 'GHOST BALLOON' uses child 'grabPtAnchor' which is the end of the balloon's string.")]
	public Transform grabAnchor;

	// Token: 0x04001B33 RID: 6963
	public int myIndex;

	// Token: 0x04001B34 RID: 6964
	[Tooltip("(Optional)")]
	public GameObject[] gameObjectsActiveOnlyWhileHeld;

	// Token: 0x04001B35 RID: 6965
	protected GameObject worldShareableInstance;

	// Token: 0x04001B36 RID: 6966
	private float interpTime = 0.1f;

	// Token: 0x04001B37 RID: 6967
	private float interpDt;

	// Token: 0x04001B38 RID: 6968
	private Vector3 interpStartPos;

	// Token: 0x04001B39 RID: 6969
	private Quaternion interpStartRot;

	// Token: 0x04001B3A RID: 6970
	protected int enabledOnFrame = -1;

	// Token: 0x04001B3B RID: 6971
	private Vector3 initOffset;

	// Token: 0x04001B3C RID: 6972
	private Quaternion initRotation;

	// Token: 0x04001B3D RID: 6973
	public bool canDrop;

	// Token: 0x04001B3E RID: 6974
	public bool shareable;

	// Token: 0x04001B3F RID: 6975
	public bool detatchOnGrab;

	// Token: 0x04001B40 RID: 6976
	private bool wasHover;

	// Token: 0x04001B41 RID: 6977
	private bool isHover;

	// Token: 0x04001B42 RID: 6978
	private bool disableItem;

	// Token: 0x04001B43 RID: 6979
	public const int kPositionStateCount = 8;

	// Token: 0x04001B44 RID: 6980
	public LegacyTransferrableObject.InterpolateState interpState;

	// Token: 0x020003F1 RID: 1009
	public enum InterpolateState
	{
		// Token: 0x04001B46 RID: 6982
		None,
		// Token: 0x04001B47 RID: 6983
		Interpolating
	}
}
