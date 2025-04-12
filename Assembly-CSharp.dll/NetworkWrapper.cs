using System;
using GorillaNetworking;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000297 RID: 663
public class NetworkWrapper : MonoBehaviour
{
	// Token: 0x0600100F RID: 4111 RVA: 0x0003A138 File Offset: 0x00038338
	[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
	public static void AutoInstantiate()
	{
		UnityEngine.Object.DontDestroyOnLoad(UnityEngine.Object.Instantiate<GameObject>(Resources.Load<GameObject>("P_NetworkWrapper")));
	}

	// Token: 0x06001010 RID: 4112 RVA: 0x000A6AD4 File Offset: 0x000A4CD4
	private void Awake()
	{
		if (this.titleRef != null)
		{
			this.titleRef.text = "PUN";
		}
		this.activeNetworkSystem = base.gameObject.AddComponent<NetworkSystemPUN>();
		this.activeNetworkSystem.AddVoiceSettings(this.VoiceSettings);
		this.activeNetworkSystem.config = this.netSysConfig;
		this.activeNetworkSystem.regionNames = this.networkRegionNames;
		this.activeNetworkSystem.OnPlayerJoined += this.UpdatePlayerCountWrapper;
		this.activeNetworkSystem.OnPlayerLeft += this.UpdatePlayerCountWrapper;
		this.activeNetworkSystem.OnMultiplayerStarted += this.UpdatePlayerCount;
		this.activeNetworkSystem.OnReturnedToSinglePlayer += this.UpdatePlayerCount;
		Debug.Log("<color=green>initialize Network System</color>");
		this.activeNetworkSystem.Initialise();
	}

	// Token: 0x06001011 RID: 4113 RVA: 0x0003A14E File Offset: 0x0003834E
	private void UpdatePlayerCountWrapper(NetPlayer player)
	{
		this.UpdatePlayerCount();
	}

	// Token: 0x06001012 RID: 4114 RVA: 0x000A6BB4 File Offset: 0x000A4DB4
	private void UpdatePlayerCount()
	{
		if (this.playerCountTextRef == null)
		{
			return;
		}
		if (!this.activeNetworkSystem.IsOnline)
		{
			this.playerCountTextRef.text = string.Format("0/{0}", this.netSysConfig.MaxPlayerCount);
			Debug.Log("Player count updated");
			return;
		}
		Debug.Log("Player count not updated");
		this.playerCountTextRef.text = string.Format("{0}/{1}", this.activeNetworkSystem.AllNetPlayers.Length, this.netSysConfig.MaxPlayerCount);
	}

	// Token: 0x0400121F RID: 4639
	[HideInInspector]
	public NetworkSystem activeNetworkSystem;

	// Token: 0x04001220 RID: 4640
	public Text titleRef;

	// Token: 0x04001221 RID: 4641
	[Header("NetSys settings")]
	public NetworkSystemConfig netSysConfig;

	// Token: 0x04001222 RID: 4642
	public string[] networkRegionNames;

	// Token: 0x04001223 RID: 4643
	public string[] devNetworkRegionNames;

	// Token: 0x04001224 RID: 4644
	[Header("Debug output refs")]
	public Text stateTextRef;

	// Token: 0x04001225 RID: 4645
	public Text playerCountTextRef;

	// Token: 0x04001226 RID: 4646
	[SerializeField]
	private SO_NetworkVoiceSettings VoiceSettings;

	// Token: 0x04001227 RID: 4647
	private const string WrapperResourcePath = "P_NetworkWrapper";
}
