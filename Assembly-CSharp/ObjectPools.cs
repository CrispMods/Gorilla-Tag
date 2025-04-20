using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200087B RID: 2171
public class ObjectPools : MonoBehaviour
{
	// Token: 0x17000570 RID: 1392
	// (get) Token: 0x060034D3 RID: 13523 RVA: 0x00052D0B File Offset: 0x00050F0B
	// (set) Token: 0x060034D4 RID: 13524 RVA: 0x00052D13 File Offset: 0x00050F13
	public bool initialized { get; private set; }

	// Token: 0x060034D5 RID: 13525 RVA: 0x00052D1C File Offset: 0x00050F1C
	protected void Awake()
	{
		ObjectPools.instance = this;
	}

	// Token: 0x060034D6 RID: 13526 RVA: 0x00052D24 File Offset: 0x00050F24
	protected void Start()
	{
		this.InitializePools();
	}

	// Token: 0x060034D7 RID: 13527 RVA: 0x0013F6BC File Offset: 0x0013D8BC
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

	// Token: 0x060034D8 RID: 13528 RVA: 0x00052D2C File Offset: 0x00050F2C
	public bool DoesPoolExist(GameObject obj)
	{
		return this.DoesPoolExist(PoolUtils.GameObjHashCode(obj));
	}

	// Token: 0x060034D9 RID: 13529 RVA: 0x00052D3A File Offset: 0x00050F3A
	public bool DoesPoolExist(int hash)
	{
		return this.lookUp.ContainsKey(hash);
	}

	// Token: 0x060034DA RID: 13530 RVA: 0x00052D48 File Offset: 0x00050F48
	public SinglePool GetPoolByHash(int hash)
	{
		return this.lookUp[hash];
	}

	// Token: 0x060034DB RID: 13531 RVA: 0x0013F7F0 File Offset: 0x0013D9F0
	public SinglePool GetPoolByObjectType(GameObject obj)
	{
		int hash = PoolUtils.GameObjHashCode(obj);
		return this.GetPoolByHash(hash);
	}

	// Token: 0x060034DC RID: 13532 RVA: 0x00052D56 File Offset: 0x00050F56
	public GameObject Instantiate(GameObject obj)
	{
		return this.GetPoolByObjectType(obj).Instantiate(true);
	}

	// Token: 0x060034DD RID: 13533 RVA: 0x00052D65 File Offset: 0x00050F65
	public GameObject Instantiate(int hash)
	{
		return this.GetPoolByHash(hash).Instantiate(true);
	}

	// Token: 0x060034DE RID: 13534 RVA: 0x00052D74 File Offset: 0x00050F74
	public GameObject Instantiate(GameObject obj, Vector3 position)
	{
		GameObject gameObject = this.Instantiate(obj);
		gameObject.transform.position = position;
		return gameObject;
	}

	// Token: 0x060034DF RID: 13535 RVA: 0x00052D89 File Offset: 0x00050F89
	public GameObject Instantiate(int hash, Vector3 position)
	{
		GameObject gameObject = this.Instantiate(hash);
		gameObject.transform.position = position;
		return gameObject;
	}

	// Token: 0x060034E0 RID: 13536 RVA: 0x00052D9E File Offset: 0x00050F9E
	public GameObject Instantiate(GameObject obj, Vector3 position, Quaternion rotation)
	{
		GameObject gameObject = this.Instantiate(obj);
		gameObject.transform.SetPositionAndRotation(position, rotation);
		return gameObject;
	}

	// Token: 0x060034E1 RID: 13537 RVA: 0x00052DB4 File Offset: 0x00050FB4
	public GameObject Instantiate(GameObject obj, Vector3 position, Quaternion rotation, float scale)
	{
		GameObject gameObject = this.Instantiate(obj);
		gameObject.transform.SetPositionAndRotation(position, rotation);
		gameObject.transform.localScale = Vector3.one * scale;
		return gameObject;
	}

	// Token: 0x060034E2 RID: 13538 RVA: 0x00052DE1 File Offset: 0x00050FE1
	public void Destroy(GameObject obj)
	{
		this.GetPoolByObjectType(obj).Destroy(obj);
	}

	// Token: 0x040037D5 RID: 14293
	public static ObjectPools instance;

	// Token: 0x040037D7 RID: 14295
	[SerializeField]
	private List<SinglePool> pools;

	// Token: 0x040037D8 RID: 14296
	private Dictionary<int, SinglePool> lookUp;
}
