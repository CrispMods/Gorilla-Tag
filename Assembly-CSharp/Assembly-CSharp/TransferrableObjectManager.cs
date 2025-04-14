using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200040B RID: 1035
[DefaultExecutionOrder(1549)]
public class TransferrableObjectManager : MonoBehaviour
{
	// Token: 0x06001999 RID: 6553 RVA: 0x0007E7C6 File Offset: 0x0007C9C6
	protected void Awake()
	{
		if (TransferrableObjectManager.hasInstance && TransferrableObjectManager.instance != this)
		{
			Object.Destroy(this);
			return;
		}
		TransferrableObjectManager.SetInstance(this);
	}

	// Token: 0x0600199A RID: 6554 RVA: 0x0007E7E9 File Offset: 0x0007C9E9
	protected void OnDestroy()
	{
		if (TransferrableObjectManager.instance == this)
		{
			TransferrableObjectManager.hasInstance = false;
			TransferrableObjectManager.instance = null;
		}
	}

	// Token: 0x0600199B RID: 6555 RVA: 0x0007E804 File Offset: 0x0007CA04
	protected void LateUpdate()
	{
		for (int i = 0; i < TransferrableObjectManager.transObs.Count; i++)
		{
			TransferrableObjectManager.transObs[i].TriggeredLateUpdate();
		}
	}

	// Token: 0x0600199C RID: 6556 RVA: 0x0007E836 File Offset: 0x0007CA36
	public static void CreateManager()
	{
		TransferrableObjectManager.SetInstance(new GameObject("TransferrableObjectManager").AddComponent<TransferrableObjectManager>());
	}

	// Token: 0x0600199D RID: 6557 RVA: 0x0007E84C File Offset: 0x0007CA4C
	private static void SetInstance(TransferrableObjectManager manager)
	{
		TransferrableObjectManager.instance = manager;
		TransferrableObjectManager.hasInstance = true;
		if (Application.isPlaying)
		{
			Object.DontDestroyOnLoad(manager);
		}
	}

	// Token: 0x0600199E RID: 6558 RVA: 0x0007E867 File Offset: 0x0007CA67
	public static void Register(TransferrableObject transOb)
	{
		if (!TransferrableObjectManager.hasInstance)
		{
			TransferrableObjectManager.CreateManager();
		}
		if (!TransferrableObjectManager.transObs.Contains(transOb))
		{
			TransferrableObjectManager.transObs.Add(transOb);
		}
	}

	// Token: 0x0600199F RID: 6559 RVA: 0x0007E88D File Offset: 0x0007CA8D
	public static void Unregister(TransferrableObject transOb)
	{
		if (!TransferrableObjectManager.hasInstance)
		{
			TransferrableObjectManager.CreateManager();
		}
		if (TransferrableObjectManager.transObs.Contains(transOb))
		{
			TransferrableObjectManager.transObs.Remove(transOb);
		}
	}

	// Token: 0x04001C83 RID: 7299
	public static TransferrableObjectManager instance;

	// Token: 0x04001C84 RID: 7300
	public static bool hasInstance = false;

	// Token: 0x04001C85 RID: 7301
	public static readonly List<TransferrableObject> transObs = new List<TransferrableObject>(1024);
}
