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

// Token: 0x0200053F RID: 1343
[RequireComponent(typeof(VRRig), typeof(VRRigReliableState))]
public class RigContainer : MonoBehaviour
{
	// Token: 0x1700034C RID: 844
	// (get) Token: 0x06002092 RID: 8338 RVA: 0x000A3586 File Offset: 0x000A1786
	// (set) Token: 0x06002093 RID: 8339 RVA: 0x000A358E File Offset: 0x000A178E
	public bool Initialized { get; private set; }

	// Token: 0x1700034D RID: 845
	// (get) Token: 0x06002094 RID: 8340 RVA: 0x000A3597 File Offset: 0x000A1797
	public VRRig Rig
	{
		get
		{
			return this.vrrig;
		}
	}

	// Token: 0x1700034E RID: 846
	// (get) Token: 0x06002095 RID: 8341 RVA: 0x000A359F File Offset: 0x000A179F
	public VRRigReliableState ReliableState
	{
		get
		{
			return this.reliableState;
		}
	}

	// Token: 0x1700034F RID: 847
	// (get) Token: 0x06002096 RID: 8342 RVA: 0x000A35A7 File Offset: 0x000A17A7
	public Transform SpeakerHead
	{
		get
		{
			return this.speakerHead;
		}
	}

	// Token: 0x17000350 RID: 848
	// (get) Token: 0x06002097 RID: 8343 RVA: 0x000A35AF File Offset: 0x000A17AF
	public AudioSource ReplacementVoiceSource
	{
		get
		{
			return this.replacementVoiceSource;
		}
	}

	// Token: 0x17000351 RID: 849
	// (get) Token: 0x06002098 RID: 8344 RVA: 0x000A35B7 File Offset: 0x000A17B7
	// (set) Token: 0x06002099 RID: 8345 RVA: 0x000A35BF File Offset: 0x000A17BF
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

	// Token: 0x17000352 RID: 850
	// (get) Token: 0x0600209A RID: 8346 RVA: 0x000A35FC File Offset: 0x000A17FC
	public NetworkView netView
	{
		get
		{
			return this.vrrig.netView;
		}
	}

	// Token: 0x17000353 RID: 851
	// (get) Token: 0x0600209B RID: 8347 RVA: 0x000A3609 File Offset: 0x000A1809
	public int CachedNetViewID
	{
		get
		{
			return this.m_cachedNetViewID;
		}
	}

	// Token: 0x17000354 RID: 852
	// (get) Token: 0x0600209C RID: 8348 RVA: 0x000A3611 File Offset: 0x000A1811
	// (set) Token: 0x0600209D RID: 8349 RVA: 0x000A361C File Offset: 0x000A181C
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

	// Token: 0x17000355 RID: 853
	// (get) Token: 0x0600209E RID: 8350 RVA: 0x000A362E File Offset: 0x000A182E
	// (set) Token: 0x0600209F RID: 8351 RVA: 0x000A363C File Offset: 0x000A183C
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

	// Token: 0x17000356 RID: 854
	// (get) Token: 0x060020A0 RID: 8352 RVA: 0x000A368D File Offset: 0x000A188D
	// (set) Token: 0x060020A1 RID: 8353 RVA: 0x000A3695 File Offset: 0x000A1895
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

	// Token: 0x17000357 RID: 855
	// (get) Token: 0x060020A2 RID: 8354 RVA: 0x000A36A4 File Offset: 0x000A18A4
	public SphereCollider HeadCollider
	{
		get
		{
			return this.headCollider;
		}
	}

	// Token: 0x17000358 RID: 856
	// (get) Token: 0x060020A3 RID: 8355 RVA: 0x000A36AC File Offset: 0x000A18AC
	public CapsuleCollider BodyCollider
	{
		get
		{
			return this.bodyCollider;
		}
	}

	// Token: 0x17000359 RID: 857
	// (get) Token: 0x060020A4 RID: 8356 RVA: 0x000A36B4 File Offset: 0x000A18B4
	public VRRigEvents RigEvents
	{
		get
		{
			return this.rigEvents;
		}
	}

	// Token: 0x060020A5 RID: 8357 RVA: 0x000A36BC File Offset: 0x000A18BC
	public bool GetIsPlayerAutoMuted()
	{
		return this.bPlayerAutoMuted;
	}

	// Token: 0x060020A6 RID: 8358 RVA: 0x000A36C4 File Offset: 0x000A18C4
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

	// Token: 0x060020A7 RID: 8359 RVA: 0x000A3724 File Offset: 0x000A1924
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

	// Token: 0x060020A8 RID: 8360 RVA: 0x000A379F File Offset: 0x000A199F
	private void OnMultiPlayerStarted()
	{
		if (this.Rig.isOfflineVRRig)
		{
			this.vrrig.creator = NetworkSystem.Instance.GetLocalPlayer();
		}
	}

	// Token: 0x060020A9 RID: 8361 RVA: 0x000A37C3 File Offset: 0x000A19C3
	private void OnReturnedToSinglePlayer()
	{
		if (this.Rig.isOfflineVRRig)
		{
			RigContainer.CancelAutomuteRequest();
		}
	}

	// Token: 0x060020AA RID: 8362 RVA: 0x000A37D8 File Offset: 0x000A19D8
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

	// Token: 0x060020AB RID: 8363 RVA: 0x000A3861 File Offset: 0x000A1A61
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

	// Token: 0x060020AC RID: 8364 RVA: 0x000A389C File Offset: 0x000A1A9C
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

	// Token: 0x060020AD RID: 8365 RVA: 0x000A3A3E File Offset: 0x000A1C3E
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

	// Token: 0x060020AE RID: 8366 RVA: 0x000A3A50 File Offset: 0x000A1C50
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

	// Token: 0x060020AF RID: 8367 RVA: 0x000A3BB4 File Offset: 0x000A1DB4
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

	// Token: 0x060020B0 RID: 8368 RVA: 0x000A3BE0 File Offset: 0x000A1DE0
	private static void ReceiveAutomuteSettings(NetPlayer player, string score)
	{
		RigContainer rigContainer;
		VRRigCache.Instance.TryGetVrrig(player, out rigContainer);
		if (rigContainer != null)
		{
			rigContainer.UpdateAutomuteLevel(score);
		}
	}

	// Token: 0x060020B1 RID: 8369 RVA: 0x000A3C0C File Offset: 0x000A1E0C
	private void ProcessAutomute()
	{
		int @int = PlayerPrefs.GetInt("autoMute", 1);
		this.bPlayerAutoMuted = (!this.hasManualMute && this.playerChatQuality < @int);
	}

	// Token: 0x060020B2 RID: 8370 RVA: 0x000A3C40 File Offset: 0x000A1E40
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

	// Token: 0x060020B3 RID: 8371 RVA: 0x000A3CE0 File Offset: 0x000A1EE0
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

	// Token: 0x04002489 RID: 9353
	[SerializeField]
	private VRRig vrrig;

	// Token: 0x0400248A RID: 9354
	[SerializeField]
	private VRRigReliableState reliableState;

	// Token: 0x0400248B RID: 9355
	[SerializeField]
	private Transform speakerHead;

	// Token: 0x0400248C RID: 9356
	[SerializeField]
	private AudioSource replacementVoiceSource;

	// Token: 0x0400248D RID: 9357
	private PhotonVoiceView voiceView;

	// Token: 0x0400248E RID: 9358
	private int m_cachedNetViewID;

	// Token: 0x0400248F RID: 9359
	private bool enableVoice = true;

	// Token: 0x04002490 RID: 9360
	private bool forceMute;

	// Token: 0x04002491 RID: 9361
	[SerializeField]
	private SphereCollider headCollider;

	// Token: 0x04002492 RID: 9362
	[SerializeField]
	private CapsuleCollider bodyCollider;

	// Token: 0x04002493 RID: 9363
	[SerializeField]
	private VRRigEvents rigEvents;

	// Token: 0x04002494 RID: 9364
	public bool hasManualMute;

	// Token: 0x04002495 RID: 9365
	private bool bPlayerAutoMuted;

	// Token: 0x04002496 RID: 9366
	public int playerChatQuality = 2;

	// Token: 0x04002497 RID: 9367
	private static List<NetPlayer> playersToCheckAutomute = new List<NetPlayer>();

	// Token: 0x04002498 RID: 9368
	private static bool automuteQueued = false;

	// Token: 0x04002499 RID: 9369
	private static List<NetPlayer> requestedAutomutePlayers;

	// Token: 0x0400249A RID: 9370
	private static bool waitingForAutomuteCallback = false;

	// Token: 0x0400249B RID: 9371
	private static RigContainer staticTempRC;
}
