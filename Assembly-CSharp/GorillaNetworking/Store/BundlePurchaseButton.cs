using System;
using System.Collections;
using UnityEngine;

namespace GorillaNetworking.Store
{
	// Token: 0x02000B24 RID: 2852
	public class BundlePurchaseButton : GorillaPressableButton, IGorillaSliceableSimple
	{
		// Token: 0x0600476F RID: 18287 RVA: 0x00032C89 File Offset: 0x00030E89
		public void OnEnable()
		{
			GorillaSlicerSimpleManager.RegisterSliceable(this, GorillaSlicerSimpleManager.UpdateStep.Update);
		}

		// Token: 0x06004770 RID: 18288 RVA: 0x00032C92 File Offset: 0x00030E92
		public void OnDisable()
		{
			GorillaSlicerSimpleManager.UnregisterSliceable(this, GorillaSlicerSimpleManager.UpdateStep.Update);
		}

		// Token: 0x06004771 RID: 18289 RVA: 0x0018A0AC File Offset: 0x001882AC
		public void SliceUpdate()
		{
			if (NetworkSystem.Instance != null && NetworkSystem.Instance.WrongVersion && !this.bError)
			{
				base.enabled = false;
				base.GetComponent<BoxCollider>().enabled = false;
				this.buttonRenderer.material = this.pressedMaterial;
				this.myText.text = this.UnavailableText;
			}
		}

		// Token: 0x06004772 RID: 18290 RVA: 0x0005E813 File Offset: 0x0005CA13
		public override void ButtonActivation()
		{
			if (this.bError)
			{
				return;
			}
			base.ButtonActivation();
			BundleManager.instance.BundlePurchaseButtonPressed(this.playfabID);
			base.StartCoroutine(this.ButtonColorUpdate());
		}

		// Token: 0x06004773 RID: 18291 RVA: 0x0018A110 File Offset: 0x00188310
		public void AlreadyOwn()
		{
			if (this.bError)
			{
				return;
			}
			base.enabled = false;
			base.GetComponent<BoxCollider>().enabled = false;
			this.buttonRenderer.material = this.pressedMaterial;
			this.onText = this.AlreadyOwnText;
			this.myText.text = this.AlreadyOwnText;
			this.isOn = true;
		}

		// Token: 0x06004774 RID: 18292 RVA: 0x0018A170 File Offset: 0x00188370
		public void ResetButton()
		{
			if (this.bError)
			{
				return;
			}
			base.enabled = true;
			base.GetComponent<BoxCollider>().enabled = true;
			this.buttonRenderer.material = this.unpressedMaterial;
			this.myText.text = this.offText;
			this.isOn = false;
		}

		// Token: 0x06004775 RID: 18293 RVA: 0x0005E843 File Offset: 0x0005CA43
		private IEnumerator ButtonColorUpdate()
		{
			this.buttonRenderer.material = this.pressedMaterial;
			yield return new WaitForSeconds(this.debounceTime);
			this.buttonRenderer.material = (this.isOn ? this.pressedMaterial : this.unpressedMaterial);
			yield break;
		}

		// Token: 0x06004776 RID: 18294 RVA: 0x0018A1C4 File Offset: 0x001883C4
		public void ErrorHappened()
		{
			this.bError = true;
			this.myText.text = this.ErrorText;
			this.buttonRenderer.material = this.unpressedMaterial;
			base.enabled = false;
			this.offText = this.ErrorText;
			this.onText = this.ErrorText;
			this.isOn = false;
		}

		// Token: 0x06004777 RID: 18295 RVA: 0x0005E852 File Offset: 0x0005CA52
		public void InitializeData()
		{
			if (this.bError)
			{
				return;
			}
			this.myText.text = this.offText;
			this.buttonRenderer.material = this.unpressedMaterial;
			base.enabled = true;
			this.isOn = false;
		}

		// Token: 0x06004778 RID: 18296 RVA: 0x0005E88D File Offset: 0x0005CA8D
		public void UpdatePurchaseButtonText(string purchaseText)
		{
			if (!this.bError)
			{
				this.offText = purchaseText;
				this.UpdateColor();
			}
		}

		// Token: 0x0600477A RID: 18298 RVA: 0x00032105 File Offset: 0x00030305
		bool IGorillaSliceableSimple.get_isActiveAndEnabled()
		{
			return base.isActiveAndEnabled;
		}

		// Token: 0x040048B6 RID: 18614
		public bool bError;

		// Token: 0x040048B7 RID: 18615
		public string ErrorText = "ERROR COMPLETING PURCHASE! PLEASE RESTART THE GAME";

		// Token: 0x040048B8 RID: 18616
		public string AlreadyOwnText = "YOU OWN THE BUNDLE ALREADY! THANK YOU!";

		// Token: 0x040048B9 RID: 18617
		public string UnavailableText = "UNAVAILABLE";

		// Token: 0x040048BA RID: 18618
		public string playfabID = "";
	}
}
