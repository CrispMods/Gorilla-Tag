using System;
using System.Collections;
using UnityEngine;

namespace GameObjectScheduling.DeepLinks
{
	// Token: 0x02000C8D RID: 3213
	public class DeepLinkButton : GorillaPressableButton
	{
		// Token: 0x060050FF RID: 20735 RVA: 0x001890D2 File Offset: 0x001872D2
		public override void ButtonActivation()
		{
			base.ButtonActivation();
			this.sendingDeepLink = DeepLinkSender.SendDeepLink(this.deepLinkAppID, this.deepLinkPayload, new Action<string>(this.OnDeepLinkSent));
			base.StartCoroutine(this.ButtonPressed_Local());
		}

		// Token: 0x06005100 RID: 20736 RVA: 0x0018910A File Offset: 0x0018730A
		private void OnDeepLinkSent(string message)
		{
			this.sendingDeepLink = false;
			if (!this.isOn)
			{
				this.UpdateColor();
			}
		}

		// Token: 0x06005101 RID: 20737 RVA: 0x00189121 File Offset: 0x00187321
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

		// Token: 0x0400535D RID: 21341
		[SerializeField]
		private ulong deepLinkAppID;

		// Token: 0x0400535E RID: 21342
		[SerializeField]
		private string deepLinkPayload = "";

		// Token: 0x0400535F RID: 21343
		[SerializeField]
		private float pressedTime = 0.2f;

		// Token: 0x04005360 RID: 21344
		private bool sendingDeepLink;
	}
}
