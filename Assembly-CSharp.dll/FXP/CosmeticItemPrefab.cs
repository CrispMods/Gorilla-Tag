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
	// Token: 0x02000A1E RID: 2590
	public class CosmeticItemPrefab : MonoBehaviour
	{
		// Token: 0x060040D5 RID: 16597 RVA: 0x00059996 File Offset: 0x00057B96
		private void Awake()
		{
			this.JonsAwakeCode();
		}

		// Token: 0x060040D6 RID: 16598 RVA: 0x0016D2B4 File Offset: 0x0016B4B4
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

		// Token: 0x060040D7 RID: 16599 RVA: 0x0005999E File Offset: 0x00057B9E
		private void OnDisable()
		{
			if (StoreUpdater.instance != null)
			{
				this.countdownTimerCoRoutine = null;
				this.StopCountdownCoroutine();
				StoreUpdater.instance.PedestalAsleep(this);
			}
		}

		// Token: 0x060040D8 RID: 16600 RVA: 0x0016D440 File Offset: 0x0016B640
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

		// Token: 0x060040D9 RID: 16601 RVA: 0x0016D510 File Offset: 0x0016B710
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

		// Token: 0x060040DA RID: 16602 RVA: 0x000599C9 File Offset: 0x00057BC9
		private void Update()
		{
			if (Time.time > this.lastUpdated + this.updateClock)
			{
				this.lastUpdated = Time.time;
				this.UpdateClock();
			}
		}

		// Token: 0x060040DB RID: 16603 RVA: 0x0016D860 File Offset: 0x0016BA60
		private void UpdateClock()
		{
			if (this.currentUpdateEvent != null && this.clockTextMeshIsValid && this.clockTextMesh.isActiveAndEnabled)
			{
				TimeSpan ts = this.currentUpdateEvent.EndTimeUTC.ToUniversalTime() - StoreUpdater.instance.DateTimeNowServerAdjusted;
				this.clockTextMesh.text = CountdownText.GetTimeDisplay(ts, this.defaultCountdownTextTemplate);
			}
		}

		// Token: 0x060040DC RID: 16604 RVA: 0x0016D8C4 File Offset: 0x0016BAC4
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

		// Token: 0x060040DD RID: 16605 RVA: 0x000599F0 File Offset: 0x00057BF0
		private void ClearCosmeticMesh()
		{
			UnityEngine.Object.Destroy(this.goCosmeticItemGameObject);
		}

		// Token: 0x060040DE RID: 16606 RVA: 0x000599FD File Offset: 0x00057BFD
		private void ClearCosmeticAtlas()
		{
			if (this.goCosmeticItemMeshAtlas.IsNotNull())
			{
				UnityEngine.Object.Destroy(this.goCosmeticItemMeshAtlas);
			}
		}

		// Token: 0x060040DF RID: 16607 RVA: 0x0016D9A8 File Offset: 0x0016BBA8
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

		// Token: 0x060040E0 RID: 16608 RVA: 0x0016DA24 File Offset: 0x0016BC24
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

		// Token: 0x060040E1 RID: 16609 RVA: 0x0016DA88 File Offset: 0x0016BC88
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

		// Token: 0x060040E2 RID: 16610 RVA: 0x00059A17 File Offset: 0x00057C17
		private IEnumerator PlayCountdownTimer()
		{
			yield return new WaitForSeconds(Mathf.Clamp((float)((this.currentUpdateEvent.EndTimeUTC.ToUniversalTime() - StoreUpdater.instance.DateTimeNowServerAdjusted).TotalSeconds - 10.0), 0f, float.MaxValue));
			this.PlaySFX();
			yield break;
		}

		// Token: 0x060040E3 RID: 16611 RVA: 0x00059A26 File Offset: 0x00057C26
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

		// Token: 0x060040E4 RID: 16612 RVA: 0x0016DAF8 File Offset: 0x0016BCF8
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

		// Token: 0x060040E5 RID: 16613 RVA: 0x0016DB84 File Offset: 0x0016BD84
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

		// Token: 0x060040E6 RID: 16614 RVA: 0x0016DBD8 File Offset: 0x0016BDD8
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

		// Token: 0x060040E7 RID: 16615 RVA: 0x00059A59 File Offset: 0x00057C59
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

		// Token: 0x060040E8 RID: 16616 RVA: 0x00059A8F File Offset: 0x00057C8F
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

		// Token: 0x060040E9 RID: 16617 RVA: 0x0016DC58 File Offset: 0x0016BE58
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

		// Token: 0x060040EA RID: 16618 RVA: 0x00059AA5 File Offset: 0x00057CA5
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

		// Token: 0x060040EB RID: 16619 RVA: 0x00059AE0 File Offset: 0x00057CE0
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

		// Token: 0x04004206 RID: 16902
		public string PedestalID = "";

		// Token: 0x04004207 RID: 16903
		public HeadModel HeadModel;

		// Token: 0x04004208 RID: 16904
		[SerializeField]
		private Guid? itemGUID;

		// Token: 0x04004209 RID: 16905
		[SerializeField]
		private string itemName = string.Empty;

		// Token: 0x0400420A RID: 16906
		[SerializeField]
		private List<Transform> sockets = new List<Transform>();

		// Token: 0x0400420B RID: 16907
		[SerializeField]
		private int itemSocket = int.MinValue;

		// Token: 0x0400420C RID: 16908
		[SerializeField]
		private int? hoursInPreviewMode;

		// Token: 0x0400420D RID: 16909
		[SerializeField]
		private int? hoursInAttractMode;

		// Token: 0x0400420E RID: 16910
		[SerializeField]
		private Mesh pedestalMesh;

		// Token: 0x0400420F RID: 16911
		[SerializeField]
		private Mesh mannequinMesh;

		// Token: 0x04004210 RID: 16912
		[SerializeField]
		private Mesh cosmeticMesh;

		// Token: 0x04004211 RID: 16913
		[SerializeField]
		private AudioClip sfxPreviewMode;

		// Token: 0x04004212 RID: 16914
		[SerializeField]
		private AudioClip sfxAttractMode;

		// Token: 0x04004213 RID: 16915
		[SerializeField]
		private AudioClip sfxPurchaseMode;

		// Token: 0x04004214 RID: 16916
		[SerializeField]
		private ParticleSystem vfxPreviewMode;

		// Token: 0x04004215 RID: 16917
		[SerializeField]
		private ParticleSystem vfxAttractMode;

		// Token: 0x04004216 RID: 16918
		[SerializeField]
		private ParticleSystem vfxPurchaseMode;

		// Token: 0x04004217 RID: 16919
		[SerializeField]
		private GameObject goPedestal;

		// Token: 0x04004218 RID: 16920
		[SerializeField]
		private GameObject goMannequin;

		// Token: 0x04004219 RID: 16921
		[SerializeField]
		private GameObject goCosmeticItem;

		// Token: 0x0400421A RID: 16922
		[SerializeField]
		private GameObject goCosmeticItemGameObject;

		// Token: 0x0400421B RID: 16923
		[SerializeField]
		private GameObject goCosmeticItemNameplate;

		// Token: 0x0400421C RID: 16924
		[SerializeField]
		private GameObject goClock;

		// Token: 0x0400421D RID: 16925
		[SerializeField]
		private GameObject goPreviewMode;

		// Token: 0x0400421E RID: 16926
		[SerializeField]
		private GameObject goAttractMode;

		// Token: 0x0400421F RID: 16927
		[SerializeField]
		private GameObject goPurchaseMode;

		// Token: 0x04004220 RID: 16928
		[SerializeField]
		private Mesh defaultPedestalMesh;

		// Token: 0x04004221 RID: 16929
		[SerializeField]
		private Material defaultPedestalMaterial;

		// Token: 0x04004222 RID: 16930
		[SerializeField]
		private Mesh defaultMannequinMesh;

		// Token: 0x04004223 RID: 16931
		[SerializeField]
		private Material defaultMannequinMaterial;

		// Token: 0x04004224 RID: 16932
		[SerializeField]
		private Mesh defaultCosmeticMesh;

		// Token: 0x04004225 RID: 16933
		[SerializeField]
		private Material defaultCosmeticMaterial;

		// Token: 0x04004226 RID: 16934
		[SerializeField]
		private string defaultItemText;

		// Token: 0x04004227 RID: 16935
		[SerializeField]
		private int defaultHoursInPreviewMode;

		// Token: 0x04004228 RID: 16936
		[SerializeField]
		private int defaultHoursInAttractMode;

		// Token: 0x04004229 RID: 16937
		[SerializeField]
		private AudioClip defaultSFXPreviewMode;

		// Token: 0x0400422A RID: 16938
		[SerializeField]
		private AudioClip defaultSFXAttractMode;

		// Token: 0x0400422B RID: 16939
		[SerializeField]
		private AudioClip defaultSFXPurchaseMode;

		// Token: 0x0400422C RID: 16940
		private GameObject goCosmeticItemMeshAtlas;

		// Token: 0x0400422D RID: 16941
		public AudioSource CountdownSFX;

		// Token: 0x0400422E RID: 16942
		private CosmeticItemPrefab.EDisplayMode currentDisplayMode;

		// Token: 0x0400422F RID: 16943
		private bool isValid;

		// Token: 0x04004230 RID: 16944
		[Nullable(2)]
		private AudioSource goPreviewModeSFX;

		// Token: 0x04004231 RID: 16945
		[Nullable(2)]
		private AudioSource goAttractModeSFX;

		// Token: 0x04004232 RID: 16946
		[Nullable(2)]
		private AudioSource goPurchaseModeSFX;

		// Token: 0x04004233 RID: 16947
		[Nullable(2)]
		private ParticleSystem goAttractModeVFX;

		// Token: 0x04004234 RID: 16948
		[Nullable(2)]
		private ParticleSystem goPurchaseModeVFX;

		// Token: 0x04004235 RID: 16949
		private IEnumerator coroutinePreviewTimer;

		// Token: 0x04004236 RID: 16950
		private IEnumerator coroutineAttractTimer;

		// Token: 0x04004237 RID: 16951
		private DateTime startTime;

		// Token: 0x04004238 RID: 16952
		private TextMeshPro clockTextMesh;

		// Token: 0x04004239 RID: 16953
		private bool clockTextMeshIsValid;

		// Token: 0x0400423A RID: 16954
		private StoreUpdateEvent currentUpdateEvent;

		// Token: 0x0400423B RID: 16955
		private string defaultCountdownTextTemplate = "";

		// Token: 0x0400423C RID: 16956
		public CosmeticStand cosmeticStand;

		// Token: 0x0400423D RID: 16957
		public string itemID = "";

		// Token: 0x0400423E RID: 16958
		public string oldItemID = "";

		// Token: 0x0400423F RID: 16959
		private Coroutine countdownTimerCoRoutine;

		// Token: 0x04004240 RID: 16960
		private float updateClock = 60f;

		// Token: 0x04004241 RID: 16961
		private float lastUpdated;

		// Token: 0x02000A1F RID: 2591
		[SerializeField]
		public enum EDisplayMode
		{
			// Token: 0x04004243 RID: 16963
			NULL,
			// Token: 0x04004244 RID: 16964
			HIDDEN,
			// Token: 0x04004245 RID: 16965
			PREVIEW,
			// Token: 0x04004246 RID: 16966
			ATTRACT,
			// Token: 0x04004247 RID: 16967
			PURCHASE,
			// Token: 0x04004248 RID: 16968
			POSTPURCHASE
		}
	}
}
