using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020003D9 RID: 985
public class CosmeticAnchorManager : MonoBehaviour, IGorillaSliceableSimple
{
	// Token: 0x060017D0 RID: 6096 RVA: 0x00073F08 File Offset: 0x00072108
	protected void Awake()
	{
		if (CosmeticAnchorManager.hasInstance && CosmeticAnchorManager.instance != this)
		{
			Object.Destroy(this);
			return;
		}
		CosmeticAnchorManager.SetInstance(this);
	}

	// Token: 0x060017D1 RID: 6097 RVA: 0x00073F2B File Offset: 0x0007212B
	public static void CreateManager()
	{
		CosmeticAnchorManager.SetInstance(new GameObject("CosmeticAnchorManager").AddComponent<CosmeticAnchorManager>());
	}

	// Token: 0x060017D2 RID: 6098 RVA: 0x00073F41 File Offset: 0x00072141
	private static void SetInstance(CosmeticAnchorManager manager)
	{
		CosmeticAnchorManager.instance = manager;
		CosmeticAnchorManager.hasInstance = true;
		if (Application.isPlaying)
		{
			Object.DontDestroyOnLoad(manager);
		}
	}

	// Token: 0x060017D3 RID: 6099 RVA: 0x00073F5C File Offset: 0x0007215C
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

	// Token: 0x060017D4 RID: 6100 RVA: 0x00073F92 File Offset: 0x00072192
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

	// Token: 0x060017D5 RID: 6101 RVA: 0x000158F9 File Offset: 0x00013AF9
	public void OnEnable()
	{
		GorillaSlicerSimpleManager.RegisterSliceable(this, GorillaSlicerSimpleManager.UpdateStep.Update);
	}

	// Token: 0x060017D6 RID: 6102 RVA: 0x00015902 File Offset: 0x00013B02
	public void OnDisable()
	{
		GorillaSlicerSimpleManager.UnregisterSliceable(this, GorillaSlicerSimpleManager.UpdateStep.Update);
	}

	// Token: 0x060017D7 RID: 6103 RVA: 0x00073FCC File Offset: 0x000721CC
	public void SliceUpdate()
	{
		for (int i = 0; i < CosmeticAnchorManager.allAnchors.Count; i++)
		{
			CosmeticAnchorManager.allAnchors[i].TryUpdate();
		}
	}

	// Token: 0x060017DA RID: 6106 RVA: 0x0000F974 File Offset: 0x0000DB74
	bool IGorillaSliceableSimple.get_isActiveAndEnabled()
	{
		return base.isActiveAndEnabled;
	}

	// Token: 0x04001A69 RID: 6761
	public static CosmeticAnchorManager instance;

	// Token: 0x04001A6A RID: 6762
	public static bool hasInstance = false;

	// Token: 0x04001A6B RID: 6763
	public static List<CosmeticAnchors> allAnchors = new List<CosmeticAnchors>();
}
