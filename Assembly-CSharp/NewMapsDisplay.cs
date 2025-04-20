using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using GorillaNetworking;
using ModIO;
using TMPro;
using UnityEngine;

// Token: 0x02000692 RID: 1682
public class NewMapsDisplay : MonoBehaviour
{
	// Token: 0x060029A7 RID: 10663 RVA: 0x0011778C File Offset: 0x0011598C
	public void OnEnable()
	{
		if (ModIOManager.GetNewMapsModId() == ModId.Null)
		{
			return;
		}
		this.mapImage.gameObject.SetActive(false);
		this.modNameText.text = "";
		this.modNameText.gameObject.SetActive(false);
		this.modCreatorLabelText.gameObject.SetActive(false);
		this.modCreatorText.text = "";
		this.modCreatorText.gameObject.SetActive(false);
		this.loadingText.gameObject.SetActive(true);
		if (GorillaServer.Instance == null || !GorillaServer.Instance.FeatureFlagsReady)
		{
			this.initCoroutine = base.StartCoroutine(this.DelayedInitialize());
			return;
		}
		this.Initialize();
	}

	// Token: 0x060029A8 RID: 10664 RVA: 0x00117858 File Offset: 0x00115A58
	public void OnDisable()
	{
		if (this.initCoroutine != null)
		{
			base.StopCoroutine(this.initCoroutine);
			this.initCoroutine = null;
		}
		this.newMapsModProfile = default(ModProfile);
		this.newMapDatas.Clear();
		this.slideshowActive = false;
		this.slideshowIndex = 0;
		this.lastSlideshowUpdate = 0f;
		this.mapImage.gameObject.SetActive(false);
		this.modNameText.text = "";
		this.modNameText.gameObject.SetActive(false);
		this.modCreatorLabelText.gameObject.SetActive(false);
		this.modCreatorText.text = "";
		this.modCreatorText.gameObject.SetActive(false);
		this.loadingText.gameObject.SetActive(true);
	}

	// Token: 0x060029A9 RID: 10665 RVA: 0x0004C1EA File Offset: 0x0004A3EA
	private IEnumerator DelayedInitialize()
	{
		bool flag = GorillaServer.Instance != null && GorillaServer.Instance.FeatureFlagsReady;
		while (!flag)
		{
			yield return new WaitForSecondsRealtime(3f);
			flag = (GorillaServer.Instance != null && GorillaServer.Instance.FeatureFlagsReady);
		}
		this.Initialize();
		this.initCoroutine = null;
		yield break;
	}

	// Token: 0x060029AA RID: 10666 RVA: 0x0004C1F9 File Offset: 0x0004A3F9
	private void Initialize()
	{
		if (!this.requestingNewMapsModProfile && !this.downloadingImages)
		{
			ModIOManager.Initialize(delegate(ModIORequestResult result)
			{
				if (result.success)
				{
					if (!base.isActiveAndEnabled)
					{
						return;
					}
					this.requestingNewMapsModProfile = true;
					ModIOManager.GetModProfile(ModIOManager.GetNewMapsModId(), new Action<ModIORequestResultAnd<ModProfile>>(this.OnGetNewMapsModProfile));
				}
			});
		}
	}

	// Token: 0x060029AB RID: 10667 RVA: 0x00117928 File Offset: 0x00115B28
	private void OnGetNewMapsModProfile(ModIORequestResultAnd<ModProfile> resultAndProfile)
	{
		NewMapsDisplay.<OnGetNewMapsModProfile>d__19 <OnGetNewMapsModProfile>d__;
		<OnGetNewMapsModProfile>d__.<>t__builder = AsyncVoidMethodBuilder.Create();
		<OnGetNewMapsModProfile>d__.<>4__this = this;
		<OnGetNewMapsModProfile>d__.resultAndProfile = resultAndProfile;
		<OnGetNewMapsModProfile>d__.<>1__state = -1;
		<OnGetNewMapsModProfile>d__.<>t__builder.Start<NewMapsDisplay.<OnGetNewMapsModProfile>d__19>(ref <OnGetNewMapsModProfile>d__);
	}

	// Token: 0x060029AC RID: 10668 RVA: 0x0004C21C File Offset: 0x0004A41C
	private void StartSlideshow()
	{
		if (this.newMapDatas.IsNullOrEmpty<NewMapsDisplay.NewMapData>())
		{
			return;
		}
		this.slideshowIndex = 0;
		this.slideshowActive = true;
		this.UpdateSlideshow();
	}

	// Token: 0x060029AD RID: 10669 RVA: 0x0004C240 File Offset: 0x0004A440
	public void Update()
	{
		if (!this.slideshowActive || Time.time - this.lastSlideshowUpdate < this.slideshowUpdateInterval)
		{
			return;
		}
		this.UpdateSlideshow();
	}

	// Token: 0x060029AE RID: 10670 RVA: 0x00117968 File Offset: 0x00115B68
	private void UpdateSlideshow()
	{
		this.loadingText.gameObject.SetActive(false);
		this.lastSlideshowUpdate = Time.time;
		Texture2D image = this.newMapDatas[this.slideshowIndex].image;
		if (image != null)
		{
			this.mapImage.sprite = Sprite.Create(image, new Rect(0f, 0f, (float)image.width, (float)image.height), new Vector2(0.5f, 0.5f));
			this.mapImage.gameObject.SetActive(true);
		}
		else
		{
			this.mapImage.gameObject.SetActive(false);
		}
		this.modNameText.text = this.newMapDatas[this.slideshowIndex].name;
		this.modCreatorText.text = this.newMapDatas[this.slideshowIndex].creator;
		this.modNameText.gameObject.SetActive(true);
		this.modCreatorLabelText.gameObject.SetActive(true);
		this.modCreatorText.gameObject.SetActive(true);
		this.slideshowIndex++;
		if (this.slideshowIndex >= this.newMapDatas.Count)
		{
			this.slideshowIndex = 0;
		}
	}

	// Token: 0x04002EE8 RID: 12008
	[SerializeField]
	private SpriteRenderer mapImage;

	// Token: 0x04002EE9 RID: 12009
	[SerializeField]
	private TMP_Text loadingText;

	// Token: 0x04002EEA RID: 12010
	[SerializeField]
	private TMP_Text modNameText;

	// Token: 0x04002EEB RID: 12011
	[SerializeField]
	private TMP_Text modCreatorLabelText;

	// Token: 0x04002EEC RID: 12012
	[SerializeField]
	private TMP_Text modCreatorText;

	// Token: 0x04002EED RID: 12013
	[SerializeField]
	private float slideshowUpdateInterval = 1f;

	// Token: 0x04002EEE RID: 12014
	private ModProfile newMapsModProfile;

	// Token: 0x04002EEF RID: 12015
	private List<NewMapsDisplay.NewMapData> newMapDatas = new List<NewMapsDisplay.NewMapData>();

	// Token: 0x04002EF0 RID: 12016
	private bool slideshowActive;

	// Token: 0x04002EF1 RID: 12017
	private int slideshowIndex;

	// Token: 0x04002EF2 RID: 12018
	private float lastSlideshowUpdate;

	// Token: 0x04002EF3 RID: 12019
	private bool requestingNewMapsModProfile;

	// Token: 0x04002EF4 RID: 12020
	private bool downloadingImages;

	// Token: 0x04002EF5 RID: 12021
	private Coroutine initCoroutine;

	// Token: 0x02000693 RID: 1683
	private struct NewMapData
	{
		// Token: 0x04002EF6 RID: 12022
		public Texture2D image;

		// Token: 0x04002EF7 RID: 12023
		public string name;

		// Token: 0x04002EF8 RID: 12024
		public string creator;
	}
}
