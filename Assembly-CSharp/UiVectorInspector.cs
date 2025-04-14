using System;
using TMPro;
using UnityEngine;

// Token: 0x02000367 RID: 871
public class UiVectorInspector : MonoBehaviour
{
	// Token: 0x06001436 RID: 5174 RVA: 0x00062FA9 File Offset: 0x000611A9
	public void SetName(string name)
	{
		this.m_nameLabel.text = name;
	}

	// Token: 0x06001437 RID: 5175 RVA: 0x00062FB7 File Offset: 0x000611B7
	public void SetValue(bool value)
	{
		this.m_valueLabel.text = string.Format("[{0}]", value);
	}

	// Token: 0x04001660 RID: 5728
	[Header("Components")]
	[SerializeField]
	private TextMeshProUGUI m_nameLabel;

	// Token: 0x04001661 RID: 5729
	[SerializeField]
	private TextMeshProUGUI m_valueLabel;
}
