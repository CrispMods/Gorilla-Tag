using System;
using System.Collections;
using UnityEngine;

namespace GorillaTagScripts.ModIO
{
	// Token: 0x020009E8 RID: 2536
	public class ModIOEjectButton : GorillaPressableButton
	{
		// Token: 0x06003F4E RID: 16206 RVA: 0x0012C001 File Offset: 0x0012A201
		public override void ButtonActivation()
		{
			base.ButtonActivation();
			base.StartCoroutine(this.ButtonPressed_Local());
			if (!this.processing)
			{
				this.HandleTeleport();
			}
		}

		// Token: 0x06003F4F RID: 16207 RVA: 0x0012C024 File Offset: 0x0012A224
		private IEnumerator ButtonPressed_Local()
		{
			this.isOn = true;
			this.UpdateColor();
			yield return new WaitForSeconds(this.debounceTime);
			this.isOn = false;
			this.UpdateColor();
			yield break;
		}

		// Token: 0x06003F50 RID: 16208 RVA: 0x0012C034 File Offset: 0x0012A234
		private void HandleTeleport()
		{
			this.processing = true;
			PrivateUIRoom.ForceStartOverlay();
			GorillaTagger.Instance.overrideNotInFocus = true;
			this.dayNightManager.RequestRepopulateLightmaps();
			CustomMapManager.ExitVirtualStump(new Action<bool>(this.FinishTeleport));
			base.StartCoroutine(this.DelayedFinishTeleport());
		}

		// Token: 0x06003F51 RID: 16209 RVA: 0x0012C081 File Offset: 0x0012A281
		private IEnumerator DelayedFinishTeleport()
		{
			yield return new WaitForSecondsRealtime(this.maxTeleportProcessingTime);
			this.FinishTeleport(true);
			yield break;
		}

		// Token: 0x06003F52 RID: 16210 RVA: 0x0012C090 File Offset: 0x0012A290
		private void FinishTeleport(bool success = true)
		{
			if (this.processing)
			{
				this.processing = false;
				CustomMapManager.DisableTeleportHUD();
				GorillaTagger.Instance.overrideNotInFocus = false;
				PrivateUIRoom.StopForcedOverlay();
			}
		}

		// Token: 0x04004070 RID: 16496
		[SerializeField]
		private float maxTeleportProcessingTime = 15f;

		// Token: 0x04004071 RID: 16497
		[SerializeField]
		private BetterDayNightManager dayNightManager;

		// Token: 0x04004072 RID: 16498
		private bool processing;
	}
}
