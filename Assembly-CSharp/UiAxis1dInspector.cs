using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x0200036D RID: 877
public class UiAxis1dInspector : MonoBehaviour
{
	// Token: 0x06001466 RID: 5222 RVA: 0x0003DBB4 File Offset: 0x0003BDB4
	public void SetExtents(float minExtent, float maxExtent)
	{
		this.m_minExtent = minExtent;
		this.m_maxExtent = maxExtent;
	}

	// Token: 0x06001467 RID: 5223 RVA: 0x0003DBC4 File Offset: 0x0003BDC4
	public void SetName(string name)
	{
		this.m_nameLabel.text = name;
	}

	// Token: 0x06001468 RID: 5224 RVA: 0x000BBE24 File Offset: 0x000BA024
	public void SetValue(float value)
	{
		this.m_valueLabel.text = string.Format("[{0}]", value.ToString("f2"));
		this.m_slider.minValue = Mathf.Min(value, this.m_minExtent);
		this.m_slider.maxValue = Mathf.Max(value, this.m_maxExtent);
		this.m_slider.value = value;
	}

	// Token: 0x04001686 RID: 5766
	[Header("Settings")]
	[SerializeField]
	private float m_minExtent;

	// Token: 0x04001687 RID: 5767
	[SerializeField]
	private float m_maxExtent = 1f;

	// Token: 0x04001688 RID: 5768
	[Header("Components")]
	[SerializeField]
	private TextMeshProUGUI m_nameLabel;

	// Token: 0x04001689 RID: 5769
	[SerializeField]
	private TextMeshProUGUI m_valueLabel;

	// Token: 0x0400168A RID: 5770
	[SerializeField]
	private Slider m_slider;
}
