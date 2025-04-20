using System;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

// Token: 0x020006F3 RID: 1779
public class KIDAgeGate : MonoBehaviour
{
	// Token: 0x170004AE RID: 1198
	// (get) Token: 0x06002C64 RID: 11364 RVA: 0x0004E2C8 File Offset: 0x0004C4C8
	// (set) Token: 0x06002C65 RID: 11365 RVA: 0x0004E2EB File Offset: 0x0004C4EB
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

	// Token: 0x170004AF RID: 1199
	// (get) Token: 0x06002C66 RID: 11366 RVA: 0x0004E2FD File Offset: 0x0004C4FD
	// (set) Token: 0x06002C67 RID: 11367 RVA: 0x0004E31A File Offset: 0x0004C51A
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

	// Token: 0x170004B0 RID: 1200
	// (get) Token: 0x06002C68 RID: 11368 RVA: 0x0004E32C File Offset: 0x0004C52C
	// (set) Token: 0x06002C69 RID: 11369 RVA: 0x0004E333 File Offset: 0x0004C533
	public static bool DisplayedScreen { get; private set; }

	// Token: 0x170004B1 RID: 1201
	// (get) Token: 0x06002C6A RID: 11370 RVA: 0x0004E33B File Offset: 0x0004C53B
	public static int UserAge
	{
		get
		{
			return KIDAgeGate._ageValue;
		}
	}

	// Token: 0x06002C6B RID: 11371 RVA: 0x00122508 File Offset: 0x00120708
	private void Start()
	{
		KIDAgeGate.<Start>d__33 <Start>d__;
		<Start>d__.<>t__builder = AsyncVoidMethodBuilder.Create();
		<Start>d__.<>4__this = this;
		<Start>d__.<>1__state = -1;
		<Start>d__.<>t__builder.Start<KIDAgeGate.<Start>d__33>(ref <Start>d__);
	}

	// Token: 0x06002C6C RID: 11372 RVA: 0x0004E342 File Offset: 0x0004C542
	private void OnDestroy()
	{
		this.requestCancellationSource.Cancel();
	}

	// Token: 0x06002C6D RID: 11373 RVA: 0x0004E34F File Offset: 0x0004C54F
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

	// Token: 0x06002C6E RID: 11374 RVA: 0x00122540 File Offset: 0x00120740
	private void InitialiseAgeGate()
	{
		KIDAgeGate.<InitialiseAgeGate>d__36 <InitialiseAgeGate>d__;
		<InitialiseAgeGate>d__.<>t__builder = AsyncVoidMethodBuilder.Create();
		<InitialiseAgeGate>d__.<>4__this = this;
		<InitialiseAgeGate>d__.<>1__state = -1;
		<InitialiseAgeGate>d__.<>t__builder.Start<KIDAgeGate.<InitialiseAgeGate>d__36>(ref <InitialiseAgeGate>d__);
	}

	// Token: 0x06002C6F RID: 11375 RVA: 0x00122578 File Offset: 0x00120778
	private Task ProcessAgeGate()
	{
		KIDAgeGate.<ProcessAgeGate>d__37 <ProcessAgeGate>d__;
		<ProcessAgeGate>d__.<>t__builder = AsyncTaskMethodBuilder.Create();
		<ProcessAgeGate>d__.<>4__this = this;
		<ProcessAgeGate>d__.<>1__state = -1;
		<ProcessAgeGate>d__.<>t__builder.Start<KIDAgeGate.<ProcessAgeGate>d__37>(ref <ProcessAgeGate>d__);
		return <ProcessAgeGate>d__.<>t__builder.Task;
	}

	// Token: 0x06002C70 RID: 11376 RVA: 0x001225BC File Offset: 0x001207BC
	private Task WaitForAgeConfirmation()
	{
		KIDAgeGate.<WaitForAgeConfirmation>d__38 <WaitForAgeConfirmation>d__;
		<WaitForAgeConfirmation>d__.<>t__builder = AsyncTaskMethodBuilder.Create();
		<WaitForAgeConfirmation>d__.<>4__this = this;
		<WaitForAgeConfirmation>d__.<>1__state = -1;
		<WaitForAgeConfirmation>d__.<>t__builder.Start<KIDAgeGate.<WaitForAgeConfirmation>d__38>(ref <WaitForAgeConfirmation>d__);
		return <WaitForAgeConfirmation>d__.<>t__builder.Task;
	}

	// Token: 0x06002C71 RID: 11377 RVA: 0x0004E37F File Offset: 0x0004C57F
	public static void OnConfirmAgePressed(int currentAge)
	{
		KIDAgeGate._ageValue = currentAge;
		KIDAgeGate._hasConfirmedAge = true;
	}

	// Token: 0x06002C72 RID: 11378 RVA: 0x00122600 File Offset: 0x00120800
	private void OnAgeGateCompleted()
	{
		KIDAgeGate.<OnAgeGateCompleted>d__40 <OnAgeGateCompleted>d__;
		<OnAgeGateCompleted>d__.<>t__builder = AsyncVoidMethodBuilder.Create();
		<OnAgeGateCompleted>d__.<>4__this = this;
		<OnAgeGateCompleted>d__.<>1__state = -1;
		<OnAgeGateCompleted>d__.<>t__builder.Start<KIDAgeGate.<OnAgeGateCompleted>d__40>(ref <OnAgeGateCompleted>d__);
	}

	// Token: 0x06002C73 RID: 11379 RVA: 0x00122638 File Offset: 0x00120838
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

	// Token: 0x06002C74 RID: 11380 RVA: 0x001226E8 File Offset: 0x001208E8
	private Task SaveAge()
	{
		KIDAgeGate.<SaveAge>d__42 <SaveAge>d__;
		<SaveAge>d__.<>t__builder = AsyncTaskMethodBuilder.Create();
		<SaveAge>d__.<>4__this = this;
		<SaveAge>d__.<>1__state = -1;
		<SaveAge>d__.<>t__builder.Start<KIDAgeGate.<SaveAge>d__42>(ref <SaveAge>d__);
		return <SaveAge>d__.<>t__builder.Task;
	}

	// Token: 0x06002C75 RID: 11381 RVA: 0x0012272C File Offset: 0x0012092C
	private Task<bool> GetAge()
	{
		KIDAgeGate.<GetAge>d__43 <GetAge>d__;
		<GetAge>d__.<>t__builder = AsyncTaskMethodBuilder<bool>.Create();
		<GetAge>d__.<>4__this = this;
		<GetAge>d__.<>1__state = -1;
		<GetAge>d__.<>t__builder.Start<KIDAgeGate.<GetAge>d__43>(ref <GetAge>d__);
		return <GetAge>d__.<>t__builder.Task;
	}

	// Token: 0x06002C76 RID: 11382 RVA: 0x0004E38D File Offset: 0x0004C58D
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

	// Token: 0x06002C77 RID: 11383 RVA: 0x0004E3C8 File Offset: 0x0004C5C8
	private void QuitGame()
	{
		Debug.Log("[KID] QUIT PRESSED");
		Application.Quit();
	}

	// Token: 0x06002C78 RID: 11384 RVA: 0x00122770 File Offset: 0x00120970
	private void AppealAge()
	{
		KIDAgeGate.<AppealAge>d__46 <AppealAge>d__;
		<AppealAge>d__.<>t__builder = AsyncVoidMethodBuilder.Create();
		<AppealAge>d__.<>4__this = this;
		<AppealAge>d__.<>1__state = -1;
		<AppealAge>d__.<>t__builder.Start<KIDAgeGate.<AppealAge>d__46>(ref <AppealAge>d__);
	}

	// Token: 0x06002C79 RID: 11385 RVA: 0x001227A8 File Offset: 0x001209A8
	private void AppealRejected()
	{
		Debug.Log("[KID] APPEAL REJECTED");
		string messageTitle = "UNDER AGE";
		string messageBody = "Your VR platform requires a certain minimum age to play Gorilla Tag. Unfortunately, due to those age requirements, we cannot allow you to play Gorilla Tag at this time.\n\nIf you incorrectly submitted your age, please appeal.";
		string messageConfirmation = "Hold any face button to appeal";
		this._pregameMessageReference.ShowMessage(messageTitle, messageBody, messageConfirmation, new Action(this.AppealAge), 0.25f, 0f);
	}

	// Token: 0x06002C7A RID: 11386 RVA: 0x00030607 File Offset: 0x0002E807
	private void RefreshChallengeStatus()
	{
	}

	// Token: 0x04003198 RID: 12696
	private const string DEFAULT_AGE_VALUE_STRING = "SET AGE";

	// Token: 0x04003199 RID: 12697
	private const string AGE_PLAYER_PREF_KEY_PREFIX = "playerAge_";

	// Token: 0x0400319A RID: 12698
	private const string PLAYER_UNDERAGE_EXPIRATION_KEY = "player_UnderAge_";

	// Token: 0x0400319B RID: 12699
	private const int MINIMUM_PLATFORM_AGE = 13;

	// Token: 0x0400319C RID: 12700
	[Header("Age Gate Settings")]
	[SerializeField]
	private PreGameMessage _pregameMessageReference;

	// Token: 0x0400319D RID: 12701
	[SerializeField]
	private GameObject _uiParent;

	// Token: 0x0400319E RID: 12702
	[SerializeField]
	private AgeSlider _ageSlider;

	// Token: 0x0400319F RID: 12703
	private const string strBlockAccessTitle = "UNDER AGE";

	// Token: 0x040031A0 RID: 12704
	private const string strBlockAccessMessage = "Your VR platform requires a certain minimum age to play Gorilla Tag. Unfortunately, due to those age requirements, we cannot allow you to play Gorilla Tag at this time.\n\nIf you incorrectly submitted your age, please appeal.";

	// Token: 0x040031A1 RID: 12705
	private const string strBlockAccessConfirm = "Hold any face button to appeal";

	// Token: 0x040031A2 RID: 12706
	private const string strVerifyAgeTitle = "VERIFY AGE";

	// Token: 0x040031A3 RID: 12707
	private const string strVerifyAgeMessage = "GETTING ONE TIME PASSCODE. PLEASE WAIT.\n\nGIVE IT TO A PARENT/GUARDIAN TO ENTER IT AT: k-id.com/code";

	// Token: 0x040031A4 RID: 12708
	private const string strDiscrepancyTitle = "AGE DISCREPANCY";

	// Token: 0x040031A5 RID: 12709
	private const string strDiscrepancyMessage = "You said you are {0} but your account says you should be [{1}+]. You could be using the wrong account.\n\nWe will use the lowest age ({0}) and you will need parental/guardian consent for some features.";

	// Token: 0x040031A6 RID: 12710
	private const string strDiscrepancyMessageRange = "You said you are {0} but your account says you should be between [{1} - {2}]. You could be using the wrong account.\n\nWe will use the lowest age ({1}) and you will need parental/guardian consent for some features.";

	// Token: 0x040031A7 RID: 12711
	private const string strDiscrepancyConfirm = "Hold any face button to continue";

	// Token: 0x040031A8 RID: 12712
	private static int _ageValue;

	// Token: 0x040031A9 RID: 12713
	private CancellationTokenSource requestCancellationSource = new CancellationTokenSource();

	// Token: 0x040031AA RID: 12714
	private static bool _hasConfirmedAge = false;

	// Token: 0x040031AB RID: 12715
	private static string _agePlayerPrefKey = "";

	// Token: 0x040031AC RID: 12716
	private static string _underagePlayerPrefKey = "";
}
