using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000309 RID: 777
public class DebugUISample : MonoBehaviour
{
	// Token: 0x0600128E RID: 4750 RVA: 0x000B1830 File Offset: 0x000AFA30
	private void Start()
	{
		DebugUIBuilder.instance.AddButton("Button Pressed", new DebugUIBuilder.OnClick(this.LogButtonPressed), -1, 0, false);
		DebugUIBuilder.instance.AddLabel("Label", 0);
		RectTransform rectTransform = DebugUIBuilder.instance.AddSlider("Slider", 1f, 10f, new DebugUIBuilder.OnSlider(this.SliderPressed), true, 0);
		Text[] componentsInChildren = rectTransform.GetComponentsInChildren<Text>();
		this.sliderText = componentsInChildren[1];
		this.sliderText.text = rectTransform.GetComponentInChildren<Slider>().value.ToString();
		DebugUIBuilder.instance.AddDivider(0);
		DebugUIBuilder.instance.AddToggle("Toggle", new DebugUIBuilder.OnToggleValueChange(this.TogglePressed), 0);
		DebugUIBuilder.instance.AddRadio("Radio1", "group", delegate(Toggle t)
		{
			this.RadioPressed("Radio1", "group", t);
		}, 0);
		DebugUIBuilder.instance.AddRadio("Radio2", "group", delegate(Toggle t)
		{
			this.RadioPressed("Radio2", "group", t);
		}, 0);
		DebugUIBuilder.instance.AddLabel("Secondary Tab", 1);
		DebugUIBuilder.instance.AddDivider(1);
		DebugUIBuilder.instance.AddRadio("Side Radio 1", "group2", delegate(Toggle t)
		{
			this.RadioPressed("Side Radio 1", "group2", t);
		}, 1);
		DebugUIBuilder.instance.AddRadio("Side Radio 2", "group2", delegate(Toggle t)
		{
			this.RadioPressed("Side Radio 2", "group2", t);
		}, 1);
		DebugUIBuilder.instance.Show();
		this.inMenu = true;
	}

	// Token: 0x0600128F RID: 4751 RVA: 0x000B19A4 File Offset: 0x000AFBA4
	public void TogglePressed(Toggle t)
	{
		Debug.Log("Toggle pressed. Is on? " + t.isOn.ToString());
	}

	// Token: 0x06001290 RID: 4752 RVA: 0x000B19D0 File Offset: 0x000AFBD0
	public void RadioPressed(string radioLabel, string group, Toggle t)
	{
		Debug.Log(string.Concat(new string[]
		{
			"Radio value changed: ",
			radioLabel,
			", from group ",
			group,
			". New value: ",
			t.isOn.ToString()
		}));
	}

	// Token: 0x06001291 RID: 4753 RVA: 0x0003CB88 File Offset: 0x0003AD88
	public void SliderPressed(float f)
	{
		Debug.Log("Slider: " + f.ToString());
		this.sliderText.text = f.ToString();
	}

	// Token: 0x06001292 RID: 4754 RVA: 0x000B1A20 File Offset: 0x000AFC20
	private void Update()
	{
		if (OVRInput.GetDown(OVRInput.Button.Two, OVRInput.Controller.Active) || OVRInput.GetDown(OVRInput.Button.Start, OVRInput.Controller.Active))
		{
			if (this.inMenu)
			{
				DebugUIBuilder.instance.Hide();
			}
			else
			{
				DebugUIBuilder.instance.Show();
			}
			this.inMenu = !this.inMenu;
		}
	}

	// Token: 0x06001293 RID: 4755 RVA: 0x0003CBB2 File Offset: 0x0003ADB2
	private void LogButtonPressed()
	{
		Debug.Log("Button pressed");
	}

	// Token: 0x04001485 RID: 5253
	private bool inMenu;

	// Token: 0x04001486 RID: 5254
	private Text sliderText;
}
