using System;
using System.Runtime.CompilerServices;
using GorillaNetworking;
using TMPro;
using UnityEngine;

// Token: 0x0200078E RID: 1934
public class FriendCard : MonoBehaviour
{
	// Token: 0x170004F6 RID: 1270
	// (get) Token: 0x06002F9F RID: 12191 RVA: 0x000E4F84 File Offset: 0x000E3184
	public TextMeshProUGUI NameText
	{
		get
		{
			return this.nameText;
		}
	}

	// Token: 0x170004F7 RID: 1271
	// (get) Token: 0x06002FA0 RID: 12192 RVA: 0x000E4F8C File Offset: 0x000E318C
	public TextMeshProUGUI RoomText
	{
		get
		{
			return this.roomText;
		}
	}

	// Token: 0x170004F8 RID: 1272
	// (get) Token: 0x06002FA1 RID: 12193 RVA: 0x000E4F94 File Offset: 0x000E3194
	public TextMeshProUGUI ZoneText
	{
		get
		{
			return this.zoneText;
		}
	}

	// Token: 0x170004F9 RID: 1273
	// (get) Token: 0x06002FA2 RID: 12194 RVA: 0x000E4F9C File Offset: 0x000E319C
	public float Width
	{
		get
		{
			return this.width;
		}
	}

	// Token: 0x170004FA RID: 1274
	// (get) Token: 0x06002FA3 RID: 12195 RVA: 0x000E4FA4 File Offset: 0x000E31A4
	// (set) Token: 0x06002FA4 RID: 12196 RVA: 0x000E4FAC File Offset: 0x000E31AC
	public float Height { get; private set; } = 0.25f;

	// Token: 0x06002FA5 RID: 12197 RVA: 0x000E4FB5 File Offset: 0x000E31B5
	private void Awake()
	{
		if (this.removeProgressBar)
		{
			this.removeProgressBar.gameObject.SetActive(false);
		}
	}

	// Token: 0x06002FA6 RID: 12198 RVA: 0x000E4FD5 File Offset: 0x000E31D5
	private void OnDestroy()
	{
		if (this._button)
		{
			this._button.onPressed -= this.OnButtonPressed;
		}
	}

	// Token: 0x06002FA7 RID: 12199 RVA: 0x000E4FFB File Offset: 0x000E31FB
	public void Init(FriendDisplay owner)
	{
		this.friendDisplay = owner;
	}

	// Token: 0x06002FA8 RID: 12200 RVA: 0x000E5004 File Offset: 0x000E3204
	private void UpdateComponentStates()
	{
		if (this.removeProgressBar)
		{
			this.removeProgressBar.gameObject.SetActive(this.canRemove);
		}
		if (this.canRemove)
		{
			this.SetButtonState((this.currentFriend != null) ? FriendDisplay.ButtonState.Alert : FriendDisplay.ButtonState.Default);
			return;
		}
		if (this.joinable)
		{
			this.SetButtonState(FriendDisplay.ButtonState.Active);
			return;
		}
		this.SetButtonState(FriendDisplay.ButtonState.Default);
	}

	// Token: 0x06002FA9 RID: 12201 RVA: 0x000E5068 File Offset: 0x000E3268
	private void SetButtonState(FriendDisplay.ButtonState newState)
	{
		if (this._button == null)
		{
			return;
		}
		if (this._buttonState == newState)
		{
			return;
		}
		this._buttonState = newState;
		MeshRenderer buttonRenderer = this._button.buttonRenderer;
		FriendDisplay.ButtonState buttonState = this._buttonState;
		Material[] sharedMaterials;
		switch (buttonState)
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
			throw new SwitchExpressionException(buttonState);
		}
		buttonRenderer.sharedMaterials = sharedMaterials;
		this._button.delayTime = (float)((this._buttonState == FriendDisplay.ButtonState.Alert) ? 3 : 0);
	}

	// Token: 0x06002FAA RID: 12202 RVA: 0x000E5104 File Offset: 0x000E3304
	public void Populate(FriendBackendController.Friend friend)
	{
		this.SetEmpty();
		if (friend != null && friend.Presence != null)
		{
			if (friend.Presence.UserName != null)
			{
				this.SetName(friend.Presence.UserName.ToUpper());
			}
			if (!string.IsNullOrEmpty(friend.Presence.RoomId) && friend.Presence.RoomId.Length > 0)
			{
				bool? isPublic = friend.Presence.IsPublic;
				bool flag = true;
				bool flag2 = isPublic.GetValueOrDefault() == flag & isPublic != null;
				bool flag3 = friend.Presence.RoomId[0] == '@';
				bool flag4 = friend.Presence.RoomId.Equals(NetworkSystem.Instance.RoomName);
				bool flag5 = false;
				if (!flag4 && flag2 && !friend.Presence.Zone.IsNullOrEmpty())
				{
					string text = friend.Presence.Zone.ToLower();
					foreach (GTZone e in ZoneManagement.instance.activeZones)
					{
						if (text.Contains(e.GetName<GTZone>().ToLower()))
						{
							flag5 = true;
						}
					}
				}
				this.joinable = (!flag3 && !flag4 && (!flag2 || flag5));
				if (flag3)
				{
					this.SetRoom(friend.Presence.RoomId.Substring(1).ToUpper());
					this.SetZone("CUSTOM");
				}
				else if (!flag2)
				{
					this.SetRoom(friend.Presence.RoomId.ToUpper());
					this.SetZone("PRIVATE");
				}
				else if (friend.Presence.Zone != null)
				{
					this.SetRoom(friend.Presence.RoomId.ToUpper());
					this.SetZone(friend.Presence.Zone.ToUpper());
				}
			}
			else
			{
				this.joinable = false;
				this.SetRoom("OFFLINE");
			}
			this.currentFriend = friend;
		}
		this.UpdateComponentStates();
	}

	// Token: 0x06002FAB RID: 12203 RVA: 0x000E5318 File Offset: 0x000E3518
	public void SetName(string friendName)
	{
		TMP_Text tmp_Text = this.nameText;
		this._friendName = friendName;
		tmp_Text.text = friendName;
	}

	// Token: 0x06002FAC RID: 12204 RVA: 0x000E533C File Offset: 0x000E353C
	public void SetRoom(string friendRoom)
	{
		TMP_Text tmp_Text = this.roomText;
		this._friendRoom = friendRoom;
		tmp_Text.text = friendRoom;
	}

	// Token: 0x06002FAD RID: 12205 RVA: 0x000E5360 File Offset: 0x000E3560
	public void SetZone(string friendZone)
	{
		TMP_Text tmp_Text = this.zoneText;
		this._friendZone = friendZone;
		tmp_Text.text = friendZone;
	}

	// Token: 0x06002FAE RID: 12206 RVA: 0x000E5384 File Offset: 0x000E3584
	public void Randomize()
	{
		this.SetEmpty();
		int num = Random.Range(0, this.randomNames.Length);
		this.SetName(this.randomNames[num].ToUpper());
		this.SetRoom(string.Format("{0}{1}{2}{3}", new object[]
		{
			(char)Random.Range(65, 91),
			(char)Random.Range(65, 91),
			(char)Random.Range(65, 91),
			(char)Random.Range(65, 91)
		}));
		bool flag = Random.Range(0f, 1f) > 0.5f;
		this.joinable = (flag && Random.Range(0f, 1f) > 0.5f);
		if (flag)
		{
			int num2 = Random.Range(0, 17);
			GTZone gtzone = (GTZone)num2;
			this.SetZone(gtzone.ToString().ToUpper());
		}
		else
		{
			this.SetZone(this.privateString);
		}
		this.UpdateComponentStates();
	}

	// Token: 0x06002FAF RID: 12207 RVA: 0x000E548A File Offset: 0x000E368A
	public void SetEmpty()
	{
		this.SetName(this.emptyString);
		this.SetRoom(this.emptyString);
		this.SetZone(this.emptyString);
		this.joinable = false;
		this.currentFriend = null;
		this.UpdateComponentStates();
	}

	// Token: 0x06002FB0 RID: 12208 RVA: 0x000E54C4 File Offset: 0x000E36C4
	public void SetRemoveEnabled(bool enabled)
	{
		this.canRemove = enabled;
		this.UpdateComponentStates();
	}

	// Token: 0x06002FB1 RID: 12209 RVA: 0x000E54D4 File Offset: 0x000E36D4
	private void JoinButtonPressed()
	{
		if (this.joinable && this.currentFriend != null && this.currentFriend.Presence != null)
		{
			bool? isPublic = this.currentFriend.Presence.IsPublic;
			bool flag = true;
			JoinType roomJoinType = (isPublic.GetValueOrDefault() == flag & isPublic != null) ? JoinType.FriendStationPublic : JoinType.FriendStationPrivate;
			GorillaComputer.instance.roomToJoin = this._friendRoom;
			PhotonNetworkController.Instance.AttemptToJoinSpecificRoom(this._friendRoom, roomJoinType);
			this.joinable = false;
			this.UpdateComponentStates();
		}
	}

	// Token: 0x06002FB2 RID: 12210 RVA: 0x000E555C File Offset: 0x000E375C
	private void RemoveFriendButtonPressed()
	{
		if (this.friendDisplay.InRemoveMode)
		{
			FriendSystem.Instance.RemoveFriend(this.currentFriend, null);
			this.SetEmpty();
		}
	}

	// Token: 0x06002FB3 RID: 12211 RVA: 0x000E5584 File Offset: 0x000E3784
	private void OnDrawGizmosSelected()
	{
		float num = this.width * 0.5f * base.transform.lossyScale.x;
		float num2 = this.Height * 0.5f * base.transform.lossyScale.y;
		float num3 = num;
		float num4 = num2;
		Vector3 vector = base.transform.position + base.transform.rotation * new Vector3(-num3, num4, 0f);
		Vector3 vector2 = base.transform.position + base.transform.rotation * new Vector3(num3, num4, 0f);
		Vector3 vector3 = base.transform.position + base.transform.rotation * new Vector3(-num3, -num4, 0f);
		Vector3 vector4 = base.transform.position + base.transform.rotation * new Vector3(num3, -num4, 0f);
		Gizmos.color = Color.white;
		Gizmos.DrawLine(vector, vector2);
		Gizmos.DrawLine(vector2, vector4);
		Gizmos.DrawLine(vector4, vector3);
		Gizmos.DrawLine(vector3, vector);
	}

	// Token: 0x06002FB4 RID: 12212 RVA: 0x000E56B8 File Offset: 0x000E38B8
	public void SetButton(GorillaPressableDelayButton friendCardButton, Material[] normalMaterials, Material[] activeMaterials, Material[] alertMaterials, TextMeshProUGUI buttonText)
	{
		this._button = friendCardButton;
		this._button.SetFillBar(this.removeProgressBar);
		this._button.onPressBegin += this.OnButtonPressBegin;
		this._button.onPressAbort += this.OnButtonPressAbort;
		this._button.onPressed += this.OnButtonPressed;
		this._buttonDefaultMaterials = normalMaterials;
		this._buttonActiveMaterials = activeMaterials;
		this._buttonAlertMaterials = alertMaterials;
		this._buttonText = buttonText;
		this.SetButtonState(FriendDisplay.ButtonState.Default);
	}

	// Token: 0x06002FB5 RID: 12213 RVA: 0x000E5747 File Offset: 0x000E3947
	private void OnRemoveFriendBegin()
	{
		this.nameText.text = "REMOVING";
		this.roomText.text = "FRIEND";
		this.zoneText.text = this.emptyString;
	}

	// Token: 0x06002FB6 RID: 12214 RVA: 0x000E577A File Offset: 0x000E397A
	private void OnRemoveFriendEnd()
	{
		this.nameText.text = this._friendName;
		this.roomText.text = this._friendRoom;
		this.zoneText.text = this._friendZone;
	}

	// Token: 0x06002FB7 RID: 12215 RVA: 0x000E57B0 File Offset: 0x000E39B0
	private void OnButtonPressBegin()
	{
		switch (this._buttonState)
		{
		case FriendDisplay.ButtonState.Default:
		case FriendDisplay.ButtonState.Active:
			break;
		case FriendDisplay.ButtonState.Alert:
			this.OnRemoveFriendBegin();
			break;
		default:
			return;
		}
	}

	// Token: 0x06002FB8 RID: 12216 RVA: 0x000E57E0 File Offset: 0x000E39E0
	private void OnButtonPressAbort()
	{
		switch (this._buttonState)
		{
		case FriendDisplay.ButtonState.Default:
		case FriendDisplay.ButtonState.Active:
			break;
		case FriendDisplay.ButtonState.Alert:
			this.OnRemoveFriendEnd();
			break;
		default:
			return;
		}
	}

	// Token: 0x06002FB9 RID: 12217 RVA: 0x000E5810 File Offset: 0x000E3A10
	private void OnButtonPressed(GorillaPressableButton button, bool isLeftHand)
	{
		switch (this._buttonState)
		{
		case FriendDisplay.ButtonState.Default:
			break;
		case FriendDisplay.ButtonState.Active:
			this.JoinButtonPressed();
			return;
		case FriendDisplay.ButtonState.Alert:
			this.RemoveFriendButtonPressed();
			break;
		default:
			return;
		}
	}

	// Token: 0x040033A5 RID: 13221
	[SerializeField]
	private TextMeshProUGUI nameText;

	// Token: 0x040033A6 RID: 13222
	[SerializeField]
	private TextMeshProUGUI roomText;

	// Token: 0x040033A7 RID: 13223
	[SerializeField]
	private TextMeshProUGUI zoneText;

	// Token: 0x040033A8 RID: 13224
	[SerializeField]
	private Transform removeProgressBar;

	// Token: 0x040033A9 RID: 13225
	[SerializeField]
	private float width = 0.25f;

	// Token: 0x040033AB RID: 13227
	private string emptyString = "";

	// Token: 0x040033AC RID: 13228
	private string privateString = "PRIVATE";

	// Token: 0x040033AD RID: 13229
	private bool joinable;

	// Token: 0x040033AE RID: 13230
	private bool canRemove;

	// Token: 0x040033AF RID: 13231
	private GorillaPressableDelayButton _button;

	// Token: 0x040033B0 RID: 13232
	private TextMeshProUGUI _buttonText;

	// Token: 0x040033B1 RID: 13233
	private string _friendName;

	// Token: 0x040033B2 RID: 13234
	private string _friendRoom;

	// Token: 0x040033B3 RID: 13235
	private string _friendZone;

	// Token: 0x040033B4 RID: 13236
	private FriendBackendController.Friend currentFriend;

	// Token: 0x040033B5 RID: 13237
	private FriendDisplay friendDisplay;

	// Token: 0x040033B6 RID: 13238
	private string[] randomNames = new string[]
	{
		"Veronica",
		"Roman",
		"Janiyah",
		"Dalton",
		"Bellamy",
		"Eithan",
		"Celeste",
		"Isaac",
		"Astrid",
		"Azariah",
		"Keilani",
		"Zeke",
		"Jayleen",
		"Yosef",
		"Jaylee",
		"Bodie",
		"Greta",
		"Cain",
		"Ella",
		"Everly",
		"Finnley",
		"Paisley",
		"Kaison",
		"Luna",
		"Nina",
		"Maison",
		"Monroe",
		"Ricardo",
		"Zariyah",
		"Travis",
		"Lacey",
		"Elian",
		"Frankie",
		"Otis",
		"Adele",
		"Edison",
		"Amira",
		"Ivan",
		"Raelynn",
		"Eliel",
		"Aliana",
		"Beckett",
		"Mylah",
		"Melvin",
		"Magdalena",
		"Leroy",
		"Madeleine"
	};

	// Token: 0x040033B7 RID: 13239
	private FriendDisplay.ButtonState _buttonState = (FriendDisplay.ButtonState)(-1);

	// Token: 0x040033B8 RID: 13240
	private Material[] _buttonDefaultMaterials;

	// Token: 0x040033B9 RID: 13241
	private Material[] _buttonActiveMaterials;

	// Token: 0x040033BA RID: 13242
	private Material[] _buttonAlertMaterials;
}
