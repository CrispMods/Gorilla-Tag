using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000362 RID: 866
public class UiAxis1dInspector : MonoBehaviour
{
	// Token: 0x0600141A RID: 5146 RVA: 0x000628DC File Offset: 0x00060ADC
	public void SetExtents(float minExtent, float maxExtent)
	{
		this.m_minExtent = minExtent;
		this.m_maxExtent = maxExtent;
	}

	// Token: 0x0600141B RID: 5147 RVA: 0x000628EC File Offset: 0x00060AEC
	public void SetName(string name)
	{
		this.m_nameLabel.text = name;
	}

	// Token: 0x0600141C RID: 5148 RVA: 0x000628FC File Offset: 0x00060AFC
	public void SetValue(float value)
	{
		this.m_valueLabel.text = string.Format("[{0}]", value.ToString("f2"));
		this.m_slider.minValue = Mathf.Min(value, this.m_minExtent);
		this.m_slider.maxValue = Mathf.Max(value, this.m_maxExtent);
		this.m_slider.value = value;
	}

	// Token: 0x0400163E RID: 5694
	[Header("Settings")]
	[SerializeField]
	private float m_minExtent;

	// Token: 0x0400163F RID: 5695
	[SerializeField]
	private float m_maxExtent = 1f;

	// Token: 0x04001640 RID: 5696
	[Header("Components")]
	[SerializeField]
	private TextMeshProUGUI m_nameLabel;

	// Token: 0x04001641 RID: 5697
	[SerializeField]
	private TextMeshProUGUI m_valueLabel;

	// Token: 0x04001642 RID: 5698
	[SerializeField]
	private Slider m_slider;
}
