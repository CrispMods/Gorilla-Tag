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

// Token: 0x02000540 RID: 1344
[RequireComponent(typeof(VRRig), typeof(VRRigReliableState))]
public class RigContainer : MonoBehaviour
{
	// Token: 0x1700034D RID: 845
	// (get) Token: 0x0600209A RID: 8346 RVA: 0x0004532D File Offset: 0x0004352D
	// (set) Token: 0x0600209B RID: 8347 RVA: 0x00045335 File Offset: 0x00043535
	public bool Initialized { get; private set; }

	// Token: 0x1700034E RID: 846
	// (get) Token: 0x0600209C RID: 8348 RVA: 0x0004533E File Offset: 0x0004353E
	public VRRig Rig
	{
		get
		{
			return this.vrrig;
		}
	}

	// Token: 0x1700034F RID: 847
	// (get) Token: 0x0600209D RID: 8349 RVA: 0x00045346 File Offset: 0x00043546
	public VRRigReliableState ReliableState
	{
		get
		{
			return this.reliableState;
		}
	}

	// Token: 0x17000350 RID: 848
	// (get) Token: 0x0600209E RID: 8350 RVA: 0x0004534E File Offset: 0x0004354E
	public Transform SpeakerHead
	{
		get
		{
			return this.speakerHead;
		}
	}

	// Token: 0x17000351 RID: 849
	// (get) Token: 0x0600209F RID: 8351 RVA: 0x00045356 File Offset: 0x00043556
	public AudioSource ReplacementVoiceSource
	{
		get
		{
			return this.replacementVoiceSource;
		}
	}

	// Token: 0x17000352 RID: 850
	// (get) Token: 0x060020A0 RID: 8352 RVA: 0x0004535E File Offset: 0x0004355E
	// (set) Token: 0x060020A1 RID: 8353 RVA: 0x00045366 File Offset: 0x00043566
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

	// Token: 0x17000353 RID: 851
	// (get) Token: 0x060020A2 RID: 8354 RVA: 0x000453A3 File Offset: 0x000435A3
	public NetworkView netView
	{
		get
		{
			return this.vrrig.netView;
		}
	}

	// Token: 0x17000354 RID: 852
	// (get) Token: 0x060020A3 RID: 8355 RVA: 0x000453B0 File Offset: 0x000435B0
	public int CachedNetViewID
	{
		get
		{
			return this.m_cachedNetViewID;
		}
	}

	// Token: 0x17000355 RID: 853
	// (get) Token: 0x060020A4 RID: 8356 RVA: 0x000453B8 File Offset: 0x000435B8
	// (set) Token: 0x060020A5 RID: 8357 RVA: 0x000453C3 File Offset: 0x000435C3
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

	// Token: 0x17000356 RID: 854
	// (get) Token: 0x060020A6 RID: 8358 RVA: 0x000453D5 File Offset: 0x000435D5
	// (set) Token: 0x060020A7 RID: 8359 RVA: 0x000F17AC File Offset: 0x000EF9AC
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

	// Token: 0x17000357 RID: 855
	// (get) Token: 0x060020A8 RID: 8360 RVA: 0x000453E2 File Offset: 0x000435E2
	// (set) Token: 0x060020A9 RID: 8361 RVA: 0x000453EA File Offset: 0x000435EA
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

	// Token: 0x17000358 RID: 856
	// (get) Token: 0x060020AA RID: 8362 RVA: 0x000453F9 File Offset: 0x000435F9
	public SphereCollider HeadCollider
	{
		get
		{
			return this.headCollider;
		}
	}

	// Token: 0x17000359 RID: 857
	// (get) Token: 0x060020AB RID: 8363 RVA: 0x00045401 File Offset: 0x00043601
	public CapsuleCollider BodyCollider
	{
		get
		{
			return this.bodyCollider;
		}
	}

	// Token: 0x1700035A RID: 858
	// (get) Token: 0x060020AC RID: 8364 RVA: 0x00045409 File Offset: 0x00043609
	public VRRigEvents RigEvents
	{
		get
		{
			return this.rigEvents;
		}
	}

	// Token: 0x060020AD RID: 8365 RVA: 0x00045411 File Offset: 0x00043611
	public bool GetIsPlayerAutoMuted()
	{
		return this.bPlayerAutoMuted;
	}

	// Token: 0x060020AE RID: 8366 RVA: 0x000F1800 File Offset: 0x000EFA00
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

	// Token: 0x060020AF RID: 8367 RVA: 0x000F1860 File Offset: 0x000EFA60
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

	// Token: 0x060020B0 RID: 8368 RVA: 0x00045419 File Offset: 0x00043619
	private void OnMultiPlayerStarted()
	{
		if (this.Rig.isOfflineVRRig)
		{
			this.vrrig.creator = NetworkSystem.Instance.GetLocalPlayer();
		}
	}

	// Token: 0x060020B1 RID: 8369 RVA: 0x0004543D File Offset: 0x0004363D
	private void OnReturnedToSinglePlayer()
	{
		if (this.Rig.isOfflineVRRig)
		{
			RigContainer.CancelAutomuteRequest();
		}
	}

	// Token: 0x060020B2 RID: 8370 RVA: 0x000F18DC File Offset: 0x000EFADC
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

	// Token: 0x060020B3 RID: 8371 RVA: 0x00045451 File Offset: 0x00043651
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

	// Token: 0x060020B4 RID: 8372 RVA: 0x000F1968 File Offset: 0x000EFB68
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

	// Token: 0x060020B5 RID: 8373 RVA: 0x00045489 File Offset: 0x00043689
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

	// Token: 0x060020B6 RID: 8374 RVA: 0x000F1B0C File Offset: 0x000EFD0C
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

	// Token: 0x060020B7 RID: 8375 RVA: 0x00045498 File Offset: 0x00043698
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

	// Token: 0x060020B8 RID: 8376 RVA: 0x000F1C70 File Offset: 0x000EFE70
	private static void ReceiveAutomuteSettings(NetPlayer player, string score)
	{
		RigContainer rigContainer;
		VRRigCache.Instance.TryGetVrrig(player, out rigContainer);
		if (rigContainer != null)
		{
			rigContainer.UpdateAutomuteLevel(score);
		}
	}

	// Token: 0x060020B9 RID: 8377 RVA: 0x000F1C9C File Offset: 0x000EFE9C
	private void ProcessAutomute()
	{
		int @int = PlayerPrefs.GetInt("autoMute", 1);
		this.bPlayerAutoMuted = (!this.hasManualMute && this.playerChatQuality < @int);
	}

	// Token: 0x060020BA RID: 8378 RVA: 0x000F1CD0 File Offset: 0x000EFED0
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

	// Token: 0x060020BB RID: 8379 RVA: 0x000F1D70 File Offset: 0x000EFF70
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

	// Token: 0x0400248F RID: 9359
	[SerializeField]
	private VRRig vrrig;

	// Token: 0x04002490 RID: 9360
	[SerializeField]
	private VRRigReliableState reliableState;

	// Token: 0x04002491 RID: 9361
	[SerializeField]
	private Transform speakerHead;

	// Token: 0x04002492 RID: 9362
	[SerializeField]
	private AudioSource replacementVoiceSource;

	// Token: 0x04002493 RID: 9363
	private PhotonVoiceView voiceView;

	// Token: 0x04002494 RID: 9364
	private int m_cachedNetViewID;

	// Token: 0x04002495 RID: 9365
	private bool enableVoice = true;

	// Token: 0x04002496 RID: 9366
	private bool forceMute;

	// Token: 0x04002497 RID: 9367
	[SerializeField]
	private SphereCollider headCollider;

	// Token: 0x04002498 RID: 9368
	[SerializeField]
	private CapsuleCollider bodyCollider;

	// Token: 0x04002499 RID: 9369
	[SerializeField]
	private VRRigEvents rigEvents;

	// Token: 0x0400249A RID: 9370
	public bool hasManualMute;

	// Token: 0x0400249B RID: 9371
	private bool bPlayerAutoMuted;

	// Token: 0x0400249C RID: 9372
	public int playerChatQuality = 2;

	// Token: 0x0400249D RID: 9373
	private static List<NetPlayer> playersToCheckAutomute = new List<NetPlayer>();

	// Token: 0x0400249E RID: 9374
	private static bool automuteQueued = false;

	// Token: 0x0400249F RID: 9375
	private static List<NetPlayer> requestedAutomutePlayers;

	// Token: 0x040024A0 RID: 9376
	private static bool waitingForAutomuteCallback = false;

	// Token: 0x040024A1 RID: 9377
	private static RigContainer staticTempRC;
}
