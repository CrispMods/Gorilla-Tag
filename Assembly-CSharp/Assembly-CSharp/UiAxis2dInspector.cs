using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000363 RID: 867
public class UiAxis2dInspector : MonoBehaviour
{
	// Token: 0x06001421 RID: 5153 RVA: 0x00062CFB File Offset: 0x00060EFB
	public void SetExtents(Vector2 xExtent, Vector2 yExtent)
	{
		this.m_xExtent = xExtent;
		this.m_yExtent = yExtent;
	}

	// Token: 0x06001422 RID: 5154 RVA: 0x00062D0B File Offset: 0x00060F0B
	public void SetName(string name)
	{
		this.m_nameLabel.text = name;
	}

	// Token: 0x06001423 RID: 5155 RVA: 0x00062D1C File Offset: 0x00060F1C
	public void SetValue(bool isTouching, Vector2 value)
	{
		this.m_handle.color = (isTouching ? Color.white : new Color(0.2f, 0.2f, 0.2f));
		Vector2 vector = new Vector2(Mathf.Clamp(value.x, this.m_xExtent.x, this.m_xExtent.y), Mathf.Clamp(value.y, this.m_yExtent.x, this.m_yExtent.y));
		this.m_valueLabel.text = string.Concat(new string[]
		{
			"[",
			vector.x.ToString("f2"),
			", ",
			vector.y.ToString("f2"),
			"]"
		});
		RectTransform component = this.m_handle.transform.parent.GetComponent<RectTransform>();
		Vector2 vector2 = (component != null) ? new Vector2(Mathf.Abs(component.sizeDelta.x), Mathf.Abs(component.sizeDelta.y)) : new Vector2(Mathf.Abs(this.m_xExtent.y - this.m_xExtent.x), Mathf.Abs(this.m_yExtent.y - this.m_yExtent.x));
		this.m_handle.transform.localPosition = new Vector3(vector.x * vector2.x * 0.5f, vector.y * vector2.y * 0.5f, 0f);
	}

	// Token: 0x04001644 RID: 5700
	[Header("Settings")]
	[SerializeField]
	private Vector2 m_xExtent = new Vector2(-1f, 1f);

	// Token: 0x04001645 RID: 5701
	[SerializeField]
	private Vector2 m_yExtent = new Vector2(-1f, 1f);

	// Token: 0x04001646 RID: 5702
	[Header("Components")]
	[SerializeField]
	private TextMeshProUGUI m_nameLabel;

	// Token: 0x04001647 RID: 5703
	[SerializeField]
	private TextMeshProUGUI m_valueLabel;

	// Token: 0x04001648 RID: 5704
	[SerializeField]
	private Image m_handle;
}
