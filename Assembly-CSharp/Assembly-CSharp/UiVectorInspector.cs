using System;
using TMPro;
using UnityEngine;

// Token: 0x02000367 RID: 871
public class UiVectorInspector : MonoBehaviour
{
	// Token: 0x06001439 RID: 5177 RVA: 0x0006332D File Offset: 0x0006152D
	public void SetName(string name)
	{
		this.m_nameLabel.text = name;
	}

	// Token: 0x0600143A RID: 5178 RVA: 0x0006333B File Offset: 0x0006153B
	public void SetValue(bool value)
	{
		this.m_valueLabel.text = string.Format("[{0}]", value);
	}

	// Token: 0x04001661 RID: 5729
	[Header("Components")]
	[SerializeField]
	private TextMeshProUGUI m_nameLabel;

	// Token: 0x04001662 RID: 5730
	[SerializeField]
	private TextMeshProUGUI m_valueLabel;
}
