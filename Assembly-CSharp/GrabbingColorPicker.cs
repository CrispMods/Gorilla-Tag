using System;
using GorillaLocomotion;
using GorillaNetworking;
using Photon.Pun;
using TMPro;
using UnityEngine;

// Token: 0x02000439 RID: 1081
public class GrabbingColorPicker : MonoBehaviour, IGorillaSliceableSimple
{
	// Token: 0x06001ABE RID: 6846 RVA: 0x000D78DC File Offset: 0x000D5ADC
	private void Start()
	{
		float @float = PlayerPrefs.GetFloat("redValue", 0f);
		float float2 = PlayerPrefs.GetFloat("greenValue", 0f);
		float float3 = PlayerPrefs.GetFloat("blueValue", 0f);
		this.Segment1 = Mathf.RoundToInt(Mathf.Lerp(0f, 9f, @float));
		this.Segment2 = Mathf.RoundToInt(Mathf.Lerp(0f, 9f, float2));
		this.Segment3 = Mathf.RoundToInt(Mathf.Lerp(0f, 9f, float3));
		this.R_PushSlider.SetProgress(@float);
		this.G_PushSlider.SetProgress(float2);
		this.B_PushSlider.SetProgress(float3);
		this.UpdateDisplay();
		if (GorillaTagger.Instance && GorillaTagger.Instance.offlineVRRig)
		{
			GorillaTagger.Instance.offlineVRRig.OnColorChanged += this.HandleLocalColorChanged;
		}
	}

	// Token: 0x06001ABF RID: 6847 RVA: 0x000421DD File Offset: 0x000403DD
	private void OnDestroy()
	{
		if (GorillaTagger.Instance && GorillaTagger.Instance.offlineVRRig)
		{
			GorillaTagger.Instance.offlineVRRig.OnColorChanged -= this.HandleLocalColorChanged;
		}
	}

	// Token: 0x06001AC0 RID: 6848 RVA: 0x00032C89 File Offset: 0x00030E89
	public void OnEnable()
	{
		GorillaSlicerSimpleManager.RegisterSliceable(this, GorillaSlicerSimpleManager.UpdateStep.Update);
	}

	// Token: 0x06001AC1 RID: 6849 RVA: 0x00032C92 File Offset: 0x00030E92
	public void OnDisable()
	{
		GorillaSlicerSimpleManager.UnregisterSliceable(this, GorillaSlicerSimpleManager.UpdateStep.Update);
	}

	// Token: 0x06001AC2 RID: 6850 RVA: 0x000D79CC File Offset: 0x000D5BCC
	public void SliceUpdate()
	{
		float num = Vector3.Distance(base.transform.position, GTPlayer.Instance.transform.position);
		this.hasUpdated = false;
		if (num < 5f)
		{
			int segment = this.Segment1;
			int segment2 = this.Segment2;
			int segment3 = this.Segment3;
			this.Segment1 = Mathf.RoundToInt(Mathf.Lerp(0f, 9f, this.R_PushSlider.GetProgress()));
			this.Segment2 = Mathf.RoundToInt(Mathf.Lerp(0f, 9f, this.G_PushSlider.GetProgress()));
			this.Segment3 = Mathf.RoundToInt(Mathf.Lerp(0f, 9f, this.B_PushSlider.GetProgress()));
			if (segment != this.Segment1 || segment2 != this.Segment2 || segment3 != this.Segment3)
			{
				this.hasUpdated = true;
				PlayerPrefs.SetFloat("redValue", (float)this.Segment1 / 9f);
				PlayerPrefs.SetFloat("greenValue", (float)this.Segment2 / 9f);
				PlayerPrefs.SetFloat("blueValue", (float)this.Segment3 / 9f);
				GorillaTagger.Instance.UpdateColor((float)this.Segment1 / 9f, (float)this.Segment2 / 9f, (float)this.Segment3 / 9f);
				GorillaComputer.instance.UpdateColor((float)this.Segment1 / 9f, (float)this.Segment2 / 9f, (float)this.Segment3 / 9f);
				PlayerPrefs.Save();
				if (NetworkSystem.Instance.InRoom)
				{
					GorillaTagger.Instance.myVRRig.SendRPC("RPC_InitializeNoobMaterial", RpcTarget.All, new object[]
					{
						(float)this.Segment1 / 9f,
						(float)this.Segment2 / 9f,
						(float)this.Segment3 / 9f
					});
				}
				this.UpdateDisplay();
				if (segment != this.Segment1)
				{
					this.R_SliderAudio.transform.position = this.R_PushSlider.transform.position;
					this.R_SliderAudio.GTPlay();
				}
				if (segment2 != this.Segment2)
				{
					this.G_SliderAudio.transform.position = this.G_PushSlider.transform.position;
					this.G_SliderAudio.GTPlay();
				}
				if (segment3 != this.Segment3)
				{
					this.B_SliderAudio.transform.position = this.B_PushSlider.transform.position;
					this.B_SliderAudio.GTPlay();
				}
			}
		}
	}

	// Token: 0x06001AC3 RID: 6851 RVA: 0x000D7C68 File Offset: 0x000D5E68
	private void SetSliderColors(float r, float g, float b)
	{
		if (!this.hasUpdated)
		{
			this.Segment1 = Mathf.RoundToInt(Mathf.Lerp(0f, 9f, r));
			this.Segment2 = Mathf.RoundToInt(Mathf.Lerp(0f, 9f, g));
			this.Segment3 = Mathf.RoundToInt(Mathf.Lerp(0f, 9f, b));
			this.R_PushSlider.SetProgress(r);
			this.G_PushSlider.SetProgress(g);
			this.B_PushSlider.SetProgress(b);
			this.UpdateDisplay();
		}
	}

	// Token: 0x06001AC4 RID: 6852 RVA: 0x00042217 File Offset: 0x00040417
	private void HandleLocalColorChanged(Color newColor)
	{
		this.SetSliderColors(newColor.r, newColor.g, newColor.b);
	}

	// Token: 0x06001AC5 RID: 6853 RVA: 0x000D7CF8 File Offset: 0x000D5EF8
	private void UpdateDisplay()
	{
		this.textR.text = this.Segment1.ToString();
		this.textG.text = this.Segment2.ToString();
		this.textB.text = this.Segment3.ToString();
		Color color = new Color((float)this.Segment1 / 9f, (float)this.Segment2 / 9f, (float)this.Segment3 / 9f);
		Renderer[] componentsInChildren = this.ColorSwatch.GetComponentsInChildren<Renderer>();
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			Material[] materials = componentsInChildren[i].materials;
			for (int j = 0; j < materials.Length; j++)
			{
				materials[j].color = color;
			}
		}
	}

	// Token: 0x06001AC7 RID: 6855 RVA: 0x00032105 File Offset: 0x00030305
	bool IGorillaSliceableSimple.get_isActiveAndEnabled()
	{
		return base.isActiveAndEnabled;
	}

	// Token: 0x04001D81 RID: 7553
	[SerializeField]
	private PushableSlider R_PushSlider;

	// Token: 0x04001D82 RID: 7554
	[SerializeField]
	private PushableSlider G_PushSlider;

	// Token: 0x04001D83 RID: 7555
	[SerializeField]
	private PushableSlider B_PushSlider;

	// Token: 0x04001D84 RID: 7556
	[SerializeField]
	private AudioSource R_SliderAudio;

	// Token: 0x04001D85 RID: 7557
	[SerializeField]
	private AudioSource G_SliderAudio;

	// Token: 0x04001D86 RID: 7558
	[SerializeField]
	private AudioSource B_SliderAudio;

	// Token: 0x04001D87 RID: 7559
	[SerializeField]
	private TextMeshPro textR;

	// Token: 0x04001D88 RID: 7560
	[SerializeField]
	private TextMeshPro textG;

	// Token: 0x04001D89 RID: 7561
	[SerializeField]
	private TextMeshPro textB;

	// Token: 0x04001D8A RID: 7562
	[SerializeField]
	private GameObject ColorSwatch;

	// Token: 0x04001D8B RID: 7563
	private int Segment1;

	// Token: 0x04001D8C RID: 7564
	private int Segment2;

	// Token: 0x04001D8D RID: 7565
	private int Segment3;

	// Token: 0x04001D8E RID: 7566
	private bool hasUpdated;
}
