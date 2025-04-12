using System;
using UnityEngine;

// Token: 0x020005A6 RID: 1446
public class GorillaUIParent : MonoBehaviour
{
	// Token: 0x060023EA RID: 9194 RVA: 0x00047418 File Offset: 0x00045618
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

	// Token: 0x040027E8 RID: 10216
	[OnEnterPlay_SetNull]
	public static volatile GorillaUIParent instance;
}
