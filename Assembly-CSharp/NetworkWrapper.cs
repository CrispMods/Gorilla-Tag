using System;
using GorillaNetworking;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020002A2 RID: 674
public class NetworkWrapper : MonoBehaviour
{
	// Token: 0x06001058 RID: 4184 RVA: 0x0003B3F8 File Offset: 0x000395F8
	[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
	public static void AutoInstantiate()
	{
		UnityEngine.Object.DontDestroyOnLoad(UnityEngine.Object.Instantiate<GameObject>(Resources.Load<GameObject>("P_NetworkWrapper")));
	}

	// Token: 0x06001059 RID: 4185 RVA: 0x000A9360 File Offset: 0x000A7560
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

	// Token: 0x0600105A RID: 4186 RVA: 0x0003B40E File Offset: 0x0003960E
	private void UpdatePlayerCountWrapper(NetPlayer player)
	{
		this.UpdatePlayerCount();
	}

	// Token: 0x0600105B RID: 4187 RVA: 0x000A9440 File Offset: 0x000A7640
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

	// Token: 0x04001266 RID: 4710
	[HideInInspector]
	public NetworkSystem activeNetworkSystem;

	// Token: 0x04001267 RID: 4711
	public Text titleRef;

	// Token: 0x04001268 RID: 4712
	[Header("NetSys settings")]
	public NetworkSystemConfig netSysConfig;

	// Token: 0x04001269 RID: 4713
	public string[] networkRegionNames;

	// Token: 0x0400126A RID: 4714
	public string[] devNetworkRegionNames;

	// Token: 0x0400126B RID: 4715
	[Header("Debug output refs")]
	public Text stateTextRef;

	// Token: 0x0400126C RID: 4716
	public Text playerCountTextRef;

	// Token: 0x0400126D RID: 4717
	[SerializeField]
	private SO_NetworkVoiceSettings VoiceSettings;

	// Token: 0x0400126E RID: 4718
	private const string WrapperResourcePath = "P_NetworkWrapper";
}
