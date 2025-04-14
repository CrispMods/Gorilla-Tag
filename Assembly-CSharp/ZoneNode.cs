using System;
using Cysharp.Text;
using UnityEngine;

// Token: 0x020008F2 RID: 2290
[Serializable]
public struct ZoneNode : IEquatable<ZoneNode>
{
	// Token: 0x170005A2 RID: 1442
	// (get) Token: 0x0600370C RID: 14092 RVA: 0x00104ABB File Offset: 0x00102CBB
	public static ZoneNode Null { get; } = new ZoneNode
	{
		zoneId = GTZone.none,
		subZoneId = GTSubZone.none,
		isValid = false
	};

	// Token: 0x170005A3 RID: 1443
	// (get) Token: 0x0600370D RID: 14093 RVA: 0x00104AC2 File Offset: 0x00102CC2
	public int zoneKey
	{
		get
		{
			return StaticHash.Compute((int)this.zoneId, (int)this.subZoneId);
		}
	}

	// Token: 0x0600370E RID: 14094 RVA: 0x00104AD5 File Offset: 0x00102CD5
	public bool ContainsPoint(Vector3 point)
	{
		return MathUtils.OrientedBoxContains(point, this.center, this.size, this.orientation);
	}

	// Token: 0x0600370F RID: 14095 RVA: 0x00104AEF File Offset: 0x00102CEF
	public int SphereOverlap(Vector3 position, float radius)
	{
		return MathUtils.OrientedBoxSphereOverlap(position, radius, this.center, this.size, this.orientation);
	}

	// Token: 0x06003710 RID: 14096 RVA: 0x00104B0A File Offset: 0x00102D0A
	public override string ToString()
	{
		if (this.subZoneId != GTSubZone.none)
		{
			return ZString.Concat<GTZone, string, GTSubZone>(this.zoneId, ".", this.subZoneId);
		}
		return ZString.Concat<GTZone>(this.zoneId);
	}

	// Token: 0x06003711 RID: 14097 RVA: 0x00104B38 File Offset: 0x00102D38
	public override int GetHashCode()
	{
		int zoneKey = this.zoneKey;
		int hashCode = this.center.QuantizedId128().GetHashCode();
		int hashCode2 = this.size.QuantizedId128().GetHashCode();
		int hashCode3 = this.orientation.QuantizedId128().GetHashCode();
		return StaticHash.Compute(zoneKey, hashCode, hashCode2, hashCode3);
	}

	// Token: 0x06003712 RID: 14098 RVA: 0x00104BA1 File Offset: 0x00102DA1
	public static bool operator ==(ZoneNode x, ZoneNode y)
	{
		return x.Equals(y);
	}

	// Token: 0x06003713 RID: 14099 RVA: 0x00104BAB File Offset: 0x00102DAB
	public static bool operator !=(ZoneNode x, ZoneNode y)
	{
		return !x.Equals(y);
	}

	// Token: 0x06003714 RID: 14100 RVA: 0x00104BB8 File Offset: 0x00102DB8
	public bool Equals(ZoneNode other)
	{
		return this.zoneId == other.zoneId && this.subZoneId == other.subZoneId && this.center.Approx(other.center, 1E-05f) && this.size.Approx(other.size, 1E-05f) && this.orientation.Approx(other.orientation, 1E-06f);
	}

	// Token: 0x06003715 RID: 14101 RVA: 0x00104C2C File Offset: 0x00102E2C
	public override bool Equals(object obj)
	{
		if (obj is ZoneNode)
		{
			ZoneNode other = (ZoneNode)obj;
			return this.Equals(other);
		}
		return false;
	}

	// Token: 0x04003A11 RID: 14865
	public GTZone zoneId;

	// Token: 0x04003A12 RID: 14866
	public GTSubZone subZoneId;

	// Token: 0x04003A13 RID: 14867
	public Vector3 center;

	// Token: 0x04003A14 RID: 14868
	public Vector3 size;

	// Token: 0x04003A15 RID: 14869
	public Quaternion orientation;

	// Token: 0x04003A16 RID: 14870
	public Bounds AABB;

	// Token: 0x04003A17 RID: 14871
	public bool isValid;
}
