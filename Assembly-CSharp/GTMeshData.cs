using System;
using UnityEngine;
using UnityEngine.Rendering;

// Token: 0x020008EB RID: 2283
public class GTMeshData
{
	// Token: 0x06003748 RID: 14152 RVA: 0x00146D5C File Offset: 0x00144F5C
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

	// Token: 0x06003749 RID: 14153 RVA: 0x00146E2C File Offset: 0x0014502C
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

	// Token: 0x0600374A RID: 14154 RVA: 0x00054836 File Offset: 0x00052A36
	public static GTMeshData Parse(Mesh mesh)
	{
		if (mesh == null)
		{
			throw new ArgumentNullException("mesh");
		}
		return new GTMeshData(mesh);
	}

	// Token: 0x04003980 RID: 14720
	public Mesh mesh;

	// Token: 0x04003981 RID: 14721
	public Vector3[] vertices;

	// Token: 0x04003982 RID: 14722
	public Vector3[] normals;

	// Token: 0x04003983 RID: 14723
	public Vector4[] tangents;

	// Token: 0x04003984 RID: 14724
	public Color32[] colors32;

	// Token: 0x04003985 RID: 14725
	public int[] triangles;

	// Token: 0x04003986 RID: 14726
	public BoneWeight[] boneWeights;

	// Token: 0x04003987 RID: 14727
	public Vector2[] uv;

	// Token: 0x04003988 RID: 14728
	public Vector2[] uv2;

	// Token: 0x04003989 RID: 14729
	public Vector2[] uv3;

	// Token: 0x0400398A RID: 14730
	public Vector2[] uv4;

	// Token: 0x0400398B RID: 14731
	public Vector2[] uv5;

	// Token: 0x0400398C RID: 14732
	public Vector2[] uv6;

	// Token: 0x0400398D RID: 14733
	public Vector2[] uv7;

	// Token: 0x0400398E RID: 14734
	public Vector2[] uv8;

	// Token: 0x0400398F RID: 14735
	public int subMeshCount;
}
