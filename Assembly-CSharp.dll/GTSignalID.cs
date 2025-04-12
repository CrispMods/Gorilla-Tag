using System;

// Token: 0x020005AD RID: 1453
[Serializable]
public struct GTSignalID : IEquatable<GTSignalID>, IEquatable<int>
{
	// Token: 0x0600240C RID: 9228 RVA: 0x000FF51C File Offset: 0x000FD71C
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

	// Token: 0x0600240D RID: 9229 RVA: 0x00047701 File Offset: 0x00045901
	public bool Equals(GTSignalID other)
	{
		return this._id == other._id;
	}

	// Token: 0x0600240E RID: 9230 RVA: 0x00047711 File Offset: 0x00045911
	public bool Equals(int other)
	{
		return this._id == other;
	}

	// Token: 0x0600240F RID: 9231 RVA: 0x0004771C File Offset: 0x0004591C
	public override int GetHashCode()
	{
		return this._id;
	}

	// Token: 0x06002410 RID: 9232 RVA: 0x00047724 File Offset: 0x00045924
	public static bool operator ==(GTSignalID x, GTSignalID y)
	{
		return x.Equals(y);
	}

	// Token: 0x06002411 RID: 9233 RVA: 0x0004772E File Offset: 0x0004592E
	public static bool operator !=(GTSignalID x, GTSignalID y)
	{
		return !x.Equals(y);
	}

	// Token: 0x06002412 RID: 9234 RVA: 0x0004771C File Offset: 0x0004591C
	public static implicit operator int(GTSignalID sid)
	{
		return sid._id;
	}

	// Token: 0x06002413 RID: 9235 RVA: 0x000FF558 File Offset: 0x000FD758
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
