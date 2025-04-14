﻿using System;
using GorillaLocomotion;
using GorillaNetworking;
using Photon.Pun;
using TMPro;
using UnityEngine;

// Token: 0x0200042D RID: 1069
public class GrabbingColorPicker : MonoBehaviour, IGorillaSliceableSimple
{
	// Token: 0x06001A6A RID: 6762 RVA: 0x000825DC File Offset: 0x000807DC
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

	// Token: 0x06001A6B RID: 6763 RVA: 0x000826CC File Offset: 0x000808CC
	private void OnDestroy()
	{
		if (GorillaTagger.Instance && GorillaTagger.Instance.offlineVRRig)
		{
			GorillaTagger.Instance.offlineVRRig.OnColorChanged -= this.HandleLocalColorChanged;
		}
	}

	// Token: 0x06001A6C RID: 6764 RVA: 0x000158F9 File Offset: 0x00013AF9
	public void OnEnable()
	{
		GorillaSlicerSimpleManager.RegisterSliceable(this, GorillaSlicerSimpleManager.UpdateStep.Update);
	}

	// Token: 0x06001A6D RID: 6765 RVA: 0x00015902 File Offset: 0x00013B02
	public void OnDisable()
	{
		GorillaSlicerSimpleManager.UnregisterSliceable(this, GorillaSlicerSimpleManager.UpdateStep.Update);
	}

	// Token: 0x06001A6E RID: 6766 RVA: 0x00082708 File Offset: 0x00080908
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

	// Token: 0x06001A6F RID: 6767 RVA: 0x000829A4 File Offset: 0x00080BA4
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

	// Token: 0x06001A70 RID: 6768 RVA: 0x00082A34 File Offset: 0x00080C34
	private void HandleLocalColorChanged(Color newColor)
	{
		this.SetSliderColors(newColor.r, newColor.g, newColor.b);
	}

	// Token: 0x06001A71 RID: 6769 RVA: 0x00082A50 File Offset: 0x00080C50
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

	// Token: 0x06001A73 RID: 6771 RVA: 0x0000F974 File Offset: 0x0000DB74
	bool IGorillaSliceableSimple.get_isActiveAndEnabled()
	{
		return base.isActiveAndEnabled;
	}

	// Token: 0x04001D32 RID: 7474
	[SerializeField]
	private PushableSlider R_PushSlider;

	// Token: 0x04001D33 RID: 7475
	[SerializeField]
	private PushableSlider G_PushSlider;

	// Token: 0x04001D34 RID: 7476
	[SerializeField]
	private PushableSlider B_PushSlider;

	// Token: 0x04001D35 RID: 7477
	[SerializeField]
	private AudioSource R_SliderAudio;

	// Token: 0x04001D36 RID: 7478
	[SerializeField]
	private AudioSource G_SliderAudio;

	// Token: 0x04001D37 RID: 7479
	[SerializeField]
	private AudioSource B_SliderAudio;

	// Token: 0x04001D38 RID: 7480
	[SerializeField]
	private TextMeshPro textR;

	// Token: 0x04001D39 RID: 7481
	[SerializeField]
	private TextMeshPro textG;

	// Token: 0x04001D3A RID: 7482
	[SerializeField]
	private TextMeshPro textB;

	// Token: 0x04001D3B RID: 7483
	[SerializeField]
	private GameObject ColorSwatch;

	// Token: 0x04001D3C RID: 7484
	private int Segment1;

	// Token: 0x04001D3D RID: 7485
	private int Segment2;

	// Token: 0x04001D3E RID: 7486
	private int Segment3;

	// Token: 0x04001D3F RID: 7487
	private bool hasUpdated;
}
