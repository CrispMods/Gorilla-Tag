using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using PlayFab;
using TMPro;
using UnityEngine;

// Token: 0x0200070D RID: 1805
public class LegalAgreements : MonoBehaviour
{
	// Token: 0x170004BE RID: 1214
	// (get) Token: 0x06002CC3 RID: 11459 RVA: 0x000DD0CA File Offset: 0x000DB2CA
	// (set) Token: 0x06002CC4 RID: 11460 RVA: 0x000DD0D1 File Offset: 0x000DB2D1
	public static LegalAgreements instance { get; private set; }

	// Token: 0x06002CC5 RID: 11461 RVA: 0x000DD0D9 File Offset: 0x000DB2D9
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

	// Token: 0x06002CC6 RID: 11462 RVA: 0x000DD113 File Offset: 0x000DB313
	private void OnEnable()
	{
		this.controllerBehaviour.OnAction += this.PostUpdate;
	}

	// Token: 0x06002CC7 RID: 11463 RVA: 0x000DD12C File Offset: 0x000DB32C
	private void OnDisable()
	{
		this.controllerBehaviour.OnAction -= this.PostUpdate;
	}

	// Token: 0x06002CC8 RID: 11464 RVA: 0x000DD148 File Offset: 0x000DB348
	public void StartLegalAgreements()
	{
		LegalAgreements.<StartLegalAgreements>d__17 <StartLegalAgreements>d__;
		<StartLegalAgreements>d__.<>t__builder = AsyncVoidMethodBuilder.Create();
		<StartLegalAgreements>d__.<>4__this = this;
		<StartLegalAgreements>d__.<>1__state = -1;
		<StartLegalAgreements>d__.<>t__builder.Start<LegalAgreements.<StartLegalAgreements>d__17>(ref <StartLegalAgreements>d__);
	}

	// Token: 0x06002CC9 RID: 11465 RVA: 0x000DD180 File Offset: 0x000DB380
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

	// Token: 0x06002CCA RID: 11466 RVA: 0x000DD1E0 File Offset: 0x000DB3E0
	private Task WaitForAcknowledgement()
	{
		LegalAgreements.<WaitForAcknowledgement>d__19 <WaitForAcknowledgement>d__;
		<WaitForAcknowledgement>d__.<>t__builder = AsyncTaskMethodBuilder.Create();
		<WaitForAcknowledgement>d__.<>4__this = this;
		<WaitForAcknowledgement>d__.<>1__state = -1;
		<WaitForAcknowledgement>d__.<>t__builder.Start<LegalAgreements.<WaitForAcknowledgement>d__19>(ref <WaitForAcknowledgement>d__);
		return <WaitForAcknowledgement>d__.<>t__builder.Task;
	}

	// Token: 0x06002CCB RID: 11467 RVA: 0x000DD224 File Offset: 0x000DB424
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

	// Token: 0x06002CCC RID: 11468 RVA: 0x000DD278 File Offset: 0x000DB478
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

	// Token: 0x06002CCD RID: 11469 RVA: 0x000DD2D3 File Offset: 0x000DB4D3
	private void OnPlayFabError(PlayFabError error)
	{
		this.state = -1;
	}

	// Token: 0x06002CCE RID: 11470 RVA: 0x000DD2DC File Offset: 0x000DB4DC
	private void OnTitleDataReceived(string obj)
	{
		this.cachedText = obj;
		this.state = 1;
	}

	// Token: 0x06002CCF RID: 11471 RVA: 0x000DD2EC File Offset: 0x000DB4EC
	private Task<string> GetTitleDataAsync(string key)
	{
		LegalAgreements.<GetTitleDataAsync>d__30 <GetTitleDataAsync>d__;
		<GetTitleDataAsync>d__.<>t__builder = AsyncTaskMethodBuilder<string>.Create();
		<GetTitleDataAsync>d__.key = key;
		<GetTitleDataAsync>d__.<>1__state = -1;
		<GetTitleDataAsync>d__.<>t__builder.Start<LegalAgreements.<GetTitleDataAsync>d__30>(ref <GetTitleDataAsync>d__);
		return <GetTitleDataAsync>d__.<>t__builder.Task;
	}

	// Token: 0x06002CD0 RID: 11472 RVA: 0x000DD330 File Offset: 0x000DB530
	private Task<Dictionary<string, string>> GetAcceptedAgreements(LegalAgreementTextAsset[] agreements)
	{
		LegalAgreements.<GetAcceptedAgreements>d__31 <GetAcceptedAgreements>d__;
		<GetAcceptedAgreements>d__.<>t__builder = AsyncTaskMethodBuilder<Dictionary<string, string>>.Create();
		<GetAcceptedAgreements>d__.agreements = agreements;
		<GetAcceptedAgreements>d__.<>1__state = -1;
		<GetAcceptedAgreements>d__.<>t__builder.Start<LegalAgreements.<GetAcceptedAgreements>d__31>(ref <GetAcceptedAgreements>d__);
		return <GetAcceptedAgreements>d__.<>t__builder.Task;
	}

	// Token: 0x06002CD1 RID: 11473 RVA: 0x000DD374 File Offset: 0x000DB574
	private Task SubmitAcceptedAgreements(Dictionary<string, string> agreements)
	{
		LegalAgreements.<SubmitAcceptedAgreements>d__32 <SubmitAcceptedAgreements>d__;
		<SubmitAcceptedAgreements>d__.<>t__builder = AsyncTaskMethodBuilder.Create();
		<SubmitAcceptedAgreements>d__.agreements = agreements;
		<SubmitAcceptedAgreements>d__.<>1__state = -1;
		<SubmitAcceptedAgreements>d__.<>t__builder.Start<LegalAgreements.<SubmitAcceptedAgreements>d__32>(ref <SubmitAcceptedAgreements>d__);
		return <SubmitAcceptedAgreements>d__.<>t__builder.Task;
	}

	// Token: 0x06002CD2 RID: 11474 RVA: 0x000DD3B8 File Offset: 0x000DB5B8
	public void TurnPage(int i)
	{
		this.tmpBody.pageToDisplay = Mathf.Clamp(this.tmpBody.pageToDisplay + i, 1, this.tmpBody.textInfo.pageCount);
		this.tmpPage.text = string.Format("page {0} of {1}", this.tmpBody.pageToDisplay, this.tmpBody.textInfo.pageCount);
		this.nextButton.SetActive(this.tmpBody.pageToDisplay < this.tmpBody.textInfo.pageCount);
		this.prevButton.SetActive(this.tmpBody.pageToDisplay > 1);
		this.ActivateAcceptButtonGroup();
	}

	// Token: 0x06002CD3 RID: 11475 RVA: 0x000DD474 File Offset: 0x000DB674
	private void ActivateAcceptButtonGroup()
	{
		this.acceptButton.SetActive(this.tmpBody.pageToDisplay == this.tmpBody.textInfo.pageCount && !this.optional);
		this.yesNoButtons.SetActive(this.tmpBody.pageToDisplay == this.tmpBody.textInfo.pageCount && this.optional);
	}

	// Token: 0x04003217 RID: 12823
	private ControllerBehaviour controllerBehaviour;

	// Token: 0x04003219 RID: 12825
	[SerializeField]
	private Transform uiParent;

	// Token: 0x0400321A RID: 12826
	[SerializeField]
	private TMP_Text tmpBody;

	// Token: 0x0400321B RID: 12827
	[SerializeField]
	private TMP_Text tmpTitle;

	// Token: 0x0400321C RID: 12828
	[SerializeField]
	private TMP_Text tmpPage;

	// Token: 0x0400321D RID: 12829
	[SerializeField]
	private LegalAgreementTextAsset[] legalAgreementScreens;

	// Token: 0x0400321E RID: 12830
	[SerializeField]
	public GameObject acceptButton;

	// Token: 0x0400321F RID: 12831
	[SerializeField]
	public GameObject yesNoButtons;

	// Token: 0x04003220 RID: 12832
	[SerializeField]
	public GameObject nextButton;

	// Token: 0x04003221 RID: 12833
	[SerializeField]
	public GameObject prevButton;

	// Token: 0x04003222 RID: 12834
	private string cachedText;

	// Token: 0x04003223 RID: 12835
	private int state;

	// Token: 0x04003224 RID: 12836
	private bool optIn;

	// Token: 0x04003225 RID: 12837
	private bool optional;

	// Token: 0x04003226 RID: 12838
	[SerializeField]
	private float holdTime = 5f;

	// Token: 0x04003227 RID: 12839
	[SerializeField]
	private LineRenderer progressBar;
}
