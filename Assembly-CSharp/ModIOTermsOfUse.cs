﻿using System;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using ModIO;
using TMPro;
using UnityEngine;

// Token: 0x0200068D RID: 1677
public class ModIOTermsOfUse : MonoBehaviour
{
	// Token: 0x06002990 RID: 10640 RVA: 0x0004C12C File Offset: 0x0004A32C
	private void Awake()
	{
		this.controllerBehaviour = base.GetComponentInChildren<ControllerBehaviour>(true);
	}

	// Token: 0x06002991 RID: 10641 RVA: 0x0004C13B File Offset: 0x0004A33B
	private void OnEnable()
	{
		this.controllerBehaviour.OnAction += this.PostUpdate;
	}

	// Token: 0x06002992 RID: 10642 RVA: 0x0004C154 File Offset: 0x0004A354
	private void OnDisable()
	{
		this.controllerBehaviour.OnAction -= this.PostUpdate;
	}

	// Token: 0x06002993 RID: 10643 RVA: 0x0004C16D File Offset: 0x0004A36D
	public void Initialize(TermsOfUse terms, Action<bool> callback)
	{
		if (terms.hash.md5hash.Length != 0)
		{
			this.termsOfUse = terms;
			this.hasTermsOfUse = true;
			this.termsAcknowledgedCallback = callback;
		}
	}

	// Token: 0x06002994 RID: 10644 RVA: 0x0011680C File Offset: 0x00114A0C
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
		if (this.waitingForAcknowledge)
		{
			this.acceptButtonDown = this.controllerBehaviour.ButtonDown;
		}
	}

	// Token: 0x06002995 RID: 10645 RVA: 0x0011685C File Offset: 0x00114A5C
	private void Start()
	{
		ModIOTermsOfUse.<Start>d__23 <Start>d__;
		<Start>d__.<>t__builder = AsyncVoidMethodBuilder.Create();
		<Start>d__.<>4__this = this;
		<Start>d__.<>1__state = -1;
		<Start>d__.<>t__builder.Start<ModIOTermsOfUse.<Start>d__23>(ref <Start>d__);
	}

	// Token: 0x06002996 RID: 10646 RVA: 0x00116894 File Offset: 0x00114A94
	private Task<bool> UpdateTextFromTerms()
	{
		ModIOTermsOfUse.<UpdateTextFromTerms>d__24 <UpdateTextFromTerms>d__;
		<UpdateTextFromTerms>d__.<>t__builder = AsyncTaskMethodBuilder<bool>.Create();
		<UpdateTextFromTerms>d__.<>4__this = this;
		<UpdateTextFromTerms>d__.<>1__state = -1;
		<UpdateTextFromTerms>d__.<>t__builder.Start<ModIOTermsOfUse.<UpdateTextFromTerms>d__24>(ref <UpdateTextFromTerms>d__);
		return <UpdateTextFromTerms>d__.<>t__builder.Task;
	}

	// Token: 0x06002997 RID: 10647 RVA: 0x001168D8 File Offset: 0x00114AD8
	public Task<bool> UpdateTextWithFullTerms()
	{
		ModIOTermsOfUse.<UpdateTextWithFullTerms>d__25 <UpdateTextWithFullTerms>d__;
		<UpdateTextWithFullTerms>d__.<>t__builder = AsyncTaskMethodBuilder<bool>.Create();
		<UpdateTextWithFullTerms>d__.<>4__this = this;
		<UpdateTextWithFullTerms>d__.<>1__state = -1;
		<UpdateTextWithFullTerms>d__.<>t__builder.Start<ModIOTermsOfUse.<UpdateTextWithFullTerms>d__25>(ref <UpdateTextWithFullTerms>d__);
		return <UpdateTextWithFullTerms>d__.<>t__builder.Task;
	}

	// Token: 0x06002998 RID: 10648 RVA: 0x0011691C File Offset: 0x00114B1C
	private string FormatAgreementText(CurrentAgreement agreement)
	{
		string text = string.Concat(new string[]
		{
			agreement.name,
			"\n\nEffective Date: ",
			agreement.dateLive.ToLongDateString(),
			"\n\n",
			agreement.content
		});
		text = Regex.Replace(text, "<!--[^>]*(-->)", "");
		text = text.Replace("<h1>", "<b>");
		text = text.Replace("</h1>", "</b>");
		text = text.Replace("<h2>", "<b>");
		text = text.Replace("</h2>", "</b>");
		text = text.Replace("<h3>", "<b>");
		text = text.Replace("</h3>", "</b>");
		text = text.Replace("<hr>", "");
		text = text.Replace("<br>", "\n");
		text = text.Replace("</li>", "</indent>\n");
		text = text.Replace("<strong>", "<b>");
		text = text.Replace("</strong>", "</b>");
		text = text.Replace("<em>", "<i>");
		text = text.Replace("</em>", "</i>");
		text = Regex.Replace(text, "<a[^>]*>{1}", "");
		text = text.Replace("</a>", "");
		Match match = Regex.Match(text, "<p[^>]*align:center[^>]*>{1}");
		while (match.Success)
		{
			text = text.Remove(match.Index, match.Length);
			text = text.Insert(match.Index, "\n<align=\"center\">");
			int startIndex = text.IndexOf("</p>", match.Index, StringComparison.Ordinal);
			text = text.Remove(startIndex, 4);
			text = text.Insert(startIndex, "</align>");
			match = Regex.Match(text, "<p[^>]*align:center[^>]*>{1}");
		}
		text = text.Replace("<p>", "\n");
		text = text.Replace("</p>", "");
		text = Regex.Replace(text, "<ol[^>]*>{1}", "<ol>");
		int num = text.IndexOf("<ol>", StringComparison.OrdinalIgnoreCase);
		bool flag = num != -1;
		while (flag)
		{
			int num2 = text.IndexOf("</ol>", num, StringComparison.OrdinalIgnoreCase);
			text = text.Remove(num, "<ol>".Length);
			int num3 = text.IndexOf("<li>", num, StringComparison.OrdinalIgnoreCase);
			bool flag2 = num3 != -1;
			int num4 = 0;
			while (flag2)
			{
				text = text.Remove(num3, "<li>".Length);
				text = text.Insert(num3, this.GetStringForListItemIdx_LowerAlpha(num4++));
				num2 = text.IndexOf("</ol>", num, StringComparison.OrdinalIgnoreCase);
				num3 = text.IndexOf("<li>", num, StringComparison.OrdinalIgnoreCase);
				flag2 = (num3 != -1 && num3 < num2);
			}
			text = text.Remove(num2, "</ol>".Length);
			num = text.IndexOf("<ol>", StringComparison.OrdinalIgnoreCase);
			flag = (num != -1);
		}
		text = Regex.Replace(text, "<ul[^>]*>{1}", "<ul>");
		int num5 = text.IndexOf("<ul>", StringComparison.OrdinalIgnoreCase);
		bool flag3 = num5 != -1;
		while (flag3)
		{
			int num6 = text.IndexOf("</ul>", num5, StringComparison.OrdinalIgnoreCase);
			text = text.Remove(num5, "<ul>".Length);
			int num7 = text.IndexOf("<li>", num5, StringComparison.OrdinalIgnoreCase);
			bool flag4 = num7 != -1;
			while (flag4)
			{
				text = text.Remove(num7, "<li>".Length);
				text = text.Insert(num7, "  - <indent=5%>");
				num6 = text.IndexOf("</ul>", num5, StringComparison.OrdinalIgnoreCase);
				num7 = text.IndexOf("<li>", num5, StringComparison.OrdinalIgnoreCase);
				flag4 = (num7 != -1 && num7 < num6);
			}
			text = text.Remove(num6, "</ul>".Length);
			num5 = text.IndexOf("<ul>", StringComparison.OrdinalIgnoreCase);
			flag3 = (num5 != -1);
		}
		text = Regex.Replace(text, "<table[^>]*>{1}", "");
		text = text.Replace("<tbody>", "");
		text = text.Replace("<tr>", "");
		text = text.Replace("<td>", "");
		text = text.Replace("<center>", "");
		text = text.Replace("</table>", "");
		text = text.Replace("</tbody>", "");
		text = text.Replace("</tr>", "\n");
		text = text.Replace("</td>", "");
		return text.Replace("</center>", "");
	}

	// Token: 0x06002999 RID: 10649 RVA: 0x00116DA0 File Offset: 0x00114FA0
	private string GetStringForListItemIdx_LowerAlpha(int idx)
	{
		switch (idx)
		{
		case 0:
			return "  a. <indent=5%>";
		case 1:
			return "  b. <indent=5%>";
		case 2:
			return "  c. <indent=5%>";
		case 3:
			return "  d. <indent=5%>";
		case 4:
			return "  e. <indent=5%>";
		case 5:
			return "  f. <indent=5%>";
		case 6:
			return "  g. <indent=5%>";
		case 7:
			return "  h. <indent=5%>";
		case 8:
			return "  i. <indent=5%>";
		case 9:
			return "  j. <indent=5%>";
		case 10:
			return "  k. <indent=5%>";
		case 11:
			return "  l. <indent=5%>";
		case 12:
			return "  m. <indent=5%>";
		case 13:
			return "  n. <indent=5%>";
		case 14:
			return "  o. <indent=5%>";
		case 15:
			return "  p. <indent=5%>";
		case 16:
			return "  q. <indent=5%>";
		case 17:
			return "  r. <indent=5%>";
		case 18:
			return "  s. <indent=5%>";
		case 19:
			return "  t. <indent=5%>";
		case 20:
			return "  u. <indent=5%>";
		case 21:
			return "  v. <indent=5%>";
		case 22:
			return "  w. <indent=5%>";
		case 23:
			return "  x. <indent=5%>";
		case 24:
			return "  y. <indent=5%>";
		case 25:
			return "  z. <indent=5%>";
		default:
			return "";
		}
	}

	// Token: 0x0600299A RID: 10650 RVA: 0x00116EC4 File Offset: 0x001150C4
	private Task WaitForAcknowledgement()
	{
		ModIOTermsOfUse.<WaitForAcknowledgement>d__28 <WaitForAcknowledgement>d__;
		<WaitForAcknowledgement>d__.<>t__builder = AsyncTaskMethodBuilder.Create();
		<WaitForAcknowledgement>d__.<>4__this = this;
		<WaitForAcknowledgement>d__.<>1__state = -1;
		<WaitForAcknowledgement>d__.<>t__builder.Start<ModIOTermsOfUse.<WaitForAcknowledgement>d__28>(ref <WaitForAcknowledgement>d__);
		return <WaitForAcknowledgement>d__.<>t__builder.Task;
	}

	// Token: 0x0600299B RID: 10651 RVA: 0x00116F08 File Offset: 0x00115108
	public void TurnPage(int i)
	{
		this.tmpBody.pageToDisplay = Mathf.Clamp(this.tmpBody.pageToDisplay + i, 1, this.tmpBody.textInfo.pageCount);
		this.tmpPage.text = string.Format("page {0} of {1}", this.tmpBody.pageToDisplay, this.tmpBody.textInfo.pageCount);
		this.nextButton.SetActive(this.tmpBody.pageToDisplay < this.tmpBody.textInfo.pageCount);
		this.prevButton.SetActive(this.tmpBody.pageToDisplay > 1);
		this.ActivateAcceptButtonGroup();
	}

	// Token: 0x0600299C RID: 10652 RVA: 0x00116FC4 File Offset: 0x001151C4
	private void ActivateAcceptButtonGroup()
	{
		bool active = this.tmpBody.pageToDisplay == this.tmpBody.textInfo.pageCount;
		this.yesNoButtons.SetActive(active);
		this.waitingForAcknowledge = active;
	}

	// Token: 0x0600299D RID: 10653 RVA: 0x0004C196 File Offset: 0x0004A396
	public void Acknowledge(bool didAccept)
	{
		this.accepted = didAccept;
	}

	// Token: 0x04002EC2 RID: 11970
	[SerializeField]
	private Transform uiParent;

	// Token: 0x04002EC3 RID: 11971
	[SerializeField]
	private string title;

	// Token: 0x04002EC4 RID: 11972
	[SerializeField]
	private TMP_Text tmpBody;

	// Token: 0x04002EC5 RID: 11973
	[SerializeField]
	private TMP_Text tmpTitle;

	// Token: 0x04002EC6 RID: 11974
	[SerializeField]
	private TMP_Text tmpPage;

	// Token: 0x04002EC7 RID: 11975
	[SerializeField]
	public GameObject yesNoButtons;

	// Token: 0x04002EC8 RID: 11976
	[SerializeField]
	public GameObject nextButton;

	// Token: 0x04002EC9 RID: 11977
	[SerializeField]
	public GameObject prevButton;

	// Token: 0x04002ECA RID: 11978
	private TermsOfUse termsOfUse;

	// Token: 0x04002ECB RID: 11979
	private bool hasTermsOfUse;

	// Token: 0x04002ECC RID: 11980
	private Action<bool> termsAcknowledgedCallback;

	// Token: 0x04002ECD RID: 11981
	private string cachedTermsText;

	// Token: 0x04002ECE RID: 11982
	private bool waitingForAcknowledge;

	// Token: 0x04002ECF RID: 11983
	private bool accepted;

	// Token: 0x04002ED0 RID: 11984
	private bool acceptButtonDown;

	// Token: 0x04002ED1 RID: 11985
	[SerializeField]
	private float holdTime = 5f;

	// Token: 0x04002ED2 RID: 11986
	[SerializeField]
	private LineRenderer progressBar;

	// Token: 0x04002ED3 RID: 11987
	private ControllerBehaviour controllerBehaviour;
}
