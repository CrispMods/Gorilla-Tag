using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000416 RID: 1046
[DefaultExecutionOrder(1549)]
public class TransferrableObjectManager : MonoBehaviour
{
	// Token: 0x060019E3 RID: 6627 RVA: 0x00041685 File Offset: 0x0003F885
	protected void Awake()
	{
		if (TransferrableObjectManager.hasInstance && TransferrableObjectManager.instance != this)
		{
			UnityEngine.Object.Destroy(this);
			return;
		}
		TransferrableObjectManager.SetInstance(this);
	}

	// Token: 0x060019E4 RID: 6628 RVA: 0x000416A8 File Offset: 0x0003F8A8
	protected void OnDestroy()
	{
		if (TransferrableObjectManager.instance == this)
		{
			TransferrableObjectManager.hasInstance = false;
			TransferrableObjectManager.instance = null;
		}
	}

	// Token: 0x060019E5 RID: 6629 RVA: 0x000D3DBC File Offset: 0x000D1FBC
	protected void LateUpdate()
	{
		for (int i = 0; i < TransferrableObjectManager.transObs.Count; i++)
		{
			TransferrableObjectManager.transObs[i].TriggeredLateUpdate();
		}
	}

	// Token: 0x060019E6 RID: 6630 RVA: 0x000416C3 File Offset: 0x0003F8C3
	public static void CreateManager()
	{
		TransferrableObjectManager.SetInstance(new GameObject("TransferrableObjectManager").AddComponent<TransferrableObjectManager>());
	}

	// Token: 0x060019E7 RID: 6631 RVA: 0x000416D9 File Offset: 0x0003F8D9
	private static void SetInstance(TransferrableObjectManager manager)
	{
		TransferrableObjectManager.instance = manager;
		TransferrableObjectManager.hasInstance = true;
		if (Application.isPlaying)
		{
			UnityEngine.Object.DontDestroyOnLoad(manager);
		}
	}

	// Token: 0x060019E8 RID: 6632 RVA: 0x000416F4 File Offset: 0x0003F8F4
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

	// Token: 0x060019E9 RID: 6633 RVA: 0x0004171A File Offset: 0x0003F91A
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

	// Token: 0x04001CCB RID: 7371
	public static TransferrableObjectManager instance;

	// Token: 0x04001CCC RID: 7372
	public static bool hasInstance = false;

	// Token: 0x04001CCD RID: 7373
	public static readonly List<TransferrableObject> transObs = new List<TransferrableObject>(1024);
}
