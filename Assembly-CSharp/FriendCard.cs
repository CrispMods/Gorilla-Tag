using System;
using System.Runtime.CompilerServices;
using GorillaNetworking;
using TMPro;
using UnityEngine;

// Token: 0x020007A5 RID: 1957
public class FriendCard : MonoBehaviour
{
	// Token: 0x17000503 RID: 1283
	// (get) Token: 0x06003049 RID: 12361 RVA: 0x000500DA File Offset: 0x0004E2DA
	public TextMeshProUGUI NameText
	{
		get
		{
			return this.nameText;
		}
	}

	// Token: 0x17000504 RID: 1284
	// (get) Token: 0x0600304A RID: 12362 RVA: 0x000500E2 File Offset: 0x0004E2E2
	public TextMeshProUGUI RoomText
	{
		get
		{
			return this.roomText;
		}
	}

	// Token: 0x17000505 RID: 1285
	// (get) Token: 0x0600304B RID: 12363 RVA: 0x000500EA File Offset: 0x0004E2EA
	public TextMeshProUGUI ZoneText
	{
		get
		{
			return this.zoneText;
		}
	}

	// Token: 0x17000506 RID: 1286
	// (get) Token: 0x0600304C RID: 12364 RVA: 0x000500F2 File Offset: 0x0004E2F2
	public float Width
	{
		get
		{
			return this.width;
		}
	}

	// Token: 0x17000507 RID: 1287
	// (get) Token: 0x0600304D RID: 12365 RVA: 0x000500FA File Offset: 0x0004E2FA
	// (set) Token: 0x0600304E RID: 12366 RVA: 0x00050102 File Offset: 0x0004E302
	public float Height { get; private set; } = 0.25f;

	// Token: 0x0600304F RID: 12367 RVA: 0x0005010B File Offset: 0x0004E30B
	private void Awake()
	{
		if (this.removeProgressBar)
		{
			this.removeProgressBar.gameObject.SetActive(false);
		}
	}

	// Token: 0x06003050 RID: 12368 RVA: 0x0005012B File Offset: 0x0004E32B
	private void OnDestroy()
	{
		if (this._button)
		{
			this._button.onPressed -= this.OnButtonPressed;
		}
	}

	// Token: 0x06003051 RID: 12369 RVA: 0x00050151 File Offset: 0x0004E351
	public void Init(FriendDisplay owner)
	{
		this.friendDisplay = owner;
	}

	// Token: 0x06003052 RID: 12370 RVA: 0x0012E4EC File Offset: 0x0012C6EC
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

	// Token: 0x06003053 RID: 12371 RVA: 0x0012E550 File Offset: 0x0012C750
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

	// Token: 0x06003054 RID: 12372 RVA: 0x0012E5EC File Offset: 0x0012C7EC
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

	// Token: 0x06003055 RID: 12373 RVA: 0x0012E800 File Offset: 0x0012CA00
	public void SetName(string friendName)
	{
		TMP_Text tmp_Text = this.nameText;
		this._friendName = friendName;
		tmp_Text.text = friendName;
	}

	// Token: 0x06003056 RID: 12374 RVA: 0x0012E824 File Offset: 0x0012CA24
	public void SetRoom(string friendRoom)
	{
		TMP_Text tmp_Text = this.roomText;
		this._friendRoom = friendRoom;
		tmp_Text.text = friendRoom;
	}

	// Token: 0x06003057 RID: 12375 RVA: 0x0012E848 File Offset: 0x0012CA48
	public void SetZone(string friendZone)
	{
		TMP_Text tmp_Text = this.zoneText;
		this._friendZone = friendZone;
		tmp_Text.text = friendZone;
	}

	// Token: 0x06003058 RID: 12376 RVA: 0x0012E86C File Offset: 0x0012CA6C
	public void Randomize()
	{
		this.SetEmpty();
		int num = UnityEngine.Random.Range(0, this.randomNames.Length);
		this.SetName(this.randomNames[num].ToUpper());
		this.SetRoom(string.Format("{0}{1}{2}{3}", new object[]
		{
			(char)UnityEngine.Random.Range(65, 91),
			(char)UnityEngine.Random.Range(65, 91),
			(char)UnityEngine.Random.Range(65, 91),
			(char)UnityEngine.Random.Range(65, 91)
		}));
		bool flag = UnityEngine.Random.Range(0f, 1f) > 0.5f;
		this.joinable = (flag && UnityEngine.Random.Range(0f, 1f) > 0.5f);
		if (flag)
		{
			int num2 = UnityEngine.Random.Range(0, 17);
			GTZone gtzone = (GTZone)num2;
			this.SetZone(gtzone.ToString().ToUpper());
		}
		else
		{
			this.SetZone(this.privateString);
		}
		this.UpdateComponentStates();
	}

	// Token: 0x06003059 RID: 12377 RVA: 0x0005015A File Offset: 0x0004E35A
	public void SetEmpty()
	{
		this.SetName(this.emptyString);
		this.SetRoom(this.emptyString);
		this.SetZone(this.emptyString);
		this.joinable = false;
		this.currentFriend = null;
		this.UpdateComponentStates();
	}

	// Token: 0x0600305A RID: 12378 RVA: 0x00050194 File Offset: 0x0004E394
	public void SetRemoveEnabled(bool enabled)
	{
		this.canRemove = enabled;
		this.UpdateComponentStates();
	}

	// Token: 0x0600305B RID: 12379 RVA: 0x0012E974 File Offset: 0x0012CB74
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

	// Token: 0x0600305C RID: 12380 RVA: 0x000501A3 File Offset: 0x0004E3A3
	private void RemoveFriendButtonPressed()
	{
		if (this.friendDisplay.InRemoveMode)
		{
			FriendSystem.Instance.RemoveFriend(this.currentFriend, null);
			this.SetEmpty();
		}
	}

	// Token: 0x0600305D RID: 12381 RVA: 0x0012E9FC File Offset: 0x0012CBFC
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

	// Token: 0x0600305E RID: 12382 RVA: 0x0012EB30 File Offset: 0x0012CD30
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

	// Token: 0x0600305F RID: 12383 RVA: 0x000501CB File Offset: 0x0004E3CB
	private void OnRemoveFriendBegin()
	{
		this.nameText.text = "REMOVING";
		this.roomText.text = "FRIEND";
		this.zoneText.text = this.emptyString;
	}

	// Token: 0x06003060 RID: 12384 RVA: 0x000501FE File Offset: 0x0004E3FE
	private void OnRemoveFriendEnd()
	{
		this.nameText.text = this._friendName;
		this.roomText.text = this._friendRoom;
		this.zoneText.text = this._friendZone;
	}

	// Token: 0x06003061 RID: 12385 RVA: 0x0012EBC0 File Offset: 0x0012CDC0
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

	// Token: 0x06003062 RID: 12386 RVA: 0x0012EBF0 File Offset: 0x0012CDF0
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

	// Token: 0x06003063 RID: 12387 RVA: 0x0012EC20 File Offset: 0x0012CE20
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

	// Token: 0x04003449 RID: 13385
	[SerializeField]
	private TextMeshProUGUI nameText;

	// Token: 0x0400344A RID: 13386
	[SerializeField]
	private TextMeshProUGUI roomText;

	// Token: 0x0400344B RID: 13387
	[SerializeField]
	private TextMeshProUGUI zoneText;

	// Token: 0x0400344C RID: 13388
	[SerializeField]
	private Transform removeProgressBar;

	// Token: 0x0400344D RID: 13389
	[SerializeField]
	private float width = 0.25f;

	// Token: 0x0400344F RID: 13391
	private string emptyString = "";

	// Token: 0x04003450 RID: 13392
	private string privateString = "PRIVATE";

	// Token: 0x04003451 RID: 13393
	private bool joinable;

	// Token: 0x04003452 RID: 13394
	private bool canRemove;

	// Token: 0x04003453 RID: 13395
	private GorillaPressableDelayButton _button;

	// Token: 0x04003454 RID: 13396
	private TextMeshProUGUI _buttonText;

	// Token: 0x04003455 RID: 13397
	private string _friendName;

	// Token: 0x04003456 RID: 13398
	private string _friendRoom;

	// Token: 0x04003457 RID: 13399
	private string _friendZone;

	// Token: 0x04003458 RID: 13400
	private FriendBackendController.Friend currentFriend;

	// Token: 0x04003459 RID: 13401
	private FriendDisplay friendDisplay;

	// Token: 0x0400345A RID: 13402
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

	// Token: 0x0400345B RID: 13403
	private FriendDisplay.ButtonState _buttonState = (FriendDisplay.ButtonState)(-1);

	// Token: 0x0400345C RID: 13404
	private Material[] _buttonDefaultMaterials;

	// Token: 0x0400345D RID: 13405
	private Material[] _buttonActiveMaterials;

	// Token: 0x0400345E RID: 13406
	private Material[] _buttonAlertMaterials;
}
