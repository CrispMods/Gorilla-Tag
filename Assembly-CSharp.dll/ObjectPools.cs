using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000862 RID: 2146
public class ObjectPools : MonoBehaviour
{
	// Token: 0x17000560 RID: 1376
	// (get) Token: 0x06003413 RID: 13331 RVA: 0x000517FE File Offset: 0x0004F9FE
	// (set) Token: 0x06003414 RID: 13332 RVA: 0x00051806 File Offset: 0x0004FA06
	public bool initialized { get; private set; }

	// Token: 0x06003415 RID: 13333 RVA: 0x0005180F File Offset: 0x0004FA0F
	protected void Awake()
	{
		ObjectPools.instance = this;
	}

	// Token: 0x06003416 RID: 13334 RVA: 0x00051817 File Offset: 0x0004FA17
	protected void Start()
	{
		this.InitializePools();
	}

	// Token: 0x06003417 RID: 13335 RVA: 0x0013A0D4 File Offset: 0x001382D4
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

	// Token: 0x06003418 RID: 13336 RVA: 0x0005181F File Offset: 0x0004FA1F
	public bool DoesPoolExist(GameObject obj)
	{
		return this.DoesPoolExist(PoolUtils.GameObjHashCode(obj));
	}

	// Token: 0x06003419 RID: 13337 RVA: 0x0005182D File Offset: 0x0004FA2D
	public bool DoesPoolExist(int hash)
	{
		return this.lookUp.ContainsKey(hash);
	}

	// Token: 0x0600341A RID: 13338 RVA: 0x0005183B File Offset: 0x0004FA3B
	public SinglePool GetPoolByHash(int hash)
	{
		return this.lookUp[hash];
	}

	// Token: 0x0600341B RID: 13339 RVA: 0x0013A208 File Offset: 0x00138408
	public SinglePool GetPoolByObjectType(GameObject obj)
	{
		int hash = PoolUtils.GameObjHashCode(obj);
		return this.GetPoolByHash(hash);
	}

	// Token: 0x0600341C RID: 13340 RVA: 0x00051849 File Offset: 0x0004FA49
	public GameObject Instantiate(GameObject obj)
	{
		return this.GetPoolByObjectType(obj).Instantiate(true);
	}

	// Token: 0x0600341D RID: 13341 RVA: 0x00051858 File Offset: 0x0004FA58
	public GameObject Instantiate(int hash)
	{
		return this.GetPoolByHash(hash).Instantiate(true);
	}

	// Token: 0x0600341E RID: 13342 RVA: 0x00051867 File Offset: 0x0004FA67
	public GameObject Instantiate(GameObject obj, Vector3 position)
	{
		GameObject gameObject = this.Instantiate(obj);
		gameObject.transform.position = position;
		return gameObject;
	}

	// Token: 0x0600341F RID: 13343 RVA: 0x0005187C File Offset: 0x0004FA7C
	public GameObject Instantiate(int hash, Vector3 position)
	{
		GameObject gameObject = this.Instantiate(hash);
		gameObject.transform.position = position;
		return gameObject;
	}

	// Token: 0x06003420 RID: 13344 RVA: 0x00051891 File Offset: 0x0004FA91
	public GameObject Instantiate(GameObject obj, Vector3 position, Quaternion rotation)
	{
		GameObject gameObject = this.Instantiate(obj);
		gameObject.transform.SetPositionAndRotation(position, rotation);
		return gameObject;
	}

	// Token: 0x06003421 RID: 13345 RVA: 0x000518A7 File Offset: 0x0004FAA7
	public GameObject Instantiate(GameObject obj, Vector3 position, Quaternion rotation, float scale)
	{
		GameObject gameObject = this.Instantiate(obj);
		gameObject.transform.SetPositionAndRotation(position, rotation);
		gameObject.transform.localScale = Vector3.one * scale;
		return gameObject;
	}

	// Token: 0x06003422 RID: 13346 RVA: 0x000518D4 File Offset: 0x0004FAD4
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
