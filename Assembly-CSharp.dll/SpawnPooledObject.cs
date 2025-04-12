using System;
using GorillaLocomotion;
using UnityEngine;
using UnityEngine.Serialization;

// Token: 0x02000639 RID: 1593
public class SpawnPooledObject : MonoBehaviour
{
	// Token: 0x06002798 RID: 10136 RVA: 0x0004A139 File Offset: 0x00048339
	private void Awake()
	{
		if (this._pooledObject == null)
		{
			return;
		}
		this._pooledObjectHash = PoolUtils.GameObjHashCode(this._pooledObject);
	}

	// Token: 0x06002799 RID: 10137 RVA: 0x0010AD10 File Offset: 0x00108F10
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

	// Token: 0x0600279A RID: 10138 RVA: 0x0004A15B File Offset: 0x0004835B
	private Vector3 SpawnLocation()
	{
		return this._spawnLocation.transform.position + this.offset;
	}

	// Token: 0x0600279B RID: 10139 RVA: 0x0010AD8C File Offset: 0x00108F8C
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

	// Token: 0x0600279C RID: 10140 RVA: 0x0004A178 File Offset: 0x00048378
	private bool ShouldSpawn()
	{
		return UnityEngine.Random.Range(0, 100) < this.chanceToSpawn;
	}

	// Token: 0x04002B53 RID: 11091
	[SerializeField]
	private Transform _spawnLocation;

	// Token: 0x04002B54 RID: 11092
	[SerializeField]
	private GameObject _pooledObject;

	// Token: 0x04002B55 RID: 11093
	[FormerlySerializedAs("_offset")]
	public Vector3 offset;

	// Token: 0x04002B56 RID: 11094
	[FormerlySerializedAs("_upright")]
	public bool upright;

	// Token: 0x04002B57 RID: 11095
	[FormerlySerializedAs("_facePlayer")]
	public bool facePlayer;

	// Token: 0x04002B58 RID: 11096
	[FormerlySerializedAs("_chanceToSpawn")]
	[Range(0f, 100f)]
	public int chanceToSpawn = 100;

	// Token: 0x04002B59 RID: 11097
	private int _pooledObjectHash;
}
