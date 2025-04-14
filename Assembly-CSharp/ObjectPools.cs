using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200085F RID: 2143
public class ObjectPools : MonoBehaviour
{
	// Token: 0x1700055F RID: 1375
	// (get) Token: 0x06003407 RID: 13319 RVA: 0x000F83CA File Offset: 0x000F65CA
	// (set) Token: 0x06003408 RID: 13320 RVA: 0x000F83D2 File Offset: 0x000F65D2
	public bool initialized { get; private set; }

	// Token: 0x06003409 RID: 13321 RVA: 0x000F83DB File Offset: 0x000F65DB
	protected void Awake()
	{
		ObjectPools.instance = this;
	}

	// Token: 0x0600340A RID: 13322 RVA: 0x000F83E3 File Offset: 0x000F65E3
	protected void Start()
	{
		this.InitializePools();
	}

	// Token: 0x0600340B RID: 13323 RVA: 0x000F83EC File Offset: 0x000F65EC
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

	// Token: 0x0600340C RID: 13324 RVA: 0x000F8520 File Offset: 0x000F6720
	public bool DoesPoolExist(GameObject obj)
	{
		return this.DoesPoolExist(PoolUtils.GameObjHashCode(obj));
	}

	// Token: 0x0600340D RID: 13325 RVA: 0x000F852E File Offset: 0x000F672E
	public bool DoesPoolExist(int hash)
	{
		return this.lookUp.ContainsKey(hash);
	}

	// Token: 0x0600340E RID: 13326 RVA: 0x000F853C File Offset: 0x000F673C
	public SinglePool GetPoolByHash(int hash)
	{
		return this.lookUp[hash];
	}

	// Token: 0x0600340F RID: 13327 RVA: 0x000F854C File Offset: 0x000F674C
	public SinglePool GetPoolByObjectType(GameObject obj)
	{
		int hash = PoolUtils.GameObjHashCode(obj);
		return this.GetPoolByHash(hash);
	}

	// Token: 0x06003410 RID: 13328 RVA: 0x000F8567 File Offset: 0x000F6767
	public GameObject Instantiate(GameObject obj)
	{
		return this.GetPoolByObjectType(obj).Instantiate(true);
	}

	// Token: 0x06003411 RID: 13329 RVA: 0x000F8576 File Offset: 0x000F6776
	public GameObject Instantiate(int hash)
	{
		return this.GetPoolByHash(hash).Instantiate(true);
	}

	// Token: 0x06003412 RID: 13330 RVA: 0x000F8585 File Offset: 0x000F6785
	public GameObject Instantiate(GameObject obj, Vector3 position)
	{
		GameObject gameObject = this.Instantiate(obj);
		gameObject.transform.position = position;
		return gameObject;
	}

	// Token: 0x06003413 RID: 13331 RVA: 0x000F859A File Offset: 0x000F679A
	public GameObject Instantiate(int hash, Vector3 position)
	{
		GameObject gameObject = this.Instantiate(hash);
		gameObject.transform.position = position;
		return gameObject;
	}

	// Token: 0x06003414 RID: 13332 RVA: 0x000F85AF File Offset: 0x000F67AF
	public GameObject Instantiate(GameObject obj, Vector3 position, Quaternion rotation)
	{
		GameObject gameObject = this.Instantiate(obj);
		gameObject.transform.SetPositionAndRotation(position, rotation);
		return gameObject;
	}

	// Token: 0x06003415 RID: 13333 RVA: 0x000F85C5 File Offset: 0x000F67C5
	public GameObject Instantiate(GameObject obj, Vector3 position, Quaternion rotation, float scale)
	{
		GameObject gameObject = this.Instantiate(obj);
		gameObject.transform.SetPositionAndRotation(position, rotation);
		gameObject.transform.localScale = Vector3.one * scale;
		return gameObject;
	}

	// Token: 0x06003416 RID: 13334 RVA: 0x000F85F2 File Offset: 0x000F67F2
	public void Destroy(GameObject obj)
	{
		this.GetPoolByObjectType(obj).Destroy(obj);
	}

	// Token: 0x04003715 RID: 14101
	public static ObjectPools instance;

	// Token: 0x04003717 RID: 14103
	[SerializeField]
	private List<SinglePool> pools;

	// Token: 0x04003718 RID: 14104
	private Dictionary<int, SinglePool> lookUp;
}
