using System;
using UnityEngine;

// Token: 0x020005B3 RID: 1459
public class GorillaUIParent : MonoBehaviour
{
	// Token: 0x06002442 RID: 9282 RVA: 0x000487ED File Offset: 0x000469ED
	private void Awake()
	{
		if (GorillaUIParent.instance == null)
		{
			GorillaUIParent.instance = this;
			return;
		}
		if (GorillaUIParent.instance != this)
		{
			UnityEngine.Object.Destroy(base.gameObject);
		}
	}

	// Token: 0x0400283E RID: 10302
	[OnEnterPlay_SetNull]
	public static volatile GorillaUIParent instance;
}
