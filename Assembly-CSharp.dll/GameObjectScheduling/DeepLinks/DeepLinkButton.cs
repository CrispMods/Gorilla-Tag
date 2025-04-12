using System;
using System.Collections;
using UnityEngine;

namespace GameObjectScheduling.DeepLinks
{
	// Token: 0x02000C90 RID: 3216
	public class DeepLinkButton : GorillaPressableButton
	{
		// Token: 0x0600510B RID: 20747 RVA: 0x00063C30 File Offset: 0x00061E30
		public override void ButtonActivation()
		{
			base.ButtonActivation();
			this.sendingDeepLink = DeepLinkSender.SendDeepLink(this.deepLinkAppID, this.deepLinkPayload, new Action<string>(this.OnDeepLinkSent));
			base.StartCoroutine(this.ButtonPressed_Local());
		}

		// Token: 0x0600510C RID: 20748 RVA: 0x00063C68 File Offset: 0x00061E68
		private void OnDeepLinkSent(string message)
		{
			this.sendingDeepLink = false;
			if (!this.isOn)
			{
				this.UpdateColor();
			}
		}

		// Token: 0x0600510D RID: 20749 RVA: 0x00063C7F File Offset: 0x00061E7F
		private IEnumerator ButtonPressed_Local()
		{
			this.isOn = true;
			this.UpdateColor();
			yield return new WaitForSeconds(this.pressedTime);
			this.isOn = false;
			if (!this.sendingDeepLink)
			{
				this.UpdateColor();
			}
			yield break;
		}

		// Token: 0x0400536F RID: 21359
		[SerializeField]
		private ulong deepLinkAppID;

		// Token: 0x04005370 RID: 21360
		[SerializeField]
		private string deepLinkPayload = "";

		// Token: 0x04005371 RID: 21361
		[SerializeField]
		private float pressedTime = 0.2f;

		// Token: 0x04005372 RID: 21362
		private bool sendingDeepLink;
	}
}
