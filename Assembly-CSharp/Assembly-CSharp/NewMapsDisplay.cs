using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using GorillaNetworking;
using ModIO;
using TMPro;
using UnityEngine;

// Token: 0x02000618 RID: 1560
public class NewMapsDisplay : MonoBehaviour
{
	// Token: 0x060026E8 RID: 9960 RVA: 0x000BF90C File Offset: 0x000BDB0C
	public void OnEnable()
	{
		if (ModIODataStore.GetNewMapsModId() == ModId.Null)
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

	// Token: 0x060026E9 RID: 9961 RVA: 0x000BF9D8 File Offset: 0x000BDBD8
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

	// Token: 0x060026EA RID: 9962 RVA: 0x000BFAA5 File Offset: 0x000BDCA5
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

	// Token: 0x060026EB RID: 9963 RVA: 0x000BFAB4 File Offset: 0x000BDCB4
	private void Initialize()
	{
		if (!this.requestingNewMapsModProfile && !this.downloadingImages)
		{
			ModIODataStore.Initialize(delegate(ModIORequestResult result)
			{
				if (result.success)
				{
					if (!base.isActiveAndEnabled)
					{
						return;
					}
					this.requestingNewMapsModProfile = true;
					ModIODataStore.GetModProfile(ModIODataStore.GetNewMapsModId(), new Action<ModIORequestResultAnd<ModProfile>>(this.OnGetNewMapsModProfile));
				}
			});
		}
	}

	// Token: 0x060026EC RID: 9964 RVA: 0x000BFAD8 File Offset: 0x000BDCD8
	private void OnGetNewMapsModProfile(ModIORequestResultAnd<ModProfile> resultAndProfile)
	{
		NewMapsDisplay.<OnGetNewMapsModProfile>d__19 <OnGetNewMapsModProfile>d__;
		<OnGetNewMapsModProfile>d__.<>t__builder = AsyncVoidMethodBuilder.Create();
		<OnGetNewMapsModProfile>d__.<>4__this = this;
		<OnGetNewMapsModProfile>d__.resultAndProfile = resultAndProfile;
		<OnGetNewMapsModProfile>d__.<>1__state = -1;
		<OnGetNewMapsModProfile>d__.<>t__builder.Start<NewMapsDisplay.<OnGetNewMapsModProfile>d__19>(ref <OnGetNewMapsModProfile>d__);
	}

	// Token: 0x060026ED RID: 9965 RVA: 0x000BFB17 File Offset: 0x000BDD17
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

	// Token: 0x060026EE RID: 9966 RVA: 0x000BFB3B File Offset: 0x000BDD3B
	public void Update()
	{
		if (!this.slideshowActive || Time.time - this.lastSlideshowUpdate < this.slideshowUpdateInterval)
		{
			return;
		}
		this.UpdateSlideshow();
	}

	// Token: 0x060026EF RID: 9967 RVA: 0x000BFB60 File Offset: 0x000BDD60
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

	// Token: 0x04002ABA RID: 10938
	[SerializeField]
	private SpriteRenderer mapImage;

	// Token: 0x04002ABB RID: 10939
	[SerializeField]
	private TMP_Text loadingText;

	// Token: 0x04002ABC RID: 10940
	[SerializeField]
	private TMP_Text modNameText;

	// Token: 0x04002ABD RID: 10941
	[SerializeField]
	private TMP_Text modCreatorLabelText;

	// Token: 0x04002ABE RID: 10942
	[SerializeField]
	private TMP_Text modCreatorText;

	// Token: 0x04002ABF RID: 10943
	[SerializeField]
	private float slideshowUpdateInterval = 1f;

	// Token: 0x04002AC0 RID: 10944
	private ModProfile newMapsModProfile;

	// Token: 0x04002AC1 RID: 10945
	private List<NewMapsDisplay.NewMapData> newMapDatas = new List<NewMapsDisplay.NewMapData>();

	// Token: 0x04002AC2 RID: 10946
	private bool slideshowActive;

	// Token: 0x04002AC3 RID: 10947
	private int slideshowIndex;

	// Token: 0x04002AC4 RID: 10948
	private float lastSlideshowUpdate;

	// Token: 0x04002AC5 RID: 10949
	private bool requestingNewMapsModProfile;

	// Token: 0x04002AC6 RID: 10950
	private bool downloadingImages;

	// Token: 0x04002AC7 RID: 10951
	private Coroutine initCoroutine;

	// Token: 0x02000619 RID: 1561
	private struct NewMapData
	{
		// Token: 0x04002AC8 RID: 10952
		public Texture2D image;

		// Token: 0x04002AC9 RID: 10953
		public string name;

		// Token: 0x04002ACA RID: 10954
		public string creator;
	}
}
