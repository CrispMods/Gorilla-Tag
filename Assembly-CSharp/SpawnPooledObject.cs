using System;
using GorillaLocomotion;
using UnityEngine;
using UnityEngine.Serialization;

// Token: 0x02000617 RID: 1559
public class SpawnPooledObject : MonoBehaviour
{
	// Token: 0x060026BB RID: 9915 RVA: 0x0004A6CE File Offset: 0x000488CE
	private void Awake()
	{
		if (this._pooledObject == null)
		{
			return;
		}
		this._pooledObjectHash = PoolUtils.GameObjHashCode(this._pooledObject);
	}

	// Token: 0x060026BC RID: 9916 RVA: 0x00109138 File Offset: 0x00107338
	public void SpawnObject()
	{
		if (!this.ShouldSpawn())
		{
			return;
		}
		if (this._pooledObject == null || this._spawnLocation == null)
		{
			return;
		}
		GameObject gameObject = ObjectPools.instance.Instantiate(this._pooledObjectHash);
		gameObject.transform.position = this.SpawnLocation();
		gameObject.transform.rotation = this.SpawnRotation();
		gameObject.transform.localScale = base.transform.lossyScale;
	}

	// Token: 0x060026BD RID: 9917 RVA: 0x0004A6F0 File Offset: 0x000488F0
	private Vector3 SpawnLocation()
	{
		return this._spawnLocation.transform.position + this.offset;
	}

	// Token: 0x060026BE RID: 9918 RVA: 0x001091B4 File Offset: 0x001073B4
	private Quaternion SpawnRotation()
	{
		Quaternion result = this._spawnLocation.transform.rotation;
		if (this.facePlayer)
		{
			result = Quaternion.LookRotation(GTPlayer.Instance.headCollider.transform.position - this._spawnLocation.transform.position);
		}
		if (this.upright)
		{
			result.eulerAngles = new Vector3(0f, result.eulerAngles.y, 0f);
		}
		return result;
	}

	// Token: 0x060026BF RID: 9919 RVA: 0x0004A70D File Offset: 0x0004890D
	private bool ShouldSpawn()
	{
		return UnityEngine.Random.Range(0, 100) < this.chanceToSpawn;
	}

	// Token: 0x04002AB3 RID: 10931
	[SerializeField]
	private Transform _spawnLocation;

	// Token: 0x04002AB4 RID: 10932
	[SerializeField]
	private GameObject _pooledObject;

	// Token: 0x04002AB5 RID: 10933
	[FormerlySerializedAs("_offset")]
	public Vector3 offset;

	// Token: 0x04002AB6 RID: 10934
	[FormerlySerializedAs("_upright")]
	public bool upright;

	// Token: 0x04002AB7 RID: 10935
	[FormerlySerializedAs("_facePlayer")]
	public bool facePlayer;

	// Token: 0x04002AB8 RID: 10936
	[FormerlySerializedAs("_chanceToSpawn")]
	[Range(0f, 100f)]
	public int chanceToSpawn = 100;

	// Token: 0x04002AB9 RID: 10937
	private int _pooledObjectHash;
}
