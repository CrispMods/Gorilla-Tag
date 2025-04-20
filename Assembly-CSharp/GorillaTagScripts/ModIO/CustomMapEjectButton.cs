using System;
using System.Collections;
using GT_CustomMapSupportRuntime;
using UnityEngine;

namespace GorillaTagScripts.ModIO
{
	// Token: 0x02000A08 RID: 2568
	public class CustomMapEjectButton : GorillaPressableButton
	{
		// Token: 0x06004032 RID: 16434 RVA: 0x00059F56 File Offset: 0x00058156
		public override void ButtonActivation()
		{
			base.ButtonActivation();
			base.StartCoroutine(this.ButtonPressed_Local());
			if (!this.processing)
			{
				this.HandleTeleport();
			}
		}

		// Token: 0x06004033 RID: 16435 RVA: 0x00059F79 File Offset: 0x00058179
		private IEnumerator ButtonPressed_Local()
		{
			this.isOn = true;
			this.UpdateColor();
			yield return new WaitForSeconds(this.debounceTime);
			this.isOn = false;
			this.UpdateColor();
			yield break;
		}

		// Token: 0x06004034 RID: 16436 RVA: 0x0016C5B4 File Offset: 0x0016A7B4
		private void HandleTeleport()
		{
			if (this.processing)
			{
				return;
			}
			this.processing = true;
			CustomMapEjectButton.EjectType ejectType = this.ejectType;
			if (ejectType != CustomMapEjectButton.EjectType.EjectFromVirtualStump)
			{
				if (ejectType == CustomMapEjectButton.EjectType.ReturnToVirtualStump)
				{
					CustomMapManager.ReturnToVirtualStump();
					this.processing = false;
					return;
				}
			}
			else
			{
				CustomMapManager.ExitVirtualStump(new Action<bool>(this.FinishTeleport));
			}
		}

		// Token: 0x06004035 RID: 16437 RVA: 0x00059F88 File Offset: 0x00058188
		private void FinishTeleport(bool success = true)
		{
			if (!this.processing)
			{
				return;
			}
			this.processing = false;
		}

		// Token: 0x06004036 RID: 16438 RVA: 0x00059F9A File Offset: 0x0005819A
		public void CopySettings(CustomMapEjectButtonSettings customMapEjectButtonSettings)
		{
			this.ejectType = (CustomMapEjectButton.EjectType)customMapEjectButtonSettings.ejectType;
		}

		// Token: 0x04004123 RID: 16675
		[SerializeField]
		private CustomMapEjectButton.EjectType ejectType;

		// Token: 0x04004124 RID: 16676
		private bool processing;

		// Token: 0x02000A09 RID: 2569
		public enum EjectType
		{
			// Token: 0x04004126 RID: 16678
			EjectFromVirtualStump,
			// Token: 0x04004127 RID: 16679
			ReturnToVirtualStump
		}
	}
}
