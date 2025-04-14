using System;
using System.Collections.Generic;
using GorillaTagScripts;
using Photon.Pun;
using UnityEngine;

// Token: 0x020004F6 RID: 1270
public class BuilderShelf : MonoBehaviour
{
	// Token: 0x06001ED6 RID: 7894 RVA: 0x0009C388 File Offset: 0x0009A588
	public void Init()
	{
		this.shelfSlot = 0;
		this.buildPieceSpawnIndex = 0;
		this.spawnCount = 0;
		this.count = 0;
		this.spawnCosts = new List<BuilderResources>(this.buildPieceSpawns.Count);
		for (int i = 0; i < this.buildPieceSpawns.Count; i++)
		{
			this.count += this.buildPieceSpawns[i].count;
			BuilderPiece component = this.buildPieceSpawns[i].buildPiecePrefab.GetComponent<BuilderPiece>();
			this.spawnCosts.Add(component.cost);
		}
	}

	// Token: 0x06001ED7 RID: 7895 RVA: 0x0009C423 File Offset: 0x0009A623
	public bool HasOpenSlot()
	{
		return this.shelfSlot < this.count;
	}

	// Token: 0x06001ED8 RID: 7896 RVA: 0x0009C434 File Offset: 0x0009A634
	public void BuildNextPiece(BuilderTable table)
	{
		if (!this.HasOpenSlot())
		{
			return;
		}
		BuilderShelf.BuildPieceSpawn buildPieceSpawn = this.buildPieceSpawns[this.buildPieceSpawnIndex];
		BuilderResources resources = this.spawnCosts[this.buildPieceSpawnIndex];
		while (!table.HasEnoughUnreservedResources(resources) && this.buildPieceSpawnIndex < this.buildPieceSpawns.Count - 1)
		{
			int num = buildPieceSpawn.count - this.spawnCount;
			this.shelfSlot += num;
			this.spawnCount = 0;
			this.buildPieceSpawnIndex++;
			buildPieceSpawn = this.buildPieceSpawns[this.buildPieceSpawnIndex];
			resources = this.spawnCosts[this.buildPieceSpawnIndex];
		}
		if (!table.HasEnoughUnreservedResources(resources))
		{
			int num2 = buildPieceSpawn.count - this.spawnCount;
			this.shelfSlot += num2;
			this.spawnCount = 0;
			return;
		}
		int staticHash = buildPieceSpawn.buildPiecePrefab.name.GetStaticHash();
		int materialType = string.IsNullOrEmpty(buildPieceSpawn.materialID) ? -1 : buildPieceSpawn.materialID.GetHashCode();
		Vector3 position;
		Quaternion rotation;
		this.GetSpawnLocation(this.shelfSlot, buildPieceSpawn, out position, out rotation);
		int pieceId = table.CreatePieceId();
		table.CreatePiece(staticHash, pieceId, position, rotation, materialType, BuilderPiece.State.OnShelf, PhotonNetwork.LocalPlayer);
		this.spawnCount++;
		this.shelfSlot++;
		if (this.spawnCount >= buildPieceSpawn.count)
		{
			this.buildPieceSpawnIndex++;
			this.spawnCount = 0;
		}
	}

	// Token: 0x06001ED9 RID: 7897 RVA: 0x0009C5B0 File Offset: 0x0009A7B0
	public void InitCount()
	{
		this.count = 0;
		for (int i = 0; i < this.buildPieceSpawns.Count; i++)
		{
			this.count += this.buildPieceSpawns[i].count;
		}
	}

	// Token: 0x06001EDA RID: 7898 RVA: 0x0009C5F8 File Offset: 0x0009A7F8
	public void BuildItems(BuilderTable table)
	{
		int num = 0;
		this.InitCount();
		for (int i = 0; i < this.buildPieceSpawns.Count; i++)
		{
			BuilderShelf.BuildPieceSpawn buildPieceSpawn = this.buildPieceSpawns[i];
			if (buildPieceSpawn != null && buildPieceSpawn.count != 0)
			{
				int staticHash = buildPieceSpawn.buildPiecePrefab.name.GetStaticHash();
				int materialType = string.IsNullOrEmpty(buildPieceSpawn.materialID) ? -1 : buildPieceSpawn.materialID.GetHashCode();
				int num2 = 0;
				while (num2 < buildPieceSpawn.count && num < this.count)
				{
					Vector3 position;
					Quaternion rotation;
					this.GetSpawnLocation(num, buildPieceSpawn, out position, out rotation);
					int pieceId = table.CreatePieceId();
					table.CreatePiece(staticHash, pieceId, position, rotation, materialType, BuilderPiece.State.OnShelf, PhotonNetwork.LocalPlayer);
					num++;
					num2++;
				}
			}
		}
	}

	// Token: 0x06001EDB RID: 7899 RVA: 0x0009C6C4 File Offset: 0x0009A8C4
	public void GetSpawnLocation(int slot, BuilderShelf.BuildPieceSpawn spawn, out Vector3 spawnPosition, out Quaternion spawnRotation)
	{
		if (this.center == null)
		{
			this.center = base.transform;
		}
		Vector3 b = spawn.positionOffset;
		Vector3 euler = spawn.rotationOffset;
		BuilderPiece component = spawn.buildPiecePrefab.GetComponent<BuilderPiece>();
		if (component != null)
		{
			b = component.desiredShelfOffset;
			euler = component.desiredShelfRotationOffset;
		}
		spawnRotation = this.center.rotation * Quaternion.Euler(euler);
		float d = (float)slot * this.separation - (float)(this.count - 1) * this.separation / 2f;
		spawnPosition = this.center.position + this.center.rotation * (spawn.localAxis * d + b);
	}

	// Token: 0x0400227E RID: 8830
	private int count;

	// Token: 0x0400227F RID: 8831
	public float separation;

	// Token: 0x04002280 RID: 8832
	public Transform center;

	// Token: 0x04002281 RID: 8833
	public Material overrideMaterial;

	// Token: 0x04002282 RID: 8834
	public List<BuilderShelf.BuildPieceSpawn> buildPieceSpawns;

	// Token: 0x04002283 RID: 8835
	private List<BuilderResources> spawnCosts;

	// Token: 0x04002284 RID: 8836
	private int shelfSlot;

	// Token: 0x04002285 RID: 8837
	private int buildPieceSpawnIndex;

	// Token: 0x04002286 RID: 8838
	private int spawnCount;

	// Token: 0x020004F7 RID: 1271
	[Serializable]
	public class BuildPieceSpawn
	{
		// Token: 0x04002287 RID: 8839
		public GameObject buildPiecePrefab;

		// Token: 0x04002288 RID: 8840
		public string materialID;

		// Token: 0x04002289 RID: 8841
		public int count = 1;

		// Token: 0x0400228A RID: 8842
		public Vector3 localAxis = Vector3.right;

		// Token: 0x0400228B RID: 8843
		[Tooltip("Use BuilderPiece:desiredShelfOffset instead")]
		public Vector3 positionOffset;

		// Token: 0x0400228C RID: 8844
		[Tooltip("Use BuilderPiece:desiredShelfRotationOffset instead")]
		public Vector3 rotationOffset;

		// Token: 0x0400228D RID: 8845
		[Tooltip("Optional Editor Visual")]
		public Mesh previewMesh;
	}
}
