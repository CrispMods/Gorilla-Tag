using System;

// Token: 0x020005AC RID: 1452
[Serializable]
public struct GTSignalID : IEquatable<GTSignalID>, IEquatable<int>
{
	// Token: 0x06002404 RID: 9220 RVA: 0x000B36D8 File Offset: 0x000B18D8
	public override bool Equals(object obj)
	{
		if (obj is GTSignalID)
		{
			GTSignalID other = (GTSignalID)obj;
			return this.Equals(other);
		}
		if (obj is int)
		{
			int other2 = (int)obj;
			return this.Equals(other2);
		}
		return false;
	}

	// Token: 0x06002405 RID: 9221 RVA: 0x000B3714 File Offset: 0x000B1914
	public bool Equals(GTSignalID other)
	{
		return this._id == other._id;
	}

	// Token: 0x06002406 RID: 9222 RVA: 0x000B3724 File Offset: 0x000B1924
	public bool Equals(int other)
	{
		return this._id == other;
	}

	// Token: 0x06002407 RID: 9223 RVA: 0x000B372F File Offset: 0x000B192F
	public override int GetHashCode()
	{
		return this._id;
	}

	// Token: 0x06002408 RID: 9224 RVA: 0x000B3737 File Offset: 0x000B1937
	public static bool operator ==(GTSignalID x, GTSignalID y)
	{
		return x.Equals(y);
	}

	// Token: 0x06002409 RID: 9225 RVA: 0x000B3741 File Offset: 0x000B1941
	public static bool operator !=(GTSignalID x, GTSignalID y)
	{
		return !x.Equals(y);
	}

	// Token: 0x0600240A RID: 9226 RVA: 0x000B372F File Offset: 0x000B192F
	public static implicit operator int(GTSignalID sid)
	{
		return sid._id;
	}

	// Token: 0x0600240B RID: 9227 RVA: 0x000B3750 File Offset: 0x000B1950
	public static implicit operator GTSignalID(string s)
	{
		return new GTSignalID
		{
			_id = GTSignal.ComputeID(s)
		};
	}

	// Token: 0x040027F9 RID: 10233
	private int _id;
}
