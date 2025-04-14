using System;
using System.Collections.Generic;
using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

// Token: 0x020002AC RID: 684
public class PUNCallbackNotifier : MonoBehaviourPunCallbacks, IOnEventCallback
{
	// Token: 0x06001092 RID: 4242 RVA: 0x00050C7A File Offset: 0x0004EE7A
	private void Start()
	{
		this.parentSystem = base.GetComponent<NetworkSystemPUN>();
	}

	// Token: 0x06001093 RID: 4243 RVA: 0x000023F4 File Offset: 0x000005F4
	private void Update()
	{
	}

	// Token: 0x06001094 RID: 4244 RVA: 0x00050C88 File Offset: 0x0004EE88
	public override void OnConnectedToMaster()
	{
		this.parentSystem.OnConnectedtoMaster();
	}

	// Token: 0x06001095 RID: 4245 RVA: 0x00050C95 File Offset: 0x0004EE95
	public override void OnJoinedRoom()
	{
		this.parentSystem.OnJoinedRoom();
	}

	// Token: 0x06001096 RID: 4246 RVA: 0x00050CA2 File Offset: 0x0004EEA2
	public override void OnJoinRoomFailed(short returnCode, string message)
	{
		this.parentSystem.OnJoinRoomFailed(returnCode, message);
	}

	// Token: 0x06001097 RID: 4247 RVA: 0x00050CA2 File Offset: 0x0004EEA2
	public override void OnJoinRandomFailed(short returnCode, string message)
	{
		this.parentSystem.OnJoinRoomFailed(returnCode, message);
	}

	// Token: 0x06001098 RID: 4248 RVA: 0x00050CB1 File Offset: 0x0004EEB1
	public override void OnCreateRoomFailed(short returnCode, string message)
	{
		this.parentSystem.OnCreateRoomFailed(returnCode, message);
	}

	// Token: 0x06001099 RID: 4249 RVA: 0x00050CC0 File Offset: 0x0004EEC0
	public override void OnPlayerEnteredRoom(Player newPlayer)
	{
		this.parentSystem.OnPlayerEnteredRoom(newPlayer);
	}

	// Token: 0x0600109A RID: 4250 RVA: 0x00050CCE File Offset: 0x0004EECE
	public override void OnPlayerLeftRoom(Player otherPlayer)
	{
		this.parentSystem.OnPlayerLeftRoom(otherPlayer);
	}

	// Token: 0x0600109B RID: 4251 RVA: 0x00050CDC File Offset: 0x0004EEDC
	public override void OnDisconnected(DisconnectCause cause)
	{
		Debug.Log("Disconnect callback, cause:" + cause.ToString());
		this.parentSystem.OnDisconnected(cause);
	}

	// Token: 0x0600109C RID: 4252 RVA: 0x00050D06 File Offset: 0x0004EF06
	public void OnEvent(EventData photonEvent)
	{
		this.parentSystem.RaiseEvent(photonEvent.Code, photonEvent.CustomData, photonEvent.Sender);
	}

	// Token: 0x0600109D RID: 4253 RVA: 0x00050D25 File Offset: 0x0004EF25
	public override void OnMasterClientSwitched(Player newMasterClient)
	{
		this.parentSystem.OnMasterClientSwitched(newMasterClient);
	}

	// Token: 0x0600109E RID: 4254 RVA: 0x00050D33 File Offset: 0x0004EF33
	public override void OnCustomAuthenticationResponse(Dictionary<string, object> data)
	{
		base.OnCustomAuthenticationResponse(data);
		NetworkSystem.Instance.CustomAuthenticationResponse(data);
	}

	// Token: 0x040012B0 RID: 4784
	private NetworkSystemPUN parentSystem;
}
