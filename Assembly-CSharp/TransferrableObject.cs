using System;
using GorillaExtensions;
using GorillaTag;
using GorillaTag.CosmeticSystem;
using JetBrains.Annotations;
using Photon.Pun;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.XR;

// Token: 0x0200040E RID: 1038
public class TransferrableObject : HoldableObject, ISelfValidator, IRequestableOwnershipGuardCallbacks, IPreDisable, ISpawnable, IBuildValidation
{
	// Token: 0x06001976 RID: 6518 RVA: 0x000412BB File Offset: 0x0003F4BB
	public void FixTransformOverride()
	{
		this.transferrableItemSlotTransformOverride = base.GetComponent<TransferrableItemSlotTransformOverride>();
	}

	// Token: 0x06001977 RID: 6519 RVA: 0x00030607 File Offset: 0x0002E807
	public void Validate(SelfValidationResult result)
	{
	}

	// Token: 0x170002C6 RID: 710
	// (get) Token: 0x06001978 RID: 6520 RVA: 0x000412C9 File Offset: 0x0003F4C9
	// (set) Token: 0x06001979 RID: 6521 RVA: 0x000412D1 File Offset: 0x0003F4D1
	public VRRig myRig
	{
		get
		{
			return this._myRig;
		}
		private set
		{
			this._myRig = value;
		}
	}

	// Token: 0x170002C7 RID: 711
	// (get) Token: 0x0600197A RID: 6522 RVA: 0x000412DA File Offset: 0x0003F4DA
	// (set) Token: 0x0600197B RID: 6523 RVA: 0x000412E2 File Offset: 0x0003F4E2
	public bool isMyRigValid { get; private set; }

	// Token: 0x170002C8 RID: 712
	// (get) Token: 0x0600197C RID: 6524 RVA: 0x000412EB File Offset: 0x0003F4EB
	// (set) Token: 0x0600197D RID: 6525 RVA: 0x000412F3 File Offset: 0x0003F4F3
	public VRRig myOnlineRig
	{
		get
		{
			return this._myOnlineRig;
		}
		private set
		{
			this._myOnlineRig = value;
			this.isMyOnlineRigValid = true;
		}
	}

	// Token: 0x170002C9 RID: 713
	// (get) Token: 0x0600197E RID: 6526 RVA: 0x00041303 File Offset: 0x0003F503
	// (set) Token: 0x0600197F RID: 6527 RVA: 0x0004130B File Offset: 0x0003F50B
	public bool isMyOnlineRigValid { get; private set; }

	// Token: 0x06001980 RID: 6528 RVA: 0x000D0F98 File Offset: 0x000CF198
	public void SetTargetRig(VRRig rig)
	{
		if (rig == null)
		{
			this.targetRigSet = false;
			if (this.isSceneObject)
			{
				this.targetRig = rig;
				this.targetDockPositions = null;
				this.anchorOverrides = null;
				return;
			}
			if (this.myRig)
			{
				this.SetTargetRig(this.myRig);
			}
			if (this.myOnlineRig)
			{
				this.SetTargetRig(this.myOnlineRig);
			}
			return;
		}
		else
		{
			this.targetRigSet = true;
			this.targetRig = rig;
			BodyDockPositions component = rig.GetComponent<BodyDockPositions>();
			VRRigAnchorOverrides component2 = rig.GetComponent<VRRigAnchorOverrides>();
			if (!component)
			{
				Debug.LogError("There is no dock attached to this rig", this);
				return;
			}
			if (!component2)
			{
				Debug.LogError("There is no overrides attached to this rig", this);
				return;
			}
			this.anchorOverrides = component2;
			this.targetDockPositions = component;
			if (this.interpState == TransferrableObject.InterpolateState.Interpolating)
			{
				this.interpState = TransferrableObject.InterpolateState.None;
			}
			return;
		}
	}

	// Token: 0x170002CA RID: 714
	// (get) Token: 0x06001981 RID: 6529 RVA: 0x00041314 File Offset: 0x0003F514
	public bool IsLocalOwnedWorldShareable
	{
		get
		{
			return this.worldShareableInstance && this.worldShareableInstance.guard.isTrulyMine;
		}
	}

	// Token: 0x06001982 RID: 6530 RVA: 0x000D1068 File Offset: 0x000CF268
	public void WorldShareableRequestOwnership()
	{
		if (this.worldShareableInstance != null && !this.worldShareableInstance.guard.isMine)
		{
			this.worldShareableInstance.guard.RequestOwnershipImmediately(delegate
			{
			});
		}
	}

	// Token: 0x170002CB RID: 715
	// (get) Token: 0x06001983 RID: 6531 RVA: 0x00041335 File Offset: 0x0003F535
	// (set) Token: 0x06001984 RID: 6532 RVA: 0x0004133D File Offset: 0x0003F53D
	public bool isRigidbodySet { get; private set; }

	// Token: 0x170002CC RID: 716
	// (get) Token: 0x06001985 RID: 6533 RVA: 0x00041346 File Offset: 0x0003F546
	// (set) Token: 0x06001986 RID: 6534 RVA: 0x0004134E File Offset: 0x0003F54E
	public bool shouldUseGravity { get; private set; }

	// Token: 0x06001987 RID: 6535 RVA: 0x00041357 File Offset: 0x0003F557
	protected virtual void Awake()
	{
		if (this.isSceneObject)
		{
			this.IsSpawned = true;
			this.OnSpawn(null);
		}
	}

	// Token: 0x170002CD RID: 717
	// (get) Token: 0x06001988 RID: 6536 RVA: 0x0004136F File Offset: 0x0003F56F
	// (set) Token: 0x06001989 RID: 6537 RVA: 0x00041377 File Offset: 0x0003F577
	public bool IsSpawned { get; set; }

	// Token: 0x170002CE RID: 718
	// (get) Token: 0x0600198A RID: 6538 RVA: 0x00041380 File Offset: 0x0003F580
	// (set) Token: 0x0600198B RID: 6539 RVA: 0x00041388 File Offset: 0x0003F588
	public ECosmeticSelectSide CosmeticSelectedSide { get; set; }

	// Token: 0x0600198C RID: 6540 RVA: 0x000D10C4 File Offset: 0x000CF2C4
	public virtual void OnSpawn(VRRig rig)
	{
		try
		{
			if (!this.isSceneObject)
			{
				if (!rig)
				{
					Debug.LogError("Disabling TransferrableObject because could not find VRRig! \"" + base.transform.GetPath() + "\"", this);
					base.enabled = false;
					this.isMyRigValid = false;
					this.isMyOnlineRigValid = false;
					return;
				}
				this.myRig = (rig.isOfflineVRRig ? rig : null);
				this.myOnlineRig = (rig.isOfflineVRRig ? null : rig);
			}
			else
			{
				this.myRig = null;
				this.myOnlineRig = null;
			}
			this.isMyRigValid = true;
			this.isMyOnlineRigValid = true;
			this.targetDockPositions = base.GetComponentInParent<BodyDockPositions>();
			this.anchor = base.transform.parent;
			if (this.rigidbodyInstance == null)
			{
				this.rigidbodyInstance = base.GetComponent<Rigidbody>();
			}
			if (this.rigidbodyInstance != null)
			{
				this.isRigidbodySet = true;
				this.shouldUseGravity = this.rigidbodyInstance.useGravity;
			}
			this.audioSrc = base.GetComponent<AudioSource>();
			this.latched = false;
			if (!this.positionInitialized)
			{
				this.SetInitMatrix();
				this.positionInitialized = true;
			}
			if (this.anchor == null)
			{
				this.InitialDockObject = base.transform.parent;
			}
			else
			{
				this.InitialDockObject = this.anchor.parent;
			}
			this.isGrabAnchorSet = (this.grabAnchor != null);
			if (this.isSceneObject)
			{
				foreach (ISpawnable spawnable in base.GetComponentsInChildren<ISpawnable>(true))
				{
					if (spawnable != this)
					{
						spawnable.IsSpawned = true;
						spawnable.CosmeticSelectedSide = this.CosmeticSelectedSide;
						spawnable.OnSpawn(this.myRig);
					}
				}
			}
		}
		catch (Exception exception)
		{
			Debug.LogException(exception, this);
			base.enabled = false;
			base.gameObject.SetActive(false);
			Debug.LogError("TransferrableObject: Disabled & deactivated self because of the exception logged above. Path: " + base.transform.GetPathQ(), this);
		}
	}

	// Token: 0x0600198D RID: 6541 RVA: 0x000D12C0 File Offset: 0x000CF4C0
	public virtual void OnDespawn()
	{
		try
		{
			if (!this.isSceneObject)
			{
				foreach (ISpawnable spawnable in base.GetComponentsInChildren<ISpawnable>(true))
				{
					if (spawnable != this)
					{
						spawnable.IsSpawned = false;
						spawnable.OnDespawn();
					}
				}
			}
		}
		catch (Exception exception)
		{
			Debug.LogException(exception, this);
			base.enabled = false;
			base.gameObject.SetActive(false);
			Debug.LogError("TransferrableObject: Disabled & deactivated self because of the exception logged above. Path: " + base.transform.GetPathQ(), this);
		}
	}

	// Token: 0x0600198E RID: 6542 RVA: 0x000D1348 File Offset: 0x000CF548
	private void SetInitMatrix()
	{
		this.initMatrix = base.transform.LocalMatrixRelativeToParentWithScale();
		if (this.handPoseLeft != null)
		{
			base.transform.localRotation = TransferrableObject.handPoseLeftReferenceRotation * Quaternion.Inverse(this.handPoseLeft.localRotation);
			base.transform.position += base.transform.parent.TransformPoint(TransferrableObject.handPoseLeftReferencePoint) - this.handPoseLeft.transform.position;
			this.leftHandMatrix = base.transform.LocalMatrixRelativeToParentWithScale();
		}
		else
		{
			this.leftHandMatrix = this.initMatrix;
		}
		if (this.handPoseRight != null)
		{
			base.transform.localRotation = TransferrableObject.handPoseRightReferenceRotation * Quaternion.Inverse(this.handPoseRight.localRotation);
			base.transform.position += base.transform.parent.TransformPoint(TransferrableObject.handPoseRightReferencePoint) - this.handPoseRight.transform.position;
			this.rightHandMatrix = base.transform.LocalMatrixRelativeToParentWithScale();
		}
		else
		{
			this.rightHandMatrix = this.initMatrix;
		}
		base.transform.localPosition = this.initMatrix.Position();
		base.transform.localRotation = this.initMatrix.Rotation();
		this.positionInitialized = true;
	}

	// Token: 0x0600198F RID: 6543 RVA: 0x00030607 File Offset: 0x0002E807
	protected virtual void Start()
	{
	}

	// Token: 0x06001990 RID: 6544 RVA: 0x000D14C0 File Offset: 0x000CF6C0
	internal virtual void OnEnable()
	{
		try
		{
			if (!ApplicationQuittingState.IsQuitting)
			{
				RoomSystem.JoinedRoomEvent = (Action)Delegate.Combine(RoomSystem.JoinedRoomEvent, new Action(this.OnJoinedRoom));
				RoomSystem.LeftRoomEvent = (Action)Delegate.Combine(RoomSystem.LeftRoomEvent, new Action(this.OnLeftRoom));
				if (!this.isSceneObject && !CosmeticsV2Spawner_Dirty.allPartsInstantiated)
				{
					Debug.LogError("`TransferrableObject.OnEnable()` was called before allPartsInstantiated was true. Path: " + base.transform.GetPathQ(), this);
					if (!this._isListeningFor_OnPostInstantiateAllPrefabs2)
					{
						this._isListeningFor_OnPostInstantiateAllPrefabs2 = true;
						CosmeticsV2Spawner_Dirty.OnPostInstantiateAllPrefabs2 = (Action)Delegate.Combine(CosmeticsV2Spawner_Dirty.OnPostInstantiateAllPrefabs2, new Action(this.OnEnable_AfterAllCosmeticsSpawnedOrIsSceneObject));
					}
				}
				else
				{
					this.OnEnable_AfterAllCosmeticsSpawnedOrIsSceneObject();
				}
			}
		}
		catch (Exception exception)
		{
			Debug.LogException(exception, this);
			base.enabled = false;
			base.gameObject.SetActive(false);
			Debug.LogError("TransferrableObject: Disabled & deactivated self because of the exception logged above. Path: " + base.transform.GetPathQ(), this);
		}
	}

	// Token: 0x06001991 RID: 6545 RVA: 0x000D15C8 File Offset: 0x000CF7C8
	public virtual void OnEnable_AfterAllCosmeticsSpawnedOrIsSceneObject()
	{
		if (ApplicationQuittingState.IsQuitting)
		{
			return;
		}
		if (!base.enabled)
		{
			base.gameObject.SetActive(false);
			return;
		}
		this._isListeningFor_OnPostInstantiateAllPrefabs2 = false;
		CosmeticsV2Spawner_Dirty.OnPostInstantiateAllPrefabs2 = (Action)Delegate.Remove(CosmeticsV2Spawner_Dirty.OnPostInstantiateAllPrefabs2, new Action(this.OnEnable_AfterAllCosmeticsSpawnedOrIsSceneObject));
		if (!base.isActiveAndEnabled)
		{
			return;
		}
		try
		{
			TransferrableObjectManager.Register(this);
			this.transferrableItemSlotTransformOverride = base.GetComponent<TransferrableItemSlotTransformOverride>();
			if (!this.positionInitialized)
			{
				this.SetInitMatrix();
				this.positionInitialized = true;
			}
			if (this.isSceneObject)
			{
				if (!this.worldShareableInstance)
				{
					Debug.LogError("Missing Sharable Instance on Scene enabled object: " + base.gameObject.name);
				}
				else
				{
					this.worldShareableInstance.SyncToSceneObject(this);
					this.worldShareableInstance.GetComponent<RequestableOwnershipGuard>().AddCallbackTarget(this);
				}
			}
			else
			{
				if (!this.isSceneObject && !this.myRig && !this.myOnlineRig && !this.ownerRig)
				{
					this.ownerRig = base.GetComponentInParent<VRRig>(true);
					if (this.ownerRig.isOfflineVRRig)
					{
						this.myRig = this.ownerRig;
					}
					else
					{
						this.myOnlineRig = this.ownerRig;
					}
				}
				if (!this.myRig && this.myOnlineRig)
				{
					this.ownerRig = this.myOnlineRig;
					this.SetTargetRig(this.myOnlineRig);
				}
				if (this.myRig == null && this.myOnlineRig == null)
				{
					if (!this.isSceneObject)
					{
						base.gameObject.SetActive(false);
					}
				}
				else
				{
					this.objectIndex = this.targetDockPositions.ReturnTransferrableItemIndex(this.myIndex);
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
					else if (this.currentState == TransferrableObject.PositionState.OnChest)
					{
						this.storedZone = BodyDockPositions.DropPositions.Chest;
					}
					if (this.IsLocalObject())
					{
						this.ownerRig = GorillaTagger.Instance.offlineVRRig;
						this.SetTargetRig(GorillaTagger.Instance.offlineVRRig);
					}
					if (this.objectIndex == -1)
					{
						base.gameObject.SetActive(false);
					}
					else
					{
						if (this.currentState == TransferrableObject.PositionState.OnLeftArm && this.flipOnXForLeftArm)
						{
							Transform transform = this.GetAnchor(this.currentState);
							transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
						}
						this.initState = this.currentState;
						this.enabledOnFrame = Time.frameCount;
						this.startInterpolation = true;
						if (NetworkSystem.Instance.InRoom)
						{
							if (this.canDrop || this.shareable)
							{
								this.SpawnTransferableObjectViews();
								if (this.myRig)
								{
									if (this.myRig != null && this.worldShareableInstance != null)
									{
										this.OnWorldShareableItemSpawn();
									}
								}
							}
						}
					}
				}
			}
		}
		catch (Exception exception)
		{
			Debug.LogException(exception, this);
			base.enabled = false;
			base.gameObject.SetActive(false);
			Debug.LogError("TransferrableObject: Disabled & deactivated self because of the exception logged above. Path: " + base.transform.GetPathQ(), this);
		}
	}

	// Token: 0x06001992 RID: 6546 RVA: 0x000D1938 File Offset: 0x000CFB38
	internal virtual void OnDisable()
	{
		TransferrableObjectManager.Unregister(this);
		if (ApplicationQuittingState.IsQuitting)
		{
			return;
		}
		RoomSystem.JoinedRoomEvent = (Action)Delegate.Remove(RoomSystem.JoinedRoomEvent, new Action(this.OnJoinedRoom));
		RoomSystem.LeftRoomEvent = (Action)Delegate.Remove(RoomSystem.LeftRoomEvent, new Action(this.OnLeftRoom));
		this._isListeningFor_OnPostInstantiateAllPrefabs2 = false;
		CosmeticsV2Spawner_Dirty.OnPostInstantiateAllPrefabs2 = (Action)Delegate.Remove(CosmeticsV2Spawner_Dirty.OnPostInstantiateAllPrefabs2, new Action(this.OnEnable_AfterAllCosmeticsSpawnedOrIsSceneObject));
		this.enabledOnFrame = -1;
		base.transform.localScale = Vector3.one;
		try
		{
			if (!this.isSceneObject && this.IsLocalObject() && this.worldShareableInstance && !this.IsMyItem())
			{
				this.worldShareableInstance.GetComponent<RequestableOwnershipGuard>().RequestOwnershipImmediately(delegate
				{
				});
			}
			if (this.worldShareableInstance)
			{
				this.worldShareableInstance.Invalidate();
				this.worldShareableInstance.GetComponent<RequestableOwnershipGuard>().RemoveCallbackTarget(this);
				if (this.targetDockPositions)
				{
					this.targetDockPositions.DeallocateSharableInstance(this.worldShareableInstance);
				}
				if (!this.isSceneObject)
				{
					this.worldShareableInstance = null;
				}
			}
			this.PlayDestroyedOrDisabledEffect();
			if (this.isSceneObject)
			{
				this.IsSpawned = false;
				this.OnDespawn();
			}
		}
		catch (Exception exception)
		{
			Debug.LogException(exception, this);
			base.enabled = false;
			base.gameObject.SetActive(false);
			Debug.LogError("TransferrableObject: Disabled & deactivated self because of the exception logged above. Path: " + base.transform.GetPathQ(), this);
		}
	}

	// Token: 0x06001993 RID: 6547 RVA: 0x00041391 File Offset: 0x0003F591
	protected virtual void OnDestroy()
	{
		TransferrableObjectManager.Unregister(this);
	}

	// Token: 0x06001994 RID: 6548 RVA: 0x000D1AE4 File Offset: 0x000CFCE4
	public void CleanupDisable()
	{
		this.currentState = TransferrableObject.PositionState.None;
		this.enabledOnFrame = -1;
		if (this.anchor)
		{
			this.anchor.parent = this.InitialDockObject;
			if (this.anchor != base.transform)
			{
				base.transform.parent = this.anchor;
			}
		}
		else
		{
			base.transform.parent = this.anchor;
		}
		this.interpState = TransferrableObject.InterpolateState.None;
		Transform transform = base.transform;
		Matrix4x4 defaultTransformationMatrix = this.GetDefaultTransformationMatrix();
		transform.SetLocalMatrixRelativeToParentWithXParity(defaultTransformationMatrix);
	}

	// Token: 0x06001995 RID: 6549 RVA: 0x00041399 File Offset: 0x0003F599
	public virtual void PreDisable()
	{
		this.itemState = TransferrableObject.ItemStates.State0;
		this.currentState = TransferrableObject.PositionState.None;
		this.interpState = TransferrableObject.InterpolateState.None;
		this.ResetToDefaultState();
	}

	// Token: 0x06001996 RID: 6550 RVA: 0x000D1B70 File Offset: 0x000CFD70
	public virtual Matrix4x4 GetDefaultTransformationMatrix()
	{
		TransferrableObject.PositionState positionState = this.currentState;
		if (positionState == TransferrableObject.PositionState.InLeftHand)
		{
			return this.leftHandMatrix;
		}
		if (positionState != TransferrableObject.PositionState.InRightHand)
		{
			return this.initMatrix;
		}
		return this.rightHandMatrix;
	}

	// Token: 0x06001997 RID: 6551 RVA: 0x000413B6 File Offset: 0x0003F5B6
	public virtual bool ShouldBeKinematic()
	{
		if (this.detatchOnGrab)
		{
			return this.currentState != TransferrableObject.PositionState.Dropped && this.currentState != TransferrableObject.PositionState.InLeftHand && this.currentState != TransferrableObject.PositionState.InRightHand;
		}
		return this.currentState != TransferrableObject.PositionState.Dropped;
	}

	// Token: 0x06001998 RID: 6552 RVA: 0x000D1BA4 File Offset: 0x000CFDA4
	private void SpawnShareableObject()
	{
		if (this.isSceneObject)
		{
			if (this.worldShareableInstance == null)
			{
				return;
			}
			this.worldShareableInstance.GetComponent<WorldShareableItem>().SetupSceneObjectOnNetwork(NetworkSystem.Instance.MasterClient);
			return;
		}
		else
		{
			if (!NetworkSystem.Instance.InRoom)
			{
				return;
			}
			this.SpawnTransferableObjectViews();
			if (!this.myRig)
			{
				return;
			}
			if (!this.canDrop && !this.shareable)
			{
				return;
			}
			if (this.myRig != null && this.worldShareableInstance != null)
			{
				this.OnWorldShareableItemSpawn();
			}
			return;
		}
	}

	// Token: 0x06001999 RID: 6553 RVA: 0x000D1C38 File Offset: 0x000CFE38
	public void SpawnTransferableObjectViews()
	{
		NetPlayer owner = NetworkSystem.Instance.LocalPlayer;
		if (!this.ownerRig.isOfflineVRRig)
		{
			owner = this.ownerRig.creator;
		}
		if (this.worldShareableInstance == null)
		{
			this.worldShareableInstance = this.targetDockPositions.AllocateSharableInstance(this.storedZone, owner);
		}
		GorillaTagger.OnPlayerSpawned(delegate
		{
			this.worldShareableInstance.SetupSharableObject(this.myIndex, owner, this.transform);
		});
	}

	// Token: 0x0600199A RID: 6554 RVA: 0x000D1CBC File Offset: 0x000CFEBC
	public virtual void OnJoinedRoom()
	{
		if (this.isSceneObject)
		{
			this.worldShareableInstance == null;
			return;
		}
		if (!NetworkSystem.Instance.InRoom)
		{
			return;
		}
		if (!this.canDrop && !this.shareable)
		{
			return;
		}
		this.SpawnTransferableObjectViews();
		if (!this.myRig)
		{
			return;
		}
		if (this.myRig != null && this.worldShareableInstance != null)
		{
			this.OnWorldShareableItemSpawn();
		}
	}

	// Token: 0x0600199B RID: 6555 RVA: 0x000D1D34 File Offset: 0x000CFF34
	public virtual void OnLeftRoom()
	{
		if (ApplicationQuittingState.IsQuitting)
		{
			return;
		}
		if (this.isSceneObject)
		{
			return;
		}
		if (!this.shareable && !this.allowWorldSharableInstance && !this.canDrop)
		{
			return;
		}
		if (base.gameObject.activeSelf && this.worldShareableInstance)
		{
			this.worldShareableInstance.Invalidate();
			this.worldShareableInstance.GetComponent<RequestableOwnershipGuard>().RemoveCallbackTarget(this);
			if (this.targetDockPositions)
			{
				this.targetDockPositions.DeallocateSharableInstance(this.worldShareableInstance);
			}
			else
			{
				this.worldShareableInstance.ResetViews();
				ObjectPools.instance.Destroy(this.worldShareableInstance.gameObject);
			}
			this.worldShareableInstance = null;
		}
		if (!this.IsLocalObject())
		{
			this.OnItemDestroyedOrDisabled();
			base.gameObject.Disable();
			return;
		}
	}

	// Token: 0x0600199C RID: 6556 RVA: 0x000413F3 File Offset: 0x0003F5F3
	public bool IsLocalObject()
	{
		return this.myRig != null && this.myRig.isOfflineVRRig;
	}

	// Token: 0x0600199D RID: 6557 RVA: 0x0004140A File Offset: 0x0003F60A
	public void SetWorldShareableItem(WorldShareableItem item)
	{
		this.worldShareableInstance = item;
		this.OnWorldShareableItemSpawn();
	}

	// Token: 0x0600199E RID: 6558 RVA: 0x00030607 File Offset: 0x0002E807
	protected virtual void OnWorldShareableItemSpawn()
	{
	}

	// Token: 0x0600199F RID: 6559 RVA: 0x00030607 File Offset: 0x0002E807
	protected virtual void PlayDestroyedOrDisabledEffect()
	{
	}

	// Token: 0x060019A0 RID: 6560 RVA: 0x000D1E04 File Offset: 0x000D0004
	protected virtual void OnItemDestroyedOrDisabled()
	{
		if (this.worldShareableInstance)
		{
			this.worldShareableInstance.Invalidate();
			this.worldShareableInstance.GetComponent<RequestableOwnershipGuard>().RemoveCallbackTarget(this);
			if (this.targetDockPositions)
			{
				this.targetDockPositions.DeallocateSharableInstance(this.worldShareableInstance);
			}
			Debug.LogError("Setting WSI to null in OnItemDestroyedOrDisabled", this);
			this.worldShareableInstance = null;
		}
		this.PlayDestroyedOrDisabledEffect();
		this.enabledOnFrame = -1;
		this.currentState = TransferrableObject.PositionState.None;
	}

	// Token: 0x060019A1 RID: 6561 RVA: 0x00041419 File Offset: 0x0003F619
	public virtual void TriggeredLateUpdate()
	{
		if (this.IsLocalObject() && this.canDrop)
		{
			this.LocalMyObjectValidation();
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
	}

	// Token: 0x060019A2 RID: 6562 RVA: 0x0004144D File Offset: 0x0003F64D
	protected Transform DefaultAnchor()
	{
		if (this._isDefaultAnchorSet)
		{
			return this._defaultAnchor;
		}
		this._isDefaultAnchorSet = true;
		this._defaultAnchor = ((this.anchor == null) ? base.transform : this.anchor);
		return this._defaultAnchor;
	}

	// Token: 0x060019A3 RID: 6563 RVA: 0x0004148D File Offset: 0x0003F68D
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

	// Token: 0x060019A4 RID: 6564 RVA: 0x000D1E80 File Offset: 0x000D0080
	protected bool Attached()
	{
		bool flag = this.InHand() && this.detatchOnGrab;
		return !this.Dropped() && !flag;
	}

	// Token: 0x060019A5 RID: 6565 RVA: 0x000D1EB0 File Offset: 0x000D00B0
	private Transform GetTargetStorageZone(BodyDockPositions.DropPositions state)
	{
		switch (state)
		{
		case BodyDockPositions.DropPositions.None:
			return null;
		case BodyDockPositions.DropPositions.LeftArm:
			return this.targetDockPositions.leftArmTransform;
		case BodyDockPositions.DropPositions.RightArm:
			return this.targetDockPositions.rightArmTransform;
		case BodyDockPositions.DropPositions.LeftArm | BodyDockPositions.DropPositions.RightArm:
		case BodyDockPositions.DropPositions.MaxDropPostions:
		case BodyDockPositions.DropPositions.RightArm | BodyDockPositions.DropPositions.Chest:
		case BodyDockPositions.DropPositions.LeftArm | BodyDockPositions.DropPositions.RightArm | BodyDockPositions.DropPositions.Chest:
			break;
		case BodyDockPositions.DropPositions.Chest:
			return this.targetDockPositions.chestTransform;
		case BodyDockPositions.DropPositions.LeftBack:
			return this.targetDockPositions.leftBackTransform;
		default:
			if (state == BodyDockPositions.DropPositions.RightBack)
			{
				return this.targetDockPositions.rightBackTransform;
			}
			break;
		}
		throw new ArgumentOutOfRangeException();
	}

	// Token: 0x060019A6 RID: 6566 RVA: 0x000414B9 File Offset: 0x0003F6B9
	public static Transform GetTargetDock(TransferrableObject.PositionState state, VRRig rig)
	{
		return TransferrableObject.GetTargetDock(state, rig.myBodyDockPositions, rig.GetComponent<VRRigAnchorOverrides>());
	}

	// Token: 0x060019A7 RID: 6567 RVA: 0x000D1F34 File Offset: 0x000D0134
	public static Transform GetTargetDock(TransferrableObject.PositionState state, BodyDockPositions dockPositions, VRRigAnchorOverrides anchorOverrides)
	{
		if (state <= TransferrableObject.PositionState.InRightHand)
		{
			switch (state)
			{
			case TransferrableObject.PositionState.OnLeftArm:
				return anchorOverrides.AnchorOverride(state, dockPositions.leftArmTransform);
			case TransferrableObject.PositionState.OnRightArm:
				return anchorOverrides.AnchorOverride(state, dockPositions.rightArmTransform);
			case TransferrableObject.PositionState.OnLeftArm | TransferrableObject.PositionState.OnRightArm:
				break;
			case TransferrableObject.PositionState.InLeftHand:
				return anchorOverrides.AnchorOverride(state, dockPositions.leftHandTransform);
			default:
				if (state == TransferrableObject.PositionState.InRightHand)
				{
					return anchorOverrides.AnchorOverride(state, dockPositions.rightHandTransform);
				}
				break;
			}
		}
		else
		{
			if (state == TransferrableObject.PositionState.OnChest)
			{
				return anchorOverrides.AnchorOverride(state, dockPositions.chestTransform);
			}
			if (state == TransferrableObject.PositionState.OnLeftShoulder)
			{
				return anchorOverrides.AnchorOverride(state, dockPositions.leftBackTransform);
			}
			if (state == TransferrableObject.PositionState.OnRightShoulder)
			{
				return anchorOverrides.AnchorOverride(state, dockPositions.rightBackTransform);
			}
		}
		return null;
	}

	// Token: 0x060019A8 RID: 6568 RVA: 0x000D1FD8 File Offset: 0x000D01D8
	private void UpdateFollowXform()
	{
		if (!this.targetRigSet)
		{
			return;
		}
		Transform transform = this.GetAnchor(this.currentState);
		Transform transform2 = transform;
		try
		{
			transform2 = TransferrableObject.GetTargetDock(this.currentState, this.targetDockPositions, this.anchorOverrides);
		}
		catch
		{
			Debug.LogError("anchorOverrides or targetDock has been destroyed", this);
			this.SetTargetRig(null);
		}
		if (this.currentState != TransferrableObject.PositionState.Dropped && this.rigidbodyInstance && this.ShouldBeKinematic() && !this.rigidbodyInstance.isKinematic)
		{
			this.rigidbodyInstance.isKinematic = true;
		}
		if (this.detatchOnGrab && (this.currentState == TransferrableObject.PositionState.InLeftHand || this.currentState == TransferrableObject.PositionState.InRightHand))
		{
			base.transform.parent = null;
		}
		if (this.interpState == TransferrableObject.InterpolateState.None)
		{
			try
			{
				if (transform == null)
				{
					return;
				}
				this.startInterpolation |= (transform2 != transform.parent);
			}
			catch
			{
			}
			if (!this.startInterpolation && !this.isGrabAnchorSet && base.transform.parent != transform && transform != base.transform)
			{
				this.startInterpolation = true;
			}
			if (this.startInterpolation)
			{
				Vector3 position = base.transform.position;
				Quaternion rotation = base.transform.rotation;
				if (base.transform.parent != transform && transform != base.transform)
				{
					base.transform.parent = transform;
				}
				transform.parent = transform2;
				transform.localPosition = Vector3.zero;
				transform.localRotation = Quaternion.identity;
				if (this.currentState == TransferrableObject.PositionState.InLeftHand)
				{
					if (this.flipOnXForLeftHand)
					{
						transform.localScale = new Vector3(-1f, 1f, 1f);
					}
					else if (this.flipOnYForLeftHand)
					{
						transform.localScale = new Vector3(1f, -1f, 1f);
					}
					else
					{
						transform.localScale = Vector3.one;
					}
				}
				else
				{
					transform.localScale = Vector3.one;
				}
				if (Time.frameCount == this.enabledOnFrame || Time.frameCount == this.enabledOnFrame + 1)
				{
					Matrix4x4 rhs = this.GetDefaultTransformationMatrix();
					if ((this.currentState != TransferrableObject.PositionState.InLeftHand || !(this.handPoseLeft != null)) && this.currentState == TransferrableObject.PositionState.InRightHand)
					{
						this.handPoseRight != null;
					}
					Matrix4x4 matrix4x;
					if (this.transferrableItemSlotTransformOverride && this.transferrableItemSlotTransformOverride.GetTransformFromPositionState(this.currentState, this.advancedGrabState, transform2, out matrix4x))
					{
						rhs = matrix4x;
					}
					Matrix4x4 matrix = transform.localToWorldMatrix * rhs;
					base.transform.SetLocalToWorldMatrixNoScale(matrix);
					base.transform.localScale = matrix.lossyScale;
				}
				else
				{
					this.interpState = TransferrableObject.InterpolateState.Interpolating;
					if (this.IsMyItem() && this.useGrabType == TransferrableObject.GrabType.Free)
					{
						bool flag = this.currentState == TransferrableObject.PositionState.InLeftHand;
						if (!flag)
						{
							GameObject rightHand = EquipmentInteractor.instance.rightHand;
						}
						else
						{
							GameObject leftHand = EquipmentInteractor.instance.leftHand;
						}
						Transform targetDock = TransferrableObject.GetTargetDock(this.currentState, GorillaTagger.Instance.offlineVRRig);
						this.SetupMatrixForFreeGrab(position, rotation, targetDock, flag);
					}
					this.interpDt = this.interpTime;
					this.interpStartRot = rotation;
					this.interpStartPos = position;
					base.transform.position = position;
					base.transform.rotation = rotation;
				}
				this.startInterpolation = false;
			}
		}
		if (this.interpState == TransferrableObject.InterpolateState.Interpolating)
		{
			Matrix4x4 rhs2 = this.GetDefaultTransformationMatrix();
			if (this.transferrableItemSlotTransformOverride != null)
			{
				if (this.transferrableItemSlotTransformOverrideCachedMatrix == null)
				{
					Matrix4x4 value;
					this.transferrableItemSlotTransformOverrideApplicable = this.transferrableItemSlotTransformOverride.GetTransformFromPositionState(this.currentState, this.advancedGrabState, transform2, out value);
					this.transferrableItemSlotTransformOverrideCachedMatrix = new Matrix4x4?(value);
				}
				if (this.transferrableItemSlotTransformOverrideApplicable)
				{
					rhs2 = this.transferrableItemSlotTransformOverrideCachedMatrix.Value;
				}
			}
			float t = Mathf.Clamp((this.interpTime - this.interpDt) / this.interpTime, 0f, 1f);
			Mathf.SmoothStep(0f, 1f, t);
			Matrix4x4 matrix2 = transform.localToWorldMatrix * rhs2;
			Transform transform3 = base.transform;
			Vector3 vector = matrix2.Position();
			transform3.position = this.interpStartPos.LerpToUnclamped(vector, t);
			base.transform.rotation = Quaternion.Slerp(this.interpStartRot, matrix2.Rotation(), t);
			base.transform.localScale = rhs2.lossyScale;
			this.interpDt -= Time.deltaTime;
			if (this.interpDt <= 0f)
			{
				transform.parent = transform2;
				this.interpState = TransferrableObject.InterpolateState.None;
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
				matrix2 = transform.localToWorldMatrix * rhs2;
				base.transform.SetLocalToWorldMatrixNoScale(matrix2);
				base.transform.localScale = rhs2.lossyScale;
			}
		}
	}

	// Token: 0x060019A9 RID: 6569 RVA: 0x000D2518 File Offset: 0x000D0718
	public virtual void DropItem()
	{
		if (EquipmentInteractor.instance.leftHandHeldEquipment == this)
		{
			GorillaTagger.Instance.StartVibration(true, GorillaTagger.Instance.tapHapticStrength / 8f, GorillaTagger.Instance.tapHapticDuration * 0.5f);
			EquipmentInteractor.instance.UpdateHandEquipment(null, true);
		}
		if (EquipmentInteractor.instance.rightHandHeldEquipment == this)
		{
			GorillaTagger.Instance.StartVibration(true, GorillaTagger.Instance.tapHapticStrength / 8f, GorillaTagger.Instance.tapHapticDuration * 0.5f);
			EquipmentInteractor.instance.UpdateHandEquipment(null, false);
		}
		this.currentState = TransferrableObject.PositionState.Dropped;
		if (this.worldShareableInstance)
		{
			this.worldShareableInstance.transferableObjectState = this.currentState;
		}
		if (this.canDrop)
		{
			base.transform.parent = null;
			if (this.anchor)
			{
				this.anchor.parent = this.InitialDockObject;
			}
			if (this.rigidbodyInstance && this.ShouldBeKinematic() && !this.rigidbodyInstance.isKinematic)
			{
				this.rigidbodyInstance.isKinematic = true;
			}
		}
	}

	// Token: 0x060019AA RID: 6570 RVA: 0x00030607 File Offset: 0x0002E807
	protected virtual void OnStateChanged()
	{
	}

	// Token: 0x060019AB RID: 6571 RVA: 0x000D2640 File Offset: 0x000D0840
	protected virtual void LateUpdateShared()
	{
		this.disableItem = true;
		if (this.isSceneObject)
		{
			this.disableItem = false;
		}
		else
		{
			for (int i = 0; i < this.ownerRig.ActiveTransferrableObjectIndexLength(); i++)
			{
				if (this.ownerRig.ActiveTransferrableObjectIndex(i) == this.myIndex)
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
		}
		if (this.previousState != this.currentState)
		{
			this.previousState = this.currentState;
			if (!this.Attached())
			{
				base.transform.parent = null;
				if (!this.ShouldBeKinematic() && this.rigidbodyInstance.isKinematic)
				{
					this.rigidbodyInstance.isKinematic = false;
				}
			}
			if (this.currentState == TransferrableObject.PositionState.None)
			{
				this.ResetToHome();
			}
			this.transferrableItemSlotTransformOverrideCachedMatrix = null;
			if (this.interpState == TransferrableObject.InterpolateState.Interpolating)
			{
				this.interpState = TransferrableObject.InterpolateState.None;
			}
			this.OnStateChanged();
		}
		if (this.currentState == TransferrableObject.PositionState.Dropped)
		{
			if (!this.canDrop || this.allowReparenting)
			{
				goto IL_15A;
			}
			if (base.transform.parent != null)
			{
				base.transform.parent = null;
			}
			try
			{
				if (this.anchor != null && this.anchor.parent != this.InitialDockObject)
				{
					this.anchor.parent = this.InitialDockObject;
				}
				goto IL_15A;
			}
			catch
			{
				goto IL_15A;
			}
		}
		if (this.currentState != TransferrableObject.PositionState.None)
		{
			this.UpdateFollowXform();
		}
		IL_15A:
		if (!this.isRigidbodySet)
		{
			return;
		}
		if (this.rigidbodyInstance.isKinematic != this.ShouldBeKinematic())
		{
			this.rigidbodyInstance.isKinematic = this.ShouldBeKinematic();
			if (this.worldShareableInstance)
			{
				if (this.currentState == TransferrableObject.PositionState.Dropped)
				{
					this.worldShareableInstance.EnableRemoteSync = true;
					return;
				}
				this.worldShareableInstance.EnableRemoteSync = !this.ShouldBeKinematic();
			}
		}
	}

	// Token: 0x060019AC RID: 6572 RVA: 0x000D2820 File Offset: 0x000D0A20
	public virtual void ResetToHome()
	{
		if (this.isSceneObject)
		{
			this.currentState = TransferrableObject.PositionState.None;
		}
		this.ResetXf();
		if (!this.isRigidbodySet)
		{
			return;
		}
		if (this.ShouldBeKinematic() && !this.rigidbodyInstance.isKinematic)
		{
			this.rigidbodyInstance.isKinematic = true;
		}
	}

	// Token: 0x060019AD RID: 6573 RVA: 0x000D286C File Offset: 0x000D0A6C
	protected void ResetXf()
	{
		if (!this.positionInitialized)
		{
			this.initOffset = base.transform.localPosition;
			this.initRotation = base.transform.localRotation;
		}
		if (this.canDrop || this.allowWorldSharableInstance)
		{
			Transform transform = this.DefaultAnchor();
			if (base.transform != transform && base.transform.parent != transform)
			{
				base.transform.parent = transform;
			}
			if (this.ClearLocalPositionOnReset)
			{
				base.transform.localPosition = Vector3.zero;
				base.transform.localRotation = Quaternion.identity;
				base.transform.localScale = Vector3.one;
			}
			if (this.InitialDockObject)
			{
				this.anchor.localPosition = Vector3.zero;
				this.anchor.localRotation = Quaternion.identity;
				this.anchor.localScale = Vector3.one;
			}
			if (this.grabAnchor)
			{
				if (this.grabAnchor.parent != base.transform)
				{
					this.grabAnchor.parent = base.transform;
				}
				this.grabAnchor.localPosition = Vector3.zero;
				this.grabAnchor.localRotation = Quaternion.identity;
				this.grabAnchor.localScale = Vector3.one;
			}
			if (this.transferrableItemSlotTransformOverride)
			{
				Transform transformFromPositionState = this.transferrableItemSlotTransformOverride.GetTransformFromPositionState(this.currentState);
				if (transformFromPositionState)
				{
					base.transform.position = transformFromPositionState.position;
					base.transform.rotation = transformFromPositionState.rotation;
					return;
				}
				if (this.anchorOverrides != null)
				{
					Transform transform2 = this.GetAnchor(this.currentState);
					Transform targetDock = TransferrableObject.GetTargetDock(this.currentState, this.targetDockPositions, this.anchorOverrides);
					Matrix4x4 rhs = this.GetDefaultTransformationMatrix();
					Matrix4x4 matrix4x;
					if (this.transferrableItemSlotTransformOverride.GetTransformFromPositionState(this.currentState, this.advancedGrabState, targetDock, out matrix4x))
					{
						rhs = matrix4x;
					}
					Matrix4x4 matrix = transform2.localToWorldMatrix * rhs;
					base.transform.SetLocalToWorldMatrixNoScale(matrix);
					base.transform.localScale = matrix.lossyScale;
					return;
				}
			}
			else
			{
				base.transform.SetLocalMatrixRelativeToParent(this.GetDefaultTransformationMatrix());
			}
		}
	}

	// Token: 0x060019AE RID: 6574 RVA: 0x000D2AAC File Offset: 0x000D0CAC
	protected void ReDock()
	{
		if (this.IsMyItem())
		{
			this.currentState = this.initState;
		}
		if (this.rigidbodyInstance && this.ShouldBeKinematic() && !this.rigidbodyInstance.isKinematic)
		{
			this.rigidbodyInstance.isKinematic = true;
		}
		this.ResetXf();
	}

	// Token: 0x060019AF RID: 6575 RVA: 0x000D2B04 File Offset: 0x000D0D04
	private void HandleLocalInput()
	{
		Behaviour[] array2;
		if (this.Dropped())
		{
			foreach (GameObject gameObject in this.gameObjectsActiveOnlyWhileHeld)
			{
				if (gameObject.activeSelf)
				{
					gameObject.SetActive(false);
				}
			}
			array2 = this.behavioursEnabledOnlyWhileHeld;
			for (int i = 0; i < array2.Length; i++)
			{
				array2[i].enabled = false;
			}
			foreach (GameObject gameObject2 in this.gameObjectsActiveOnlyWhileDocked)
			{
				if (gameObject2.activeSelf)
				{
					gameObject2.SetActive(false);
				}
			}
			array2 = this.behavioursEnabledOnlyWhileDocked;
			for (int i = 0; i < array2.Length; i++)
			{
				array2[i].enabled = false;
			}
			return;
		}
		if (!this.InHand())
		{
			foreach (GameObject gameObject3 in this.gameObjectsActiveOnlyWhileHeld)
			{
				if (gameObject3.activeSelf)
				{
					gameObject3.SetActive(false);
				}
			}
			array2 = this.behavioursEnabledOnlyWhileHeld;
			for (int i = 0; i < array2.Length; i++)
			{
				array2[i].enabled = false;
			}
			foreach (GameObject gameObject4 in this.gameObjectsActiveOnlyWhileDocked)
			{
				if (!gameObject4.activeSelf)
				{
					gameObject4.SetActive(true);
				}
			}
			array2 = this.behavioursEnabledOnlyWhileDocked;
			for (int i = 0; i < array2.Length; i++)
			{
				array2[i].enabled = true;
			}
			return;
		}
		foreach (GameObject gameObject5 in this.gameObjectsActiveOnlyWhileHeld)
		{
			if (!gameObject5.activeSelf)
			{
				gameObject5.SetActive(true);
			}
		}
		array2 = this.behavioursEnabledOnlyWhileHeld;
		for (int i = 0; i < array2.Length; i++)
		{
			array2[i].enabled = true;
		}
		foreach (GameObject gameObject6 in this.gameObjectsActiveOnlyWhileDocked)
		{
			if (gameObject6.activeSelf)
			{
				gameObject6.SetActive(false);
			}
		}
		array2 = this.behavioursEnabledOnlyWhileDocked;
		for (int i = 0; i < array2.Length; i++)
		{
			array2[i].enabled = false;
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

	// Token: 0x060019B0 RID: 6576 RVA: 0x00030607 File Offset: 0x0002E807
	protected virtual void LocalMyObjectValidation()
	{
	}

	// Token: 0x060019B1 RID: 6577 RVA: 0x000D2D8C File Offset: 0x000D0F8C
	protected virtual void LocalPersistanceValidation()
	{
		if (this.maxDistanceFromOriginBeforeRespawn != 0f && Vector3.Distance(base.transform.position, this.originPoint.position) > this.maxDistanceFromOriginBeforeRespawn)
		{
			if (this.audioSrc != null && this.resetPositionAudioClip != null)
			{
				this.audioSrc.GTPlayOneShot(this.resetPositionAudioClip, 1f);
			}
			if (this.currentState != TransferrableObject.PositionState.Dropped)
			{
				this.DropItem();
				this.currentState = TransferrableObject.PositionState.Dropped;
			}
			base.transform.position = this.originPoint.position;
			if (!this.rigidbodyInstance.isKinematic)
			{
				this.rigidbodyInstance.velocity = Vector3.zero;
			}
		}
		if (this.rigidbodyInstance && this.rigidbodyInstance.velocity.sqrMagnitude > 10000f)
		{
			Debug.Log("Moving too fast, Assuming ive fallen out of the map. Ressetting position", this);
			this.ResetToHome();
		}
	}

	// Token: 0x060019B2 RID: 6578 RVA: 0x000D2E8C File Offset: 0x000D108C
	public void ObjectBeingTaken()
	{
		if (EquipmentInteractor.instance.leftHandHeldEquipment == this)
		{
			GorillaTagger.Instance.StartVibration(true, GorillaTagger.Instance.tapHapticStrength / 8f, GorillaTagger.Instance.tapHapticDuration * 0.5f);
			EquipmentInteractor.instance.UpdateHandEquipment(null, true);
		}
		if (EquipmentInteractor.instance.rightHandHeldEquipment == this)
		{
			GorillaTagger.Instance.StartVibration(true, GorillaTagger.Instance.tapHapticStrength / 8f, GorillaTagger.Instance.tapHapticDuration * 0.5f);
			EquipmentInteractor.instance.UpdateHandEquipment(null, false);
		}
	}

	// Token: 0x060019B3 RID: 6579 RVA: 0x000D2F2C File Offset: 0x000D112C
	protected virtual void LateUpdateLocal()
	{
		this.wasHover = this.isHover;
		this.isHover = false;
		this.LocalPersistanceValidation();
		if (NetworkSystem.Instance.InRoom)
		{
			if (!this.isSceneObject && this.IsLocalObject())
			{
				this.myRig.SetTransferrablePosStates(this.objectIndex, this.currentState);
				this.myRig.SetTransferrableItemStates(this.objectIndex, this.itemState);
				this.myRig.SetTransferrableDockPosition(this.objectIndex, this.storedZone);
			}
			if (this.worldShareableInstance)
			{
				this.worldShareableInstance.transferableObjectState = this.currentState;
				this.worldShareableInstance.transferableObjectItemState = this.itemState;
			}
		}
		this.HandleLocalInput();
	}

	// Token: 0x060019B4 RID: 6580 RVA: 0x000D2FEC File Offset: 0x000D11EC
	protected void LateUpdateReplicatedSceneObject()
	{
		if (this.myOnlineRig != null)
		{
			this.storedZone = this.myOnlineRig.TransferrableDockPosition(this.objectIndex);
		}
		if (this.worldShareableInstance != null)
		{
			this.currentState = this.worldShareableInstance.transferableObjectState;
			this.itemState = this.worldShareableInstance.transferableObjectItemState;
			this.worldShareableInstance.EnableRemoteSync = (!this.ShouldBeKinematic() || this.currentState == TransferrableObject.PositionState.Dropped);
		}
		if (this.isRigidbodySet && this.ShouldBeKinematic() && !this.rigidbodyInstance.isKinematic)
		{
			this.rigidbodyInstance.isKinematic = true;
		}
	}

	// Token: 0x060019B5 RID: 6581 RVA: 0x000D3090 File Offset: 0x000D1290
	protected virtual void LateUpdateReplicated()
	{
		if (this.isSceneObject || this.shareable)
		{
			this.LateUpdateReplicatedSceneObject();
			return;
		}
		if (this.myOnlineRig == null)
		{
			return;
		}
		this.currentState = this.myOnlineRig.TransferrablePosStates(this.objectIndex);
		if (!this.ValidateState(this.currentState))
		{
			if (this.previousState == TransferrableObject.PositionState.None)
			{
				base.gameObject.Disable();
			}
			this.currentState = this.previousState;
		}
		if (this.isRigidbodySet)
		{
			this.rigidbodyInstance.isKinematic = this.ShouldBeKinematic();
		}
		bool flag = true;
		this.itemState = this.myOnlineRig.TransferrableItemStates(this.objectIndex);
		this.storedZone = this.myOnlineRig.TransferrableDockPosition(this.objectIndex);
		int num = this.myOnlineRig.ActiveTransferrableObjectIndexLength();
		for (int i = 0; i < num; i++)
		{
			if (this.myOnlineRig.ActiveTransferrableObjectIndex(i) == this.myIndex)
			{
				flag = false;
				foreach (GameObject gameObject in this.gameObjectsActiveOnlyWhileHeld)
				{
					bool flag2 = this.InHand();
					if (gameObject.activeSelf != flag2)
					{
						gameObject.SetActive(flag2);
					}
				}
				Behaviour[] array2 = this.behavioursEnabledOnlyWhileHeld;
				for (int j = 0; j < array2.Length; j++)
				{
					array2[j].enabled = this.InHand();
				}
				foreach (GameObject gameObject2 in this.gameObjectsActiveOnlyWhileDocked)
				{
					bool flag3 = this.InHand();
					if (gameObject2.activeSelf == flag3)
					{
						gameObject2.SetActive(flag3);
					}
				}
				array2 = this.behavioursEnabledOnlyWhileDocked;
				for (int j = 0; j < array2.Length; j++)
				{
					array2[j].enabled = !this.InHand();
				}
			}
		}
		if (flag)
		{
			base.gameObject.SetActive(false);
		}
	}

	// Token: 0x060019B6 RID: 6582 RVA: 0x000D3260 File Offset: 0x000D1460
	public virtual void ResetToDefaultState()
	{
		this.canAutoGrabLeft = true;
		this.canAutoGrabRight = true;
		this.wasHover = false;
		this.isHover = false;
		if (!this.IsLocalObject() && this.worldShareableInstance && !this.isSceneObject)
		{
			if (this.IsMyItem())
			{
				return;
			}
			this.worldShareableInstance.GetComponent<RequestableOwnershipGuard>().RequestOwnershipImmediately(delegate
			{
			});
		}
		this.ResetXf();
	}

	// Token: 0x060019B7 RID: 6583 RVA: 0x000D32E4 File Offset: 0x000D14E4
	public override void OnGrab(InteractionPoint pointGrabbed, GameObject grabbingHand)
	{
		if (!(this.worldShareableInstance == null) && !this.worldShareableInstance.guard.isTrulyMine)
		{
			if (!this.IsGrabbable())
			{
				return;
			}
			this.worldShareableInstance.guard.RequestOwnershipImmediately(delegate
			{
			});
		}
		if (grabbingHand == EquipmentInteractor.instance.leftHand && this.currentState != TransferrableObject.PositionState.OnLeftArm)
		{
			if (this.currentState == TransferrableObject.PositionState.InRightHand && this.disableStealing)
			{
				return;
			}
			this.canAutoGrabLeft = false;
			this.interpState = TransferrableObject.InterpolateState.None;
			Debug.Log("<color=red>Setting current State</color>");
			this.currentState = TransferrableObject.PositionState.InLeftHand;
			if (this.transferrableItemSlotTransformOverride)
			{
				this.advancedGrabState = this.transferrableItemSlotTransformOverride.GetAdvancedItemStateFromHand(TransferrableObject.PositionState.InLeftHand, EquipmentInteractor.instance.leftHand.transform, TransferrableObject.GetTargetDock(this.currentState, GorillaTagger.Instance.offlineVRRig));
			}
			EquipmentInteractor.instance.UpdateHandEquipment(this, true);
			GorillaTagger.Instance.StartVibration(true, GorillaTagger.Instance.tapHapticStrength / 8f, GorillaTagger.Instance.tapHapticDuration * 0.5f);
		}
		else if (grabbingHand == EquipmentInteractor.instance.rightHand && this.currentState != TransferrableObject.PositionState.OnRightArm)
		{
			if (this.currentState == TransferrableObject.PositionState.InLeftHand && this.disableStealing)
			{
				return;
			}
			this.canAutoGrabRight = false;
			this.interpState = TransferrableObject.InterpolateState.None;
			this.currentState = TransferrableObject.PositionState.InRightHand;
			if (this.transferrableItemSlotTransformOverride)
			{
				this.advancedGrabState = this.transferrableItemSlotTransformOverride.GetAdvancedItemStateFromHand(TransferrableObject.PositionState.InRightHand, EquipmentInteractor.instance.rightHand.transform, TransferrableObject.GetTargetDock(this.currentState, GorillaTagger.Instance.offlineVRRig));
			}
			EquipmentInteractor.instance.UpdateHandEquipment(this, false);
			GorillaTagger.Instance.StartVibration(false, GorillaTagger.Instance.tapHapticStrength / 8f, GorillaTagger.Instance.tapHapticDuration * 0.5f);
		}
		if (this.rigidbodyInstance && !this.rigidbodyInstance.isKinematic && this.ShouldBeKinematic())
		{
			this.rigidbodyInstance.isKinematic = true;
		}
		PlayerGameEvents.GrabbedObject(this.interactEventName);
	}

	// Token: 0x060019B8 RID: 6584 RVA: 0x000D3528 File Offset: 0x000D1728
	private void SetupMatrixForFreeGrab(Vector3 worldPosition, Quaternion worldRotation, Transform attachPoint, bool leftHand)
	{
		Quaternion rotation = attachPoint.transform.rotation;
		Vector3 position = attachPoint.transform.position;
		Quaternion localRotation = Quaternion.Inverse(rotation) * worldRotation;
		Vector3 localPosition = Quaternion.Inverse(rotation) * (worldPosition - position);
		this.OnHandMatrixUpdate(localPosition, localRotation, leftHand);
	}

	// Token: 0x060019B9 RID: 6585 RVA: 0x000414CD File Offset: 0x0003F6CD
	protected void SetupHandMatrix(Vector3 leftHandPos, Quaternion leftHandRot, Vector3 rightHandPos, Quaternion rightHandRot)
	{
		this.leftHandMatrix = Matrix4x4.TRS(leftHandPos, leftHandRot, Vector3.one);
		this.rightHandMatrix = Matrix4x4.TRS(rightHandPos, rightHandRot, Vector3.one);
	}

	// Token: 0x060019BA RID: 6586 RVA: 0x00030607 File Offset: 0x0002E807
	protected virtual void OnHandMatrixUpdate(Vector3 localPosition, Quaternion localRotation, bool leftHand)
	{
	}

	// Token: 0x060019BB RID: 6587 RVA: 0x000D357C File Offset: 0x000D177C
	public override bool OnRelease(DropZone zoneReleased, GameObject releasingHand)
	{
		if (!base.OnRelease(zoneReleased, releasingHand))
		{
			return false;
		}
		if (!this.IsMyItem())
		{
			return false;
		}
		if (!this.CanDeactivate())
		{
			return false;
		}
		if (!this.IsHeld())
		{
			return false;
		}
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
			if (this.targetDockPositions.DropZoneStorageUsed(zoneReleased.dropPosition) == -1 && zoneReleased.forBodyDock == this.targetDockPositions && (zoneReleased.dropPosition & this.dockPositions) != BodyDockPositions.DropPositions.None)
			{
				this.storedZone = zoneReleased.dropPosition;
			}
		}
		bool flag3 = false;
		this.interpState = TransferrableObject.InterpolateState.None;
		if (this.isSceneObject || this.canDrop || this.allowWorldSharableInstance)
		{
			if (!this.rigidbodyInstance)
			{
				return false;
			}
			if (this.worldShareableInstance)
			{
				this.worldShareableInstance.EnableRemoteSync = true;
			}
			if (!flag3)
			{
				this.currentState = TransferrableObject.PositionState.Dropped;
			}
			if (this.rigidbodyInstance.isKinematic && !this.ShouldBeKinematic())
			{
				this.rigidbodyInstance.isKinematic = false;
			}
			GorillaVelocityEstimator component = base.GetComponent<GorillaVelocityEstimator>();
			if (component != null && this.rigidbodyInstance != null)
			{
				this.rigidbodyInstance.velocity = component.linearVelocity;
				this.rigidbodyInstance.angularVelocity = component.angularVelocity;
			}
		}
		else
		{
			bool flag4 = this.allowWorldSharableInstance;
		}
		this.DropItemCleanup();
		EquipmentInteractor.instance.ForceDropEquipment(this);
		PlayerGameEvents.DroppedObject(this.interactEventName);
		return true;
	}

	// Token: 0x060019BC RID: 6588 RVA: 0x000D3730 File Offset: 0x000D1930
	public override void DropItemCleanup()
	{
		if (this.currentState == TransferrableObject.PositionState.Dropped)
		{
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

	// Token: 0x060019BD RID: 6589 RVA: 0x000D37A0 File Offset: 0x000D19A0
	public override void OnHover(InteractionPoint pointHovered, GameObject hoveringHand)
	{
		if (!this.IsGrabbable())
		{
			return;
		}
		if (!this.wasHover)
		{
			GorillaTagger.Instance.StartVibration(hoveringHand == EquipmentInteractor.instance.leftHand, GorillaTagger.Instance.tapHapticStrength / 8f, GorillaTagger.Instance.tapHapticDuration * 0.5f);
		}
		this.isHover = true;
	}

	// Token: 0x060019BE RID: 6590 RVA: 0x000D3804 File Offset: 0x000D1A04
	protected void ActivateItemFX(float hapticStrength, float hapticDuration, int soundIndex, float soundVolume)
	{
		bool flag = this.currentState == TransferrableObject.PositionState.InLeftHand;
		if (this.myRig.netView != null)
		{
			this.myRig.netView.SendRPC("RPC_PlayHandTap", RpcTarget.Others, new object[]
			{
				soundIndex,
				flag,
				0.1f
			});
		}
		this.myRig.PlayHandTapLocal(soundIndex, flag, soundVolume);
		GorillaTagger.Instance.StartVibration(flag, hapticStrength, hapticDuration);
	}

	// Token: 0x060019BF RID: 6591 RVA: 0x00030607 File Offset: 0x0002E807
	public virtual void PlayNote(int note, float volume)
	{
	}

	// Token: 0x060019C0 RID: 6592 RVA: 0x000414F4 File Offset: 0x0003F6F4
	public virtual bool AutoGrabTrue(bool leftGrabbingHand)
	{
		if (!leftGrabbingHand)
		{
			return this.canAutoGrabRight;
		}
		return this.canAutoGrabLeft;
	}

	// Token: 0x060019C1 RID: 6593 RVA: 0x00039846 File Offset: 0x00037A46
	public virtual bool CanActivate()
	{
		return true;
	}

	// Token: 0x060019C2 RID: 6594 RVA: 0x00039846 File Offset: 0x00037A46
	public virtual bool CanDeactivate()
	{
		return true;
	}

	// Token: 0x060019C3 RID: 6595 RVA: 0x00041506 File Offset: 0x0003F706
	public virtual void OnActivate()
	{
		this.latched = true;
	}

	// Token: 0x060019C4 RID: 6596 RVA: 0x0004150F File Offset: 0x0003F70F
	public virtual void OnDeactivate()
	{
		this.latched = false;
	}

	// Token: 0x060019C5 RID: 6597 RVA: 0x00041518 File Offset: 0x0003F718
	public virtual bool IsMyItem()
	{
		return GorillaTagger.Instance == null || (this.targetRig != null && this.targetRig == GorillaTagger.Instance.offlineVRRig);
	}

	// Token: 0x060019C6 RID: 6598 RVA: 0x00041542 File Offset: 0x0003F742
	protected virtual bool IsHeld()
	{
		return EquipmentInteractor.instance != null && (EquipmentInteractor.instance.leftHandHeldEquipment == this || EquipmentInteractor.instance.rightHandHeldEquipment == this);
	}

	// Token: 0x060019C7 RID: 6599 RVA: 0x000D3888 File Offset: 0x000D1A88
	public virtual bool IsGrabbable()
	{
		return this.IsMyItem() || ((this.isSceneObject || this.shareable) && (this.isSceneObject || this.shareable) && (this.allowPlayerStealing || this.currentState == TransferrableObject.PositionState.Dropped || this.currentState == TransferrableObject.PositionState.None));
	}

	// Token: 0x060019C8 RID: 6600 RVA: 0x0004156F File Offset: 0x0003F76F
	public bool InHand()
	{
		return this.currentState == TransferrableObject.PositionState.InLeftHand || this.currentState == TransferrableObject.PositionState.InRightHand;
	}

	// Token: 0x060019C9 RID: 6601 RVA: 0x00041585 File Offset: 0x0003F785
	public bool Dropped()
	{
		return this.currentState == TransferrableObject.PositionState.Dropped;
	}

	// Token: 0x060019CA RID: 6602 RVA: 0x00041594 File Offset: 0x0003F794
	public bool InLeftHand()
	{
		return this.currentState == TransferrableObject.PositionState.InLeftHand;
	}

	// Token: 0x060019CB RID: 6603 RVA: 0x0004159F File Offset: 0x0003F79F
	public bool InRightHand()
	{
		return this.currentState == TransferrableObject.PositionState.InRightHand;
	}

	// Token: 0x060019CC RID: 6604 RVA: 0x000415AA File Offset: 0x0003F7AA
	public bool OnChest()
	{
		return this.currentState == TransferrableObject.PositionState.OnChest;
	}

	// Token: 0x060019CD RID: 6605 RVA: 0x000415B6 File Offset: 0x0003F7B6
	public bool OnShoulder()
	{
		return this.currentState == TransferrableObject.PositionState.OnLeftShoulder || this.currentState == TransferrableObject.PositionState.OnRightShoulder;
	}

	// Token: 0x060019CE RID: 6606 RVA: 0x000415CE File Offset: 0x0003F7CE
	protected NetPlayer OwningPlayer()
	{
		if (this.myRig == null)
		{
			return this.myOnlineRig.netView.Owner;
		}
		return NetworkSystem.Instance.LocalPlayer;
	}

	// Token: 0x060019CF RID: 6607 RVA: 0x000D38E8 File Offset: 0x000D1AE8
	public bool ValidateState(TransferrableObject.PositionState state)
	{
		if (state <= TransferrableObject.PositionState.OnChest)
		{
			switch (state)
			{
			case TransferrableObject.PositionState.OnLeftArm:
				if ((this.dockPositions & BodyDockPositions.DropPositions.LeftArm) != BodyDockPositions.DropPositions.None)
				{
					return true;
				}
				return false;
			case TransferrableObject.PositionState.OnRightArm:
				if ((this.dockPositions & BodyDockPositions.DropPositions.RightArm) != BodyDockPositions.DropPositions.None)
				{
					return true;
				}
				return false;
			case TransferrableObject.PositionState.OnLeftArm | TransferrableObject.PositionState.OnRightArm:
				return false;
			case TransferrableObject.PositionState.InLeftHand:
				break;
			default:
				if (state != TransferrableObject.PositionState.InRightHand)
				{
					if (state != TransferrableObject.PositionState.OnChest)
					{
						return false;
					}
					if ((this.dockPositions & BodyDockPositions.DropPositions.Chest) != BodyDockPositions.DropPositions.None)
					{
						return true;
					}
					return false;
				}
				break;
			}
			return true;
		}
		if (state != TransferrableObject.PositionState.OnLeftShoulder)
		{
			if (state != TransferrableObject.PositionState.OnRightShoulder)
			{
				if (state == TransferrableObject.PositionState.Dropped)
				{
					return this.canDrop || this.shareable;
				}
			}
			else if ((this.dockPositions & BodyDockPositions.DropPositions.RightBack) != BodyDockPositions.DropPositions.None)
			{
				return true;
			}
		}
		else if ((this.dockPositions & BodyDockPositions.DropPositions.LeftBack) != BodyDockPositions.DropPositions.None)
		{
			return true;
		}
		return false;
	}

	// Token: 0x060019D0 RID: 6608 RVA: 0x000D3988 File Offset: 0x000D1B88
	public virtual void OnOwnershipTransferred(NetPlayer toPlayer, NetPlayer fromPlayer)
	{
		if (toPlayer != null && toPlayer.Equals(fromPlayer))
		{
			return;
		}
		if (object.Equals(fromPlayer, NetworkSystem.Instance.LocalPlayer) && this.IsHeld())
		{
			this.DropItem();
		}
		if (toPlayer == null)
		{
			this.SetTargetRig(null);
			return;
		}
		this.rigidbodyInstance.useGravity = (this.shouldUseGravity && object.Equals(toPlayer, NetworkSystem.Instance.LocalPlayer));
		if (!this.shareable && !this.isSceneObject)
		{
			return;
		}
		if (object.Equals(toPlayer, NetworkSystem.Instance.LocalPlayer))
		{
			if (GorillaTagger.Instance == null)
			{
				Debug.LogError("OnOwnershipTransferred has been initiated too quickly, The local player is not ready");
				return;
			}
			this.SetTargetRig(GorillaTagger.Instance.offlineVRRig);
			return;
		}
		else
		{
			VRRig exists = GorillaGameManager.StaticFindRigForPlayer(toPlayer);
			if (!exists)
			{
				Debug.LogError("failed to find target rig for ownershiptransfer");
				return;
			}
			this.SetTargetRig(exists);
			return;
		}
	}

	// Token: 0x060019D1 RID: 6609 RVA: 0x000D3A60 File Offset: 0x000D1C60
	public bool OnOwnershipRequest(NetPlayer fromPlayer)
	{
		RigContainer rigContainer;
		if (!VRRigCache.Instance.TryGetVrrig(fromPlayer, out rigContainer))
		{
			return false;
		}
		if (Vector3.SqrMagnitude(base.transform.position - rigContainer.transform.position) > 16f)
		{
			Debug.Log("Player whos trying to get is too far, Denying takeover");
			return false;
		}
		if (this.allowPlayerStealing || this.currentState == TransferrableObject.PositionState.Dropped || this.currentState == TransferrableObject.PositionState.None)
		{
			return true;
		}
		if (this.isSceneObject)
		{
			return false;
		}
		if (this.canDrop)
		{
			if (this.ownerRig == null || this.ownerRig.creator == null)
			{
				return true;
			}
			if (this.ownerRig.creator.Equals(fromPlayer))
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x060019D2 RID: 6610 RVA: 0x000D3B18 File Offset: 0x000D1D18
	public bool OnMasterClientAssistedTakeoverRequest(NetPlayer fromPlayer, NetPlayer toPlayer)
	{
		RigContainer rigContainer;
		if (!VRRigCache.Instance.TryGetVrrig(fromPlayer, out rigContainer))
		{
			return true;
		}
		if (Vector3.SqrMagnitude(base.transform.position - rigContainer.transform.position) > 16f)
		{
			Debug.Log("Player whos trying to get is too far, Denying takeover");
			return false;
		}
		if (this.currentState == TransferrableObject.PositionState.Dropped || this.currentState == TransferrableObject.PositionState.None)
		{
			return true;
		}
		if (this.canDrop)
		{
			if (this.ownerRig == null || this.ownerRig.creator == null)
			{
				return true;
			}
			if (this.ownerRig.creator.Equals(fromPlayer))
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x060019D3 RID: 6611 RVA: 0x000D3BBC File Offset: 0x000D1DBC
	public void OnMyOwnerLeft()
	{
		if (this.currentState == TransferrableObject.PositionState.None || this.currentState == TransferrableObject.PositionState.Dropped)
		{
			return;
		}
		this.DropItem();
		if (this.anchor)
		{
			this.anchor.parent = this.InitialDockObject;
			this.anchor.localPosition = Vector3.zero;
			this.anchor.localRotation = Quaternion.identity;
		}
	}

	// Token: 0x060019D4 RID: 6612 RVA: 0x000415F9 File Offset: 0x0003F7F9
	public void OnMyCreatorLeft()
	{
		this.OnItemDestroyedOrDisabled();
		UnityEngine.Object.Destroy(base.gameObject);
	}

	// Token: 0x060019D5 RID: 6613 RVA: 0x000D3C24 File Offset: 0x000D1E24
	public bool BuildValidationCheck()
	{
		int num = 0;
		if (this.storedZone.HasFlag(BodyDockPositions.DropPositions.LeftArm))
		{
			num++;
		}
		if (this.storedZone.HasFlag(BodyDockPositions.DropPositions.RightArm))
		{
			num++;
		}
		if (this.storedZone.HasFlag(BodyDockPositions.DropPositions.Chest))
		{
			num++;
		}
		if (this.storedZone.HasFlag(BodyDockPositions.DropPositions.LeftBack))
		{
			num++;
		}
		if (this.storedZone.HasFlag(BodyDockPositions.DropPositions.RightBack))
		{
			num++;
		}
		if (num > 1)
		{
			Debug.LogError("transferrableitem is starting with multiple storedzones: " + base.transform.parent.name, base.gameObject);
			return false;
		}
		return true;
	}

	// Token: 0x04001C4F RID: 7247
	private VRRig _myRig;

	// Token: 0x04001C51 RID: 7249
	private VRRig _myOnlineRig;

	// Token: 0x04001C53 RID: 7251
	public bool latched;

	// Token: 0x04001C54 RID: 7252
	private float indexTrigger;

	// Token: 0x04001C55 RID: 7253
	public bool testActivate;

	// Token: 0x04001C56 RID: 7254
	public bool testDeactivate;

	// Token: 0x04001C57 RID: 7255
	public float myThreshold = 0.8f;

	// Token: 0x04001C58 RID: 7256
	public float hysterisis = 0.05f;

	// Token: 0x04001C59 RID: 7257
	public bool flipOnXForLeftHand;

	// Token: 0x04001C5A RID: 7258
	public bool flipOnYForLeftHand;

	// Token: 0x04001C5B RID: 7259
	public bool flipOnXForLeftArm;

	// Token: 0x04001C5C RID: 7260
	public bool disableStealing;

	// Token: 0x04001C5D RID: 7261
	public bool allowPlayerStealing;

	// Token: 0x04001C5E RID: 7262
	private TransferrableObject.PositionState initState;

	// Token: 0x04001C5F RID: 7263
	public TransferrableObject.ItemStates itemState;

	// Token: 0x04001C60 RID: 7264
	[DevInspectorShow]
	public BodyDockPositions.DropPositions storedZone;

	// Token: 0x04001C61 RID: 7265
	protected TransferrableObject.PositionState previousState;

	// Token: 0x04001C62 RID: 7266
	[DevInspectorYellow]
	[DevInspectorShow]
	public TransferrableObject.PositionState currentState;

	// Token: 0x04001C63 RID: 7267
	public BodyDockPositions.DropPositions dockPositions;

	// Token: 0x04001C64 RID: 7268
	[DevInspectorCyan]
	[DevInspectorShow]
	public AdvancedItemState advancedGrabState;

	// Token: 0x04001C65 RID: 7269
	[DevInspectorShow]
	[DevInspectorCyan]
	public VRRig targetRig;

	// Token: 0x04001C66 RID: 7270
	public bool targetRigSet;

	// Token: 0x04001C67 RID: 7271
	public TransferrableObject.GrabType useGrabType;

	// Token: 0x04001C68 RID: 7272
	[DevInspectorShow]
	[DevInspectorCyan]
	public VRRig ownerRig;

	// Token: 0x04001C69 RID: 7273
	[DebugReadout]
	[NonSerialized]
	public BodyDockPositions targetDockPositions;

	// Token: 0x04001C6A RID: 7274
	private VRRigAnchorOverrides anchorOverrides;

	// Token: 0x04001C6B RID: 7275
	public bool canAutoGrabLeft;

	// Token: 0x04001C6C RID: 7276
	public bool canAutoGrabRight;

	// Token: 0x04001C6D RID: 7277
	[DevInspectorShow]
	public int objectIndex;

	// Token: 0x04001C6E RID: 7278
	[NonSerialized]
	public Transform anchor;

	// Token: 0x04001C6F RID: 7279
	[Tooltip("In core prefab, assign to the Collider to grab this object")]
	public InteractionPoint gripInteractor;

	// Token: 0x04001C70 RID: 7280
	[Tooltip("(Optional) Use this to override the transform used when the object is in the hand.\nExample: 'GHOST BALLOON' uses child 'grabPtAnchor' which is the end of the balloon's string.")]
	public Transform grabAnchor;

	// Token: 0x04001C71 RID: 7281
	[Tooltip("(Optional) Use this (with the GorillaHandClosed_Left mesh) to intuitively define how\nthe player holds this object, by placing a representation of their hand gripping it.")]
	public Transform handPoseLeft;

	// Token: 0x04001C72 RID: 7282
	[Tooltip("(Optional) Use this (with the GorillaHandClosed_Right mesh) to intuitively define how\nthe player holds this object, by placing a representation of their hand gripping it.")]
	public Transform handPoseRight;

	// Token: 0x04001C73 RID: 7283
	public bool isGrabAnchorSet;

	// Token: 0x04001C74 RID: 7284
	private static Vector3 handPoseRightReferencePoint = new Vector3(-0.0141f, 0.0065f, -0.278f);

	// Token: 0x04001C75 RID: 7285
	private static Quaternion handPoseRightReferenceRotation = Quaternion.Euler(-2.058f, -17.2f, 65.05f);

	// Token: 0x04001C76 RID: 7286
	private static Vector3 handPoseLeftReferencePoint = new Vector3(0.0136f, 0.0045f, -0.2809f);

	// Token: 0x04001C77 RID: 7287
	private static Quaternion handPoseLeftReferenceRotation = Quaternion.Euler(-0.58f, 21.356f, -63.965f);

	// Token: 0x04001C78 RID: 7288
	public TransferrableItemSlotTransformOverride transferrableItemSlotTransformOverride;

	// Token: 0x04001C79 RID: 7289
	public int myIndex;

	// Token: 0x04001C7A RID: 7290
	[Tooltip("(Optional)")]
	public GameObject[] gameObjectsActiveOnlyWhileHeld;

	// Token: 0x04001C7B RID: 7291
	[Tooltip("(Optional)")]
	public GameObject[] gameObjectsActiveOnlyWhileDocked;

	// Token: 0x04001C7C RID: 7292
	[Tooltip("(Optional)")]
	public Behaviour[] behavioursEnabledOnlyWhileHeld;

	// Token: 0x04001C7D RID: 7293
	[Tooltip("(Optional)")]
	public Behaviour[] behavioursEnabledOnlyWhileDocked;

	// Token: 0x04001C7E RID: 7294
	[SerializeField]
	protected internal WorldShareableItem worldShareableInstance;

	// Token: 0x04001C7F RID: 7295
	private float interpTime = 0.2f;

	// Token: 0x04001C80 RID: 7296
	private float interpDt;

	// Token: 0x04001C81 RID: 7297
	private Vector3 interpStartPos;

	// Token: 0x04001C82 RID: 7298
	private Quaternion interpStartRot;

	// Token: 0x04001C83 RID: 7299
	protected int enabledOnFrame = -1;

	// Token: 0x04001C84 RID: 7300
	protected Vector3 initOffset;

	// Token: 0x04001C85 RID: 7301
	protected Quaternion initRotation;

	// Token: 0x04001C86 RID: 7302
	private Matrix4x4 initMatrix = Matrix4x4.identity;

	// Token: 0x04001C87 RID: 7303
	private Matrix4x4 leftHandMatrix = Matrix4x4.identity;

	// Token: 0x04001C88 RID: 7304
	private Matrix4x4 rightHandMatrix = Matrix4x4.identity;

	// Token: 0x04001C89 RID: 7305
	private bool positionInitialized;

	// Token: 0x04001C8A RID: 7306
	public bool isSceneObject;

	// Token: 0x04001C8B RID: 7307
	public Rigidbody rigidbodyInstance;

	// Token: 0x04001C8E RID: 7310
	public bool canDrop;

	// Token: 0x04001C8F RID: 7311
	public bool allowReparenting;

	// Token: 0x04001C90 RID: 7312
	public bool shareable;

	// Token: 0x04001C91 RID: 7313
	public bool detatchOnGrab;

	// Token: 0x04001C92 RID: 7314
	public bool allowWorldSharableInstance;

	// Token: 0x04001C93 RID: 7315
	[ItemCanBeNull]
	public Transform originPoint;

	// Token: 0x04001C94 RID: 7316
	[ItemCanBeNull]
	public float maxDistanceFromOriginBeforeRespawn;

	// Token: 0x04001C95 RID: 7317
	public AudioClip resetPositionAudioClip;

	// Token: 0x04001C96 RID: 7318
	public float maxDistanceFromTargetPlayerBeforeRespawn;

	// Token: 0x04001C97 RID: 7319
	private bool wasHover;

	// Token: 0x04001C98 RID: 7320
	private bool isHover;

	// Token: 0x04001C99 RID: 7321
	private bool disableItem;

	// Token: 0x04001C9A RID: 7322
	protected bool loaded;

	// Token: 0x04001C9B RID: 7323
	public bool ClearLocalPositionOnReset;

	// Token: 0x04001C9C RID: 7324
	public string interactEventName;

	// Token: 0x04001C9D RID: 7325
	public const int kPositionStateCount = 8;

	// Token: 0x04001C9E RID: 7326
	[DevInspectorShow]
	public TransferrableObject.InterpolateState interpState;

	// Token: 0x04001C9F RID: 7327
	public bool startInterpolation;

	// Token: 0x04001CA0 RID: 7328
	public Transform InitialDockObject;

	// Token: 0x04001CA1 RID: 7329
	private AudioSource audioSrc;

	// Token: 0x04001CA2 RID: 7330
	private bool _isListeningFor_OnPostInstantiateAllPrefabs2;

	// Token: 0x04001CA5 RID: 7333
	protected Transform _defaultAnchor;

	// Token: 0x04001CA6 RID: 7334
	protected bool _isDefaultAnchorSet;

	// Token: 0x04001CA7 RID: 7335
	private Matrix4x4? transferrableItemSlotTransformOverrideCachedMatrix;

	// Token: 0x04001CA8 RID: 7336
	private bool transferrableItemSlotTransformOverrideApplicable;

	// Token: 0x0200040F RID: 1039
	public enum ItemStates
	{
		// Token: 0x04001CAA RID: 7338
		State0 = 1,
		// Token: 0x04001CAB RID: 7339
		State1,
		// Token: 0x04001CAC RID: 7340
		State2 = 4,
		// Token: 0x04001CAD RID: 7341
		State3 = 8,
		// Token: 0x04001CAE RID: 7342
		State4 = 16,
		// Token: 0x04001CAF RID: 7343
		State5 = 32,
		// Token: 0x04001CB0 RID: 7344
		Part0Held = 64,
		// Token: 0x04001CB1 RID: 7345
		Part1Held = 128
	}

	// Token: 0x02000410 RID: 1040
	public enum GrabType
	{
		// Token: 0x04001CB3 RID: 7347
		Default,
		// Token: 0x04001CB4 RID: 7348
		Free
	}

	// Token: 0x02000411 RID: 1041
	[Flags]
	public enum PositionState
	{
		// Token: 0x04001CB6 RID: 7350
		OnLeftArm = 1,
		// Token: 0x04001CB7 RID: 7351
		OnRightArm = 2,
		// Token: 0x04001CB8 RID: 7352
		InLeftHand = 4,
		// Token: 0x04001CB9 RID: 7353
		InRightHand = 8,
		// Token: 0x04001CBA RID: 7354
		OnChest = 16,
		// Token: 0x04001CBB RID: 7355
		OnLeftShoulder = 32,
		// Token: 0x04001CBC RID: 7356
		OnRightShoulder = 64,
		// Token: 0x04001CBD RID: 7357
		Dropped = 128,
		// Token: 0x04001CBE RID: 7358
		None = 0
	}

	// Token: 0x02000412 RID: 1042
	public enum InterpolateState
	{
		// Token: 0x04001CC0 RID: 7360
		None,
		// Token: 0x04001CC1 RID: 7361
		Interpolating
	}
}
