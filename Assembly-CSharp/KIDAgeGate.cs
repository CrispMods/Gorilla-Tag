using System;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

// Token: 0x020006DE RID: 1758
public class KIDAgeGate : MonoBehaviour
{
	// Token: 0x170004A1 RID: 1185
	// (get) Token: 0x06002BCE RID: 11214 RVA: 0x000D73F1 File Offset: 0x000D55F1
	// (set) Token: 0x06002BCF RID: 11215 RVA: 0x000D7414 File Offset: 0x000D5614
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

	// Token: 0x170004A2 RID: 1186
	// (get) Token: 0x06002BD0 RID: 11216 RVA: 0x000D7426 File Offset: 0x000D5626
	// (set) Token: 0x06002BD1 RID: 11217 RVA: 0x000D7443 File Offset: 0x000D5643
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

	// Token: 0x170004A3 RID: 1187
	// (get) Token: 0x06002BD2 RID: 11218 RVA: 0x000D7455 File Offset: 0x000D5655
	// (set) Token: 0x06002BD3 RID: 11219 RVA: 0x000D745C File Offset: 0x000D565C
	public static bool DisplayedScreen { get; private set; }

	// Token: 0x170004A4 RID: 1188
	// (get) Token: 0x06002BD4 RID: 11220 RVA: 0x000D7464 File Offset: 0x000D5664
	public static int UserAge
	{
		get
		{
			return KIDAgeGate._ageValue;
		}
	}

	// Token: 0x06002BD5 RID: 11221 RVA: 0x000D746C File Offset: 0x000D566C
	private void Start()
	{
		KIDAgeGate.<Start>d__33 <Start>d__;
		<Start>d__.<>t__builder = AsyncVoidMethodBuilder.Create();
		<Start>d__.<>4__this = this;
		<Start>d__.<>1__state = -1;
		<Start>d__.<>t__builder.Start<KIDAgeGate.<Start>d__33>(ref <Start>d__);
	}

	// Token: 0x06002BD6 RID: 11222 RVA: 0x000D74A3 File Offset: 0x000D56A3
	private void OnDestroy()
	{
		this.requestCancellationSource.Cancel();
	}

	// Token: 0x06002BD7 RID: 11223 RVA: 0x000D74B0 File Offset: 0x000D56B0
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

	// Token: 0x06002BD8 RID: 11224 RVA: 0x000D74E0 File Offset: 0x000D56E0
	private void InitialiseAgeGate()
	{
		KIDAgeGate.<InitialiseAgeGate>d__36 <InitialiseAgeGate>d__;
		<InitialiseAgeGate>d__.<>t__builder = AsyncVoidMethodBuilder.Create();
		<InitialiseAgeGate>d__.<>4__this = this;
		<InitialiseAgeGate>d__.<>1__state = -1;
		<InitialiseAgeGate>d__.<>t__builder.Start<KIDAgeGate.<InitialiseAgeGate>d__36>(ref <InitialiseAgeGate>d__);
	}

	// Token: 0x06002BD9 RID: 11225 RVA: 0x000D7518 File Offset: 0x000D5718
	private Task ProcessAgeGate()
	{
		KIDAgeGate.<ProcessAgeGate>d__37 <ProcessAgeGate>d__;
		<ProcessAgeGate>d__.<>t__builder = AsyncTaskMethodBuilder.Create();
		<ProcessAgeGate>d__.<>4__this = this;
		<ProcessAgeGate>d__.<>1__state = -1;
		<ProcessAgeGate>d__.<>t__builder.Start<KIDAgeGate.<ProcessAgeGate>d__37>(ref <ProcessAgeGate>d__);
		return <ProcessAgeGate>d__.<>t__builder.Task;
	}

	// Token: 0x06002BDA RID: 11226 RVA: 0x000D755C File Offset: 0x000D575C
	private Task WaitForAgeConfirmation()
	{
		KIDAgeGate.<WaitForAgeConfirmation>d__38 <WaitForAgeConfirmation>d__;
		<WaitForAgeConfirmation>d__.<>t__builder = AsyncTaskMethodBuilder.Create();
		<WaitForAgeConfirmation>d__.<>4__this = this;
		<WaitForAgeConfirmation>d__.<>1__state = -1;
		<WaitForAgeConfirmation>d__.<>t__builder.Start<KIDAgeGate.<WaitForAgeConfirmation>d__38>(ref <WaitForAgeConfirmation>d__);
		return <WaitForAgeConfirmation>d__.<>t__builder.Task;
	}

	// Token: 0x06002BDB RID: 11227 RVA: 0x000D759F File Offset: 0x000D579F
	public static void OnConfirmAgePressed(int currentAge)
	{
		KIDAgeGate._ageValue = currentAge;
		KIDAgeGate._hasConfirmedAge = true;
	}

	// Token: 0x06002BDC RID: 11228 RVA: 0x000D75B0 File Offset: 0x000D57B0
	private void OnAgeGateCompleted()
	{
		KIDAgeGate.<OnAgeGateCompleted>d__40 <OnAgeGateCompleted>d__;
		<OnAgeGateCompleted>d__.<>t__builder = AsyncVoidMethodBuilder.Create();
		<OnAgeGateCompleted>d__.<>4__this = this;
		<OnAgeGateCompleted>d__.<>1__state = -1;
		<OnAgeGateCompleted>d__.<>t__builder.Start<KIDAgeGate.<OnAgeGateCompleted>d__40>(ref <OnAgeGateCompleted>d__);
	}

	// Token: 0x06002BDD RID: 11229 RVA: 0x000D75E8 File Offset: 0x000D57E8
	private void BlockGameAccess()
	{
		Debug.Log("[KID] Age is less than " + 13.ToString() + ", blocking game-access");
		if (this._pregameMessageReference == null)
		{
			Debug.LogError("[KID] [_pregameMessageReference] is not set, trying to find in scene");
			this._pregameMessageReference = Object.FindAnyObjectByType<PreGameMessage>();
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

	// Token: 0x06002BDE RID: 11230 RVA: 0x000D7698 File Offset: 0x000D5898
	private Task SaveAge()
	{
		KIDAgeGate.<SaveAge>d__42 <SaveAge>d__;
		<SaveAge>d__.<>t__builder = AsyncTaskMethodBuilder.Create();
		<SaveAge>d__.<>4__this = this;
		<SaveAge>d__.<>1__state = -1;
		<SaveAge>d__.<>t__builder.Start<KIDAgeGate.<SaveAge>d__42>(ref <SaveAge>d__);
		return <SaveAge>d__.<>t__builder.Task;
	}

	// Token: 0x06002BDF RID: 11231 RVA: 0x000D76DC File Offset: 0x000D58DC
	private Task<bool> GetAge()
	{
		KIDAgeGate.<GetAge>d__43 <GetAge>d__;
		<GetAge>d__.<>t__builder = AsyncTaskMethodBuilder<bool>.Create();
		<GetAge>d__.<>4__this = this;
		<GetAge>d__.<>1__state = -1;
		<GetAge>d__.<>t__builder.Start<KIDAgeGate.<GetAge>d__43>(ref <GetAge>d__);
		return <GetAge>d__.<>t__builder.Task;
	}

	// Token: 0x06002BE0 RID: 11232 RVA: 0x000D771F File Offset: 0x000D591F
	private void FinaliseAgeGateAndContinue()
	{
		if (this.requestCancellationSource.IsCancellationRequested)
		{
			return;
		}
		AgeSlider.ToggleAgeGate(false);
		PrivateUIRoom.RemoveUI(this._uiParent.transform);
		LegalAgreements.instance.StartLegalAgreements();
		Object.Destroy(base.gameObject);
	}

	// Token: 0x06002BE1 RID: 11233 RVA: 0x000D775A File Offset: 0x000D595A
	private void QuitGame()
	{
		Debug.Log("[KID] QUIT PRESSED");
		Application.Quit();
	}

	// Token: 0x06002BE2 RID: 11234 RVA: 0x000D776C File Offset: 0x000D596C
	private void AppealAge()
	{
		KIDAgeGate.<AppealAge>d__46 <AppealAge>d__;
		<AppealAge>d__.<>t__builder = AsyncVoidMethodBuilder.Create();
		<AppealAge>d__.<>4__this = this;
		<AppealAge>d__.<>1__state = -1;
		<AppealAge>d__.<>t__builder.Start<KIDAgeGate.<AppealAge>d__46>(ref <AppealAge>d__);
	}

	// Token: 0x06002BE3 RID: 11235 RVA: 0x000D77A4 File Offset: 0x000D59A4
	private void AppealRejected()
	{
		Debug.Log("[KID] APPEAL REJECTED");
		string messageTitle = "UNDER AGE";
		string messageBody = "Your VR platform requires a certain minimum age to play Gorilla Tag. Unfortunately, due to those age requirements, we cannot allow you to play Gorilla Tag at this time.\n\nIf you incorrectly submitted your age, please appeal.";
		string messageConfirmation = "Hold any face button to appeal";
		this._pregameMessageReference.ShowMessage(messageTitle, messageBody, messageConfirmation, new Action(this.AppealAge), 0.25f, 0f);
	}

	// Token: 0x06002BE4 RID: 11236 RVA: 0x000023F4 File Offset: 0x000005F4
	private void RefreshChallengeStatus()
	{
	}

	// Token: 0x040030FB RID: 12539
	private const string DEFAULT_AGE_VALUE_STRING = "SET AGE";

	// Token: 0x040030FC RID: 12540
	private const string AGE_PLAYER_PREF_KEY_PREFIX = "playerAge_";

	// Token: 0x040030FD RID: 12541
	private const string PLAYER_UNDERAGE_EXPIRATION_KEY = "player_UnderAge_";

	// Token: 0x040030FE RID: 12542
	private const int MINIMUM_PLATFORM_AGE = 13;

	// Token: 0x040030FF RID: 12543
	[Header("Age Gate Settings")]
	[SerializeField]
	private PreGameMessage _pregameMessageReference;

	// Token: 0x04003100 RID: 12544
	[SerializeField]
	private GameObject _uiParent;

	// Token: 0x04003101 RID: 12545
	[SerializeField]
	private AgeSlider _ageSlider;

	// Token: 0x04003102 RID: 12546
	private const string strBlockAccessTitle = "UNDER AGE";

	// Token: 0x04003103 RID: 12547
	private const string strBlockAccessMessage = "Your VR platform requires a certain minimum age to play Gorilla Tag. Unfortunately, due to those age requirements, we cannot allow you to play Gorilla Tag at this time.\n\nIf you incorrectly submitted your age, please appeal.";

	// Token: 0x04003104 RID: 12548
	private const string strBlockAccessConfirm = "Hold any face button to appeal";

	// Token: 0x04003105 RID: 12549
	private const string strVerifyAgeTitle = "VERIFY AGE";

	// Token: 0x04003106 RID: 12550
	private const string strVerifyAgeMessage = "GETTING ONE TIME PASSCODE. PLEASE WAIT.\n\nGIVE IT TO A PARENT/GUARDIAN TO ENTER IT AT: k-id.com/code";

	// Token: 0x04003107 RID: 12551
	private const string strDiscrepancyTitle = "AGE DISCREPANCY";

	// Token: 0x04003108 RID: 12552
	private const string strDiscrepancyMessage = "You said you are {0} but your account says you should be [{1}+]. You could be using the wrong account.\n\nWe will use the lowest age ({0}) and you will need parental/guardian consent for some features.";

	// Token: 0x04003109 RID: 12553
	private const string strDiscrepancyMessageRange = "You said you are {0} but your account says you should be between [{1} - {2}]. You could be using the wrong account.\n\nWe will use the lowest age ({1}) and you will need parental/guardian consent for some features.";

	// Token: 0x0400310A RID: 12554
	private const string strDiscrepancyConfirm = "Hold any face button to continue";

	// Token: 0x0400310B RID: 12555
	private static int _ageValue;

	// Token: 0x0400310C RID: 12556
	private CancellationTokenSource requestCancellationSource = new CancellationTokenSource();

	// Token: 0x0400310D RID: 12557
	private static bool _hasConfirmedAge = false;

	// Token: 0x0400310E RID: 12558
	private static string _agePlayerPrefKey = "";

	// Token: 0x0400310F RID: 12559
	private static string _underagePlayerPrefKey = "";
}
