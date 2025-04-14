using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000364 RID: 868
public class UiBoolInspector : MonoBehaviour
{
	// Token: 0x06001422 RID: 5154 RVA: 0x00062B63 File Offset: 0x00060D63
	public void SetName(string name)
	{
		this.m_nameLabel.text = name;
	}

	// Token: 0x06001423 RID: 5155 RVA: 0x00062B71 File Offset: 0x00060D71
	public void SetValue(bool value)
	{
		this.m_toggle.isOn = value;
	}

	// Token: 0x04001648 RID: 5704
	[Header("Components")]
	[SerializeField]
	private TextMeshProUGUI m_nameLabel;

	// Token: 0x04001649 RID: 5705
	[SerializeField]
	private Toggle m_toggle;
}
