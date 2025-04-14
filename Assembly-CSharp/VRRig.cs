using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Fusion;
using GorillaExtensions;
using GorillaGameModes;
using GorillaLocomotion;
using GorillaLocomotion.Climbing;
using GorillaLocomotion.Gameplay;
using GorillaNetworking;
using GorillaTag;
using GorillaTag.Cosmetics;
using GorillaTagScripts;
using KID.Model;
using Photon.Pun;
using Photon.Realtime;
using Photon.Voice;
using Photon.Voice.PUN;
using Photon.Voice.Unity;
using PlayFab;
using PlayFab.ClientModels;
using TagEffects;
using TMPro;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

// Token: 0x020003AF RID: 943
public class VRRig : MonoBehaviour, IWrappedSerializable, INetworkStruct, IPreDisable, IUserCosmeticsCallback, IEyeScannable
{
	// Token: 0x06001603 RID: 5635 RVA: 0x0006A288 File Offset: 0x00068488
	private void CosmeticsV2_Awake()
	{
		if (CosmeticsV2Spawner_Dirty.allPartsInstantiated)
		{
			this.Handle_CosmeticsV2_OnPostInstantiateAllPrefabs_DoEnableAllCosmetics();
			return;
		}
		if (!this._isListeningFor_OnPostInstantiateAllPrefabs)
		{
			this._isListeningFor_OnPostInstantiateAllPrefabs = true;
			CosmeticsV2Spawner_Dirty.OnPostInstantiateAllPrefabs = (Action)Delegate.Combine(CosmeticsV2Spawner_Dirty.OnPostInstantiateAllPrefabs, new Action(this.Handle_CosmeticsV2_OnPostInstantiateAllPrefabs_DoEnableAllCosmetics));
		}
	}

	// Token: 0x06001604 RID: 5636 RVA: 0x0006A2C7 File Offset: 0x000684C7
	private void CosmeticsV2_OnDestroy()
	{
		if (CosmeticsV2Spawner_Dirty.allPartsInstantiated)
		{
			this.Handle_CosmeticsV2_OnPostInstantiateAllPrefabs_DoEnableAllCosmetics();
			return;
		}
		CosmeticsV2Spawner_Dirty.OnPostInstantiateAllPrefabs = (Action)Delegate.Remove(CosmeticsV2Spawner_Dirty.OnPostInstantiateAllPrefabs, new Action(this.Handle_CosmeticsV2_OnPostInstantiateAllPrefabs_DoEnableAllCosmetics));
	}

	// Token: 0x06001605 RID: 5637 RVA: 0x0006A2F7 File Offset: 0x000684F7
	internal void Handle_CosmeticsV2_OnPostInstantiateAllPrefabs_DoEnableAllCosmetics()
	{
		CosmeticsV2Spawner_Dirty.OnPostInstantiateAllPrefabs = (Action)Delegate.Remove(CosmeticsV2Spawner_Dirty.OnPostInstantiateAllPrefabs, new Action(this.Handle_CosmeticsV2_OnPostInstantiateAllPrefabs_DoEnableAllCosmetics));
		this.CheckForEarlyAccess();
		this.BuildInitialize_AfterCosmeticsV2Instantiated();
		this.SetCosmeticsActive();
	}

	// Token: 0x17000265 RID: 613
	// (get) Token: 0x06001606 RID: 5638 RVA: 0x0006A32B File Offset: 0x0006852B
	// (set) Token: 0x06001607 RID: 5639 RVA: 0x0006A338 File Offset: 0x00068538
	public Vector3 syncPos
	{
		get
		{
			return this.netSyncPos.CurrentSyncTarget;
		}
		set
		{
			this.netSyncPos.SetNewSyncTarget(value);
		}
	}

	// Token: 0x17000266 RID: 614
	// (get) Token: 0x06001608 RID: 5640 RVA: 0x0006A346 File Offset: 0x00068546
	// (set) Token: 0x06001609 RID: 5641 RVA: 0x0006A34E File Offset: 0x0006854E
	public GameObject[] cosmetics
	{
		get
		{
			return this._cosmetics;
		}
		set
		{
			this._cosmetics = value;
		}
	}

	// Token: 0x17000267 RID: 615
	// (get) Token: 0x0600160A RID: 5642 RVA: 0x0006A357 File Offset: 0x00068557
	// (set) Token: 0x0600160B RID: 5643 RVA: 0x0006A35F File Offset: 0x0006855F
	public GameObject[] overrideCosmetics
	{
		get
		{
			return this._overrideCosmetics;
		}
		set
		{
			this._overrideCosmetics = value;
		}
	}

	// Token: 0x0600160C RID: 5644 RVA: 0x0006A368 File Offset: 0x00068568
	internal void SetTaggedBy(VRRig taggingRig)
	{
		this.taggedById = taggingRig.OwningNetPlayer.ActorNumber;
	}

	// Token: 0x17000268 RID: 616
	// (get) Token: 0x0600160D RID: 5645 RVA: 0x0006A37B File Offset: 0x0006857B
	// (set) Token: 0x0600160E RID: 5646 RVA: 0x0006A383 File Offset: 0x00068583
	internal bool InitializedCosmetics
	{
		get
		{
			return this.initializedCosmetics;
		}
		set
		{
			this.initializedCosmetics = value;
		}
	}

	// Token: 0x17000269 RID: 617
	// (get) Token: 0x0600160F RID: 5647 RVA: 0x0006A38C File Offset: 0x0006858C
	// (set) Token: 0x06001610 RID: 5648 RVA: 0x0006A394 File Offset: 0x00068594
	public CosmeticRefRegistry cosmeticReferences { get; private set; }

	// Token: 0x1700026A RID: 618
	// (get) Token: 0x06001611 RID: 5649 RVA: 0x0006A39D File Offset: 0x0006859D
	public bool HasBracelet
	{
		get
		{
			return this.reliableState.HasBracelet;
		}
	}

	// Token: 0x06001612 RID: 5650 RVA: 0x0006A3AA File Offset: 0x000685AA
	public Vector3 GetMouthPosition()
	{
		return this.MouthPosition.position;
	}

	// Token: 0x1700026B RID: 619
	// (get) Token: 0x06001613 RID: 5651 RVA: 0x0006A3B7 File Offset: 0x000685B7
	// (set) Token: 0x06001614 RID: 5652 RVA: 0x0006A3BF File Offset: 0x000685BF
	public GorillaSkin CurrentCosmeticSkin { get; set; }

	// Token: 0x1700026C RID: 620
	// (get) Token: 0x06001615 RID: 5653 RVA: 0x0006A3C8 File Offset: 0x000685C8
	// (set) Token: 0x06001616 RID: 5654 RVA: 0x0006A3D0 File Offset: 0x000685D0
	public GorillaSkin CurrentModeSkin { get; set; }

	// Token: 0x1700026D RID: 621
	// (get) Token: 0x06001617 RID: 5655 RVA: 0x0006A3D9 File Offset: 0x000685D9
	// (set) Token: 0x06001618 RID: 5656 RVA: 0x0006A3E1 File Offset: 0x000685E1
	public GorillaSkin TemporaryEffectSkin { get; set; }

	// Token: 0x06001619 RID: 5657 RVA: 0x0006A3EA File Offset: 0x000685EA
	public VRRig.PartyMemberStatus GetPartyMemberStatus()
	{
		if (this.partyMemberStatus == VRRig.PartyMemberStatus.NeedsUpdate)
		{
			this.partyMemberStatus = (FriendshipGroupDetection.Instance.IsInMyGroup(this.creator.UserId) ? VRRig.PartyMemberStatus.InLocalParty : VRRig.PartyMemberStatus.NotInLocalParty);
		}
		return this.partyMemberStatus;
	}

	// Token: 0x1700026E RID: 622
	// (get) Token: 0x0600161A RID: 5658 RVA: 0x0006A41B File Offset: 0x0006861B
	public bool IsLocalPartyMember
	{
		get
		{
			return this.GetPartyMemberStatus() != VRRig.PartyMemberStatus.NotInLocalParty;
		}
	}

	// Token: 0x0600161B RID: 5659 RVA: 0x0006A429 File Offset: 0x00068629
	public void ClearPartyMemberStatus()
	{
		this.partyMemberStatus = VRRig.PartyMemberStatus.NeedsUpdate;
	}

	// Token: 0x0600161C RID: 5660 RVA: 0x0006A432 File Offset: 0x00068632
	public int ActiveTransferrableObjectIndex(int idx)
	{
		return this.reliableState.activeTransferrableObjectIndex[idx];
	}

	// Token: 0x0600161D RID: 5661 RVA: 0x0006A441 File Offset: 0x00068641
	public int ActiveTransferrableObjectIndexLength()
	{
		return this.reliableState.activeTransferrableObjectIndex.Length;
	}

	// Token: 0x0600161E RID: 5662 RVA: 0x0006A450 File Offset: 0x00068650
	public void SetActiveTransferrableObjectIndex(int idx, int v)
	{
		if (this.reliableState.activeTransferrableObjectIndex[idx] != v)
		{
			this.reliableState.activeTransferrableObjectIndex[idx] = v;
			this.reliableState.SetIsDirty();
		}
	}

	// Token: 0x0600161F RID: 5663 RVA: 0x0006A47B File Offset: 0x0006867B
	public TransferrableObject.PositionState TransferrablePosStates(int idx)
	{
		return this.reliableState.transferrablePosStates[idx];
	}

	// Token: 0x06001620 RID: 5664 RVA: 0x0006A48A File Offset: 0x0006868A
	public void SetTransferrablePosStates(int idx, TransferrableObject.PositionState v)
	{
		if (this.reliableState.transferrablePosStates[idx] != v)
		{
			this.reliableState.transferrablePosStates[idx] = v;
			this.reliableState.SetIsDirty();
		}
	}

	// Token: 0x06001621 RID: 5665 RVA: 0x0006A4B5 File Offset: 0x000686B5
	public TransferrableObject.ItemStates TransferrableItemStates(int idx)
	{
		return this.reliableState.transferrableItemStates[idx];
	}

	// Token: 0x06001622 RID: 5666 RVA: 0x0006A4C4 File Offset: 0x000686C4
	public void SetTransferrableItemStates(int idx, TransferrableObject.ItemStates v)
	{
		if (this.reliableState.transferrableItemStates[idx] != v)
		{
			this.reliableState.transferrableItemStates[idx] = v;
			this.reliableState.SetIsDirty();
		}
	}

	// Token: 0x06001623 RID: 5667 RVA: 0x0006A4EF File Offset: 0x000686EF
	public void SetTransferrableDockPosition(int idx, BodyDockPositions.DropPositions v)
	{
		if (this.reliableState.transferableDockPositions[idx] != v)
		{
			this.reliableState.transferableDockPositions[idx] = v;
			this.reliableState.SetIsDirty();
		}
	}

	// Token: 0x06001624 RID: 5668 RVA: 0x0006A51A File Offset: 0x0006871A
	public BodyDockPositions.DropPositions TransferrableDockPosition(int idx)
	{
		return this.reliableState.transferableDockPositions[idx];
	}

	// Token: 0x1700026F RID: 623
	// (get) Token: 0x06001625 RID: 5669 RVA: 0x0006A529 File Offset: 0x00068729
	// (set) Token: 0x06001626 RID: 5670 RVA: 0x0006A536 File Offset: 0x00068736
	public int WearablePackedStates
	{
		get
		{
			return this.reliableState.wearablesPackedStates;
		}
		set
		{
			if (this.reliableState.wearablesPackedStates != value)
			{
				this.reliableState.wearablesPackedStates = value;
				this.reliableState.SetIsDirty();
			}
		}
	}

	// Token: 0x17000270 RID: 624
	// (get) Token: 0x06001627 RID: 5671 RVA: 0x0006A55D File Offset: 0x0006875D
	// (set) Token: 0x06001628 RID: 5672 RVA: 0x0006A56A File Offset: 0x0006876A
	public int LeftThrowableProjectileIndex
	{
		get
		{
			return this.reliableState.lThrowableProjectileIndex;
		}
		set
		{
			if (this.reliableState.lThrowableProjectileIndex != value)
			{
				this.reliableState.lThrowableProjectileIndex = value;
				this.reliableState.SetIsDirty();
			}
		}
	}

	// Token: 0x17000271 RID: 625
	// (get) Token: 0x06001629 RID: 5673 RVA: 0x0006A591 File Offset: 0x00068791
	// (set) Token: 0x0600162A RID: 5674 RVA: 0x0006A59E File Offset: 0x0006879E
	public int RightThrowableProjectileIndex
	{
		get
		{
			return this.reliableState.rThrowableProjectileIndex;
		}
		set
		{
			if (this.reliableState.rThrowableProjectileIndex != value)
			{
				this.reliableState.rThrowableProjectileIndex = value;
				this.reliableState.SetIsDirty();
			}
		}
	}

	// Token: 0x17000272 RID: 626
	// (get) Token: 0x0600162B RID: 5675 RVA: 0x0006A5C5 File Offset: 0x000687C5
	// (set) Token: 0x0600162C RID: 5676 RVA: 0x0006A5D2 File Offset: 0x000687D2
	public Color32 LeftThrowableProjectileColor
	{
		get
		{
			return this.reliableState.lThrowableProjectileColor;
		}
		set
		{
			if (!this.reliableState.lThrowableProjectileColor.Equals(value))
			{
				this.reliableState.lThrowableProjectileColor = value;
				this.reliableState.SetIsDirty();
			}
		}
	}

	// Token: 0x17000273 RID: 627
	// (get) Token: 0x0600162D RID: 5677 RVA: 0x0006A609 File Offset: 0x00068809
	// (set) Token: 0x0600162E RID: 5678 RVA: 0x0006A616 File Offset: 0x00068816
	public Color32 RightThrowableProjectileColor
	{
		get
		{
			return this.reliableState.rThrowableProjectileColor;
		}
		set
		{
			if (!this.reliableState.rThrowableProjectileColor.Equals(value))
			{
				this.reliableState.rThrowableProjectileColor = value;
				this.reliableState.SetIsDirty();
			}
		}
	}

	// Token: 0x0600162F RID: 5679 RVA: 0x0006A64D File Offset: 0x0006884D
	public Color32 GetThrowableProjectileColor(bool isLeftHand)
	{
		if (!isLeftHand)
		{
			return this.RightThrowableProjectileColor;
		}
		return this.LeftThrowableProjectileColor;
	}

	// Token: 0x06001630 RID: 5680 RVA: 0x0006A65F File Offset: 0x0006885F
	public void SetThrowableProjectileColor(bool isLeftHand, Color32 color)
	{
		if (isLeftHand)
		{
			this.LeftThrowableProjectileColor = color;
			return;
		}
		this.RightThrowableProjectileColor = color;
	}

	// Token: 0x06001631 RID: 5681 RVA: 0x0006A673 File Offset: 0x00068873
	public void SetRandomThrowableModelIndex(int randModelIndex)
	{
		this.RandomThrowableIndex = randModelIndex;
	}

	// Token: 0x06001632 RID: 5682 RVA: 0x0006A67C File Offset: 0x0006887C
	public int GetRandomThrowableModelIndex()
	{
		return this.RandomThrowableIndex;
	}

	// Token: 0x17000274 RID: 628
	// (get) Token: 0x06001633 RID: 5683 RVA: 0x0006A684 File Offset: 0x00068884
	// (set) Token: 0x06001634 RID: 5684 RVA: 0x0006A691 File Offset: 0x00068891
	private int RandomThrowableIndex
	{
		get
		{
			return this.reliableState.randomThrowableIndex;
		}
		set
		{
			if (this.reliableState.randomThrowableIndex != value)
			{
				this.reliableState.randomThrowableIndex = value;
				this.reliableState.SetIsDirty();
			}
		}
	}

	// Token: 0x17000275 RID: 629
	// (get) Token: 0x06001635 RID: 5685 RVA: 0x0006A6B8 File Offset: 0x000688B8
	// (set) Token: 0x06001636 RID: 5686 RVA: 0x0006A6C5 File Offset: 0x000688C5
	public bool IsMicEnabled
	{
		get
		{
			return this.reliableState.isMicEnabled;
		}
		set
		{
			if (this.reliableState.isMicEnabled != value)
			{
				this.reliableState.isMicEnabled = value;
				this.reliableState.SetIsDirty();
			}
		}
	}

	// Token: 0x17000276 RID: 630
	// (get) Token: 0x06001637 RID: 5687 RVA: 0x0006A6EC File Offset: 0x000688EC
	// (set) Token: 0x06001638 RID: 5688 RVA: 0x0006A6F9 File Offset: 0x000688F9
	public int SizeLayerMask
	{
		get
		{
			return this.reliableState.sizeLayerMask;
		}
		set
		{
			if (this.reliableState.sizeLayerMask != value)
			{
				this.reliableState.sizeLayerMask = value;
				this.reliableState.SetIsDirty();
			}
		}
	}

	// Token: 0x17000277 RID: 631
	// (get) Token: 0x06001639 RID: 5689 RVA: 0x0006A720 File Offset: 0x00068920
	public float scaleFactor
	{
		get
		{
			return this.scaleMultiplier * this.nativeScale;
		}
	}

	// Token: 0x17000278 RID: 632
	// (get) Token: 0x0600163A RID: 5690 RVA: 0x0006A72F File Offset: 0x0006892F
	// (set) Token: 0x0600163B RID: 5691 RVA: 0x0006A737 File Offset: 0x00068937
	public float ScaleMultiplier
	{
		get
		{
			return this.scaleMultiplier;
		}
		set
		{
			this.scaleMultiplier = value;
		}
	}

	// Token: 0x17000279 RID: 633
	// (get) Token: 0x0600163C RID: 5692 RVA: 0x0006A740 File Offset: 0x00068940
	// (set) Token: 0x0600163D RID: 5693 RVA: 0x0006A748 File Offset: 0x00068948
	public float NativeScale
	{
		get
		{
			return this.nativeScale;
		}
		set
		{
			this.nativeScale = value;
		}
	}

	// Token: 0x1700027A RID: 634
	// (get) Token: 0x0600163E RID: 5694 RVA: 0x0006A751 File Offset: 0x00068951
	public NetPlayer Creator
	{
		get
		{
			return this.creator;
		}
	}

	// Token: 0x1700027B RID: 635
	// (get) Token: 0x0600163F RID: 5695 RVA: 0x0006A759 File Offset: 0x00068959
	internal bool Initialized
	{
		get
		{
			return this.initialized;
		}
	}

	// Token: 0x1700027C RID: 636
	// (get) Token: 0x06001640 RID: 5696 RVA: 0x0006A761 File Offset: 0x00068961
	public float SpeakingLoudness
	{
		get
		{
			return this.speakingLoudness;
		}
	}

	// Token: 0x1700027D RID: 637
	// (get) Token: 0x06001641 RID: 5697 RVA: 0x0006A769 File Offset: 0x00068969
	internal HandEffectContext LeftHandEffect
	{
		get
		{
			return this._leftHandEffect;
		}
	}

	// Token: 0x1700027E RID: 638
	// (get) Token: 0x06001642 RID: 5698 RVA: 0x0006A771 File Offset: 0x00068971
	internal HandEffectContext RightHandEffect
	{
		get
		{
			return this._rightHandEffect;
		}
	}

	// Token: 0x06001643 RID: 5699 RVA: 0x0006A77C File Offset: 0x0006897C
	public void BuildInitialize()
	{
		this.fxSettings = Object.Instantiate<FXSystemSettings>(this.sharedFXSettings);
		this.fxSettings.forLocalRig = this.isOfflineVRRig;
		this.lastPosition = base.transform.position;
		if (!this.isOfflineVRRig)
		{
			base.transform.parent = null;
		}
		SizeManager component = base.GetComponent<SizeManager>();
		if (component != null)
		{
			component.BuildInitialize();
		}
		this.myMouthFlap = base.GetComponent<GorillaMouthFlap>();
		this.mySpeakerLoudness = base.GetComponent<GorillaSpeakerLoudness>();
		if (this.myReplacementVoice == null)
		{
			this.myReplacementVoice = base.GetComponentInChildren<ReplacementVoice>();
		}
		this.myEyeExpressions = base.GetComponent<GorillaEyeExpressions>();
	}

	// Token: 0x06001644 RID: 5700 RVA: 0x0006A820 File Offset: 0x00068A20
	public void BuildInitialize_AfterCosmeticsV2Instantiated()
	{
		if (!this._rigBuildFullyInitialized)
		{
			Dictionary<string, GameObject> dictionary = new Dictionary<string, GameObject>();
			foreach (GameObject gameObject in this.cosmetics)
			{
				GameObject gameObject2;
				if (!dictionary.TryGetValue(gameObject.name, out gameObject2))
				{
					dictionary.Add(gameObject.name, gameObject);
				}
			}
			foreach (GameObject gameObject3 in this.overrideCosmetics)
			{
				GameObject gameObject2;
				if (dictionary.TryGetValue(gameObject3.name, out gameObject2) && gameObject2.name == gameObject3.name)
				{
					gameObject2.name = "OVERRIDDEN";
				}
			}
			this.cosmetics = this.cosmetics.Concat(this.overrideCosmetics).ToArray<GameObject>();
		}
		this.cosmeticsObjectRegistry.Initialize(this.cosmetics);
		this._rigBuildFullyInitialized = true;
	}

	// Token: 0x06001645 RID: 5701 RVA: 0x0006A8F8 File Offset: 0x00068AF8
	private void Awake()
	{
		this.CosmeticsV2_Awake();
		PlayFabAuthenticator instance = PlayFabAuthenticator.instance;
		instance.OnSafetyUpdate = (Action<bool>)Delegate.Combine(instance.OnSafetyUpdate, new Action<bool>(this.UpdateName));
		if (this.isOfflineVRRig)
		{
			this.BuildInitialize();
		}
		this.SharedStart();
	}

	// Token: 0x06001646 RID: 5702 RVA: 0x0006A947 File Offset: 0x00068B47
	private void EnsureInstantiatedMaterial()
	{
		if (this.myDefaultSkinMaterialInstance == null)
		{
			this.myDefaultSkinMaterialInstance = Object.Instantiate<Material>(this.materialsToChangeTo[0]);
			this.materialsToChangeTo[0] = this.myDefaultSkinMaterialInstance;
		}
	}

	// Token: 0x06001647 RID: 5703 RVA: 0x0006A978 File Offset: 0x00068B78
	private void ApplyColorCode()
	{
		float defaultValue = 0f;
		float @float = PlayerPrefs.GetFloat("redValue", defaultValue);
		float float2 = PlayerPrefs.GetFloat("greenValue", defaultValue);
		float float3 = PlayerPrefs.GetFloat("blueValue", defaultValue);
		GorillaTagger.Instance.UpdateColor(@float, float2, float3);
	}

	// Token: 0x06001648 RID: 5704 RVA: 0x0006A9BC File Offset: 0x00068BBC
	private void SharedStart()
	{
		if (this.isInitialized)
		{
			return;
		}
		this.lastScaleFactor = this.scaleFactor;
		this.isInitialized = true;
		this.myBodyDockPositions = base.GetComponent<BodyDockPositions>();
		this.reliableState.SharedStart(this.isOfflineVRRig, this.myBodyDockPositions);
		this.concatStringOfCosmeticsAllowed = "";
		this.EnsureInstantiatedMaterial();
		this.initialized = false;
		if (this.isOfflineVRRig)
		{
			if (CosmeticsController.hasInstance && CosmeticsController.instance.v2_allCosmeticsInfoAssetRef_isLoaded)
			{
				CosmeticsController.instance.currentWornSet.LoadFromPlayerPreferences(CosmeticsController.instance);
			}
			if (Application.platform == RuntimePlatform.Android && this.spectatorSkin != null)
			{
				Object.Destroy(this.spectatorSkin);
			}
			base.StartCoroutine(this.OccasionalUpdate());
			this.initialized = true;
		}
		else if (!this.isOfflineVRRig)
		{
			if (this.spectatorSkin != null)
			{
				Object.Destroy(this.spectatorSkin);
			}
			this.head.syncPos = -this.headBodyOffset;
		}
		GorillaSkin.ShowActiveSkin(this);
		base.Invoke("ApplyColorCode", 1f);
		List<Material> m = new List<Material>();
		this.mainSkin.GetSharedMaterials(m);
		this.layerChanger = base.GetComponent<LayerChanger>();
		if (this.layerChanger != null)
		{
			this.layerChanger.InitializeLayers(base.transform);
		}
		this.frozenEffectMinY = this.frozenEffect.transform.localScale.y;
		this.frozenEffectMinHorizontalScale = this.frozenEffect.transform.localScale.x;
	}

	// Token: 0x06001649 RID: 5705 RVA: 0x0006AB4D File Offset: 0x00068D4D
	private IEnumerator OccasionalUpdate()
	{
		for (;;)
		{
			try
			{
				if (RoomSystem.JoinedRoom && NetworkSystem.Instance.IsMasterClient && GorillaGameModes.GameMode.ActiveNetworkHandler.IsNull())
				{
					GorillaGameModes.GameMode.LoadGameModeFromProperty();
				}
			}
			catch
			{
			}
			yield return new WaitForSeconds(1f);
		}
		yield break;
	}

	// Token: 0x0600164A RID: 5706 RVA: 0x0006AB58 File Offset: 0x00068D58
	public bool IsItemAllowed(string itemName)
	{
		if (itemName == "Slingshot")
		{
			return NetworkSystem.Instance.InRoom && GorillaGameManager.instance is GorillaPaintbrawlManager;
		}
		if (BuilderSetManager.instance.GetStarterSetsConcat().Contains(itemName))
		{
			return true;
		}
		if (this.concatStringOfCosmeticsAllowed == null)
		{
			return false;
		}
		if (this.concatStringOfCosmeticsAllowed.Contains(itemName))
		{
			return true;
		}
		bool canTryOn = CosmeticsController.instance.GetItemFromDict(itemName).canTryOn;
		return this.inTryOnRoom && canTryOn;
	}

	// Token: 0x0600164B RID: 5707 RVA: 0x0006ABDE File Offset: 0x00068DDE
	public void ApplyLocalTrajectoryOverride(Vector3 overrideVelocity)
	{
		this.LocalTrajectoryOverrideBlend = 1f;
		this.LocalTrajectoryOverridePosition = base.transform.position;
		this.LocalTrajectoryOverrideVelocity = overrideVelocity;
	}

	// Token: 0x0600164C RID: 5708 RVA: 0x0006AC03 File Offset: 0x00068E03
	public bool IsLocalTrajectoryOverrideActive()
	{
		return this.LocalTrajectoryOverrideBlend > 0f;
	}

	// Token: 0x0600164D RID: 5709 RVA: 0x0006AC12 File Offset: 0x00068E12
	public void ApplyLocalGrabOverride(bool isBody, bool isLeftHand, Transform grabbingHand)
	{
		this.localOverrideIsBody = isBody;
		this.localOverrideIsLeftHand = isLeftHand;
		this.localOverrideGrabbingHand = grabbingHand;
		this.localGrabOverrideBlend = 1f;
	}

	// Token: 0x0600164E RID: 5710 RVA: 0x0006AC34 File Offset: 0x00068E34
	public void ClearLocalGrabOverride()
	{
		this.localGrabOverrideBlend = -1f;
	}

	// Token: 0x0600164F RID: 5711 RVA: 0x0006AC44 File Offset: 0x00068E44
	public void RemoteRigUpdate()
	{
		if (this.scaleFactor != this.lastScaleFactor)
		{
			this.ScaleUpdate();
		}
		if (this.voiceAudio != null)
		{
			float time = GorillaTagger.Instance.offlineVRRig.scaleFactor / this.scaleFactor;
			float num = this.voicePitchForRelativeScale.Evaluate(time);
			if (float.IsNaN(num) || num <= 0f)
			{
				Debug.LogError("Voice pitch curve is invalid, please fix!");
			}
			float num2 = this.UsingHauntedRing ? this.HauntedRingVoicePitch : num;
			num2 = (this.IsHaunted ? this.HauntedVoicePitch : num2);
			if (!Mathf.Approximately(this.voiceAudio.pitch, num2))
			{
				this.voiceAudio.pitch = num2;
			}
		}
		this.jobPos = base.transform.position;
		if (Time.time > this.timeSpawned + this.doNotLerpConstant)
		{
			this.jobPos = Vector3.Lerp(base.transform.position, this.SanitizeVector3(this.syncPos), this.lerpValueBody * 0.66f);
			if (this.currentRopeSwing && this.currentRopeSwingTarget)
			{
				Vector3 b;
				if (this.grabbedRopeIsLeft)
				{
					b = this.currentRopeSwingTarget.position - this.leftHandTransform.position;
				}
				else
				{
					b = this.currentRopeSwingTarget.position - this.rightHandTransform.position;
				}
				if (this.shouldLerpToRope)
				{
					this.jobPos += Vector3.Lerp(Vector3.zero, b, this.lastRopeGrabTimer * 4f);
					if (this.lastRopeGrabTimer < 1f)
					{
						this.lastRopeGrabTimer += Time.deltaTime;
					}
				}
				else
				{
					this.jobPos += b;
				}
			}
			else if (this.currentHoldParent)
			{
				Transform transform;
				if (this.grabbedRopeIsBody)
				{
					transform = this.bodyTransform;
				}
				else if (this.grabbedRopeIsLeft)
				{
					transform = this.leftHandTransform;
				}
				else
				{
					transform = this.rightHandTransform;
				}
				this.jobPos += this.currentHoldParent.TransformPoint(this.grabbedRopeOffset) - transform.position;
			}
			else if (this.mountedMonkeBlock || this.mountedMovingSurface)
			{
				Transform transform2 = this.movingSurfaceIsMonkeBlock ? this.mountedMonkeBlock.transform : this.mountedMovingSurface.transform;
				Vector3 b2 = Vector3.zero;
				Vector3 b3 = this.jobPos - base.transform.position;
				Transform transform3;
				if (this.mountedMovingSurfaceIsBody)
				{
					transform3 = this.bodyTransform;
				}
				else if (this.mountedMovingSurfaceIsLeft)
				{
					transform3 = this.leftHandTransform;
				}
				else
				{
					transform3 = this.rightHandTransform;
				}
				b2 = transform2.TransformPoint(this.mountedMonkeBlockOffset) - (transform3.position + b3);
				if (this.shouldLerpToMovingSurface)
				{
					this.lastMountedSurfaceTimer += Time.deltaTime;
					this.jobPos += Vector3.Lerp(Vector3.zero, b2, this.lastMountedSurfaceTimer * 4f);
					if (this.lastMountedSurfaceTimer * 4f >= 1f)
					{
						this.shouldLerpToMovingSurface = false;
					}
				}
				else
				{
					this.jobPos += b2;
				}
			}
		}
		else
		{
			this.jobPos = this.SanitizeVector3(this.syncPos);
		}
		if (this.LocalTrajectoryOverrideBlend > 0f)
		{
			this.LocalTrajectoryOverrideBlend -= Time.deltaTime / this.LocalTrajectoryOverrideDuration;
			this.LocalTrajectoryOverrideVelocity += Physics.gravity * Time.deltaTime * 0.5f;
			Vector3 localTrajectoryOverrideVelocity;
			Vector3 localTrajectoryOverridePosition;
			if (this.LocalTestMovementCollision(this.LocalTrajectoryOverridePosition, this.LocalTrajectoryOverrideVelocity, out localTrajectoryOverrideVelocity, out localTrajectoryOverridePosition))
			{
				this.LocalTrajectoryOverrideVelocity = localTrajectoryOverrideVelocity;
				this.LocalTrajectoryOverridePosition = localTrajectoryOverridePosition;
			}
			else
			{
				this.LocalTrajectoryOverridePosition += this.LocalTrajectoryOverrideVelocity * Time.deltaTime;
			}
			this.LocalTrajectoryOverrideVelocity += Physics.gravity * Time.deltaTime * 0.5f;
			this.jobPos = Vector3.Lerp(this.jobPos, this.LocalTrajectoryOverridePosition, this.LocalTrajectoryOverrideBlend);
		}
		else if (this.localGrabOverrideBlend > 0f)
		{
			this.localGrabOverrideBlend -= Time.deltaTime / this.LocalGrabOverrideDuration;
			if (this.localOverrideGrabbingHand != null)
			{
				Transform transform4;
				if (this.localOverrideIsBody)
				{
					transform4 = this.bodyTransform;
				}
				else if (this.localOverrideIsLeftHand)
				{
					transform4 = this.leftHandTransform;
				}
				else
				{
					transform4 = this.rightHandTransform;
				}
				this.jobPos += this.localOverrideGrabbingHand.TransformPoint(this.grabbedRopeOffset) - transform4.position;
			}
		}
		this.jobRotation = Quaternion.Lerp(base.transform.rotation, this.SanitizeQuaternion(this.syncRotation), this.lerpValueBody);
		this.head.syncPos = base.transform.rotation * -this.headBodyOffset * this.scaleFactor;
		this.head.MapOther(this.lerpValueBody);
		this.rightHand.MapOther(this.lerpValueBody);
		this.leftHand.MapOther(this.lerpValueBody);
		this.rightIndex.MapOtherFinger((float)(this.handSync % 10) / 10f, this.lerpValueFingers);
		this.rightMiddle.MapOtherFinger((float)(this.handSync % 100) / 100f, this.lerpValueFingers);
		this.rightThumb.MapOtherFinger((float)(this.handSync % 1000) / 1000f, this.lerpValueFingers);
		this.leftIndex.MapOtherFinger((float)(this.handSync % 10000) / 10000f, this.lerpValueFingers);
		this.leftMiddle.MapOtherFinger((float)(this.handSync % 100000) / 100000f, this.lerpValueFingers);
		this.leftThumb.MapOtherFinger((float)(this.handSync % 1000000) / 1000000f, this.lerpValueFingers);
		this.leftHandHoldableStatus = this.handSync % 10000000 / 1000000;
		this.rightHandHoldableStatus = this.handSync % 100000000 / 10000000;
	}

	// Token: 0x06001650 RID: 5712 RVA: 0x0006B2C4 File Offset: 0x000694C4
	private void ScaleUpdate()
	{
		this.frameScale = Mathf.MoveTowards(this.lastScaleFactor, this.scaleFactor, Time.deltaTime * 4f);
		base.transform.localScale = Vector3.one * this.frameScale;
		this.lastScaleFactor = this.frameScale;
	}

	// Token: 0x06001651 RID: 5713 RVA: 0x0006B31A File Offset: 0x0006951A
	public void AddLateUpdateCallback(ICallBack action)
	{
		this.lateUpdateCallbacks.Add(action);
	}

	// Token: 0x06001652 RID: 5714 RVA: 0x0006B328 File Offset: 0x00069528
	public void RemoveLateUpdateCallback(ICallBack action)
	{
		this.lateUpdateCallbacks.Remove(action);
	}

	// Token: 0x06001653 RID: 5715 RVA: 0x0006B338 File Offset: 0x00069538
	private void LateUpdate()
	{
		if (this.isOfflineVRRig)
		{
			if (GorillaGameManager.instance != null)
			{
				this.speedArray = GorillaGameManager.instance.LocalPlayerSpeed();
				GTPlayer.Instance.jumpMultiplier = this.speedArray[1];
				GTPlayer.Instance.maxJumpSpeed = this.speedArray[0];
			}
			else
			{
				GTPlayer.Instance.jumpMultiplier = 1.1f;
				GTPlayer.Instance.maxJumpSpeed = 6.5f;
			}
			this.nativeScale = GTPlayer.Instance.NativeScale;
			this.scaleMultiplier = GTPlayer.Instance.ScaleMultiplier;
			if (this.scaleFactor != this.lastScaleFactor)
			{
				this.ScaleUpdate();
			}
			base.transform.eulerAngles = new Vector3(0f, this.mainCamera.transform.rotation.eulerAngles.y, 0f);
			this.syncPos = this.mainCamera.transform.position + this.headConstraint.rotation * this.head.trackingPositionOffset * this.lastScaleFactor + base.transform.rotation * this.headBodyOffset * this.lastScaleFactor;
			base.transform.position = this.syncPos;
			this.head.MapMine(this.lastScaleFactor, this.playerOffsetTransform);
			this.rightHand.MapMine(this.lastScaleFactor, this.playerOffsetTransform);
			this.leftHand.MapMine(this.lastScaleFactor, this.playerOffsetTransform);
			this.rightIndex.MapMyFinger(this.lerpValueFingers);
			this.rightMiddle.MapMyFinger(this.lerpValueFingers);
			this.rightThumb.MapMyFinger(this.lerpValueFingers);
			this.leftIndex.MapMyFinger(this.lerpValueFingers);
			this.leftMiddle.MapMyFinger(this.lerpValueFingers);
			this.leftThumb.MapMyFinger(this.lerpValueFingers);
			if (GorillaTagger.Instance.loadedDeviceName == "Oculus")
			{
				this.mainSkin.enabled = OVRManager.hasInputFocus;
			}
			this.bodyRenderer.ActiveBody.enabled = !GTPlayer.Instance.inOverlay;
			int i = this.loudnessCheckFrame - 1;
			this.loudnessCheckFrame = i;
			if (i < 0)
			{
				this.speakingLoudness = 0f;
				if (this.shouldSendSpeakingLoudness && this.netView)
				{
					PhotonVoiceView component = this.netView.GetComponent<PhotonVoiceView>();
					if (component && component.RecorderInUse)
					{
						MicWrapper micWrapper = component.RecorderInUse.InputSource as MicWrapper;
						if (micWrapper != null)
						{
							int num = this.replacementVoiceDetectionDelay;
							if (num > this.voiceSampleBuffer.Length)
							{
								Array.Resize<float>(ref this.voiceSampleBuffer, num);
							}
							float[] array = this.voiceSampleBuffer;
							if (micWrapper != null && micWrapper.Mic != null && micWrapper.Mic.samples >= num && micWrapper.Mic.GetData(array, micWrapper.Mic.samples - num))
							{
								float num2 = 0f;
								for (int j = 0; j < num; j++)
								{
									float num3 = Mathf.Sqrt(array[j]);
									if (num3 > num2)
									{
										num2 = num3;
									}
								}
								this.speakingLoudness = num2;
							}
						}
					}
				}
				this.loudnessCheckFrame = 10;
			}
		}
		if (this.creator != null)
		{
			if (GorillaGameManager.instance != null)
			{
				GorillaGameManager.instance.UpdatePlayerAppearance(this);
			}
			else if (this.setMatIndex != 0)
			{
				this.ChangeMaterialLocal(0);
				this.ForceResetFrozenEffect();
			}
		}
		if (this.inDuplicationZone)
		{
			this.renderTransform.position = base.transform.position + this.duplicationZone.VisualOffsetForRigs;
		}
		if (this.frozenEffect.activeSelf)
		{
			GorillaFreezeTagManager gorillaFreezeTagManager = GorillaGameManager.instance as GorillaFreezeTagManager;
			if (gorillaFreezeTagManager != null)
			{
				Vector3 localScale = this.frozenEffect.transform.localScale;
				Vector3 vector = localScale;
				vector.y = Mathf.Lerp(this.frozenEffectMinY, this.frozenEffectMaxY, this.frozenTimeElapsed / gorillaFreezeTagManager.freezeDuration);
				localScale = new Vector3(localScale.x, vector.y, localScale.z);
				this.frozenEffect.transform.localScale = localScale;
				this.frozenTimeElapsed += Time.deltaTime;
			}
		}
		if (this.TemporaryCosmeticEffects.Count > 0)
		{
			foreach (CosmeticEffectsOnPlayers.CosmeticEffect cosmeticEffect in this.TemporaryCosmeticEffects.ToArray())
			{
				if (Time.time - cosmeticEffect.effectStartedTime >= cosmeticEffect.effectDuration)
				{
					this.RemoveTemporaryCosmeticEffects(cosmeticEffect);
				}
			}
		}
		this.lateUpdateCallbacks.TryRunCallbacks();
	}

	// Token: 0x06001654 RID: 5716 RVA: 0x0006B808 File Offset: 0x00069A08
	private void RemoveTemporaryCosmeticEffects(CosmeticEffectsOnPlayers.CosmeticEffect effect)
	{
		bool flag;
		if (effect.newSkin != null && GorillaSkin.GetActiveSkin(this, out flag) == effect.newSkin)
		{
			GorillaSkin.ApplyToRig(this, null, GorillaSkin.SkinType.temporaryEffect);
			this.TemporaryCosmeticEffects.Remove(effect);
		}
		if (this.FPVEffectsParent != null)
		{
			this.SpawnFPVEffects(effect, false);
		}
	}

	// Token: 0x06001655 RID: 5717 RVA: 0x0006B863 File Offset: 0x00069A63
	public void SpawnSkinEffects(CosmeticEffectsOnPlayers.CosmeticEffect effect)
	{
		GorillaSkin.ApplyToRig(this, effect.newSkin, GorillaSkin.SkinType.temporaryEffect);
		this.TemporaryCosmeticEffects.Add(effect);
	}

	// Token: 0x06001656 RID: 5718 RVA: 0x0006B880 File Offset: 0x00069A80
	public void SpawnFPVEffects(CosmeticEffectsOnPlayers.CosmeticEffect effect, bool enable)
	{
		if (this.FPVEffectsParent == null)
		{
			return;
		}
		if (enable)
		{
			if (effect != null)
			{
				GameObject gameObject = ObjectPools.instance.Instantiate(effect.FPVEffect, this.FPVEffectsParent.transform.position, this.FPVEffectsParent.transform.rotation);
				if (gameObject != null)
				{
					gameObject.gameObject.transform.SetParent(this.FPVEffectsParent.transform);
					gameObject.gameObject.transform.localPosition = Vector3.zero;
				}
				this.TemporaryCosmeticEffects.Add(effect);
				return;
			}
		}
		else
		{
			foreach (object obj in this.FPVEffectsParent.transform)
			{
				Transform transform = (Transform)obj;
				ObjectPools.instance.Destroy(transform.gameObject);
			}
			this.TemporaryCosmeticEffects.Remove(effect);
		}
	}

	// Token: 0x1700027F RID: 639
	// (get) Token: 0x06001657 RID: 5719 RVA: 0x0006B984 File Offset: 0x00069B84
	public bool IsPlayerMeshHidden
	{
		get
		{
			return !this.mainSkin.enabled;
		}
	}

	// Token: 0x06001658 RID: 5720 RVA: 0x0006B994 File Offset: 0x00069B94
	public void SetPlayerMeshHidden(bool hide)
	{
		this.mainSkin.enabled = !hide;
		this.faceSkin.enabled = !hide;
		this.nameTagAnchor.SetActive(!hide);
		this.UpdateMatParticles(-1);
	}

	// Token: 0x06001659 RID: 5721 RVA: 0x0006B9CA File Offset: 0x00069BCA
	public void SetInvisibleToLocalPlayer(bool invisible)
	{
		if (this.IsInvisibleToLocalPlayer == invisible)
		{
			return;
		}
		this.IsInvisibleToLocalPlayer = invisible;
		this.nameTagAnchor.SetActive(!invisible);
		this.UpdateFriendshipBracelet();
	}

	// Token: 0x0600165A RID: 5722 RVA: 0x0006B9F2 File Offset: 0x00069BF2
	public void ChangeLayer(string layerName)
	{
		if (this.layerChanger != null)
		{
			this.layerChanger.ChangeLayer(base.transform.parent, layerName);
		}
		GTPlayer.Instance.ChangeLayer(layerName);
	}

	// Token: 0x0600165B RID: 5723 RVA: 0x0006BA24 File Offset: 0x00069C24
	public void RestoreLayer()
	{
		if (this.layerChanger != null)
		{
			this.layerChanger.RestoreOriginalLayers();
		}
		GTPlayer.Instance.RestoreLayer();
	}

	// Token: 0x0600165C RID: 5724 RVA: 0x000023F4 File Offset: 0x000005F4
	public void SetHeadBodyOffset()
	{
	}

	// Token: 0x0600165D RID: 5725 RVA: 0x0006BA49 File Offset: 0x00069C49
	public void VRRigResize(float ratioVar)
	{
		this.ratio *= ratioVar;
	}

	// Token: 0x0600165E RID: 5726 RVA: 0x0006BA5C File Offset: 0x00069C5C
	public int ReturnHandPosition()
	{
		return 0 + Mathf.FloorToInt(this.rightIndex.calcT * 9.99f) + Mathf.FloorToInt(this.rightMiddle.calcT * 9.99f) * 10 + Mathf.FloorToInt(this.rightThumb.calcT * 9.99f) * 100 + Mathf.FloorToInt(this.leftIndex.calcT * 9.99f) * 1000 + Mathf.FloorToInt(this.leftMiddle.calcT * 9.99f) * 10000 + Mathf.FloorToInt(this.leftThumb.calcT * 9.99f) * 100000 + this.leftHandHoldableStatus * 1000000 + this.rightHandHoldableStatus * 10000000;
	}

	// Token: 0x0600165F RID: 5727 RVA: 0x0006BB26 File Offset: 0x00069D26
	public void OnDestroy()
	{
		if (ApplicationQuittingState.IsQuitting)
		{
			return;
		}
		if (this.currentRopeSwingTarget && this.currentRopeSwingTarget.gameObject)
		{
			Object.Destroy(this.currentRopeSwingTarget.gameObject);
		}
		this.ClearRopeData();
	}

	// Token: 0x06001660 RID: 5728 RVA: 0x0006BB68 File Offset: 0x00069D68
	private InputStruct SerializeWriteShared()
	{
		InputStruct result = default(InputStruct);
		result.headRotation = BitPackUtils.PackQuaternionForNetwork(this.head.rigTarget.localRotation);
		result.rightHandLong = BitPackUtils.PackHandPosRotForNetwork(this.rightHand.rigTarget.localPosition, this.rightHand.rigTarget.localRotation);
		result.leftHandLong = BitPackUtils.PackHandPosRotForNetwork(this.leftHand.rigTarget.localPosition, this.leftHand.rigTarget.localRotation);
		result.position = BitPackUtils.PackWorldPosForNetwork(base.transform.position);
		result.handPosition = this.ReturnHandPosition();
		result.taggedById = (short)this.taggedById;
		int num = Mathf.Clamp(Mathf.RoundToInt(base.transform.rotation.eulerAngles.y + 360f) % 360, 0, 360);
		int num2 = Mathf.RoundToInt(Mathf.Clamp01(this.speakingLoudness) * 255f);
		int packedFields = num + (this.remoteUseReplacementVoice ? 512 : 0) + ((this.grabbedRopeIndex != -1) ? 1024 : 0) + (this.grabbedRopeIsPhotonView ? 2048 : 0) + (this.hoverboardVisual.IsHeld ? 8192 : 0) + (this.hoverboardVisual.IsLeftHanded ? 16384 : 0) + ((this.mountedMovingSurfaceId != -1) ? 32768 : 0) + (num2 << 16);
		result.packedFields = packedFields;
		result.packedCompetitiveData = this.PackCompetitiveData();
		if (this.grabbedRopeIndex != -1)
		{
			result.grabbedRopeIndex = this.grabbedRopeIndex;
			result.ropeBoneIndex = this.grabbedRopeBoneIndex;
			result.ropeGrabIsLeft = this.grabbedRopeIsLeft;
			result.ropeGrabIsBody = this.grabbedRopeIsBody;
			result.ropeGrabOffset = this.grabbedRopeOffset;
		}
		if (this.grabbedRopeIndex == -1 && this.mountedMovingSurfaceId != -1)
		{
			result.grabbedRopeIndex = this.mountedMovingSurfaceId;
			result.ropeGrabIsLeft = this.mountedMovingSurfaceIsLeft;
			result.ropeGrabIsBody = this.mountedMovingSurfaceIsBody;
			result.ropeGrabOffset = this.mountedMonkeBlockOffset;
		}
		if (this.hoverboardVisual.IsHeld)
		{
			result.hoverboardPosRot = BitPackUtils.PackHandPosRotForNetwork(this.hoverboardVisual.NominalLocalPosition, this.hoverboardVisual.NominalLocalRotation);
			result.hoverboardColor = BitPackUtils.PackColorForNetwork(this.hoverboardVisual.boardColor);
		}
		return result;
	}

	// Token: 0x06001661 RID: 5729 RVA: 0x0006BDD4 File Offset: 0x00069FD4
	private void SerializeReadShared(InputStruct data)
	{
		VRMap vrmap = this.head;
		Quaternion quaternion = BitPackUtils.UnpackQuaternionFromNetwork(data.headRotation);
		ref vrmap.syncRotation.SetValueSafe(quaternion);
		BitPackUtils.UnpackHandPosRotFromNetwork(data.rightHandLong, out this.tempVec, out this.tempQuat);
		this.rightHand.syncPos = this.tempVec;
		ref this.rightHand.syncRotation.SetValueSafe(this.tempQuat);
		BitPackUtils.UnpackHandPosRotFromNetwork(data.leftHandLong, out this.tempVec, out this.tempQuat);
		this.leftHand.syncPos = this.tempVec;
		ref this.leftHand.syncRotation.SetValueSafe(this.tempQuat);
		this.syncPos = BitPackUtils.UnpackWorldPosFromNetwork(data.position);
		this.handSync = data.handPosition;
		int packedFields = data.packedFields;
		int num = packedFields & 511;
		this.syncRotation.eulerAngles = this.SanitizeVector3(new Vector3(0f, (float)num, 0f));
		this.remoteUseReplacementVoice = ((packedFields & 512) != 0);
		int num2 = packedFields >> 16 & 255;
		this.speakingLoudness = (float)num2 / 255f;
		this.UpdateReplacementVoice();
		this.UnpackCompetitiveData(data.packedCompetitiveData);
		this.taggedById = (int)data.taggedById;
		bool flag = (packedFields & 1024) != 0;
		this.grabbedRopeIsPhotonView = ((packedFields & 2048) != 0);
		if (flag)
		{
			this.grabbedRopeIndex = data.grabbedRopeIndex;
			this.grabbedRopeBoneIndex = data.ropeBoneIndex;
			this.grabbedRopeIsLeft = data.ropeGrabIsLeft;
			this.grabbedRopeIsBody = data.ropeGrabIsBody;
			ref this.grabbedRopeOffset.SetValueSafe(data.ropeGrabOffset);
		}
		else
		{
			this.grabbedRopeIndex = -1;
		}
		bool flag2 = (packedFields & 32768) != 0;
		if (!flag && flag2)
		{
			this.mountedMovingSurfaceId = data.grabbedRopeIndex;
			this.mountedMovingSurfaceIsLeft = data.ropeGrabIsLeft;
			this.mountedMovingSurfaceIsBody = data.ropeGrabIsBody;
			ref this.mountedMonkeBlockOffset.SetValueSafe(data.ropeGrabOffset);
			this.movingSurfaceIsMonkeBlock = data.movingSurfaceIsMonkeBlock;
		}
		else
		{
			this.mountedMovingSurfaceId = -1;
		}
		bool flag3 = (packedFields & 8192) != 0;
		bool isHeldLeftHanded = (packedFields & 16384) != 0;
		if (flag3)
		{
			Vector3 v;
			Quaternion localRotation;
			BitPackUtils.UnpackHandPosRotFromNetwork(data.hoverboardPosRot, out v, out localRotation);
			Color boardColor = BitPackUtils.UnpackColorFromNetwork(data.hoverboardColor);
			if (localRotation.IsValid())
			{
				this.hoverboardVisual.SetIsHeld(isHeldLeftHanded, v.ClampMagnitudeSafe(1f), localRotation, boardColor);
			}
		}
		else if (this.hoverboardVisual.gameObject.activeSelf)
		{
			this.hoverboardVisual.SetNotHeld();
		}
		if (this.grabbedRopeIsPhotonView)
		{
			this.localGrabOverrideBlend = -1f;
		}
		this.UpdateRopeData();
		this.UpdateMovingMonkeBlockData();
		this.AddVelocityToQueue(this.syncPos, data.serverTimeStamp);
	}

	// Token: 0x06001662 RID: 5730 RVA: 0x0006C080 File Offset: 0x0006A280
	void IWrappedSerializable.OnSerializeWrite(PhotonStream stream, PhotonMessageInfo info)
	{
		InputStruct inputStruct = this.SerializeWriteShared();
		stream.SendNext(inputStruct.headRotation);
		stream.SendNext(inputStruct.rightHandLong);
		stream.SendNext(inputStruct.leftHandLong);
		stream.SendNext(inputStruct.position);
		stream.SendNext(inputStruct.handPosition);
		stream.SendNext(inputStruct.packedFields);
		stream.SendNext(inputStruct.packedCompetitiveData);
		if (this.grabbedRopeIndex != -1)
		{
			stream.SendNext(inputStruct.grabbedRopeIndex);
			stream.SendNext(inputStruct.ropeBoneIndex);
			stream.SendNext(inputStruct.ropeGrabIsLeft);
			stream.SendNext(inputStruct.ropeGrabIsBody);
			stream.SendNext(inputStruct.ropeGrabOffset);
		}
		else if (this.mountedMovingSurfaceId != -1)
		{
			stream.SendNext(inputStruct.grabbedRopeIndex);
			stream.SendNext(inputStruct.ropeGrabIsLeft);
			stream.SendNext(inputStruct.ropeGrabIsBody);
			stream.SendNext(inputStruct.ropeGrabOffset);
			stream.SendNext(inputStruct.movingSurfaceIsMonkeBlock);
		}
		if ((inputStruct.packedFields & 8192) != 0)
		{
			stream.SendNext(inputStruct.hoverboardPosRot);
			stream.SendNext(inputStruct.hoverboardColor);
		}
	}

	// Token: 0x06001663 RID: 5731 RVA: 0x0006C1FC File Offset: 0x0006A3FC
	void IWrappedSerializable.OnSerializeRead(PhotonStream stream, PhotonMessageInfo info)
	{
		InputStruct inputStruct = new InputStruct
		{
			headRotation = (int)stream.ReceiveNext(),
			rightHandLong = (long)stream.ReceiveNext(),
			leftHandLong = (long)stream.ReceiveNext(),
			position = (long)stream.ReceiveNext(),
			handPosition = (int)stream.ReceiveNext(),
			packedFields = (int)stream.ReceiveNext(),
			packedCompetitiveData = (short)stream.ReceiveNext()
		};
		bool flag = (inputStruct.packedFields & 1024) != 0;
		bool flag2 = (inputStruct.packedFields & 32768) != 0;
		if (flag)
		{
			inputStruct.grabbedRopeIndex = (int)stream.ReceiveNext();
			inputStruct.ropeBoneIndex = (int)stream.ReceiveNext();
			inputStruct.ropeGrabIsLeft = (bool)stream.ReceiveNext();
			inputStruct.ropeGrabIsBody = (bool)stream.ReceiveNext();
			inputStruct.ropeGrabOffset = (Vector3)stream.ReceiveNext();
		}
		else if (flag2)
		{
			inputStruct.grabbedRopeIndex = (int)stream.ReceiveNext();
			inputStruct.ropeGrabIsLeft = (bool)stream.ReceiveNext();
			inputStruct.ropeGrabIsBody = (bool)stream.ReceiveNext();
			inputStruct.ropeGrabOffset = (Vector3)stream.ReceiveNext();
		}
		if ((inputStruct.packedFields & 8192) != 0)
		{
			inputStruct.hoverboardPosRot = (long)stream.ReceiveNext();
			inputStruct.hoverboardColor = (short)stream.ReceiveNext();
		}
		inputStruct.serverTimeStamp = info.SentServerTime;
		this.SerializeReadShared(inputStruct);
	}

	// Token: 0x06001664 RID: 5732 RVA: 0x0006C3A4 File Offset: 0x0006A5A4
	public object OnSerializeWrite()
	{
		InputStruct inputStruct = this.SerializeWriteShared();
		double serverTimeStamp = NetworkSystem.Instance.SimTick / 1000.0;
		inputStruct.serverTimeStamp = serverTimeStamp;
		return inputStruct;
	}

	// Token: 0x06001665 RID: 5733 RVA: 0x0006C3E0 File Offset: 0x0006A5E0
	public void OnSerializeRead(object objectData)
	{
		InputStruct data = (InputStruct)objectData;
		this.SerializeReadShared(data);
	}

	// Token: 0x06001666 RID: 5734 RVA: 0x0006C3FC File Offset: 0x0006A5FC
	private void UpdateExtrapolationTarget()
	{
		float num = (float)(NetworkSystem.Instance.SimTime - this.remoteLatestTimestamp);
		num -= 0.15f;
		num = Mathf.Clamp(num, -0.5f, 0.5f);
		this.syncPos += this.remoteVelocity * num;
		this.remoteCorrectionNeeded = this.syncPos - base.transform.position;
		if (this.remoteCorrectionNeeded.magnitude > 1.5f && this.grabbedRopeIndex <= 0)
		{
			base.transform.position = this.syncPos;
			this.remoteCorrectionNeeded = Vector3.zero;
		}
	}

	// Token: 0x06001667 RID: 5735 RVA: 0x0006C4A8 File Offset: 0x0006A6A8
	private void UpdateRopeData()
	{
		if (this.previousGrabbedRope == this.grabbedRopeIndex && this.previousGrabbedRopeBoneIndex == this.grabbedRopeBoneIndex && this.previousGrabbedRopeWasLeft == this.grabbedRopeIsLeft && this.previousGrabbedRopeWasBody == this.grabbedRopeIsBody)
		{
			return;
		}
		this.ClearRopeData();
		if (this.grabbedRopeIndex != -1)
		{
			GorillaRopeSwing gorillaRopeSwing;
			if (this.grabbedRopeIsPhotonView)
			{
				PhotonView photonView = PhotonView.Find(this.grabbedRopeIndex);
				GorillaClimbable gorillaClimbable;
				HandHoldXSceneRef handHoldXSceneRef;
				VRRigSerializer vrrigSerializer;
				if (photonView.TryGetComponent<GorillaClimbable>(out gorillaClimbable))
				{
					this.currentHoldParent = photonView.transform;
				}
				else if (photonView.TryGetComponent<HandHoldXSceneRef>(out handHoldXSceneRef))
				{
					GameObject targetObject = handHoldXSceneRef.targetObject;
					this.currentHoldParent = ((targetObject != null) ? targetObject.transform : null);
				}
				else if (photonView && photonView.TryGetComponent<VRRigSerializer>(out vrrigSerializer))
				{
					this.currentHoldParent = ((this.grabbedRopeBoneIndex == 1) ? vrrigSerializer.VRRig.leftHandHoldsPlayer.transform : vrrigSerializer.VRRig.rightHandHoldsPlayer.transform);
				}
			}
			else if (RopeSwingManager.instance.TryGetRope(this.grabbedRopeIndex, out gorillaRopeSwing) && gorillaRopeSwing != null)
			{
				if (this.currentRopeSwingTarget == null || this.currentRopeSwingTarget.gameObject == null)
				{
					this.currentRopeSwingTarget = new GameObject("RopeSwingTarget").transform;
				}
				if (gorillaRopeSwing.AttachRemotePlayer(this.creator.ActorNumber, this.grabbedRopeBoneIndex, this.currentRopeSwingTarget, this.grabbedRopeOffset))
				{
					this.currentRopeSwing = gorillaRopeSwing;
				}
				this.lastRopeGrabTimer = 0f;
			}
		}
		else if (this.previousGrabbedRope != -1)
		{
			PhotonView photonView2 = PhotonView.Find(this.previousGrabbedRope);
			VRRigSerializer vrrigSerializer2;
			if (photonView2 && photonView2.TryGetComponent<VRRigSerializer>(out vrrigSerializer2) && vrrigSerializer2.VRRig == VRRig.LocalRig)
			{
				EquipmentInteractor.instance.ForceDropEquipment(this.bodyHolds);
				EquipmentInteractor.instance.ForceDropEquipment(this.leftHolds);
				EquipmentInteractor.instance.ForceDropEquipment(this.rightHolds);
			}
		}
		this.shouldLerpToRope = true;
		this.previousGrabbedRope = this.grabbedRopeIndex;
		this.previousGrabbedRopeBoneIndex = this.grabbedRopeBoneIndex;
		this.previousGrabbedRopeWasLeft = this.grabbedRopeIsLeft;
		this.previousGrabbedRopeWasBody = this.grabbedRopeIsBody;
	}

	// Token: 0x06001668 RID: 5736 RVA: 0x0006C6E8 File Offset: 0x0006A8E8
	private void UpdateMovingMonkeBlockData()
	{
		if (this.mountedMonkeBlockOffset.sqrMagnitude > 2f)
		{
			this.mountedMovingSurfaceId = -1;
			this.mountedMovingSurfaceIsLeft = false;
			this.mountedMovingSurfaceIsBody = false;
			this.mountedMonkeBlock = null;
			this.mountedMovingSurface = null;
		}
		if (this.prevMovingSurfaceID == this.mountedMovingSurfaceId && this.movingSurfaceWasBody == this.mountedMovingSurfaceIsBody && this.movingSurfaceWasLeft == this.mountedMovingSurfaceIsLeft && this.movingSurfaceWasMonkeBlock == this.movingSurfaceIsMonkeBlock)
		{
			return;
		}
		if (this.mountedMovingSurfaceId == -1)
		{
			this.mountedMovingSurfaceIsLeft = false;
			this.mountedMovingSurfaceIsBody = false;
			this.mountedMonkeBlock = null;
			this.mountedMovingSurface = null;
		}
		else if (this.movingSurfaceIsMonkeBlock)
		{
			this.mountedMonkeBlock = BuilderTable.instance.GetPiece(this.mountedMovingSurfaceId);
			if (this.mountedMonkeBlock == null)
			{
				this.mountedMovingSurfaceId = -1;
				this.mountedMovingSurfaceIsLeft = false;
				this.mountedMovingSurfaceIsBody = false;
				this.mountedMonkeBlock = null;
				this.mountedMovingSurface = null;
			}
		}
		else if (MovingSurfaceManager.instance == null || !MovingSurfaceManager.instance.TryGetMovingSurface(this.mountedMovingSurfaceId, out this.mountedMovingSurface))
		{
			this.mountedMovingSurfaceId = -1;
			this.mountedMovingSurfaceIsLeft = false;
			this.mountedMovingSurfaceIsBody = false;
			this.mountedMonkeBlock = null;
			this.mountedMovingSurface = null;
		}
		if (this.mountedMovingSurfaceId != -1 && this.prevMovingSurfaceID == -1)
		{
			this.shouldLerpToMovingSurface = true;
			this.lastMountedSurfaceTimer = 0f;
		}
		this.prevMovingSurfaceID = this.mountedMovingSurfaceId;
		this.movingSurfaceWasLeft = this.mountedMovingSurfaceIsLeft;
		this.movingSurfaceWasBody = this.mountedMovingSurfaceIsBody;
		this.movingSurfaceWasMonkeBlock = this.movingSurfaceIsMonkeBlock;
	}

	// Token: 0x06001669 RID: 5737 RVA: 0x0006C87C File Offset: 0x0006AA7C
	public static void AttachLocalPlayerToMovingSurface(int blockId, bool isLeft, bool isBody, Vector3 offset, bool isMonkeBlock)
	{
		if (GorillaTagger.hasInstance && GorillaTagger.Instance.offlineVRRig)
		{
			GorillaTagger.Instance.offlineVRRig.mountedMovingSurfaceId = blockId;
			GorillaTagger.Instance.offlineVRRig.mountedMovingSurfaceIsLeft = isLeft;
			GorillaTagger.Instance.offlineVRRig.mountedMovingSurfaceIsBody = isBody;
			GorillaTagger.Instance.offlineVRRig.movingSurfaceIsMonkeBlock = isMonkeBlock;
			GorillaTagger.Instance.offlineVRRig.mountedMonkeBlockOffset = offset;
		}
	}

	// Token: 0x0600166A RID: 5738 RVA: 0x0006C8F2 File Offset: 0x0006AAF2
	public static void DetachLocalPlayerFromMovingSurface()
	{
		if (GorillaTagger.hasInstance && GorillaTagger.Instance.offlineVRRig)
		{
			GorillaTagger.Instance.offlineVRRig.mountedMovingSurfaceId = -1;
		}
	}

	// Token: 0x0600166B RID: 5739 RVA: 0x0006C91C File Offset: 0x0006AB1C
	public static void AttachLocalPlayerToPhotonView(PhotonView view, XRNode xrNode, Vector3 offset, Vector3 velocity)
	{
		if (GorillaTagger.hasInstance && GorillaTagger.Instance.offlineVRRig)
		{
			GorillaTagger.Instance.offlineVRRig.grabbedRopeIndex = view.ViewID;
			GorillaTagger.Instance.offlineVRRig.grabbedRopeIsLeft = (xrNode == XRNode.LeftHand);
			GorillaTagger.Instance.offlineVRRig.grabbedRopeOffset = offset;
			GorillaTagger.Instance.offlineVRRig.grabbedRopeIsPhotonView = true;
		}
	}

	// Token: 0x0600166C RID: 5740 RVA: 0x0006C989 File Offset: 0x0006AB89
	public static void DetachLocalPlayerFromPhotonView()
	{
		if (GorillaTagger.hasInstance && GorillaTagger.Instance.offlineVRRig)
		{
			GorillaTagger.Instance.offlineVRRig.grabbedRopeIndex = -1;
		}
	}

	// Token: 0x0600166D RID: 5741 RVA: 0x0006C9B4 File Offset: 0x0006ABB4
	private void ClearRopeData()
	{
		if (this.currentRopeSwing)
		{
			this.currentRopeSwing.DetachRemotePlayer(this.creator.ActorNumber);
		}
		if (this.currentRopeSwingTarget)
		{
			this.currentRopeSwingTarget.SetParent(null);
		}
		this.currentRopeSwing = null;
		this.currentHoldParent = null;
	}

	// Token: 0x0600166E RID: 5742 RVA: 0x0006CA0B File Offset: 0x0006AC0B
	public void ChangeMaterial(int materialIndex, PhotonMessageInfo info)
	{
		if (info.Sender == PhotonNetwork.MasterClient)
		{
			this.ChangeMaterialLocal(materialIndex);
		}
	}

	// Token: 0x0600166F RID: 5743 RVA: 0x0006CA24 File Offset: 0x0006AC24
	public void UpdateFrozenEffect(bool enable)
	{
		if (this.frozenEffect != null && ((!this.frozenEffect.activeSelf && enable) || (this.frozenEffect.activeSelf && !enable)))
		{
			this.frozenEffect.SetActive(enable);
			if (enable)
			{
				this.frozenTimeElapsed = 0f;
			}
			else
			{
				Vector3 localScale = this.frozenEffect.transform.localScale;
				localScale = new Vector3(localScale.x, this.frozenEffectMinY, localScale.z);
				this.frozenEffect.transform.localScale = localScale;
			}
		}
		if (this.iceCubeLeft != null && ((!this.iceCubeLeft.activeSelf && enable) || (this.iceCubeLeft.activeSelf && !enable)))
		{
			this.iceCubeLeft.SetActive(enable);
		}
		if (this.iceCubeRight != null && ((!this.iceCubeRight.activeSelf && enable) || (this.iceCubeRight.activeSelf && !enable)))
		{
			this.iceCubeRight.SetActive(enable);
		}
	}

	// Token: 0x06001670 RID: 5744 RVA: 0x0006CB30 File Offset: 0x0006AD30
	public void ForceResetFrozenEffect()
	{
		this.frozenEffect.SetActive(false);
		this.iceCubeRight.SetActive(false);
		this.iceCubeLeft.SetActive(false);
	}

	// Token: 0x06001671 RID: 5745 RVA: 0x0006CB58 File Offset: 0x0006AD58
	public void ChangeMaterialLocal(int materialIndex)
	{
		if (this.setMatIndex == materialIndex)
		{
			return;
		}
		this.setMatIndex = materialIndex;
		if (this.setMatIndex > -1 && this.setMatIndex < this.materialsToChangeTo.Length)
		{
			this.bodyRenderer.SetMaterialIndex(materialIndex);
		}
		this.UpdateMatParticles(materialIndex);
		if (materialIndex > 0 && VRRig.LocalRig != this)
		{
			this.PlayTaggedEffect();
		}
	}

	// Token: 0x06001672 RID: 5746 RVA: 0x0006CBBC File Offset: 0x0006ADBC
	public void PlayTaggedEffect()
	{
		TagEffectPack tagEffectPack = null;
		quaternion q = base.transform.rotation;
		TagEffectsLibrary.EffectType effectType = (VRRig.LocalRig == this) ? TagEffectsLibrary.EffectType.FIRST_PERSON : TagEffectsLibrary.EffectType.THIRD_PERSON;
		if (GorillaGameManager.instance != null)
		{
			GorillaGameManager.instance.lastTaggedActorNr.TryGetValue(this.OwningNetPlayer.ActorNumber, out this.taggedById);
		}
		NetPlayer player = NetworkSystem.Instance.GetPlayer(this.taggedById);
		RigContainer rigContainer;
		if (player != null && VRRigCache.Instance.TryGetVrrig(player, out rigContainer))
		{
			tagEffectPack = rigContainer.Rig.CosmeticEffectPack;
			if (tagEffectPack && tagEffectPack.shouldFaceTagger && effectType == TagEffectsLibrary.EffectType.THIRD_PERSON)
			{
				q = Quaternion.LookRotation((rigContainer.Rig.transform.position - base.transform.position).normalized);
			}
		}
		TagEffectsLibrary.PlayEffect(base.transform, false, this.scaleFactor, effectType, this.CosmeticEffectPack, tagEffectPack, q);
	}

	// Token: 0x06001673 RID: 5747 RVA: 0x0006CCB8 File Offset: 0x0006AEB8
	public void UpdateMatParticles(int materialIndex)
	{
		if (this.lavaParticleSystem != null)
		{
			if (!this.isOfflineVRRig && materialIndex == 2 && this.lavaParticleSystem.isStopped)
			{
				this.lavaParticleSystem.Play();
			}
			else if (!this.isOfflineVRRig && this.lavaParticleSystem.isPlaying)
			{
				this.lavaParticleSystem.Stop();
			}
		}
		if (this.rockParticleSystem != null)
		{
			if (!this.isOfflineVRRig && materialIndex == 1 && this.rockParticleSystem.isStopped)
			{
				this.rockParticleSystem.Play();
			}
			else if (!this.isOfflineVRRig && this.rockParticleSystem.isPlaying)
			{
				this.rockParticleSystem.Stop();
			}
		}
		if (this.iceParticleSystem != null)
		{
			if (!this.isOfflineVRRig && materialIndex == 3 && this.rockParticleSystem.isStopped)
			{
				this.iceParticleSystem.Play();
			}
			else if (!this.isOfflineVRRig && this.iceParticleSystem.isPlaying)
			{
				this.iceParticleSystem.Stop();
			}
		}
		if (this.snowFlakeParticleSystem != null)
		{
			if (!this.isOfflineVRRig && materialIndex == 14 && this.snowFlakeParticleSystem.isStopped)
			{
				this.snowFlakeParticleSystem.Play();
				return;
			}
			if (!this.isOfflineVRRig && this.snowFlakeParticleSystem.isPlaying)
			{
				this.snowFlakeParticleSystem.Stop();
			}
		}
	}

	// Token: 0x06001674 RID: 5748 RVA: 0x0006CE18 File Offset: 0x0006B018
	public void InitializeNoobMaterial(float red, float green, float blue, PhotonMessageInfoWrapped info)
	{
		this.IncrementRPC(info, "RPC_InitializeNoobMaterial");
		NetworkSystem.Instance.GetPlayer(info.senderID);
		string userID = NetworkSystem.Instance.GetUserID(info.senderID);
		if (info.senderID == NetworkSystem.Instance.GetOwningPlayerID(this.rigSerializer.gameObject) && (!this.initialized || (this.initialized && GorillaComputer.instance.friendJoinCollider.playerIDsCurrentlyTouching.Contains(userID)) || (this.initialized && CosmeticWardrobeProximityDetector.IsUserNearWardrobe(userID))))
		{
			this.initialized = true;
			blue = blue.ClampSafe(0f, 1f);
			red = red.ClampSafe(0f, 1f);
			green = green.ClampSafe(0f, 1f);
			this.InitializeNoobMaterialLocal(red, green, blue);
		}
	}

	// Token: 0x06001675 RID: 5749 RVA: 0x0006CEF4 File Offset: 0x0006B0F4
	public void InitializeNoobMaterialLocal(float red, float green, float blue)
	{
		Color color = new Color(red, green, blue);
		this.EnsureInstantiatedMaterial();
		if (this.myDefaultSkinMaterialInstance != null)
		{
			color.r = Mathf.Clamp(color.r, 0f, 1f);
			color.g = Mathf.Clamp(color.g, 0f, 1f);
			color.b = Mathf.Clamp(color.b, 0f, 1f);
			this.skeleton.UpdateColor(color);
			this.myDefaultSkinMaterialInstance.color = color;
		}
		this.SetColor(color);
		ValueTuple<bool, Permission.ManagedByEnum> customNicknamePermissionStatus = KIDManager.Instance.GetCustomNicknamePermissionStatus();
		bool isNamePermissionEnabled = (customNicknamePermissionStatus.Item1 || customNicknamePermissionStatus.Item2 == Permission.ManagedByEnum.PLAYER) && customNicknamePermissionStatus.Item2 != Permission.ManagedByEnum.PROHIBITED;
		this.UpdateName(isNamePermissionEnabled);
	}

	// Token: 0x06001676 RID: 5750 RVA: 0x0006CFC8 File Offset: 0x0006B1C8
	public void UpdateName(bool isNamePermissionEnabled)
	{
		if (!this.isOfflineVRRig && this.creator != null)
		{
			string text = isNamePermissionEnabled ? this.creator.NickName : this.creator.DefaultName;
			this.playerNameVisible = this.NormalizeName(true, text);
		}
		else if (this.showName && NetworkSystem.Instance != null)
		{
			this.playerNameVisible = (isNamePermissionEnabled ? NetworkSystem.Instance.GetMyNickName() : NetworkSystem.Instance.GetMyDefaultName());
		}
		this.playerText1.text = this.playerNameVisible;
		this.playerText2.text = this.playerNameVisible;
		if (this.creator != null)
		{
			this.creator.SanitizedNickName = this.playerNameVisible;
		}
	}

	// Token: 0x06001677 RID: 5751 RVA: 0x0006D080 File Offset: 0x0006B280
	public void UpdateName()
	{
		ValueTuple<bool, Permission.ManagedByEnum> customNicknamePermissionStatus = KIDManager.Instance.GetCustomNicknamePermissionStatus();
		bool isNamePermissionEnabled = (customNicknamePermissionStatus.Item1 || customNicknamePermissionStatus.Item2 == Permission.ManagedByEnum.PLAYER) && customNicknamePermissionStatus.Item2 != Permission.ManagedByEnum.PROHIBITED;
		this.UpdateName(isNamePermissionEnabled);
	}

	// Token: 0x06001678 RID: 5752 RVA: 0x0006D0C0 File Offset: 0x0006B2C0
	public string NormalizeName(bool doIt, string text)
	{
		if (doIt)
		{
			if (GorillaComputer.instance.CheckAutoBanListForName(text))
			{
				text = new string(Array.FindAll<char>(text.ToCharArray(), (char c) => Utils.IsASCIILetterOrDigit(c)));
				if (text.Length > 12)
				{
					text = text.Substring(0, 11);
				}
				text = text.ToUpper();
			}
			else
			{
				text = "BADGORILLA";
			}
		}
		return text;
	}

	// Token: 0x06001679 RID: 5753 RVA: 0x0006D137 File Offset: 0x0006B337
	public void SetJumpLimitLocal(float maxJumpSpeed)
	{
		GTPlayer.Instance.maxJumpSpeed = maxJumpSpeed;
	}

	// Token: 0x0600167A RID: 5754 RVA: 0x0006D144 File Offset: 0x0006B344
	public void SetJumpMultiplierLocal(float jumpMultiplier)
	{
		GTPlayer.Instance.jumpMultiplier = jumpMultiplier;
	}

	// Token: 0x0600167B RID: 5755 RVA: 0x0006D154 File Offset: 0x0006B354
	public void RequestMaterialColor(int askingPlayerID, PhotonMessageInfoWrapped info)
	{
		this.IncrementRPC(info, "RequestMaterialColor");
		Player playerRef = ((PunNetPlayer)NetworkSystem.Instance.GetPlayer(info.senderID)).PlayerRef;
		if (this.netView.IsMine)
		{
			this.netView.GetView.RPC("RPC_InitializeNoobMaterial", playerRef, new object[]
			{
				this.myDefaultSkinMaterialInstance.color.r,
				this.myDefaultSkinMaterialInstance.color.g,
				this.myDefaultSkinMaterialInstance.color.b
			});
		}
	}

	// Token: 0x0600167C RID: 5756 RVA: 0x0006D1FC File Offset: 0x0006B3FC
	public void RequestCosmetics(PhotonMessageInfoWrapped info)
	{
		NetPlayer player = NetworkSystem.Instance.GetPlayer(info.senderID);
		if (this.netView.IsMine && CosmeticsController.hasInstance)
		{
			if (CosmeticsController.instance.isHidingCosmeticsFromRemotePlayers)
			{
				this.netView.SendRPC("RPC_HideAllCosmetics", info.Sender, Array.Empty<object>());
				return;
			}
			int[] array = CosmeticsController.instance.currentWornSet.ToPackedIDArray();
			int[] array2 = CosmeticsController.instance.tryOnSet.ToPackedIDArray();
			this.netView.SendRPC("RPC_UpdateCosmeticsWithTryonPacked", player, new object[]
			{
				array,
				array2
			});
		}
	}

	// Token: 0x0600167D RID: 5757 RVA: 0x0006D29C File Offset: 0x0006B49C
	public void PlayTagSoundLocal(int soundIndex, float soundVolume, bool stopCurrentAudio)
	{
		if (soundIndex < 0 || soundIndex >= this.clipToPlay.Length)
		{
			return;
		}
		this.tagSound.volume = Mathf.Min(0.25f, soundVolume);
		if (stopCurrentAudio)
		{
			this.tagSound.Stop();
		}
		this.tagSound.GTPlayOneShot(this.clipToPlay[soundIndex], 1f);
	}

	// Token: 0x0600167E RID: 5758 RVA: 0x0006D2F5 File Offset: 0x0006B4F5
	public void AssignDrumToMusicDrums(int drumIndex, AudioSource drum)
	{
		if (drumIndex >= 0 && drumIndex < this.musicDrums.Length && drum != null)
		{
			this.musicDrums[drumIndex] = drum;
		}
	}

	// Token: 0x0600167F RID: 5759 RVA: 0x0006D318 File Offset: 0x0006B518
	public void PlayDrum(int drumIndex, float drumVolume, PhotonMessageInfoWrapped info)
	{
		this.IncrementRPC(info, "RPC_PlayDrum");
		NetPlayer player = NetworkSystem.Instance.GetPlayer(info.senderID);
		RigContainer rigContainer;
		if (VRRigCache.Instance.TryGetVrrig(player, out rigContainer))
		{
			this.senderRig = rigContainer.Rig;
		}
		if (this.senderRig == null || this.senderRig.muted)
		{
			return;
		}
		if (drumIndex < 0 || drumIndex >= this.musicDrums.Length || (this.senderRig.transform.position - base.transform.position).sqrMagnitude > 9f || !float.IsFinite(drumVolume))
		{
			GorillaNot.instance.SendReport("inappropriate tag data being sent drum", player.UserId, player.NickName);
			return;
		}
		AudioSource audioSource = this.netView.IsMine ? GorillaTagger.Instance.offlineVRRig.musicDrums[drumIndex] : this.musicDrums[drumIndex];
		if (!audioSource.gameObject.activeSelf)
		{
			return;
		}
		float instrumentVolume = GorillaComputer.instance.instrumentVolume;
		audioSource.time = 0f;
		audioSource.volume = Mathf.Max(Mathf.Min(instrumentVolume, drumVolume * instrumentVolume), 0f);
		audioSource.GTPlay();
	}

	// Token: 0x06001680 RID: 5760 RVA: 0x0006D44C File Offset: 0x0006B64C
	public int AssignInstrumentToInstrumentSelfOnly(TransferrableObject instrument)
	{
		if (instrument == null)
		{
			return -1;
		}
		if (!this.instrumentSelfOnly.Contains(instrument))
		{
			this.instrumentSelfOnly.Add(instrument);
		}
		return this.instrumentSelfOnly.IndexOf(instrument);
	}

	// Token: 0x06001681 RID: 5761 RVA: 0x0006D480 File Offset: 0x0006B680
	public void PlaySelfOnlyInstrument(int selfOnlyIndex, int noteIndex, float instrumentVol, PhotonMessageInfoWrapped info)
	{
		this.IncrementRPC(info, "RPC_PlaySelfOnlyInstrument");
		NetPlayer player = NetworkSystem.Instance.GetPlayer(info.senderID);
		if (player == this.netView.Owner && !this.muted)
		{
			if (selfOnlyIndex >= 0 && selfOnlyIndex < this.instrumentSelfOnly.Count && float.IsFinite(instrumentVol))
			{
				if (this.instrumentSelfOnly[selfOnlyIndex].gameObject.activeSelf)
				{
					this.instrumentSelfOnly[selfOnlyIndex].PlayNote(noteIndex, Mathf.Max(Mathf.Min(GorillaComputer.instance.instrumentVolume, instrumentVol * GorillaComputer.instance.instrumentVolume), 0f) / 2f);
					return;
				}
			}
			else
			{
				GorillaNot.instance.SendReport("inappropriate tag data being sent self only instrument", player.UserId, player.NickName);
			}
		}
	}

	// Token: 0x06001682 RID: 5762 RVA: 0x0006D55C File Offset: 0x0006B75C
	public void PlayHandTapLocal(int soundIndex, bool isLeftHand, float tapVolume)
	{
		if (soundIndex > -1 && soundIndex < GTPlayer.Instance.materialData.Count)
		{
			GTPlayer.MaterialData materialData = GTPlayer.Instance.materialData[soundIndex];
			AudioSource audioSource = isLeftHand ? this.leftHandPlayer : this.rightHandPlayer;
			audioSource.volume = tapVolume;
			AudioClip clip = materialData.overrideAudio ? materialData.audio : GTPlayer.Instance.materialData[0].audio;
			audioSource.GTPlayOneShot(clip, 1f);
		}
	}

	// Token: 0x06001683 RID: 5763 RVA: 0x0006D5DC File Offset: 0x0006B7DC
	internal void OnHandTap(int soundIndex, bool isLeftHand, float handSpeed, Vector3 tapDir)
	{
		if (soundIndex < 0 || soundIndex > GTPlayer.Instance.materialData.Count - 1)
		{
			return;
		}
		if (isLeftHand)
		{
			FXSystem.PlayFX(this.GetLeftHandEffect(soundIndex, handSpeed, tapDir));
			if (CrittersManager.instance.IsNotNull() && CrittersManager.instance.LocalAuthority() && CrittersManager.instance.rigSetupByRig[this].IsNotNull())
			{
				CrittersLoudNoise crittersLoudNoise = (CrittersLoudNoise)CrittersManager.instance.rigSetupByRig[this].rigActors[0].actorSet;
				if (crittersLoudNoise.IsNotNull())
				{
					crittersLoudNoise.PlayHandTapLocal(true);
					return;
				}
			}
		}
		else
		{
			FXSystem.PlayFX(this.GetRightHandEffect(soundIndex, handSpeed, tapDir));
			if (CrittersManager.instance.IsNotNull() && CrittersManager.instance.LocalAuthority() && CrittersManager.instance.rigSetupByRig[this].IsNotNull())
			{
				CrittersLoudNoise crittersLoudNoise2 = (CrittersLoudNoise)CrittersManager.instance.rigSetupByRig[this].rigActors[2].actorSet;
				if (crittersLoudNoise2.IsNotNull())
				{
					crittersLoudNoise2.PlayHandTapLocal(false);
				}
			}
		}
	}

	// Token: 0x06001684 RID: 5764 RVA: 0x0006D70B File Offset: 0x0006B90B
	internal HandEffectContext GetLeftHandEffect(int index, float handSpeed, Vector3 tapDir)
	{
		return this.SetHandEffectData(index, this._leftHandEffect, this.leftHand, handSpeed, tapDir);
	}

	// Token: 0x06001685 RID: 5765 RVA: 0x0006D722 File Offset: 0x0006B922
	internal HandEffectContext GetRightHandEffect(int index, float handSpeed, Vector3 tapDir)
	{
		return this.SetHandEffectData(index, this._rightHandEffect, this.rightHand, handSpeed, tapDir);
	}

	// Token: 0x06001686 RID: 5766 RVA: 0x0006D73C File Offset: 0x0006B93C
	internal HandEffectContext SetHandEffectData(int index, HandEffectContext effectContext, VRMap targetHand, float handSpeed, Vector3 tapDir)
	{
		GTPlayer.MaterialData handSurfaceData = this.GetHandSurfaceData(index);
		Vector3 b = tapDir * this.tapPointDistance * this.scaleFactor;
		if (this.isOfflineVRRig)
		{
			Vector3 b2 = targetHand.rigTarget.rotation * targetHand.trackingPositionOffset * this.scaleFactor;
			effectContext.position = targetHand.rigTarget.position - b2 + b;
		}
		else
		{
			Quaternion rotation = targetHand.rigTarget.parent.rotation * targetHand.syncRotation;
			Vector3 b3 = this.netSyncPos.GetPredictedFuture() - base.transform.position;
			Vector3 b2 = rotation * targetHand.trackingPositionOffset * this.scaleFactor;
			effectContext.position = targetHand.rigTarget.parent.TransformPoint(targetHand.netSyncPos.GetPredictedFuture()) - b2 + b + b3;
		}
		int[] prefabHashes = effectContext.prefabHashes;
		int num = 0;
		HashWrapper hashWrapper = GTPlayer.Instance.materialDatasSO.surfaceEffects[handSurfaceData.surfaceEffectIndex];
		prefabHashes[num] = hashWrapper;
		if (RoomSystem.JoinedRoom && GorillaGameModes.GameMode.ActiveGameMode.IsNotNull())
		{
			effectContext.prefabHashes[1] = GorillaGameModes.GameMode.ActiveGameMode.SpecialHandFX(this.creator, this.rigContainer);
		}
		else
		{
			effectContext.prefabHashes[1] = -1;
		}
		effectContext.soundFX = handSurfaceData.audio;
		effectContext.clipVolume = handSpeed * this.handSpeedToVolumeModifier;
		effectContext.handSpeed = handSpeed;
		return effectContext;
	}

	// Token: 0x06001687 RID: 5767 RVA: 0x0006D8C4 File Offset: 0x0006BAC4
	internal GTPlayer.MaterialData GetHandSurfaceData(int index)
	{
		GTPlayer.MaterialData materialData = GTPlayer.Instance.materialData[index];
		if (!materialData.overrideAudio)
		{
			materialData = GTPlayer.Instance.materialData[0];
		}
		return materialData;
	}

	// Token: 0x06001688 RID: 5768 RVA: 0x0006D8FC File Offset: 0x0006BAFC
	public void PlaySplashEffect(Vector3 splashPosition, Quaternion splashRotation, float splashScale, float boundingRadius, bool bigSplash, bool enteringWater, PhotonMessageInfoWrapped info)
	{
		this.IncrementRPC(info, "RPC_PlaySplashEffect");
		NetPlayer player = NetworkSystem.Instance.GetPlayer(info.senderID);
		if (player == this.netView.Owner)
		{
			float num = 10000f;
			if (splashPosition.IsValid(num) && splashRotation.IsValid() && float.IsFinite(splashScale) && float.IsFinite(boundingRadius))
			{
				if ((base.transform.position - splashPosition).sqrMagnitude >= 9f)
				{
					return;
				}
				float time = Time.time;
				int num2 = -1;
				float num3 = time + 10f;
				for (int i = 0; i < this.splashEffectTimes.Length; i++)
				{
					if (this.splashEffectTimes[i] < num3)
					{
						num3 = this.splashEffectTimes[i];
						num2 = i;
					}
				}
				if (time - 0.5f > num3)
				{
					this.splashEffectTimes[num2] = time;
					boundingRadius = Mathf.Clamp(boundingRadius, 0.0001f, 0.5f);
					ObjectPools.instance.Instantiate(GTPlayer.Instance.waterParams.rippleEffect, splashPosition, splashRotation, GTPlayer.Instance.waterParams.rippleEffectScale * boundingRadius * 2f);
					splashScale = Mathf.Clamp(splashScale, 1E-05f, 1f);
					ObjectPools.instance.Instantiate(GTPlayer.Instance.waterParams.splashEffect, splashPosition, splashRotation, splashScale).GetComponent<WaterSplashEffect>().PlayEffect(bigSplash, enteringWater, splashScale, null);
					return;
				}
				return;
			}
		}
		GorillaNot.instance.SendReport("inappropriate tag data being sent splash effect", player.UserId, player.NickName);
	}

	// Token: 0x06001689 RID: 5769 RVA: 0x0006DA94 File Offset: 0x0006BC94
	[Rpc(RpcSources.StateAuthority, RpcTargets.All)]
	public void RPC_EnableNonCosmeticHandItem(bool enable, bool isLeftHand, RpcInfo info = default(RpcInfo))
	{
		PhotonMessageInfoWrapped info2 = new PhotonMessageInfoWrapped(info);
		this.IncrementRPC(info2, "EnableNonCosmeticHandItem");
		if (info2.Sender == this.creator)
		{
			this.senderRig = GorillaGameManager.StaticFindRigForPlayer(info2.Sender);
			if (this.senderRig == null)
			{
				return;
			}
			if (isLeftHand && this.nonCosmeticLeftHandItem)
			{
				this.senderRig.nonCosmeticLeftHandItem.EnableItem(enable);
				return;
			}
			if (!isLeftHand && this.nonCosmeticRightHandItem)
			{
				this.senderRig.nonCosmeticRightHandItem.EnableItem(enable);
				return;
			}
		}
		else
		{
			GorillaNot.instance.SendReport("inappropriate tag data being sent Enable Non Cosmetic Hand Item", info2.Sender.UserId, info2.Sender.NickName);
		}
	}

	// Token: 0x0600168A RID: 5770 RVA: 0x0006DB54 File Offset: 0x0006BD54
	[PunRPC]
	public void EnableNonCosmeticHandItemRPC(bool enable, bool isLeftHand, PhotonMessageInfoWrapped info)
	{
		NetPlayer sender = info.Sender;
		this.IncrementRPC(info, "EnableNonCosmeticHandItem");
		if (sender == this.netView.Owner)
		{
			this.senderRig = GorillaGameManager.StaticFindRigForPlayer(sender);
			if (this.senderRig == null)
			{
				return;
			}
			if (isLeftHand && this.nonCosmeticLeftHandItem)
			{
				this.senderRig.nonCosmeticLeftHandItem.EnableItem(enable);
				return;
			}
			if (!isLeftHand && this.nonCosmeticRightHandItem)
			{
				this.senderRig.nonCosmeticRightHandItem.EnableItem(enable);
				return;
			}
		}
		else
		{
			GorillaNot.instance.SendReport("inappropriate tag data being sent Enable Non Cosmetic Hand Item", info.Sender.UserId, info.Sender.NickName);
		}
	}

	// Token: 0x0600168B RID: 5771 RVA: 0x0006DC0C File Offset: 0x0006BE0C
	public bool IsMakingFistLeft()
	{
		if (this.isOfflineVRRig)
		{
			return ControllerInputPoller.GripFloat(XRNode.LeftHand) > 0.25f && ControllerInputPoller.TriggerFloat(XRNode.LeftHand) > 0.25f;
		}
		return this.leftIndex.calcT > 0.25f && this.leftMiddle.calcT > 0.25f;
	}

	// Token: 0x0600168C RID: 5772 RVA: 0x0006DC64 File Offset: 0x0006BE64
	public bool IsMakingFistRight()
	{
		if (this.isOfflineVRRig)
		{
			return ControllerInputPoller.GripFloat(XRNode.RightHand) > 0.25f && ControllerInputPoller.TriggerFloat(XRNode.RightHand) > 0.25f;
		}
		return this.rightIndex.calcT > 0.25f && this.rightMiddle.calcT > 0.25f;
	}

	// Token: 0x0600168D RID: 5773 RVA: 0x0006DCBC File Offset: 0x0006BEBC
	public bool IsMakingFiveLeft()
	{
		if (this.isOfflineVRRig)
		{
			return ControllerInputPoller.GripFloat(XRNode.LeftHand) < 0.25f && ControllerInputPoller.TriggerFloat(XRNode.LeftHand) < 0.25f;
		}
		return this.leftIndex.calcT < 0.25f && this.leftMiddle.calcT < 0.25f;
	}

	// Token: 0x0600168E RID: 5774 RVA: 0x0006DD14 File Offset: 0x0006BF14
	public bool IsMakingFiveRight()
	{
		if (this.isOfflineVRRig)
		{
			return ControllerInputPoller.GripFloat(XRNode.RightHand) < 0.25f && ControllerInputPoller.TriggerFloat(XRNode.RightHand) < 0.25f;
		}
		return this.rightIndex.calcT < 0.25f && this.rightMiddle.calcT < 0.25f;
	}

	// Token: 0x0600168F RID: 5775 RVA: 0x0006DD6C File Offset: 0x0006BF6C
	public VRMap GetMakingFist(bool debug, out bool isLeftHand)
	{
		if (this.IsMakingFistRight())
		{
			isLeftHand = false;
			return this.rightHand;
		}
		if (this.IsMakingFistLeft())
		{
			isLeftHand = true;
			return this.leftHand;
		}
		isLeftHand = false;
		return null;
	}

	// Token: 0x06001690 RID: 5776 RVA: 0x0006DD98 File Offset: 0x0006BF98
	public void PlayGeodeEffect(Vector3 hitPosition)
	{
		if ((base.transform.position - hitPosition).sqrMagnitude < 9f && this.geodeCrackingSound)
		{
			this.geodeCrackingSound.GTPlay();
		}
	}

	// Token: 0x06001691 RID: 5777 RVA: 0x0006DDE0 File Offset: 0x0006BFE0
	public void PlayClimbSound(AudioClip clip, bool isLeftHand)
	{
		if (isLeftHand)
		{
			this.leftHandPlayer.volume = 0.1f;
			this.leftHandPlayer.clip = clip;
			this.leftHandPlayer.GTPlayOneShot(this.leftHandPlayer.clip, 1f);
			return;
		}
		this.rightHandPlayer.volume = 0.1f;
		this.rightHandPlayer.clip = clip;
		this.rightHandPlayer.GTPlayOneShot(this.rightHandPlayer.clip, 1f);
	}

	// Token: 0x06001692 RID: 5778 RVA: 0x0006DE60 File Offset: 0x0006C060
	public void HideAllCosmetics(PhotonMessageInfo info)
	{
		this.IncrementRPC(info, "HideAllCosmetics");
		if (NetworkSystem.Instance.GetPlayer(info.Sender) == this.netView.Owner)
		{
			this.LocalUpdateCosmeticsWithTryon(CosmeticsController.CosmeticSet.EmptySet, CosmeticsController.CosmeticSet.EmptySet);
			return;
		}
		GorillaNot.instance.SendReport("inappropriate tag data being sent update cosmetics", info.Sender.UserId, info.Sender.NickName);
	}

	// Token: 0x06001693 RID: 5779 RVA: 0x0006DED0 File Offset: 0x0006C0D0
	public void UpdateCosmetics(string[] currentItems, PhotonMessageInfoWrapped info)
	{
		this.IncrementRPC(info, "RPC_UpdateCosmetics");
		NetPlayer player = NetworkSystem.Instance.GetPlayer(info.senderID);
		if (info.Sender == this.netView.Owner && currentItems.Length <= 16)
		{
			CosmeticsController.CosmeticSet newSet = new CosmeticsController.CosmeticSet(currentItems, CosmeticsController.instance);
			this.LocalUpdateCosmetics(newSet);
			return;
		}
		GorillaNot.instance.SendReport("inappropriate tag data being sent update cosmetics", player.UserId, player.NickName);
	}

	// Token: 0x06001694 RID: 5780 RVA: 0x0006DF48 File Offset: 0x0006C148
	public void UpdateCosmeticsWithTryon(string[] currentItems, string[] tryOnItems, PhotonMessageInfoWrapped info)
	{
		this.IncrementRPC(info, "RPC_UpdateCosmeticsWithTryon");
		NetPlayer player = NetworkSystem.Instance.GetPlayer(info.senderID);
		if (info.Sender == this.netView.Owner && currentItems.Length == 16 && tryOnItems.Length == 16)
		{
			CosmeticsController.CosmeticSet newSet = new CosmeticsController.CosmeticSet(currentItems, CosmeticsController.instance);
			CosmeticsController.CosmeticSet newTryOnSet = new CosmeticsController.CosmeticSet(tryOnItems, CosmeticsController.instance);
			this.LocalUpdateCosmeticsWithTryon(newSet, newTryOnSet);
			return;
		}
		GorillaNot.instance.SendReport("inappropriate tag data being sent update cosmetics with tryon", player.UserId, player.NickName);
	}

	// Token: 0x06001695 RID: 5781 RVA: 0x0006DFD8 File Offset: 0x0006C1D8
	public void UpdateCosmeticsWithTryon(int[] currentItemsPacked, int[] tryOnItemsPacked, PhotonMessageInfoWrapped info)
	{
		this.IncrementRPC(info, "RPC_UpdateCosmeticsWithTryon");
		NetPlayer player = NetworkSystem.Instance.GetPlayer(info.senderID);
		if (info.Sender == this.netView.Owner && CosmeticsController.instance.ValidatePackedItems(currentItemsPacked) && CosmeticsController.instance.ValidatePackedItems(tryOnItemsPacked))
		{
			CosmeticsController.CosmeticSet newSet = new CosmeticsController.CosmeticSet(currentItemsPacked, CosmeticsController.instance);
			CosmeticsController.CosmeticSet newTryOnSet = new CosmeticsController.CosmeticSet(tryOnItemsPacked, CosmeticsController.instance);
			this.LocalUpdateCosmeticsWithTryon(newSet, newTryOnSet);
			return;
		}
		GorillaNot.instance.SendReport("inappropriate tag data being sent update cosmetics with tryon", player.UserId, player.NickName);
	}

	// Token: 0x06001696 RID: 5782 RVA: 0x0006E076 File Offset: 0x0006C276
	public void LocalUpdateCosmetics(CosmeticsController.CosmeticSet newSet)
	{
		this.cosmeticSet = newSet;
		if (this.InitializedCosmetics)
		{
			this.SetCosmeticsActive();
		}
	}

	// Token: 0x06001697 RID: 5783 RVA: 0x0006E08D File Offset: 0x0006C28D
	public void LocalUpdateCosmeticsWithTryon(CosmeticsController.CosmeticSet newSet, CosmeticsController.CosmeticSet newTryOnSet)
	{
		this.cosmeticSet = newSet;
		this.tryOnSet = newTryOnSet;
		if (this.initializedCosmetics)
		{
			this.SetCosmeticsActive();
		}
	}

	// Token: 0x06001698 RID: 5784 RVA: 0x0006E0AB File Offset: 0x0006C2AB
	private void CheckForEarlyAccess()
	{
		if (this.concatStringOfCosmeticsAllowed.Contains("Early Access Supporter Pack"))
		{
			this.concatStringOfCosmeticsAllowed += "LBAAE.LFAAM.LFAAN.LHAAA.LHAAK.LHAAL.LHAAM.LHAAN.LHAAO.LHAAP.LHABA.LHABB.";
		}
		this.InitializedCosmetics = true;
	}

	// Token: 0x06001699 RID: 5785 RVA: 0x0006E0DC File Offset: 0x0006C2DC
	public void SetCosmeticsActive()
	{
		if (CosmeticsController.instance == null || !CosmeticsV2Spawner_Dirty.allPartsInstantiated)
		{
			return;
		}
		this.prevSet.CopyItems(this.mergedSet);
		this.mergedSet.MergeSets(this.inTryOnRoom ? this.tryOnSet : null, this.cosmeticSet);
		BodyDockPositions component = base.GetComponent<BodyDockPositions>();
		this.mergedSet.ActivateCosmetics(this.prevSet, this, component, this.cosmeticsObjectRegistry);
	}

	// Token: 0x0600169A RID: 5786 RVA: 0x0006E154 File Offset: 0x0006C354
	public void GetCosmeticsPlayFabCatalogData()
	{
		if (CosmeticsController.instance != null)
		{
			PlayFabClientAPI.GetUserInventory(new GetUserInventoryRequest(), delegate(GetUserInventoryResult result)
			{
				Dictionary<string, string> dictionary = new Dictionary<string, string>();
				foreach (ItemInstance itemInstance in result.Inventory)
				{
					if (!dictionary.ContainsKey(itemInstance.ItemId))
					{
						dictionary[itemInstance.ItemId] = itemInstance.ItemId;
						if (itemInstance.CatalogVersion == CosmeticsController.instance.catalog)
						{
							this.concatStringOfCosmeticsAllowed += itemInstance.ItemId;
						}
					}
				}
				if (CosmeticsV2Spawner_Dirty.allPartsInstantiated)
				{
					this.Handle_CosmeticsV2_OnPostInstantiateAllPrefabs_DoEnableAllCosmetics();
				}
			}, delegate(PlayFabError error)
			{
				this.initializedCosmetics = true;
				if (CosmeticsV2Spawner_Dirty.allPartsInstantiated)
				{
					this.SetCosmeticsActive();
				}
			}, null, null);
		}
		this.concatStringOfCosmeticsAllowed += "Slingshot";
		this.concatStringOfCosmeticsAllowed += BuilderSetManager.instance.GetStarterSetsConcat();
	}

	// Token: 0x0600169B RID: 5787 RVA: 0x0006E1C8 File Offset: 0x0006C3C8
	public void GenerateFingerAngleLookupTables()
	{
		this.GenerateTableIndex(ref this.leftIndex);
		this.GenerateTableIndex(ref this.rightIndex);
		this.GenerateTableMiddle(ref this.leftMiddle);
		this.GenerateTableMiddle(ref this.rightMiddle);
		this.GenerateTableThumb(ref this.leftThumb);
		this.GenerateTableThumb(ref this.rightThumb);
	}

	// Token: 0x0600169C RID: 5788 RVA: 0x0006E220 File Offset: 0x0006C420
	private void GenerateTableThumb(ref VRMapThumb thumb)
	{
		thumb.angle1Table = new Quaternion[11];
		thumb.angle2Table = new Quaternion[11];
		for (int i = 0; i < thumb.angle1Table.Length; i++)
		{
			Debug.Log((float)i / 10f);
			thumb.angle1Table[i] = Quaternion.Lerp(Quaternion.Euler(thumb.startingAngle1), Quaternion.Euler(thumb.closedAngle1), (float)i / 10f);
			thumb.angle2Table[i] = Quaternion.Lerp(Quaternion.Euler(thumb.startingAngle2), Quaternion.Euler(thumb.closedAngle2), (float)i / 10f);
		}
	}

	// Token: 0x0600169D RID: 5789 RVA: 0x0006E2D8 File Offset: 0x0006C4D8
	private void GenerateTableIndex(ref VRMapIndex index)
	{
		index.angle1Table = new Quaternion[11];
		index.angle2Table = new Quaternion[11];
		index.angle3Table = new Quaternion[11];
		for (int i = 0; i < index.angle1Table.Length; i++)
		{
			index.angle1Table[i] = Quaternion.Lerp(Quaternion.Euler(index.startingAngle1), Quaternion.Euler(index.closedAngle1), (float)i / 10f);
			index.angle2Table[i] = Quaternion.Lerp(Quaternion.Euler(index.startingAngle2), Quaternion.Euler(index.closedAngle2), (float)i / 10f);
			index.angle3Table[i] = Quaternion.Lerp(Quaternion.Euler(index.startingAngle3), Quaternion.Euler(index.closedAngle3), (float)i / 10f);
		}
	}

	// Token: 0x0600169E RID: 5790 RVA: 0x0006E3C0 File Offset: 0x0006C5C0
	private void GenerateTableMiddle(ref VRMapMiddle middle)
	{
		middle.angle1Table = new Quaternion[11];
		middle.angle2Table = new Quaternion[11];
		middle.angle3Table = new Quaternion[11];
		for (int i = 0; i < middle.angle1Table.Length; i++)
		{
			middle.angle1Table[i] = Quaternion.Lerp(Quaternion.Euler(middle.startingAngle1), Quaternion.Euler(middle.closedAngle1), (float)i / 10f);
			middle.angle2Table[i] = Quaternion.Lerp(Quaternion.Euler(middle.startingAngle2), Quaternion.Euler(middle.closedAngle2), (float)i / 10f);
			middle.angle3Table[i] = Quaternion.Lerp(Quaternion.Euler(middle.startingAngle3), Quaternion.Euler(middle.closedAngle3), (float)i / 10f);
		}
	}

	// Token: 0x0600169F RID: 5791 RVA: 0x0006E4A8 File Offset: 0x0006C6A8
	private Quaternion SanitizeQuaternion(Quaternion quat)
	{
		if (float.IsNaN(quat.w) || float.IsNaN(quat.x) || float.IsNaN(quat.y) || float.IsNaN(quat.z) || float.IsInfinity(quat.w) || float.IsInfinity(quat.x) || float.IsInfinity(quat.y) || float.IsInfinity(quat.z))
		{
			return Quaternion.identity;
		}
		return quat;
	}

	// Token: 0x060016A0 RID: 5792 RVA: 0x0006E524 File Offset: 0x0006C724
	private Vector3 SanitizeVector3(Vector3 vec)
	{
		if (float.IsNaN(vec.x) || float.IsNaN(vec.y) || float.IsNaN(vec.z) || float.IsInfinity(vec.x) || float.IsInfinity(vec.y) || float.IsInfinity(vec.z))
		{
			return Vector3.zero;
		}
		return Vector3.ClampMagnitude(vec, 5000f);
	}

	// Token: 0x060016A1 RID: 5793 RVA: 0x0006E590 File Offset: 0x0006C790
	private void IncrementRPC(PhotonMessageInfoWrapped info, string sourceCall)
	{
		if (GorillaGameManager.instance != null)
		{
			GorillaNot.IncrementRPCCall(info, sourceCall);
		}
	}

	// Token: 0x060016A2 RID: 5794 RVA: 0x0006E5A6 File Offset: 0x0006C7A6
	private void IncrementRPC(PhotonMessageInfo info, string sourceCall)
	{
		if (GorillaGameManager.instance != null)
		{
			GorillaNot.IncrementRPCCall(info, sourceCall);
		}
	}

	// Token: 0x060016A3 RID: 5795 RVA: 0x0006E5BC File Offset: 0x0006C7BC
	private void AddVelocityToQueue(Vector3 position, double serverTime)
	{
		Vector3 velocity;
		if (this.velocityHistoryList.Count == 0)
		{
			velocity = Vector3.zero;
		}
		else
		{
			velocity = (position - this.lastPosition) / (float)(serverTime - this.velocityHistoryList[0].time);
		}
		this.velocityHistoryList.Add(new VRRig.VelocityTime(velocity, serverTime));
		this.lastPosition = position;
	}

	// Token: 0x060016A4 RID: 5796 RVA: 0x0006E620 File Offset: 0x0006C820
	private Vector3 ReturnVelocityAtTime(double timeToReturn)
	{
		if (this.velocityHistoryList.Count <= 1)
		{
			return Vector3.zero;
		}
		int num = 0;
		int num2 = this.velocityHistoryList.Count - 1;
		int num3 = 0;
		if (num2 == num)
		{
			return this.velocityHistoryList[num].vel;
		}
		while (num2 - num > 1 && num3 < 1000)
		{
			num3++;
			int num4 = (num2 - num) / 2;
			if (this.velocityHistoryList[num4].time > timeToReturn)
			{
				num2 = num4;
			}
			else
			{
				num = num4;
			}
		}
		float num5 = (float)(this.velocityHistoryList[num].time - timeToReturn);
		double num6 = this.velocityHistoryList[num].time - this.velocityHistoryList[num2].time;
		if (num6 == 0.0)
		{
			num6 = 0.001;
		}
		num5 /= (float)num6;
		num5 = Mathf.Clamp(num5, 0f, 1f);
		return Vector3.Lerp(this.velocityHistoryList[num].vel, this.velocityHistoryList[num2].vel, num5);
	}

	// Token: 0x060016A5 RID: 5797 RVA: 0x0006E732 File Offset: 0x0006C932
	public Vector3 LatestVelocity()
	{
		if (this.velocityHistoryList.Count > 0)
		{
			return this.velocityHistoryList[0].vel;
		}
		return Vector3.zero;
	}

	// Token: 0x060016A6 RID: 5798 RVA: 0x0006E759 File Offset: 0x0006C959
	public bool IsPositionInRange(Vector3 position, float range)
	{
		return (this.syncPos - position).IsShorterThan(range * this.scaleFactor);
	}

	// Token: 0x060016A7 RID: 5799 RVA: 0x0006E774 File Offset: 0x0006C974
	public bool CheckTagDistanceRollback(VRRig otherRig, float max, float timeInterval)
	{
		Vector3 a;
		Vector3 b;
		GorillaMath.LineSegClosestPoints(this.syncPos, -this.LatestVelocity() * timeInterval, otherRig.syncPos, -otherRig.LatestVelocity() * timeInterval, out a, out b);
		return Vector3.SqrMagnitude(a - b) < max * max * this.scaleFactor;
	}

	// Token: 0x060016A8 RID: 5800 RVA: 0x0006E7D0 File Offset: 0x0006C9D0
	public Vector3 ClampVelocityRelativeToPlayerSafe(Vector3 inVel, float max)
	{
		Vector3 vector = Vector3.zero;
		ref vector.SetValueSafe(inVel);
		Vector3 vector2 = (this.velocityHistoryList.Count > 0) ? this.velocityHistoryList[0].vel : Vector3.zero;
		Vector3 vector3 = vector - vector2;
		vector3 = Vector3.ClampMagnitude(vector3, max);
		vector = vector2 + vector3;
		return vector;
	}

	// Token: 0x14000046 RID: 70
	// (add) Token: 0x060016A9 RID: 5801 RVA: 0x0006E82C File Offset: 0x0006CA2C
	// (remove) Token: 0x060016AA RID: 5802 RVA: 0x0006E864 File Offset: 0x0006CA64
	public event Action<Color> OnColorChanged;

	// Token: 0x060016AB RID: 5803 RVA: 0x0006E89C File Offset: 0x0006CA9C
	public void SetColor(Color color)
	{
		this.skeleton.UpdateColor(color);
		Action<Color> onColorChanged = this.OnColorChanged;
		if (onColorChanged != null)
		{
			onColorChanged(color);
		}
		Action<Color> action = this.onColorInitialized;
		if (action != null)
		{
			action(color);
		}
		this.onColorInitialized = delegate(Color color1)
		{
		};
		this.colorInitialized = true;
		this.playerColor = color;
		if (this.OnDataChange != null)
		{
			this.OnDataChange();
		}
	}

	// Token: 0x060016AC RID: 5804 RVA: 0x0006E91F File Offset: 0x0006CB1F
	public void OnColorInitialized(Action<Color> action)
	{
		if (this.colorInitialized)
		{
			action(this.playerColor);
			return;
		}
		this.onColorInitialized = (Action<Color>)Delegate.Combine(this.onColorInitialized, action);
	}

	// Token: 0x14000047 RID: 71
	// (add) Token: 0x060016AD RID: 5805 RVA: 0x0006E950 File Offset: 0x0006CB50
	// (remove) Token: 0x060016AE RID: 5806 RVA: 0x0006E988 File Offset: 0x0006CB88
	public event Action<int> OnQuestScoreChanged;

	// Token: 0x060016AF RID: 5807 RVA: 0x0006E9C0 File Offset: 0x0006CBC0
	public void SetQuestScore(int score)
	{
		this.SetQuestScoreLocal(score);
		Action<int> onQuestScoreChanged = this.OnQuestScoreChanged;
		if (onQuestScoreChanged != null)
		{
			onQuestScoreChanged(this.currentQuestScore);
		}
		if (this.netView != null)
		{
			this.netView.SendRPC("RPC_UpdateQuestScore", RpcTarget.All, new object[]
			{
				this.currentQuestScore
			});
		}
	}

	// Token: 0x060016B0 RID: 5808 RVA: 0x0006EA1E File Offset: 0x0006CC1E
	public int GetCurrentQuestScore()
	{
		if (!this._scoreUpdated)
		{
			this.SetQuestScoreLocal(ProgressionController.TotalPoints);
		}
		return this.currentQuestScore;
	}

	// Token: 0x060016B1 RID: 5809 RVA: 0x0006EA39 File Offset: 0x0006CC39
	private void SetQuestScoreLocal(int score)
	{
		this.currentQuestScore = score;
		this._scoreUpdated = true;
	}

	// Token: 0x060016B2 RID: 5810 RVA: 0x0006EA4C File Offset: 0x0006CC4C
	public void UpdateQuestScore(int score, PhotonMessageInfoWrapped info)
	{
		this.IncrementRPC(info, "UpdateQuestScore");
		if (NetworkSystem.Instance.GetPlayer(info.senderID) == this.netView.Owner)
		{
			if (!this.updateQuestCallLimit.CheckCallTime(Time.time))
			{
				return;
			}
			if (score < this.currentQuestScore)
			{
				return;
			}
			this.SetQuestScoreLocal(score);
			Action<int> onQuestScoreChanged = this.OnQuestScoreChanged;
			if (onQuestScoreChanged == null)
			{
				return;
			}
			onQuestScoreChanged(this.currentQuestScore);
		}
	}

	// Token: 0x060016B3 RID: 5811 RVA: 0x0006EABC File Offset: 0x0006CCBC
	public void RequestQuestScore(PhotonMessageInfoWrapped info)
	{
		this.IncrementRPC(info, "RequestQuestScore");
		NetPlayer player = NetworkSystem.Instance.GetPlayer(info.senderID);
		if (player.CheckSingleCallRPC(NetPlayer.SingleCallRPC.CMS_RequestQuestScore))
		{
			return;
		}
		if (this.netView.IsMine)
		{
			this.netView.SendRPC("RPC_UpdateQuestScore", player, new object[]
			{
				this.currentQuestScore
			});
		}
	}

	// Token: 0x060016B4 RID: 5812 RVA: 0x0006EB24 File Offset: 0x0006CD24
	public void OnEnable()
	{
		EyeScannerMono.Register(this);
		GorillaComputer.RegisterOnNametagSettingChanged(new Action<bool>(this.UpdateName));
		if (this.currentRopeSwingTarget != null)
		{
			this.currentRopeSwingTarget.SetParent(null);
		}
		if (!this.isOfflineVRRig)
		{
			PlayerCosmeticsSystem.RegisterCosmeticCallback(this.creator.ActorNumber, this);
		}
		this.bodyRenderer.SetDefaults();
		this.SetInvisibleToLocalPlayer(false);
		if (this.isOfflineVRRig)
		{
			HandHold.HandPositionRequestOverride += this.HandHold_HandPositionRequestOverride;
			HandHold.HandPositionReleaseOverride += this.HandHold_HandPositionReleaseOverride;
			return;
		}
		VRRigJobManager.Instance.RegisterVRRig(this);
	}

	// Token: 0x060016B5 RID: 5813 RVA: 0x0006EBC4 File Offset: 0x0006CDC4
	void IPreDisable.PreDisable()
	{
		try
		{
			this.ClearRopeData();
			if (this.currentRopeSwingTarget)
			{
				this.currentRopeSwingTarget.SetParent(base.transform);
			}
			this.EnableHuntWatch(false);
			this.EnablePaintbrawlCosmetics(false);
			this.ClearPartyMemberStatus();
			this.concatStringOfCosmeticsAllowed = "";
			this.rawCosmeticString = "";
			if (this.cosmeticSet != null)
			{
				this.mergedSet.DeactivateAllCosmetcs(this.myBodyDockPositions, CosmeticsController.instance.nullItem, this.cosmeticsObjectRegistry);
				this.mergedSet.ClearSet(CosmeticsController.instance.nullItem);
				this.prevSet.ClearSet(CosmeticsController.instance.nullItem);
				this.tryOnSet.ClearSet(CosmeticsController.instance.nullItem);
				this.cosmeticSet.ClearSet(CosmeticsController.instance.nullItem);
			}
			if (!this.isOfflineVRRig)
			{
				PlayerCosmeticsSystem.RemoveCosmeticCallback(this.creator.ActorNumber);
				this.pendingCosmeticUpdate = true;
			}
		}
		catch (Exception)
		{
		}
	}

	// Token: 0x060016B6 RID: 5814 RVA: 0x0006ECDC File Offset: 0x0006CEDC
	public void OnDisable()
	{
		try
		{
			GorillaSkin.ApplyToRig(this, null, GorillaSkin.SkinType.gameMode);
			this.ChangeMaterialLocal(0);
			GorillaComputer.UnregisterOnNametagSettingChanged(new Action<bool>(this.UpdateName));
			this.netView = null;
			this.voiceAudio = null;
			this.muted = false;
			this.initialized = false;
			this.initializedCosmetics = false;
			this.inTryOnRoom = false;
			this.timeSpawned = 0f;
			this.setMatIndex = 0;
			this.currentCosmeticTries = 0;
			this.velocityHistoryList.Clear();
			this.netSyncPos.Reset();
			this.rightHand.netSyncPos.Reset();
			this.leftHand.netSyncPos.Reset();
			this.ForceResetFrozenEffect();
			this.nativeScale = (this.frameScale = (this.lastScaleFactor = 1f));
			base.transform.localScale = Vector3.one;
			this.currentQuestScore = 0;
			this._scoreUpdated = false;
			this.TemporaryCosmeticEffects.Clear();
			try
			{
				CallLimitType<CallLimiter>[] callSettings = this.fxSettings.callSettings;
				for (int i = 0; i < callSettings.Length; i++)
				{
					callSettings[i].CallLimitSettings.Reset();
				}
			}
			catch
			{
				Debug.LogError("fxtype missing in fxSettings, please fix or remove this");
			}
		}
		catch (Exception)
		{
		}
		if (this.isOfflineVRRig)
		{
			HandHold.HandPositionRequestOverride -= this.HandHold_HandPositionRequestOverride;
			HandHold.HandPositionReleaseOverride -= this.HandHold_HandPositionReleaseOverride;
		}
		else
		{
			VRRigJobManager.Instance.DeregisterVRRig(this);
		}
		EyeScannerMono.Unregister(this);
		this.creator = null;
	}

	// Token: 0x060016B7 RID: 5815 RVA: 0x0006EE84 File Offset: 0x0006D084
	private void HandHold_HandPositionReleaseOverride(HandHold hh, bool rightHand)
	{
		if (rightHand)
		{
			this.rightHand.handholdOverrideTarget = null;
			return;
		}
		this.leftHand.handholdOverrideTarget = null;
	}

	// Token: 0x060016B8 RID: 5816 RVA: 0x0006EEA2 File Offset: 0x0006D0A2
	private void HandHold_HandPositionRequestOverride(HandHold hh, bool rightHand, Vector3 pos)
	{
		if (rightHand)
		{
			this.rightHand.handholdOverrideTarget = hh.transform;
			this.rightHand.handholdOverrideTargetOffset = pos;
			return;
		}
		this.leftHand.handholdOverrideTarget = hh.transform;
		this.leftHand.handholdOverrideTargetOffset = pos;
	}

	// Token: 0x060016B9 RID: 5817 RVA: 0x0006EEE4 File Offset: 0x0006D0E4
	public void NetInitialize()
	{
		this.timeSpawned = Time.time;
		if (NetworkSystem.Instance.InRoom)
		{
			GorillaGameManager instance = GorillaGameManager.instance;
			if (instance != null)
			{
				if (instance is GorillaHuntManager || instance.GameModeName() == "HUNT")
				{
					this.EnableHuntWatch(true);
				}
				else if (instance is GorillaPaintbrawlManager || instance.GameModeName() == "PAINTBRAWL")
				{
					this.EnablePaintbrawlCosmetics(true);
				}
			}
			else
			{
				string gameModeString = NetworkSystem.Instance.GameModeString;
				if (!gameModeString.IsNullOrEmpty())
				{
					string text = gameModeString;
					if (text.Contains("HUNT"))
					{
						this.EnableHuntWatch(true);
					}
					else if (text.Contains("PAINTBRAWL"))
					{
						this.EnablePaintbrawlCosmetics(true);
					}
				}
			}
			this.UpdateFriendshipBracelet();
			if (this.IsLocalPartyMember && !this.isOfflineVRRig)
			{
				FriendshipGroupDetection.Instance.SendVerifyPartyMember(this.creator);
			}
		}
		if (this.netView != null)
		{
			base.transform.position = this.netView.gameObject.transform.position;
			base.transform.rotation = this.netView.gameObject.transform.rotation;
		}
		try
		{
			Action action = VRRig.newPlayerJoined;
			if (action != null)
			{
				action();
			}
		}
		catch (Exception message)
		{
			Debug.LogError(message);
		}
	}

	// Token: 0x060016BA RID: 5818 RVA: 0x0006F040 File Offset: 0x0006D240
	public void GrabbedByPlayer(VRRig grabbedByRig, bool grabbedBody, bool grabbedLeftHand, bool grabbedWithLeftHand)
	{
		GorillaClimbable climbable = grabbedWithLeftHand ? grabbedByRig.leftHandHoldsPlayer : grabbedByRig.rightHandHoldsPlayer;
		GorillaHandClimber gorillaHandClimber;
		if (grabbedBody)
		{
			gorillaHandClimber = EquipmentInteractor.instance.BodyClimber;
		}
		else if (grabbedLeftHand)
		{
			gorillaHandClimber = EquipmentInteractor.instance.LeftClimber;
		}
		else
		{
			gorillaHandClimber = EquipmentInteractor.instance.RightClimber;
		}
		gorillaHandClimber.SetCanRelease(false);
		GTPlayer.Instance.BeginClimbing(climbable, gorillaHandClimber, null);
		this.grabbedRopeIsBody = grabbedBody;
		this.grabbedRopeIsLeft = grabbedLeftHand;
		this.grabbedRopeIndex = grabbedByRig.netView.ViewID;
		this.grabbedRopeBoneIndex = (grabbedWithLeftHand ? 1 : 0);
		this.grabbedRopeOffset = Vector3.zero;
		this.grabbedRopeIsPhotonView = true;
	}

	// Token: 0x060016BB RID: 5819 RVA: 0x0006F0E4 File Offset: 0x0006D2E4
	public void DroppedByPlayer(VRRig grabbedByRig, Vector3 throwVelocity)
	{
		throwVelocity = Vector3.ClampMagnitude(throwVelocity, 20f);
		GorillaClimbable currentClimbable = GTPlayer.Instance.CurrentClimbable;
		if (GTPlayer.Instance.isClimbing && (currentClimbable == grabbedByRig.leftHandHoldsPlayer || currentClimbable == grabbedByRig.rightHandHoldsPlayer))
		{
			GorillaHandClimber currentClimber = GTPlayer.Instance.CurrentClimber;
			GTPlayer.Instance.EndClimbing(currentClimber, false, false);
			GTPlayer.Instance.SetVelocity(throwVelocity);
			this.grabbedRopeIsBody = false;
			this.grabbedRopeIsLeft = false;
			this.grabbedRopeIndex = -1;
			this.grabbedRopeBoneIndex = 0;
			this.grabbedRopeOffset = Vector3.zero;
			this.grabbedRopeIsPhotonView = false;
		}
	}

	// Token: 0x060016BC RID: 5820 RVA: 0x0006F184 File Offset: 0x0006D384
	public bool IsOnGround(float headCheckDistance, float handCheckDistance, out Vector3 groundNormal)
	{
		GTPlayer instance = GTPlayer.Instance;
		Vector3 position = base.transform.position;
		Vector3 vector;
		RaycastHit raycastHit;
		if (this.LocalCheckCollision(position, Vector3.down * headCheckDistance * this.scaleFactor, instance.headCollider.radius * this.scaleFactor, out vector, out raycastHit))
		{
			groundNormal = raycastHit.normal;
			return true;
		}
		Vector3 position2 = this.leftHand.rigTarget.position;
		if (this.LocalCheckCollision(position2, Vector3.down * handCheckDistance * this.scaleFactor, instance.minimumRaycastDistance * this.scaleFactor, out vector, out raycastHit))
		{
			groundNormal = raycastHit.normal;
			return true;
		}
		Vector3 position3 = this.rightHand.rigTarget.position;
		if (this.LocalCheckCollision(position3, Vector3.down * handCheckDistance * this.scaleFactor, instance.minimumRaycastDistance * this.scaleFactor, out vector, out raycastHit))
		{
			groundNormal = raycastHit.normal;
			return true;
		}
		groundNormal = Vector3.up;
		return false;
	}

	// Token: 0x060016BD RID: 5821 RVA: 0x0006F298 File Offset: 0x0006D498
	private bool LocalTestMovementCollision(Vector3 startPosition, Vector3 startVelocity, out Vector3 modifiedVelocity, out Vector3 finalPosition)
	{
		GTPlayer instance = GTPlayer.Instance;
		Vector3 vector = startVelocity * Time.deltaTime;
		finalPosition = startPosition + vector;
		modifiedVelocity = startVelocity;
		Vector3 a;
		RaycastHit raycastHit;
		bool flag = this.LocalCheckCollision(startPosition, vector, instance.headCollider.radius * this.scaleFactor, out a, out raycastHit);
		if (flag)
		{
			finalPosition = a - vector.normalized * 0.01f;
			modifiedVelocity = startVelocity - raycastHit.normal * Vector3.Dot(raycastHit.normal, startVelocity);
		}
		Vector3 position = this.leftHand.rigTarget.position;
		Vector3 a2;
		RaycastHit raycastHit2;
		bool flag2 = this.LocalCheckCollision(position, vector, instance.minimumRaycastDistance * this.scaleFactor, out a2, out raycastHit2);
		if (flag2)
		{
			finalPosition = a2 - (this.leftHand.rigTarget.position - startPosition) - vector.normalized * 0.01f;
			modifiedVelocity = Vector3.zero;
		}
		Vector3 position2 = this.rightHand.rigTarget.position;
		Vector3 a3;
		RaycastHit raycastHit3;
		bool flag3 = this.LocalCheckCollision(position2, vector, instance.minimumRaycastDistance * this.scaleFactor, out a3, out raycastHit3);
		if (flag3)
		{
			finalPosition = a3 - (this.rightHand.rigTarget.position - startPosition) - vector.normalized * 0.01f;
			modifiedVelocity = Vector3.zero;
		}
		return flag || flag2 || flag3;
	}

	// Token: 0x060016BE RID: 5822 RVA: 0x0006F428 File Offset: 0x0006D628
	private bool LocalCheckCollision(Vector3 startPosition, Vector3 movement, float radius, out Vector3 finalPosition, out RaycastHit hit)
	{
		GTPlayer instance = GTPlayer.Instance;
		finalPosition = startPosition + movement;
		RaycastHit raycastHit = default(RaycastHit);
		bool flag = false;
		int num = Physics.SphereCastNonAlloc(startPosition, radius, movement.normalized, this.rayCastNonAllocColliders, movement.magnitude, instance.locomotionEnabledLayers.value);
		if (num > 0)
		{
			raycastHit = this.rayCastNonAllocColliders[0];
			for (int i = 0; i < num; i++)
			{
				if (raycastHit.distance > 0f && (!flag || this.rayCastNonAllocColliders[i].distance < raycastHit.distance))
				{
					flag = true;
					raycastHit = this.rayCastNonAllocColliders[i];
				}
			}
		}
		hit = raycastHit;
		if (flag)
		{
			finalPosition = raycastHit.point + raycastHit.normal * radius;
			return true;
		}
		return false;
	}

	// Token: 0x060016BF RID: 5823 RVA: 0x0006F508 File Offset: 0x0006D708
	public void UpdateFriendshipBracelet()
	{
		bool flag = false;
		if (this.isOfflineVRRig)
		{
			bool flag2 = false;
			VRRig.PartyMemberStatus partyMemberStatus = this.GetPartyMemberStatus();
			if (partyMemberStatus != VRRig.PartyMemberStatus.InLocalParty)
			{
				if (partyMemberStatus == VRRig.PartyMemberStatus.NotInLocalParty)
				{
					flag2 = false;
					this.reliableState.isBraceletLeftHanded = false;
				}
			}
			else
			{
				flag2 = true;
				this.reliableState.isBraceletLeftHanded = (FriendshipGroupDetection.Instance.DidJoinLeftHanded && !this.huntComputer.activeSelf);
			}
			if (this.reliableState.HasBracelet != flag2 || this.reliableState.braceletBeadColors.Count != FriendshipGroupDetection.Instance.myBeadColors.Count)
			{
				this.reliableState.SetIsDirty();
				flag = (this.reliableState.HasBracelet == flag2);
			}
			this.reliableState.braceletBeadColors.Clear();
			if (flag2)
			{
				this.reliableState.braceletBeadColors.AddRange(FriendshipGroupDetection.Instance.myBeadColors);
			}
			this.reliableState.braceletSelfIndex = FriendshipGroupDetection.Instance.MyBraceletSelfIndex;
		}
		if (this.nonCosmeticLeftHandItem != null)
		{
			bool flag3 = this.reliableState.HasBracelet && this.reliableState.isBraceletLeftHanded && !this.IsInvisibleToLocalPlayer;
			this.nonCosmeticLeftHandItem.EnableItem(flag3);
			if (flag3)
			{
				this.friendshipBraceletLeftHand.UpdateBeads(this.reliableState.braceletBeadColors, this.reliableState.braceletSelfIndex);
				if (flag)
				{
					this.friendshipBraceletLeftHand.PlayAppearEffects();
				}
			}
		}
		if (this.nonCosmeticRightHandItem != null)
		{
			bool flag4 = this.reliableState.HasBracelet && !this.reliableState.isBraceletLeftHanded && !this.IsInvisibleToLocalPlayer;
			this.nonCosmeticRightHandItem.EnableItem(flag4);
			if (flag4)
			{
				this.friendshipBraceletRightHand.UpdateBeads(this.reliableState.braceletBeadColors, this.reliableState.braceletSelfIndex);
				if (flag)
				{
					this.friendshipBraceletRightHand.PlayAppearEffects();
				}
			}
		}
	}

	// Token: 0x060016C0 RID: 5824 RVA: 0x0006F6D8 File Offset: 0x0006D8D8
	public void EnableHuntWatch(bool on)
	{
		this.huntComputer.SetActive(on);
		if (this.builderResizeWatch != null)
		{
			MeshRenderer component = this.builderResizeWatch.GetComponent<MeshRenderer>();
			if (component != null)
			{
				component.enabled = !on;
			}
		}
	}

	// Token: 0x060016C1 RID: 5825 RVA: 0x0006F71E File Offset: 0x0006D91E
	public void EnablePaintbrawlCosmetics(bool on)
	{
		this.paintbrawlBalloons.gameObject.SetActive(on);
	}

	// Token: 0x060016C2 RID: 5826 RVA: 0x0006F734 File Offset: 0x0006D934
	public void EnableBuilderResizeWatch(bool on)
	{
		if (this.builderResizeWatch != null && this.builderResizeWatch.activeSelf != on)
		{
			this.builderResizeWatch.SetActive(on);
			if (this.builderArmShelfLeft != null)
			{
				this.builderArmShelfLeft.gameObject.SetActive(on);
			}
			if (this.builderArmShelfRight != null)
			{
				this.builderArmShelfRight.gameObject.SetActive(on);
			}
		}
		if (this.isOfflineVRRig)
		{
			bool flag = this.reliableState.isBuilderWatchEnabled != on;
			this.reliableState.isBuilderWatchEnabled = on;
			if (flag)
			{
				this.reliableState.SetIsDirty();
			}
		}
	}

	// Token: 0x060016C3 RID: 5827 RVA: 0x0006F7D9 File Offset: 0x0006D9D9
	public void EnableGuardianEjectWatch(bool on)
	{
		if (this.guardianEjectWatch != null && this.guardianEjectWatch.activeSelf != on)
		{
			this.guardianEjectWatch.SetActive(on);
		}
	}

	// Token: 0x060016C4 RID: 5828 RVA: 0x0006F804 File Offset: 0x0006DA04
	private void UpdateReplacementVoice()
	{
		if (this.remoteUseReplacementVoice || this.localUseReplacementVoice || GorillaComputer.instance.voiceChatOn != "TRUE")
		{
			this.voiceAudio.mute = true;
			return;
		}
		this.voiceAudio.mute = false;
	}

	// Token: 0x060016C5 RID: 5829 RVA: 0x0006F854 File Offset: 0x0006DA54
	public bool ShouldPlayReplacementVoice()
	{
		return this.netView && !this.netView.IsMine && !(GorillaComputer.instance.voiceChatOn == "OFF") && (this.remoteUseReplacementVoice || this.localUseReplacementVoice || GorillaComputer.instance.voiceChatOn == "FALSE") && this.speakingLoudness > this.replacementVoiceLoudnessThreshold;
	}

	// Token: 0x060016C6 RID: 5830 RVA: 0x0006F8CF File Offset: 0x0006DACF
	public void SetDuplicationZone(RigDuplicationZone duplicationZone)
	{
		this.duplicationZone = duplicationZone;
		this.inDuplicationZone = (duplicationZone != null);
	}

	// Token: 0x060016C7 RID: 5831 RVA: 0x0006F8E5 File Offset: 0x0006DAE5
	public void ClearDuplicationZone(RigDuplicationZone duplicationZone)
	{
		if (this.duplicationZone == duplicationZone)
		{
			this.SetDuplicationZone(null);
			this.renderTransform.localPosition = Vector3.zero;
		}
	}

	// Token: 0x17000280 RID: 640
	// (get) Token: 0x060016C8 RID: 5832 RVA: 0x0006F90C File Offset: 0x0006DB0C
	// (set) Token: 0x060016C9 RID: 5833 RVA: 0x0006F914 File Offset: 0x0006DB14
	bool IUserCosmeticsCallback.PendingUpdate
	{
		get
		{
			return this.pendingCosmeticUpdate;
		}
		set
		{
			this.pendingCosmeticUpdate = value;
		}
	}

	// Token: 0x17000281 RID: 641
	// (get) Token: 0x060016CA RID: 5834 RVA: 0x0006F91D File Offset: 0x0006DB1D
	// (set) Token: 0x060016CB RID: 5835 RVA: 0x0006F925 File Offset: 0x0006DB25
	public bool IsFrozen { get; set; }

	// Token: 0x060016CC RID: 5836 RVA: 0x0006F930 File Offset: 0x0006DB30
	bool IUserCosmeticsCallback.OnGetUserCosmetics(string cosmetics)
	{
		if (cosmetics == this.rawCosmeticString && this.currentCosmeticTries < this.cosmeticRetries)
		{
			this.currentCosmeticTries++;
			return false;
		}
		this.rawCosmeticString = (cosmetics ?? "");
		this.concatStringOfCosmeticsAllowed = this.rawCosmeticString;
		this.InitializedCosmetics = true;
		this.currentCosmeticTries = 0;
		this.CheckForEarlyAccess();
		this.SetCosmeticsActive();
		this.myBodyDockPositions.RefreshTransferrableItems();
		NetworkView networkView = this.netView;
		if (networkView != null)
		{
			networkView.SendRPC("RPC_RequestCosmetics", this.creator, Array.Empty<object>());
		}
		return true;
	}

	// Token: 0x060016CD RID: 5837 RVA: 0x0006F9CC File Offset: 0x0006DBCC
	private short PackCompetitiveData()
	{
		if (!this.turningCompInitialized)
		{
			this.GorillaSnapTurningComp = GorillaTagger.Instance.GetComponent<GorillaSnapTurn>();
			this.turningCompInitialized = true;
		}
		this.fps = Mathf.Min(Mathf.RoundToInt(1f / Time.smoothDeltaTime), 255);
		int num = 0;
		if (this.GorillaSnapTurningComp != null)
		{
			this.turnFactor = this.GorillaSnapTurningComp.turnFactor;
			this.turnType = this.GorillaSnapTurningComp.turnType;
			string a = this.turnType;
			if (!(a == "SNAP"))
			{
				if (a == "SMOOTH")
				{
					num = 2;
				}
			}
			else
			{
				num = 1;
			}
			num *= 10;
			num += this.turnFactor;
		}
		return (short)(this.fps + (num << 8));
	}

	// Token: 0x060016CE RID: 5838 RVA: 0x0006FA8C File Offset: 0x0006DC8C
	private void UnpackCompetitiveData(short packed)
	{
		int num = 255;
		this.fps = ((int)packed & num);
		int num2 = 31;
		int num3 = packed >> 8 & num2;
		this.turnFactor = num3 % 10;
		int num4 = num3 / 10;
		if (num4 == 1)
		{
			this.turnType = "SNAP";
			return;
		}
		if (num4 != 2)
		{
			this.turnType = "NONE";
			return;
		}
		this.turnType = "SMOOTH";
	}

	// Token: 0x060016CF RID: 5839 RVA: 0x0006FAF0 File Offset: 0x0006DCF0
	private void OnKIDSessionUpdated(bool showCustomNames, Permission.ManagedByEnum managedBy)
	{
		bool flag = (showCustomNames || managedBy == Permission.ManagedByEnum.PLAYER) && managedBy != Permission.ManagedByEnum.PROHIBITED;
		GorillaComputer.instance.SetComputerSettingsBySafety(!flag, new GorillaComputer.ComputerState[]
		{
			GorillaComputer.ComputerState.Name
		}, false);
		bool flag2 = PlayerPrefs.GetInt("nameTagsOn", -1) > 0;
		switch (managedBy)
		{
		case Permission.ManagedByEnum.PLAYER:
			flag = GorillaComputer.instance.NametagsEnabled;
			break;
		case Permission.ManagedByEnum.GUARDIAN:
			flag = (showCustomNames && flag2);
			break;
		case Permission.ManagedByEnum.PROHIBITED:
			flag = false;
			break;
		}
		this.UpdateName(flag);
		Debug.Log("[KID] On Session Update - Custom Names Permission changed - Has enabled customNames? [" + flag.ToString() + "]");
	}

	// Token: 0x060016D0 RID: 5840 RVA: 0x0006FB8C File Offset: 0x0006DD8C
	[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
	private static void CacheLocalRig()
	{
		if (VRRig.gLocalRig != null)
		{
			return;
		}
		GameObject gameObject = GameObject.Find("Local Gorilla Player");
		VRRig.gLocalRig = ((gameObject != null) ? gameObject.GetComponentInChildren<VRRig>() : null);
	}

	// Token: 0x17000282 RID: 642
	// (get) Token: 0x060016D1 RID: 5841 RVA: 0x0006FBB7 File Offset: 0x0006DDB7
	public static VRRig LocalRig
	{
		get
		{
			return VRRig.gLocalRig;
		}
	}

	// Token: 0x17000283 RID: 643
	// (get) Token: 0x060016D2 RID: 5842 RVA: 0x0006FBBE File Offset: 0x0006DDBE
	public bool isLocal
	{
		get
		{
			return VRRig.gLocalRig == this;
		}
	}

	// Token: 0x17000284 RID: 644
	// (get) Token: 0x060016D3 RID: 5843 RVA: 0x0000EA77 File Offset: 0x0000CC77
	int IEyeScannable.scannableId
	{
		get
		{
			return base.gameObject.GetInstanceID();
		}
	}

	// Token: 0x17000285 RID: 645
	// (get) Token: 0x060016D4 RID: 5844 RVA: 0x0006FBCB File Offset: 0x0006DDCB
	Vector3 IEyeScannable.Position
	{
		get
		{
			return base.transform.position;
		}
	}

	// Token: 0x17000286 RID: 646
	// (get) Token: 0x060016D5 RID: 5845 RVA: 0x0006FBD8 File Offset: 0x0006DDD8
	Bounds IEyeScannable.Bounds
	{
		get
		{
			return default(Bounds);
		}
	}

	// Token: 0x17000287 RID: 647
	// (get) Token: 0x060016D6 RID: 5846 RVA: 0x0006FBEE File Offset: 0x0006DDEE
	IList<KeyValueStringPair> IEyeScannable.Entries
	{
		get
		{
			return this.buildEntries();
		}
	}

	// Token: 0x060016D7 RID: 5847 RVA: 0x0006FBF8 File Offset: 0x0006DDF8
	private IList<KeyValueStringPair> buildEntries()
	{
		return new KeyValueStringPair[]
		{
			new KeyValueStringPair("Name", this.playerNameVisible),
			new KeyValueStringPair("Color", string.Format("{0}, {1}, {2}", Mathf.RoundToInt(this.playerColor.r * 9f), Mathf.RoundToInt(this.playerColor.g * 9f), Mathf.RoundToInt(this.playerColor.b * 9f)))
		};
	}

	// Token: 0x14000048 RID: 72
	// (add) Token: 0x060016D8 RID: 5848 RVA: 0x0006FC90 File Offset: 0x0006DE90
	// (remove) Token: 0x060016D9 RID: 5849 RVA: 0x0006FCC8 File Offset: 0x0006DEC8
	public event Action OnDataChange;

	// Token: 0x04001839 RID: 6201
	private bool _isListeningFor_OnPostInstantiateAllPrefabs;

	// Token: 0x0400183A RID: 6202
	public static Action newPlayerJoined;

	// Token: 0x0400183B RID: 6203
	public VRMap head;

	// Token: 0x0400183C RID: 6204
	public VRMap rightHand;

	// Token: 0x0400183D RID: 6205
	public VRMap leftHand;

	// Token: 0x0400183E RID: 6206
	public VRMapThumb leftThumb;

	// Token: 0x0400183F RID: 6207
	public VRMapIndex leftIndex;

	// Token: 0x04001840 RID: 6208
	public VRMapMiddle leftMiddle;

	// Token: 0x04001841 RID: 6209
	public VRMapThumb rightThumb;

	// Token: 0x04001842 RID: 6210
	public VRMapIndex rightIndex;

	// Token: 0x04001843 RID: 6211
	public VRMapMiddle rightMiddle;

	// Token: 0x04001844 RID: 6212
	public CrittersLoudNoise leftHandNoise;

	// Token: 0x04001845 RID: 6213
	public CrittersLoudNoise rightHandNoise;

	// Token: 0x04001846 RID: 6214
	public CrittersLoudNoise speakingNoise;

	// Token: 0x04001847 RID: 6215
	private int previousGrabbedRope = -1;

	// Token: 0x04001848 RID: 6216
	private int previousGrabbedRopeBoneIndex;

	// Token: 0x04001849 RID: 6217
	private bool previousGrabbedRopeWasLeft;

	// Token: 0x0400184A RID: 6218
	private bool previousGrabbedRopeWasBody;

	// Token: 0x0400184B RID: 6219
	private GorillaRopeSwing currentRopeSwing;

	// Token: 0x0400184C RID: 6220
	private Transform currentHoldParent;

	// Token: 0x0400184D RID: 6221
	private Transform currentRopeSwingTarget;

	// Token: 0x0400184E RID: 6222
	private float lastRopeGrabTimer;

	// Token: 0x0400184F RID: 6223
	private bool shouldLerpToRope;

	// Token: 0x04001850 RID: 6224
	[NonSerialized]
	public int grabbedRopeIndex = -1;

	// Token: 0x04001851 RID: 6225
	[NonSerialized]
	public int grabbedRopeBoneIndex;

	// Token: 0x04001852 RID: 6226
	[NonSerialized]
	public bool grabbedRopeIsLeft;

	// Token: 0x04001853 RID: 6227
	[NonSerialized]
	public bool grabbedRopeIsBody;

	// Token: 0x04001854 RID: 6228
	[NonSerialized]
	public bool grabbedRopeIsPhotonView;

	// Token: 0x04001855 RID: 6229
	[NonSerialized]
	public Vector3 grabbedRopeOffset = Vector3.zero;

	// Token: 0x04001856 RID: 6230
	private int prevMovingSurfaceID = -1;

	// Token: 0x04001857 RID: 6231
	private bool movingSurfaceWasLeft;

	// Token: 0x04001858 RID: 6232
	private bool movingSurfaceWasBody;

	// Token: 0x04001859 RID: 6233
	private bool movingSurfaceWasMonkeBlock;

	// Token: 0x0400185A RID: 6234
	[NonSerialized]
	public int mountedMovingSurfaceId = -1;

	// Token: 0x0400185B RID: 6235
	[NonSerialized]
	private BuilderPiece mountedMonkeBlock;

	// Token: 0x0400185C RID: 6236
	[NonSerialized]
	private MovingSurface mountedMovingSurface;

	// Token: 0x0400185D RID: 6237
	[NonSerialized]
	public bool mountedMovingSurfaceIsLeft;

	// Token: 0x0400185E RID: 6238
	[NonSerialized]
	public bool mountedMovingSurfaceIsBody;

	// Token: 0x0400185F RID: 6239
	[NonSerialized]
	public bool movingSurfaceIsMonkeBlock;

	// Token: 0x04001860 RID: 6240
	[NonSerialized]
	public Vector3 mountedMonkeBlockOffset = Vector3.zero;

	// Token: 0x04001861 RID: 6241
	private float lastMountedSurfaceTimer;

	// Token: 0x04001862 RID: 6242
	private bool shouldLerpToMovingSurface;

	// Token: 0x04001863 RID: 6243
	[Tooltip("- False in 'Gorilla Player Networked.prefab'.\n- True in 'Local VRRig.prefab/Local Gorilla Player'.\n- False in 'Local VRRig.prefab/Actual Gorilla'")]
	public bool isOfflineVRRig;

	// Token: 0x04001864 RID: 6244
	public GameObject mainCamera;

	// Token: 0x04001865 RID: 6245
	public Transform playerOffsetTransform;

	// Token: 0x04001866 RID: 6246
	public int SDKIndex;

	// Token: 0x04001867 RID: 6247
	public bool isMyPlayer;

	// Token: 0x04001868 RID: 6248
	public AudioSource leftHandPlayer;

	// Token: 0x04001869 RID: 6249
	public AudioSource rightHandPlayer;

	// Token: 0x0400186A RID: 6250
	public AudioSource tagSound;

	// Token: 0x0400186B RID: 6251
	[SerializeField]
	private float ratio;

	// Token: 0x0400186C RID: 6252
	public Transform headConstraint;

	// Token: 0x0400186D RID: 6253
	public Vector3 headBodyOffset = Vector3.zero;

	// Token: 0x0400186E RID: 6254
	public GameObject headMesh;

	// Token: 0x0400186F RID: 6255
	private NetworkVector3 netSyncPos = new NetworkVector3();

	// Token: 0x04001870 RID: 6256
	public Vector3 jobPos;

	// Token: 0x04001871 RID: 6257
	public Quaternion syncRotation;

	// Token: 0x04001872 RID: 6258
	public Quaternion jobRotation;

	// Token: 0x04001873 RID: 6259
	public AudioClip[] clipToPlay;

	// Token: 0x04001874 RID: 6260
	public AudioClip[] handTapSound;

	// Token: 0x04001875 RID: 6261
	public int currentMatIndex;

	// Token: 0x04001876 RID: 6262
	public int setMatIndex;

	// Token: 0x04001877 RID: 6263
	public float lerpValueFingers;

	// Token: 0x04001878 RID: 6264
	public float lerpValueBody;

	// Token: 0x04001879 RID: 6265
	public GameObject backpack;

	// Token: 0x0400187A RID: 6266
	public Transform leftHandTransform;

	// Token: 0x0400187B RID: 6267
	public Transform rightHandTransform;

	// Token: 0x0400187C RID: 6268
	public Transform bodyTransform;

	// Token: 0x0400187D RID: 6269
	public SkinnedMeshRenderer mainSkin;

	// Token: 0x0400187E RID: 6270
	public GorillaSkin defaultSkin;

	// Token: 0x0400187F RID: 6271
	public MeshRenderer faceSkin;

	// Token: 0x04001880 RID: 6272
	public XRaySkeleton skeleton;

	// Token: 0x04001881 RID: 6273
	public GorillaBodyRenderer bodyRenderer;

	// Token: 0x04001882 RID: 6274
	public ZoneEntity zoneEntity;

	// Token: 0x04001883 RID: 6275
	public Material myDefaultSkinMaterialInstance;

	// Token: 0x04001884 RID: 6276
	public Material scoreboardMaterial;

	// Token: 0x04001885 RID: 6277
	public GameObject spectatorSkin;

	// Token: 0x04001886 RID: 6278
	public int handSync;

	// Token: 0x04001887 RID: 6279
	public Material[] materialsToChangeTo;

	// Token: 0x04001888 RID: 6280
	public float red;

	// Token: 0x04001889 RID: 6281
	public float green;

	// Token: 0x0400188A RID: 6282
	public float blue;

	// Token: 0x0400188B RID: 6283
	public TextMeshPro playerText1;

	// Token: 0x0400188C RID: 6284
	public TextMeshPro playerText2;

	// Token: 0x0400188D RID: 6285
	public string playerNameVisible;

	// Token: 0x0400188E RID: 6286
	[Tooltip("- True in 'Gorilla Player Networked.prefab'.\n- True in 'Local VRRig.prefab/Local Gorilla Player'.\n- False in 'Local VRRig.prefab/Actual Gorilla'")]
	public bool showName;

	// Token: 0x0400188F RID: 6287
	public CosmeticItemRegistry cosmeticsObjectRegistry = new CosmeticItemRegistry();

	// Token: 0x04001890 RID: 6288
	[FormerlySerializedAs("cosmetics")]
	public GameObject[] _cosmetics;

	// Token: 0x04001891 RID: 6289
	[FormerlySerializedAs("overrideCosmetics")]
	public GameObject[] _overrideCosmetics;

	// Token: 0x04001892 RID: 6290
	private int taggedById;

	// Token: 0x04001893 RID: 6291
	public string concatStringOfCosmeticsAllowed = "";

	// Token: 0x04001894 RID: 6292
	private bool initializedCosmetics;

	// Token: 0x04001895 RID: 6293
	public CosmeticsController.CosmeticSet cosmeticSet;

	// Token: 0x04001896 RID: 6294
	public CosmeticsController.CosmeticSet tryOnSet;

	// Token: 0x04001897 RID: 6295
	public CosmeticsController.CosmeticSet mergedSet;

	// Token: 0x04001898 RID: 6296
	public CosmeticsController.CosmeticSet prevSet;

	// Token: 0x04001899 RID: 6297
	private int cosmeticRetries = 2;

	// Token: 0x0400189A RID: 6298
	private int currentCosmeticTries;

	// Token: 0x0400189C RID: 6300
	public SizeManager sizeManager;

	// Token: 0x0400189D RID: 6301
	public float pitchScale = 0.3f;

	// Token: 0x0400189E RID: 6302
	public float pitchOffset = 1f;

	// Token: 0x0400189F RID: 6303
	[NonSerialized]
	public bool IsHaunted;

	// Token: 0x040018A0 RID: 6304
	public float HauntedVoicePitch = 0.5f;

	// Token: 0x040018A1 RID: 6305
	public float HauntedHearingVolume = 0.15f;

	// Token: 0x040018A2 RID: 6306
	[NonSerialized]
	public bool UsingHauntedRing;

	// Token: 0x040018A3 RID: 6307
	[NonSerialized]
	public float HauntedRingVoicePitch;

	// Token: 0x040018A4 RID: 6308
	public FriendshipBracelet friendshipBraceletLeftHand;

	// Token: 0x040018A5 RID: 6309
	public NonCosmeticHandItem nonCosmeticLeftHandItem;

	// Token: 0x040018A6 RID: 6310
	public FriendshipBracelet friendshipBraceletRightHand;

	// Token: 0x040018A7 RID: 6311
	public NonCosmeticHandItem nonCosmeticRightHandItem;

	// Token: 0x040018A8 RID: 6312
	public HoverboardVisual hoverboardVisual;

	// Token: 0x040018A9 RID: 6313
	public HoldableHand bodyHolds;

	// Token: 0x040018AA RID: 6314
	public HoldableHand leftHolds;

	// Token: 0x040018AB RID: 6315
	public HoldableHand rightHolds;

	// Token: 0x040018AC RID: 6316
	public GorillaClimbable leftHandHoldsPlayer;

	// Token: 0x040018AD RID: 6317
	public GorillaClimbable rightHandHoldsPlayer;

	// Token: 0x040018AE RID: 6318
	public GameObject nameTagAnchor;

	// Token: 0x040018AF RID: 6319
	public GameObject frozenEffect;

	// Token: 0x040018B0 RID: 6320
	public GameObject iceCubeLeft;

	// Token: 0x040018B1 RID: 6321
	public GameObject iceCubeRight;

	// Token: 0x040018B2 RID: 6322
	public float frozenEffectMaxY;

	// Token: 0x040018B3 RID: 6323
	public float frozenEffectMaxHorizontalScale = 0.8f;

	// Token: 0x040018B4 RID: 6324
	public GameObject FPVEffectsParent;

	// Token: 0x040018B5 RID: 6325
	public List<CosmeticEffectsOnPlayers.CosmeticEffect> TemporaryCosmeticEffects = new List<CosmeticEffectsOnPlayers.CosmeticEffect>();

	// Token: 0x040018B6 RID: 6326
	public VRRigReliableState reliableState;

	// Token: 0x040018B7 RID: 6327
	[SerializeField]
	private Transform MouthPosition;

	// Token: 0x040018BB RID: 6331
	internal RigContainer rigContainer;

	// Token: 0x040018BC RID: 6332
	private Vector3 remoteVelocity;

	// Token: 0x040018BD RID: 6333
	private double remoteLatestTimestamp;

	// Token: 0x040018BE RID: 6334
	private Vector3 remoteCorrectionNeeded;

	// Token: 0x040018BF RID: 6335
	private const float REMOTE_CORRECTION_RATE = 5f;

	// Token: 0x040018C0 RID: 6336
	private const bool USE_NEW_NETCODE = false;

	// Token: 0x040018C1 RID: 6337
	private float stealthTimer;

	// Token: 0x040018C2 RID: 6338
	private GorillaAmbushManager stealthManager;

	// Token: 0x040018C3 RID: 6339
	private LayerChanger layerChanger;

	// Token: 0x040018C4 RID: 6340
	private float frozenEffectMinY;

	// Token: 0x040018C5 RID: 6341
	private float frozenEffectMinHorizontalScale;

	// Token: 0x040018C6 RID: 6342
	private float frozenTimeElapsed;

	// Token: 0x040018C7 RID: 6343
	public TagEffectPack CosmeticEffectPack;

	// Token: 0x040018C8 RID: 6344
	private GorillaSnapTurn GorillaSnapTurningComp;

	// Token: 0x040018C9 RID: 6345
	private bool turningCompInitialized;

	// Token: 0x040018CA RID: 6346
	private string turnType = "NONE";

	// Token: 0x040018CB RID: 6347
	private int turnFactor;

	// Token: 0x040018CC RID: 6348
	private int fps;

	// Token: 0x040018CD RID: 6349
	private VRRig.PartyMemberStatus partyMemberStatus;

	// Token: 0x040018CE RID: 6350
	public static readonly GTBitOps.BitWriteInfo[] WearablePackedStatesBitWriteInfos = new GTBitOps.BitWriteInfo[]
	{
		new GTBitOps.BitWriteInfo(0, 1),
		new GTBitOps.BitWriteInfo(1, 2),
		new GTBitOps.BitWriteInfo(3, 2),
		new GTBitOps.BitWriteInfo(5, 2),
		new GTBitOps.BitWriteInfo(7, 2),
		new GTBitOps.BitWriteInfo(9, 2)
	};

	// Token: 0x040018CF RID: 6351
	public bool inTryOnRoom;

	// Token: 0x040018D0 RID: 6352
	public bool muted;

	// Token: 0x040018D1 RID: 6353
	private float lastScaleFactor = 1f;

	// Token: 0x040018D2 RID: 6354
	private float scaleMultiplier = 1f;

	// Token: 0x040018D3 RID: 6355
	private float nativeScale = 1f;

	// Token: 0x040018D4 RID: 6356
	private float timeSpawned;

	// Token: 0x040018D5 RID: 6357
	public float doNotLerpConstant = 1f;

	// Token: 0x040018D6 RID: 6358
	public string tempString;

	// Token: 0x040018D7 RID: 6359
	private Player tempPlayer;

	// Token: 0x040018D8 RID: 6360
	internal NetPlayer creator;

	// Token: 0x040018D9 RID: 6361
	private float[] speedArray;

	// Token: 0x040018DA RID: 6362
	private double handLerpValues;

	// Token: 0x040018DB RID: 6363
	private bool initialized;

	// Token: 0x040018DC RID: 6364
	[FormerlySerializedAs("battleBalloons")]
	public PaintbrawlBalloons paintbrawlBalloons;

	// Token: 0x040018DD RID: 6365
	private int tempInt;

	// Token: 0x040018DE RID: 6366
	public BodyDockPositions myBodyDockPositions;

	// Token: 0x040018DF RID: 6367
	public ParticleSystem lavaParticleSystem;

	// Token: 0x040018E0 RID: 6368
	public ParticleSystem rockParticleSystem;

	// Token: 0x040018E1 RID: 6369
	public ParticleSystem iceParticleSystem;

	// Token: 0x040018E2 RID: 6370
	public ParticleSystem snowFlakeParticleSystem;

	// Token: 0x040018E3 RID: 6371
	public string tempItemName;

	// Token: 0x040018E4 RID: 6372
	public CosmeticsController.CosmeticItem tempItem;

	// Token: 0x040018E5 RID: 6373
	public string tempItemId;

	// Token: 0x040018E6 RID: 6374
	public int tempItemCost;

	// Token: 0x040018E7 RID: 6375
	public int leftHandHoldableStatus;

	// Token: 0x040018E8 RID: 6376
	public int rightHandHoldableStatus;

	// Token: 0x040018E9 RID: 6377
	[Tooltip("This has to match the drumsAS array in DrumsItem.cs.")]
	[SerializeReference]
	public AudioSource[] musicDrums;

	// Token: 0x040018EA RID: 6378
	private List<TransferrableObject> instrumentSelfOnly = new List<TransferrableObject>();

	// Token: 0x040018EB RID: 6379
	public AudioSource geodeCrackingSound;

	// Token: 0x040018EC RID: 6380
	public float bonkTime;

	// Token: 0x040018ED RID: 6381
	public float bonkCooldown = 2f;

	// Token: 0x040018EE RID: 6382
	private VRRig tempVRRig;

	// Token: 0x040018EF RID: 6383
	public GameObject huntComputer;

	// Token: 0x040018F0 RID: 6384
	public GameObject builderResizeWatch;

	// Token: 0x040018F1 RID: 6385
	public BuilderArmShelf builderArmShelfLeft;

	// Token: 0x040018F2 RID: 6386
	public BuilderArmShelf builderArmShelfRight;

	// Token: 0x040018F3 RID: 6387
	public GameObject guardianEjectWatch;

	// Token: 0x040018F4 RID: 6388
	public ProjectileWeapon projectileWeapon;

	// Token: 0x040018F5 RID: 6389
	private PhotonVoiceView myPhotonVoiceView;

	// Token: 0x040018F6 RID: 6390
	private VRRig senderRig;

	// Token: 0x040018F7 RID: 6391
	private bool isInitialized;

	// Token: 0x040018F8 RID: 6392
	private CircularBuffer<VRRig.VelocityTime> velocityHistoryList = new CircularBuffer<VRRig.VelocityTime>(200);

	// Token: 0x040018F9 RID: 6393
	public int velocityHistoryMaxLength = 200;

	// Token: 0x040018FA RID: 6394
	private Vector3 lastPosition;

	// Token: 0x040018FB RID: 6395
	public const int splashLimitCount = 4;

	// Token: 0x040018FC RID: 6396
	public const float splashLimitCooldown = 0.5f;

	// Token: 0x040018FD RID: 6397
	private float[] splashEffectTimes = new float[4];

	// Token: 0x040018FE RID: 6398
	internal AudioSource voiceAudio;

	// Token: 0x040018FF RID: 6399
	public bool remoteUseReplacementVoice;

	// Token: 0x04001900 RID: 6400
	public bool localUseReplacementVoice;

	// Token: 0x04001901 RID: 6401
	private MicWrapper currentMicWrapper;

	// Token: 0x04001902 RID: 6402
	private IAudioDesc audioDesc;

	// Token: 0x04001903 RID: 6403
	private float speakingLoudness;

	// Token: 0x04001904 RID: 6404
	public bool shouldSendSpeakingLoudness = true;

	// Token: 0x04001905 RID: 6405
	public float replacementVoiceLoudnessThreshold = 0.05f;

	// Token: 0x04001906 RID: 6406
	public int replacementVoiceDetectionDelay = 128;

	// Token: 0x04001907 RID: 6407
	private GorillaMouthFlap myMouthFlap;

	// Token: 0x04001908 RID: 6408
	private GorillaSpeakerLoudness mySpeakerLoudness;

	// Token: 0x04001909 RID: 6409
	public ReplacementVoice myReplacementVoice;

	// Token: 0x0400190A RID: 6410
	private GorillaEyeExpressions myEyeExpressions;

	// Token: 0x0400190B RID: 6411
	[SerializeField]
	internal NetworkView netView;

	// Token: 0x0400190C RID: 6412
	[SerializeField]
	internal VRRigSerializer rigSerializer;

	// Token: 0x0400190D RID: 6413
	public NetPlayer OwningNetPlayer;

	// Token: 0x0400190E RID: 6414
	[SerializeField]
	private FXSystemSettings sharedFXSettings;

	// Token: 0x0400190F RID: 6415
	[NonSerialized]
	public FXSystemSettings fxSettings;

	// Token: 0x04001910 RID: 6416
	[SerializeField]
	private float tapPointDistance = 0.035f;

	// Token: 0x04001911 RID: 6417
	[SerializeField]
	private float handSpeedToVolumeModifier = 0.05f;

	// Token: 0x04001912 RID: 6418
	[SerializeField]
	private HandEffectContext _leftHandEffect;

	// Token: 0x04001913 RID: 6419
	[SerializeField]
	private HandEffectContext _rightHandEffect;

	// Token: 0x04001914 RID: 6420
	private bool _rigBuildFullyInitialized;

	// Token: 0x04001915 RID: 6421
	[SerializeField]
	private Transform renderTransform;

	// Token: 0x04001916 RID: 6422
	private bool playerWasHaunted;

	// Token: 0x04001917 RID: 6423
	private float nonHauntedVolume;

	// Token: 0x04001918 RID: 6424
	[SerializeField]
	private AnimationCurve voicePitchForRelativeScale;

	// Token: 0x04001919 RID: 6425
	private Vector3 LocalTrajectoryOverridePosition;

	// Token: 0x0400191A RID: 6426
	private Vector3 LocalTrajectoryOverrideVelocity;

	// Token: 0x0400191B RID: 6427
	private float LocalTrajectoryOverrideBlend;

	// Token: 0x0400191C RID: 6428
	[SerializeField]
	private float LocalTrajectoryOverrideDuration = 1f;

	// Token: 0x0400191D RID: 6429
	private bool localOverrideIsBody;

	// Token: 0x0400191E RID: 6430
	private bool localOverrideIsLeftHand;

	// Token: 0x0400191F RID: 6431
	private Transform localOverrideGrabbingHand;

	// Token: 0x04001920 RID: 6432
	private float localGrabOverrideBlend;

	// Token: 0x04001921 RID: 6433
	[SerializeField]
	private float LocalGrabOverrideDuration = 0.25f;

	// Token: 0x04001922 RID: 6434
	private float[] voiceSampleBuffer = new float[128];

	// Token: 0x04001923 RID: 6435
	private const int CHECK_LOUDNESS_FREQ_FRAMES = 10;

	// Token: 0x04001924 RID: 6436
	private CallbackContainer<ICallBack> lateUpdateCallbacks = new CallbackContainer<ICallBack>(5);

	// Token: 0x04001925 RID: 6437
	private bool IsInvisibleToLocalPlayer;

	// Token: 0x04001926 RID: 6438
	private const int remoteUseReplacementVoice_BIT = 512;

	// Token: 0x04001927 RID: 6439
	private const int grabbedRope_BIT = 1024;

	// Token: 0x04001928 RID: 6440
	private const int grabbedRopeIsPhotonView_BIT = 2048;

	// Token: 0x04001929 RID: 6441
	private const int isHoldingHoverboard_BIT = 8192;

	// Token: 0x0400192A RID: 6442
	private const int isHoverboardLeftHanded_BIT = 16384;

	// Token: 0x0400192B RID: 6443
	private const int isOnMovingSurface_BIT = 32768;

	// Token: 0x0400192C RID: 6444
	private Vector3 tempVec;

	// Token: 0x0400192D RID: 6445
	private Quaternion tempQuat;

	// Token: 0x0400192E RID: 6446
	public Color playerColor;

	// Token: 0x0400192F RID: 6447
	public bool colorInitialized;

	// Token: 0x04001930 RID: 6448
	private Action<Color> onColorInitialized;

	// Token: 0x04001933 RID: 6451
	private int currentQuestScore;

	// Token: 0x04001934 RID: 6452
	private bool _scoreUpdated;

	// Token: 0x04001935 RID: 6453
	private CallLimiter updateQuestCallLimit = new CallLimiter(1, 0.5f, 0.5f);

	// Token: 0x04001936 RID: 6454
	public const float maxThrowVelocity = 20f;

	// Token: 0x04001937 RID: 6455
	private RaycastHit[] rayCastNonAllocColliders = new RaycastHit[5];

	// Token: 0x04001938 RID: 6456
	private bool inDuplicationZone;

	// Token: 0x04001939 RID: 6457
	private RigDuplicationZone duplicationZone;

	// Token: 0x0400193A RID: 6458
	private bool pendingCosmeticUpdate = true;

	// Token: 0x0400193B RID: 6459
	private string rawCosmeticString = "";

	// Token: 0x0400193D RID: 6461
	public List<HandEffectsOverrideCosmetic> CosmeticHandEffectsOverride_Right = new List<HandEffectsOverrideCosmetic>();

	// Token: 0x0400193E RID: 6462
	public List<HandEffectsOverrideCosmetic> CosmeticHandEffectsOverride_Left = new List<HandEffectsOverrideCosmetic>();

	// Token: 0x0400193F RID: 6463
	private int loudnessCheckFrame;

	// Token: 0x04001940 RID: 6464
	private float frameScale;

	// Token: 0x04001941 RID: 6465
	private const bool SHOW_SCREENS = false;

	// Token: 0x04001942 RID: 6466
	private static VRRig gLocalRig;

	// Token: 0x020003B0 RID: 944
	public enum PartyMemberStatus
	{
		// Token: 0x04001945 RID: 6469
		NeedsUpdate,
		// Token: 0x04001946 RID: 6470
		InLocalParty,
		// Token: 0x04001947 RID: 6471
		NotInLocalParty
	}

	// Token: 0x020003B1 RID: 945
	public enum WearablePackedStateSlots
	{
		// Token: 0x04001949 RID: 6473
		Hat,
		// Token: 0x0400194A RID: 6474
		LeftHand,
		// Token: 0x0400194B RID: 6475
		RightHand,
		// Token: 0x0400194C RID: 6476
		Face,
		// Token: 0x0400194D RID: 6477
		Pants1,
		// Token: 0x0400194E RID: 6478
		Pants2
	}

	// Token: 0x020003B2 RID: 946
	public struct VelocityTime
	{
		// Token: 0x060016DE RID: 5854 RVA: 0x00070016 File Offset: 0x0006E216
		public VelocityTime(Vector3 velocity, double velTime)
		{
			this.vel = velocity;
			this.time = velTime;
		}

		// Token: 0x0400194F RID: 6479
		public Vector3 vel;

		// Token: 0x04001950 RID: 6480
		public double time;
	}
}
