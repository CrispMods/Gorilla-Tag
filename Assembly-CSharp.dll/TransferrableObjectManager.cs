using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200040B RID: 1035
[DefaultExecutionOrder(1549)]
public class TransferrableObjectManager : MonoBehaviour
{
	// Token: 0x06001999 RID: 6553 RVA: 0x0004039B File Offset: 0x0003E59B
	protected void Awake()
	{
		if (TransferrableObjectManager.hasInstance && TransferrableObjectManager.instance != this)
		{
			UnityEngine.Object.Destroy(this);
			return;
		}
		TransferrableObjectManager.SetInstance(this);
	}

	// Token: 0x0600199A RID: 6554 RVA: 0x000403BE File Offset: 0x0003E5BE
	protected void OnDestroy()
	{
		if (TransferrableObjectManager.instance == this)
		{
			TransferrableObjectManager.hasInstance = false;
			TransferrableObjectManager.instance = null;
		}
	}

	// Token: 0x0600199B RID: 6555 RVA: 0x000D1594 File Offset: 0x000CF794
	protected void LateUpdate()
	{
		for (int i = 0; i < TransferrableObjectManager.transObs.Count; i++)
		{
			TransferrableObjectManager.transObs[i].TriggeredLateUpdate();
		}
	}

	// Token: 0x0600199C RID: 6556 RVA: 0x000403D9 File Offset: 0x0003E5D9
	public static void CreateManager()
	{
		TransferrableObjectManager.SetInstance(new GameObject("TransferrableObjectManager").AddComponent<TransferrableObjectManager>());
	}

	// Token: 0x0600199D RID: 6557 RVA: 0x000403EF File Offset: 0x0003E5EF
	private static void SetInstance(TransferrableObjectManager manager)
	{
		TransferrableObjectManager.instance = manager;
		TransferrableObjectManager.hasInstance = true;
		if (Application.isPlaying)
		{
			UnityEngine.Object.DontDestroyOnLoad(manager);
		}
	}

	// Token: 0x0600199E RID: 6558 RVA: 0x0004040A File Offset: 0x0003E60A
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

	// Token: 0x0600199F RID: 6559 RVA: 0x00040430 File Offset: 0x0003E630
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
