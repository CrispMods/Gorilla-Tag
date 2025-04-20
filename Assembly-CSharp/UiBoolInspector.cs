using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x0200036F RID: 879
public class UiBoolInspector : MonoBehaviour
{
	// Token: 0x0600146E RID: 5230 RVA: 0x0003DC35 File Offset: 0x0003BE35
	public void SetName(string name)
	{
		this.m_nameLabel.text = name;
	}

	// Token: 0x0600146F RID: 5231 RVA: 0x0003DC43 File Offset: 0x0003BE43
	public void SetValue(bool value)
	{
		this.m_toggle.isOn = value;
	}

	// Token: 0x04001690 RID: 5776
	[Header("Components")]
	[SerializeField]
	private TextMeshProUGUI m_nameLabel;

	// Token: 0x04001691 RID: 5777
	[SerializeField]
	private Toggle m_toggle;
}
