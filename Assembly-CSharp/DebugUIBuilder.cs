using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020002B3 RID: 691
public class DebugUIBuilder : MonoBehaviour
{
	// Token: 0x060010BA RID: 4282 RVA: 0x00050FD4 File Offset: 0x0004F1D4
	public void Awake()
	{
		DebugUIBuilder.instance = this;
		this.menuOffset = base.transform.position;
		base.gameObject.SetActive(false);
		this.rig = Object.FindObjectOfType<OVRCameraRig>();
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
			Object.Instantiate<GameObject>(this.uiHelpersToInstantiate);
		}
		this.lp = Object.FindObjectOfType<LaserPointer>();
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

	// Token: 0x060010BB RID: 4283 RVA: 0x00051164 File Offset: 0x0004F364
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

	// Token: 0x060010BC RID: 4284 RVA: 0x000512EC File Offset: 0x0004F4EC
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

	// Token: 0x060010BD RID: 4285 RVA: 0x00051374 File Offset: 0x0004F574
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

	// Token: 0x060010BE RID: 4286 RVA: 0x00051484 File Offset: 0x0004F684
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

	// Token: 0x060010BF RID: 4287 RVA: 0x0005160F File Offset: 0x0004F80F
	private void Relayout()
	{
		if (this.usePanelCentricRelayout)
		{
			this.PanelCentricRelayout();
			return;
		}
		this.StackedRelayout();
	}

	// Token: 0x060010C0 RID: 4288 RVA: 0x00051628 File Offset: 0x0004F828
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

	// Token: 0x060010C1 RID: 4289 RVA: 0x000516BC File Offset: 0x0004F8BC
	public RectTransform AddButton(string label, DebugUIBuilder.OnClick handler = null, int buttonIndex = -1, int targetCanvas = 0, bool highResolutionText = false)
	{
		RectTransform component;
		if (buttonIndex == -1)
		{
			component = Object.Instantiate<RectTransform>(this.buttonPrefab).GetComponent<RectTransform>();
		}
		else
		{
			component = Object.Instantiate<RectTransform>(this.additionalButtonPrefab[buttonIndex]).GetComponent<RectTransform>();
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

	// Token: 0x060010C2 RID: 4290 RVA: 0x00051774 File Offset: 0x0004F974
	public RectTransform AddLabel(string label, int targetCanvas = 0)
	{
		RectTransform component = Object.Instantiate<RectTransform>(this.labelPrefab).GetComponent<RectTransform>();
		component.GetComponent<Text>().text = label;
		this.AddRect(component, targetCanvas);
		return component;
	}

	// Token: 0x060010C3 RID: 4291 RVA: 0x000517A8 File Offset: 0x0004F9A8
	public RectTransform AddSlider(string label, float min, float max, DebugUIBuilder.OnSlider onValueChanged, bool wholeNumbersOnly = false, int targetCanvas = 0)
	{
		RectTransform rectTransform = Object.Instantiate<RectTransform>(this.sliderPrefab);
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

	// Token: 0x060010C4 RID: 4292 RVA: 0x0005180C File Offset: 0x0004FA0C
	public RectTransform AddDivider(int targetCanvas = 0)
	{
		RectTransform rectTransform = Object.Instantiate<RectTransform>(this.dividerPrefab);
		this.AddRect(rectTransform, targetCanvas);
		return rectTransform;
	}

	// Token: 0x060010C5 RID: 4293 RVA: 0x00051830 File Offset: 0x0004FA30
	public RectTransform AddToggle(string label, DebugUIBuilder.OnToggleValueChange onValueChanged, int targetCanvas = 0)
	{
		RectTransform rectTransform = Object.Instantiate<RectTransform>(this.togglePrefab);
		this.AddRect(rectTransform, targetCanvas);
		rectTransform.GetComponentInChildren<Text>().text = label;
		Toggle t = rectTransform.GetComponentInChildren<Toggle>();
		t.onValueChanged.AddListener(delegate(bool <p0>)
		{
			onValueChanged(t);
		});
		return rectTransform;
	}

	// Token: 0x060010C6 RID: 4294 RVA: 0x00051894 File Offset: 0x0004FA94
	public RectTransform AddToggle(string label, DebugUIBuilder.OnToggleValueChange onValueChanged, bool defaultValue, int targetCanvas = 0)
	{
		RectTransform rectTransform = Object.Instantiate<RectTransform>(this.togglePrefab);
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

	// Token: 0x060010C7 RID: 4295 RVA: 0x00051904 File Offset: 0x0004FB04
	public RectTransform AddRadio(string label, string group, DebugUIBuilder.OnToggleValueChange handler, int targetCanvas = 0)
	{
		RectTransform rectTransform = Object.Instantiate<RectTransform>(this.radioPrefab);
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

	// Token: 0x060010C8 RID: 4296 RVA: 0x000519CC File Offset: 0x0004FBCC
	public RectTransform AddTextField(string label, int targetCanvas = 0)
	{
		RectTransform component = Object.Instantiate<RectTransform>(this.textPrefab).GetComponent<RectTransform>();
		component.GetComponentInChildren<InputField>().text = label;
		this.AddRect(component, targetCanvas);
		return component;
	}

	// Token: 0x060010C9 RID: 4297 RVA: 0x000519FF File Offset: 0x0004FBFF
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

	// Token: 0x040012C0 RID: 4800
	public const int DEBUG_PANE_CENTER = 0;

	// Token: 0x040012C1 RID: 4801
	public const int DEBUG_PANE_RIGHT = 1;

	// Token: 0x040012C2 RID: 4802
	public const int DEBUG_PANE_LEFT = 2;

	// Token: 0x040012C3 RID: 4803
	[SerializeField]
	private RectTransform buttonPrefab;

	// Token: 0x040012C4 RID: 4804
	[SerializeField]
	private RectTransform[] additionalButtonPrefab;

	// Token: 0x040012C5 RID: 4805
	[SerializeField]
	private RectTransform labelPrefab;

	// Token: 0x040012C6 RID: 4806
	[SerializeField]
	private RectTransform sliderPrefab;

	// Token: 0x040012C7 RID: 4807
	[SerializeField]
	private RectTransform dividerPrefab;

	// Token: 0x040012C8 RID: 4808
	[SerializeField]
	private RectTransform togglePrefab;

	// Token: 0x040012C9 RID: 4809
	[SerializeField]
	private RectTransform radioPrefab;

	// Token: 0x040012CA RID: 4810
	[SerializeField]
	private RectTransform textPrefab;

	// Token: 0x040012CB RID: 4811
	[SerializeField]
	private GameObject uiHelpersToInstantiate;

	// Token: 0x040012CC RID: 4812
	[SerializeField]
	private Transform[] targetContentPanels;

	// Token: 0x040012CD RID: 4813
	private bool[] reEnable;

	// Token: 0x040012CE RID: 4814
	[SerializeField]
	private List<GameObject> toEnable;

	// Token: 0x040012CF RID: 4815
	[SerializeField]
	private List<GameObject> toDisable;

	// Token: 0x040012D0 RID: 4816
	public static DebugUIBuilder instance;

	// Token: 0x040012D1 RID: 4817
	public float elementSpacing = 16f;

	// Token: 0x040012D2 RID: 4818
	public float marginH = 16f;

	// Token: 0x040012D3 RID: 4819
	public float marginV = 16f;

	// Token: 0x040012D4 RID: 4820
	private Vector2[] insertPositions;

	// Token: 0x040012D5 RID: 4821
	private List<RectTransform>[] insertedElements;

	// Token: 0x040012D6 RID: 4822
	private Vector3 menuOffset;

	// Token: 0x040012D7 RID: 4823
	private OVRCameraRig rig;

	// Token: 0x040012D8 RID: 4824
	private Dictionary<string, ToggleGroup> radioGroups = new Dictionary<string, ToggleGroup>();

	// Token: 0x040012D9 RID: 4825
	private LaserPointer lp;

	// Token: 0x040012DA RID: 4826
	private LineRenderer lr;

	// Token: 0x040012DB RID: 4827
	public LaserPointer.LaserBeamBehavior laserBeamBehavior;

	// Token: 0x040012DC RID: 4828
	public bool isHorizontal;

	// Token: 0x040012DD RID: 4829
	public bool usePanelCentricRelayout;

	// Token: 0x020002B4 RID: 692
	// (Invoke) Token: 0x060010CC RID: 4300
	public delegate void OnClick();

	// Token: 0x020002B5 RID: 693
	// (Invoke) Token: 0x060010D0 RID: 4304
	public delegate void OnToggleValueChange(Toggle t);

	// Token: 0x020002B6 RID: 694
	// (Invoke) Token: 0x060010D4 RID: 4308
	public delegate void OnSlider(float f);

	// Token: 0x020002B7 RID: 695
	// (Invoke) Token: 0x060010D8 RID: 4312
	public delegate bool ActiveUpdate();
}
