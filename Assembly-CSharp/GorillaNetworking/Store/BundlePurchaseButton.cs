using System;
using System.Collections;
using UnityEngine;

namespace GorillaNetworking.Store
{
	// Token: 0x02000AF7 RID: 2807
	public class BundlePurchaseButton : GorillaPressableButton, IGorillaSliceableSimple
	{
		// Token: 0x06004627 RID: 17959 RVA: 0x000158F9 File Offset: 0x00013AF9
		public void OnEnable()
		{
			GorillaSlicerSimpleManager.RegisterSliceable(this, GorillaSlicerSimpleManager.UpdateStep.Update);
		}

		// Token: 0x06004628 RID: 17960 RVA: 0x00015902 File Offset: 0x00013B02
		public void OnDisable()
		{
			GorillaSlicerSimpleManager.UnregisterSliceable(this, GorillaSlicerSimpleManager.UpdateStep.Update);
		}

		// Token: 0x06004629 RID: 17961 RVA: 0x0014D140 File Offset: 0x0014B340
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

		// Token: 0x0600462A RID: 17962 RVA: 0x0014D1A3 File Offset: 0x0014B3A3
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

		// Token: 0x0600462B RID: 17963 RVA: 0x0014D1D4 File Offset: 0x0014B3D4
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

		// Token: 0x0600462C RID: 17964 RVA: 0x0014D234 File Offset: 0x0014B434
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

		// Token: 0x0600462D RID: 17965 RVA: 0x0014D286 File Offset: 0x0014B486
		private IEnumerator ButtonColorUpdate()
		{
			this.buttonRenderer.material = this.pressedMaterial;
			yield return new WaitForSeconds(this.debounceTime);
			this.buttonRenderer.material = (this.isOn ? this.pressedMaterial : this.unpressedMaterial);
			yield break;
		}

		// Token: 0x0600462E RID: 17966 RVA: 0x0014D298 File Offset: 0x0014B498
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

		// Token: 0x0600462F RID: 17967 RVA: 0x0014D2F4 File Offset: 0x0014B4F4
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

		// Token: 0x06004630 RID: 17968 RVA: 0x0014D32F File Offset: 0x0014B52F
		public void UpdatePurchaseButtonText(string purchaseText)
		{
			if (!this.bError)
			{
				this.offText = purchaseText;
				this.UpdateColor();
			}
		}

		// Token: 0x06004632 RID: 17970 RVA: 0x0000F974 File Offset: 0x0000DB74
		bool IGorillaSliceableSimple.get_isActiveAndEnabled()
		{
			return base.isActiveAndEnabled;
		}

		// Token: 0x040047C1 RID: 18369
		public bool bError;

		// Token: 0x040047C2 RID: 18370
		public string ErrorText = "ERROR COMPLETING PURCHASE! PLEASE RESTART THE GAME";

		// Token: 0x040047C3 RID: 18371
		public string AlreadyOwnText = "YOU OWN THE BUNDLE ALREADY! THANK YOU!";

		// Token: 0x040047C4 RID: 18372
		public string UnavailableText = "UNAVAILABLE";

		// Token: 0x040047C5 RID: 18373
		public string playfabID = "";
	}
}
