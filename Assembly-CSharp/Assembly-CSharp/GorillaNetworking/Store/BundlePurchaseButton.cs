using System;
using System.Collections;
using UnityEngine;

namespace GorillaNetworking.Store
{
	// Token: 0x02000AFA RID: 2810
	public class BundlePurchaseButton : GorillaPressableButton, IGorillaSliceableSimple
	{
		// Token: 0x06004633 RID: 17971 RVA: 0x00015C1D File Offset: 0x00013E1D
		public void OnEnable()
		{
			GorillaSlicerSimpleManager.RegisterSliceable(this, GorillaSlicerSimpleManager.UpdateStep.Update);
		}

		// Token: 0x06004634 RID: 17972 RVA: 0x00015C26 File Offset: 0x00013E26
		public void OnDisable()
		{
			GorillaSlicerSimpleManager.UnregisterSliceable(this, GorillaSlicerSimpleManager.UpdateStep.Update);
		}

		// Token: 0x06004635 RID: 17973 RVA: 0x0014D708 File Offset: 0x0014B908
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

		// Token: 0x06004636 RID: 17974 RVA: 0x0014D76B File Offset: 0x0014B96B
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

		// Token: 0x06004637 RID: 17975 RVA: 0x0014D79C File Offset: 0x0014B99C
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

		// Token: 0x06004638 RID: 17976 RVA: 0x0014D7FC File Offset: 0x0014B9FC
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

		// Token: 0x06004639 RID: 17977 RVA: 0x0014D84E File Offset: 0x0014BA4E
		private IEnumerator ButtonColorUpdate()
		{
			this.buttonRenderer.material = this.pressedMaterial;
			yield return new WaitForSeconds(this.debounceTime);
			this.buttonRenderer.material = (this.isOn ? this.pressedMaterial : this.unpressedMaterial);
			yield break;
		}

		// Token: 0x0600463A RID: 17978 RVA: 0x0014D860 File Offset: 0x0014BA60
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

		// Token: 0x0600463B RID: 17979 RVA: 0x0014D8BC File Offset: 0x0014BABC
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

		// Token: 0x0600463C RID: 17980 RVA: 0x0014D8F7 File Offset: 0x0014BAF7
		public void UpdatePurchaseButtonText(string purchaseText)
		{
			if (!this.bError)
			{
				this.offText = purchaseText;
				this.UpdateColor();
			}
		}

		// Token: 0x0600463E RID: 17982 RVA: 0x0000FD18 File Offset: 0x0000DF18
		bool IGorillaSliceableSimple.get_isActiveAndEnabled()
		{
			return base.isActiveAndEnabled;
		}

		// Token: 0x040047D3 RID: 18387
		public bool bError;

		// Token: 0x040047D4 RID: 18388
		public string ErrorText = "ERROR COMPLETING PURCHASE! PLEASE RESTART THE GAME";

		// Token: 0x040047D5 RID: 18389
		public string AlreadyOwnText = "YOU OWN THE BUNDLE ALREADY! THANK YOU!";

		// Token: 0x040047D6 RID: 18390
		public string UnavailableText = "UNAVAILABLE";

		// Token: 0x040047D7 RID: 18391
		public string playfabID = "";
	}
}
