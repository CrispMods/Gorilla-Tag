using System;
using MathGeoLib;
using UnityEngine;

// Token: 0x020008F0 RID: 2288
[Serializable]
public struct BoundsInfo
{
	// Token: 0x17000597 RID: 1431
	// (get) Token: 0x060036E5 RID: 14053 RVA: 0x000536E8 File Offset: 0x000518E8
	public Vector3 sizeComputed
	{
		get
		{
			return Vector3.Scale(this.size, this.scale) * this.inflate;
		}
	}

	// Token: 0x17000598 RID: 1432
	// (get) Token: 0x060036E6 RID: 14054 RVA: 0x00053706 File Offset: 0x00051906
	public Vector3 sizeComputedAA
	{
		get
		{
			return Vector3.Scale(this.sizeAA, this.scaleAA) * this.inflateAA;
		}
	}

	// Token: 0x060036E7 RID: 14055 RVA: 0x00143A24 File Offset: 0x00141C24
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

	// Token: 0x060036E8 RID: 14056 RVA: 0x00143B28 File Offset: 0x00141D28
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

	// Token: 0x060036E9 RID: 14057 RVA: 0x00143BD4 File Offset: 0x00141DD4
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

	// Token: 0x040039ED RID: 14829
	public Vector3 center;

	// Token: 0x040039EE RID: 14830
	public Vector3 size;

	// Token: 0x040039EF RID: 14831
	public Quaternion rotation;

	// Token: 0x040039F0 RID: 14832
	public Vector3 scale;

	// Token: 0x040039F1 RID: 14833
	public float inflate;

	// Token: 0x040039F2 RID: 14834
	[Space]
	public Vector3 centerAA;

	// Token: 0x040039F3 RID: 14835
	public Vector3 sizeAA;

	// Token: 0x040039F4 RID: 14836
	public Vector3 scaleAA;

	// Token: 0x040039F5 RID: 14837
	public float inflateAA;
}
