using System;
using System.Collections.Generic;
using GorillaLocomotion;
using GorillaNetworking;
using Photon.Pun;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

// Token: 0x020007A6 RID: 1958
public class FriendDisplay : MonoBehaviour
{
	// Token: 0x17000508 RID: 1288
	// (get) Token: 0x06003065 RID: 12389 RVA: 0x00050233 File Offset: 0x0004E433
	public bool InRemoveMode
	{
		get
		{
			return this.inRemoveMode;
		}
	}

	// Token: 0x06003066 RID: 12390 RVA: 0x0012EE48 File Offset: 0x0012D048
	private void Start()
	{
		this.InitFriendCards();
		this.InitLocalPlayerCard();
		this.UpdateLocalPlayerPrivacyButtons();
		this.triggerNotifier.TriggerEnterEvent += this.TriggerEntered;
		this.triggerNotifier.TriggerExitEvent += this.TriggerExited;
		NetworkSystem.Instance.OnJoinedRoomEvent += this.OnJoinedRoom;
	}

	// Token: 0x06003067 RID: 12391 RVA: 0x0012EEAC File Offset: 0x0012D0AC
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

	// Token: 0x06003068 RID: 12392 RVA: 0x0012EF18 File Offset: 0x0012D118
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

	// Token: 0x06003069 RID: 12393 RVA: 0x0012EF78 File Offset: 0x0012D178
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

	// Token: 0x0600306A RID: 12394 RVA: 0x0005023B File Offset: 0x0004E43B
	private void OnJoinedRoom()
	{
		this.Refresh();
	}

	// Token: 0x0600306B RID: 12395 RVA: 0x00050243 File Offset: 0x0004E443
	private void Refresh()
	{
		if (this.localPlayerAtDisplay)
		{
			FriendSystem.Instance.RefreshFriendsList();
			this.PopulateLocalPlayerCard();
		}
	}

	// Token: 0x0600306C RID: 12396 RVA: 0x0005025F File Offset: 0x0004E45F
	public void LocalPlayerFullyVisiblePress()
	{
		FriendSystem.Instance.SetLocalPlayerPrivacy(FriendSystem.PlayerPrivacy.Visible);
		this.UpdateLocalPlayerPrivacyButtons();
		this.PopulateLocalPlayerCard();
	}

	// Token: 0x0600306D RID: 12397 RVA: 0x0005027A File Offset: 0x0004E47A
	public void LocalPlayerPublicOnlyPress()
	{
		FriendSystem.Instance.SetLocalPlayerPrivacy(FriendSystem.PlayerPrivacy.PublicOnly);
		this.UpdateLocalPlayerPrivacyButtons();
		this.PopulateLocalPlayerCard();
	}

	// Token: 0x0600306E RID: 12398 RVA: 0x00050295 File Offset: 0x0004E495
	public void LocalPlayerFullyHiddenPress()
	{
		FriendSystem.Instance.SetLocalPlayerPrivacy(FriendSystem.PlayerPrivacy.Hidden);
		this.UpdateLocalPlayerPrivacyButtons();
		this.PopulateLocalPlayerCard();
	}

	// Token: 0x0600306F RID: 12399 RVA: 0x0012EFD0 File Offset: 0x0012D1D0
	private void UpdateLocalPlayerPrivacyButtons()
	{
		FriendSystem.PlayerPrivacy localPlayerPrivacy = FriendSystem.Instance.LocalPlayerPrivacy;
		this.SetButtonAppearance(this._localPlayerFullyVisibleButton, localPlayerPrivacy == FriendSystem.PlayerPrivacy.Visible);
		this.SetButtonAppearance(this._localPlayerPublicOnlyButton, localPlayerPrivacy == FriendSystem.PlayerPrivacy.PublicOnly);
		this.SetButtonAppearance(this._localPlayerFullyHiddenButton, localPlayerPrivacy == FriendSystem.PlayerPrivacy.Hidden);
	}

	// Token: 0x06003070 RID: 12400 RVA: 0x000502B0 File Offset: 0x0004E4B0
	private void SetButtonAppearance(MeshRenderer buttonRenderer, bool active)
	{
		this.SetButtonAppearance(buttonRenderer, active ? FriendDisplay.ButtonState.Active : FriendDisplay.ButtonState.Default);
	}

	// Token: 0x06003071 RID: 12401 RVA: 0x0012F01C File Offset: 0x0012D21C
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

	// Token: 0x06003072 RID: 12402 RVA: 0x0012F074 File Offset: 0x0012D274
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

	// Token: 0x06003073 RID: 12403 RVA: 0x0012F0CC File Offset: 0x0012D2CC
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

	// Token: 0x06003074 RID: 12404 RVA: 0x0012F26C File Offset: 0x0012D46C
	public void RandomizeFriendCards()
	{
		for (int i = 0; i < this.friendCards.Length; i++)
		{
			this.friendCards[i].Randomize();
		}
	}

	// Token: 0x06003075 RID: 12405 RVA: 0x0012F29C File Offset: 0x0012D49C
	private void ClearFriendCards()
	{
		for (int i = 0; i < this.friendCards.Length; i++)
		{
			this.friendCards[i].SetEmpty();
		}
	}

	// Token: 0x06003076 RID: 12406 RVA: 0x000502C0 File Offset: 0x0004E4C0
	public void OnGetFriendsReceived(List<FriendBackendController.Friend> friendsList)
	{
		this.PopulateFriendCards(friendsList);
		this.UpdateLocalPlayerPrivacyButtons();
		this.PopulateLocalPlayerCard();
	}

	// Token: 0x06003077 RID: 12407 RVA: 0x0012F2CC File Offset: 0x0012D4CC
	private void PopulateFriendCards(List<FriendBackendController.Friend> friendsList)
	{
		int num = 0;
		while (num < friendsList.Count && friendsList[num] != null)
		{
			this.friendCards[num].Populate(friendsList[num]);
			num++;
		}
	}

	// Token: 0x06003078 RID: 12408 RVA: 0x000502D5 File Offset: 0x0004E4D5
	private void InitLocalPlayerCard()
	{
		this._localPlayerCard.Init(this);
		this.ClearLocalPlayerCard();
	}

	// Token: 0x06003079 RID: 12409 RVA: 0x0012F308 File Offset: 0x0012D508
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

	// Token: 0x0600307A RID: 12410 RVA: 0x000502E9 File Offset: 0x0004E4E9
	private void ClearLocalPlayerCard()
	{
		this._localPlayerCard.SetEmpty();
	}

	// Token: 0x0600307B RID: 12411 RVA: 0x0012F498 File Offset: 0x0012D698
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

	// Token: 0x0400345F RID: 13407
	[FormerlySerializedAs("gridCenter")]
	[SerializeField]
	private FriendCard[] friendCards = new FriendCard[9];

	// Token: 0x04003460 RID: 13408
	[SerializeField]
	private Transform gridRoot;

	// Token: 0x04003461 RID: 13409
	[SerializeField]
	private float gridWidth = 2f;

	// Token: 0x04003462 RID: 13410
	[SerializeField]
	private float gridHeight = 1f;

	// Token: 0x04003463 RID: 13411
	[SerializeField]
	private int gridDimension = 3;

	// Token: 0x04003464 RID: 13412
	[SerializeField]
	private TriggerEventNotifier triggerNotifier;

	// Token: 0x04003465 RID: 13413
	[FormerlySerializedAs("_joinButtons")]
	[Header("Buttons")]
	[SerializeField]
	private GorillaPressableDelayButton[] _friendCardButtons;

	// Token: 0x04003466 RID: 13414
	[SerializeField]
	private TextMeshProUGUI[] _friendCardButtonText;

	// Token: 0x04003467 RID: 13415
	[SerializeField]
	private MeshRenderer _localPlayerFullyVisibleButton;

	// Token: 0x04003468 RID: 13416
	[SerializeField]
	private MeshRenderer _localPlayerPublicOnlyButton;

	// Token: 0x04003469 RID: 13417
	[SerializeField]
	private MeshRenderer _localPlayerFullyHiddenButton;

	// Token: 0x0400346A RID: 13418
	[SerializeField]
	private MeshRenderer _removeFriendButton;

	// Token: 0x0400346B RID: 13419
	[SerializeField]
	private FriendCard _localPlayerCard;

	// Token: 0x0400346C RID: 13420
	[SerializeField]
	private Material[] _buttonDefaultMaterials;

	// Token: 0x0400346D RID: 13421
	[SerializeField]
	private Material[] _buttonActiveMaterials;

	// Token: 0x0400346E RID: 13422
	[SerializeField]
	private Material[] _buttonAlertMaterials;

	// Token: 0x0400346F RID: 13423
	private MeshRenderer[] _joinButtonRenderers;

	// Token: 0x04003470 RID: 13424
	private bool inRemoveMode;

	// Token: 0x04003471 RID: 13425
	private bool localPlayerAtDisplay;

	// Token: 0x020007A7 RID: 1959
	public enum ButtonState
	{
		// Token: 0x04003473 RID: 13427
		Default,
		// Token: 0x04003474 RID: 13428
		Active,
		// Token: 0x04003475 RID: 13429
		Alert
	}
}
