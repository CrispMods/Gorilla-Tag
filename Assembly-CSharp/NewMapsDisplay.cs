using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using GorillaNetworking;
using ModIO;
using TMPro;
using UnityEngine;

// Token: 0x02000617 RID: 1559
public class NewMapsDisplay : MonoBehaviour
{
	// Token: 0x060026E0 RID: 9952 RVA: 0x000BF48C File Offset: 0x000BD68C
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

	// Token: 0x060026E1 RID: 9953 RVA: 0x000BF558 File Offset: 0x000BD758
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

	// Token: 0x060026E2 RID: 9954 RVA: 0x000BF625 File Offset: 0x000BD825
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

	// Token: 0x060026E3 RID: 9955 RVA: 0x000BF634 File Offset: 0x000BD834
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

	// Token: 0x060026E4 RID: 9956 RVA: 0x000BF658 File Offset: 0x000BD858
	private void OnGetNewMapsModProfile(ModIORequestResultAnd<ModProfile> resultAndProfile)
	{
		NewMapsDisplay.<OnGetNewMapsModProfile>d__19 <OnGetNewMapsModProfile>d__;
		<OnGetNewMapsModProfile>d__.<>t__builder = AsyncVoidMethodBuilder.Create();
		<OnGetNewMapsModProfile>d__.<>4__this = this;
		<OnGetNewMapsModProfile>d__.resultAndProfile = resultAndProfile;
		<OnGetNewMapsModProfile>d__.<>1__state = -1;
		<OnGetNewMapsModProfile>d__.<>t__builder.Start<NewMapsDisplay.<OnGetNewMapsModProfile>d__19>(ref <OnGetNewMapsModProfile>d__);
	}

	// Token: 0x060026E5 RID: 9957 RVA: 0x000BF697 File Offset: 0x000BD897
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

	// Token: 0x060026E6 RID: 9958 RVA: 0x000BF6BB File Offset: 0x000BD8BB
	public void Update()
	{
		if (!this.slideshowActive || Time.time - this.lastSlideshowUpdate < this.slideshowUpdateInterval)
		{
			return;
		}
		this.UpdateSlideshow();
	}

	// Token: 0x060026E7 RID: 9959 RVA: 0x000BF6E0 File Offset: 0x000BD8E0
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

	// Token: 0x04002AB4 RID: 10932
	[SerializeField]
	private SpriteRenderer mapImage;

	// Token: 0x04002AB5 RID: 10933
	[SerializeField]
	private TMP_Text loadingText;

	// Token: 0x04002AB6 RID: 10934
	[SerializeField]
	private TMP_Text modNameText;

	// Token: 0x04002AB7 RID: 10935
	[SerializeField]
	private TMP_Text modCreatorLabelText;

	// Token: 0x04002AB8 RID: 10936
	[SerializeField]
	private TMP_Text modCreatorText;

	// Token: 0x04002AB9 RID: 10937
	[SerializeField]
	private float slideshowUpdateInterval = 1f;

	// Token: 0x04002ABA RID: 10938
	private ModProfile newMapsModProfile;

	// Token: 0x04002ABB RID: 10939
	private List<NewMapsDisplay.NewMapData> newMapDatas = new List<NewMapsDisplay.NewMapData>();

	// Token: 0x04002ABC RID: 10940
	private bool slideshowActive;

	// Token: 0x04002ABD RID: 10941
	private int slideshowIndex;

	// Token: 0x04002ABE RID: 10942
	private float lastSlideshowUpdate;

	// Token: 0x04002ABF RID: 10943
	private bool requestingNewMapsModProfile;

	// Token: 0x04002AC0 RID: 10944
	private bool downloadingImages;

	// Token: 0x04002AC1 RID: 10945
	private Coroutine initCoroutine;

	// Token: 0x02000618 RID: 1560
	private struct NewMapData
	{
		// Token: 0x04002AC2 RID: 10946
		public Texture2D image;

		// Token: 0x04002AC3 RID: 10947
		public string name;

		// Token: 0x04002AC4 RID: 10948
		public string creator;
	}
}
