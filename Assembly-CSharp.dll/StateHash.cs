using System;

// Token: 0x020006D3 RID: 1747
[Serializable]
public struct StateHash
{
	// Token: 0x06002B66 RID: 11110 RVA: 0x0004C902 File Offset: 0x0004AB02
	public override int GetHashCode()
	{
		return HashCode.Combine<int, int>(this.last, this.next);
	}

	// Token: 0x06002B67 RID: 11111 RVA: 0x0011CC1C File Offset: 0x0011AE1C
	public override string ToString()
	{
		return this.GetHashCode().ToString();
	}

	// Token: 0x06002B68 RID: 11112 RVA: 0x0004C915 File Offset: 0x0004AB15
	public bool Changed()
	{
		if (this.last == this.next)
		{
			return false;
		}
		this.last = this.next;
		return true;
	}

	// Token: 0x06002B69 RID: 11113 RVA: 0x0004C934 File Offset: 0x0004AB34
	public void Poll<T0>(T0 v0)
	{
		this.last = this.next;
		this.next = HashCode.Combine<T0>(v0);
	}

	// Token: 0x06002B6A RID: 11114 RVA: 0x0004C94E File Offset: 0x0004AB4E
	public void Poll<T1, T2>(T1 v1, T2 v2)
	{
		this.last = this.next;
		this.next = HashCode.Combine<T1, T2>(v1, v2);
	}

	// Token: 0x06002B6B RID: 11115 RVA: 0x0004C969 File Offset: 0x0004AB69
	public void Poll<T1, T2, T3>(T1 v1, T2 v2, T3 v3)
	{
		this.last = this.next;
		this.next = HashCode.Combine<T1, T2, T3>(v1, v2, v3);
	}

	// Token: 0x06002B6C RID: 11116 RVA: 0x0004C985 File Offset: 0x0004AB85
	public void Poll<T1, T2, T3, T4>(T1 v1, T2 v2, T3 v3, T4 v4)
	{
		this.last = this.next;
		this.next = HashCode.Combine<T1, T2, T3, T4>(v1, v2, v3, v4);
	}

	// Token: 0x06002B6D RID: 11117 RVA: 0x0004C9A3 File Offset: 0x0004ABA3
	public void Poll<T1, T2, T3, T4, T5>(T1 v1, T2 v2, T3 v3, T4 v4, T5 v5)
	{
		this.last = this.next;
		this.next = HashCode.Combine<T1, T2, T3, T4, T5>(v1, v2, v3, v4, v5);
	}

	// Token: 0x06002B6E RID: 11118 RVA: 0x0004C9C3 File Offset: 0x0004ABC3
	public void Poll<T1, T2, T3, T4, T5, T6>(T1 v1, T2 v2, T3 v3, T4 v4, T5 v5, T6 v6)
	{
		this.last = this.next;
		this.next = HashCode.Combine<T1, T2, T3, T4, T5, T6>(v1, v2, v3, v4, v5, v6);
	}

	// Token: 0x06002B6F RID: 11119 RVA: 0x0004C9E5 File Offset: 0x0004ABE5
	public void Poll<T1, T2, T3, T4, T5, T6, T7>(T1 v1, T2 v2, T3 v3, T4 v4, T5 v5, T6 v6, T7 v7)
	{
		this.last = this.next;
		this.next = HashCode.Combine<T1, T2, T3, T4, T5, T6, T7>(v1, v2, v3, v4, v5, v6, v7);
	}

	// Token: 0x06002B70 RID: 11120 RVA: 0x0011CC40 File Offset: 0x0011AE40
	public void Poll<T1, T2, T3, T4, T5, T6, T7, T8>(T1 v1, T2 v2, T3 v3, T4 v4, T5 v5, T6 v6, T7 v7, T8 v8)
	{
		this.last = this.next;
		this.next = HashCode.Combine<T1, T2, T3, T4, T5, T6, T7, T8>(v1, v2, v3, v4, v5, v6, v7, v8);
	}

	// Token: 0x06002B71 RID: 11121 RVA: 0x0011CC74 File Offset: 0x0011AE74
	public void Poll<T1, T2, T3, T4, T5, T6, T7, T8, T9>(T1 v1, T2 v2, T3 v3, T4 v4, T5 v5, T6 v6, T7 v7, T8 v8, T9 v9)
	{
		this.last = this.next;
		int value = HashCode.Combine<T1, T2, T3, T4, T5, T6, T7, T8>(v1, v2, v3, v4, v5, v6, v7, v8);
		this.next = HashCode.Combine<int, T9>(value, v9);
	}

	// Token: 0x06002B72 RID: 11122 RVA: 0x0011CCB0 File Offset: 0x0011AEB0
	public void Poll<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(T1 v1, T2 v2, T3 v3, T4 v4, T5 v5, T6 v6, T7 v7, T8 v8, T9 v9, T10 v10)
	{
		this.last = this.next;
		int value = HashCode.Combine<T1, T2, T3, T4, T5, T6, T7, T8>(v1, v2, v3, v4, v5, v6, v7, v8);
		this.next = HashCode.Combine<int, T9, T10>(value, v9, v10);
	}

	// Token: 0x06002B73 RID: 11123 RVA: 0x0011CCEC File Offset: 0x0011AEEC
	public void Poll<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(T1 v1, T2 v2, T3 v3, T4 v4, T5 v5, T6 v6, T7 v7, T8 v8, T9 v9, T10 v10, T11 v11)
	{
		this.last = this.next;
		int value = HashCode.Combine<T1, T2, T3, T4, T5, T6, T7, T8>(v1, v2, v3, v4, v5, v6, v7, v8);
		this.next = HashCode.Combine<int, T9, T10, T11>(value, v9, v10, v11);
	}

	// Token: 0x06002B74 RID: 11124 RVA: 0x0011CD2C File Offset: 0x0011AF2C
	public void Poll<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(T1 v1, T2 v2, T3 v3, T4 v4, T5 v5, T6 v6, T7 v7, T8 v8, T9 v9, T10 v10, T11 v11, T12 v12)
	{
		this.last = this.next;
		int value = HashCode.Combine<T1, T2, T3, T4, T5, T6, T7, T8>(v1, v2, v3, v4, v5, v6, v7, v8);
		this.next = HashCode.Combine<int, T9, T10, T11, T12>(value, v9, v10, v11, v12);
	}

	// Token: 0x06002B75 RID: 11125 RVA: 0x0011CD6C File Offset: 0x0011AF6C
	public void Poll<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(T1 v1, T2 v2, T3 v3, T4 v4, T5 v5, T6 v6, T7 v7, T8 v8, T9 v9, T10 v10, T11 v11, T12 v12, T13 v13)
	{
		this.last = this.next;
		int value = HashCode.Combine<T1, T2, T3, T4, T5, T6, T7, T8>(v1, v2, v3, v4, v5, v6, v7, v8);
		this.next = HashCode.Combine<int, T9, T10, T11, T12, T13>(value, v9, v10, v11, v12, v13);
	}

	// Token: 0x06002B76 RID: 11126 RVA: 0x0011CDB0 File Offset: 0x0011AFB0
	public void Poll<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(T1 v1, T2 v2, T3 v3, T4 v4, T5 v5, T6 v6, T7 v7, T8 v8, T9 v9, T10 v10, T11 v11, T12 v12, T13 v13, T14 v14)
	{
		this.last = this.next;
		int value = HashCode.Combine<T1, T2, T3, T4, T5, T6, T7, T8>(v1, v2, v3, v4, v5, v6, v7, v8);
		this.next = HashCode.Combine<int, T9, T10, T11, T12, T13, T14>(value, v9, v10, v11, v12, v13, v14);
	}

	// Token: 0x06002B77 RID: 11127 RVA: 0x0011CDF4 File Offset: 0x0011AFF4
	public void Poll<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>(T1 v1, T2 v2, T3 v3, T4 v4, T5 v5, T6 v6, T7 v7, T8 v8, T9 v9, T10 v10, T11 v11, T12 v12, T13 v13, T14 v14, T15 v15)
	{
		this.last = this.next;
		int value = HashCode.Combine<T1, T2, T3, T4, T5, T6, T7, T8>(v1, v2, v3, v4, v5, v6, v7, v8);
		this.next = HashCode.Combine<int, T9, T10, T11, T12, T13, T14, T15>(value, v9, v10, v11, v12, v13, v14, v15);
	}

	// Token: 0x06002B78 RID: 11128 RVA: 0x0011CE3C File Offset: 0x0011B03C
	public void Poll<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>(T1 v1, T2 v2, T3 v3, T4 v4, T5 v5, T6 v6, T7 v7, T8 v8, T9 v9, T10 v10, T11 v11, T12 v12, T13 v13, T14 v14, T15 v15, T16 v16)
	{
		this.last = this.next;
		int value = HashCode.Combine<T1, T2, T3, T4, T5, T6, T7, T8>(v1, v2, v3, v4, v5, v6, v7, v8);
		int value2 = HashCode.Combine<int, T9, T10, T11, T12, T13, T14, T15>(value, v9, v10, v11, v12, v13, v14, v15);
		this.next = HashCode.Combine<int, int, T16>(value, value2, v16);
	}

	// Token: 0x040030B5 RID: 12469
	public int last;

	// Token: 0x040030B6 RID: 12470
	public int next;
}
