using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using PlayFab;
using TMPro;
using UnityEngine;

// Token: 0x02000721 RID: 1825
public class LegalAgreements : MonoBehaviour
{
	// Token: 0x170004CA RID: 1226
	// (get) Token: 0x06002D51 RID: 11601 RVA: 0x0004ECBD File Offset: 0x0004CEBD
	// (set) Token: 0x06002D52 RID: 11602 RVA: 0x0004ECC4 File Offset: 0x0004CEC4
	public static LegalAgreements instance { get; private set; }

	// Token: 0x06002D53 RID: 11603 RVA: 0x0004ECCC File Offset: 0x0004CECC
	private void Awake()
	{
		if (LegalAgreements.instance != null)
		{
			Debug.LogError("Trying to set [LegalAgreements] instance but it is not null", this);
			base.gameObject.SetActive(false);
			return;
		}
		this.controllerBehaviour = base.GetComponentInChildren<ControllerBehaviour>(true);
		LegalAgreements.instance = this;
	}

	// Token: 0x06002D54 RID: 11604 RVA: 0x0004ED06 File Offset: 0x0004CF06
	private void OnEnable()
	{
		this.controllerBehaviour.OnAction += this.PostUpdate;
	}

	// Token: 0x06002D55 RID: 11605 RVA: 0x0004ED1F File Offset: 0x0004CF1F
	private void OnDisable()
	{
		this.controllerBehaviour.OnAction -= this.PostUpdate;
	}

	// Token: 0x06002D56 RID: 11606 RVA: 0x00127330 File Offset: 0x00125530
	public void StartLegalAgreements()
	{
		LegalAgreements.<StartLegalAgreements>d__17 <StartLegalAgreements>d__;
		<StartLegalAgreements>d__.<>t__builder = AsyncVoidMethodBuilder.Create();
		<StartLegalAgreements>d__.<>4__this = this;
		<StartLegalAgreements>d__.<>1__state = -1;
		<StartLegalAgreements>d__.<>t__builder.Start<LegalAgreements.<StartLegalAgreements>d__17>(ref <StartLegalAgreements>d__);
	}

	// Token: 0x06002D57 RID: 11607 RVA: 0x00127368 File Offset: 0x00125568
	private void PostUpdate()
	{
		if (this.controllerBehaviour.IsLeftStick)
		{
			this.TurnPage(-1);
		}
		if (this.controllerBehaviour.IsRightStick)
		{
			this.TurnPage(1);
		}
		if (this.controllerBehaviour.IsUpStick)
		{
			this.TurnPage(-1);
		}
		if (this.controllerBehaviour.IsDownStick)
		{
			this.TurnPage(1);
		}
	}

	// Token: 0x06002D58 RID: 11608 RVA: 0x001273C8 File Offset: 0x001255C8
	private Task WaitForAcknowledgement()
	{
		LegalAgreements.<WaitForAcknowledgement>d__19 <WaitForAcknowledgement>d__;
		<WaitForAcknowledgement>d__.<>t__builder = AsyncTaskMethodBuilder.Create();
		<WaitForAcknowledgement>d__.<>4__this = this;
		<WaitForAcknowledgement>d__.<>1__state = -1;
		<WaitForAcknowledgement>d__.<>t__builder.Start<LegalAgreements.<WaitForAcknowledgement>d__19>(ref <WaitForAcknowledgement>d__);
		return <WaitForAcknowledgement>d__.<>t__builder.Task;
	}

	// Token: 0x06002D59 RID: 11609 RVA: 0x0012740C File Offset: 0x0012560C
	private Task<bool> UpdateText(LegalAgreementTextAsset asset, string version)
	{
		LegalAgreements.<UpdateText>d__20 <UpdateText>d__;
		<UpdateText>d__.<>t__builder = AsyncTaskMethodBuilder<bool>.Create();
		<UpdateText>d__.<>4__this = this;
		<UpdateText>d__.asset = asset;
		<UpdateText>d__.version = version;
		<UpdateText>d__.<>1__state = -1;
		<UpdateText>d__.<>t__builder.Start<LegalAgreements.<UpdateText>d__20>(ref <UpdateText>d__);
		return <UpdateText>d__.<>t__builder.Task;
	}

	// Token: 0x06002D5A RID: 11610 RVA: 0x00127460 File Offset: 0x00125660
	public Task<bool> UpdateTextFromPlayFabTitleData(string key, string version, TMP_Text target)
	{
		LegalAgreements.<UpdateTextFromPlayFabTitleData>d__27 <UpdateTextFromPlayFabTitleData>d__;
		<UpdateTextFromPlayFabTitleData>d__.<>t__builder = AsyncTaskMethodBuilder<bool>.Create();
		<UpdateTextFromPlayFabTitleData>d__.<>4__this = this;
		<UpdateTextFromPlayFabTitleData>d__.key = key;
		<UpdateTextFromPlayFabTitleData>d__.version = version;
		<UpdateTextFromPlayFabTitleData>d__.target = target;
		<UpdateTextFromPlayFabTitleData>d__.<>1__state = -1;
		<UpdateTextFromPlayFabTitleData>d__.<>t__builder.Start<LegalAgreements.<UpdateTextFromPlayFabTitleData>d__27>(ref <UpdateTextFromPlayFabTitleData>d__);
		return <UpdateTextFromPlayFabTitleData>d__.<>t__builder.Task;
	}

	// Token: 0x06002D5B RID: 11611 RVA: 0x0004ED38 File Offset: 0x0004CF38
	private void OnPlayFabError(PlayFabError error)
	{
		this.state = -1;
	}

	// Token: 0x06002D5C RID: 11612 RVA: 0x0004ED41 File Offset: 0x0004CF41
	private void OnTitleDataReceived(string obj)
	{
		this.cachedText = obj;
		this.state = 1;
	}

	// Token: 0x06002D5D RID: 11613 RVA: 0x001274BC File Offset: 0x001256BC
	private Task<string> GetTitleDataAsync(string key)
	{
		LegalAgreements.<GetTitleDataAsync>d__30 <GetTitleDataAsync>d__;
		<GetTitleDataAsync>d__.<>t__builder = AsyncTaskMethodBuilder<string>.Create();
		<GetTitleDataAsync>d__.key = key;
		<GetTitleDataAsync>d__.<>1__state = -1;
		<GetTitleDataAsync>d__.<>t__builder.Start<LegalAgreements.<GetTitleDataAsync>d__30>(ref <GetTitleDataAsync>d__);
		return <GetTitleDataAsync>d__.<>t__builder.Task;
	}

	// Token: 0x06002D5E RID: 11614 RVA: 0x00127500 File Offset: 0x00125700
	private Task<Dictionary<string, string>> GetAcceptedAgreements(LegalAgreementTextAsset[] agreements)
	{
		LegalAgreements.<GetAcceptedAgreements>d__31 <GetAcceptedAgreements>d__;
		<GetAcceptedAgreements>d__.<>t__builder = AsyncTaskMethodBuilder<Dictionary<string, string>>.Create();
		<GetAcceptedAgreements>d__.agreements = agreements;
		<GetAcceptedAgreements>d__.<>1__state = -1;
		<GetAcceptedAgreements>d__.<>t__builder.Start<LegalAgreements.<GetAcceptedAgreements>d__31>(ref <GetAcceptedAgreements>d__);
		return <GetAcceptedAgreements>d__.<>t__builder.Task;
	}

	// Token: 0x06002D5F RID: 11615 RVA: 0x00127544 File Offset: 0x00125744
	private Task SubmitAcceptedAgreements(Dictionary<string, string> agreements)
	{
		LegalAgreements.<SubmitAcceptedAgreements>d__32 <SubmitAcceptedAgreements>d__;
		<SubmitAcceptedAgreements>d__.<>t__builder = AsyncTaskMethodBuilder.Create();
		<SubmitAcceptedAgreements>d__.agreements = agreements;
		<SubmitAcceptedAgreements>d__.<>1__state = -1;
		<SubmitAcceptedAgreements>d__.<>t__builder.Start<LegalAgreements.<SubmitAcceptedAgreements>d__32>(ref <SubmitAcceptedAgreements>d__);
		return <SubmitAcceptedAgreements>d__.<>t__builder.Task;
	}

	// Token: 0x06002D60 RID: 11616 RVA: 0x00127588 File Offset: 0x00125788
	public void TurnPage(int i)
	{
		this.tmpBody.pageToDisplay = Mathf.Clamp(this.tmpBody.pageToDisplay + i, 1, this.tmpBody.textInfo.pageCount);
		this.tmpPage.text = string.Format("page {0} of {1}", this.tmpBody.pageToDisplay, this.tmpBody.textInfo.pageCount);
		this.nextButton.SetActive(this.tmpBody.pageToDisplay < this.tmpBody.textInfo.pageCount);
		this.prevButton.SetActive(this.tmpBody.pageToDisplay > 1);
		this.ActivateAcceptButtonGroup();
	}

	// Token: 0x06002D61 RID: 11617 RVA: 0x00127644 File Offset: 0x00125844
	private void ActivateAcceptButtonGroup()
	{
		this.acceptButton.SetActive(this.tmpBody.pageToDisplay == this.tmpBody.textInfo.pageCount && !this.optional);
		this.yesNoButtons.SetActive(this.tmpBody.pageToDisplay == this.tmpBody.textInfo.pageCount && this.optional);
	}

	// Token: 0x040032AE RID: 12974
	private ControllerBehaviour controllerBehaviour;

	// Token: 0x040032B0 RID: 12976
	[SerializeField]
	private Transform uiParent;

	// Token: 0x040032B1 RID: 12977
	[SerializeField]
	private TMP_Text tmpBody;

	// Token: 0x040032B2 RID: 12978
	[SerializeField]
	private TMP_Text tmpTitle;

	// Token: 0x040032B3 RID: 12979
	[SerializeField]
	private TMP_Text tmpPage;

	// Token: 0x040032B4 RID: 12980
	[SerializeField]
	private LegalAgreementTextAsset[] legalAgreementScreens;

	// Token: 0x040032B5 RID: 12981
	[SerializeField]
	public GameObject acceptButton;

	// Token: 0x040032B6 RID: 12982
	[SerializeField]
	public GameObject yesNoButtons;

	// Token: 0x040032B7 RID: 12983
	[SerializeField]
	public GameObject nextButton;

	// Token: 0x040032B8 RID: 12984
	[SerializeField]
	public GameObject prevButton;

	// Token: 0x040032B9 RID: 12985
	private string cachedText;

	// Token: 0x040032BA RID: 12986
	private int state;

	// Token: 0x040032BB RID: 12987
	private bool optIn;

	// Token: 0x040032BC RID: 12988
	private bool optional;

	// Token: 0x040032BD RID: 12989
	[SerializeField]
	private float holdTime = 5f;

	// Token: 0x040032BE RID: 12990
	[SerializeField]
	private LineRenderer progressBar;
}
