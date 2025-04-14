using System;
using GorillaExtensions;
using GorillaTag;
using GorillaTag.CosmeticSystem;
using JetBrains.Annotations;
using Photon.Pun;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.XR;

// Token: 0x02000403 RID: 1027
public class TransferrableObject : HoldableObject, ISelfValidator, IRequestableOwnershipGuardCallbacks, IPreDisable, ISpawnable, IBuildValidation
{
	// Token: 0x06001929 RID: 6441 RVA: 0x0007B254 File Offset: 0x00079454
	public void FixTransformOverride()
	{
		this.transferrableItemSlotTransformOverride = base.GetComponent<TransferrableItemSlotTransformOverride>();
	}

	// Token: 0x0600192A RID: 6442 RVA: 0x000023F4 File Offset: 0x000005F4
	public void Validate(SelfValidationResult result)
	{
	}

	// Token: 0x170002BF RID: 703
	// (get) Token: 0x0600192B RID: 6443 RVA: 0x0007B262 File Offset: 0x00079462
	// (set) Token: 0x0600192C RID: 6444 RVA: 0x0007B26A File Offset: 0x0007946A
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

	// Token: 0x170002C0 RID: 704
	// (get) Token: 0x0600192D RID: 6445 RVA: 0x0007B273 File Offset: 0x00079473
	// (set) Token: 0x0600192E RID: 6446 RVA: 0x0007B27B File Offset: 0x0007947B
	public bool isMyRigValid { get; private set; }

	// Token: 0x170002C1 RID: 705
	// (get) Token: 0x0600192F RID: 6447 RVA: 0x0007B284 File Offset: 0x00079484
	// (set) Token: 0x06001930 RID: 6448 RVA: 0x0007B28C File Offset: 0x0007948C
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

	// Token: 0x170002C2 RID: 706
	// (get) Token: 0x06001931 RID: 6449 RVA: 0x0007B29C File Offset: 0x0007949C
	// (set) Token: 0x06001932 RID: 6450 RVA: 0x0007B2A4 File Offset: 0x000794A4
	public bool isMyOnlineRigValid { get; private set; }

	// Token: 0x06001933 RID: 6451 RVA: 0x0007B2B0 File Offset: 0x000794B0
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

	// Token: 0x170002C3 RID: 707
	// (get) Token: 0x06001934 RID: 6452 RVA: 0x0007B380 File Offset: 0x00079580
	public bool IsLocalOwnedWorldShareable
	{
		get
		{
			return this.worldShareableInstance && this.worldShareableInstance.guard.isTrulyMine;
		}
	}

	// Token: 0x06001935 RID: 6453 RVA: 0x0007B3A4 File Offset: 0x000795A4
	public void WorldShareableRequestOwnership()
	{
		if (this.worldShareableInstance != null && !this.worldShareableInstance.guard.isMine)
		{
			this.worldShareableInstance.guard.RequestOwnershipImmediately(delegate
			{
			});
		}
	}

	// Token: 0x170002C4 RID: 708
	// (get) Token: 0x06001936 RID: 6454 RVA: 0x0007B400 File Offset: 0x00079600
	// (set) Token: 0x06001937 RID: 6455 RVA: 0x0007B408 File Offset: 0x00079608
	public bool isRigidbodySet { get; private set; }

	// Token: 0x170002C5 RID: 709
	// (get) Token: 0x06001938 RID: 6456 RVA: 0x0007B411 File Offset: 0x00079611
	// (set) Token: 0x06001939 RID: 6457 RVA: 0x0007B419 File Offset: 0x00079619
	public bool shouldUseGravity { get; private set; }

	// Token: 0x0600193A RID: 6458 RVA: 0x0007B422 File Offset: 0x00079622
	protected virtual void Awake()
	{
		if (this.isSceneObject)
		{
			this.IsSpawned = true;
			this.OnSpawn(null);
		}
	}

	// Token: 0x170002C6 RID: 710
	// (get) Token: 0x0600193B RID: 6459 RVA: 0x0007B43A File Offset: 0x0007963A
	// (set) Token: 0x0600193C RID: 6460 RVA: 0x0007B442 File Offset: 0x00079642
	public bool IsSpawned { get; set; }

	// Token: 0x170002C7 RID: 711
	// (get) Token: 0x0600193D RID: 6461 RVA: 0x0007B44B File Offset: 0x0007964B
	// (set) Token: 0x0600193E RID: 6462 RVA: 0x0007B453 File Offset: 0x00079653
	public ECosmeticSelectSide CosmeticSelectedSide { get; set; }

	// Token: 0x0600193F RID: 6463 RVA: 0x0007B45C File Offset: 0x0007965C
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

	// Token: 0x06001940 RID: 6464 RVA: 0x0007B658 File Offset: 0x00079858
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

	// Token: 0x06001941 RID: 6465 RVA: 0x0007B6E0 File Offset: 0x000798E0
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

	// Token: 0x06001942 RID: 6466 RVA: 0x000023F4 File Offset: 0x000005F4
	protected virtual void Start()
	{
	}

	// Token: 0x06001943 RID: 6467 RVA: 0x0007B858 File Offset: 0x00079A58
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

	// Token: 0x06001944 RID: 6468 RVA: 0x0007B960 File Offset: 0x00079B60
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

	// Token: 0x06001945 RID: 6469 RVA: 0x0007BCD0 File Offset: 0x00079ED0
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

	// Token: 0x06001946 RID: 6470 RVA: 0x0007BE7C File Offset: 0x0007A07C
	protected virtual void OnDestroy()
	{
		TransferrableObjectManager.Unregister(this);
	}

	// Token: 0x06001947 RID: 6471 RVA: 0x0007BE84 File Offset: 0x0007A084
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

	// Token: 0x06001948 RID: 6472 RVA: 0x0007BF0F File Offset: 0x0007A10F
	public virtual void PreDisable()
	{
		this.itemState = TransferrableObject.ItemStates.State0;
		this.currentState = TransferrableObject.PositionState.None;
		this.interpState = TransferrableObject.InterpolateState.None;
		this.ResetToDefaultState();
	}

	// Token: 0x06001949 RID: 6473 RVA: 0x0007BF2C File Offset: 0x0007A12C
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

	// Token: 0x0600194A RID: 6474 RVA: 0x0007BF5E File Offset: 0x0007A15E
	public virtual bool ShouldBeKinematic()
	{
		if (this.detatchOnGrab)
		{
			return this.currentState != TransferrableObject.PositionState.Dropped && this.currentState != TransferrableObject.PositionState.InLeftHand && this.currentState != TransferrableObject.PositionState.InRightHand;
		}
		return this.currentState != TransferrableObject.PositionState.Dropped;
	}

	// Token: 0x0600194B RID: 6475 RVA: 0x0007BF9C File Offset: 0x0007A19C
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

	// Token: 0x0600194C RID: 6476 RVA: 0x0007C030 File Offset: 0x0007A230
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

	// Token: 0x0600194D RID: 6477 RVA: 0x0007C0B4 File Offset: 0x0007A2B4
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

	// Token: 0x0600194E RID: 6478 RVA: 0x0007C12C File Offset: 0x0007A32C
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

	// Token: 0x0600194F RID: 6479 RVA: 0x0007C1FA File Offset: 0x0007A3FA
	public bool IsLocalObject()
	{
		return this.myRig != null && this.myRig.isOfflineVRRig;
	}

	// Token: 0x06001950 RID: 6480 RVA: 0x0007C211 File Offset: 0x0007A411
	public void SetWorldShareableItem(WorldShareableItem item)
	{
		this.worldShareableInstance = item;
		this.OnWorldShareableItemSpawn();
	}

	// Token: 0x06001951 RID: 6481 RVA: 0x000023F4 File Offset: 0x000005F4
	protected virtual void OnWorldShareableItemSpawn()
	{
	}

	// Token: 0x06001952 RID: 6482 RVA: 0x000023F4 File Offset: 0x000005F4
	protected virtual void PlayDestroyedOrDisabledEffect()
	{
	}

	// Token: 0x06001953 RID: 6483 RVA: 0x0007C220 File Offset: 0x0007A420
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

	// Token: 0x06001954 RID: 6484 RVA: 0x0007C29A File Offset: 0x0007A49A
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

	// Token: 0x06001955 RID: 6485 RVA: 0x0007C2CE File Offset: 0x0007A4CE
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

	// Token: 0x06001956 RID: 6486 RVA: 0x0007C30E File Offset: 0x0007A50E
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

	// Token: 0x06001957 RID: 6487 RVA: 0x0007C33C File Offset: 0x0007A53C
	protected bool Attached()
	{
		bool flag = this.InHand() && this.detatchOnGrab;
		return !this.Dropped() && !flag;
	}

	// Token: 0x06001958 RID: 6488 RVA: 0x0007C36C File Offset: 0x0007A56C
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

	// Token: 0x06001959 RID: 6489 RVA: 0x0007C3ED File Offset: 0x0007A5ED
	public static Transform GetTargetDock(TransferrableObject.PositionState state, VRRig rig)
	{
		return TransferrableObject.GetTargetDock(state, rig.myBodyDockPositions, rig.GetComponent<VRRigAnchorOverrides>());
	}

	// Token: 0x0600195A RID: 6490 RVA: 0x0007C404 File Offset: 0x0007A604
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

	// Token: 0x0600195B RID: 6491 RVA: 0x0007C4A8 File Offset: 0x0007A6A8
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

	// Token: 0x0600195C RID: 6492 RVA: 0x0007C9E8 File Offset: 0x0007ABE8
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

	// Token: 0x0600195D RID: 6493 RVA: 0x000023F4 File Offset: 0x000005F4
	protected virtual void OnStateChanged()
	{
	}

	// Token: 0x0600195E RID: 6494 RVA: 0x0007CB10 File Offset: 0x0007AD10
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

	// Token: 0x0600195F RID: 6495 RVA: 0x0007CCF0 File Offset: 0x0007AEF0
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

	// Token: 0x06001960 RID: 6496 RVA: 0x0007CD3C File Offset: 0x0007AF3C
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

	// Token: 0x06001961 RID: 6497 RVA: 0x0007CF7C File Offset: 0x0007B17C
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

	// Token: 0x06001962 RID: 6498 RVA: 0x0007CFD4 File Offset: 0x0007B1D4
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

	// Token: 0x06001963 RID: 6499 RVA: 0x000023F4 File Offset: 0x000005F4
	protected virtual void LocalMyObjectValidation()
	{
	}

	// Token: 0x06001964 RID: 6500 RVA: 0x0007D25C File Offset: 0x0007B45C
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

	// Token: 0x06001965 RID: 6501 RVA: 0x0007D35C File Offset: 0x0007B55C
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

	// Token: 0x06001966 RID: 6502 RVA: 0x0007D3FC File Offset: 0x0007B5FC
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

	// Token: 0x06001967 RID: 6503 RVA: 0x0007D4BC File Offset: 0x0007B6BC
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

	// Token: 0x06001968 RID: 6504 RVA: 0x0007D560 File Offset: 0x0007B760
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

	// Token: 0x06001969 RID: 6505 RVA: 0x0007D730 File Offset: 0x0007B930
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

	// Token: 0x0600196A RID: 6506 RVA: 0x0007D7B4 File Offset: 0x0007B9B4
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

	// Token: 0x0600196B RID: 6507 RVA: 0x0007D9F8 File Offset: 0x0007BBF8
	private void SetupMatrixForFreeGrab(Vector3 worldPosition, Quaternion worldRotation, Transform attachPoint, bool leftHand)
	{
		Quaternion rotation = attachPoint.transform.rotation;
		Vector3 position = attachPoint.transform.position;
		Quaternion localRotation = Quaternion.Inverse(rotation) * worldRotation;
		Vector3 localPosition = Quaternion.Inverse(rotation) * (worldPosition - position);
		this.OnHandMatrixUpdate(localPosition, localRotation, leftHand);
	}

	// Token: 0x0600196C RID: 6508 RVA: 0x0007DA4B File Offset: 0x0007BC4B
	protected void SetupHandMatrix(Vector3 leftHandPos, Quaternion leftHandRot, Vector3 rightHandPos, Quaternion rightHandRot)
	{
		this.leftHandMatrix = Matrix4x4.TRS(leftHandPos, leftHandRot, Vector3.one);
		this.rightHandMatrix = Matrix4x4.TRS(rightHandPos, rightHandRot, Vector3.one);
	}

	// Token: 0x0600196D RID: 6509 RVA: 0x000023F4 File Offset: 0x000005F4
	protected virtual void OnHandMatrixUpdate(Vector3 localPosition, Quaternion localRotation, bool leftHand)
	{
	}

	// Token: 0x0600196E RID: 6510 RVA: 0x0007DA74 File Offset: 0x0007BC74
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

	// Token: 0x0600196F RID: 6511 RVA: 0x0007DC28 File Offset: 0x0007BE28
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

	// Token: 0x06001970 RID: 6512 RVA: 0x0007DC98 File Offset: 0x0007BE98
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

	// Token: 0x06001971 RID: 6513 RVA: 0x0007DCFC File Offset: 0x0007BEFC
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

	// Token: 0x06001972 RID: 6514 RVA: 0x000023F4 File Offset: 0x000005F4
	public virtual void PlayNote(int note, float volume)
	{
	}

	// Token: 0x06001973 RID: 6515 RVA: 0x0007DD7D File Offset: 0x0007BF7D
	public virtual bool AutoGrabTrue(bool leftGrabbingHand)
	{
		if (!leftGrabbingHand)
		{
			return this.canAutoGrabRight;
		}
		return this.canAutoGrabLeft;
	}

	// Token: 0x06001974 RID: 6516 RVA: 0x000444E2 File Offset: 0x000426E2
	public virtual bool CanActivate()
	{
		return true;
	}

	// Token: 0x06001975 RID: 6517 RVA: 0x000444E2 File Offset: 0x000426E2
	public virtual bool CanDeactivate()
	{
		return true;
	}

	// Token: 0x06001976 RID: 6518 RVA: 0x0007DD8F File Offset: 0x0007BF8F
	public virtual void OnActivate()
	{
		this.latched = true;
	}

	// Token: 0x06001977 RID: 6519 RVA: 0x0007DD98 File Offset: 0x0007BF98
	public virtual void OnDeactivate()
	{
		this.latched = false;
	}

	// Token: 0x06001978 RID: 6520 RVA: 0x0007DDA1 File Offset: 0x0007BFA1
	public virtual bool IsMyItem()
	{
		return GorillaTagger.Instance == null || (this.targetRig != null && this.targetRig == GorillaTagger.Instance.offlineVRRig);
	}

	// Token: 0x06001979 RID: 6521 RVA: 0x0007DDCB File Offset: 0x0007BFCB
	protected virtual bool IsHeld()
	{
		return EquipmentInteractor.instance != null && (EquipmentInteractor.instance.leftHandHeldEquipment == this || EquipmentInteractor.instance.rightHandHeldEquipment == this);
	}

	// Token: 0x0600197A RID: 6522 RVA: 0x0007DDF8 File Offset: 0x0007BFF8
	public virtual bool IsGrabbable()
	{
		return this.IsMyItem() || ((this.isSceneObject || this.shareable) && (this.isSceneObject || this.shareable) && (this.allowPlayerStealing || this.currentState == TransferrableObject.PositionState.Dropped || this.currentState == TransferrableObject.PositionState.None));
	}

	// Token: 0x0600197B RID: 6523 RVA: 0x0007DE57 File Offset: 0x0007C057
	public bool InHand()
	{
		return this.currentState == TransferrableObject.PositionState.InLeftHand || this.currentState == TransferrableObject.PositionState.InRightHand;
	}

	// Token: 0x0600197C RID: 6524 RVA: 0x0007DE6D File Offset: 0x0007C06D
	public bool Dropped()
	{
		return this.currentState == TransferrableObject.PositionState.Dropped;
	}

	// Token: 0x0600197D RID: 6525 RVA: 0x0007DE7C File Offset: 0x0007C07C
	public bool InLeftHand()
	{
		return this.currentState == TransferrableObject.PositionState.InLeftHand;
	}

	// Token: 0x0600197E RID: 6526 RVA: 0x0007DE87 File Offset: 0x0007C087
	public bool InRightHand()
	{
		return this.currentState == TransferrableObject.PositionState.InRightHand;
	}

	// Token: 0x0600197F RID: 6527 RVA: 0x0007DE92 File Offset: 0x0007C092
	public bool OnChest()
	{
		return this.currentState == TransferrableObject.PositionState.OnChest;
	}

	// Token: 0x06001980 RID: 6528 RVA: 0x0007DE9E File Offset: 0x0007C09E
	public bool OnShoulder()
	{
		return this.currentState == TransferrableObject.PositionState.OnLeftShoulder || this.currentState == TransferrableObject.PositionState.OnRightShoulder;
	}

	// Token: 0x06001981 RID: 6529 RVA: 0x0007DEB6 File Offset: 0x0007C0B6
	protected NetPlayer OwningPlayer()
	{
		if (this.myRig == null)
		{
			return this.myOnlineRig.netView.Owner;
		}
		return NetworkSystem.Instance.LocalPlayer;
	}

	// Token: 0x06001982 RID: 6530 RVA: 0x0007DEE4 File Offset: 0x0007C0E4
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

	// Token: 0x06001983 RID: 6531 RVA: 0x0007DF84 File Offset: 0x0007C184
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

	// Token: 0x06001984 RID: 6532 RVA: 0x0007E05C File Offset: 0x0007C25C
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

	// Token: 0x06001985 RID: 6533 RVA: 0x0007E114 File Offset: 0x0007C314
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

	// Token: 0x06001986 RID: 6534 RVA: 0x0007E1B8 File Offset: 0x0007C3B8
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

	// Token: 0x06001987 RID: 6535 RVA: 0x0007E21F File Offset: 0x0007C41F
	public void OnMyCreatorLeft()
	{
		this.OnItemDestroyedOrDisabled();
		Object.Destroy(base.gameObject);
	}

	// Token: 0x06001988 RID: 6536 RVA: 0x0007E234 File Offset: 0x0007C434
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

	// Token: 0x04001C06 RID: 7174
	private VRRig _myRig;

	// Token: 0x04001C08 RID: 7176
	private VRRig _myOnlineRig;

	// Token: 0x04001C0A RID: 7178
	public bool latched;

	// Token: 0x04001C0B RID: 7179
	private float indexTrigger;

	// Token: 0x04001C0C RID: 7180
	public bool testActivate;

	// Token: 0x04001C0D RID: 7181
	public bool testDeactivate;

	// Token: 0x04001C0E RID: 7182
	public float myThreshold = 0.8f;

	// Token: 0x04001C0F RID: 7183
	public float hysterisis = 0.05f;

	// Token: 0x04001C10 RID: 7184
	public bool flipOnXForLeftHand;

	// Token: 0x04001C11 RID: 7185
	public bool flipOnYForLeftHand;

	// Token: 0x04001C12 RID: 7186
	public bool flipOnXForLeftArm;

	// Token: 0x04001C13 RID: 7187
	public bool disableStealing;

	// Token: 0x04001C14 RID: 7188
	public bool allowPlayerStealing;

	// Token: 0x04001C15 RID: 7189
	private TransferrableObject.PositionState initState;

	// Token: 0x04001C16 RID: 7190
	public TransferrableObject.ItemStates itemState;

	// Token: 0x04001C17 RID: 7191
	[DevInspectorShow]
	public BodyDockPositions.DropPositions storedZone;

	// Token: 0x04001C18 RID: 7192
	protected TransferrableObject.PositionState previousState;

	// Token: 0x04001C19 RID: 7193
	[DevInspectorYellow]
	[DevInspectorShow]
	public TransferrableObject.PositionState currentState;

	// Token: 0x04001C1A RID: 7194
	public BodyDockPositions.DropPositions dockPositions;

	// Token: 0x04001C1B RID: 7195
	[DevInspectorCyan]
	[DevInspectorShow]
	public AdvancedItemState advancedGrabState;

	// Token: 0x04001C1C RID: 7196
	[DevInspectorShow]
	[DevInspectorCyan]
	public VRRig targetRig;

	// Token: 0x04001C1D RID: 7197
	public bool targetRigSet;

	// Token: 0x04001C1E RID: 7198
	public TransferrableObject.GrabType useGrabType;

	// Token: 0x04001C1F RID: 7199
	[DevInspectorShow]
	[DevInspectorCyan]
	public VRRig ownerRig;

	// Token: 0x04001C20 RID: 7200
	[DebugReadout]
	[NonSerialized]
	public BodyDockPositions targetDockPositions;

	// Token: 0x04001C21 RID: 7201
	private VRRigAnchorOverrides anchorOverrides;

	// Token: 0x04001C22 RID: 7202
	public bool canAutoGrabLeft;

	// Token: 0x04001C23 RID: 7203
	public bool canAutoGrabRight;

	// Token: 0x04001C24 RID: 7204
	[DevInspectorShow]
	public int objectIndex;

	// Token: 0x04001C25 RID: 7205
	[NonSerialized]
	public Transform anchor;

	// Token: 0x04001C26 RID: 7206
	[Tooltip("In core prefab, assign to the Collider to grab this object")]
	public InteractionPoint gripInteractor;

	// Token: 0x04001C27 RID: 7207
	[Tooltip("(Optional) Use this to override the transform used when the object is in the hand.\nExample: 'GHOST BALLOON' uses child 'grabPtAnchor' which is the end of the balloon's string.")]
	public Transform grabAnchor;

	// Token: 0x04001C28 RID: 7208
	[Tooltip("(Optional) Use this (with the GorillaHandClosed_Left mesh) to intuitively define how\nthe player holds this object, by placing a representation of their hand gripping it.")]
	public Transform handPoseLeft;

	// Token: 0x04001C29 RID: 7209
	[Tooltip("(Optional) Use this (with the GorillaHandClosed_Right mesh) to intuitively define how\nthe player holds this object, by placing a representation of their hand gripping it.")]
	public Transform handPoseRight;

	// Token: 0x04001C2A RID: 7210
	public bool isGrabAnchorSet;

	// Token: 0x04001C2B RID: 7211
	private static Vector3 handPoseRightReferencePoint = new Vector3(-0.0141f, 0.0065f, -0.278f);

	// Token: 0x04001C2C RID: 7212
	private static Quaternion handPoseRightReferenceRotation = Quaternion.Euler(-2.058f, -17.2f, 65.05f);

	// Token: 0x04001C2D RID: 7213
	private static Vector3 handPoseLeftReferencePoint = new Vector3(0.0136f, 0.0045f, -0.2809f);

	// Token: 0x04001C2E RID: 7214
	private static Quaternion handPoseLeftReferenceRotation = Quaternion.Euler(-0.58f, 21.356f, -63.965f);

	// Token: 0x04001C2F RID: 7215
	public TransferrableItemSlotTransformOverride transferrableItemSlotTransformOverride;

	// Token: 0x04001C30 RID: 7216
	public int myIndex;

	// Token: 0x04001C31 RID: 7217
	[Tooltip("(Optional)")]
	public GameObject[] gameObjectsActiveOnlyWhileHeld;

	// Token: 0x04001C32 RID: 7218
	[Tooltip("(Optional)")]
	public GameObject[] gameObjectsActiveOnlyWhileDocked;

	// Token: 0x04001C33 RID: 7219
	[Tooltip("(Optional)")]
	public Behaviour[] behavioursEnabledOnlyWhileHeld;

	// Token: 0x04001C34 RID: 7220
	[Tooltip("(Optional)")]
	public Behaviour[] behavioursEnabledOnlyWhileDocked;

	// Token: 0x04001C35 RID: 7221
	[SerializeField]
	protected internal WorldShareableItem worldShareableInstance;

	// Token: 0x04001C36 RID: 7222
	private float interpTime = 0.2f;

	// Token: 0x04001C37 RID: 7223
	private float interpDt;

	// Token: 0x04001C38 RID: 7224
	private Vector3 interpStartPos;

	// Token: 0x04001C39 RID: 7225
	private Quaternion interpStartRot;

	// Token: 0x04001C3A RID: 7226
	protected int enabledOnFrame = -1;

	// Token: 0x04001C3B RID: 7227
	protected Vector3 initOffset;

	// Token: 0x04001C3C RID: 7228
	protected Quaternion initRotation;

	// Token: 0x04001C3D RID: 7229
	private Matrix4x4 initMatrix = Matrix4x4.identity;

	// Token: 0x04001C3E RID: 7230
	private Matrix4x4 leftHandMatrix = Matrix4x4.identity;

	// Token: 0x04001C3F RID: 7231
	private Matrix4x4 rightHandMatrix = Matrix4x4.identity;

	// Token: 0x04001C40 RID: 7232
	private bool positionInitialized;

	// Token: 0x04001C41 RID: 7233
	public bool isSceneObject;

	// Token: 0x04001C42 RID: 7234
	public Rigidbody rigidbodyInstance;

	// Token: 0x04001C45 RID: 7237
	public bool canDrop;

	// Token: 0x04001C46 RID: 7238
	public bool allowReparenting;

	// Token: 0x04001C47 RID: 7239
	public bool shareable;

	// Token: 0x04001C48 RID: 7240
	public bool detatchOnGrab;

	// Token: 0x04001C49 RID: 7241
	public bool allowWorldSharableInstance;

	// Token: 0x04001C4A RID: 7242
	[ItemCanBeNull]
	public Transform originPoint;

	// Token: 0x04001C4B RID: 7243
	[ItemCanBeNull]
	public float maxDistanceFromOriginBeforeRespawn;

	// Token: 0x04001C4C RID: 7244
	public AudioClip resetPositionAudioClip;

	// Token: 0x04001C4D RID: 7245
	public float maxDistanceFromTargetPlayerBeforeRespawn;

	// Token: 0x04001C4E RID: 7246
	private bool wasHover;

	// Token: 0x04001C4F RID: 7247
	private bool isHover;

	// Token: 0x04001C50 RID: 7248
	private bool disableItem;

	// Token: 0x04001C51 RID: 7249
	protected bool loaded;

	// Token: 0x04001C52 RID: 7250
	public bool ClearLocalPositionOnReset;

	// Token: 0x04001C53 RID: 7251
	public string interactEventName;

	// Token: 0x04001C54 RID: 7252
	public const int kPositionStateCount = 8;

	// Token: 0x04001C55 RID: 7253
	[DevInspectorShow]
	public TransferrableObject.InterpolateState interpState;

	// Token: 0x04001C56 RID: 7254
	public bool startInterpolation;

	// Token: 0x04001C57 RID: 7255
	public Transform InitialDockObject;

	// Token: 0x04001C58 RID: 7256
	private AudioSource audioSrc;

	// Token: 0x04001C59 RID: 7257
	private bool _isListeningFor_OnPostInstantiateAllPrefabs2;

	// Token: 0x04001C5C RID: 7260
	protected Transform _defaultAnchor;

	// Token: 0x04001C5D RID: 7261
	protected bool _isDefaultAnchorSet;

	// Token: 0x04001C5E RID: 7262
	private Matrix4x4? transferrableItemSlotTransformOverrideCachedMatrix;

	// Token: 0x04001C5F RID: 7263
	private bool transferrableItemSlotTransformOverrideApplicable;

	// Token: 0x02000404 RID: 1028
	public enum ItemStates
	{
		// Token: 0x04001C61 RID: 7265
		State0 = 1,
		// Token: 0x04001C62 RID: 7266
		State1,
		// Token: 0x04001C63 RID: 7267
		State2 = 4,
		// Token: 0x04001C64 RID: 7268
		State3 = 8,
		// Token: 0x04001C65 RID: 7269
		State4 = 16,
		// Token: 0x04001C66 RID: 7270
		State5 = 32,
		// Token: 0x04001C67 RID: 7271
		Part0Held = 64,
		// Token: 0x04001C68 RID: 7272
		Part1Held = 128
	}

	// Token: 0x02000405 RID: 1029
	public enum GrabType
	{
		// Token: 0x04001C6A RID: 7274
		Default,
		// Token: 0x04001C6B RID: 7275
		Free
	}

	// Token: 0x02000406 RID: 1030
	[Flags]
	public enum PositionState
	{
		// Token: 0x04001C6D RID: 7277
		OnLeftArm = 1,
		// Token: 0x04001C6E RID: 7278
		OnRightArm = 2,
		// Token: 0x04001C6F RID: 7279
		InLeftHand = 4,
		// Token: 0x04001C70 RID: 7280
		InRightHand = 8,
		// Token: 0x04001C71 RID: 7281
		OnChest = 16,
		// Token: 0x04001C72 RID: 7282
		OnLeftShoulder = 32,
		// Token: 0x04001C73 RID: 7283
		OnRightShoulder = 64,
		// Token: 0x04001C74 RID: 7284
		Dropped = 128,
		// Token: 0x04001C75 RID: 7285
		None = 0
	}

	// Token: 0x02000407 RID: 1031
	public enum InterpolateState
	{
		// Token: 0x04001C77 RID: 7287
		None,
		// Token: 0x04001C78 RID: 7288
		Interpolating
	}
}
