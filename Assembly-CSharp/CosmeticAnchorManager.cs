using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020003E4 RID: 996
public class CosmeticAnchorManager : MonoBehaviour, IGorillaSliceableSimple
{
	// Token: 0x0600181D RID: 6173 RVA: 0x00040593 File Offset: 0x0003E793
	protected void Awake()
	{
		if (CosmeticAnchorManager.hasInstance && CosmeticAnchorManager.instance != this)
		{
			UnityEngine.Object.Destroy(this);
			return;
		}
		CosmeticAnchorManager.SetInstance(this);
	}

	// Token: 0x0600181E RID: 6174 RVA: 0x000405B6 File Offset: 0x0003E7B6
	public static void CreateManager()
	{
		CosmeticAnchorManager.SetInstance(new GameObject("CosmeticAnchorManager").AddComponent<CosmeticAnchorManager>());
	}

	// Token: 0x0600181F RID: 6175 RVA: 0x000405CC File Offset: 0x0003E7CC
	private static void SetInstance(CosmeticAnchorManager manager)
	{
		CosmeticAnchorManager.instance = manager;
		CosmeticAnchorManager.hasInstance = true;
		if (Application.isPlaying)
		{
			UnityEngine.Object.DontDestroyOnLoad(manager);
		}
	}

	// Token: 0x06001820 RID: 6176 RVA: 0x000405E7 File Offset: 0x0003E7E7
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

	// Token: 0x06001821 RID: 6177 RVA: 0x0004061D File Offset: 0x0003E81D
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

	// Token: 0x06001822 RID: 6178 RVA: 0x00032C89 File Offset: 0x00030E89
	public void OnEnable()
	{
		GorillaSlicerSimpleManager.RegisterSliceable(this, GorillaSlicerSimpleManager.UpdateStep.Update);
	}

	// Token: 0x06001823 RID: 6179 RVA: 0x00032C92 File Offset: 0x00030E92
	public void OnDisable()
	{
		GorillaSlicerSimpleManager.UnregisterSliceable(this, GorillaSlicerSimpleManager.UpdateStep.Update);
	}

	// Token: 0x06001824 RID: 6180 RVA: 0x000CAA10 File Offset: 0x000C8C10
	public void SliceUpdate()
	{
		for (int i = 0; i < CosmeticAnchorManager.allAnchors.Count; i++)
		{
			CosmeticAnchorManager.allAnchors[i].TryUpdate();
		}
	}

	// Token: 0x06001827 RID: 6183 RVA: 0x00032105 File Offset: 0x00030305
	bool IGorillaSliceableSimple.get_isActiveAndEnabled()
	{
		return base.isActiveAndEnabled;
	}

	// Token: 0x04001AB2 RID: 6834
	public static CosmeticAnchorManager instance;

	// Token: 0x04001AB3 RID: 6835
	public static bool hasInstance = false;

	// Token: 0x04001AB4 RID: 6836
	public static List<CosmeticAnchors> allAnchors = new List<CosmeticAnchors>();
}
