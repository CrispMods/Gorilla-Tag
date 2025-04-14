using System;
using GorillaLocomotion;
using Photon.Pun;
using TMPro;
using UnityEngine;

// Token: 0x02000794 RID: 1940
public class FriendingStation : MonoBehaviour
{
	// Token: 0x170004FC RID: 1276
	// (get) Token: 0x06002FF6 RID: 12278 RVA: 0x000E7926 File Offset: 0x000E5B26
	public TextMeshProUGUI Player1Text
	{
		get
		{
			return this.player1Text;
		}
	}

	// Token: 0x170004FD RID: 1277
	// (get) Token: 0x06002FF7 RID: 12279 RVA: 0x000E792E File Offset: 0x000E5B2E
	public TextMeshProUGUI Player2Text
	{
		get
		{
			return this.player2Text;
		}
	}

	// Token: 0x170004FE RID: 1278
	// (get) Token: 0x06002FF8 RID: 12280 RVA: 0x000E7936 File Offset: 0x000E5B36
	public TextMeshProUGUI StatusText
	{
		get
		{
			return this.statusText;
		}
	}

	// Token: 0x170004FF RID: 1279
	// (get) Token: 0x06002FF9 RID: 12281 RVA: 0x000E793E File Offset: 0x000E5B3E
	public GTZone Zone
	{
		get
		{
			return this.zone;
		}
	}

	// Token: 0x06002FFA RID: 12282 RVA: 0x000E7946 File Offset: 0x000E5B46
	private void Awake()
	{
		this.triggerNotifier.TriggerEnterEvent += this.TriggerEntered;
		this.triggerNotifier.TriggerExitEvent += this.TriggerExited;
	}

	// Token: 0x06002FFB RID: 12283 RVA: 0x000E7978 File Offset: 0x000E5B78
	private void OnEnable()
	{
		FriendingManager.Instance.RegisterFriendingStation(this);
		if (PhotonNetwork.InRoom)
		{
			this.displayedData.actorNumberA = -1;
			this.displayedData.actorNumberB = -1;
			this.displayedData.state = FriendingManager.FriendStationState.WaitingForPlayers;
		}
		else
		{
			this.displayedData.actorNumberA = -2;
			this.displayedData.actorNumberB = -2;
			this.displayedData.state = FriendingManager.FriendStationState.NotInRoom;
		}
		this.UpdatePlayerText(this.player1Text, this.displayedData.actorNumberA);
		this.UpdatePlayerText(this.player2Text, this.displayedData.actorNumberB);
		this.UpdateDisplayedState(this.displayedData.state);
	}

	// Token: 0x06002FFC RID: 12284 RVA: 0x000E7A24 File Offset: 0x000E5C24
	private void OnDisable()
	{
		FriendingManager.Instance.UnregisterFriendingStation(this);
	}

	// Token: 0x06002FFD RID: 12285 RVA: 0x000E7A34 File Offset: 0x000E5C34
	private void UpdatePlayerText(TextMeshProUGUI playerText, int playerId)
	{
		if (playerId == -2)
		{
			playerText.text = "";
			return;
		}
		if (playerId == -1)
		{
			playerText.text = "PLAYER:\nNONE";
			return;
		}
		NetPlayer netPlayerByID = NetworkSystem.Instance.GetNetPlayerByID(playerId);
		if (netPlayerByID != null)
		{
			playerText.text = "PLAYER:\n" + netPlayerByID.SanitizedNickName;
			return;
		}
		playerText.text = "PLAYER:\nNONE";
	}

	// Token: 0x06002FFE RID: 12286 RVA: 0x000E7A94 File Offset: 0x000E5C94
	private void UpdateDisplayedState(FriendingManager.FriendStationState state)
	{
		switch (state)
		{
		case FriendingManager.FriendStationState.NotInRoom:
			this.statusText.text = "JOIN A ROOM TO USE";
			return;
		case FriendingManager.FriendStationState.WaitingForPlayers:
			this.statusText.text = "";
			return;
		case FriendingManager.FriendStationState.WaitingOnFriendStatusBoth:
			this.statusText.text = "LOADING";
			return;
		case FriendingManager.FriendStationState.WaitingOnFriendStatusPlayerA:
			this.statusText.text = "LOADING";
			return;
		case FriendingManager.FriendStationState.WaitingOnFriendStatusPlayerB:
			this.statusText.text = "LOADING";
			return;
		case FriendingManager.FriendStationState.WaitingOnButtonBoth:
			this.statusText.text = "PRESS [       ] PRESS";
			return;
		case FriendingManager.FriendStationState.WaitingOnButtonPlayerA:
			this.statusText.text = "PRESS [       ] READY";
			return;
		case FriendingManager.FriendStationState.WaitingOnButtonPlayerB:
			this.statusText.text = "READY [       ] PRESS";
			return;
		case FriendingManager.FriendStationState.ButtonConfirmationTimer0:
			this.statusText.text = "READY [       ] READY";
			return;
		case FriendingManager.FriendStationState.ButtonConfirmationTimer1:
			this.statusText.text = "READY [-     -] READY";
			return;
		case FriendingManager.FriendStationState.ButtonConfirmationTimer2:
			this.statusText.text = "READY [--   --] READY";
			return;
		case FriendingManager.FriendStationState.ButtonConfirmationTimer3:
			this.statusText.text = "READY [--- ---] READY";
			return;
		case FriendingManager.FriendStationState.ButtonConfirmationTimer4:
			this.statusText.text = "READY [-------] READY";
			return;
		case FriendingManager.FriendStationState.WaitingOnRequestBoth:
			this.statusText.text = " SENT [-------] SENT ";
			return;
		case FriendingManager.FriendStationState.WaitingOnRequestPlayerA:
			this.statusText.text = " SENT [-------] DONE ";
			return;
		case FriendingManager.FriendStationState.WaitingOnRequestPlayerB:
			this.statusText.text = " DONE [-------] SENT ";
			return;
		case FriendingManager.FriendStationState.RequestFailed:
			this.statusText.text = "FRIEND REQUEST FAILED";
			return;
		case FriendingManager.FriendStationState.Friends:
			this.statusText.text = "\\O/ FRIENDS \\O/";
			return;
		case FriendingManager.FriendStationState.AlreadyFriends:
			this.statusText.text = "ALREADY FRIENDS";
			return;
		default:
			return;
		}
	}

	// Token: 0x06002FFF RID: 12287 RVA: 0x000E7C38 File Offset: 0x000E5E38
	private void UpdateAddFriendButton()
	{
		int actorNumber = NetworkSystem.Instance.LocalPlayer.ActorNumber;
		if ((this.displayedData.state >= FriendingManager.FriendStationState.ButtonConfirmationTimer0 && this.displayedData.state <= FriendingManager.FriendStationState.ButtonConfirmationTimer4) || (this.displayedData.actorNumberA == actorNumber && this.displayedData.state == FriendingManager.FriendStationState.WaitingOnButtonPlayerB) || (this.displayedData.actorNumberB == actorNumber && this.displayedData.state == FriendingManager.FriendStationState.WaitingOnButtonPlayerA))
		{
			this.addFriendButton.isOn = true;
		}
		else
		{
			this.addFriendButton.isOn = false;
		}
		this.addFriendButton.UpdateColor();
	}

	// Token: 0x06003000 RID: 12288 RVA: 0x000E7CD8 File Offset: 0x000E5ED8
	private void UpdateDisplay(ref FriendingManager.FriendStationData data)
	{
		if (this.displayedData.actorNumberA != data.actorNumberA)
		{
			this.UpdatePlayerText(this.player1Text, data.actorNumberA);
		}
		if (this.displayedData.actorNumberB != data.actorNumberB)
		{
			this.UpdatePlayerText(this.player2Text, data.actorNumberB);
		}
		if (this.displayedData.state != data.state)
		{
			this.UpdateDisplayedState(data.state);
		}
		this.displayedData = data;
		this.UpdateAddFriendButton();
	}

	// Token: 0x06003001 RID: 12289 RVA: 0x000E7D60 File Offset: 0x000E5F60
	public void UpdateState(FriendingManager.FriendStationData data)
	{
		this.UpdateDisplay(ref data);
	}

	// Token: 0x06003002 RID: 12290 RVA: 0x000E7D6C File Offset: 0x000E5F6C
	public void TriggerEntered(TriggerEventNotifier notifier, Collider other)
	{
		if (PhotonNetwork.InRoom)
		{
			VRRig component = other.GetComponent<VRRig>();
			if (component != null && component.OwningNetPlayer != null)
			{
				this.addFriendButton.ResetState();
				FriendingManager.Instance.PlayerEnteredStation(this.zone, component.OwningNetPlayer);
				return;
			}
		}
		else if (other == GTPlayer.Instance.headCollider)
		{
			this.displayedData.state = FriendingManager.FriendStationState.NotInRoom;
			this.displayedData.actorNumberA = -2;
			this.displayedData.actorNumberB = -2;
			this.UpdateDisplayedState(this.displayedData.state);
			this.UpdatePlayerText(this.player1Text, this.displayedData.actorNumberA);
			this.UpdatePlayerText(this.player2Text, this.displayedData.actorNumberB);
			this.addFriendButton.ResetState();
		}
	}

	// Token: 0x06003003 RID: 12291 RVA: 0x000E7E44 File Offset: 0x000E6044
	public void TriggerExited(TriggerEventNotifier notifier, Collider other)
	{
		if (PhotonNetwork.InRoom)
		{
			VRRig component = other.GetComponent<VRRig>();
			if (component != null)
			{
				this.addFriendButton.ResetState();
				FriendingManager.Instance.PlayerExitedStation(this.zone, component.OwningNetPlayer);
				return;
			}
		}
		else if (other == GTPlayer.Instance.headCollider)
		{
			this.displayedData.state = FriendingManager.FriendStationState.NotInRoom;
			this.displayedData.actorNumberA = -2;
			this.displayedData.actorNumberB = -2;
			this.UpdateDisplayedState(this.displayedData.state);
			this.UpdatePlayerText(this.player1Text, this.displayedData.actorNumberA);
			this.UpdatePlayerText(this.player2Text, this.displayedData.actorNumberB);
			this.addFriendButton.ResetState();
		}
	}

	// Token: 0x06003004 RID: 12292 RVA: 0x000E7F14 File Offset: 0x000E6114
	public void FriendButtonPressed()
	{
		if (this.displayedData.state == FriendingManager.FriendStationState.WaitingForPlayers || this.displayedData.state == FriendingManager.FriendStationState.Friends)
		{
			return;
		}
		if (!this.addFriendButton.isOn)
		{
			FriendingManager.Instance.photonView.RPC("FriendButtonPressedRPC", RpcTarget.MasterClient, new object[]
			{
				this.zone
			});
			int actorNumber = NetworkSystem.Instance.LocalPlayer.ActorNumber;
			if (this.displayedData.state == FriendingManager.FriendStationState.WaitingOnButtonBoth || (this.displayedData.actorNumberA == actorNumber && this.displayedData.state == FriendingManager.FriendStationState.WaitingOnButtonPlayerA) || (this.displayedData.actorNumberB == actorNumber && this.displayedData.state == FriendingManager.FriendStationState.WaitingOnButtonPlayerB))
			{
				this.addFriendButton.isOn = true;
				this.addFriendButton.UpdateColor();
			}
		}
	}

	// Token: 0x06003005 RID: 12293 RVA: 0x000E7FE8 File Offset: 0x000E61E8
	public void FriendButtonReleased()
	{
		if (this.displayedData.state == FriendingManager.FriendStationState.WaitingForPlayers || this.displayedData.state == FriendingManager.FriendStationState.Friends)
		{
			return;
		}
		if (this.addFriendButton.isOn)
		{
			FriendingManager.Instance.photonView.RPC("FriendButtonUnpressedRPC", RpcTarget.MasterClient, new object[]
			{
				this.zone
			});
			int actorNumber = NetworkSystem.Instance.LocalPlayer.ActorNumber;
			if ((this.displayedData.state >= FriendingManager.FriendStationState.ButtonConfirmationTimer0 && this.displayedData.state <= FriendingManager.FriendStationState.ButtonConfirmationTimer4) || (this.displayedData.actorNumberA == actorNumber && this.displayedData.state == FriendingManager.FriendStationState.WaitingOnButtonPlayerB) || (this.displayedData.actorNumberB == actorNumber && this.displayedData.state == FriendingManager.FriendStationState.WaitingOnButtonPlayerA))
			{
				this.addFriendButton.isOn = false;
				this.addFriendButton.UpdateColor();
			}
		}
	}

	// Token: 0x040033F1 RID: 13297
	[SerializeField]
	private TriggerEventNotifier triggerNotifier;

	// Token: 0x040033F2 RID: 13298
	[SerializeField]
	private TextMeshProUGUI player1Text;

	// Token: 0x040033F3 RID: 13299
	[SerializeField]
	private TextMeshProUGUI player2Text;

	// Token: 0x040033F4 RID: 13300
	[SerializeField]
	private TextMeshProUGUI statusText;

	// Token: 0x040033F5 RID: 13301
	[SerializeField]
	private GTZone zone;

	// Token: 0x040033F6 RID: 13302
	[SerializeField]
	private GorillaPressableButton addFriendButton;

	// Token: 0x040033F7 RID: 13303
	private FriendingManager.FriendStationData displayedData;
}
