using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace GorillaNetworking.Store
{
	// Token: 0x02000AFC RID: 2812
	public class BundleStand : MonoBehaviour
	{
		// Token: 0x1700073D RID: 1853
		// (get) Token: 0x06004645 RID: 17989 RVA: 0x0005CEF0 File Offset: 0x0005B0F0
		public string playfabBundleID
		{
			get
			{
				return this._bundleDataReference.playfabBundleID;
			}
		}

		// Token: 0x06004646 RID: 17990 RVA: 0x001833A8 File Offset: 0x001815A8
		public void Awake()
		{
			this._bundlePurchaseButton.playfabID = this.playfabBundleID;
			if (this._bundleIcon != null && this._bundleDataReference != null && this._bundleDataReference.bundleImage != null)
			{
				this._bundleIcon.sprite = this._bundleDataReference.bundleImage;
			}
		}

		// Token: 0x06004647 RID: 17991 RVA: 0x0005CEFD File Offset: 0x0005B0FD
		public void InitializeEventListeners()
		{
			this.AlreadyOwnEvent.AddListener(new UnityAction(this._bundlePurchaseButton.AlreadyOwn));
			this.ErrorHappenedEvent.AddListener(new UnityAction(this._bundlePurchaseButton.ErrorHappened));
		}

		// Token: 0x06004648 RID: 17992 RVA: 0x0005CF37 File Offset: 0x0005B137
		public void NotifyAlreadyOwn()
		{
			this.AlreadyOwnEvent.Invoke();
		}

		// Token: 0x06004649 RID: 17993 RVA: 0x0005CF44 File Offset: 0x0005B144
		public void ErrorHappened()
		{
			this.ErrorHappenedEvent.Invoke();
		}

		// Token: 0x0600464A RID: 17994 RVA: 0x0005CF51 File Offset: 0x0005B151
		public void UpdatePurchaseButtonText(string purchaseText)
		{
			if (this._bundlePurchaseButton != null)
			{
				this._bundlePurchaseButton.UpdatePurchaseButtonText(purchaseText);
			}
		}

		// Token: 0x0600464B RID: 17995 RVA: 0x0005CF6D File Offset: 0x0005B16D
		public void UpdateDescriptionText(string descriptionText)
		{
			if (this._bundleDescriptionText != null)
			{
				this._bundleDescriptionText.text = descriptionText;
			}
		}

		// Token: 0x040047DB RID: 18395
		public BundlePurchaseButton _bundlePurchaseButton;

		// Token: 0x040047DC RID: 18396
		[SerializeField]
		public StoreBundleData _bundleDataReference;

		// Token: 0x040047DD RID: 18397
		public GameObject[] EditorOnlyObjects;

		// Token: 0x040047DE RID: 18398
		public Text _bundleDescriptionText;

		// Token: 0x040047DF RID: 18399
		public Image _bundleIcon;

		// Token: 0x040047E0 RID: 18400
		public UnityEvent AlreadyOwnEvent;

		// Token: 0x040047E1 RID: 18401
		public UnityEvent ErrorHappenedEvent;
	}
}
