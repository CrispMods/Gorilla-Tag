using System;
using UnityEngine;
using UnityEngine.Rendering;

// Token: 0x020008CF RID: 2255
public class GTMeshData
{
	// Token: 0x06003680 RID: 13952 RVA: 0x0010187C File Offset: 0x000FFA7C
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

	// Token: 0x06003681 RID: 13953 RVA: 0x0010194C File Offset: 0x000FFB4C
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

	// Token: 0x06003682 RID: 13954 RVA: 0x00101B1A File Offset: 0x000FFD1A
	public static GTMeshData Parse(Mesh mesh)
	{
		if (mesh == null)
		{
			throw new ArgumentNullException("mesh");
		}
		return new GTMeshData(mesh);
	}

	// Token: 0x040038BF RID: 14527
	public Mesh mesh;

	// Token: 0x040038C0 RID: 14528
	public Vector3[] vertices;

	// Token: 0x040038C1 RID: 14529
	public Vector3[] normals;

	// Token: 0x040038C2 RID: 14530
	public Vector4[] tangents;

	// Token: 0x040038C3 RID: 14531
	public Color32[] colors32;

	// Token: 0x040038C4 RID: 14532
	public int[] triangles;

	// Token: 0x040038C5 RID: 14533
	public BoneWeight[] boneWeights;

	// Token: 0x040038C6 RID: 14534
	public Vector2[] uv;

	// Token: 0x040038C7 RID: 14535
	public Vector2[] uv2;

	// Token: 0x040038C8 RID: 14536
	public Vector2[] uv3;

	// Token: 0x040038C9 RID: 14537
	public Vector2[] uv4;

	// Token: 0x040038CA RID: 14538
	public Vector2[] uv5;

	// Token: 0x040038CB RID: 14539
	public Vector2[] uv6;

	// Token: 0x040038CC RID: 14540
	public Vector2[] uv7;

	// Token: 0x040038CD RID: 14541
	public Vector2[] uv8;

	// Token: 0x040038CE RID: 14542
	public int subMeshCount;
}
