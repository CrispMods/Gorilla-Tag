using System;
using UnityEngine;

namespace GorillaNetworking
{
	// Token: 0x02000ACD RID: 2765
	public class SafeAccountObjectSwapper : MonoBehaviour
	{
		// Token: 0x0600453E RID: 17726 RVA: 0x0005D216 File Offset: 0x0005B416
		public void Start()
		{
			if (PlayFabAuthenticator.instance.GetSafety())
			{
				this.SwitchToSafeMode();
			}
			PlayFabAuthenticator instance = PlayFabAuthenticator.instance;
			instance.OnSafetyUpdate = (Action<bool>)Delegate.Combine(instance.OnSafetyUpdate, new Action<bool>(this.SafeAccountUpdated));
		}

		// Token: 0x0600453F RID: 17727 RVA: 0x0005D254 File Offset: 0x0005B454
		public void SafeAccountUpdated(bool isSafety)
		{
			if (isSafety)
			{
				this.SwitchToSafeMode();
			}
		}

		// Token: 0x06004540 RID: 17728 RVA: 0x0018076C File Offset: 0x0017E96C
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

		// Token: 0x04004643 RID: 17987
		public GameObject[] UnSafeGameObjects;

		// Token: 0x04004644 RID: 17988
		public GameObject[] UnSafeTexts;

		// Token: 0x04004645 RID: 17989
		public GameObject[] SafeTexts;

		// Token: 0x04004646 RID: 17990
		public GameObject[] SafeModeObjects;
	}
}
