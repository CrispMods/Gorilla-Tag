using System;
using System.Collections.Generic;
using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

// Token: 0x020002AC RID: 684
public class PUNCallbackNotifier : MonoBehaviourPunCallbacks, IOnEventCallback
{
	// Token: 0x06001095 RID: 4245 RVA: 0x0003A517 File Offset: 0x00038717
	private void Start()
	{
		this.parentSystem = base.GetComponent<NetworkSystemPUN>();
	}

	// Token: 0x06001096 RID: 4246 RVA: 0x0002F75F File Offset: 0x0002D95F
	private void Update()
	{
	}

	// Token: 0x06001097 RID: 4247 RVA: 0x0003A525 File Offset: 0x00038725
	public override void OnConnectedToMaster()
	{
		this.parentSystem.OnConnectedtoMaster();
	}

	// Token: 0x06001098 RID: 4248 RVA: 0x0003A532 File Offset: 0x00038732
	public override void OnJoinedRoom()
	{
		this.parentSystem.OnJoinedRoom();
	}

	// Token: 0x06001099 RID: 4249 RVA: 0x0003A53F File Offset: 0x0003873F
	public override void OnJoinRoomFailed(short returnCode, string message)
	{
		this.parentSystem.OnJoinRoomFailed(returnCode, message);
	}

	// Token: 0x0600109A RID: 4250 RVA: 0x0003A53F File Offset: 0x0003873F
	public override void OnJoinRandomFailed(short returnCode, string message)
	{
		this.parentSystem.OnJoinRoomFailed(returnCode, message);
	}

	// Token: 0x0600109B RID: 4251 RVA: 0x0003A54E File Offset: 0x0003874E
	public override void OnCreateRoomFailed(short returnCode, string message)
	{
		this.parentSystem.OnCreateRoomFailed(returnCode, message);
	}

	// Token: 0x0600109C RID: 4252 RVA: 0x0003A55D File Offset: 0x0003875D
	public override void OnPlayerEnteredRoom(Player newPlayer)
	{
		this.parentSystem.OnPlayerEnteredRoom(newPlayer);
	}

	// Token: 0x0600109D RID: 4253 RVA: 0x0003A56B File Offset: 0x0003876B
	public override void OnPlayerLeftRoom(Player otherPlayer)
	{
		this.parentSystem.OnPlayerLeftRoom(otherPlayer);
	}

	// Token: 0x0600109E RID: 4254 RVA: 0x0003A579 File Offset: 0x00038779
	public override void OnDisconnected(DisconnectCause cause)
	{
		Debug.Log("Disconnect callback, cause:" + cause.ToString());
		this.parentSystem.OnDisconnected(cause);
	}

	// Token: 0x0600109F RID: 4255 RVA: 0x0003A5A3 File Offset: 0x000387A3
	public void OnEvent(EventData photonEvent)
	{
		this.parentSystem.RaiseEvent(photonEvent.Code, photonEvent.CustomData, photonEvent.Sender);
	}

	// Token: 0x060010A0 RID: 4256 RVA: 0x0003A5C2 File Offset: 0x000387C2
	public override void OnMasterClientSwitched(Player newMasterClient)
	{
		this.parentSystem.OnMasterClientSwitched(newMasterClient);
	}

	// Token: 0x060010A1 RID: 4257 RVA: 0x0003A5D0 File Offset: 0x000387D0
	public override void OnCustomAuthenticationResponse(Dictionary<string, object> data)
	{
		base.OnCustomAuthenticationResponse(data);
		NetworkSystem.Instance.CustomAuthenticationResponse(data);
	}

	// Token: 0x040012B1 RID: 4785
	private NetworkSystemPUN parentSystem;
}
