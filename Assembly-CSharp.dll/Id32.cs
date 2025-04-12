using System;
using System.Runtime.CompilerServices;
using UnityEngine;

// Token: 0x020006BD RID: 1725
[Serializable]
public struct Id32
{
	// Token: 0x06002ABF RID: 10943 RVA: 0x0004C00B File Offset: 0x0004A20B
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

	// Token: 0x06002AC0 RID: 10944 RVA: 0x0004C040 File Offset: 0x0004A240
	public unsafe static implicit operator int(Id32 i32)
	{
		return *Unsafe.As<Id32, int>(ref i32);
	}

	// Token: 0x06002AC1 RID: 10945 RVA: 0x0004C04A File Offset: 0x0004A24A
	public static implicit operator Id32(string s)
	{
		return Id32.ComputeID(s);
	}

	// Token: 0x06002AC2 RID: 10946 RVA: 0x0011B988 File Offset: 0x00119B88
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public unsafe static Id32 ComputeID(string s)
	{
		int num = Id32.ComputeHash(s);
		return *Unsafe.As<int, Id32>(ref num);
	}

	// Token: 0x06002AC3 RID: 10947 RVA: 0x0004C052 File Offset: 0x0004A252
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

	// Token: 0x06002AC4 RID: 10948 RVA: 0x0004C072 File Offset: 0x0004A272
	public override int GetHashCode()
	{
		return this._id;
	}

	// Token: 0x06002AC5 RID: 10949 RVA: 0x0004C07A File Offset: 0x0004A27A
	public override string ToString()
	{
		return string.Format("{{ {0} : {1} }}", "Id32", this._id);
	}

	// Token: 0x04003027 RID: 12327
	[SerializeField]
	private int _id;
}
