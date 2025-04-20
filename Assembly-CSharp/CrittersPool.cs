using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200005F RID: 95
public class CrittersPool : MonoBehaviour
{
	// Token: 0x06000264 RID: 612 RVA: 0x00031D90 File Offset: 0x0002FF90
	public static GameObject GetPooled(GameObject prefab)
	{
		CrittersPool crittersPool = CrittersPool.instance;
		if (crittersPool == null)
		{
			return null;
		}
		return crittersPool.GetInstance(prefab);
	}

	// Token: 0x06000265 RID: 613 RVA: 0x00031DA3 File Offset: 0x0002FFA3
	public static void Return(GameObject pooledGO)
	{
		CrittersPool crittersPool = CrittersPool.instance;
		if (crittersPool == null)
		{
			return;
		}
		crittersPool.ReturnInstance(pooledGO);
	}

	// Token: 0x06000266 RID: 614 RVA: 0x00031DB5 File Offset: 0x0002FFB5
	private void Awake()
	{
		if (CrittersPool.instance != null)
		{
			UnityEngine.Object.Destroy(this);
			return;
		}
		CrittersPool.instance = this;
		this.SetupPools();
	}

	// Token: 0x06000267 RID: 615 RVA: 0x00073D14 File Offset: 0x00071F14
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
					GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(crittersPoolSettings.poolObject);
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

	// Token: 0x06000268 RID: 616 RVA: 0x00073E10 File Offset: 0x00072010
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

	// Token: 0x06000269 RID: 617 RVA: 0x00031DD7 File Offset: 0x0002FFD7
	private void ReturnInstance(GameObject instance)
	{
		instance.transform.SetParent(this.poolParent);
		instance.SetActive(false);
	}

	// Token: 0x040002F1 RID: 753
	private static CrittersPool instance;

	// Token: 0x040002F2 RID: 754
	public CrittersPool.CrittersPoolSettings[] eventEffects;

	// Token: 0x040002F3 RID: 755
	private Dictionary<GameObject, List<GameObject>> pools;

	// Token: 0x040002F4 RID: 756
	public Transform poolParent;

	// Token: 0x02000060 RID: 96
	[Serializable]
	public class CrittersPoolSettings
	{
		// Token: 0x040002F5 RID: 757
		public GameObject poolObject;

		// Token: 0x040002F6 RID: 758
		public int poolSize = 20;
	}
}
