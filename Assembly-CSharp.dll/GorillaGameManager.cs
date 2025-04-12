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

// Token: 0x0200055A RID: 1370
public abstract class GorillaGameManager : MonoBehaviourPunCallbacks, ITickSystemTick, IWrappedSerializable, INetworkStruct
{
	// Token: 0x1400004E RID: 78
	// (add) Token: 0x06002179 RID: 8569 RVA: 0x000F3F7C File Offset: 0x000F217C
	// (remove) Token: 0x0600217A RID: 8570 RVA: 0x000F3FB0 File Offset: 0x000F21B0
	public static event GorillaGameManager.OnTouchDelegate OnTouch;

	// Token: 0x17000370 RID: 880
	// (get) Token: 0x0600217B RID: 8571 RVA: 0x00045C8A File Offset: 0x00043E8A
	public static GorillaGameManager instance
	{
		get
		{
			return GorillaGameModes.GameMode.ActiveGameMode;
		}
	}

	// Token: 0x17000371 RID: 881
	// (get) Token: 0x0600217C RID: 8572 RVA: 0x00045C91 File Offset: 0x00043E91
	// (set) Token: 0x0600217D RID: 8573 RVA: 0x00045C99 File Offset: 0x00043E99
	bool ITickSystemTick.TickRunning { get; set; }

	// Token: 0x0600217E RID: 8574 RVA: 0x0002F75F File Offset: 0x0002D95F
	public virtual void Awake()
	{
	}

	// Token: 0x0600217F RID: 8575 RVA: 0x0002F75F File Offset: 0x0002D95F
	private new void OnEnable()
	{
	}

	// Token: 0x06002180 RID: 8576 RVA: 0x0002F75F File Offset: 0x0002D95F
	private new void OnDisable()
	{
	}

	// Token: 0x06002181 RID: 8577 RVA: 0x000F3FE4 File Offset: 0x000F21E4
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

	// Token: 0x06002182 RID: 8578 RVA: 0x00045CA2 File Offset: 0x00043EA2
	public virtual void InfrequentUpdate()
	{
		GorillaGameModes.GameMode.RefreshPlayers();
		this.currentNetPlayerArray = NetworkSystem.Instance.AllNetPlayers;
	}

	// Token: 0x06002183 RID: 8579 RVA: 0x00045CB9 File Offset: 0x00043EB9
	public virtual string GameModeName()
	{
		return "NONE";
	}

	// Token: 0x06002184 RID: 8580 RVA: 0x0002F75F File Offset: 0x0002D95F
	public virtual void LocalTag(NetPlayer taggedPlayer, NetPlayer taggingPlayer, bool bodyHit, bool leftHand)
	{
	}

	// Token: 0x06002185 RID: 8581 RVA: 0x0002F75F File Offset: 0x0002D95F
	public virtual void ReportTag(NetPlayer taggedPlayer, NetPlayer taggingPlayer)
	{
	}

	// Token: 0x06002186 RID: 8582 RVA: 0x0002F75F File Offset: 0x0002D95F
	public virtual void HitPlayer(NetPlayer player)
	{
	}

	// Token: 0x06002187 RID: 8583 RVA: 0x0002F5F0 File Offset: 0x0002D7F0
	public virtual bool CanAffectPlayer(NetPlayer player, bool thisFrame)
	{
		return false;
	}

	// Token: 0x06002188 RID: 8584 RVA: 0x0002F75F File Offset: 0x0002D95F
	public virtual void HandleHandTap(NetPlayer tappingPlayer, Tappable hitTappable, bool leftHand, Vector3 handVelocity, Vector3 tapSurfaceNormal)
	{
	}

	// Token: 0x06002189 RID: 8585 RVA: 0x00038586 File Offset: 0x00036786
	public virtual bool CanJoinFrienship(NetPlayer player)
	{
		return true;
	}

	// Token: 0x0600218A RID: 8586 RVA: 0x00045CC0 File Offset: 0x00043EC0
	public virtual void HandleRoundComplete()
	{
		PlayerGameEvents.GameModeCompleteRound();
	}

	// Token: 0x0600218B RID: 8587 RVA: 0x0002F75F File Offset: 0x0002D95F
	public virtual void NewVRRig(NetPlayer player, int vrrigPhotonViewID, bool didTutorial)
	{
	}

	// Token: 0x0600218C RID: 8588 RVA: 0x0002F5F0 File Offset: 0x0002D7F0
	public virtual bool LocalCanTag(NetPlayer myPlayer, NetPlayer otherPlayer)
	{
		return false;
	}

	// Token: 0x0600218D RID: 8589 RVA: 0x00045CC7 File Offset: 0x00043EC7
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

	// Token: 0x0600218E RID: 8590 RVA: 0x000F4034 File Offset: 0x000F2234
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

	// Token: 0x0600218F RID: 8591 RVA: 0x00045D06 File Offset: 0x00043F06
	public virtual float[] LocalPlayerSpeed()
	{
		this.playerSpeed[0] = this.slowJumpLimit;
		this.playerSpeed[1] = this.slowJumpMultiplier;
		return this.playerSpeed;
	}

	// Token: 0x06002190 RID: 8592 RVA: 0x000F4078 File Offset: 0x000F2278
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

	// Token: 0x06002191 RID: 8593 RVA: 0x0002F5F0 File Offset: 0x0002D7F0
	public virtual int MyMatIndex(NetPlayer forPlayer)
	{
		return 0;
	}

	// Token: 0x06002192 RID: 8594 RVA: 0x00045D2A File Offset: 0x00043F2A
	public virtual int SpecialHandFX(NetPlayer player, RigContainer rigContainer)
	{
		return -1;
	}

	// Token: 0x06002193 RID: 8595 RVA: 0x00045D2D File Offset: 0x00043F2D
	public virtual bool ValidGameMode()
	{
		return NetworkSystem.Instance.InRoom && NetworkSystem.Instance.GameModeString.Contains(this.GameTypeName());
	}

	// Token: 0x06002194 RID: 8596 RVA: 0x00045D55 File Offset: 0x00043F55
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

	// Token: 0x06002195 RID: 8597 RVA: 0x00045D73 File Offset: 0x00043F73
	public static void ReplicatedClientReady()
	{
		GorillaGameManager.replicatedClientReady = true;
	}

	// Token: 0x06002196 RID: 8598 RVA: 0x00045D7B File Offset: 0x00043F7B
	public static void OnReplicatedClientReady(Action action)
	{
		if (GorillaGameManager.replicatedClientReady)
		{
			action();
			return;
		}
		GorillaGameManager.onReplicatedClientReady = (Action)Delegate.Combine(GorillaGameManager.onReplicatedClientReady, action);
	}

	// Token: 0x17000372 RID: 882
	// (get) Token: 0x06002197 RID: 8599 RVA: 0x00045DA0 File Offset: 0x00043FA0
	internal GameModeSerializer Serializer
	{
		get
		{
			return this.serializer;
		}
	}

	// Token: 0x06002198 RID: 8600 RVA: 0x00045DA8 File Offset: 0x00043FA8
	internal virtual void NetworkLinkSetup(GameModeSerializer netSerializer)
	{
		this.serializer = netSerializer;
	}

	// Token: 0x06002199 RID: 8601 RVA: 0x00045DB1 File Offset: 0x00043FB1
	internal virtual void NetworkLinkDestroyed(GameModeSerializer netSerializer)
	{
		if (this.serializer == netSerializer)
		{
			this.serializer = null;
		}
	}

	// Token: 0x0600219A RID: 8602
	public abstract GameModeType GameType();

	// Token: 0x0600219B RID: 8603 RVA: 0x000F40C8 File Offset: 0x000F22C8
	public string GameTypeName()
	{
		return this.GameType().ToString();
	}

	// Token: 0x0600219C RID: 8604
	public abstract void AddFusionDataBehaviour(NetworkObject behaviour);

	// Token: 0x0600219D RID: 8605
	public abstract void OnSerializeRead(object newData);

	// Token: 0x0600219E RID: 8606
	public abstract object OnSerializeWrite();

	// Token: 0x0600219F RID: 8607
	public abstract void OnSerializeRead(PhotonStream stream, PhotonMessageInfo info);

	// Token: 0x060021A0 RID: 8608
	public abstract void OnSerializeWrite(PhotonStream stream, PhotonMessageInfo info);

	// Token: 0x060021A1 RID: 8609 RVA: 0x0002F75F File Offset: 0x0002D95F
	public virtual void Reset()
	{
	}

	// Token: 0x060021A2 RID: 8610 RVA: 0x000F40EC File Offset: 0x000F22EC
	public virtual void StartPlaying()
	{
		TickSystem<object>.AddTickCallback(this);
		NetworkSystem.Instance.OnPlayerJoined += this.OnPlayerEnteredRoom;
		NetworkSystem.Instance.OnPlayerLeft += this.OnPlayerLeftRoom;
		NetworkSystem.Instance.OnMasterClientSwitchedEvent += this.OnMasterClientSwitched;
		this.currentNetPlayerArray = NetworkSystem.Instance.AllNetPlayers;
	}

	// Token: 0x060021A3 RID: 8611 RVA: 0x000F4154 File Offset: 0x000F2354
	public virtual void StopPlaying()
	{
		TickSystem<object>.RemoveTickCallback(this);
		NetworkSystem.Instance.OnPlayerJoined -= this.OnPlayerEnteredRoom;
		NetworkSystem.Instance.OnPlayerLeft -= this.OnPlayerLeftRoom;
		NetworkSystem.Instance.OnMasterClientSwitchedEvent -= this.OnMasterClientSwitched;
		this.lastCheck = 0f;
	}

	// Token: 0x060021A4 RID: 8612 RVA: 0x0002F75F File Offset: 0x0002D95F
	public new virtual void OnMasterClientSwitched(Player newMaster)
	{
	}

	// Token: 0x060021A5 RID: 8613 RVA: 0x0002F75F File Offset: 0x0002D95F
	public new virtual void OnRoomPropertiesUpdate(Hashtable propertiesThatChanged)
	{
	}

	// Token: 0x060021A6 RID: 8614 RVA: 0x0002F75F File Offset: 0x0002D95F
	public new virtual void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps)
	{
	}

	// Token: 0x060021A7 RID: 8615 RVA: 0x00045DC8 File Offset: 0x00043FC8
	public virtual void OnPlayerLeftRoom(NetPlayer otherPlayer)
	{
		this.currentNetPlayerArray = NetworkSystem.Instance.AllNetPlayers;
		if (this.lastTaggedActorNr.ContainsKey(otherPlayer.ActorNumber))
		{
			this.lastTaggedActorNr.Remove(otherPlayer.ActorNumber);
		}
	}

	// Token: 0x060021A8 RID: 8616 RVA: 0x00045DFF File Offset: 0x00043FFF
	public virtual void OnPlayerEnteredRoom(NetPlayer newPlayer)
	{
		this.currentNetPlayerArray = NetworkSystem.Instance.AllNetPlayers;
	}

	// Token: 0x060021A9 RID: 8617 RVA: 0x0002F75F File Offset: 0x0002D95F
	public virtual void OnMasterClientSwitched(NetPlayer newMaster)
	{
	}

	// Token: 0x060021AA RID: 8618 RVA: 0x000F41B8 File Offset: 0x000F23B8
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

	// Token: 0x060021AB RID: 8619 RVA: 0x000F4210 File Offset: 0x000F2410
	public void AddLastTagged(NetPlayer taggedPlayer, NetPlayer taggingPlayer)
	{
		if (this.lastTaggedActorNr.ContainsKey(taggedPlayer.ActorNumber))
		{
			this.lastTaggedActorNr[taggedPlayer.ActorNumber] = taggingPlayer.ActorNumber;
			return;
		}
		this.lastTaggedActorNr.Add(taggedPlayer.ActorNumber, taggingPlayer.ActorNumber);
	}

	// Token: 0x060021AC RID: 8620 RVA: 0x000F4260 File Offset: 0x000F2460
	public void WriteLastTagged(PhotonStream stream)
	{
		stream.SendNext(this.lastTaggedActorNr.Count);
		foreach (KeyValuePair<int, int> keyValuePair in this.lastTaggedActorNr)
		{
			stream.SendNext(keyValuePair.Key);
			stream.SendNext(keyValuePair.Value);
		}
	}

	// Token: 0x060021AD RID: 8621 RVA: 0x000F42E8 File Offset: 0x000F24E8
	public void ReadLastTagged(PhotonStream stream)
	{
		this.lastTaggedActorNr.Clear();
		int num = Mathf.Min((int)stream.ReceiveNext(), 10);
		for (int i = 0; i < num; i++)
		{
			this.lastTaggedActorNr.Add((int)stream.ReceiveNext(), (int)stream.ReceiveNext());
		}
	}

	// Token: 0x04002536 RID: 9526
	public object obj;

	// Token: 0x04002537 RID: 9527
	public float fastJumpLimit;

	// Token: 0x04002538 RID: 9528
	public float fastJumpMultiplier;

	// Token: 0x04002539 RID: 9529
	public float slowJumpLimit;

	// Token: 0x0400253A RID: 9530
	public float slowJumpMultiplier;

	// Token: 0x0400253B RID: 9531
	public float lastCheck;

	// Token: 0x0400253C RID: 9532
	public float checkCooldown = 3f;

	// Token: 0x0400253D RID: 9533
	public float startingToLookForFriend;

	// Token: 0x0400253E RID: 9534
	public float timeToSpendLookingForFriend = 10f;

	// Token: 0x0400253F RID: 9535
	public bool successfullyFoundFriend;

	// Token: 0x04002540 RID: 9536
	public float tagDistanceThreshold = 4f;

	// Token: 0x04002541 RID: 9537
	public bool testAssault;

	// Token: 0x04002542 RID: 9538
	public bool endGameManually;

	// Token: 0x04002543 RID: 9539
	public NetPlayer currentMasterClient;

	// Token: 0x04002544 RID: 9540
	public VRRig returnRig;

	// Token: 0x04002545 RID: 9541
	private NetPlayer outPlayer;

	// Token: 0x04002546 RID: 9542
	private int outInt;

	// Token: 0x04002547 RID: 9543
	private VRRig tempRig;

	// Token: 0x04002548 RID: 9544
	public NetPlayer[] currentNetPlayerArray;

	// Token: 0x04002549 RID: 9545
	public float[] playerSpeed = new float[2];

	// Token: 0x0400254A RID: 9546
	private RigContainer outContainer;

	// Token: 0x0400254B RID: 9547
	public Dictionary<int, int> lastTaggedActorNr = new Dictionary<int, int>();

	// Token: 0x0400254D RID: 9549
	private static Action onInstanceReady;

	// Token: 0x0400254E RID: 9550
	private static bool replicatedClientReady;

	// Token: 0x0400254F RID: 9551
	private static Action onReplicatedClientReady;

	// Token: 0x04002550 RID: 9552
	private GameModeSerializer serializer;

	// Token: 0x0200055B RID: 1371
	// (Invoke) Token: 0x060021B0 RID: 8624
	public delegate void OnTouchDelegate(NetPlayer taggedPlayer, NetPlayer taggingPlayer);
}
