using System;
using UnityEngine;

// Token: 0x020006C9 RID: 1737
public class MonoBehaviourStatic<T> : MonoBehaviour where T : MonoBehaviour
{
	// Token: 0x1700048C RID: 1164
	// (get) Token: 0x06002B07 RID: 11015 RVA: 0x0004C4FC File Offset: 0x0004A6FC
	public static T Instance
	{
		get
		{
			return MonoBehaviourStatic<T>.gInstance;
		}
	}

	// Token: 0x06002B08 RID: 11016 RVA: 0x0011C2C0 File Offset: 0x0011A4C0
	protected void Awake()
	{
		if (MonoBehaviourStatic<T>.gInstance && MonoBehaviourStatic<T>.gInstance != this)
		{
			UnityEngine.Object.Destroy(this);
		}
		MonoBehaviourStatic<T>.gInstance = (this as T);
		this.OnAwake();
	}

	// Token: 0x06002B09 RID: 11017 RVA: 0x0002F75F File Offset: 0x0002D95F
	protected virtual void OnAwake()
	{
	}

	// Token: 0x04003090 RID: 12432
	protected static T gInstance;
}
