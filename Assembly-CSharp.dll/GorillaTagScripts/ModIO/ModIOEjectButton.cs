using System;
using System.Collections;
using UnityEngine;

namespace GorillaTagScripts.ModIO
{
	// Token: 0x020009EB RID: 2539
	public class ModIOEjectButton : GorillaPressableButton
	{
		// Token: 0x06003F5A RID: 16218 RVA: 0x00058946 File Offset: 0x00056B46
		public override void ButtonActivation()
		{
			base.ButtonActivation();
			base.StartCoroutine(this.ButtonPressed_Local());
			if (!this.processing)
			{
				this.HandleTeleport();
			}
		}

		// Token: 0x06003F5B RID: 16219 RVA: 0x00058969 File Offset: 0x00056B69
		private IEnumerator ButtonPressed_Local()
		{
			this.isOn = true;
			this.UpdateColor();
			yield return new WaitForSeconds(this.debounceTime);
			this.isOn = false;
			this.UpdateColor();
			yield break;
		}

		// Token: 0x06003F5C RID: 16220 RVA: 0x00166880 File Offset: 0x00164A80
		private void HandleTeleport()
		{
			this.processing = true;
			PrivateUIRoom.ForceStartOverlay();
			GorillaTagger.Instance.overrideNotInFocus = true;
			this.dayNightManager.RequestRepopulateLightmaps();
			CustomMapManager.ExitVirtualStump(new Action<bool>(this.FinishTeleport));
			base.StartCoroutine(this.DelayedFinishTeleport());
		}

		// Token: 0x06003F5D RID: 16221 RVA: 0x00058978 File Offset: 0x00056B78
		private IEnumerator DelayedFinishTeleport()
		{
			yield return new WaitForSecondsRealtime(this.maxTeleportProcessingTime);
			this.FinishTeleport(true);
			yield break;
		}

		// Token: 0x06003F5E RID: 16222 RVA: 0x00058987 File Offset: 0x00056B87
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

		// Token: 0x04004082 RID: 16514
		[SerializeField]
		private float maxTeleportProcessingTime = 15f;

		// Token: 0x04004083 RID: 16515
		[SerializeField]
		private BetterDayNightManager dayNightManager;

		// Token: 0x04004084 RID: 16516
		private bool processing;
	}
}
