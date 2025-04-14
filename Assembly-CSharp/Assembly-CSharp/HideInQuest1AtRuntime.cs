using System;
using GorillaNetworking;
using UnityEngine;

// Token: 0x02000852 RID: 2130
public class HideInQuest1AtRuntime : MonoBehaviour
{
	// Token: 0x060033AF RID: 13231 RVA: 0x000F6D26 File Offset: 0x000F4F26
	private void OnEnable()
	{
		if (PlayFabAuthenticator.instance != null && "Quest1" == PlayFabAuthenticator.instance.platform.ToString())
		{
			Object.Destroy(base.gameObject);
		}
	}
}
