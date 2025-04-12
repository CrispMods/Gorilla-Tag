using System;
using UnityEngine;

namespace GorillaNetworking
{
	// Token: 0x02000AA3 RID: 2723
	public class SafeAccountObjectSwapper : MonoBehaviour
	{
		// Token: 0x06004405 RID: 17413 RVA: 0x0005B837 File Offset: 0x00059A37
		public void Start()
		{
			if (PlayFabAuthenticator.instance.GetSafety())
			{
				this.SwitchToSafeMode();
			}
			PlayFabAuthenticator instance = PlayFabAuthenticator.instance;
			instance.OnSafetyUpdate = (Action<bool>)Delegate.Combine(instance.OnSafetyUpdate, new Action<bool>(this.SafeAccountUpdated));
		}

		// Token: 0x06004406 RID: 17414 RVA: 0x0005B875 File Offset: 0x00059A75
		public void SafeAccountUpdated(bool isSafety)
		{
			if (isSafety)
			{
				this.SwitchToSafeMode();
			}
		}

		// Token: 0x06004407 RID: 17415 RVA: 0x0017985C File Offset: 0x00177A5C
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

		// Token: 0x0400455E RID: 17758
		public GameObject[] UnSafeGameObjects;

		// Token: 0x0400455F RID: 17759
		public GameObject[] UnSafeTexts;

		// Token: 0x04004560 RID: 17760
		public GameObject[] SafeTexts;

		// Token: 0x04004561 RID: 17761
		public GameObject[] SafeModeObjects;
	}
}
