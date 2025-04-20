using System;
using System.Collections.Generic;
using GorillaTagScripts;
using Photon.Pun;
using UnityEngine;

// Token: 0x02000503 RID: 1283
public class BuilderShelf : MonoBehaviour
{
	// Token: 0x06001F2C RID: 7980 RVA: 0x000EE440 File Offset: 0x000EC640
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

	// Token: 0x06001F2D RID: 7981 RVA: 0x00045120 File Offset: 0x00043320
	public bool HasOpenSlot()
	{
		return this.shelfSlot < this.count;
	}

	// Token: 0x06001F2E RID: 7982 RVA: 0x000EE4DC File Offset: 0x000EC6DC
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

	// Token: 0x06001F2F RID: 7983 RVA: 0x000EE658 File Offset: 0x000EC858
	public void InitCount()
	{
		this.count = 0;
		for (int i = 0; i < this.buildPieceSpawns.Count; i++)
		{
			this.count += this.buildPieceSpawns[i].count;
		}
	}

	// Token: 0x06001F30 RID: 7984 RVA: 0x000EE6A0 File Offset: 0x000EC8A0
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

	// Token: 0x06001F31 RID: 7985 RVA: 0x000EE76C File Offset: 0x000EC96C
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

	// Token: 0x040022D0 RID: 8912
	private int count;

	// Token: 0x040022D1 RID: 8913
	public float separation;

	// Token: 0x040022D2 RID: 8914
	public Transform center;

	// Token: 0x040022D3 RID: 8915
	public Material overrideMaterial;

	// Token: 0x040022D4 RID: 8916
	public List<BuilderShelf.BuildPieceSpawn> buildPieceSpawns;

	// Token: 0x040022D5 RID: 8917
	private List<BuilderResources> spawnCosts;

	// Token: 0x040022D6 RID: 8918
	private int shelfSlot;

	// Token: 0x040022D7 RID: 8919
	private int buildPieceSpawnIndex;

	// Token: 0x040022D8 RID: 8920
	private int spawnCount;

	// Token: 0x02000504 RID: 1284
	[Serializable]
	public class BuildPieceSpawn
	{
		// Token: 0x040022D9 RID: 8921
		public GameObject buildPiecePrefab;

		// Token: 0x040022DA RID: 8922
		public string materialID;

		// Token: 0x040022DB RID: 8923
		public int count = 1;

		// Token: 0x040022DC RID: 8924
		public Vector3 localAxis = Vector3.right;

		// Token: 0x040022DD RID: 8925
		[Tooltip("Use BuilderPiece:desiredShelfOffset instead")]
		public Vector3 positionOffset;

		// Token: 0x040022DE RID: 8926
		[Tooltip("Use BuilderPiece:desiredShelfRotationOffset instead")]
		public Vector3 rotationOffset;

		// Token: 0x040022DF RID: 8927
		[Tooltip("Optional Editor Visual")]
		public Mesh previewMesh;
	}
}
