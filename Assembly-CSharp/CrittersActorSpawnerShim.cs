using System;
using Critters.Scripts;
using UnityEngine;

// Token: 0x0200003B RID: 59
public class CrittersActorSpawnerShim : MonoBehaviour
{
	// Token: 0x0600012F RID: 303 RVA: 0x0006D7A8 File Offset: 0x0006B9A8
	[ContextMenu("Copy Spawner Data To Shim")]
	private CrittersActorSpawner CopySpawnerDataInPrefab()
	{
		CrittersActorSpawner component = base.gameObject.GetComponent<CrittersActorSpawner>();
		this.spawnerPointTransform = component.spawnPoint.transform;
		this.actorType = component.actorType;
		this.subActorIndex = component.subActorIndex;
		this.insideSpawnerBounds = (BoxCollider)component.insideSpawnerCheck;
		this.spawnDelay = component.spawnDelay;
		this.applyImpulseOnSpawn = component.applyImpulseOnSpawn;
		this.attachSpawnedObjectToSpawnLocation = component.attachSpawnedObjectToSpawnLocation;
		this.colliderTrigger = base.gameObject.GetComponent<BoxCollider>();
		return component;
	}

	// Token: 0x06000130 RID: 304 RVA: 0x0006D834 File Offset: 0x0006BA34
	[ContextMenu("Replace Spawner With Shim")]
	private void ReplaceSpawnerWithShim()
	{
		CrittersActorSpawner crittersActorSpawner = this.CopySpawnerDataInPrefab();
		if (crittersActorSpawner.spawnPoint.GetComponent<Rigidbody>() != null)
		{
			UnityEngine.Object.DestroyImmediate(crittersActorSpawner.spawnPoint.GetComponent<Rigidbody>());
		}
		UnityEngine.Object.DestroyImmediate(crittersActorSpawner.spawnPoint);
		UnityEngine.Object.DestroyImmediate(crittersActorSpawner);
	}

	// Token: 0x0400015C RID: 348
	public Transform spawnerPointTransform;

	// Token: 0x0400015D RID: 349
	public CrittersActor.CrittersActorType actorType;

	// Token: 0x0400015E RID: 350
	public int subActorIndex;

	// Token: 0x0400015F RID: 351
	public BoxCollider insideSpawnerBounds;

	// Token: 0x04000160 RID: 352
	public int spawnDelay;

	// Token: 0x04000161 RID: 353
	public bool applyImpulseOnSpawn;

	// Token: 0x04000162 RID: 354
	public bool attachSpawnedObjectToSpawnLocation;

	// Token: 0x04000163 RID: 355
	public BoxCollider colliderTrigger;
}
