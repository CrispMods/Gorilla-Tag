﻿using System;
using UnityEngine;
using UnityEngine.Rendering;

// Token: 0x0200085B RID: 2139
public static class MeshUtils
{
	// Token: 0x060033F7 RID: 13303 RVA: 0x000F7FDC File Offset: 0x000F61DC
	public static Mesh CreateReadableMeshCopy(this Mesh sourceMesh)
	{
		Mesh mesh = new Mesh();
		mesh.indexFormat = sourceMesh.indexFormat;
		GraphicsBuffer vertexBuffer = sourceMesh.GetVertexBuffer(0);
		int num = vertexBuffer.stride * vertexBuffer.count;
		byte[] data = new byte[num];
		vertexBuffer.GetData(data);
		mesh.SetVertexBufferParams(sourceMesh.vertexCount, sourceMesh.GetVertexAttributes());
		mesh.SetVertexBufferData<byte>(data, 0, 0, num, 0, MeshUpdateFlags.Default);
		vertexBuffer.Release();
		mesh.subMeshCount = sourceMesh.subMeshCount;
		GraphicsBuffer indexBuffer = sourceMesh.GetIndexBuffer();
		int num2 = indexBuffer.stride * indexBuffer.count;
		byte[] data2 = new byte[num2];
		indexBuffer.GetData(data2);
		mesh.SetIndexBufferParams(indexBuffer.count, sourceMesh.indexFormat);
		mesh.SetIndexBufferData<byte>(data2, 0, 0, num2, MeshUpdateFlags.Default);
		indexBuffer.Release();
		uint num3 = 0U;
		for (int i = 0; i < mesh.subMeshCount; i++)
		{
			uint indexCount = sourceMesh.GetIndexCount(i);
			mesh.SetSubMesh(i, new SubMeshDescriptor((int)num3, (int)indexCount, MeshTopology.Triangles), MeshUpdateFlags.Default);
			num3 += indexCount;
		}
		mesh.RecalculateNormals();
		mesh.RecalculateBounds();
		return mesh;
	}
}
