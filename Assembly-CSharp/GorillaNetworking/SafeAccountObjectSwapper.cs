using System;
using UnityEngine;

namespace GorillaNetworking
{
	// Token: 0x02000AA0 RID: 2720
	public class SafeAccountObjectSwapper : MonoBehaviour
	{
		// Token: 0x060043F9 RID: 17401 RVA: 0x001421EA File Offset: 0x001403EA
		public void Start()
		{
			if (PlayFabAuthenticator.instance.GetSafety())
			{
				this.SwitchToSafeMode();
			}
			PlayFabAuthenticator instance = PlayFabAuthenticator.instance;
			instance.OnSafetyUpdate = (Action<bool>)Delegate.Combine(instance.OnSafetyUpdate, new Action<bool>(this.SafeAccountUpdated));
		}

		// Token: 0x060043FA RID: 17402 RVA: 0x00142228 File Offset: 0x00140428
		public void SafeAccountUpdated(bool isSafety)
		{
			if (isSafety)
			{
				this.SwitchToSafeMode();
			}
		}

		// Token: 0x060043FB RID: 17403 RVA: 0x00142234 File Offset: 0x00140434
		public void SwitchToSafeMode()
		{
			foreach (GameObject gameObject in this.UnSafeGameObjects)
			{
				if (gameObject != null)
				{
					gameObject.SetActive(false);
				}
			}
			foreach (GameObject gameObject2 in this.UnSafeTexts)
			{
				if (gameObject2 != null)
				{
					gameObject2.SetActive(false);
				}
			}
			foreach (GameObject gameObject3 in this.SafeTexts)
			{
				if (gameObject3 != null)
				{
					gameObject3.SetActive(true);
				}
			}
			foreach (GameObject gameObject4 in this.SafeModeObjects)
			{
				if (gameObject4 != null)
				{
					gameObject4.SetActive(true);
				}
			}
		}

		// Token: 0x0400454C RID: 17740
		public GameObject[] UnSafeGameObjects;

		// Token: 0x0400454D RID: 17741
		public GameObject[] UnSafeTexts;

		// Token: 0x0400454E RID: 17742
		public GameObject[] SafeTexts;

		// Token: 0x0400454F RID: 17743
		public GameObject[] SafeModeObjects;
	}
}
