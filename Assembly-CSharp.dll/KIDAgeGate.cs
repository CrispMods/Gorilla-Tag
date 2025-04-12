using System;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

// Token: 0x020006DF RID: 1759
public class KIDAgeGate : MonoBehaviour
{
	// Token: 0x170004A2 RID: 1186
	// (get) Token: 0x06002BD6 RID: 11222 RVA: 0x0004CF83 File Offset: 0x0004B183
	// (set) Token: 0x06002BD7 RID: 11223 RVA: 0x0004CFA6 File Offset: 0x0004B1A6
	public static string AgePlayerPrefKey
	{
		get
		{
			if (string.IsNullOrEmpty(KIDAgeGate._agePlayerPrefKey))
			{
				Debug.LogError("Trying to get [_agePlayerPrefKey] but not yet set");
				return "";
			}
			return KIDAgeGate._agePlayerPrefKey;
		}
		private set
		{
			KIDAgeGate._agePlayerPrefKey = "playerAge_" + value;
		}
	}

	// Token: 0x170004A3 RID: 1187
	// (get) Token: 0x06002BD8 RID: 11224 RVA: 0x0004CFB8 File Offset: 0x0004B1B8
	// (set) Token: 0x06002BD9 RID: 11225 RVA: 0x0004CFD5 File Offset: 0x0004B1D5
	private static string UnderAgePlayerPrefKey
	{
		get
		{
			if (string.IsNullOrEmpty(KIDAgeGate._underagePlayerPrefKey))
			{
				Debug.LogError("Trying to get [_underagePlayerPrefKey] but not yet set");
			}
			return KIDAgeGate._underagePlayerPrefKey;
		}
		set
		{
			KIDAgeGate._underagePlayerPrefKey = "player_UnderAge_" + value;
		}
	}

	// Token: 0x170004A4 RID: 1188
	// (get) Token: 0x06002BDA RID: 11226 RVA: 0x0004CFE7 File Offset: 0x0004B1E7
	// (set) Token: 0x06002BDB RID: 11227 RVA: 0x0004CFEE File Offset: 0x0004B1EE
	public static bool DisplayedScreen { get; private set; }

	// Token: 0x170004A5 RID: 1189
	// (get) Token: 0x06002BDC RID: 11228 RVA: 0x0004CFF6 File Offset: 0x0004B1F6
	public static int UserAge
	{
		get
		{
			return KIDAgeGate._ageValue;
		}
	}

	// Token: 0x06002BDD RID: 11229 RVA: 0x0011D950 File Offset: 0x0011BB50
	private void Start()
	{
		KIDAgeGate.<Start>d__33 <Start>d__;
		<Start>d__.<>t__builder = AsyncVoidMethodBuilder.Create();
		<Start>d__.<>4__this = this;
		<Start>d__.<>1__state = -1;
		<Start>d__.<>t__builder.Start<KIDAgeGate.<Start>d__33>(ref <Start>d__);
	}

	// Token: 0x06002BDE RID: 11230 RVA: 0x0004CFFD File Offset: 0x0004B1FD
	private void OnDestroy()
	{
		this.requestCancellationSource.Cancel();
	}

	// Token: 0x06002BDF RID: 11231 RVA: 0x0004D00A File Offset: 0x0004B20A
	public static void PlayFabAuthenticated(string playFabId)
	{
		if (string.IsNullOrEmpty(playFabId))
		{
			Debug.LogError("[KID] PlayFab Id is null, but executing the PlayFab authenticated flow");
			return;
		}
		Debug.Log("[KID] Sucecssfully set playfab Id to [" + playFabId + "]");
		KIDAgeGate.AgePlayerPrefKey = playFabId;
	}

	// Token: 0x06002BE0 RID: 11232 RVA: 0x0011D988 File Offset: 0x0011BB88
	private void InitialiseAgeGate()
	{
		KIDAgeGate.<InitialiseAgeGate>d__36 <InitialiseAgeGate>d__;
		<InitialiseAgeGate>d__.<>t__builder = AsyncVoidMethodBuilder.Create();
		<InitialiseAgeGate>d__.<>4__this = this;
		<InitialiseAgeGate>d__.<>1__state = -1;
		<InitialiseAgeGate>d__.<>t__builder.Start<KIDAgeGate.<InitialiseAgeGate>d__36>(ref <InitialiseAgeGate>d__);
	}

	// Token: 0x06002BE1 RID: 11233 RVA: 0x0011D9C0 File Offset: 0x0011BBC0
	private Task ProcessAgeGate()
	{
		KIDAgeGate.<ProcessAgeGate>d__37 <ProcessAgeGate>d__;
		<ProcessAgeGate>d__.<>t__builder = AsyncTaskMethodBuilder.Create();
		<ProcessAgeGate>d__.<>4__this = this;
		<ProcessAgeGate>d__.<>1__state = -1;
		<ProcessAgeGate>d__.<>t__builder.Start<KIDAgeGate.<ProcessAgeGate>d__37>(ref <ProcessAgeGate>d__);
		return <ProcessAgeGate>d__.<>t__builder.Task;
	}

	// Token: 0x06002BE2 RID: 11234 RVA: 0x0011DA04 File Offset: 0x0011BC04
	private Task WaitForAgeConfirmation()
	{
		KIDAgeGate.<WaitForAgeConfirmation>d__38 <WaitForAgeConfirmation>d__;
		<WaitForAgeConfirmation>d__.<>t__builder = AsyncTaskMethodBuilder.Create();
		<WaitForAgeConfirmation>d__.<>4__this = this;
		<WaitForAgeConfirmation>d__.<>1__state = -1;
		<WaitForAgeConfirmation>d__.<>t__builder.Start<KIDAgeGate.<WaitForAgeConfirmation>d__38>(ref <WaitForAgeConfirmation>d__);
		return <WaitForAgeConfirmation>d__.<>t__builder.Task;
	}

	// Token: 0x06002BE3 RID: 11235 RVA: 0x0004D03A File Offset: 0x0004B23A
	public static void OnConfirmAgePressed(int currentAge)
	{
		KIDAgeGate._ageValue = currentAge;
		KIDAgeGate._hasConfirmedAge = true;
	}

	// Token: 0x06002BE4 RID: 11236 RVA: 0x0011DA48 File Offset: 0x0011BC48
	private void OnAgeGateCompleted()
	{
		KIDAgeGate.<OnAgeGateCompleted>d__40 <OnAgeGateCompleted>d__;
		<OnAgeGateCompleted>d__.<>t__builder = AsyncVoidMethodBuilder.Create();
		<OnAgeGateCompleted>d__.<>4__this = this;
		<OnAgeGateCompleted>d__.<>1__state = -1;
		<OnAgeGateCompleted>d__.<>t__builder.Start<KIDAgeGate.<OnAgeGateCompleted>d__40>(ref <OnAgeGateCompleted>d__);
	}

	// Token: 0x06002BE5 RID: 11237 RVA: 0x0011DA80 File Offset: 0x0011BC80
	private void BlockGameAccess()
	{
		Debug.Log("[KID] Age is less than " + 13.ToString() + ", blocking game-access");
		if (this._pregameMessageReference == null)
		{
			Debug.LogError("[KID] [_pregameMessageReference] is not set, trying to find in scene");
			this._pregameMessageReference = UnityEngine.Object.FindAnyObjectByType<PreGameMessage>();
			if (this._pregameMessageReference == null)
			{
				Debug.LogError("[KID] Unable to find [PreGameMessage] in the scene!");
				return;
			}
		}
		PrivateUIRoom.RemoveUI(this._uiParent.transform);
		string messageTitle = "UNDER AGE";
		string messageBody = "Your VR platform requires a certain minimum age to play Gorilla Tag. Unfortunately, due to those age requirements, we cannot allow you to play Gorilla Tag at this time.\n\nIf you incorrectly submitted your age, please appeal.";
		string messageConfirmation = "Hold any face button to appeal";
		this._pregameMessageReference.ShowMessage(messageTitle, messageBody, messageConfirmation, new Action(this.AppealAge), 0.25f, 0f);
	}

	// Token: 0x06002BE6 RID: 11238 RVA: 0x0011DB30 File Offset: 0x0011BD30
	private Task SaveAge()
	{
		KIDAgeGate.<SaveAge>d__42 <SaveAge>d__;
		<SaveAge>d__.<>t__builder = AsyncTaskMethodBuilder.Create();
		<SaveAge>d__.<>4__this = this;
		<SaveAge>d__.<>1__state = -1;
		<SaveAge>d__.<>t__builder.Start<KIDAgeGate.<SaveAge>d__42>(ref <SaveAge>d__);
		return <SaveAge>d__.<>t__builder.Task;
	}

	// Token: 0x06002BE7 RID: 11239 RVA: 0x0011DB74 File Offset: 0x0011BD74
	private Task<bool> GetAge()
	{
		KIDAgeGate.<GetAge>d__43 <GetAge>d__;
		<GetAge>d__.<>t__builder = AsyncTaskMethodBuilder<bool>.Create();
		<GetAge>d__.<>4__this = this;
		<GetAge>d__.<>1__state = -1;
		<GetAge>d__.<>t__builder.Start<KIDAgeGate.<GetAge>d__43>(ref <GetAge>d__);
		return <GetAge>d__.<>t__builder.Task;
	}

	// Token: 0x06002BE8 RID: 11240 RVA: 0x0004D048 File Offset: 0x0004B248
	private void FinaliseAgeGateAndContinue()
	{
		if (this.requestCancellationSource.IsCancellationRequested)
		{
			return;
		}
		AgeSlider.ToggleAgeGate(false);
		PrivateUIRoom.RemoveUI(this._uiParent.transform);
		LegalAgreements.instance.StartLegalAgreements();
		UnityEngine.Object.Destroy(base.gameObject);
	}

	// Token: 0x06002BE9 RID: 11241 RVA: 0x0004D083 File Offset: 0x0004B283
	private void QuitGame()
	{
		Debug.Log("[KID] QUIT PRESSED");
		Application.Quit();
	}

	// Token: 0x06002BEA RID: 11242 RVA: 0x0011DBB8 File Offset: 0x0011BDB8
	private void AppealAge()
	{
		KIDAgeGate.<AppealAge>d__46 <AppealAge>d__;
		<AppealAge>d__.<>t__builder = AsyncVoidMethodBuilder.Create();
		<AppealAge>d__.<>4__this = this;
		<AppealAge>d__.<>1__state = -1;
		<AppealAge>d__.<>t__builder.Start<KIDAgeGate.<AppealAge>d__46>(ref <AppealAge>d__);
	}

	// Token: 0x06002BEB RID: 11243 RVA: 0x0011DBF0 File Offset: 0x0011BDF0
	private void AppealRejected()
	{
		Debug.Log("[KID] APPEAL REJECTED");
		string messageTitle = "UNDER AGE";
		string messageBody = "Your VR platform requires a certain minimum age to play Gorilla Tag. Unfortunately, due to those age requirements, we cannot allow you to play Gorilla Tag at this time.\n\nIf you incorrectly submitted your age, please appeal.";
		string messageConfirmation = "Hold any face button to appeal";
		this._pregameMessageReference.ShowMessage(messageTitle, messageBody, messageConfirmation, new Action(this.AppealAge), 0.25f, 0f);
	}

	// Token: 0x06002BEC RID: 11244 RVA: 0x0002F75F File Offset: 0x0002D95F
	private void RefreshChallengeStatus()
	{
	}

	// Token: 0x04003101 RID: 12545
	private const string DEFAULT_AGE_VALUE_STRING = "SET AGE";

	// Token: 0x04003102 RID: 12546
	private const string AGE_PLAYER_PREF_KEY_PREFIX = "playerAge_";

	// Token: 0x04003103 RID: 12547
	private const string PLAYER_UNDERAGE_EXPIRATION_KEY = "player_UnderAge_";

	// Token: 0x04003104 RID: 12548
	private const int MINIMUM_PLATFORM_AGE = 13;

	// Token: 0x04003105 RID: 12549
	[Header("Age Gate Settings")]
	[SerializeField]
	private PreGameMessage _pregameMessageReference;

	// Token: 0x04003106 RID: 12550
	[SerializeField]
	private GameObject _uiParent;

	// Token: 0x04003107 RID: 12551
	[SerializeField]
	private AgeSlider _ageSlider;

	// Token: 0x04003108 RID: 12552
	private const string strBlockAccessTitle = "UNDER AGE";

	// Token: 0x04003109 RID: 12553
	private const string strBlockAccessMessage = "Your VR platform requires a certain minimum age to play Gorilla Tag. Unfortunately, due to those age requirements, we cannot allow you to play Gorilla Tag at this time.\n\nIf you incorrectly submitted your age, please appeal.";

	// Token: 0x0400310A RID: 12554
	private const string strBlockAccessConfirm = "Hold any face button to appeal";

	// Token: 0x0400310B RID: 12555
	private const string strVerifyAgeTitle = "VERIFY AGE";

	// Token: 0x0400310C RID: 12556
	private const string strVerifyAgeMessage = "GETTING ONE TIME PASSCODE. PLEASE WAIT.\n\nGIVE IT TO A PARENT/GUARDIAN TO ENTER IT AT: k-id.com/code";

	// Token: 0x0400310D RID: 12557
	private const string strDiscrepancyTitle = "AGE DISCREPANCY";

	// Token: 0x0400310E RID: 12558
	private const string strDiscrepancyMessage = "You said you are {0} but your account says you should be [{1}+]. You could be using the wrong account.\n\nWe will use the lowest age ({0}) and you will need parental/guardian consent for some features.";

	// Token: 0x0400310F RID: 12559
	private const string strDiscrepancyMessageRange = "You said you are {0} but your account says you should be between [{1} - {2}]. You could be using the wrong account.\n\nWe will use the lowest age ({1}) and you will need parental/guardian consent for some features.";

	// Token: 0x04003110 RID: 12560
	private const string strDiscrepancyConfirm = "Hold any face button to continue";

	// Token: 0x04003111 RID: 12561
	private static int _ageValue;

	// Token: 0x04003112 RID: 12562
	private CancellationTokenSource requestCancellationSource = new CancellationTokenSource();

	// Token: 0x04003113 RID: 12563
	private static bool _hasConfirmedAge = false;

	// Token: 0x04003114 RID: 12564
	private static string _agePlayerPrefKey = "";

	// Token: 0x04003115 RID: 12565
	private static string _underagePlayerPrefKey = "";
}
