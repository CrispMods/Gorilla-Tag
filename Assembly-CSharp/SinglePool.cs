using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200087A RID: 2170
[Serializable]
public class SinglePool
{
	// Token: 0x060034CA RID: 13514 RVA: 0x0013F538 File Offset: 0x0013D738
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

	// Token: 0x060034CB RID: 13515 RVA: 0x00052C8B File Offset: 0x00050E8B
	public void Initialize(GameObject gameObject_)
	{
		this.gameObject = gameObject_;
		this.activePool = new Dictionary<int, GameObject>(this.initAmountToPool);
		this.inactivePool = new Stack<GameObject>(this.initAmountToPool);
		this.pooledObjects = new HashSet<int>();
		this.PrivAllocPooledObjects();
	}

	// Token: 0x060034CC RID: 13516 RVA: 0x0013F5CC File Offset: 0x0013D7CC
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

	// Token: 0x060034CD RID: 13517 RVA: 0x0013F634 File Offset: 0x0013D834
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

	// Token: 0x060034CE RID: 13518 RVA: 0x00052CC7 File Offset: 0x00050EC7
	public int PoolGUID()
	{
		return PoolUtils.GameObjHashCode(this.objectToPool);
	}

	// Token: 0x060034CF RID: 13519 RVA: 0x00052CD4 File Offset: 0x00050ED4
	public int GetTotalCount()
	{
		return this.pooledObjects.Count;
	}

	// Token: 0x060034D0 RID: 13520 RVA: 0x00052CE1 File Offset: 0x00050EE1
	public int GetActiveCount()
	{
		return this.activePool.Count;
	}

	// Token: 0x060034D1 RID: 13521 RVA: 0x00052CEE File Offset: 0x00050EEE
	public int GetInactiveCount()
	{
		return this.inactivePool.Count;
	}

	// Token: 0x040037CF RID: 14287
	public GameObject objectToPool;

	// Token: 0x040037D0 RID: 14288
	public int initAmountToPool = 32;

	// Token: 0x040037D1 RID: 14289
	private HashSet<int> pooledObjects;

	// Token: 0x040037D2 RID: 14290
	private Stack<GameObject> inactivePool;

	// Token: 0x040037D3 RID: 14291
	private Dictionary<int, GameObject> activePool;

	// Token: 0x040037D4 RID: 14292
	private GameObject gameObject;
}
