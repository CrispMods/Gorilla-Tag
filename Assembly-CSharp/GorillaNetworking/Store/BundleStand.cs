using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace GorillaNetworking.Store
{
	// Token: 0x02000AF9 RID: 2809
	public class BundleStand : MonoBehaviour
	{
		// Token: 0x1700073C RID: 1852
		// (get) Token: 0x06004639 RID: 17977 RVA: 0x0014D413 File Offset: 0x0014B613
		public string playfabBundleID
		{
			get
			{
				return this._bundleDataReference.playfabBundleID;
			}
		}

		// Token: 0x0600463A RID: 17978 RVA: 0x0014D420 File Offset: 0x0014B620
		public void Awake()
		{
			this._bundlePurchaseButton.playfabID = this.playfabBundleID;
			if (this._bundleIcon != null && this._bundleDataReference != null && this._bundleDataReference.bundleImage != null)
			{
				this._bundleIcon.sprite = this._bundleDataReference.bundleImage;
			}
		}

		// Token: 0x0600463B RID: 17979 RVA: 0x0014D483 File Offset: 0x0014B683
		public void InitializeEventListeners()
		{
			this.AlreadyOwnEvent.AddListener(new UnityAction(this._bundlePurchaseButton.AlreadyOwn));
			this.ErrorHappenedEvent.AddListener(new UnityAction(this._bundlePurchaseButton.ErrorHappened));
		}

		// Token: 0x0600463C RID: 17980 RVA: 0x0014D4BD File Offset: 0x0014B6BD
		public void NotifyAlreadyOwn()
		{
			this.AlreadyOwnEvent.Invoke();
		}

		// Token: 0x0600463D RID: 17981 RVA: 0x0014D4CA File Offset: 0x0014B6CA
		public void ErrorHappened()
		{
			this.ErrorHappenedEvent.Invoke();
		}

		// Token: 0x0600463E RID: 17982 RVA: 0x0014D4D7 File Offset: 0x0014B6D7
		public void UpdatePurchaseButtonText(string purchaseText)
		{
			if (this._bundlePurchaseButton != null)
			{
				this._bundlePurchaseButton.UpdatePurchaseButtonText(purchaseText);
			}
		}

		// Token: 0x0600463F RID: 17983 RVA: 0x0014D4F3 File Offset: 0x0014B6F3
		public void UpdateDescriptionText(string descriptionText)
		{
			if (this._bundleDescriptionText != null)
			{
				this._bundleDescriptionText.text = descriptionText;
			}
		}

		// Token: 0x040047C9 RID: 18377
		public BundlePurchaseButton _bundlePurchaseButton;

		// Token: 0x040047CA RID: 18378
		[SerializeField]
		public StoreBundleData _bundleDataReference;

		// Token: 0x040047CB RID: 18379
		public GameObject[] EditorOnlyObjects;

		// Token: 0x040047CC RID: 18380
		public Text _bundleDescriptionText;

		// Token: 0x040047CD RID: 18381
		public Image _bundleIcon;

		// Token: 0x040047CE RID: 18382
		public UnityEvent AlreadyOwnEvent;

		// Token: 0x040047CF RID: 18383
		public UnityEvent ErrorHappenedEvent;
	}
}
