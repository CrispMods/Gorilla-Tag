using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;

// Token: 0x0200034E RID: 846
[RequireComponent(typeof(MeshFilter))]
public class SetMeshBarycentricCoordinates : MonoBehaviour
{
	// Token: 0x060013AD RID: 5037 RVA: 0x00060886 File Offset: 0x0005EA86
	private void Start()
	{
		this._meshFilter = base.GetComponent<MeshFilter>();
		base.StartCoroutine(this.CheckMeshData());
	}

	// Token: 0x060013AE RID: 5038 RVA: 0x000608A1 File Offset: 0x0005EAA1
	private IEnumerator CheckMeshData()
	{
		yield return null;
		if (this._meshFilter.mesh == null || this._mesh == this._meshFilter.mesh || this._meshFilter.mesh.vertexCount == 0)
		{
			yield return new WaitForSeconds(1f);
		}
		this.CreateBarycentricCoordinates();
		yield break;
	}

	// Token: 0x060013AF RID: 5039 RVA: 0x000608B0 File Offset: 0x0005EAB0
	private void CreateBarycentricCoordinates()
	{
		Mesh mesh = this._meshFilter.mesh;
		Vector3[] vertices = mesh.vertices;
		int[] triangles = mesh.GetTriangles(0);
		Color[] array = new Color[triangles.Length];
		Vector3[] array2 = new Vector3[triangles.Length];
		int[] array3 = new int[triangles.Length];
		for (int i = 0; i < triangles.Length; i++)
		{
			array[i] = new Color((i % 3 == 0) ? 1f : 0f, (i % 3 == 1) ? 1f : 0f, (i % 3 == 2) ? 1f : 0f);
			array2[i] = vertices[triangles[i]];
			array3[i] = i;
		}
		mesh.indexFormat = IndexFormat.UInt32;
		mesh.SetVertices(array2);
		mesh.SetColors(array);
		mesh.SetIndices(array3, MeshTopology.Triangles, 0, true, 0);
		this._mesh = mesh;
		this._meshFilter.mesh = this._mesh;
		this._meshFilter.mesh.RecalculateNormals();
	}

	// Token: 0x040015D7 RID: 5591
	private MeshFilter _meshFilter;

	// Token: 0x040015D8 RID: 5592
	private Mesh _mesh;
}
