using System;
using Cysharp.Text;
using UnityEngine;

// Token: 0x020008F5 RID: 2293
[Serializable]
public struct ZoneNode : IEquatable<ZoneNode>
{
	// Token: 0x170005A3 RID: 1443
	// (get) Token: 0x06003718 RID: 14104 RVA: 0x00105083 File Offset: 0x00103283
	public static ZoneNode Null { get; } = new ZoneNode
	{
		zoneId = GTZone.none,
		subZoneId = GTSubZone.none,
		isValid = false
	};

	// Token: 0x170005A4 RID: 1444
	// (get) Token: 0x06003719 RID: 14105 RVA: 0x0010508A File Offset: 0x0010328A
	public int zoneKey
	{
		get
		{
			return StaticHash.Compute((int)this.zoneId, (int)this.subZoneId);
		}
	}

	// Token: 0x0600371A RID: 14106 RVA: 0x0010509D File Offset: 0x0010329D
	public bool ContainsPoint(Vector3 point)
	{
		return MathUtils.OrientedBoxContains(point, this.center, this.size, this.orientation);
	}

	// Token: 0x0600371B RID: 14107 RVA: 0x001050B7 File Offset: 0x001032B7
	public int SphereOverlap(Vector3 position, float radius)
	{
		return MathUtils.OrientedBoxSphereOverlap(position, radius, this.center, this.size, this.orientation);
	}

	// Token: 0x0600371C RID: 14108 RVA: 0x001050D2 File Offset: 0x001032D2
	public override string ToString()
	{
		if (this.subZoneId != GTSubZone.none)
		{
			return ZString.Concat<GTZone, string, GTSubZone>(this.zoneId, ".", this.subZoneId);
		}
		return ZString.Concat<GTZone>(this.zoneId);
	}

	// Token: 0x0600371D RID: 14109 RVA: 0x00105100 File Offset: 0x00103300
	public override int GetHashCode()
	{
		int zoneKey = this.zoneKey;
		int hashCode = this.center.QuantizedId128().GetHashCode();
		int hashCode2 = this.size.QuantizedId128().GetHashCode();
		int hashCode3 = this.orientation.QuantizedId128().GetHashCode();
		return StaticHash.Compute(zoneKey, hashCode, hashCode2, hashCode3);
	}

	// Token: 0x0600371E RID: 14110 RVA: 0x00105169 File Offset: 0x00103369
	public static bool operator ==(ZoneNode x, ZoneNode y)
	{
		return x.Equals(y);
	}

	// Token: 0x0600371F RID: 14111 RVA: 0x00105173 File Offset: 0x00103373
	public static bool operator !=(ZoneNode x, ZoneNode y)
	{
		return !x.Equals(y);
	}

	// Token: 0x06003720 RID: 14112 RVA: 0x00105180 File Offset: 0x00103380
	public bool Equals(ZoneNode other)
	{
		return this.zoneId == other.zoneId && this.subZoneId == other.subZoneId && this.center.Approx(other.center, 1E-05f) && this.size.Approx(other.size, 1E-05f) && this.orientation.Approx(other.orientation, 1E-06f);
	}

	// Token: 0x06003721 RID: 14113 RVA: 0x001051F4 File Offset: 0x001033F4
	public override bool Equals(object obj)
	{
		if (obj is ZoneNode)
		{
			ZoneNode other = (ZoneNode)obj;
			return this.Equals(other);
		}
		return false;
	}

	// Token: 0x04003A23 RID: 14883
	public GTZone zoneId;

	// Token: 0x04003A24 RID: 14884
	public GTSubZone subZoneId;

	// Token: 0x04003A25 RID: 14885
	public Vector3 center;

	// Token: 0x04003A26 RID: 14886
	public Vector3 size;

	// Token: 0x04003A27 RID: 14887
	public Quaternion orientation;

	// Token: 0x04003A28 RID: 14888
	public Bounds AABB;

	// Token: 0x04003A29 RID: 14889
	public bool isValid;
}
