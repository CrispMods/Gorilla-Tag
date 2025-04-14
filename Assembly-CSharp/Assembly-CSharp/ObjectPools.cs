using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000862 RID: 2146
public class ObjectPools : MonoBehaviour
{
	// Token: 0x17000560 RID: 1376
	// (get) Token: 0x06003413 RID: 13331 RVA: 0x000F8992 File Offset: 0x000F6B92
	// (set) Token: 0x06003414 RID: 13332 RVA: 0x000F899A File Offset: 0x000F6B9A
	public bool initialized { get; private set; }

	// Token: 0x06003415 RID: 13333 RVA: 0x000F89A3 File Offset: 0x000F6BA3
	protected void Awake()
	{
		ObjectPools.instance = this;
	}

	// Token: 0x06003416 RID: 13334 RVA: 0x000F89AB File Offset: 0x000F6BAB
	protected void Start()
	{
		this.InitializePools();
	}

	// Token: 0x06003417 RID: 13335 RVA: 0x000F89B4 File Offset: 0x000F6BB4
	public void InitializePools()
	{
		if (this.initialized)
		{
			return;
		}
		this.lookUp = new Dictionary<int, SinglePool>();
		foreach (SinglePool singlePool in this.pools)
		{
			singlePool.Initialize(base.gameObject);
			int num = singlePool.PoolGUID();
			if (this.lookUp.ContainsKey(num))
			{
				using (List<SinglePool>.Enumerator enumerator2 = this.pools.GetEnumerator())
				{
					while (enumerator2.MoveNext())
					{
						SinglePool singlePool2 = enumerator2.Current;
						if (singlePool2.PoolGUID() == num)
						{
							Debug.LogError("Pools contain more then one instance of the same object\n" + string.Format("First object in question is {0} tag: {1}\n", singlePool2.objectToPool, singlePool2.objectToPool.tag) + string.Format("Second object is {0} tag: {1}", singlePool.objectToPool, singlePool.objectToPool.tag));
							break;
						}
					}
					continue;
				}
			}
			this.lookUp.Add(singlePool.PoolGUID(), singlePool);
		}
		this.initialized = true;
	}

	// Token: 0x06003418 RID: 13336 RVA: 0x000F8AE8 File Offset: 0x000F6CE8
	public bool DoesPoolExist(GameObject obj)
	{
		return this.DoesPoolExist(PoolUtils.GameObjHashCode(obj));
	}

	// Token: 0x06003419 RID: 13337 RVA: 0x000F8AF6 File Offset: 0x000F6CF6
	public bool DoesPoolExist(int hash)
	{
		return this.lookUp.ContainsKey(hash);
	}

	// Token: 0x0600341A RID: 13338 RVA: 0x000F8B04 File Offset: 0x000F6D04
	public SinglePool GetPoolByHash(int hash)
	{
		return this.lookUp[hash];
	}

	// Token: 0x0600341B RID: 13339 RVA: 0x000F8B14 File Offset: 0x000F6D14
	public SinglePool GetPoolByObjectType(GameObject obj)
	{
		int hash = PoolUtils.GameObjHashCode(obj);
		return this.GetPoolByHash(hash);
	}

	// Token: 0x0600341C RID: 13340 RVA: 0x000F8B2F File Offset: 0x000F6D2F
	public GameObject Instantiate(GameObject obj)
	{
		return this.GetPoolByObjectType(obj).Instantiate(true);
	}

	// Token: 0x0600341D RID: 13341 RVA: 0x000F8B3E File Offset: 0x000F6D3E
	public GameObject Instantiate(int hash)
	{
		return this.GetPoolByHash(hash).Instantiate(true);
	}

	// Token: 0x0600341E RID: 13342 RVA: 0x000F8B4D File Offset: 0x000F6D4D
	public GameObject Instantiate(GameObject obj, Vector3 position)
	{
		GameObject gameObject = this.Instantiate(obj);
		gameObject.transform.position = position;
		return gameObject;
	}

	// Token: 0x0600341F RID: 13343 RVA: 0x000F8B62 File Offset: 0x000F6D62
	public GameObject Instantiate(int hash, Vector3 position)
	{
		GameObject gameObject = this.Instantiate(hash);
		gameObject.transform.position = position;
		return gameObject;
	}

	// Token: 0x06003420 RID: 13344 RVA: 0x000F8B77 File Offset: 0x000F6D77
	public GameObject Instantiate(GameObject obj, Vector3 position, Quaternion rotation)
	{
		GameObject gameObject = this.Instantiate(obj);
		gameObject.transform.SetPositionAndRotation(position, rotation);
		return gameObject;
	}

	// Token: 0x06003421 RID: 13345 RVA: 0x000F8B8D File Offset: 0x000F6D8D
	public GameObject Instantiate(GameObject obj, Vector3 position, Quaternion rotation, float scale)
	{
		GameObject gameObject = this.Instantiate(obj);
		gameObject.transform.SetPositionAndRotation(position, rotation);
		gameObject.transform.localScale = Vector3.one * scale;
		return gameObject;
	}

	// Token: 0x06003422 RID: 13346 RVA: 0x000F8BBA File Offset: 0x000F6DBA
	public void Destroy(GameObject obj)
	{
		this.GetPoolByObjectType(obj).Destroy(obj);
	}

	// Token: 0x04003727 RID: 14119
	public static ObjectPools instance;

	// Token: 0x04003729 RID: 14121
	[SerializeField]
	private List<SinglePool> pools;

	// Token: 0x0400372A RID: 14122
	private Dictionary<int, SinglePool> lookUp;
}
