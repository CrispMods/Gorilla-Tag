using System;
using System.Collections.Generic;
using GorillaLocomotion;
using GorillaNetworking;
using Photon.Pun;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

// Token: 0x0200078F RID: 1935
public class FriendDisplay : MonoBehaviour
{
	// Token: 0x170004FB RID: 1275
	// (get) Token: 0x06002FBB RID: 12219 RVA: 0x0004EE31 File Offset: 0x0004D031
	public bool InRemoveMode
	{
		get
		{
			return this.inRemoveMode;
		}
	}

	// Token: 0x06002FBC RID: 12220 RVA: 0x00129C28 File Offset: 0x00127E28
	private void Start()
	{
		this.InitFriendCards();
		this.InitLocalPlayerCard();
		this.UpdateLocalPlayerPrivacyButtons();
		this.triggerNotifier.TriggerEnterEvent += this.TriggerEntered;
		this.triggerNotifier.TriggerExitEvent += this.TriggerExited;
		NetworkSystem.Instance.OnJoinedRoomEvent += this.OnJoinedRoom;
	}

	// Token: 0x06002FBD RID: 12221 RVA: 0x00129C8C File Offset: 0x00127E8C
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

	// Token: 0x06002FBE RID: 12222 RVA: 0x00129CF8 File Offset: 0x00127EF8
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

	// Token: 0x06002FBF RID: 12223 RVA: 0x00129D58 File Offset: 0x00127F58
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

	// Token: 0x06002FC0 RID: 12224 RVA: 0x0004EE39 File Offset: 0x0004D039
	private void OnJoinedRoom()
	{
		this.Refresh();
	}

	// Token: 0x06002FC1 RID: 12225 RVA: 0x0004EE41 File Offset: 0x0004D041
	private void Refresh()
	{
		if (this.localPlayerAtDisplay)
		{
			FriendSystem.Instance.RefreshFriendsList();
			this.PopulateLocalPlayerCard();
		}
	}

	// Token: 0x06002FC2 RID: 12226 RVA: 0x0004EE5D File Offset: 0x0004D05D
	public void LocalPlayerFullyVisiblePress()
	{
		FriendSystem.Instance.SetLocalPlayerPrivacy(FriendSystem.PlayerPrivacy.Visible);
		this.UpdateLocalPlayerPrivacyButtons();
		this.PopulateLocalPlayerCard();
	}

	// Token: 0x06002FC3 RID: 12227 RVA: 0x0004EE78 File Offset: 0x0004D078
	public void LocalPlayerPublicOnlyPress()
	{
		FriendSystem.Instance.SetLocalPlayerPrivacy(FriendSystem.PlayerPrivacy.PublicOnly);
		this.UpdateLocalPlayerPrivacyButtons();
		this.PopulateLocalPlayerCard();
	}

	// Token: 0x06002FC4 RID: 12228 RVA: 0x0004EE93 File Offset: 0x0004D093
	public void LocalPlayerFullyHiddenPress()
	{
		FriendSystem.Instance.SetLocalPlayerPrivacy(FriendSystem.PlayerPrivacy.Hidden);
		this.UpdateLocalPlayerPrivacyButtons();
		this.PopulateLocalPlayerCard();
	}

	// Token: 0x06002FC5 RID: 12229 RVA: 0x00129DB0 File Offset: 0x00127FB0
	private void UpdateLocalPlayerPrivacyButtons()
	{
		FriendSystem.PlayerPrivacy localPlayerPrivacy = FriendSystem.Instance.LocalPlayerPrivacy;
		this.SetButtonAppearance(this._localPlayerFullyVisibleButton, localPlayerPrivacy == FriendSystem.PlayerPrivacy.Visible);
		this.SetButtonAppearance(this._localPlayerPublicOnlyButton, localPlayerPrivacy == FriendSystem.PlayerPrivacy.PublicOnly);
		this.SetButtonAppearance(this._localPlayerFullyHiddenButton, localPlayerPrivacy == FriendSystem.PlayerPrivacy.Hidden);
	}

	// Token: 0x06002FC6 RID: 12230 RVA: 0x0004EEAE File Offset: 0x0004D0AE
	private void SetButtonAppearance(MeshRenderer buttonRenderer, bool active)
	{
		this.SetButtonAppearance(buttonRenderer, active ? FriendDisplay.ButtonState.Active : FriendDisplay.ButtonState.Default);
	}

	// Token: 0x06002FC7 RID: 12231 RVA: 0x00129DFC File Offset: 0x00127FFC
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

	// Token: 0x06002FC8 RID: 12232 RVA: 0x00129E54 File Offset: 0x00128054
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

	// Token: 0x06002FC9 RID: 12233 RVA: 0x00129EAC File Offset: 0x001280AC
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

	// Token: 0x06002FCA RID: 12234 RVA: 0x0012A04C File Offset: 0x0012824C
	public void RandomizeFriendCards()
	{
		for (int i = 0; i < this.friendCards.Length; i++)
		{
			this.friendCards[i].Randomize();
		}
	}

	// Token: 0x06002FCB RID: 12235 RVA: 0x0012A07C File Offset: 0x0012827C
	private void ClearFriendCards()
	{
		for (int i = 0; i < this.friendCards.Length; i++)
		{
			this.friendCards[i].SetEmpty();
		}
	}

	// Token: 0x06002FCC RID: 12236 RVA: 0x0004EEBE File Offset: 0x0004D0BE
	public void OnGetFriendsReceived(List<FriendBackendController.Friend> friendsList)
	{
		this.PopulateFriendCards(friendsList);
		this.UpdateLocalPlayerPrivacyButtons();
		this.PopulateLocalPlayerCard();
	}

	// Token: 0x06002FCD RID: 12237 RVA: 0x0012A0AC File Offset: 0x001282AC
	private void PopulateFriendCards(List<FriendBackendController.Friend> friendsList)
	{
		int num = 0;
		while (num < friendsList.Count && friendsList[num] != null)
		{
			this.friendCards[num].Populate(friendsList[num]);
			num++;
		}
	}

	// Token: 0x06002FCE RID: 12238 RVA: 0x0004EED3 File Offset: 0x0004D0D3
	private void InitLocalPlayerCard()
	{
		this._localPlayerCard.Init(this);
		this.ClearLocalPlayerCard();
	}

	// Token: 0x06002FCF RID: 12239 RVA: 0x0012A0E8 File Offset: 0x001282E8
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

	// Token: 0x06002FD0 RID: 12240 RVA: 0x0004EEE7 File Offset: 0x0004D0E7
	private void ClearLocalPlayerCard()
	{
		this._localPlayerCard.SetEmpty();
	}

	// Token: 0x06002FD1 RID: 12241 RVA: 0x0012A278 File Offset: 0x00128478
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

	// Token: 0x040033BB RID: 13243
	[FormerlySerializedAs("gridCenter")]
	[SerializeField]
	private FriendCard[] friendCards = new FriendCard[9];

	// Token: 0x040033BC RID: 13244
	[SerializeField]
	private Transform gridRoot;

	// Token: 0x040033BD RID: 13245
	[SerializeField]
	private float gridWidth = 2f;

	// Token: 0x040033BE RID: 13246
	[SerializeField]
	private float gridHeight = 1f;

	// Token: 0x040033BF RID: 13247
	[SerializeField]
	private int gridDimension = 3;

	// Token: 0x040033C0 RID: 13248
	[SerializeField]
	private TriggerEventNotifier triggerNotifier;

	// Token: 0x040033C1 RID: 13249
	[FormerlySerializedAs("_joinButtons")]
	[Header("Buttons")]
	[SerializeField]
	private GorillaPressableDelayButton[] _friendCardButtons;

	// Token: 0x040033C2 RID: 13250
	[SerializeField]
	private TextMeshProUGUI[] _friendCardButtonText;

	// Token: 0x040033C3 RID: 13251
	[SerializeField]
	private MeshRenderer _localPlayerFullyVisibleButton;

	// Token: 0x040033C4 RID: 13252
	[SerializeField]
	private MeshRenderer _localPlayerPublicOnlyButton;

	// Token: 0x040033C5 RID: 13253
	[SerializeField]
	private MeshRenderer _localPlayerFullyHiddenButton;

	// Token: 0x040033C6 RID: 13254
	[SerializeField]
	private MeshRenderer _removeFriendButton;

	// Token: 0x040033C7 RID: 13255
	[SerializeField]
	private FriendCard _localPlayerCard;

	// Token: 0x040033C8 RID: 13256
	[SerializeField]
	private Material[] _buttonDefaultMaterials;

	// Token: 0x040033C9 RID: 13257
	[SerializeField]
	private Material[] _buttonActiveMaterials;

	// Token: 0x040033CA RID: 13258
	[SerializeField]
	private Material[] _buttonAlertMaterials;

	// Token: 0x040033CB RID: 13259
	private MeshRenderer[] _joinButtonRenderers;

	// Token: 0x040033CC RID: 13260
	private bool inRemoveMode;

	// Token: 0x040033CD RID: 13261
	private bool localPlayerAtDisplay;

	// Token: 0x02000790 RID: 1936
	public enum ButtonState
	{
		// Token: 0x040033CF RID: 13263
		Default,
		// Token: 0x040033D0 RID: 13264
		Active,
		// Token: 0x040033D1 RID: 13265
		Alert
	}
}
