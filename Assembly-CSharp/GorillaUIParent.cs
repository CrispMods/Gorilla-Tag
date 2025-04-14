using System;
using UnityEngine;

// Token: 0x020005A5 RID: 1445
public class GorillaUIParent : MonoBehaviour
{
	// Token: 0x060023E2 RID: 9186 RVA: 0x000B3051 File Offset: 0x000B1251
	private void Awake()
	{
		if (GorillaUIParent.instance == null)
		{
			GorillaUIParent.instance = this;
			return;
		}
		if (GorillaUIParent.instance != this)
		{
			Object.Destroy(base.gameObject);
		}
	}

	// Token: 0x040027E2 RID: 10210
	[OnEnterPlay_SetNull]
	public static volatile GorillaUIParent instance;
}
