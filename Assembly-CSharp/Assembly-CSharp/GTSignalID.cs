using System;

// Token: 0x020005AD RID: 1453
[Serializable]
public struct GTSignalID : IEquatable<GTSignalID>, IEquatable<int>
{
	// Token: 0x0600240C RID: 9228 RVA: 0x000B3B58 File Offset: 0x000B1D58
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

	// Token: 0x0600240D RID: 9229 RVA: 0x000B3B94 File Offset: 0x000B1D94
	public bool Equals(GTSignalID other)
	{
		return this._id == other._id;
	}

	// Token: 0x0600240E RID: 9230 RVA: 0x000B3BA4 File Offset: 0x000B1DA4
	public bool Equals(int other)
	{
		return this._id == other;
	}

	// Token: 0x0600240F RID: 9231 RVA: 0x000B3BAF File Offset: 0x000B1DAF
	public override int GetHashCode()
	{
		return this._id;
	}

	// Token: 0x06002410 RID: 9232 RVA: 0x000B3BB7 File Offset: 0x000B1DB7
	public static bool operator ==(GTSignalID x, GTSignalID y)
	{
		return x.Equals(y);
	}

	// Token: 0x06002411 RID: 9233 RVA: 0x000B3BC1 File Offset: 0x000B1DC1
	public static bool operator !=(GTSignalID x, GTSignalID y)
	{
		return !x.Equals(y);
	}

	// Token: 0x06002412 RID: 9234 RVA: 0x000B3BAF File Offset: 0x000B1DAF
	public static implicit operator int(GTSignalID sid)
	{
		return sid._id;
	}

	// Token: 0x06002413 RID: 9235 RVA: 0x000B3BD0 File Offset: 0x000B1DD0
	public static implicit operator GTSignalID(string s)
	{
		return new GTSignalID
		{
			_id = GTSignal.ComputeID(s)
		};
	}

	// Token: 0x040027FF RID: 10239
	private int _id;
}
