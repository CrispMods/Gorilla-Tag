using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace GorillaNetworking.Store
{
	// Token: 0x02000B26 RID: 2854
	public class BundleStand : MonoBehaviour
	{
		// Token: 0x17000758 RID: 1880
		// (get) Token: 0x06004781 RID: 18305 RVA: 0x0005E8EF File Offset: 0x0005CAEF
		public string playfabBundleID
		{
			get
			{
				return this._bundleDataReference.playfabBundleID;
			}
		}

		// Token: 0x06004782 RID: 18306 RVA: 0x0018A2A0 File Offset: 0x001884A0
		public void Awake()
		{
			this._bundlePurchaseButton.playfabID = this.playfabBundleID;
			if (this._bundleIcon != null && this._bundleDataReference != null && this._bundleDataReference.bundleImage != null)
			{
				this._bundleIcon.sprite = this._bundleDataReference.bundleImage;
			}
		}

		// Token: 0x06004783 RID: 18307 RVA: 0x0005E8FC File Offset: 0x0005CAFC
		public void InitializeEventListeners()
		{
			this.AlreadyOwnEvent.AddListener(new UnityAction(this._bundlePurchaseButton.AlreadyOwn));
			this.ErrorHappenedEvent.AddListener(new UnityAction(this._bundlePurchaseButton.ErrorHappened));
		}

		// Token: 0x06004784 RID: 18308 RVA: 0x0005E936 File Offset: 0x0005CB36
		public void NotifyAlreadyOwn()
		{
			this.AlreadyOwnEvent.Invoke();
		}

		// Token: 0x06004785 RID: 18309 RVA: 0x0005E943 File Offset: 0x0005CB43
		public void ErrorHappened()
		{
			this.ErrorHappenedEvent.Invoke();
		}

		// Token: 0x06004786 RID: 18310 RVA: 0x0005E950 File Offset: 0x0005CB50
		public void UpdatePurchaseButtonText(string purchaseText)
		{
			if (this._bundlePurchaseButton != null)
			{
				this._bundlePurchaseButton.UpdatePurchaseButtonText(purchaseText);
			}
		}

		// Token: 0x06004787 RID: 18311 RVA: 0x0005E96C File Offset: 0x0005CB6C
		public void UpdateDescriptionText(string descriptionText)
		{
			if (this._bundleDescriptionText != null)
			{
				this._bundleDescriptionText.text = descriptionText;
			}
		}

		// Token: 0x040048BE RID: 18622
		public BundlePurchaseButton _bundlePurchaseButton;

		// Token: 0x040048BF RID: 18623
		[SerializeField]
		public StoreBundleData _bundleDataReference;

		// Token: 0x040048C0 RID: 18624
		public GameObject[] EditorOnlyObjects;

		// Token: 0x040048C1 RID: 18625
		public Text _bundleDescriptionText;

		// Token: 0x040048C2 RID: 18626
		public Image _bundleIcon;

		// Token: 0x040048C3 RID: 18627
		public UnityEvent AlreadyOwnEvent;

		// Token: 0x040048C4 RID: 18628
		public UnityEvent ErrorHappenedEvent;
	}
}
