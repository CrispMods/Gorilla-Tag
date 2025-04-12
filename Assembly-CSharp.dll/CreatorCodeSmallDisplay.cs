using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000422 RID: 1058
public class CreatorCodeSmallDisplay : MonoBehaviour
{
	// Token: 0x06001A25 RID: 6693 RVA: 0x000409F3 File Offset: 0x0003EBF3
	private void Awake()
	{
		this.codeText.text = "Creator Code: <none>";
		ATM_Manager.instance.smallDisplays.Add(this);
	}

	// Token: 0x06001A26 RID: 6694 RVA: 0x00040A17 File Offset: 0x0003EC17
	public void SetCode(string code)
	{
		if (code == "")
		{
			this.codeText.text = "Creator Code: <none>";
			return;
		}
		this.codeText.text = "Creator Code: " + code;
	}

	// Token: 0x06001A27 RID: 6695 RVA: 0x00040A4D File Offset: 0x0003EC4D
	public void SuccessfulPurchase(string memberName)
	{
		if (!string.IsNullOrWhiteSpace(memberName))
		{
			this.codeText.text = "Supported: " + memberName + "!";
		}
	}

	// Token: 0x04001CFA RID: 7418
	public Text codeText;
}
