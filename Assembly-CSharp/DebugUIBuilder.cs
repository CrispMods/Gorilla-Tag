using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020002BE RID: 702
public class DebugUIBuilder : MonoBehaviour
{
	// Token: 0x06001106 RID: 4358 RVA: 0x000AC704 File Offset: 0x000AA904
	public void Awake()
	{
		DebugUIBuilder.instance = this;
		this.menuOffset = base.transform.position;
		base.gameObject.SetActive(false);
		this.rig = UnityEngine.Object.FindObjectOfType<OVRCameraRig>();
		for (int i = 0; i < this.toEnable.Count; i++)
		{
			this.toEnable[i].SetActive(false);
		}
		this.insertPositions = new Vector2[this.targetContentPanels.Length];
		for (int j = 0; j < this.insertPositions.Length; j++)
		{
			this.insertPositions[j].x = this.marginH;
			this.insertPositions[j].y = -this.marginV;
		}
		this.insertedElements = new List<RectTransform>[this.targetContentPanels.Length];
		for (int k = 0; k < this.insertedElements.Length; k++)
		{
			this.insertedElements[k] = new List<RectTransform>();
		}
		if (this.uiHelpersToInstantiate)
		{
			UnityEngine.Object.Instantiate<GameObject>(this.uiHelpersToInstantiate);
		}
		this.lp = UnityEngine.Object.FindObjectOfType<LaserPointer>();
		if (!this.lp)
		{
			Debug.LogError("Debug UI requires use of a LaserPointer and will not function without it. Add one to your scene, or assign the UIHelpers prefab to the DebugUIBuilder in the inspector.");
			return;
		}
		this.lp.laserBeamBehavior = this.laserBeamBehavior;
		if (!this.toEnable.Contains(this.lp.gameObject))
		{
			this.toEnable.Add(this.lp.gameObject);
		}
		base.GetComponent<OVRRaycaster>().pointer = this.lp.gameObject;
		this.lp.gameObject.SetActive(false);
	}

	// Token: 0x06001107 RID: 4359 RVA: 0x000AC894 File Offset: 0x000AAA94
	public void Show()
	{
		this.Relayout();
		base.gameObject.SetActive(true);
		base.transform.position = this.rig.transform.TransformPoint(this.menuOffset);
		Vector3 eulerAngles = this.rig.transform.rotation.eulerAngles;
		eulerAngles.x = 0f;
		eulerAngles.z = 0f;
		base.transform.eulerAngles = eulerAngles;
		if (this.reEnable == null || this.reEnable.Length < this.toDisable.Count)
		{
			this.reEnable = new bool[this.toDisable.Count];
		}
		this.reEnable.Initialize();
		int count = this.toDisable.Count;
		for (int i = 0; i < count; i++)
		{
			if (this.toDisable[i])
			{
				this.reEnable[i] = this.toDisable[i].activeSelf;
				this.toDisable[i].SetActive(false);
			}
		}
		count = this.toEnable.Count;
		for (int j = 0; j < count; j++)
		{
			this.toEnable[j].SetActive(true);
		}
		int num = this.targetContentPanels.Length;
		for (int k = 0; k < num; k++)
		{
			this.targetContentPanels[k].gameObject.SetActive(this.insertedElements[k].Count > 0);
		}
	}

	// Token: 0x06001108 RID: 4360 RVA: 0x000ACA1C File Offset: 0x000AAC1C
	public void Hide()
	{
		base.gameObject.SetActive(false);
		for (int i = 0; i < this.reEnable.Length; i++)
		{
			if (this.toDisable[i] && this.reEnable[i])
			{
				this.toDisable[i].SetActive(true);
			}
		}
		int count = this.toEnable.Count;
		for (int j = 0; j < count; j++)
		{
			this.toEnable[j].SetActive(false);
		}
	}

	// Token: 0x06001109 RID: 4361 RVA: 0x000ACAA4 File Offset: 0x000AACA4
	private void StackedRelayout()
	{
		for (int i = 0; i < this.targetContentPanels.Length; i++)
		{
			RectTransform component = this.targetContentPanels[i].GetComponent<RectTransform>();
			List<RectTransform> list = this.insertedElements[i];
			int count = list.Count;
			float num = this.marginH;
			float num2 = -this.marginV;
			float num3 = 0f;
			for (int j = 0; j < count; j++)
			{
				RectTransform rectTransform = list[j];
				rectTransform.anchoredPosition = new Vector2(num, num2);
				if (this.isHorizontal)
				{
					num += rectTransform.rect.width + this.elementSpacing;
				}
				else
				{
					num2 -= rectTransform.rect.height + this.elementSpacing;
				}
				num3 = Mathf.Max(rectTransform.rect.width + 2f * this.marginH, num3);
			}
			component.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, num3);
			component.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, -num2 + this.marginV);
		}
	}

	// Token: 0x0600110A RID: 4362 RVA: 0x000ACBB4 File Offset: 0x000AADB4
	private void PanelCentricRelayout()
	{
		if (!this.isHorizontal)
		{
			Debug.Log("Error:Panel Centeric relayout is implemented only for horizontal panels");
			return;
		}
		for (int i = 0; i < this.targetContentPanels.Length; i++)
		{
			RectTransform component = this.targetContentPanels[i].GetComponent<RectTransform>();
			List<RectTransform> list = this.insertedElements[i];
			int count = list.Count;
			float num = this.marginH;
			float num2 = -this.marginV;
			float num3 = num;
			for (int j = 0; j < count; j++)
			{
				RectTransform rectTransform = list[j];
				num3 += rectTransform.rect.width + this.elementSpacing;
			}
			num3 -= this.elementSpacing;
			num3 += this.marginH;
			float num4 = num3;
			num = -0.5f * num4;
			num2 = -this.marginV;
			for (int k = 0; k < count; k++)
			{
				RectTransform rectTransform2 = list[k];
				if (k == 0)
				{
					num += this.marginH;
				}
				num += 0.5f * rectTransform2.rect.width;
				rectTransform2.anchoredPosition = new Vector2(num, num2);
				num += rectTransform2.rect.width * 0.5f + this.elementSpacing;
				num3 = Mathf.Max(rectTransform2.rect.width + 2f * this.marginH, num3);
			}
			component.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, num3);
			component.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, -num2 + this.marginV);
		}
	}

	// Token: 0x0600110B RID: 4363 RVA: 0x0003BA5B File Offset: 0x00039C5B
	private void Relayout()
	{
		if (this.usePanelCentricRelayout)
		{
			this.PanelCentricRelayout();
			return;
		}
		this.StackedRelayout();
	}

	// Token: 0x0600110C RID: 4364 RVA: 0x000ACD40 File Offset: 0x000AAF40
	private void AddRect(RectTransform r, int targetCanvas)
	{
		if (targetCanvas > this.targetContentPanels.Length)
		{
			Debug.LogError(string.Concat(new string[]
			{
				"Attempted to add debug panel to canvas ",
				targetCanvas.ToString(),
				", but only ",
				this.targetContentPanels.Length.ToString(),
				" panels were provided. Fix in the inspector or pass a lower value for target canvas."
			}));
			return;
		}
		r.transform.SetParent(this.targetContentPanels[targetCanvas], false);
		this.insertedElements[targetCanvas].Add(r);
		if (base.gameObject.activeInHierarchy)
		{
			this.Relayout();
		}
	}

	// Token: 0x0600110D RID: 4365 RVA: 0x000ACDD4 File Offset: 0x000AAFD4
	public RectTransform AddButton(string label, DebugUIBuilder.OnClick handler = null, int buttonIndex = -1, int targetCanvas = 0, bool highResolutionText = false)
	{
		RectTransform component;
		if (buttonIndex == -1)
		{
			component = UnityEngine.Object.Instantiate<RectTransform>(this.buttonPrefab).GetComponent<RectTransform>();
		}
		else
		{
			component = UnityEngine.Object.Instantiate<RectTransform>(this.additionalButtonPrefab[buttonIndex]).GetComponent<RectTransform>();
		}
		Button componentInChildren = component.GetComponentInChildren<Button>();
		if (handler != null)
		{
			componentInChildren.onClick.AddListener(delegate()
			{
				handler();
			});
		}
		if (highResolutionText)
		{
			((TextMeshProUGUI)component.GetComponentsInChildren(typeof(TextMeshProUGUI), true)[0]).text = label;
		}
		else
		{
			((Text)component.GetComponentsInChildren(typeof(Text), true)[0]).text = label;
		}
		this.AddRect(component, targetCanvas);
		return component;
	}

	// Token: 0x0600110E RID: 4366 RVA: 0x000ACE8C File Offset: 0x000AB08C
	public RectTransform AddLabel(string label, int targetCanvas = 0)
	{
		RectTransform component = UnityEngine.Object.Instantiate<RectTransform>(this.labelPrefab).GetComponent<RectTransform>();
		component.GetComponent<Text>().text = label;
		this.AddRect(component, targetCanvas);
		return component;
	}

	// Token: 0x0600110F RID: 4367 RVA: 0x000ACEC0 File Offset: 0x000AB0C0
	public RectTransform AddSlider(string label, float min, float max, DebugUIBuilder.OnSlider onValueChanged, bool wholeNumbersOnly = false, int targetCanvas = 0)
	{
		RectTransform rectTransform = UnityEngine.Object.Instantiate<RectTransform>(this.sliderPrefab);
		Slider componentInChildren = rectTransform.GetComponentInChildren<Slider>();
		componentInChildren.minValue = min;
		componentInChildren.maxValue = max;
		componentInChildren.onValueChanged.AddListener(delegate(float f)
		{
			onValueChanged(f);
		});
		componentInChildren.wholeNumbers = wholeNumbersOnly;
		this.AddRect(rectTransform, targetCanvas);
		return rectTransform;
	}

	// Token: 0x06001110 RID: 4368 RVA: 0x000ACF24 File Offset: 0x000AB124
	public RectTransform AddDivider(int targetCanvas = 0)
	{
		RectTransform rectTransform = UnityEngine.Object.Instantiate<RectTransform>(this.dividerPrefab);
		this.AddRect(rectTransform, targetCanvas);
		return rectTransform;
	}

	// Token: 0x06001111 RID: 4369 RVA: 0x000ACF48 File Offset: 0x000AB148
	public RectTransform AddToggle(string label, DebugUIBuilder.OnToggleValueChange onValueChanged, int targetCanvas = 0)
	{
		RectTransform rectTransform = UnityEngine.Object.Instantiate<RectTransform>(this.togglePrefab);
		this.AddRect(rectTransform, targetCanvas);
		rectTransform.GetComponentInChildren<Text>().text = label;
		Toggle t = rectTransform.GetComponentInChildren<Toggle>();
		t.onValueChanged.AddListener(delegate(bool <p0>)
		{
			onValueChanged(t);
		});
		return rectTransform;
	}

	// Token: 0x06001112 RID: 4370 RVA: 0x000ACFAC File Offset: 0x000AB1AC
	public RectTransform AddToggle(string label, DebugUIBuilder.OnToggleValueChange onValueChanged, bool defaultValue, int targetCanvas = 0)
	{
		RectTransform rectTransform = UnityEngine.Object.Instantiate<RectTransform>(this.togglePrefab);
		this.AddRect(rectTransform, targetCanvas);
		rectTransform.GetComponentInChildren<Text>().text = label;
		Toggle t = rectTransform.GetComponentInChildren<Toggle>();
		t.isOn = defaultValue;
		t.onValueChanged.AddListener(delegate(bool <p0>)
		{
			onValueChanged(t);
		});
		return rectTransform;
	}

	// Token: 0x06001113 RID: 4371 RVA: 0x000AD01C File Offset: 0x000AB21C
	public RectTransform AddRadio(string label, string group, DebugUIBuilder.OnToggleValueChange handler, int targetCanvas = 0)
	{
		RectTransform rectTransform = UnityEngine.Object.Instantiate<RectTransform>(this.radioPrefab);
		this.AddRect(rectTransform, targetCanvas);
		rectTransform.GetComponentInChildren<Text>().text = label;
		Toggle tb = rectTransform.GetComponentInChildren<Toggle>();
		if (group == null)
		{
			group = "default";
		}
		bool isOn = false;
		ToggleGroup toggleGroup;
		if (!this.radioGroups.ContainsKey(group))
		{
			toggleGroup = tb.gameObject.AddComponent<ToggleGroup>();
			this.radioGroups[group] = toggleGroup;
			isOn = true;
		}
		else
		{
			toggleGroup = this.radioGroups[group];
		}
		tb.group = toggleGroup;
		tb.isOn = isOn;
		tb.onValueChanged.AddListener(delegate(bool <p0>)
		{
			handler(tb);
		});
		return rectTransform;
	}

	// Token: 0x06001114 RID: 4372 RVA: 0x000AD0E4 File Offset: 0x000AB2E4
	public RectTransform AddTextField(string label, int targetCanvas = 0)
	{
		RectTransform component = UnityEngine.Object.Instantiate<RectTransform>(this.textPrefab).GetComponent<RectTransform>();
		component.GetComponentInChildren<InputField>().text = label;
		this.AddRect(component, targetCanvas);
		return component;
	}

	// Token: 0x06001115 RID: 4373 RVA: 0x0003BA72 File Offset: 0x00039C72
	public void ToggleLaserPointer(bool isOn)
	{
		if (this.lp)
		{
			if (isOn)
			{
				this.lp.enabled = true;
				return;
			}
			this.lp.enabled = false;
		}
	}

	// Token: 0x04001308 RID: 4872
	public const int DEBUG_PANE_CENTER = 0;

	// Token: 0x04001309 RID: 4873
	public const int DEBUG_PANE_RIGHT = 1;

	// Token: 0x0400130A RID: 4874
	public const int DEBUG_PANE_LEFT = 2;

	// Token: 0x0400130B RID: 4875
	[SerializeField]
	private RectTransform buttonPrefab;

	// Token: 0x0400130C RID: 4876
	[SerializeField]
	private RectTransform[] additionalButtonPrefab;

	// Token: 0x0400130D RID: 4877
	[SerializeField]
	private RectTransform labelPrefab;

	// Token: 0x0400130E RID: 4878
	[SerializeField]
	private RectTransform sliderPrefab;

	// Token: 0x0400130F RID: 4879
	[SerializeField]
	private RectTransform dividerPrefab;

	// Token: 0x04001310 RID: 4880
	[SerializeField]
	private RectTransform togglePrefab;

	// Token: 0x04001311 RID: 4881
	[SerializeField]
	private RectTransform radioPrefab;

	// Token: 0x04001312 RID: 4882
	[SerializeField]
	private RectTransform textPrefab;

	// Token: 0x04001313 RID: 4883
	[SerializeField]
	private GameObject uiHelpersToInstantiate;

	// Token: 0x04001314 RID: 4884
	[SerializeField]
	private Transform[] targetContentPanels;

	// Token: 0x04001315 RID: 4885
	private bool[] reEnable;

	// Token: 0x04001316 RID: 4886
	[SerializeField]
	private List<GameObject> toEnable;

	// Token: 0x04001317 RID: 4887
	[SerializeField]
	private List<GameObject> toDisable;

	// Token: 0x04001318 RID: 4888
	public static DebugUIBuilder instance;

	// Token: 0x04001319 RID: 4889
	public float elementSpacing = 16f;

	// Token: 0x0400131A RID: 4890
	public float marginH = 16f;

	// Token: 0x0400131B RID: 4891
	public float marginV = 16f;

	// Token: 0x0400131C RID: 4892
	private Vector2[] insertPositions;

	// Token: 0x0400131D RID: 4893
	private List<RectTransform>[] insertedElements;

	// Token: 0x0400131E RID: 4894
	private Vector3 menuOffset;

	// Token: 0x0400131F RID: 4895
	private OVRCameraRig rig;

	// Token: 0x04001320 RID: 4896
	private Dictionary<string, ToggleGroup> radioGroups = new Dictionary<string, ToggleGroup>();

	// Token: 0x04001321 RID: 4897
	private LaserPointer lp;

	// Token: 0x04001322 RID: 4898
	private LineRenderer lr;

	// Token: 0x04001323 RID: 4899
	public LaserPointer.LaserBeamBehavior laserBeamBehavior;

	// Token: 0x04001324 RID: 4900
	public bool isHorizontal;

	// Token: 0x04001325 RID: 4901
	public bool usePanelCentricRelayout;

	// Token: 0x020002BF RID: 703
	// (Invoke) Token: 0x06001118 RID: 4376
	public delegate void OnClick();

	// Token: 0x020002C0 RID: 704
	// (Invoke) Token: 0x0600111C RID: 4380
	public delegate void OnToggleValueChange(Toggle t);

	// Token: 0x020002C1 RID: 705
	// (Invoke) Token: 0x06001120 RID: 4384
	public delegate void OnSlider(float f);

	// Token: 0x020002C2 RID: 706
	// (Invoke) Token: 0x06001124 RID: 4388
	public delegate bool ActiveUpdate();
}
