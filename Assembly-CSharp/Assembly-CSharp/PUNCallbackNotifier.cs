using System;
using System.Collections.Generic;
using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

// Token: 0x020002AC RID: 684
public class PUNCallbackNotifier : MonoBehaviourPunCallbacks, IOnEventCallback
{
	// Token: 0x06001095 RID: 4245 RVA: 0x00050FFE File Offset: 0x0004F1FE
	private void Start()
	{
		this.parentSystem = base.GetComponent<NetworkSystemPUN>();
	}

	// Token: 0x06001096 RID: 4246 RVA: 0x000023F4 File Offset: 0x000005F4
	private void Update()
	{
	}

	// Token: 0x06001097 RID: 4247 RVA: 0x0005100C File Offset: 0x0004F20C
	public override void OnConnectedToMaster()
	{
		this.parentSystem.OnConnectedtoMaster();
	}

	// Token: 0x06001098 RID: 4248 RVA: 0x00051019 File Offset: 0x0004F219
	public override void OnJoinedRoom()
	{
		this.parentSystem.OnJoinedRoom();
	}

	// Token: 0x06001099 RID: 4249 RVA: 0x00051026 File Offset: 0x0004F226
	public override void OnJoinRoomFailed(short returnCode, string message)
	{
		this.parentSystem.OnJoinRoomFailed(returnCode, message);
	}

	// Token: 0x0600109A RID: 4250 RVA: 0x00051026 File Offset: 0x0004F226
	public override void OnJoinRandomFailed(short returnCode, string message)
	{
		this.parentSystem.OnJoinRoomFailed(returnCode, message);
	}

	// Token: 0x0600109B RID: 4251 RVA: 0x00051035 File Offset: 0x0004F235
	public override void OnCreateRoomFailed(short returnCode, string message)
	{
		this.parentSystem.OnCreateRoomFailed(returnCode, message);
	}

	// Token: 0x0600109C RID: 4252 RVA: 0x00051044 File Offset: 0x0004F244
	public override void OnPlayerEnteredRoom(Player newPlayer)
	{
		this.parentSystem.OnPlayerEnteredRoom(newPlayer);
	}

	// Token: 0x0600109D RID: 4253 RVA: 0x00051052 File Offset: 0x0004F252
	public override void OnPlayerLeftRoom(Player otherPlayer)
	{
		this.parentSystem.OnPlayerLeftRoom(otherPlayer);
	}

	// Token: 0x0600109E RID: 4254 RVA: 0x00051060 File Offset: 0x0004F260
	public override void OnDisconnected(DisconnectCause cause)
	{
		Debug.Log("Disconnect callback, cause:" + cause.ToString());
		this.parentSystem.OnDisconnected(cause);
	}

	// Token: 0x0600109F RID: 4255 RVA: 0x0005108A File Offset: 0x0004F28A
	public void OnEvent(EventData photonEvent)
	{
		this.parentSystem.RaiseEvent(photonEvent.Code, photonEvent.CustomData, photonEvent.Sender);
	}

	// Token: 0x060010A0 RID: 4256 RVA: 0x000510A9 File Offset: 0x0004F2A9
	public override void OnMasterClientSwitched(Player newMasterClient)
	{
		this.parentSystem.OnMasterClientSwitched(newMasterClient);
	}

	// Token: 0x060010A1 RID: 4257 RVA: 0x000510B7 File Offset: 0x0004F2B7
	public override void OnCustomAuthenticationResponse(Dictionary<string, object> data)
	{
		base.OnCustomAuthenticationResponse(data);
		NetworkSystem.Instance.CustomAuthenticationResponse(data);
	}

	// Token: 0x040012B1 RID: 4785
	private NetworkSystemPUN parentSystem;
}
