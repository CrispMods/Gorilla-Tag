using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x0200042E RID: 1070
public class CreatorCodeSmallDisplay : MonoBehaviour
{
	// Token: 0x06001A76 RID: 6774 RVA: 0x00041D2C File Offset: 0x0003FF2C
	private void Awake()
	{
		this.codeText.text = "Creator Code: <none>";
		ATM_Manager.instance.smallDisplays.Add(this);
	}

	// Token: 0x06001A77 RID: 6775 RVA: 0x00041D50 File Offset: 0x0003FF50
	public void SetCode(string code)
	{
		if (code == "")
		{
			this.codeText.text = "Creator Code: <none>";
			return;
		}
		this.codeText.text = "Creator Code: " + code;
	}

	// Token: 0x06001A78 RID: 6776 RVA: 0x00041D86 File Offset: 0x0003FF86
	public void SuccessfulPurchase(string memberName)
	{
		if (!string.IsNullOrWhiteSpace(memberName))
		{
			this.codeText.text = "Supported: " + memberName + "!";
		}
	}

	// Token: 0x04001D48 RID: 7496
	public Text codeText;
}
