using System;
using System.Runtime.CompilerServices;
using UnityEngine;

// Token: 0x020006D1 RID: 1745
[Serializable]
public struct Id32
{
	// Token: 0x06002B4D RID: 11085 RVA: 0x0004D350 File Offset: 0x0004B550
	public Id32(string idString)
	{
		if (idString == null)
		{
			throw new ArgumentNullException("idString");
		}
		if (string.IsNullOrWhiteSpace(idString.Trim()))
		{
			throw new ArgumentNullException("idString");
		}
		this._id = XXHash32.Compute(idString, 0U);
	}

	// Token: 0x06002B4E RID: 11086 RVA: 0x0004D385 File Offset: 0x0004B585
	public unsafe static implicit operator int(Id32 i32)
	{
		return *Unsafe.As<Id32, int>(ref i32);
	}

	// Token: 0x06002B4F RID: 11087 RVA: 0x0004D38F File Offset: 0x0004B58F
	public static implicit operator Id32(string s)
	{
		return Id32.ComputeID(s);
	}

	// Token: 0x06002B50 RID: 11088 RVA: 0x00120540 File Offset: 0x0011E740
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public unsafe static Id32 ComputeID(string s)
	{
		int num = Id32.ComputeHash(s);
		return *Unsafe.As<int, Id32>(ref num);
	}

	// Token: 0x06002B51 RID: 11089 RVA: 0x0004D397 File Offset: 0x0004B597
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int ComputeHash(string s)
	{
		if (s == null)
		{
			return 0;
		}
		s = s.Trim();
		if (string.IsNullOrWhiteSpace(s))
		{
			return 0;
		}
		return XXHash32.Compute(s, 0U);
	}

	// Token: 0x06002B52 RID: 11090 RVA: 0x0004D3B7 File Offset: 0x0004B5B7
	public override int GetHashCode()
	{
		return this._id;
	}

	// Token: 0x06002B53 RID: 11091 RVA: 0x0004D3BF File Offset: 0x0004B5BF
	public override string ToString()
	{
		return string.Format("{{ {0} : {1} }}", "Id32", this._id);
	}

	// Token: 0x040030BE RID: 12478
	[SerializeField]
	private int _id;
}
