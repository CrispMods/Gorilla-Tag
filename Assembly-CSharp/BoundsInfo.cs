using System;
using MathGeoLib;
using UnityEngine;

// Token: 0x02000909 RID: 2313
[Serializable]
public struct BoundsInfo
{
	// Token: 0x170005A7 RID: 1447
	// (get) Token: 0x060037A1 RID: 14241 RVA: 0x00054C05 File Offset: 0x00052E05
	public Vector3 sizeComputed
	{
		get
		{
			return Vector3.Scale(this.size, this.scale) * this.inflate;
		}
	}

	// Token: 0x170005A8 RID: 1448
	// (get) Token: 0x060037A2 RID: 14242 RVA: 0x00054C23 File Offset: 0x00052E23
	public Vector3 sizeComputedAA
	{
		get
		{
			return Vector3.Scale(this.sizeAA, this.scaleAA) * this.inflateAA;
		}
	}

	// Token: 0x060037A3 RID: 14243 RVA: 0x00148FE4 File Offset: 0x001471E4
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

	// Token: 0x060037A4 RID: 14244 RVA: 0x001490E8 File Offset: 0x001472E8
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

	// Token: 0x060037A5 RID: 14245 RVA: 0x00149194 File Offset: 0x00147394
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

	// Token: 0x04003A9C RID: 15004
	public Vector3 center;

	// Token: 0x04003A9D RID: 15005
	public Vector3 size;

	// Token: 0x04003A9E RID: 15006
	public Quaternion rotation;

	// Token: 0x04003A9F RID: 15007
	public Vector3 scale;

	// Token: 0x04003AA0 RID: 15008
	public float inflate;

	// Token: 0x04003AA1 RID: 15009
	[Space]
	public Vector3 centerAA;

	// Token: 0x04003AA2 RID: 15010
	public Vector3 sizeAA;

	// Token: 0x04003AA3 RID: 15011
	public Vector3 scaleAA;

	// Token: 0x04003AA4 RID: 15012
	public float inflateAA;
}
