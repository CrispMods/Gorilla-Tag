using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// Token: 0x020008EC RID: 2284
public class BoundsCalcs : MonoBehaviour
{
	// Token: 0x060036D6 RID: 14038 RVA: 0x00103D58 File Offset: 0x00101F58
	public void Compute()
	{
		MeshFilter[] array;
		if (this.useRootMeshOnly)
		{
			BoundsCalcs.singleMesh[0] = base.GetComponent<MeshFilter>();
			array = BoundsCalcs.singleMesh;
		}
		else if (this.optionalTargets != null && this.optionalTargets.Length != 0)
		{
			array = base.GetComponentsInChildren<MeshFilter>().Concat(this.optionalTargets).ToArray<MeshFilter>();
		}
		else
		{
			array = base.GetComponentsInChildren<MeshFilter>();
		}
		List<Mesh> list = new List<Mesh>((array.Length + 1) / 2);
		List<Vector3> list2 = new List<Vector3>(array.Length * 512);
		this.elements.Clear();
		for (int i = 0; i < array.Length; i++)
		{
			Matrix4x4 localToWorldMatrix = array[i].transform.localToWorldMatrix;
			Mesh mesh = array[i].sharedMesh;
			if (!mesh.isReadable)
			{
				Mesh mesh2 = mesh.CreateReadableMeshCopy();
				list.Add(mesh2);
				mesh = mesh2;
			}
			Vector3[] vertices = mesh.vertices;
			for (int j = 0; j < vertices.Length; j++)
			{
				vertices[j] = localToWorldMatrix.MultiplyPoint3x4(vertices[j]);
			}
			BoundsInfo item = BoundsInfo.ComputeBounds(vertices);
			this.elements.Add(item);
			list2.AddRange(vertices);
		}
		this.composite = BoundsInfo.ComputeBounds(list2.ToArray());
		list.ForEach(new Action<Mesh>(Object.DestroyImmediate));
	}

	// Token: 0x040039D5 RID: 14805
	public MeshFilter[] optionalTargets = new MeshFilter[0];

	// Token: 0x040039D6 RID: 14806
	public bool useRootMeshOnly;

	// Token: 0x040039D7 RID: 14807
	[Space]
	public List<BoundsInfo> elements = new List<BoundsInfo>();

	// Token: 0x040039D8 RID: 14808
	[Space]
	public BoundsInfo composite;

	// Token: 0x040039D9 RID: 14809
	[Space]
	private StateHash _state;

	// Token: 0x040039DA RID: 14810
	private static MeshFilter[] singleMesh = new MeshFilter[1];
}
