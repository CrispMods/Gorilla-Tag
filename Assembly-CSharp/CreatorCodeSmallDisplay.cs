using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000422 RID: 1058
public class CreatorCodeSmallDisplay : MonoBehaviour
{
	// Token: 0x06001A22 RID: 6690 RVA: 0x00080A9B File Offset: 0x0007EC9B
	private void Awake()
	{
		this.codeText.text = "Creator Code: <none>";
		ATM_Manager.instance.smallDisplays.Add(this);
	}

	// Token: 0x06001A23 RID: 6691 RVA: 0x00080ABF File Offset: 0x0007ECBF
	public void SetCode(string code)
	{
		if (code == "")
		{
			this.codeText.text = "Creator Code: <none>";
			return;
		}
		this.codeText.text = "Creator Code: " + code;
	}

	// Token: 0x06001A24 RID: 6692 RVA: 0x00080AF5 File Offset: 0x0007ECF5
	public void SuccessfulPurchase(string memberName)
	{
		if (!string.IsNullOrWhiteSpace(memberName))
		{
			this.codeText.text = "Supported: " + memberName + "!";
		}
	}

	// Token: 0x04001CF9 RID: 7417
	public Text codeText;
}
