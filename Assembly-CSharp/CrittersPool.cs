using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000059 RID: 89
public class CrittersPool : MonoBehaviour
{
	// Token: 0x06000242 RID: 578 RVA: 0x0000EE04 File Offset: 0x0000D004
	public static GameObject GetPooled(GameObject prefab)
	{
		CrittersPool crittersPool = CrittersPool.instance;
		if (crittersPool == null)
		{
			return null;
		}
		return crittersPool.GetInstance(prefab);
	}

	// Token: 0x06000243 RID: 579 RVA: 0x0000EE17 File Offset: 0x0000D017
	public static void Return(GameObject pooledGO)
	{
		CrittersPool crittersPool = CrittersPool.instance;
		if (crittersPool == null)
		{
			return;
		}
		crittersPool.ReturnInstance(pooledGO);
	}

	// Token: 0x06000244 RID: 580 RVA: 0x0000EE29 File Offset: 0x0000D029
	private void Awake()
	{
		if (CrittersPool.instance != null)
		{
			Object.Destroy(this);
			return;
		}
		CrittersPool.instance = this;
		this.SetupPools();
	}

	// Token: 0x06000245 RID: 581 RVA: 0x0000EE4C File Offset: 0x0000D04C
	private void SetupPools()
	{
		this.pools = new Dictionary<GameObject, List<GameObject>>();
		this.poolParent = new GameObject("CrittersPool")
		{
			transform = 
			{
				parent = base.transform
			}
		}.transform;
		for (int i = 0; i < this.eventEffects.Length; i++)
		{
			CrittersPool.CrittersPoolSettings crittersPoolSettings = this.eventEffects[i];
			if (crittersPoolSettings.poolObject == null || crittersPoolSettings.poolSize <= 0)
			{
				GTDev.Log<string>("CrittersPool.SetupPools Failed. Pool has no poolObject or has size 0.", null);
			}
			else
			{
				List<GameObject> list = new List<GameObject>();
				for (int j = 0; j < crittersPoolSettings.poolSize; j++)
				{
					GameObject gameObject = Object.Instantiate<GameObject>(crittersPoolSettings.poolObject);
					gameObject.transform.SetParent(this.poolParent);
					GameObject gameObject2 = gameObject;
					gameObject2.name += j.ToString();
					gameObject.SetActive(false);
					list.Add(gameObject);
				}
				this.pools.Add(crittersPoolSettings.poolObject, list);
			}
		}
	}

	// Token: 0x06000246 RID: 582 RVA: 0x0000EF48 File Offset: 0x0000D148
	private GameObject GetInstance(GameObject prefab)
	{
		List<GameObject> list;
		if (this.pools.TryGetValue(prefab, out list))
		{
			for (int i = 0; i < list.Count; i++)
			{
				if (list[i] != null && !list[i].activeSelf)
				{
					list[i].SetActive(true);
					return list[i];
				}
			}
			GTDev.Log<string>("CrittersPool.GetInstance Failed. No available instance.", null);
			return null;
		}
		GTDev.LogError<string>("CrittersPool.GetInstance Failed. Prefab doesn't have a valid pool setup.", null);
		return null;
	}

	// Token: 0x06000247 RID: 583 RVA: 0x0000EFC1 File Offset: 0x0000D1C1
	private void ReturnInstance(GameObject instance)
	{
		instance.transform.SetParent(this.poolParent);
		instance.SetActive(false);
	}

	// Token: 0x040002C5 RID: 709
	private static CrittersPool instance;

	// Token: 0x040002C6 RID: 710
	public CrittersPool.CrittersPoolSettings[] eventEffects;

	// Token: 0x040002C7 RID: 711
	private Dictionary<GameObject, List<GameObject>> pools;

	// Token: 0x040002C8 RID: 712
	public Transform poolParent;

	// Token: 0x0200005A RID: 90
	[Serializable]
	public class CrittersPoolSettings
	{
		// Token: 0x040002C9 RID: 713
		public GameObject poolObject;

		// Token: 0x040002CA RID: 714
		public int poolSize = 20;
	}
}
