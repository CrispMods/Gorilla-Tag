using System;
using System.Collections.Generic;
using GorillaLocomotion;
using GorillaNetworking;
using Photon.Pun;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

// Token: 0x0200078E RID: 1934
public class FriendDisplay : MonoBehaviour
{
	// Token: 0x170004FA RID: 1274
	// (get) Token: 0x06002FB3 RID: 12211 RVA: 0x000E55B5 File Offset: 0x000E37B5
	public bool InRemoveMode
	{
		get
		{
			return this.inRemoveMode;
		}
	}

	// Token: 0x06002FB4 RID: 12212 RVA: 0x000E55C0 File Offset: 0x000E37C0
	private void Start()
	{
		this.InitFriendCards();
		this.InitLocalPlayerCard();
		this.UpdateLocalPlayerPrivacyButtons();
		this.triggerNotifier.TriggerEnterEvent += this.TriggerEntered;
		this.triggerNotifier.TriggerExitEvent += this.TriggerExited;
		NetworkSystem.Instance.OnJoinedRoomEvent += this.OnJoinedRoom;
	}

	// Token: 0x06002FB5 RID: 12213 RVA: 0x000E5624 File Offset: 0x000E3824
	private void OnDestroy()
	{
		if (NetworkSystem.Instance != null)
		{
			NetworkSystem.Instance.OnJoinedRoomEvent -= this.OnJoinedRoom;
		}
		if (this.triggerNotifier != null)
		{
			this.triggerNotifier.TriggerEnterEvent -= this.TriggerEntered;
			this.triggerNotifier.TriggerExitEvent -= this.TriggerExited;
		}
	}

	// Token: 0x06002FB6 RID: 12214 RVA: 0x000E5690 File Offset: 0x000E3890
	public void TriggerEntered(TriggerEventNotifier notifier, Collider other)
	{
		if (other == GTPlayer.Instance.headCollider)
		{
			FriendSystem.Instance.OnFriendListRefresh += this.OnGetFriendsReceived;
			FriendSystem.Instance.RefreshFriendsList();
			this.PopulateLocalPlayerCard();
			this.localPlayerAtDisplay = true;
			if (this.InRemoveMode)
			{
				this.ToggleRemoveFriendMode();
			}
		}
	}

	// Token: 0x06002FB7 RID: 12215 RVA: 0x000E56F0 File Offset: 0x000E38F0
	public void TriggerExited(TriggerEventNotifier notifier, Collider other)
	{
		if (other == GTPlayer.Instance.headCollider)
		{
			FriendSystem.Instance.OnFriendListRefresh -= this.OnGetFriendsReceived;
			this.ClearFriendCards();
			this.ClearLocalPlayerCard();
			this.localPlayerAtDisplay = false;
			if (this.InRemoveMode)
			{
				this.ToggleRemoveFriendMode();
			}
		}
	}

	// Token: 0x06002FB8 RID: 12216 RVA: 0x000E5748 File Offset: 0x000E3948
	private void OnJoinedRoom()
	{
		this.Refresh();
	}

	// Token: 0x06002FB9 RID: 12217 RVA: 0x000E5750 File Offset: 0x000E3950
	private void Refresh()
	{
		if (this.localPlayerAtDisplay)
		{
			FriendSystem.Instance.RefreshFriendsList();
			this.PopulateLocalPlayerCard();
		}
	}

	// Token: 0x06002FBA RID: 12218 RVA: 0x000E576C File Offset: 0x000E396C
	public void LocalPlayerFullyVisiblePress()
	{
		FriendSystem.Instance.SetLocalPlayerPrivacy(FriendSystem.PlayerPrivacy.Visible);
		this.UpdateLocalPlayerPrivacyButtons();
		this.PopulateLocalPlayerCard();
	}

	// Token: 0x06002FBB RID: 12219 RVA: 0x000E5787 File Offset: 0x000E3987
	public void LocalPlayerPublicOnlyPress()
	{
		FriendSystem.Instance.SetLocalPlayerPrivacy(FriendSystem.PlayerPrivacy.PublicOnly);
		this.UpdateLocalPlayerPrivacyButtons();
		this.PopulateLocalPlayerCard();
	}

	// Token: 0x06002FBC RID: 12220 RVA: 0x000E57A2 File Offset: 0x000E39A2
	public void LocalPlayerFullyHiddenPress()
	{
		FriendSystem.Instance.SetLocalPlayerPrivacy(FriendSystem.PlayerPrivacy.Hidden);
		this.UpdateLocalPlayerPrivacyButtons();
		this.PopulateLocalPlayerCard();
	}

	// Token: 0x06002FBD RID: 12221 RVA: 0x000E57C0 File Offset: 0x000E39C0
	private void UpdateLocalPlayerPrivacyButtons()
	{
		FriendSystem.PlayerPrivacy localPlayerPrivacy = FriendSystem.Instance.LocalPlayerPrivacy;
		this.SetButtonAppearance(this._localPlayerFullyVisibleButton, localPlayerPrivacy == FriendSystem.PlayerPrivacy.Visible);
		this.SetButtonAppearance(this._localPlayerPublicOnlyButton, localPlayerPrivacy == FriendSystem.PlayerPrivacy.PublicOnly);
		this.SetButtonAppearance(this._localPlayerFullyHiddenButton, localPlayerPrivacy == FriendSystem.PlayerPrivacy.Hidden);
	}

	// Token: 0x06002FBE RID: 12222 RVA: 0x000E580A File Offset: 0x000E3A0A
	private void SetButtonAppearance(MeshRenderer buttonRenderer, bool active)
	{
		this.SetButtonAppearance(buttonRenderer, active ? FriendDisplay.ButtonState.Active : FriendDisplay.ButtonState.Default);
	}

	// Token: 0x06002FBF RID: 12223 RVA: 0x000E581C File Offset: 0x000E3A1C
	private void SetButtonAppearance(MeshRenderer buttonRenderer, FriendDisplay.ButtonState state)
	{
		Material[] sharedMaterials;
		switch (state)
		{
		case FriendDisplay.ButtonState.Default:
			sharedMaterials = this._buttonDefaultMaterials;
			break;
		case FriendDisplay.ButtonState.Active:
			sharedMaterials = this._buttonActiveMaterials;
			break;
		case FriendDisplay.ButtonState.Alert:
			sharedMaterials = this._buttonAlertMaterials;
			break;
		default:
			throw new ArgumentOutOfRangeException("state", state, null);
		}
		buttonRenderer.sharedMaterials = sharedMaterials;
	}

	// Token: 0x06002FC0 RID: 12224 RVA: 0x000E5874 File Offset: 0x000E3A74
	public void ToggleRemoveFriendMode()
	{
		this.inRemoveMode = !this.inRemoveMode;
		FriendCard[] array = this.friendCards;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].SetRemoveEnabled(this.inRemoveMode);
		}
		this.SetButtonAppearance(this._removeFriendButton, this.inRemoveMode ? FriendDisplay.ButtonState.Alert : FriendDisplay.ButtonState.Default);
	}

	// Token: 0x06002FC1 RID: 12225 RVA: 0x000E58CC File Offset: 0x000E3ACC
	private void InitFriendCards()
	{
		float num = this.gridWidth / (float)this.gridDimension;
		float num2 = this.gridHeight / (float)this.gridDimension;
		Vector3 right = this.gridRoot.right;
		Vector3 a = -this.gridRoot.up;
		Vector3 a2 = this.gridRoot.position - right * (this.gridWidth * 0.5f - num * 0.5f) - a * (this.gridHeight * 0.5f - num2 * 0.5f);
		int num3 = 0;
		int num4 = 0;
		for (int i = 0; i < this.gridDimension; i++)
		{
			for (int j = 0; j < this.gridDimension; j++)
			{
				FriendCard friendCard = this.friendCards[num4];
				friendCard.gameObject.SetActive(true);
				friendCard.transform.localScale = Vector3.one * (num / friendCard.Width);
				friendCard.transform.position = a2 + right * num * (float)j + a * num2 * (float)i;
				friendCard.transform.rotation = this.gridRoot.transform.rotation;
				friendCard.Init(this);
				friendCard.SetButton(this._friendCardButtons[num3++], this._buttonDefaultMaterials, this._buttonActiveMaterials, this._buttonAlertMaterials, this._friendCardButtonText[num4]);
				friendCard.SetEmpty();
				num4++;
			}
		}
	}

	// Token: 0x06002FC2 RID: 12226 RVA: 0x000E5A6C File Offset: 0x000E3C6C
	public void RandomizeFriendCards()
	{
		for (int i = 0; i < this.friendCards.Length; i++)
		{
			this.friendCards[i].Randomize();
		}
	}

	// Token: 0x06002FC3 RID: 12227 RVA: 0x000E5A9C File Offset: 0x000E3C9C
	private void ClearFriendCards()
	{
		for (int i = 0; i < this.friendCards.Length; i++)
		{
			this.friendCards[i].SetEmpty();
		}
	}

	// Token: 0x06002FC4 RID: 12228 RVA: 0x000E5AC9 File Offset: 0x000E3CC9
	public void OnGetFriendsReceived(List<FriendBackendController.Friend> friendsList)
	{
		this.PopulateFriendCards(friendsList);
		this.UpdateLocalPlayerPrivacyButtons();
		this.PopulateLocalPlayerCard();
	}

	// Token: 0x06002FC5 RID: 12229 RVA: 0x000E5AE0 File Offset: 0x000E3CE0
	private void PopulateFriendCards(List<FriendBackendController.Friend> friendsList)
	{
		int num = 0;
		while (num < friendsList.Count && friendsList[num] != null)
		{
			this.friendCards[num].Populate(friendsList[num]);
			num++;
		}
	}

	// Token: 0x06002FC6 RID: 12230 RVA: 0x000E5B1B File Offset: 0x000E3D1B
	private void InitLocalPlayerCard()
	{
		this._localPlayerCard.Init(this);
		this.ClearLocalPlayerCard();
	}

	// Token: 0x06002FC7 RID: 12231 RVA: 0x000E5B30 File Offset: 0x000E3D30
	private void PopulateLocalPlayerCard()
	{
		string zone = PhotonNetworkController.Instance.CurrentRoomZone.GetName<GTZone>().ToUpper();
		this._localPlayerCard.SetName(NetworkSystem.Instance.LocalPlayer.NickName.ToUpper());
		if (!PhotonNetwork.InRoom || string.IsNullOrEmpty(NetworkSystem.Instance.RoomName) || NetworkSystem.Instance.RoomName.Length <= 0)
		{
			this._localPlayerCard.SetRoom("OFFLINE");
			this._localPlayerCard.SetZone("");
			return;
		}
		bool flag = NetworkSystem.Instance.RoomName[0] == '@';
		bool flag2 = !NetworkSystem.Instance.SessionIsPrivate;
		if (FriendSystem.Instance.LocalPlayerPrivacy == FriendSystem.PlayerPrivacy.Hidden || (FriendSystem.Instance.LocalPlayerPrivacy == FriendSystem.PlayerPrivacy.PublicOnly && !flag2))
		{
			this._localPlayerCard.SetRoom("OFFLINE");
			this._localPlayerCard.SetZone("");
			return;
		}
		if (flag)
		{
			this._localPlayerCard.SetRoom(NetworkSystem.Instance.RoomName.Substring(1).ToUpper());
			this._localPlayerCard.SetZone("CUSTOM");
			return;
		}
		if (!flag2)
		{
			this._localPlayerCard.SetRoom(NetworkSystem.Instance.RoomName.ToUpper());
			this._localPlayerCard.SetZone("PRIVATE");
			return;
		}
		this._localPlayerCard.SetRoom(NetworkSystem.Instance.RoomName.ToUpper());
		this._localPlayerCard.SetZone(zone);
	}

	// Token: 0x06002FC8 RID: 12232 RVA: 0x000E5CBD File Offset: 0x000E3EBD
	private void ClearLocalPlayerCard()
	{
		this._localPlayerCard.SetEmpty();
	}

	// Token: 0x06002FC9 RID: 12233 RVA: 0x000E5CCC File Offset: 0x000E3ECC
	private void OnDrawGizmosSelected()
	{
		Gizmos.color = Color.white;
		float num = this.gridWidth * 0.5f;
		float num2 = this.gridHeight * 0.5f;
		float num3 = num;
		float num4 = num2;
		Vector3 a = this.gridRoot.position + this.gridRoot.rotation * new Vector3(-num3, num4, 0f);
		Vector3 vector = this.gridRoot.position + this.gridRoot.rotation * new Vector3(num3, num4, 0f);
		Vector3 vector2 = this.gridRoot.position + this.gridRoot.rotation * new Vector3(-num3, -num4, 0f);
		Vector3 b = this.gridRoot.position + this.gridRoot.rotation * new Vector3(num3, -num4, 0f);
		for (int i = 0; i <= this.gridDimension; i++)
		{
			float t = (float)i / (float)this.gridDimension;
			Vector3 from = Vector3.Lerp(a, vector, t);
			Vector3 to = Vector3.Lerp(vector2, b, t);
			Gizmos.DrawLine(from, to);
			Vector3 from2 = Vector3.Lerp(a, vector2, t);
			Vector3 to2 = Vector3.Lerp(vector, b, t);
			Gizmos.DrawLine(from2, to2);
		}
	}

	// Token: 0x040033B5 RID: 13237
	[FormerlySerializedAs("gridCenter")]
	[SerializeField]
	private FriendCard[] friendCards = new FriendCard[9];

	// Token: 0x040033B6 RID: 13238
	[SerializeField]
	private Transform gridRoot;

	// Token: 0x040033B7 RID: 13239
	[SerializeField]
	private float gridWidth = 2f;

	// Token: 0x040033B8 RID: 13240
	[SerializeField]
	private float gridHeight = 1f;

	// Token: 0x040033B9 RID: 13241
	[SerializeField]
	private int gridDimension = 3;

	// Token: 0x040033BA RID: 13242
	[SerializeField]
	private TriggerEventNotifier triggerNotifier;

	// Token: 0x040033BB RID: 13243
	[FormerlySerializedAs("_joinButtons")]
	[Header("Buttons")]
	[SerializeField]
	private GorillaPressableDelayButton[] _friendCardButtons;

	// Token: 0x040033BC RID: 13244
	[SerializeField]
	private TextMeshProUGUI[] _friendCardButtonText;

	// Token: 0x040033BD RID: 13245
	[SerializeField]
	private MeshRenderer _localPlayerFullyVisibleButton;

	// Token: 0x040033BE RID: 13246
	[SerializeField]
	private MeshRenderer _localPlayerPublicOnlyButton;

	// Token: 0x040033BF RID: 13247
	[SerializeField]
	private MeshRenderer _localPlayerFullyHiddenButton;

	// Token: 0x040033C0 RID: 13248
	[SerializeField]
	private MeshRenderer _removeFriendButton;

	// Token: 0x040033C1 RID: 13249
	[SerializeField]
	private FriendCard _localPlayerCard;

	// Token: 0x040033C2 RID: 13250
	[SerializeField]
	private Material[] _buttonDefaultMaterials;

	// Token: 0x040033C3 RID: 13251
	[SerializeField]
	private Material[] _buttonActiveMaterials;

	// Token: 0x040033C4 RID: 13252
	[SerializeField]
	private Material[] _buttonAlertMaterials;

	// Token: 0x040033C5 RID: 13253
	private MeshRenderer[] _joinButtonRenderers;

	// Token: 0x040033C6 RID: 13254
	private bool inRemoveMode;

	// Token: 0x040033C7 RID: 13255
	private bool localPlayerAtDisplay;

	// Token: 0x0200078F RID: 1935
	public enum ButtonState
	{
		// Token: 0x040033C9 RID: 13257
		Default,
		// Token: 0x040033CA RID: 13258
		Active,
		// Token: 0x040033CB RID: 13259
		Alert
	}
}
