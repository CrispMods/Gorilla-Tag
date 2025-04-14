using System;
using GorillaNetworking;
using UnityEngine;

// Token: 0x0200084F RID: 2127
public class HideInQuest1AtRuntime : MonoBehaviour
{
	// Token: 0x060033A3 RID: 13219 RVA: 0x000F675E File Offset: 0x000F495E
	private void OnEnable()
	{
		if (PlayFabAuthenticator.instance != null && "Quest1" == PlayFabAuthenticator.instance.platform.ToString())
		{
			Object.Destroy(base.gameObject);
		}
	}
}
