using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000861 RID: 2145
[Serializable]
public class SinglePool
{
	// Token: 0x0600340A RID: 13322 RVA: 0x00139F50 File Offset: 0x00138150
	private void PrivAllocPooledObjects()
	{
		int count = this.inactivePool.Count;
		for (int i = count; i < count + this.initAmountToPool; i++)
		{
			GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.objectToPool, this.gameObject.transform, true);
			gameObject.name = this.objectToPool.name + "(PoolIndex=" + i.ToString() + ")";
			gameObject.SetActive(false);
			this.inactivePool.Push(gameObject);
			int instanceID = gameObject.GetInstanceID();
			this.pooledObjects.Add(instanceID);
		}
	}

	// Token: 0x0600340B RID: 13323 RVA: 0x0005177E File Offset: 0x0004F97E
	public void Initialize(GameObject gameObject_)
	{
		this.gameObject = gameObject_;
		this.activePool = new Dictionary<int, GameObject>(this.initAmountToPool);
		this.inactivePool = new Stack<GameObject>(this.initAmountToPool);
		this.pooledObjects = new HashSet<int>();
		this.PrivAllocPooledObjects();
	}

	// Token: 0x0600340C RID: 13324 RVA: 0x00139FE4 File Offset: 0x001381E4
	public GameObject Instantiate(bool setActive = true)
	{
		if (this.inactivePool.Count == 0)
		{
			Debug.LogWarning("Pool '" + this.objectToPool.name + "'is expanding consider changing initial pool size");
			this.PrivAllocPooledObjects();
		}
		GameObject gameObject = this.inactivePool.Pop();
		int instanceID = gameObject.GetInstanceID();
		gameObject.SetActive(setActive);
		this.activePool.Add(instanceID, gameObject);
		return gameObject;
	}

	// Token: 0x0600340D RID: 13325 RVA: 0x0013A04C File Offset: 0x0013824C
	public void Destroy(GameObject obj)
	{
		int instanceID = obj.GetInstanceID();
		if (!this.activePool.ContainsKey(instanceID))
		{
			Debug.Log("Failed to destroy Object " + obj.name + " in pool, It is not contained in the activePool");
			return;
		}
		if (!this.pooledObjects.Contains(instanceID))
		{
			Debug.Log("Failed to destroy Object " + obj.name + " in pool, It is not contained in the pooledObjects");
			return;
		}
		obj.SetActive(false);
		this.inactivePool.Push(obj);
		this.activePool.Remove(instanceID);
	}

	// Token: 0x0600340E RID: 13326 RVA: 0x000517BA File Offset: 0x0004F9BA
	public int PoolGUID()
	{
		return PoolUtils.GameObjHashCode(this.objectToPool);
	}

	// Token: 0x0600340F RID: 13327 RVA: 0x000517C7 File Offset: 0x0004F9C7
	public int GetTotalCount()
	{
		return this.pooledObjects.Count;
	}

	// Token: 0x06003410 RID: 13328 RVA: 0x000517D4 File Offset: 0x0004F9D4
	public int GetActiveCount()
	{
		return this.activePool.Count;
	}

	// Token: 0x06003411 RID: 13329 RVA: 0x000517E1 File Offset: 0x0004F9E1
	public int GetInactiveCount()
	{
		return this.inactivePool.Count;
	}

	// Token: 0x04003721 RID: 14113
	public GameObject objectToPool;

	// Token: 0x04003722 RID: 14114
	public int initAmountToPool = 32;

	// Token: 0x04003723 RID: 14115
	private HashSet<int> pooledObjects;

	// Token: 0x04003724 RID: 14116
	private Stack<GameObject> inactivePool;

	// Token: 0x04003725 RID: 14117
	private Dictionary<int, GameObject> activePool;

	// Token: 0x04003726 RID: 14118
	private GameObject gameObject;
}
