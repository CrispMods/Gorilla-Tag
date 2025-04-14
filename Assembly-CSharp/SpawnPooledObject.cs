using System;
using GorillaLocomotion;
using UnityEngine;
using UnityEngine.Serialization;

// Token: 0x02000638 RID: 1592
public class SpawnPooledObject : MonoBehaviour
{
	// Token: 0x06002790 RID: 10128 RVA: 0x000C191F File Offset: 0x000BFB1F
	private void Awake()
	{
		if (this._pooledObject == null)
		{
			return;
		}
		this._pooledObjectHash = PoolUtils.GameObjHashCode(this._pooledObject);
	}

	// Token: 0x06002791 RID: 10129 RVA: 0x000C1944 File Offset: 0x000BFB44
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

	// Token: 0x06002792 RID: 10130 RVA: 0x000C19BE File Offset: 0x000BFBBE
	private Vector3 SpawnLocation()
	{
		return this._spawnLocation.transform.position + this.offset;
	}

	// Token: 0x06002793 RID: 10131 RVA: 0x000C19DC File Offset: 0x000BFBDC
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

	// Token: 0x06002794 RID: 10132 RVA: 0x000C1A5C File Offset: 0x000BFC5C
	private bool ShouldSpawn()
	{
		return Random.Range(0, 100) < this.chanceToSpawn;
	}

	// Token: 0x04002B4D RID: 11085
	[SerializeField]
	private Transform _spawnLocation;

	// Token: 0x04002B4E RID: 11086
	[SerializeField]
	private GameObject _pooledObject;

	// Token: 0x04002B4F RID: 11087
	[FormerlySerializedAs("_offset")]
	public Vector3 offset;

	// Token: 0x04002B50 RID: 11088
	[FormerlySerializedAs("_upright")]
	public bool upright;

	// Token: 0x04002B51 RID: 11089
	[FormerlySerializedAs("_facePlayer")]
	public bool facePlayer;

	// Token: 0x04002B52 RID: 11090
	[FormerlySerializedAs("_chanceToSpawn")]
	[Range(0f, 100f)]
	public int chanceToSpawn = 100;

	// Token: 0x04002B53 RID: 11091
	private int _pooledObjectHash;
}
