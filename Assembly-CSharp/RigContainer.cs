using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using GorillaNetworking;
using Newtonsoft.Json;
using Photon.Voice.PUN;
using PlayFab;
using PlayFab.CloudScriptModels;
using UnityEngine;

// Token: 0x0200054D RID: 1357
[RequireComponent(typeof(VRRig), typeof(VRRigReliableState))]
public class RigContainer : MonoBehaviour
{
	// Token: 0x17000354 RID: 852
	// (get) Token: 0x060020F0 RID: 8432 RVA: 0x000466CC File Offset: 0x000448CC
	// (set) Token: 0x060020F1 RID: 8433 RVA: 0x000466D4 File Offset: 0x000448D4
	public bool Initialized { get; private set; }

	// Token: 0x17000355 RID: 853
	// (get) Token: 0x060020F2 RID: 8434 RVA: 0x000466DD File Offset: 0x000448DD
	public VRRig Rig
	{
		get
		{
			return this.vrrig;
		}
	}

	// Token: 0x17000356 RID: 854
	// (get) Token: 0x060020F3 RID: 8435 RVA: 0x000466E5 File Offset: 0x000448E5
	public VRRigReliableState ReliableState
	{
		get
		{
			return this.reliableState;
		}
	}

	// Token: 0x17000357 RID: 855
	// (get) Token: 0x060020F4 RID: 8436 RVA: 0x000466ED File Offset: 0x000448ED
	public Transform SpeakerHead
	{
		get
		{
			return this.speakerHead;
		}
	}

	// Token: 0x17000358 RID: 856
	// (get) Token: 0x060020F5 RID: 8437 RVA: 0x000466F5 File Offset: 0x000448F5
	public AudioSource ReplacementVoiceSource
	{
		get
		{
			return this.replacementVoiceSource;
		}
	}

	// Token: 0x17000359 RID: 857
	// (get) Token: 0x060020F6 RID: 8438 RVA: 0x000466FD File Offset: 0x000448FD
	// (set) Token: 0x060020F7 RID: 8439 RVA: 0x00046705 File Offset: 0x00044905
	public PhotonVoiceView Voice
	{
		get
		{
			return this.voiceView;
		}
		set
		{
			if (value == this.voiceView)
			{
				return;
			}
			if (this.voiceView != null)
			{
				this.voiceView.SpeakerInUse.enabled = false;
			}
			this.voiceView = value;
			this.RefreshVoiceChat();
		}
	}

	// Token: 0x1700035A RID: 858
	// (get) Token: 0x060020F8 RID: 8440 RVA: 0x00046742 File Offset: 0x00044942
	public NetworkView netView
	{
		get
		{
			return this.vrrig.netView;
		}
	}

	// Token: 0x1700035B RID: 859
	// (get) Token: 0x060020F9 RID: 8441 RVA: 0x0004674F File Offset: 0x0004494F
	public int CachedNetViewID
	{
		get
		{
			return this.m_cachedNetViewID;
		}
	}

	// Token: 0x1700035C RID: 860
	// (get) Token: 0x060020FA RID: 8442 RVA: 0x00046757 File Offset: 0x00044957
	// (set) Token: 0x060020FB RID: 8443 RVA: 0x00046762 File Offset: 0x00044962
	public bool Muted
	{
		get
		{
			return !this.enableVoice;
		}
		set
		{
			this.enableVoice = !value;
			this.RefreshVoiceChat();
		}
	}

	// Token: 0x1700035D RID: 861
	// (get) Token: 0x060020FC RID: 8444 RVA: 0x00046774 File Offset: 0x00044974
	// (set) Token: 0x060020FD RID: 8445 RVA: 0x000F4530 File Offset: 0x000F2730
	public NetPlayer Creator
	{
		get
		{
			return this.vrrig.creator;
		}
		set
		{
			if (this.vrrig.isOfflineVRRig || (this.vrrig.creator != null && this.vrrig.creator.InRoom))
			{
				return;
			}
			this.vrrig.creator = value;
			this.vrrig.UpdateName();
		}
	}

	// Token: 0x1700035E RID: 862
	// (get) Token: 0x060020FE RID: 8446 RVA: 0x00046781 File Offset: 0x00044981
	// (set) Token: 0x060020FF RID: 8447 RVA: 0x00046789 File Offset: 0x00044989
	public bool ForceMute
	{
		get
		{
			return this.forceMute;
		}
		set
		{
			this.forceMute = value;
			this.RefreshVoiceChat();
		}
	}

	// Token: 0x1700035F RID: 863
	// (get) Token: 0x06002100 RID: 8448 RVA: 0x00046798 File Offset: 0x00044998
	public SphereCollider HeadCollider
	{
		get
		{
			return this.headCollider;
		}
	}

	// Token: 0x17000360 RID: 864
	// (get) Token: 0x06002101 RID: 8449 RVA: 0x000467A0 File Offset: 0x000449A0
	public CapsuleCollider BodyCollider
	{
		get
		{
			return this.bodyCollider;
		}
	}

	// Token: 0x17000361 RID: 865
	// (get) Token: 0x06002102 RID: 8450 RVA: 0x000467A8 File Offset: 0x000449A8
	public VRRigEvents RigEvents
	{
		get
		{
			return this.rigEvents;
		}
	}

	// Token: 0x06002103 RID: 8451 RVA: 0x000467B0 File Offset: 0x000449B0
	public bool GetIsPlayerAutoMuted()
	{
		return this.bPlayerAutoMuted;
	}

	// Token: 0x06002104 RID: 8452 RVA: 0x000F4584 File Offset: 0x000F2784
	public void UpdateAutomuteLevel(string autoMuteLevel)
	{
		if (autoMuteLevel.Equals("LOW", StringComparison.OrdinalIgnoreCase))
		{
			this.playerChatQuality = 1;
		}
		else if (autoMuteLevel.Equals("HIGH", StringComparison.OrdinalIgnoreCase))
		{
			this.playerChatQuality = 0;
		}
		else if (autoMuteLevel.Equals("ERROR", StringComparison.OrdinalIgnoreCase))
		{
			this.playerChatQuality = 2;
		}
		else
		{
			this.playerChatQuality = 2;
		}
		this.RefreshVoiceChat();
	}

	// Token: 0x06002105 RID: 8453 RVA: 0x000F45E4 File Offset: 0x000F27E4
	private void Start()
	{
		if (this.Rig.isOfflineVRRig)
		{
			this.vrrig.creator = NetworkSystem.Instance.LocalPlayer;
			RoomSystem.JoinedRoomEvent = (Action)Delegate.Combine(RoomSystem.JoinedRoomEvent, new Action(this.OnMultiPlayerStarted));
			RoomSystem.LeftRoomEvent = (Action)Delegate.Combine(RoomSystem.LeftRoomEvent, new Action(this.OnReturnedToSinglePlayer));
		}
		this.Rig.rigContainer = this;
	}

	// Token: 0x06002106 RID: 8454 RVA: 0x000467B8 File Offset: 0x000449B8
	private void OnMultiPlayerStarted()
	{
		if (this.Rig.isOfflineVRRig)
		{
			this.vrrig.creator = NetworkSystem.Instance.GetLocalPlayer();
		}
	}

	// Token: 0x06002107 RID: 8455 RVA: 0x000467DC File Offset: 0x000449DC
	private void OnReturnedToSinglePlayer()
	{
		if (this.Rig.isOfflineVRRig)
		{
			RigContainer.CancelAutomuteRequest();
		}
	}

	// Token: 0x06002108 RID: 8456 RVA: 0x000F4660 File Offset: 0x000F2860
	private void OnDisable()
	{
		this.Initialized = false;
		this.enableVoice = true;
		this.voiceView = null;
		base.gameObject.transform.localPosition = Vector3.zero;
		base.gameObject.transform.localRotation = Quaternion.identity;
		this.vrrig.syncPos = base.gameObject.transform.position;
		this.vrrig.syncRotation = base.gameObject.transform.rotation;
		this.forceMute = false;
	}

	// Token: 0x06002109 RID: 8457 RVA: 0x000467F0 File Offset: 0x000449F0
	internal void InitializeNetwork(NetworkView netView, PhotonVoiceView voiceView, VRRigSerializer vrRigSerializer)
	{
		if (!netView || !voiceView)
		{
			return;
		}
		this.InitializeNetwork_Shared(netView, vrRigSerializer);
		this.Voice = voiceView;
		this.vrrig.voiceAudio = voiceView.SpeakerInUse.GetComponent<AudioSource>();
	}

	// Token: 0x0600210A RID: 8458 RVA: 0x000F46EC File Offset: 0x000F28EC
	private void InitializeNetwork_Shared(NetworkView netView, VRRigSerializer vrRigSerializer)
	{
		if (this.vrrig.netView)
		{
			GorillaNot.instance.SendReport("inappropriate tag data being sent creating multiple vrrigs", this.Creator.UserId, this.Creator.NickName);
			if (this.vrrig.netView.IsMine)
			{
				NetworkSystem.Instance.NetDestroy(this.vrrig.gameObject);
			}
			else
			{
				this.vrrig.netView.gameObject.SetActive(false);
			}
		}
		this.vrrig.netView = netView;
		this.vrrig.rigSerializer = vrRigSerializer;
		this.vrrig.OwningNetPlayer = NetworkSystem.Instance.GetPlayer(NetworkSystem.Instance.GetOwningPlayerID(vrRigSerializer.gameObject));
		this.m_cachedNetViewID = netView.ViewID;
		if (!this.Initialized)
		{
			this.vrrig.NetInitialize();
			if (GorillaGameManager.instance != null && NetworkSystem.Instance.IsMasterClient)
			{
				int owningPlayerID = NetworkSystem.Instance.GetOwningPlayerID(vrRigSerializer.gameObject);
				bool playerTutorialCompletion = NetworkSystem.Instance.GetPlayerTutorialCompletion(owningPlayerID);
				GorillaGameManager.instance.NewVRRig(netView.Owner, netView.ViewID, playerTutorialCompletion);
			}
			if (!this.vrrig.OwningNetPlayer.IsLocal)
			{
				netView.SendRPC("RPC_RequestQuestScore", netView.Owner, Array.Empty<object>());
			}
			if (this.vrrig.InitializedCosmetics)
			{
				netView.SendRPC("RPC_RequestCosmetics", netView.Owner, Array.Empty<object>());
			}
		}
		this.Initialized = true;
		if (!this.vrrig.isOfflineVRRig)
		{
			base.StartCoroutine(RigContainer.QueueAutomute(this.Creator));
		}
	}

	// Token: 0x0600210B RID: 8459 RVA: 0x00046828 File Offset: 0x00044A28
	private static IEnumerator QueueAutomute(NetPlayer player)
	{
		RigContainer.playersToCheckAutomute.Add(player);
		if (!RigContainer.automuteQueued)
		{
			RigContainer.automuteQueued = true;
			yield return new WaitForSecondsRealtime(1f);
			while (RigContainer.waitingForAutomuteCallback)
			{
				yield return null;
			}
			RigContainer.automuteQueued = false;
			RigContainer.RequestAutomuteSettings();
		}
		yield break;
	}

	// Token: 0x0600210C RID: 8460 RVA: 0x000F4890 File Offset: 0x000F2A90
	private static void RequestAutomuteSettings()
	{
		if (RigContainer.playersToCheckAutomute.Count == 0)
		{
			return;
		}
		RigContainer.waitingForAutomuteCallback = true;
		RigContainer.playersToCheckAutomute.RemoveAll((NetPlayer player) => player == null);
		RigContainer.requestedAutomutePlayers = new List<NetPlayer>(RigContainer.playersToCheckAutomute);
		RigContainer.playersToCheckAutomute.Clear();
		string[] value = (from x in RigContainer.requestedAutomutePlayers
		select x.UserId).ToArray<string>();
		foreach (NetPlayer netPlayer in RigContainer.requestedAutomutePlayers)
		{
		}
		ExecuteFunctionRequest executeFunctionRequest = new ExecuteFunctionRequest();
		executeFunctionRequest.Entity = new EntityKey
		{
			Id = PlayFabSettings.staticPlayer.EntityId,
			Type = PlayFabSettings.staticPlayer.EntityType
		};
		executeFunctionRequest.FunctionName = "ShouldUserAutomutePlayer";
		executeFunctionRequest.FunctionParameter = string.Join(",", value);
		PlayFabCloudScriptAPI.ExecuteFunction(executeFunctionRequest, delegate(ExecuteFunctionResult result)
		{
			Dictionary<string, string> dictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(result.FunctionResult.ToString());
			if (dictionary == null)
			{
				using (List<NetPlayer>.Enumerator enumerator2 = RigContainer.requestedAutomutePlayers.GetEnumerator())
				{
					while (enumerator2.MoveNext())
					{
						NetPlayer netPlayer2 = enumerator2.Current;
						if (netPlayer2 != null)
						{
							RigContainer.ReceiveAutomuteSettings(netPlayer2, "none");
						}
					}
					goto IL_A6;
				}
			}
			foreach (NetPlayer netPlayer3 in RigContainer.requestedAutomutePlayers)
			{
				if (netPlayer3 != null)
				{
					string score;
					if (dictionary.TryGetValue(netPlayer3.UserId, out score))
					{
						RigContainer.ReceiveAutomuteSettings(netPlayer3, score);
					}
					else
					{
						RigContainer.ReceiveAutomuteSettings(netPlayer3, "none");
					}
				}
			}
			IL_A6:
			RigContainer.requestedAutomutePlayers.Clear();
			RigContainer.waitingForAutomuteCallback = false;
		}, delegate(PlayFabError error)
		{
			foreach (NetPlayer player in RigContainer.requestedAutomutePlayers)
			{
				RigContainer.ReceiveAutomuteSettings(player, "ERROR");
			}
			RigContainer.requestedAutomutePlayers.Clear();
			RigContainer.waitingForAutomuteCallback = false;
		}, null, null);
	}

	// Token: 0x0600210D RID: 8461 RVA: 0x00046837 File Offset: 0x00044A37
	private static void CancelAutomuteRequest()
	{
		RigContainer.playersToCheckAutomute.Clear();
		RigContainer.automuteQueued = false;
		if (RigContainer.requestedAutomutePlayers != null)
		{
			RigContainer.requestedAutomutePlayers.Clear();
		}
		RigContainer.waitingForAutomuteCallback = false;
	}

	// Token: 0x0600210E RID: 8462 RVA: 0x000F49F4 File Offset: 0x000F2BF4
	private static void ReceiveAutomuteSettings(NetPlayer player, string score)
	{
		RigContainer rigContainer;
		VRRigCache.Instance.TryGetVrrig(player, out rigContainer);
		if (rigContainer != null)
		{
			rigContainer.UpdateAutomuteLevel(score);
		}
	}

	// Token: 0x0600210F RID: 8463 RVA: 0x000F4A20 File Offset: 0x000F2C20
	private void ProcessAutomute()
	{
		int @int = PlayerPrefs.GetInt("autoMute", 1);
		this.bPlayerAutoMuted = (!this.hasManualMute && this.playerChatQuality < @int);
	}

	// Token: 0x06002110 RID: 8464 RVA: 0x000F4A54 File Offset: 0x000F2C54
	public void RefreshVoiceChat()
	{
		if (this.Voice == null)
		{
			return;
		}
		this.ProcessAutomute();
		this.Voice.SpeakerInUse.enabled = (!this.forceMute && this.enableVoice && !this.bPlayerAutoMuted && GorillaComputer.instance.voiceChatOn == "TRUE");
		this.replacementVoiceSource.mute = (this.forceMute || !this.enableVoice || this.bPlayerAutoMuted || GorillaComputer.instance.voiceChatOn == "OFF");
	}

	// Token: 0x06002111 RID: 8465 RVA: 0x000F4AF4 File Offset: 0x000F2CF4
	public static void RefreshAllRigVoices()
	{
		RigContainer.staticTempRC = null;
		if (!NetworkSystem.Instance.InRoom || VRRigCache.Instance == null)
		{
			return;
		}
		foreach (NetPlayer targetPlayer in NetworkSystem.Instance.AllNetPlayers)
		{
			if (VRRigCache.Instance.TryGetVrrig(targetPlayer, out RigContainer.staticTempRC))
			{
				RigContainer.staticTempRC.RefreshVoiceChat();
			}
		}
	}

	// Token: 0x040024E1 RID: 9441
	[SerializeField]
	private VRRig vrrig;

	// Token: 0x040024E2 RID: 9442
	[SerializeField]
	private VRRigReliableState reliableState;

	// Token: 0x040024E3 RID: 9443
	[SerializeField]
	private Transform speakerHead;

	// Token: 0x040024E4 RID: 9444
	[SerializeField]
	private AudioSource replacementVoiceSource;

	// Token: 0x040024E5 RID: 9445
	private PhotonVoiceView voiceView;

	// Token: 0x040024E6 RID: 9446
	private int m_cachedNetViewID;

	// Token: 0x040024E7 RID: 9447
	private bool enableVoice = true;

	// Token: 0x040024E8 RID: 9448
	private bool forceMute;

	// Token: 0x040024E9 RID: 9449
	[SerializeField]
	private SphereCollider headCollider;

	// Token: 0x040024EA RID: 9450
	[SerializeField]
	private CapsuleCollider bodyCollider;

	// Token: 0x040024EB RID: 9451
	[SerializeField]
	private VRRigEvents rigEvents;

	// Token: 0x040024EC RID: 9452
	public bool hasManualMute;

	// Token: 0x040024ED RID: 9453
	private bool bPlayerAutoMuted;

	// Token: 0x040024EE RID: 9454
	public int playerChatQuality = 2;

	// Token: 0x040024EF RID: 9455
	private static List<NetPlayer> playersToCheckAutomute = new List<NetPlayer>();

	// Token: 0x040024F0 RID: 9456
	private static bool automuteQueued = false;

	// Token: 0x040024F1 RID: 9457
	private static List<NetPlayer> requestedAutomutePlayers;

	// Token: 0x040024F2 RID: 9458
	private static bool waitingForAutomuteCallback = false;

	// Token: 0x040024F3 RID: 9459
	private static RigContainer staticTempRC;
}
