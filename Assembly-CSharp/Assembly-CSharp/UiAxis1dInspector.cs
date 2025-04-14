using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000362 RID: 866
public class UiAxis1dInspector : MonoBehaviour
{
	// Token: 0x0600141D RID: 5149 RVA: 0x00062C60 File Offset: 0x00060E60
	public void SetExtents(float minExtent, float maxExtent)
	{
		this.m_minExtent = minExtent;
		this.m_maxExtent = maxExtent;
	}

	// Token: 0x0600141E RID: 5150 RVA: 0x00062C70 File Offset: 0x00060E70
	public void SetName(string name)
	{
		this.m_nameLabel.text = name;
	}

	// Token: 0x0600141F RID: 5151 RVA: 0x00062C80 File Offset: 0x00060E80
	public void SetValue(float value)
	{
		this.m_valueLabel.text = string.Format("[{0}]", value.ToString("f2"));
		this.m_slider.minValue = Mathf.Min(value, this.m_minExtent);
		this.m_slider.maxValue = Mathf.Max(value, this.m_maxExtent);
		this.m_slider.value = value;
	}

	// Token: 0x0400163F RID: 5695
	[Header("Settings")]
	[SerializeField]
	private float m_minExtent;

	// Token: 0x04001640 RID: 5696
	[SerializeField]
	private float m_maxExtent = 1f;

	// Token: 0x04001641 RID: 5697
	[Header("Components")]
	[SerializeField]
	private TextMeshProUGUI m_nameLabel;

	// Token: 0x04001642 RID: 5698
	[SerializeField]
	private TextMeshProUGUI m_valueLabel;

	// Token: 0x04001643 RID: 5699
	[SerializeField]
	private Slider m_slider;
}
