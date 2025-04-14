using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000364 RID: 868
public class UiBoolInspector : MonoBehaviour
{
	// Token: 0x06001425 RID: 5157 RVA: 0x00062EE7 File Offset: 0x000610E7
	public void SetName(string name)
	{
		this.m_nameLabel.text = name;
	}

	// Token: 0x06001426 RID: 5158 RVA: 0x00062EF5 File Offset: 0x000610F5
	public void SetValue(bool value)
	{
		this.m_toggle.isOn = value;
	}

	// Token: 0x04001649 RID: 5705
	[Header("Components")]
	[SerializeField]
	private TextMeshProUGUI m_nameLabel;

	// Token: 0x0400164A RID: 5706
	[SerializeField]
	private Toggle m_toggle;
}
