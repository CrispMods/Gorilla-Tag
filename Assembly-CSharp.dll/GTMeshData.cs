using System;
using UnityEngine;
using UnityEngine.Rendering;

// Token: 0x020008D2 RID: 2258
public class GTMeshData
{
	// Token: 0x0600368C RID: 13964 RVA: 0x0014179C File Offset: 0x0013F99C
	public GTMeshData(Mesh m)
	{
		this.mesh = m;
		this.subMeshCount = m.subMeshCount;
		this.vertices = m.vertices;
		this.triangles = m.triangles;
		this.normals = m.normals;
		this.tangents = m.tangents;
		this.colors32 = m.colors32;
		this.boneWeights = m.boneWeights;
		this.uv = m.uv;
		this.uv2 = m.uv2;
		this.uv3 = m.uv3;
		this.uv4 = m.uv4;
		this.uv5 = m.uv5;
		this.uv6 = m.uv6;
		this.uv7 = m.uv7;
		this.uv8 = m.uv8;
	}

	// Token: 0x0600368D RID: 13965 RVA: 0x0014186C File Offset: 0x0013FA6C
	public Mesh ExtractSubmesh(int subMeshIndex, bool optimize = false)
	{
		if (subMeshIndex < 0 || subMeshIndex >= this.subMeshCount)
		{
			throw new IndexOutOfRangeException("subMeshIndex");
		}
		SubMeshDescriptor subMesh = this.mesh.GetSubMesh(subMeshIndex);
		int firstVertex = subMesh.firstVertex;
		int vertexCount = subMesh.vertexCount;
		MeshTopology topology = subMesh.topology;
		int[] indices = this.mesh.GetIndices(subMeshIndex, false);
		for (int i = 0; i < indices.Length; i++)
		{
			indices[i] -= firstVertex;
		}
		Mesh mesh = new Mesh();
		mesh.indexFormat = ((vertexCount > 65535) ? IndexFormat.UInt32 : IndexFormat.UInt16);
		mesh.SetVertices(this.vertices, firstVertex, vertexCount);
		mesh.SetIndices(indices, topology, 0);
		mesh.SetNormals(this.normals, firstVertex, vertexCount);
		mesh.SetTangents(this.tangents, firstVertex, vertexCount);
		if (!this.uv.IsNullOrEmpty<Vector2>())
		{
			mesh.SetUVs(0, this.uv, firstVertex, vertexCount);
		}
		if (!this.uv2.IsNullOrEmpty<Vector2>())
		{
			mesh.SetUVs(1, this.uv2, firstVertex, vertexCount);
		}
		if (!this.uv3.IsNullOrEmpty<Vector2>())
		{
			mesh.SetUVs(2, this.uv3, firstVertex, vertexCount);
		}
		if (!this.uv4.IsNullOrEmpty<Vector2>())
		{
			mesh.SetUVs(3, this.uv4, firstVertex, vertexCount);
		}
		if (!this.uv5.IsNullOrEmpty<Vector2>())
		{
			mesh.SetUVs(4, this.uv5, firstVertex, vertexCount);
		}
		if (!this.uv6.IsNullOrEmpty<Vector2>())
		{
			mesh.SetUVs(5, this.uv6, firstVertex, vertexCount);
		}
		if (!this.uv7.IsNullOrEmpty<Vector2>())
		{
			mesh.SetUVs(6, this.uv7, firstVertex, vertexCount);
		}
		if (!this.uv8.IsNullOrEmpty<Vector2>())
		{
			mesh.SetUVs(7, this.uv8, firstVertex, vertexCount);
		}
		if (optimize)
		{
			mesh.Optimize();
			mesh.OptimizeIndexBuffers();
		}
		mesh.RecalculateBounds();
		return mesh;
	}

	// Token: 0x0600368E RID: 13966 RVA: 0x00053319 File Offset: 0x00051519
	public static GTMeshData Parse(Mesh mesh)
	{
		if (mesh == null)
		{
			throw new ArgumentNullException("mesh");
		}
		return new GTMeshData(mesh);
	}

	// Token: 0x040038D1 RID: 14545
	public Mesh mesh;

	// Token: 0x040038D2 RID: 14546
	public Vector3[] vertices;

	// Token: 0x040038D3 RID: 14547
	public Vector3[] normals;

	// Token: 0x040038D4 RID: 14548
	public Vector4[] tangents;

	// Token: 0x040038D5 RID: 14549
	public Color32[] colors32;

	// Token: 0x040038D6 RID: 14550
	public int[] triangles;

	// Token: 0x040038D7 RID: 14551
	public BoneWeight[] boneWeights;

	// Token: 0x040038D8 RID: 14552
	public Vector2[] uv;

	// Token: 0x040038D9 RID: 14553
	public Vector2[] uv2;

	// Token: 0x040038DA RID: 14554
	public Vector2[] uv3;

	// Token: 0x040038DB RID: 14555
	public Vector2[] uv4;

	// Token: 0x040038DC RID: 14556
	public Vector2[] uv5;

	// Token: 0x040038DD RID: 14557
	public Vector2[] uv6;

	// Token: 0x040038DE RID: 14558
	public Vector2[] uv7;

	// Token: 0x040038DF RID: 14559
	public Vector2[] uv8;

	// Token: 0x040038E0 RID: 14560
	public int subMeshCount;
}
