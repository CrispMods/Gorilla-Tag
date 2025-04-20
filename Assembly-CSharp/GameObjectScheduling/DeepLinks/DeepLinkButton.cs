using System;
using System.Collections;
using UnityEngine;

namespace GameObjectScheduling.DeepLinks
{
	// Token: 0x02000CBE RID: 3262
	public class DeepLinkButton : GorillaPressableButton
	{
		// Token: 0x06005261 RID: 21089 RVA: 0x000656A6 File Offset: 0x000638A6
		public override void ButtonActivation()
		{
			base.ButtonActivation();
			this.sendingDeepLink = DeepLinkSender.SendDeepLink(this.deepLinkAppID, this.deepLinkPayload, new Action<string>(this.OnDeepLinkSent));
			base.StartCoroutine(this.ButtonPressed_Local());
		}

		// Token: 0x06005262 RID: 21090 RVA: 0x000656DE File Offset: 0x000638DE
		private void OnDeepLinkSent(string message)
		{
			this.sendingDeepLink = false;
			if (!this.isOn)
			{
				this.UpdateColor();
			}
		}

		// Token: 0x06005263 RID: 21091 RVA: 0x000656F5 File Offset: 0x000638F5
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

		// Token: 0x04005469 RID: 21609
		[SerializeField]
		private ulong deepLinkAppID;

		// Token: 0x0400546A RID: 21610
		[SerializeField]
		private string deepLinkPayload = "";

		// Token: 0x0400546B RID: 21611
		[SerializeField]
		private float pressedTime = 0.2f;

		// Token: 0x0400546C RID: 21612
		private bool sendingDeepLink;
	}
}
