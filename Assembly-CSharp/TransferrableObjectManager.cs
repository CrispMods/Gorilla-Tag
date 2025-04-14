using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200040B RID: 1035
[DefaultExecutionOrder(1549)]
public class TransferrableObjectManager : MonoBehaviour
{
	// Token: 0x06001996 RID: 6550 RVA: 0x0007E442 File Offset: 0x0007C642
	protected void Awake()
	{
		if (TransferrableObjectManager.hasInstance && TransferrableObjectManager.instance != this)
		{
			Object.Destroy(this);
			return;
		}
		TransferrableObjectManager.SetInstance(this);
	}

	// Token: 0x06001997 RID: 6551 RVA: 0x0007E465 File Offset: 0x0007C665
	protected void OnDestroy()
	{
		if (TransferrableObjectManager.instance == this)
		{
			TransferrableObjectManager.hasInstance = false;
			TransferrableObjectManager.instance = null;
		}
	}

	// Token: 0x06001998 RID: 6552 RVA: 0x0007E480 File Offset: 0x0007C680
	protected void LateUpdate()
	{
		for (int i = 0; i < TransferrableObjectManager.transObs.Count; i++)
		{
			TransferrableObjectManager.transObs[i].TriggeredLateUpdate();
		}
	}

	// Token: 0x06001999 RID: 6553 RVA: 0x0007E4B2 File Offset: 0x0007C6B2
	public static void CreateManager()
	{
		TransferrableObjectManager.SetInstance(new GameObject("TransferrableObjectManager").AddComponent<TransferrableObjectManager>());
	}

	// Token: 0x0600199A RID: 6554 RVA: 0x0007E4C8 File Offset: 0x0007C6C8
	private static void SetInstance(TransferrableObjectManager manager)
	{
		TransferrableObjectManager.instance = manager;
		TransferrableObjectManager.hasInstance = true;
		if (Application.isPlaying)
		{
			Object.DontDestroyOnLoad(manager);
		}
	}

	// Token: 0x0600199B RID: 6555 RVA: 0x0007E4E3 File Offset: 0x0007C6E3
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

	// Token: 0x0600199C RID: 6556 RVA: 0x0007E509 File Offset: 0x0007C709
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

	// Token: 0x04001C82 RID: 7298
	public static TransferrableObjectManager instance;

	// Token: 0x04001C83 RID: 7299
	public static bool hasInstance = false;

	// Token: 0x04001C84 RID: 7300
	public static readonly List<TransferrableObject> transObs = new List<TransferrableObject>(1024);
}
