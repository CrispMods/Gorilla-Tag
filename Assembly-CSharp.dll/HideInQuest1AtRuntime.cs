using System;
using GorillaNetworking;
using UnityEngine;

// Token: 0x02000852 RID: 2130
public class HideInQuest1AtRuntime : MonoBehaviour
{
	// Token: 0x060033AF RID: 13231 RVA: 0x000513D3 File Offset: 0x0004F5D3
	private void OnEnable()
	{
		if (PlayFabAuthenticator.instance != null && "Quest1" == PlayFabAuthenticator.instance.platform.ToString())
		{
			UnityEngine.Object.Destroy(base.gameObject);
		}
	}
}
