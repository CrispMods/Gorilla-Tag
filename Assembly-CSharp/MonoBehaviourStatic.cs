using System;
using UnityEngine;

// Token: 0x020006DD RID: 1757
public class MonoBehaviourStatic<T> : MonoBehaviour where T : MonoBehaviour
{
	// Token: 0x17000498 RID: 1176
	// (get) Token: 0x06002B95 RID: 11157 RVA: 0x0004D841 File Offset: 0x0004BA41
	public static T Instance
	{
		get
		{
			return MonoBehaviourStatic<T>.gInstance;
		}
	}

	// Token: 0x06002B96 RID: 11158 RVA: 0x00120E78 File Offset: 0x0011F078
	protected void Awake()
	{
		if (MonoBehaviourStatic<T>.gInstance && MonoBehaviourStatic<T>.gInstance != this)
		{
			UnityEngine.Object.Destroy(this);
		}
		MonoBehaviourStatic<T>.gInstance = (this as T);
		this.OnAwake();
	}

	// Token: 0x06002B97 RID: 11159 RVA: 0x00030607 File Offset: 0x0002E807
	protected virtual void OnAwake()
	{
	}

	// Token: 0x04003127 RID: 12583
	protected static T gInstance;
}
