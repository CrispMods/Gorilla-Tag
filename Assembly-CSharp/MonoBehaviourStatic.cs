using System;
using UnityEngine;

// Token: 0x020006C8 RID: 1736
public class MonoBehaviourStatic<T> : MonoBehaviour where T : MonoBehaviour
{
	// Token: 0x1700048B RID: 1163
	// (get) Token: 0x06002AFF RID: 11007 RVA: 0x000D5291 File Offset: 0x000D3491
	public static T Instance
	{
		get
		{
			return MonoBehaviourStatic<T>.gInstance;
		}
	}

	// Token: 0x06002B00 RID: 11008 RVA: 0x000D5298 File Offset: 0x000D3498
	protected void Awake()
	{
		if (MonoBehaviourStatic<T>.gInstance && MonoBehaviourStatic<T>.gInstance != this)
		{
			Object.Destroy(this);
		}
		MonoBehaviourStatic<T>.gInstance = (this as T);
		this.OnAwake();
	}

	// Token: 0x06002B01 RID: 11009 RVA: 0x000023F4 File Offset: 0x000005F4
	protected virtual void OnAwake()
	{
	}

	// Token: 0x0400308A RID: 12426
	protected static T gInstance;
}
