using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200085E RID: 2142
[Serializable]
public class SinglePool
{
	// Token: 0x060033FE RID: 13310 RVA: 0x000F81C8 File Offset: 0x000F63C8
	private void PrivAllocPooledObjects()
	{
		int count = this.inactivePool.Count;
		for (int i = count; i < count + this.initAmountToPool; i++)
		{
			GameObject gameObject = Object.Instantiate<GameObject>(this.objectToPool, this.gameObject.transform, true);
			gameObject.name = this.objectToPool.name + "(PoolIndex=" + i.ToString() + ")";
			gameObject.SetActive(false);
			this.inactivePool.Push(gameObject);
			int instanceID = gameObject.GetInstanceID();
			this.pooledObjects.Add(instanceID);
		}
	}

	// Token: 0x060033FF RID: 13311 RVA: 0x000F825A File Offset: 0x000F645A
	public void Initialize(GameObject gameObject_)
	{
		this.gameObject = gameObject_;
		this.activePool = new Dictionary<int, GameObject>(this.initAmountToPool);
		this.inactivePool = new Stack<GameObject>(this.initAmountToPool);
		this.pooledObjects = new HashSet<int>();
		this.PrivAllocPooledObjects();
	}

	// Token: 0x06003400 RID: 13312 RVA: 0x000F8298 File Offset: 0x000F6498
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

	// Token: 0x06003401 RID: 13313 RVA: 0x000F8300 File Offset: 0x000F6500
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

	// Token: 0x06003402 RID: 13314 RVA: 0x000F8386 File Offset: 0x000F6586
	public int PoolGUID()
	{
		return PoolUtils.GameObjHashCode(this.objectToPool);
	}

	// Token: 0x06003403 RID: 13315 RVA: 0x000F8393 File Offset: 0x000F6593
	public int GetTotalCount()
	{
		return this.pooledObjects.Count;
	}

	// Token: 0x06003404 RID: 13316 RVA: 0x000F83A0 File Offset: 0x000F65A0
	public int GetActiveCount()
	{
		return this.activePool.Count;
	}

	// Token: 0x06003405 RID: 13317 RVA: 0x000F83AD File Offset: 0x000F65AD
	public int GetInactiveCount()
	{
		return this.inactivePool.Count;
	}

	// Token: 0x0400370F RID: 14095
	public GameObject objectToPool;

	// Token: 0x04003710 RID: 14096
	public int initAmountToPool = 32;

	// Token: 0x04003711 RID: 14097
	private HashSet<int> pooledObjects;

	// Token: 0x04003712 RID: 14098
	private Stack<GameObject> inactivePool;

	// Token: 0x04003713 RID: 14099
	private Dictionary<int, GameObject> activePool;

	// Token: 0x04003714 RID: 14100
	private GameObject gameObject;
}
