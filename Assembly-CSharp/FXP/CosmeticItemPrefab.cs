using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using GameObjectScheduling;
using GorillaExtensions;
using GorillaNetworking;
using GorillaNetworking.Store;
using TMPro;
using UnityEngine;

namespace FXP
{
	// Token: 0x02000A48 RID: 2632
	public class CosmeticItemPrefab : MonoBehaviour
	{
		// Token: 0x0600420E RID: 16910 RVA: 0x0005B398 File Offset: 0x00059598
		private void Awake()
		{
			this.JonsAwakeCode();
		}

		// Token: 0x0600420F RID: 16911 RVA: 0x00174138 File Offset: 0x00172338
		private void JonsAwakeCode()
		{
			this.lastUpdated = -this.updateClock;
			this.isValid = (this.goPedestal && this.goMannequin && this.goCosmeticItem && this.goCosmeticItemNameplate && this.goClock && this.goPreviewMode && this.goAttractMode && this.goPurchaseMode);
			this.goPreviewModeSFX = this.goPreviewMode.transform.GetComponentInChildren<AudioSource>();
			this.goAttractModeSFX = this.goAttractMode.transform.FindChildRecursive("SFXAttractMode").GetComponent<AudioSource>();
			this.goPurchaseModeSFX = this.goPurchaseMode.transform.FindChildRecursive("SFXPurchaseMode").GetComponent<AudioSource>();
			this.goAttractModeVFX = this.goAttractMode.transform.FindChildRecursive("VFXAttractMode").GetComponent<ParticleSystem>();
			this.goPurchaseModeVFX = this.goPurchaseMode.transform.FindChildRecursive("VFXPurchaseMode").GetComponent<ParticleSystem>();
			this.clockTextMesh = this.goClock.GetComponent<TextMeshPro>();
			this.clockTextMeshIsValid = (this.clockTextMesh != null);
			if (this.clockTextMeshIsValid)
			{
				this.defaultCountdownTextTemplate = this.clockTextMesh.text;
			}
			this.isValid = (this.goPreviewModeSFX && this.goAttractModeSFX && this.goPurchaseModeSFX);
		}

		// Token: 0x06004210 RID: 16912 RVA: 0x0005B3A0 File Offset: 0x000595A0
		private void OnDisable()
		{
			if (StoreUpdater.instance != null)
			{
				this.countdownTimerCoRoutine = null;
				this.StopCountdownCoroutine();
				StoreUpdater.instance.PedestalAsleep(this);
			}
		}

		// Token: 0x06004211 RID: 16913 RVA: 0x001742C4 File Offset: 0x001724C4
		private void OnEnable()
		{
			if (this.goPreviewModeSFX == null)
			{
				this.goPreviewModeSFX = this.goPreviewMode.transform.GetComponentInChildren<AudioSource>();
			}
			if (this.goAttractModeSFX == null)
			{
				this.goAttractModeSFX = this.goAttractMode.transform.transform.GetComponentInChildren<AudioSource>();
			}
			if (this.goPurchaseModeSFX == null)
			{
				this.goPurchaseModeSFX = this.goPurchaseMode.transform.transform.GetComponentInChildren<AudioSource>();
			}
			this.isValid = (this.goPreviewModeSFX && this.goAttractModeSFX && this.goPurchaseModeSFX);
			if (StoreUpdater.instance != null)
			{
				StoreUpdater.instance.PedestalAwakened(this);
			}
		}

		// Token: 0x06004212 RID: 16914 RVA: 0x00174394 File Offset: 0x00172594
		public void SwitchDisplayMode(CosmeticItemPrefab.EDisplayMode NewDisplayMode)
		{
			if (!this.isValid)
			{
				return;
			}
			if (NewDisplayMode.Equals(CosmeticItemPrefab.EDisplayMode.NULL))
			{
				return;
			}
			if (NewDisplayMode == this.currentDisplayMode)
			{
				return;
			}
			switch (NewDisplayMode)
			{
			case CosmeticItemPrefab.EDisplayMode.HIDDEN:
			{
				this.goPedestal.SetActive(false);
				this.goMannequin.SetActive(false);
				this.goCosmeticItem.SetActive(false);
				this.goCosmeticItemNameplate.SetActive(false);
				this.goClock.SetActive(false);
				this.goPreviewMode.SetActive(false);
				AudioSource audioSource = this.goPreviewModeSFX;
				if (audioSource != null)
				{
					audioSource.GTStop();
				}
				this.goAttractMode.SetActive(false);
				AudioSource audioSource2 = this.goAttractModeSFX;
				if (audioSource2 != null)
				{
					audioSource2.GTStop();
				}
				this.goPurchaseMode.SetActive(false);
				AudioSource audioSource3 = this.goPurchaseModeSFX;
				if (audioSource3 != null)
				{
					audioSource3.GTStop();
				}
				this.StopPreviewTimer();
				this.StopAttractTimer();
				break;
			}
			case CosmeticItemPrefab.EDisplayMode.PREVIEW:
				this.goPedestal.SetActive(true);
				this.goMannequin.SetActive(true);
				this.goCosmeticItem.SetActive(true);
				this.goCosmeticItemNameplate.SetActive(false);
				this.goClock.SetActive(true);
				this.goAttractMode.SetActive(false);
				this.goAttractModeSFX.GTStop();
				this.goPurchaseMode.SetActive(false);
				this.goPurchaseModeSFX.GTStop();
				this.goPreviewMode.SetActive(true);
				this.goPreviewModeSFX.GTPlay();
				this.StopPreviewTimer();
				this.StartPreviewTimer();
				break;
			case CosmeticItemPrefab.EDisplayMode.ATTRACT:
				this.goPedestal.SetActive(true);
				this.goMannequin.SetActive(true);
				this.goCosmeticItem.SetActive(true);
				this.goCosmeticItemNameplate.SetActive(true);
				this.goClock.SetActive(true);
				this.goPreviewMode.SetActive(false);
				this.goPreviewModeSFX.GTStop();
				this.goPurchaseMode.SetActive(false);
				this.goPurchaseModeSFX.GTStop();
				this.goAttractMode.SetActive(true);
				this.goAttractModeSFX.GTPlay();
				this.StopPreviewTimer();
				this.StartAttractTimer();
				break;
			case CosmeticItemPrefab.EDisplayMode.PURCHASE:
				this.goPedestal.SetActive(true);
				this.goMannequin.SetActive(true);
				this.goCosmeticItem.SetActive(true);
				this.goCosmeticItemNameplate.SetActive(true);
				this.goClock.SetActive(false);
				this.goPreviewMode.SetActive(false);
				this.goPreviewModeSFX.GTStop();
				this.goAttractMode.SetActive(false);
				this.goAttractModeSFX.GTStop();
				this.goPurchaseMode.SetActive(true);
				this.goPurchaseModeSFX.GTPlay();
				this.goCosmeticItemNameplate.GetComponent<TextMesh>().text = "Purchased!";
				this.StopPreviewTimer();
				break;
			case CosmeticItemPrefab.EDisplayMode.POSTPURCHASE:
				this.goPedestal.SetActive(true);
				this.goMannequin.SetActive(true);
				this.goCosmeticItem.SetActive(true);
				this.goCosmeticItemNameplate.SetActive(false);
				this.goClock.SetActive(false);
				this.goPreviewMode.SetActive(false);
				this.goPreviewModeSFX.GTStop();
				this.goAttractMode.SetActive(false);
				this.goAttractModeSFX.GTStop();
				this.goPurchaseMode.SetActive(false);
				this.goPurchaseModeSFX.GTStop();
				this.StopPreviewTimer();
				break;
			}
			this.currentDisplayMode = NewDisplayMode;
		}

		// Token: 0x06004213 RID: 16915 RVA: 0x0005B3CB File Offset: 0x000595CB
		private void Update()
		{
			if (Time.time > this.lastUpdated + this.updateClock)
			{
				this.lastUpdated = Time.time;
				this.UpdateClock();
			}
		}

		// Token: 0x06004214 RID: 16916 RVA: 0x001746E4 File Offset: 0x001728E4
		private void UpdateClock()
		{
			if (this.currentUpdateEvent != null && this.clockTextMeshIsValid && this.clockTextMesh.isActiveAndEnabled)
			{
				TimeSpan ts = this.currentUpdateEvent.EndTimeUTC.ToUniversalTime() - StoreUpdater.instance.DateTimeNowServerAdjusted;
				this.clockTextMesh.text = CountdownText.GetTimeDisplay(ts, this.defaultCountdownTextTemplate);
			}
		}

		// Token: 0x06004215 RID: 16917 RVA: 0x00174748 File Offset: 0x00172948
		public void SetDefaultProperties()
		{
			if (!this.isValid)
			{
				return;
			}
			this.goPedestal.GetComponent<MeshFilter>().sharedMesh = this.defaultPedestalMesh;
			this.goPedestal.GetComponent<MeshRenderer>().sharedMaterial = this.defaultPedestalMaterial;
			this.goMannequin.GetComponent<MeshFilter>().sharedMesh = this.defaultMannequinMesh;
			this.goMannequin.GetComponent<MeshRenderer>().sharedMaterial = this.defaultMannequinMaterial;
			this.goCosmeticItem.GetComponent<MeshFilter>().sharedMesh = this.defaultCosmeticMesh;
			this.goCosmeticItem.GetComponent<MeshRenderer>().sharedMaterial = this.defaultCosmeticMaterial;
			this.goCosmeticItemNameplate.GetComponent<TextMesh>().text = this.defaultItemText;
			this.goPreviewModeSFX.clip = this.defaultSFXPreviewMode;
			this.goAttractModeSFX.clip = this.defaultSFXAttractMode;
			this.goPurchaseModeSFX.clip = this.defaultSFXPurchaseMode;
		}

		// Token: 0x06004216 RID: 16918 RVA: 0x0005B3F2 File Offset: 0x000595F2
		private void ClearCosmeticMesh()
		{
			UnityEngine.Object.Destroy(this.goCosmeticItemGameObject);
		}

		// Token: 0x06004217 RID: 16919 RVA: 0x0005B3FF File Offset: 0x000595FF
		private void ClearCosmeticAtlas()
		{
			if (this.goCosmeticItemMeshAtlas.IsNotNull())
			{
				UnityEngine.Object.Destroy(this.goCosmeticItemMeshAtlas);
			}
		}

		// Token: 0x06004218 RID: 16920 RVA: 0x0017482C File Offset: 0x00172A2C
		public void SetCosmeticItemFromCosmeticController(CosmeticsController.CosmeticItem item)
		{
			if (!this.isValid)
			{
				return;
			}
			this.ClearCosmeticAtlas();
			this.ClearCosmeticMesh();
			this.oldItemID = this.itemID;
			this.itemID = item.itemName;
			this.itemName = item.displayName;
			if (item.overrideDisplayName != string.Empty)
			{
				this.itemName = item.overrideDisplayName;
			}
			this.HeadModel.SetCosmeticActive(this.itemID, false);
			this.SetCosmeticStand();
		}

		// Token: 0x06004219 RID: 16921 RVA: 0x001748A8 File Offset: 0x00172AA8
		public void SetCosmeticStand()
		{
			this.cosmeticStand.thisCosmeticName = this.itemID;
			this.cosmeticStand.InitializeCosmetic();
			if (this.oldItemID.Length > 0)
			{
				if (this.oldItemID != this.itemID)
				{
					this.cosmeticStand.isOn = false;
				}
				this.cosmeticStand.UpdateColor();
			}
		}

		// Token: 0x0600421A RID: 16922 RVA: 0x0017490C File Offset: 0x00172B0C
		public void SetStoreUpdateEvent(StoreUpdateEvent storeUpdateEvent, bool playFX)
		{
			if (!this.isValid)
			{
				return;
			}
			if (playFX)
			{
				this.goAttractMode.SetActive(true);
				this.goAttractModeVFX.Play();
			}
			this.currentUpdateEvent = storeUpdateEvent;
			this.SetCosmeticItemFromCosmeticController(CosmeticsController.instance.GetItemFromDict(storeUpdateEvent.ItemName));
			if (base.isActiveAndEnabled)
			{
				this.countdownTimerCoRoutine = base.StartCoroutine(this.PlayCountdownTimer());
			}
			this.UpdateClock();
		}

		// Token: 0x0600421B RID: 16923 RVA: 0x0005B419 File Offset: 0x00059619
		private IEnumerator PlayCountdownTimer()
		{
			yield return new WaitForSeconds(Mathf.Clamp((float)((this.currentUpdateEvent.EndTimeUTC.ToUniversalTime() - StoreUpdater.instance.DateTimeNowServerAdjusted).TotalSeconds - 10.0), 0f, float.MaxValue));
			this.PlaySFX();
			yield break;
		}

		// Token: 0x0600421C RID: 16924 RVA: 0x0005B428 File Offset: 0x00059628
		public void StopCountdownCoroutine()
		{
			this.CountdownSFX.GTStop();
			this.goAttractModeVFX.Stop();
			if (this.countdownTimerCoRoutine != null)
			{
				base.StopCoroutine(this.countdownTimerCoRoutine);
				this.countdownTimerCoRoutine = null;
			}
		}

		// Token: 0x0600421D RID: 16925 RVA: 0x0017497C File Offset: 0x00172B7C
		private void PlaySFX()
		{
			if (this.currentUpdateEvent != null)
			{
				TimeSpan timeSpan = this.currentUpdateEvent.EndTimeUTC.ToUniversalTime() - StoreUpdater.instance.DateTimeNowServerAdjusted;
				if (timeSpan.TotalSeconds >= 10.0)
				{
					this.CountdownSFX.time = 0f;
					this.CountdownSFX.GTPlay();
					return;
				}
				this.CountdownSFX.time = 10f - (float)timeSpan.TotalSeconds;
				this.CountdownSFX.GTPlay();
			}
		}

		// Token: 0x0600421E RID: 16926 RVA: 0x00174A08 File Offset: 0x00172C08
		public void SetCosmeticItemProperties(string WhichGUID, string Name, List<Transform> SocketsList, int Socket, string PedestalMesh = null, string MannequinMesh = null)
		{
			if (!this.isValid)
			{
				return;
			}
			Guid guid;
			if (!Guid.TryParse(WhichGUID, out guid))
			{
				return;
			}
			this.itemName = Name;
			this.itemSocket = Socket;
			if (this.pedestalMesh != null)
			{
				this.goPedestal.GetComponent<MeshFilter>().sharedMesh = this.pedestalMesh;
			}
		}

		// Token: 0x0600421F RID: 16927 RVA: 0x00174A5C File Offset: 0x00172C5C
		private void StartPreviewTimer()
		{
			if (!this.isValid)
			{
				return;
			}
			if (this.coroutinePreviewTimer != null)
			{
				base.StopCoroutine(this.coroutinePreviewTimer);
				this.coroutinePreviewTimer = null;
			}
			this.coroutinePreviewTimer = this.DoPreviewTimer(DateTime.UtcNow + TimeSpan.FromSeconds((double)((this.hoursInPreviewMode ?? this.defaultHoursInPreviewMode) * 60 * 60)));
			base.StartCoroutine(this.coroutinePreviewTimer);
		}

		// Token: 0x06004220 RID: 16928 RVA: 0x0005B45B File Offset: 0x0005965B
		private void StopPreviewTimer()
		{
			if (!this.isValid)
			{
				return;
			}
			if (this.coroutinePreviewTimer != null)
			{
				base.StopCoroutine(this.coroutinePreviewTimer);
				this.coroutinePreviewTimer = null;
			}
			this.clockTextMesh.text = "Clock";
		}

		// Token: 0x06004221 RID: 16929 RVA: 0x0005B491 File Offset: 0x00059691
		private IEnumerator DoPreviewTimer(DateTime ReleaseTime)
		{
			if (this.isValid)
			{
				bool timerDone = false;
				TimeSpan remainingTime = ReleaseTime - DateTime.UtcNow;
				while (!timerDone)
				{
					string text;
					int delayTime;
					if (remainingTime.TotalSeconds <= 59.0)
					{
						text = remainingTime.Seconds.ToString() + "s";
						delayTime = 1;
					}
					else
					{
						delayTime = 60;
						text = string.Empty;
						if (remainingTime.Days > 0)
						{
							text = text + remainingTime.Days.ToString() + "d ";
						}
						if (remainingTime.Hours > 0)
						{
							text = text + remainingTime.Hours.ToString() + "h ";
						}
						if (remainingTime.Minutes > 0)
						{
							text = text + remainingTime.Minutes.ToString() + "m ";
						}
						text = text.TrimEnd();
					}
					this.clockTextMesh.text = text;
					yield return new WaitForSecondsRealtime((float)delayTime);
					remainingTime = remainingTime.Subtract(TimeSpan.FromSeconds((double)delayTime));
					if (remainingTime.TotalSeconds <= 0.0)
					{
						timerDone = true;
					}
				}
				this.SwitchDisplayMode(CosmeticItemPrefab.EDisplayMode.ATTRACT);
				yield return null;
				remainingTime = default(TimeSpan);
			}
			yield break;
		}

		// Token: 0x06004222 RID: 16930 RVA: 0x00174ADC File Offset: 0x00172CDC
		public void StartAttractTimer()
		{
			if (!this.isValid)
			{
				return;
			}
			if (this.coroutineAttractTimer != null)
			{
				base.StopCoroutine(this.coroutineAttractTimer);
				this.coroutineAttractTimer = null;
			}
			this.coroutineAttractTimer = this.DoAttractTimer(DateTime.UtcNow + TimeSpan.FromSeconds((double)((this.hoursInAttractMode ?? this.defaultHoursInAttractMode) * 60 * 60)));
			base.StartCoroutine(this.coroutineAttractTimer);
		}

		// Token: 0x06004223 RID: 16931 RVA: 0x0005B4A7 File Offset: 0x000596A7
		private void StopAttractTimer()
		{
			if (!this.isValid)
			{
				return;
			}
			if (this.coroutineAttractTimer != null)
			{
				base.StopCoroutine(this.coroutineAttractTimer);
				this.coroutineAttractTimer = null;
			}
			this.goClock.GetComponent<TextMesh>().text = "Clock";
		}

		// Token: 0x06004224 RID: 16932 RVA: 0x0005B4E2 File Offset: 0x000596E2
		private IEnumerator DoAttractTimer(DateTime ReleaseTime)
		{
			if (this.isValid)
			{
				bool timerDone = false;
				TimeSpan remainingTime = ReleaseTime - DateTime.UtcNow;
				while (!timerDone)
				{
					string text;
					int delayTime;
					if (remainingTime.TotalSeconds <= 59.0)
					{
						text = remainingTime.Seconds.ToString() + "s";
						delayTime = 1;
					}
					else
					{
						delayTime = 60;
						text = string.Empty;
						if (remainingTime.Days > 0)
						{
							text = text + remainingTime.Days.ToString() + "d ";
						}
						if (remainingTime.Hours > 0)
						{
							text = text + remainingTime.Hours.ToString() + "h ";
						}
						if (remainingTime.Minutes > 0)
						{
							text = text + remainingTime.Minutes.ToString() + "m ";
						}
						text = text.TrimEnd();
					}
					this.goClock.GetComponent<TextMesh>().text = text;
					yield return new WaitForSecondsRealtime((float)delayTime);
					remainingTime = remainingTime.Subtract(TimeSpan.FromSeconds((double)delayTime));
					if (remainingTime.TotalSeconds <= 0.0)
					{
						timerDone = true;
					}
				}
				this.SwitchDisplayMode(CosmeticItemPrefab.EDisplayMode.HIDDEN);
				yield return null;
				remainingTime = default(TimeSpan);
			}
			yield break;
		}

		// Token: 0x040042EE RID: 17134
		public string PedestalID = "";

		// Token: 0x040042EF RID: 17135
		public HeadModel HeadModel;

		// Token: 0x040042F0 RID: 17136
		[SerializeField]
		private Guid? itemGUID;

		// Token: 0x040042F1 RID: 17137
		[SerializeField]
		private string itemName = string.Empty;

		// Token: 0x040042F2 RID: 17138
		[SerializeField]
		private List<Transform> sockets = new List<Transform>();

		// Token: 0x040042F3 RID: 17139
		[SerializeField]
		private int itemSocket = int.MinValue;

		// Token: 0x040042F4 RID: 17140
		[SerializeField]
		private int? hoursInPreviewMode;

		// Token: 0x040042F5 RID: 17141
		[SerializeField]
		private int? hoursInAttractMode;

		// Token: 0x040042F6 RID: 17142
		[SerializeField]
		private Mesh pedestalMesh;

		// Token: 0x040042F7 RID: 17143
		[SerializeField]
		private Mesh mannequinMesh;

		// Token: 0x040042F8 RID: 17144
		[SerializeField]
		private Mesh cosmeticMesh;

		// Token: 0x040042F9 RID: 17145
		[SerializeField]
		private AudioClip sfxPreviewMode;

		// Token: 0x040042FA RID: 17146
		[SerializeField]
		private AudioClip sfxAttractMode;

		// Token: 0x040042FB RID: 17147
		[SerializeField]
		private AudioClip sfxPurchaseMode;

		// Token: 0x040042FC RID: 17148
		[SerializeField]
		private ParticleSystem vfxPreviewMode;

		// Token: 0x040042FD RID: 17149
		[SerializeField]
		private ParticleSystem vfxAttractMode;

		// Token: 0x040042FE RID: 17150
		[SerializeField]
		private ParticleSystem vfxPurchaseMode;

		// Token: 0x040042FF RID: 17151
		[SerializeField]
		private GameObject goPedestal;

		// Token: 0x04004300 RID: 17152
		[SerializeField]
		private GameObject goMannequin;

		// Token: 0x04004301 RID: 17153
		[SerializeField]
		private GameObject goCosmeticItem;

		// Token: 0x04004302 RID: 17154
		[SerializeField]
		private GameObject goCosmeticItemGameObject;

		// Token: 0x04004303 RID: 17155
		[SerializeField]
		private GameObject goCosmeticItemNameplate;

		// Token: 0x04004304 RID: 17156
		[SerializeField]
		private GameObject goClock;

		// Token: 0x04004305 RID: 17157
		[SerializeField]
		private GameObject goPreviewMode;

		// Token: 0x04004306 RID: 17158
		[SerializeField]
		private GameObject goAttractMode;

		// Token: 0x04004307 RID: 17159
		[SerializeField]
		private GameObject goPurchaseMode;

		// Token: 0x04004308 RID: 17160
		[SerializeField]
		private Mesh defaultPedestalMesh;

		// Token: 0x04004309 RID: 17161
		[SerializeField]
		private Material defaultPedestalMaterial;

		// Token: 0x0400430A RID: 17162
		[SerializeField]
		private Mesh defaultMannequinMesh;

		// Token: 0x0400430B RID: 17163
		[SerializeField]
		private Material defaultMannequinMaterial;

		// Token: 0x0400430C RID: 17164
		[SerializeField]
		private Mesh defaultCosmeticMesh;

		// Token: 0x0400430D RID: 17165
		[SerializeField]
		private Material defaultCosmeticMaterial;

		// Token: 0x0400430E RID: 17166
		[SerializeField]
		private string defaultItemText;

		// Token: 0x0400430F RID: 17167
		[SerializeField]
		private int defaultHoursInPreviewMode;

		// Token: 0x04004310 RID: 17168
		[SerializeField]
		private int defaultHoursInAttractMode;

		// Token: 0x04004311 RID: 17169
		[SerializeField]
		private AudioClip defaultSFXPreviewMode;

		// Token: 0x04004312 RID: 17170
		[SerializeField]
		private AudioClip defaultSFXAttractMode;

		// Token: 0x04004313 RID: 17171
		[SerializeField]
		private AudioClip defaultSFXPurchaseMode;

		// Token: 0x04004314 RID: 17172
		private GameObject goCosmeticItemMeshAtlas;

		// Token: 0x04004315 RID: 17173
		public AudioSource CountdownSFX;

		// Token: 0x04004316 RID: 17174
		private CosmeticItemPrefab.EDisplayMode currentDisplayMode;

		// Token: 0x04004317 RID: 17175
		private bool isValid;

		// Token: 0x04004318 RID: 17176
		[Nullable(2)]
		private AudioSource goPreviewModeSFX;

		// Token: 0x04004319 RID: 17177
		[Nullable(2)]
		private AudioSource goAttractModeSFX;

		// Token: 0x0400431A RID: 17178
		[Nullable(2)]
		private AudioSource goPurchaseModeSFX;

		// Token: 0x0400431B RID: 17179
		[Nullable(2)]
		private ParticleSystem goAttractModeVFX;

		// Token: 0x0400431C RID: 17180
		[Nullable(2)]
		private ParticleSystem goPurchaseModeVFX;

		// Token: 0x0400431D RID: 17181
		private IEnumerator coroutinePreviewTimer;

		// Token: 0x0400431E RID: 17182
		private IEnumerator coroutineAttractTimer;

		// Token: 0x0400431F RID: 17183
		private DateTime startTime;

		// Token: 0x04004320 RID: 17184
		private TextMeshPro clockTextMesh;

		// Token: 0x04004321 RID: 17185
		private bool clockTextMeshIsValid;

		// Token: 0x04004322 RID: 17186
		private StoreUpdateEvent currentUpdateEvent;

		// Token: 0x04004323 RID: 17187
		private string defaultCountdownTextTemplate = "";

		// Token: 0x04004324 RID: 17188
		public CosmeticStand cosmeticStand;

		// Token: 0x04004325 RID: 17189
		public string itemID = "";

		// Token: 0x04004326 RID: 17190
		public string oldItemID = "";

		// Token: 0x04004327 RID: 17191
		private Coroutine countdownTimerCoRoutine;

		// Token: 0x04004328 RID: 17192
		private float updateClock = 60f;

		// Token: 0x04004329 RID: 17193
		private float lastUpdated;

		// Token: 0x02000A49 RID: 2633
		[SerializeField]
		public enum EDisplayMode
		{
			// Token: 0x0400432B RID: 17195
			NULL,
			// Token: 0x0400432C RID: 17196
			HIDDEN,
			// Token: 0x0400432D RID: 17197
			PREVIEW,
			// Token: 0x0400432E RID: 17198
			ATTRACT,
			// Token: 0x0400432F RID: 17199
			PURCHASE,
			// Token: 0x04004330 RID: 17200
			POSTPURCHASE
		}
	}
}
