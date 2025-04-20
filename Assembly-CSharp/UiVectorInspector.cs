using System;
using TMPro;
using UnityEngine;

// Token: 0x02000372 RID: 882
public class UiVectorInspector : MonoBehaviour
{
	// Token: 0x06001482 RID: 5250 RVA: 0x0003DD56 File Offset: 0x0003BF56
	public void SetName(string name)
	{
		this.m_nameLabel.text = name;
	}

	// Token: 0x06001483 RID: 5251 RVA: 0x0003DD64 File Offset: 0x0003BF64
	public void SetValue(bool value)
	{
		this.m_valueLabel.text = string.Format("[{0}]", value);
	}

	// Token: 0x040016A8 RID: 5800
	[Header("Components")]
	[SerializeField]
	private TextMeshProUGUI m_nameLabel;

	// Token: 0x040016A9 RID: 5801
	[SerializeField]
	private TextMeshProUGUI m_valueLabel;
}
