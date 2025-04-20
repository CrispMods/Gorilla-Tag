using System;
using GorillaNetworking;
using UnityEngine;

// Token: 0x02000869 RID: 2153
public class HideInQuest1AtRuntime : MonoBehaviour
{
	// Token: 0x0600345E RID: 13406 RVA: 0x000527E1 File Offset: 0x000509E1
	private void OnEnable()
	{
		if (PlayFabAuthenticator.instance != null && "Quest1" == PlayFabAuthenticator.instance.platform.ToString())
		{
			UnityEngine.Object.Destroy(base.gameObject);
		}
	}
}
