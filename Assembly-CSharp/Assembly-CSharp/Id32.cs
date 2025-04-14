using System;
using System.Runtime.CompilerServices;
using UnityEngine;

// Token: 0x020006BD RID: 1725
[Serializable]
public struct Id32
{
	// Token: 0x06002ABF RID: 10943 RVA: 0x000D48F0 File Offset: 0x000D2AF0
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

	// Token: 0x06002AC0 RID: 10944 RVA: 0x000D4925 File Offset: 0x000D2B25
	public unsafe static implicit operator int(Id32 i32)
	{
		return *Unsafe.As<Id32, int>(ref i32);
	}

	// Token: 0x06002AC1 RID: 10945 RVA: 0x000D492F File Offset: 0x000D2B2F
	public static implicit operator Id32(string s)
	{
		return Id32.ComputeID(s);
	}

	// Token: 0x06002AC2 RID: 10946 RVA: 0x000D4938 File Offset: 0x000D2B38
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public unsafe static Id32 ComputeID(string s)
	{
		int num = Id32.ComputeHash(s);
		return *Unsafe.As<int, Id32>(ref num);
	}

	// Token: 0x06002AC3 RID: 10947 RVA: 0x000D4958 File Offset: 0x000D2B58
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

	// Token: 0x06002AC4 RID: 10948 RVA: 0x000D4978 File Offset: 0x000D2B78
	public override int GetHashCode()
	{
		return this._id;
	}

	// Token: 0x06002AC5 RID: 10949 RVA: 0x000D4980 File Offset: 0x000D2B80
	public override string ToString()
	{
		return string.Format("{{ {0} : {1} }}", "Id32", this._id);
	}

	// Token: 0x04003027 RID: 12327
	[SerializeField]
	private int _id;
}
