using System;
using MathGeoLib;
using UnityEngine;

// Token: 0x020008ED RID: 2285
[Serializable]
public struct BoundsInfo
{
	// Token: 0x17000596 RID: 1430
	// (get) Token: 0x060036D9 RID: 14041 RVA: 0x00103EC7 File Offset: 0x001020C7
	public Vector3 sizeComputed
	{
		get
		{
			return Vector3.Scale(this.size, this.scale) * this.inflate;
		}
	}

	// Token: 0x17000597 RID: 1431
	// (get) Token: 0x060036DA RID: 14042 RVA: 0x00103EE5 File Offset: 0x001020E5
	public Vector3 sizeComputedAA
	{
		get
		{
			return Vector3.Scale(this.sizeAA, this.scaleAA) * this.inflateAA;
		}
	}

	// Token: 0x060036DB RID: 14043 RVA: 0x00103F04 File Offset: 0x00102104
	public static BoundsInfo ComputeBounds(Vector3[] vertices)
	{
		if (vertices.Length == 0)
		{
			return default(BoundsInfo);
		}
		OrientedBoundingBox orientedBoundingBox = OrientedBoundingBox.BruteEnclosing(vertices);
		Vector4 column = orientedBoundingBox.Axis1;
		Vector4 column2 = orientedBoundingBox.Axis2;
		Vector4 column3 = orientedBoundingBox.Axis3;
		Vector4 column4 = new Vector4(0f, 0f, 0f, 1f);
		BoundsInfo result = default(BoundsInfo);
		result.center = orientedBoundingBox.Center;
		result.size = orientedBoundingBox.Extent * 2f;
		result.rotation = new Matrix4x4(column, column2, column3, column4).rotation;
		result.scale = Vector3.one;
		result.inflate = 1f;
		Bounds bounds = GeometryUtility.CalculateBounds(vertices, Matrix4x4.identity);
		result.centerAA = bounds.center;
		result.sizeAA = bounds.size;
		result.scaleAA = Vector3.one;
		result.inflateAA = 1f;
		return result;
	}

	// Token: 0x060036DC RID: 14044 RVA: 0x00104008 File Offset: 0x00102208
	public static BoxCollider CreateBoxCollider(BoundsInfo bounds)
	{
		int hashCode = bounds.center.QuantizedId128().GetHashCode();
		int hashCode2 = bounds.size.QuantizedId128().GetHashCode();
		int hashCode3 = bounds.rotation.QuantizedId128().GetHashCode();
		int num = StaticHash.Compute(hashCode, hashCode2, hashCode3);
		Transform transform = new GameObject(string.Format("BoxCollider_{0:X8}", num)).transform;
		transform.position = bounds.center;
		transform.rotation = bounds.rotation;
		BoxCollider boxCollider = transform.gameObject.AddComponent<BoxCollider>();
		boxCollider.size = bounds.sizeComputed;
		return boxCollider;
	}

	// Token: 0x060036DD RID: 14045 RVA: 0x001040B4 File Offset: 0x001022B4
	public static BoxCollider CreateBoxColliderAA(BoundsInfo bounds)
	{
		int hashCode = bounds.center.QuantizedId128().GetHashCode();
		int hashCode2 = bounds.size.QuantizedId128().GetHashCode();
		int num = StaticHash.Compute(hashCode, hashCode2);
		Transform transform = new GameObject(string.Format("BoxCollider_{0:X8}", num)).transform;
		transform.position = bounds.centerAA;
		BoxCollider boxCollider = transform.gameObject.AddComponent<BoxCollider>();
		boxCollider.size = bounds.sizeComputedAA;
		return boxCollider;
	}

	// Token: 0x040039DB RID: 14811
	public Vector3 center;

	// Token: 0x040039DC RID: 14812
	public Vector3 size;

	// Token: 0x040039DD RID: 14813
	public Quaternion rotation;

	// Token: 0x040039DE RID: 14814
	public Vector3 scale;

	// Token: 0x040039DF RID: 14815
	public float inflate;

	// Token: 0x040039E0 RID: 14816
	[Space]
	public Vector3 centerAA;

	// Token: 0x040039E1 RID: 14817
	public Vector3 sizeAA;

	// Token: 0x040039E2 RID: 14818
	public Vector3 scaleAA;

	// Token: 0x040039E3 RID: 14819
	public float inflateAA;
}
