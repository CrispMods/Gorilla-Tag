using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using PlayFab;
using TMPro;
using UnityEngine;

// Token: 0x0200070C RID: 1804
public class LegalAgreements : MonoBehaviour
{
	// Token: 0x170004BD RID: 1213
	// (get) Token: 0x06002CBB RID: 11451 RVA: 0x000DCC4A File Offset: 0x000DAE4A
	// (set) Token: 0x06002CBC RID: 11452 RVA: 0x000DCC51 File Offset: 0x000DAE51
	public static LegalAgreements instance { get; private set; }

	// Token: 0x06002CBD RID: 11453 RVA: 0x000DCC59 File Offset: 0x000DAE59
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

	// Token: 0x06002CBE RID: 11454 RVA: 0x000DCC93 File Offset: 0x000DAE93
	private void OnEnable()
	{
		this.controllerBehaviour.OnAction += this.PostUpdate;
	}

	// Token: 0x06002CBF RID: 11455 RVA: 0x000DCCAC File Offset: 0x000DAEAC
	private void OnDisable()
	{
		this.controllerBehaviour.OnAction -= this.PostUpdate;
	}

	// Token: 0x06002CC0 RID: 11456 RVA: 0x000DCCC8 File Offset: 0x000DAEC8
	public void StartLegalAgreements()
	{
		LegalAgreements.<StartLegalAgreements>d__17 <StartLegalAgreements>d__;
		<StartLegalAgreements>d__.<>t__builder = AsyncVoidMethodBuilder.Create();
		<StartLegalAgreements>d__.<>4__this = this;
		<StartLegalAgreements>d__.<>1__state = -1;
		<StartLegalAgreements>d__.<>t__builder.Start<LegalAgreements.<StartLegalAgreements>d__17>(ref <StartLegalAgreements>d__);
	}

	// Token: 0x06002CC1 RID: 11457 RVA: 0x000DCD00 File Offset: 0x000DAF00
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

	// Token: 0x06002CC2 RID: 11458 RVA: 0x000DCD60 File Offset: 0x000DAF60
	private Task WaitForAcknowledgement()
	{
		LegalAgreements.<WaitForAcknowledgement>d__19 <WaitForAcknowledgement>d__;
		<WaitForAcknowledgement>d__.<>t__builder = AsyncTaskMethodBuilder.Create();
		<WaitForAcknowledgement>d__.<>4__this = this;
		<WaitForAcknowledgement>d__.<>1__state = -1;
		<WaitForAcknowledgement>d__.<>t__builder.Start<LegalAgreements.<WaitForAcknowledgement>d__19>(ref <WaitForAcknowledgement>d__);
		return <WaitForAcknowledgement>d__.<>t__builder.Task;
	}

	// Token: 0x06002CC3 RID: 11459 RVA: 0x000DCDA4 File Offset: 0x000DAFA4
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

	// Token: 0x06002CC4 RID: 11460 RVA: 0x000DCDF8 File Offset: 0x000DAFF8
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

	// Token: 0x06002CC5 RID: 11461 RVA: 0x000DCE53 File Offset: 0x000DB053
	private void OnPlayFabError(PlayFabError error)
	{
		this.state = -1;
	}

	// Token: 0x06002CC6 RID: 11462 RVA: 0x000DCE5C File Offset: 0x000DB05C
	private void OnTitleDataReceived(string obj)
	{
		this.cachedText = obj;
		this.state = 1;
	}

	// Token: 0x06002CC7 RID: 11463 RVA: 0x000DCE6C File Offset: 0x000DB06C
	private Task<string> GetTitleDataAsync(string key)
	{
		LegalAgreements.<GetTitleDataAsync>d__30 <GetTitleDataAsync>d__;
		<GetTitleDataAsync>d__.<>t__builder = AsyncTaskMethodBuilder<string>.Create();
		<GetTitleDataAsync>d__.key = key;
		<GetTitleDataAsync>d__.<>1__state = -1;
		<GetTitleDataAsync>d__.<>t__builder.Start<LegalAgreements.<GetTitleDataAsync>d__30>(ref <GetTitleDataAsync>d__);
		return <GetTitleDataAsync>d__.<>t__builder.Task;
	}

	// Token: 0x06002CC8 RID: 11464 RVA: 0x000DCEB0 File Offset: 0x000DB0B0
	private Task<Dictionary<string, string>> GetAcceptedAgreements(LegalAgreementTextAsset[] agreements)
	{
		LegalAgreements.<GetAcceptedAgreements>d__31 <GetAcceptedAgreements>d__;
		<GetAcceptedAgreements>d__.<>t__builder = AsyncTaskMethodBuilder<Dictionary<string, string>>.Create();
		<GetAcceptedAgreements>d__.agreements = agreements;
		<GetAcceptedAgreements>d__.<>1__state = -1;
		<GetAcceptedAgreements>d__.<>t__builder.Start<LegalAgreements.<GetAcceptedAgreements>d__31>(ref <GetAcceptedAgreements>d__);
		return <GetAcceptedAgreements>d__.<>t__builder.Task;
	}

	// Token: 0x06002CC9 RID: 11465 RVA: 0x000DCEF4 File Offset: 0x000DB0F4
	private Task SubmitAcceptedAgreements(Dictionary<string, string> agreements)
	{
		LegalAgreements.<SubmitAcceptedAgreements>d__32 <SubmitAcceptedAgreements>d__;
		<SubmitAcceptedAgreements>d__.<>t__builder = AsyncTaskMethodBuilder.Create();
		<SubmitAcceptedAgreements>d__.agreements = agreements;
		<SubmitAcceptedAgreements>d__.<>1__state = -1;
		<SubmitAcceptedAgreements>d__.<>t__builder.Start<LegalAgreements.<SubmitAcceptedAgreements>d__32>(ref <SubmitAcceptedAgreements>d__);
		return <SubmitAcceptedAgreements>d__.<>t__builder.Task;
	}

	// Token: 0x06002CCA RID: 11466 RVA: 0x000DCF38 File Offset: 0x000DB138
	public void TurnPage(int i)
	{
		this.tmpBody.pageToDisplay = Mathf.Clamp(this.tmpBody.pageToDisplay + i, 1, this.tmpBody.textInfo.pageCount);
		this.tmpPage.text = string.Format("page {0} of {1}", this.tmpBody.pageToDisplay, this.tmpBody.textInfo.pageCount);
		this.nextButton.SetActive(this.tmpBody.pageToDisplay < this.tmpBody.textInfo.pageCount);
		this.prevButton.SetActive(this.tmpBody.pageToDisplay > 1);
		this.ActivateAcceptButtonGroup();
	}

	// Token: 0x06002CCB RID: 11467 RVA: 0x000DCFF4 File Offset: 0x000DB1F4
	private void ActivateAcceptButtonGroup()
	{
		this.acceptButton.SetActive(this.tmpBody.pageToDisplay == this.tmpBody.textInfo.pageCount && !this.optional);
		this.yesNoButtons.SetActive(this.tmpBody.pageToDisplay == this.tmpBody.textInfo.pageCount && this.optional);
	}

	// Token: 0x04003211 RID: 12817
	private ControllerBehaviour controllerBehaviour;

	// Token: 0x04003213 RID: 12819
	[SerializeField]
	private Transform uiParent;

	// Token: 0x04003214 RID: 12820
	[SerializeField]
	private TMP_Text tmpBody;

	// Token: 0x04003215 RID: 12821
	[SerializeField]
	private TMP_Text tmpTitle;

	// Token: 0x04003216 RID: 12822
	[SerializeField]
	private TMP_Text tmpPage;

	// Token: 0x04003217 RID: 12823
	[SerializeField]
	private LegalAgreementTextAsset[] legalAgreementScreens;

	// Token: 0x04003218 RID: 12824
	[SerializeField]
	public GameObject acceptButton;

	// Token: 0x04003219 RID: 12825
	[SerializeField]
	public GameObject yesNoButtons;

	// Token: 0x0400321A RID: 12826
	[SerializeField]
	public GameObject nextButton;

	// Token: 0x0400321B RID: 12827
	[SerializeField]
	public GameObject prevButton;

	// Token: 0x0400321C RID: 12828
	private string cachedText;

	// Token: 0x0400321D RID: 12829
	private int state;

	// Token: 0x0400321E RID: 12830
	private bool optIn;

	// Token: 0x0400321F RID: 12831
	private bool optional;

	// Token: 0x04003220 RID: 12832
	[SerializeField]
	private float holdTime = 5f;

	// Token: 0x04003221 RID: 12833
	[SerializeField]
	private LineRenderer progressBar;
}
