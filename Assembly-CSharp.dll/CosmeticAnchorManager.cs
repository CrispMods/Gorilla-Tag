using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020003D9 RID: 985
public class CosmeticAnchorManager : MonoBehaviour, IGorillaSliceableSimple
{
	// Token: 0x060017D3 RID: 6099 RVA: 0x0003F2A9 File Offset: 0x0003D4A9
	protected void Awake()
	{
		if (CosmeticAnchorManager.hasInstance && CosmeticAnchorManager.instance != this)
		{
			UnityEngine.Object.Destroy(this);
			return;
		}
		CosmeticAnchorManager.SetInstance(this);
	}

	// Token: 0x060017D4 RID: 6100 RVA: 0x0003F2CC File Offset: 0x0003D4CC
	public static void CreateManager()
	{
		CosmeticAnchorManager.SetInstance(new GameObject("CosmeticAnchorManager").AddComponent<CosmeticAnchorManager>());
	}

	// Token: 0x060017D5 RID: 6101 RVA: 0x0003F2E2 File Offset: 0x0003D4E2
	private static void SetInstance(CosmeticAnchorManager manager)
	{
		CosmeticAnchorManager.instance = manager;
		CosmeticAnchorManager.hasInstance = true;
		if (Application.isPlaying)
		{
			UnityEngine.Object.DontDestroyOnLoad(manager);
		}
	}

	// Token: 0x060017D6 RID: 6102 RVA: 0x0003F2FD File Offset: 0x0003D4FD
	public static void RegisterCosmeticAnchor(CosmeticAnchors cA)
	{
		if (!CosmeticAnchorManager.hasInstance)
		{
			CosmeticAnchorManager.CreateManager();
		}
		if ((cA.AffectedByHunt() || cA.AffectedByBuilder()) && !CosmeticAnchorManager.allAnchors.Contains(cA))
		{
			CosmeticAnchorManager.allAnchors.Add(cA);
		}
	}

	// Token: 0x060017D7 RID: 6103 RVA: 0x0003F333 File Offset: 0x0003D533
	public static void UnregisterCosmeticAnchor(CosmeticAnchors cA)
	{
		if (!CosmeticAnchorManager.hasInstance)
		{
			CosmeticAnchorManager.CreateManager();
		}
		if ((cA.AffectedByHunt() || cA.AffectedByBuilder()) && CosmeticAnchorManager.allAnchors.Contains(cA))
		{
			CosmeticAnchorManager.allAnchors.Remove(cA);
		}
	}

	// Token: 0x060017D8 RID: 6104 RVA: 0x00031B26 File Offset: 0x0002FD26
	public void OnEnable()
	{
		GorillaSlicerSimpleManager.RegisterSliceable(this, GorillaSlicerSimpleManager.UpdateStep.Update);
	}

	// Token: 0x060017D9 RID: 6105 RVA: 0x00031B2F File Offset: 0x0002FD2F
	public void OnDisable()
	{
		GorillaSlicerSimpleManager.UnregisterSliceable(this, GorillaSlicerSimpleManager.UpdateStep.Update);
	}

	// Token: 0x060017DA RID: 6106 RVA: 0x000C81E8 File Offset: 0x000C63E8
	public void SliceUpdate()
	{
		for (int i = 0; i < CosmeticAnchorManager.allAnchors.Count; i++)
		{
			CosmeticAnchorManager.allAnchors[i].TryUpdate();
		}
	}

	// Token: 0x060017DD RID: 6109 RVA: 0x00030F9B File Offset: 0x0002F19B
	bool IGorillaSliceableSimple.get_isActiveAndEnabled()
	{
		return base.isActiveAndEnabled;
	}

	// Token: 0x04001A6A RID: 6762
	public static CosmeticAnchorManager instance;

	// Token: 0x04001A6B RID: 6763
	public static bool hasInstance = false;

	// Token: 0x04001A6C RID: 6764
	public static List<CosmeticAnchors> allAnchors = new List<CosmeticAnchors>();
}
