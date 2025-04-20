using System;

// Token: 0x020005BA RID: 1466
[Serializable]
public struct GTSignalID : IEquatable<GTSignalID>, IEquatable<int>
{
	// Token: 0x06002464 RID: 9316 RVA: 0x00102350 File Offset: 0x00100550
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

	// Token: 0x06002465 RID: 9317 RVA: 0x00048AD6 File Offset: 0x00046CD6
	public bool Equals(GTSignalID other)
	{
		return this._id == other._id;
	}

	// Token: 0x06002466 RID: 9318 RVA: 0x00048AE6 File Offset: 0x00046CE6
	public bool Equals(int other)
	{
		return this._id == other;
	}

	// Token: 0x06002467 RID: 9319 RVA: 0x00048AF1 File Offset: 0x00046CF1
	public override int GetHashCode()
	{
		return this._id;
	}

	// Token: 0x06002468 RID: 9320 RVA: 0x00048AF9 File Offset: 0x00046CF9
	public static bool operator ==(GTSignalID x, GTSignalID y)
	{
		return x.Equals(y);
	}

	// Token: 0x06002469 RID: 9321 RVA: 0x00048B03 File Offset: 0x00046D03
	public static bool operator !=(GTSignalID x, GTSignalID y)
	{
		return !x.Equals(y);
	}

	// Token: 0x0600246A RID: 9322 RVA: 0x00048AF1 File Offset: 0x00046CF1
	public static implicit operator int(GTSignalID sid)
	{
		return sid._id;
	}

	// Token: 0x0600246B RID: 9323 RVA: 0x0010238C File Offset: 0x0010058C
	public static implicit operator GTSignalID(string s)
	{
		return new GTSignalID
		{
			_id = GTSignal.ComputeID(s)
		};
	}

	// Token: 0x04002855 RID: 10325
	private int _id;
}
