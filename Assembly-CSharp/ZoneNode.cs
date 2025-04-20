using System;
using Cysharp.Text;
using UnityEngine;

// Token: 0x0200090F RID: 2319
[Serializable]
public struct ZoneNode : IEquatable<ZoneNode>
{
	// Token: 0x170005B5 RID: 1461
	// (get) Token: 0x060037DD RID: 14301 RVA: 0x00054F3E File Offset: 0x0005313E
	public static ZoneNode Null { get; } = new ZoneNode
	{
		zoneId = GTZone.none,
		subZoneId = GTSubZone.none,
		isValid = false
	};

	// Token: 0x170005B6 RID: 1462
	// (get) Token: 0x060037DE RID: 14302 RVA: 0x00054F45 File Offset: 0x00053145
	public int zoneKey
	{
		get
		{
			return StaticHash.Compute((int)this.zoneId, (int)this.subZoneId);
		}
	}

	// Token: 0x060037DF RID: 14303 RVA: 0x00054F58 File Offset: 0x00053158
	public bool ContainsPoint(Vector3 point)
	{
		return MathUtils.OrientedBoxContains(point, this.center, this.size, this.orientation);
	}

	// Token: 0x060037E0 RID: 14304 RVA: 0x00054F72 File Offset: 0x00053172
	public int SphereOverlap(Vector3 position, float radius)
	{
		return MathUtils.OrientedBoxSphereOverlap(position, radius, this.center, this.size, this.orientation);
	}

	// Token: 0x060037E1 RID: 14305 RVA: 0x00054F8D File Offset: 0x0005318D
	public override string ToString()
	{
		if (this.subZoneId != GTSubZone.none)
		{
			return ZString.Concat<GTZone, string, GTSubZone>(this.zoneId, ".", this.subZoneId);
		}
		return ZString.Concat<GTZone>(this.zoneId);
	}

	// Token: 0x060037E2 RID: 14306 RVA: 0x001499B4 File Offset: 0x00147BB4
	public override int GetHashCode()
	{
		int zoneKey = this.zoneKey;
		int hashCode = this.center.QuantizedId128().GetHashCode();
		int hashCode2 = this.size.QuantizedId128().GetHashCode();
		int hashCode3 = this.orientation.QuantizedId128().GetHashCode();
		return StaticHash.Compute(zoneKey, hashCode, hashCode2, hashCode3);
	}

	// Token: 0x060037E3 RID: 14307 RVA: 0x00054FB9 File Offset: 0x000531B9
	public static bool operator ==(ZoneNode x, ZoneNode y)
	{
		return x.Equals(y);
	}

	// Token: 0x060037E4 RID: 14308 RVA: 0x00054FC3 File Offset: 0x000531C3
	public static bool operator !=(ZoneNode x, ZoneNode y)
	{
		return !x.Equals(y);
	}

	// Token: 0x060037E5 RID: 14309 RVA: 0x00149A20 File Offset: 0x00147C20
	public bool Equals(ZoneNode other)
	{
		return this.zoneId == other.zoneId && this.subZoneId == other.subZoneId && this.center.Approx(other.center, 1E-05f) && this.size.Approx(other.size, 1E-05f) && this.orientation.Approx(other.orientation, 1E-06f);
	}

	// Token: 0x060037E6 RID: 14310 RVA: 0x00149A94 File Offset: 0x00147C94
	public override bool Equals(object obj)
	{
		if (obj is ZoneNode)
		{
			ZoneNode other = (ZoneNode)obj;
			return this.Equals(other);
		}
		return false;
	}

	// Token: 0x04003AD6 RID: 15062
	public GTZone zoneId;

	// Token: 0x04003AD7 RID: 15063
	public GTSubZone subZoneId;

	// Token: 0x04003AD8 RID: 15064
	public Vector3 center;

	// Token: 0x04003AD9 RID: 15065
	public Vector3 size;

	// Token: 0x04003ADA RID: 15066
	public Quaternion orientation;

	// Token: 0x04003ADB RID: 15067
	public Bounds AABB;

	// Token: 0x04003ADC RID: 15068
	public bool isValid;
}
