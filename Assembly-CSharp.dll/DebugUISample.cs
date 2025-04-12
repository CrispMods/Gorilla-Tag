using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020002FE RID: 766
public class DebugUISample : MonoBehaviour
{
	// Token: 0x06001245 RID: 4677 RVA: 0x000AEF98 File Offset: 0x000AD198
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

	// Token: 0x06001246 RID: 4678 RVA: 0x000AF10C File Offset: 0x000AD30C
	public void TogglePressed(Toggle t)
	{
		Debug.Log("Toggle pressed. Is on? " + t.isOn.ToString());
	}

	// Token: 0x06001247 RID: 4679 RVA: 0x000AF138 File Offset: 0x000AD338
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

	// Token: 0x06001248 RID: 4680 RVA: 0x0003B8C8 File Offset: 0x00039AC8
	public void SliderPressed(float f)
	{
		Debug.Log("Slider: " + f.ToString());
		this.sliderText.text = f.ToString();
	}

	// Token: 0x06001249 RID: 4681 RVA: 0x000AF188 File Offset: 0x000AD388
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

	// Token: 0x0600124A RID: 4682 RVA: 0x0003B8F2 File Offset: 0x00039AF2
	private void LogButtonPressed()
	{
		Debug.Log("Button pressed");
	}

	// Token: 0x0400143E RID: 5182
	private bool inMenu;

	// Token: 0x0400143F RID: 5183
	private Text sliderText;
}
