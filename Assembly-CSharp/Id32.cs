using System;
using System.Runtime.CompilerServices;
using UnityEngine;

// Token: 0x020006BC RID: 1724
[Serializable]
public struct Id32
{
	// Token: 0x06002AB7 RID: 10935 RVA: 0x000D4470 File Offset: 0x000D2670
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

	// Token: 0x06002AB8 RID: 10936 RVA: 0x000D44A5 File Offset: 0x000D26A5
	public unsafe static implicit operator int(Id32 i32)
	{
		return *Unsafe.As<Id32, int>(ref i32);
	}

	// Token: 0x06002AB9 RID: 10937 RVA: 0x000D44AF File Offset: 0x000D26AF
	public static implicit operator Id32(string s)
	{
		return Id32.ComputeID(s);
	}

	// Token: 0x06002ABA RID: 10938 RVA: 0x000D44B8 File Offset: 0x000D26B8
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public unsafe static Id32 ComputeID(string s)
	{
		int num = Id32.ComputeHash(s);
		return *Unsafe.As<int, Id32>(ref num);
	}

	// Token: 0x06002ABB RID: 10939 RVA: 0x000D44D8 File Offset: 0x000D26D8
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

	// Token: 0x06002ABC RID: 10940 RVA: 0x000D44F8 File Offset: 0x000D26F8
	public override int GetHashCode()
	{
		return this._id;
	}

	// Token: 0x06002ABD RID: 10941 RVA: 0x000D4500 File Offset: 0x000D2700
	public override string ToString()
	{
		return string.Format("{{ {0} : {1} }}", "Id32", this._id);
	}

	// Token: 0x04003021 RID: 12321
	[SerializeField]
	private int _id;
}
