using System;
using System.Collections.Generic;
using ExitGames.Client.Photon;
using Fusion;
using GorillaGameModes;
using GorillaLocomotion;
using GorillaNetworking;
using GorillaTag;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

// Token: 0x02000567 RID: 1383
public abstract class GorillaGameManager : MonoBehaviourPunCallbacks, ITickSystemTick, IWrappedSerializable, INetworkStruct
{
	// Token: 0x1400004F RID: 79
	// (add) Token: 0x060021CF RID: 8655 RVA: 0x000F6CF8 File Offset: 0x000F4EF8
	// (remove) Token: 0x060021D0 RID: 8656 RVA: 0x000F6D2C File Offset: 0x000F4F2C
	public static event GorillaGameManager.OnTouchDelegate OnTouch;

	// Token: 0x17000377 RID: 887
	// (get) Token: 0x060021D1 RID: 8657 RVA: 0x0004702F File Offset: 0x0004522F
	public static GorillaGameManager instance
	{
		get
		{
			return GorillaGameModes.GameMode.ActiveGameMode;
		}
	}

	// Token: 0x17000378 RID: 888
	// (get) Token: 0x060021D2 RID: 8658 RVA: 0x00047036 File Offset: 0x00045236
	// (set) Token: 0x060021D3 RID: 8659 RVA: 0x0004703E File Offset: 0x0004523E
	bool ITickSystemTick.TickRunning { get; set; }

	// Token: 0x060021D4 RID: 8660 RVA: 0x00030607 File Offset: 0x0002E807
	public virtual void Awake()
	{
	}

	// Token: 0x060021D5 RID: 8661 RVA: 0x00030607 File Offset: 0x0002E807
	private new void OnEnable()
	{
	}

	// Token: 0x060021D6 RID: 8662 RVA: 0x00030607 File Offset: 0x0002E807
	private new void OnDisable()
	{
	}

	// Token: 0x060021D7 RID: 8663 RVA: 0x000F6D60 File Offset: 0x000F4F60
	public virtual void Tick()
	{
		if (this.lastCheck + this.checkCooldown < Time.time)
		{
			this.lastCheck = Time.time;
			if (NetworkSystem.Instance.IsMasterClient && !this.ValidGameMode())
			{
				GorillaGameModes.GameMode.ChangeGameFromProperty();
				return;
			}
			this.InfrequentUpdate();
		}
	}

	// Token: 0x060021D8 RID: 8664 RVA: 0x00047047 File Offset: 0x00045247
	public virtual void InfrequentUpdate()
	{
		GorillaGameModes.GameMode.RefreshPlayers();
		this.currentNetPlayerArray = NetworkSystem.Instance.AllNetPlayers;
	}

	// Token: 0x060021D9 RID: 8665 RVA: 0x0004705E File Offset: 0x0004525E
	public virtual string GameModeName()
	{
		return "NONE";
	}

	// Token: 0x060021DA RID: 8666 RVA: 0x00030607 File Offset: 0x0002E807
	public virtual void LocalTag(NetPlayer taggedPlayer, NetPlayer taggingPlayer, bool bodyHit, bool leftHand)
	{
	}

	// Token: 0x060021DB RID: 8667 RVA: 0x00030607 File Offset: 0x0002E807
	public virtual void ReportTag(NetPlayer taggedPlayer, NetPlayer taggingPlayer)
	{
	}

	// Token: 0x060021DC RID: 8668 RVA: 0x00030607 File Offset: 0x0002E807
	public virtual void HitPlayer(NetPlayer player)
	{
	}

	// Token: 0x060021DD RID: 8669 RVA: 0x00030498 File Offset: 0x0002E698
	public virtual bool CanAffectPlayer(NetPlayer player, bool thisFrame)
	{
		return false;
	}

	// Token: 0x060021DE RID: 8670 RVA: 0x00030607 File Offset: 0x0002E807
	public virtual void HandleHandTap(NetPlayer tappingPlayer, Tappable hitTappable, bool leftHand, Vector3 handVelocity, Vector3 tapSurfaceNormal)
	{
	}

	// Token: 0x060021DF RID: 8671 RVA: 0x00039846 File Offset: 0x00037A46
	public virtual bool CanJoinFrienship(NetPlayer player)
	{
		return true;
	}

	// Token: 0x060021E0 RID: 8672 RVA: 0x00047065 File Offset: 0x00045265
	public virtual void HandleRoundComplete()
	{
		PlayerGameEvents.GameModeCompleteRound();
	}

	// Token: 0x060021E1 RID: 8673 RVA: 0x00030607 File Offset: 0x0002E807
	public virtual void NewVRRig(NetPlayer player, int vrrigPhotonViewID, bool didTutorial)
	{
	}

	// Token: 0x060021E2 RID: 8674 RVA: 0x00030498 File Offset: 0x0002E698
	public virtual bool LocalCanTag(NetPlayer myPlayer, NetPlayer otherPlayer)
	{
		return false;
	}

	// Token: 0x060021E3 RID: 8675 RVA: 0x0004706C File Offset: 0x0004526C
	public virtual VRRig FindPlayerVRRig(NetPlayer player)
	{
		this.returnRig = null;
		this.outContainer = null;
		if (player == null)
		{
			return null;
		}
		if (VRRigCache.Instance.TryGetVrrig(player, out this.outContainer))
		{
			this.returnRig = this.outContainer.Rig;
		}
		return this.returnRig;
	}

	// Token: 0x060021E4 RID: 8676 RVA: 0x000F6DB0 File Offset: 0x000F4FB0
	public static VRRig StaticFindRigForPlayer(NetPlayer player)
	{
		VRRig result = null;
		RigContainer rigContainer;
		if (GorillaGameManager.instance != null)
		{
			result = GorillaGameManager.instance.FindPlayerVRRig(player);
		}
		else if (VRRigCache.Instance.TryGetVrrig(player, out rigContainer))
		{
			result = rigContainer.Rig;
		}
		return result;
	}

	// Token: 0x060021E5 RID: 8677 RVA: 0x000470AB File Offset: 0x000452AB
	public virtual float[] LocalPlayerSpeed()
	{
		this.playerSpeed[0] = this.slowJumpLimit;
		this.playerSpeed[1] = this.slowJumpMultiplier;
		return this.playerSpeed;
	}

	// Token: 0x060021E6 RID: 8678 RVA: 0x000F6DF4 File Offset: 0x000F4FF4
	public virtual void UpdatePlayerAppearance(VRRig rig)
	{
		ScienceExperimentManager instance = ScienceExperimentManager.instance;
		int materialIndex;
		if (instance != null && instance.GetMaterialIfPlayerInGame(rig.creator.ActorNumber, out materialIndex))
		{
			rig.ChangeMaterialLocal(materialIndex);
			return;
		}
		int materialIndex2 = this.MyMatIndex(rig.creator);
		rig.ChangeMaterialLocal(materialIndex2);
	}

	// Token: 0x060021E7 RID: 8679 RVA: 0x00030498 File Offset: 0x0002E698
	public virtual int MyMatIndex(NetPlayer forPlayer)
	{
		return 0;
	}

	// Token: 0x060021E8 RID: 8680 RVA: 0x000470CF File Offset: 0x000452CF
	public virtual int SpecialHandFX(NetPlayer player, RigContainer rigContainer)
	{
		return -1;
	}

	// Token: 0x060021E9 RID: 8681 RVA: 0x000470D2 File Offset: 0x000452D2
	public virtual bool ValidGameMode()
	{
		return NetworkSystem.Instance.InRoom && NetworkSystem.Instance.GameModeString.Contains(this.GameTypeName());
	}

	// Token: 0x060021EA RID: 8682 RVA: 0x000470FA File Offset: 0x000452FA
	public static void OnInstanceReady(Action action)
	{
		GorillaParent.OnReplicatedClientReady(delegate
		{
			if (GorillaGameManager.instance)
			{
				action();
				return;
			}
			GorillaGameManager.onInstanceReady = (Action)Delegate.Combine(GorillaGameManager.onInstanceReady, action);
		});
	}

	// Token: 0x060021EB RID: 8683 RVA: 0x00047118 File Offset: 0x00045318
	public static void ReplicatedClientReady()
	{
		GorillaGameManager.replicatedClientReady = true;
	}

	// Token: 0x060021EC RID: 8684 RVA: 0x00047120 File Offset: 0x00045320
	public static void OnReplicatedClientReady(Action action)
	{
		if (GorillaGameManager.replicatedClientReady)
		{
			action();
			return;
		}
		GorillaGameManager.onReplicatedClientReady = (Action)Delegate.Combine(GorillaGameManager.onReplicatedClientReady, action);
	}

	// Token: 0x17000379 RID: 889
	// (get) Token: 0x060021ED RID: 8685 RVA: 0x00047145 File Offset: 0x00045345
	internal GameModeSerializer Serializer
	{
		get
		{
			return this.serializer;
		}
	}

	// Token: 0x060021EE RID: 8686 RVA: 0x0004714D File Offset: 0x0004534D
	internal virtual void NetworkLinkSetup(GameModeSerializer netSerializer)
	{
		this.serializer = netSerializer;
	}

	// Token: 0x060021EF RID: 8687 RVA: 0x00047156 File Offset: 0x00045356
	internal virtual void NetworkLinkDestroyed(GameModeSerializer netSerializer)
	{
		if (this.serializer == netSerializer)
		{
			this.serializer = null;
		}
	}

	// Token: 0x060021F0 RID: 8688
	public abstract GameModeType GameType();

	// Token: 0x060021F1 RID: 8689 RVA: 0x000F6E44 File Offset: 0x000F5044
	public string GameTypeName()
	{
		return this.GameType().ToString();
	}

	// Token: 0x060021F2 RID: 8690
	public abstract void AddFusionDataBehaviour(NetworkObject behaviour);

	// Token: 0x060021F3 RID: 8691
	public abstract void OnSerializeRead(object newData);

	// Token: 0x060021F4 RID: 8692
	public abstract object OnSerializeWrite();

	// Token: 0x060021F5 RID: 8693
	public abstract void OnSerializeRead(PhotonStream stream, PhotonMessageInfo info);

	// Token: 0x060021F6 RID: 8694
	public abstract void OnSerializeWrite(PhotonStream stream, PhotonMessageInfo info);

	// Token: 0x060021F7 RID: 8695 RVA: 0x00030607 File Offset: 0x0002E807
	public virtual void Reset()
	{
	}

	// Token: 0x060021F8 RID: 8696 RVA: 0x000F6E68 File Offset: 0x000F5068
	public virtual void StartPlaying()
	{
		TickSystem<object>.AddTickCallback(this);
		NetworkSystem.Instance.OnPlayerJoined += this.OnPlayerEnteredRoom;
		NetworkSystem.Instance.OnPlayerLeft += this.OnPlayerLeftRoom;
		NetworkSystem.Instance.OnMasterClientSwitchedEvent += this.OnMasterClientSwitched;
		this.currentNetPlayerArray = NetworkSystem.Instance.AllNetPlayers;
	}

	// Token: 0x060021F9 RID: 8697 RVA: 0x000F6ED0 File Offset: 0x000F50D0
	public virtual void StopPlaying()
	{
		TickSystem<object>.RemoveTickCallback(this);
		NetworkSystem.Instance.OnPlayerJoined -= this.OnPlayerEnteredRoom;
		NetworkSystem.Instance.OnPlayerLeft -= this.OnPlayerLeftRoom;
		NetworkSystem.Instance.OnMasterClientSwitchedEvent -= this.OnMasterClientSwitched;
		this.lastCheck = 0f;
	}

	// Token: 0x060021FA RID: 8698 RVA: 0x00030607 File Offset: 0x0002E807
	public new virtual void OnMasterClientSwitched(Player newMaster)
	{
	}

	// Token: 0x060021FB RID: 8699 RVA: 0x00030607 File Offset: 0x0002E807
	public new virtual void OnRoomPropertiesUpdate(Hashtable propertiesThatChanged)
	{
	}

	// Token: 0x060021FC RID: 8700 RVA: 0x00030607 File Offset: 0x0002E807
	public new virtual void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps)
	{
	}

	// Token: 0x060021FD RID: 8701 RVA: 0x0004716D File Offset: 0x0004536D
	public virtual void OnPlayerLeftRoom(NetPlayer otherPlayer)
	{
		this.currentNetPlayerArray = NetworkSystem.Instance.AllNetPlayers;
		if (this.lastTaggedActorNr.ContainsKey(otherPlayer.ActorNumber))
		{
			this.lastTaggedActorNr.Remove(otherPlayer.ActorNumber);
		}
	}

	// Token: 0x060021FE RID: 8702 RVA: 0x000471A4 File Offset: 0x000453A4
	public virtual void OnPlayerEnteredRoom(NetPlayer newPlayer)
	{
		this.currentNetPlayerArray = NetworkSystem.Instance.AllNetPlayers;
	}

	// Token: 0x060021FF RID: 8703 RVA: 0x00030607 File Offset: 0x0002E807
	public virtual void OnMasterClientSwitched(NetPlayer newMaster)
	{
	}

	// Token: 0x06002200 RID: 8704 RVA: 0x000F6F34 File Offset: 0x000F5134
	internal static void ForceStopGame_DisconnectAndDestroy()
	{
		Application.Quit();
		NetworkSystem instance = NetworkSystem.Instance;
		if (instance != null)
		{
			instance.ReturnToSinglePlayer();
		}
		UnityEngine.Object.DestroyImmediate(PhotonNetworkController.Instance);
		UnityEngine.Object.DestroyImmediate(GTPlayer.Instance);
		GameObject[] array = UnityEngine.Object.FindObjectsOfType<GameObject>();
		for (int i = 0; i < array.Length; i++)
		{
			UnityEngine.Object.Destroy(array[i]);
		}
	}

	// Token: 0x06002201 RID: 8705 RVA: 0x000F6F8C File Offset: 0x000F518C
	public void AddLastTagged(NetPlayer taggedPlayer, NetPlayer taggingPlayer)
	{
		if (this.lastTaggedActorNr.ContainsKey(taggedPlayer.ActorNumber))
		{
			this.lastTaggedActorNr[taggedPlayer.ActorNumber] = taggingPlayer.ActorNumber;
			return;
		}
		this.lastTaggedActorNr.Add(taggedPlayer.ActorNumber, taggingPlayer.ActorNumber);
	}

	// Token: 0x06002202 RID: 8706 RVA: 0x000F6FDC File Offset: 0x000F51DC
	public void WriteLastTagged(PhotonStream stream)
	{
		stream.SendNext(this.lastTaggedActorNr.Count);
		foreach (KeyValuePair<int, int> keyValuePair in this.lastTaggedActorNr)
		{
			stream.SendNext(keyValuePair.Key);
			stream.SendNext(keyValuePair.Value);
		}
	}

	// Token: 0x06002203 RID: 8707 RVA: 0x000F7064 File Offset: 0x000F5264
	public void ReadLastTagged(PhotonStream stream)
	{
		this.lastTaggedActorNr.Clear();
		int num = Mathf.Min((int)stream.ReceiveNext(), 10);
		for (int i = 0; i < num; i++)
		{
			this.lastTaggedActorNr.Add((int)stream.ReceiveNext(), (int)stream.ReceiveNext());
		}
	}

	// Token: 0x04002588 RID: 9608
	public object obj;

	// Token: 0x04002589 RID: 9609
	public float fastJumpLimit;

	// Token: 0x0400258A RID: 9610
	public float fastJumpMultiplier;

	// Token: 0x0400258B RID: 9611
	public float slowJumpLimit;

	// Token: 0x0400258C RID: 9612
	public float slowJumpMultiplier;

	// Token: 0x0400258D RID: 9613
	public float lastCheck;

	// Token: 0x0400258E RID: 9614
	public float checkCooldown = 3f;

	// Token: 0x0400258F RID: 9615
	public float startingToLookForFriend;

	// Token: 0x04002590 RID: 9616
	public float timeToSpendLookingForFriend = 10f;

	// Token: 0x04002591 RID: 9617
	public bool successfullyFoundFriend;

	// Token: 0x04002592 RID: 9618
	public float tagDistanceThreshold = 4f;

	// Token: 0x04002593 RID: 9619
	public bool testAssault;

	// Token: 0x04002594 RID: 9620
	public bool endGameManually;

	// Token: 0x04002595 RID: 9621
	public NetPlayer currentMasterClient;

	// Token: 0x04002596 RID: 9622
	public VRRig returnRig;

	// Token: 0x04002597 RID: 9623
	private NetPlayer outPlayer;

	// Token: 0x04002598 RID: 9624
	private int outInt;

	// Token: 0x04002599 RID: 9625
	private VRRig tempRig;

	// Token: 0x0400259A RID: 9626
	public NetPlayer[] currentNetPlayerArray;

	// Token: 0x0400259B RID: 9627
	public float[] playerSpeed = new float[2];

	// Token: 0x0400259C RID: 9628
	private RigContainer outContainer;

	// Token: 0x0400259D RID: 9629
	public Dictionary<int, int> lastTaggedActorNr = new Dictionary<int, int>();

	// Token: 0x0400259F RID: 9631
	private static Action onInstanceReady;

	// Token: 0x040025A0 RID: 9632
	private static bool replicatedClientReady;

	// Token: 0x040025A1 RID: 9633
	private static Action onReplicatedClientReady;

	// Token: 0x040025A2 RID: 9634
	private GameModeSerializer serializer;

	// Token: 0x02000568 RID: 1384
	// (Invoke) Token: 0x06002206 RID: 8710
	public delegate void OnTouchDelegate(NetPlayer taggedPlayer, NetPlayer taggingPlayer);
}
