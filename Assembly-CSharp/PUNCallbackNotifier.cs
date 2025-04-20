using System;
using System.Collections.Generic;
using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

// Token: 0x020002B7 RID: 695
public class PUNCallbackNotifier : MonoBehaviourPunCallbacks, IOnEventCallback
{
	// Token: 0x060010DE RID: 4318 RVA: 0x0003B7D7 File Offset: 0x000399D7
	private void Start()
	{
		this.parentSystem = base.GetComponent<NetworkSystemPUN>();
	}

	// Token: 0x060010DF RID: 4319 RVA: 0x00030607 File Offset: 0x0002E807
	private void Update()
	{
	}

	// Token: 0x060010E0 RID: 4320 RVA: 0x0003B7E5 File Offset: 0x000399E5
	public override void OnConnectedToMaster()
	{
		this.parentSystem.OnConnectedtoMaster();
	}

	// Token: 0x060010E1 RID: 4321 RVA: 0x0003B7F2 File Offset: 0x000399F2
	public override void OnJoinedRoom()
	{
		this.parentSystem.OnJoinedRoom();
	}

	// Token: 0x060010E2 RID: 4322 RVA: 0x0003B7FF File Offset: 0x000399FF
	public override void OnJoinRoomFailed(short returnCode, string message)
	{
		this.parentSystem.OnJoinRoomFailed(returnCode, message);
	}

	// Token: 0x060010E3 RID: 4323 RVA: 0x0003B7FF File Offset: 0x000399FF
	public override void OnJoinRandomFailed(short returnCode, string message)
	{
		this.parentSystem.OnJoinRoomFailed(returnCode, message);
	}

	// Token: 0x060010E4 RID: 4324 RVA: 0x0003B80E File Offset: 0x00039A0E
	public override void OnCreateRoomFailed(short returnCode, string message)
	{
		this.parentSystem.OnCreateRoomFailed(returnCode, message);
	}

	// Token: 0x060010E5 RID: 4325 RVA: 0x0003B81D File Offset: 0x00039A1D
	public override void OnPlayerEnteredRoom(Player newPlayer)
	{
		this.parentSystem.OnPlayerEnteredRoom(newPlayer);
	}

	// Token: 0x060010E6 RID: 4326 RVA: 0x0003B82B File Offset: 0x00039A2B
	public override void OnPlayerLeftRoom(Player otherPlayer)
	{
		this.parentSystem.OnPlayerLeftRoom(otherPlayer);
	}

	// Token: 0x060010E7 RID: 4327 RVA: 0x0003B839 File Offset: 0x00039A39
	public override void OnDisconnected(DisconnectCause cause)
	{
		Debug.Log("Disconnect callback, cause:" + cause.ToString());
		this.parentSystem.OnDisconnected(cause);
	}

	// Token: 0x060010E8 RID: 4328 RVA: 0x0003B863 File Offset: 0x00039A63
	public void OnEvent(EventData photonEvent)
	{
		this.parentSystem.RaiseEvent(photonEvent.Code, photonEvent.CustomData, photonEvent.Sender);
	}

	// Token: 0x060010E9 RID: 4329 RVA: 0x0003B882 File Offset: 0x00039A82
	public override void OnMasterClientSwitched(Player newMasterClient)
	{
		this.parentSystem.OnMasterClientSwitched(newMasterClient);
	}

	// Token: 0x060010EA RID: 4330 RVA: 0x0003B890 File Offset: 0x00039A90
	public override void OnCustomAuthenticationResponse(Dictionary<string, object> data)
	{
		base.OnCustomAuthenticationResponse(data);
		NetworkSystem.Instance.CustomAuthenticationResponse(data);
	}

	// Token: 0x040012F8 RID: 4856
	private NetworkSystemPUN parentSystem;
}
