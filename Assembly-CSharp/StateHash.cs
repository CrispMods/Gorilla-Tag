using System;

// Token: 0x020006E7 RID: 1767
[Serializable]
public struct StateHash
{
	// Token: 0x06002BF4 RID: 11252 RVA: 0x0004DC47 File Offset: 0x0004BE47
	public override int GetHashCode()
	{
		return HashCode.Combine<int, int>(this.last, this.next);
	}

	// Token: 0x06002BF5 RID: 11253 RVA: 0x001217D4 File Offset: 0x0011F9D4
	public override string ToString()
	{
		return this.GetHashCode().ToString();
	}

	// Token: 0x06002BF6 RID: 11254 RVA: 0x0004DC5A File Offset: 0x0004BE5A
	public bool Changed()
	{
		if (this.last == this.next)
		{
			return false;
		}
		this.last = this.next;
		return true;
	}

	// Token: 0x06002BF7 RID: 11255 RVA: 0x0004DC79 File Offset: 0x0004BE79
	public void Poll<T0>(T0 v0)
	{
		this.last = this.next;
		this.next = HashCode.Combine<T0>(v0);
	}

	// Token: 0x06002BF8 RID: 11256 RVA: 0x0004DC93 File Offset: 0x0004BE93
	public void Poll<T1, T2>(T1 v1, T2 v2)
	{
		this.last = this.next;
		this.next = HashCode.Combine<T1, T2>(v1, v2);
	}

	// Token: 0x06002BF9 RID: 11257 RVA: 0x0004DCAE File Offset: 0x0004BEAE
	public void Poll<T1, T2, T3>(T1 v1, T2 v2, T3 v3)
	{
		this.last = this.next;
		this.next = HashCode.Combine<T1, T2, T3>(v1, v2, v3);
	}

	// Token: 0x06002BFA RID: 11258 RVA: 0x0004DCCA File Offset: 0x0004BECA
	public void Poll<T1, T2, T3, T4>(T1 v1, T2 v2, T3 v3, T4 v4)
	{
		this.last = this.next;
		this.next = HashCode.Combine<T1, T2, T3, T4>(v1, v2, v3, v4);
	}

	// Token: 0x06002BFB RID: 11259 RVA: 0x0004DCE8 File Offset: 0x0004BEE8
	public void Poll<T1, T2, T3, T4, T5>(T1 v1, T2 v2, T3 v3, T4 v4, T5 v5)
	{
		this.last = this.next;
		this.next = HashCode.Combine<T1, T2, T3, T4, T5>(v1, v2, v3, v4, v5);
	}

	// Token: 0x06002BFC RID: 11260 RVA: 0x0004DD08 File Offset: 0x0004BF08
	public void Poll<T1, T2, T3, T4, T5, T6>(T1 v1, T2 v2, T3 v3, T4 v4, T5 v5, T6 v6)
	{
		this.last = this.next;
		this.next = HashCode.Combine<T1, T2, T3, T4, T5, T6>(v1, v2, v3, v4, v5, v6);
	}

	// Token: 0x06002BFD RID: 11261 RVA: 0x0004DD2A File Offset: 0x0004BF2A
	public void Poll<T1, T2, T3, T4, T5, T6, T7>(T1 v1, T2 v2, T3 v3, T4 v4, T5 v5, T6 v6, T7 v7)
	{
		this.last = this.next;
		this.next = HashCode.Combine<T1, T2, T3, T4, T5, T6, T7>(v1, v2, v3, v4, v5, v6, v7);
	}

	// Token: 0x06002BFE RID: 11262 RVA: 0x001217F8 File Offset: 0x0011F9F8
	public void Poll<T1, T2, T3, T4, T5, T6, T7, T8>(T1 v1, T2 v2, T3 v3, T4 v4, T5 v5, T6 v6, T7 v7, T8 v8)
	{
		this.last = this.next;
		this.next = HashCode.Combine<T1, T2, T3, T4, T5, T6, T7, T8>(v1, v2, v3, v4, v5, v6, v7, v8);
	}

	// Token: 0x06002BFF RID: 11263 RVA: 0x0012182C File Offset: 0x0011FA2C
	public void Poll<T1, T2, T3, T4, T5, T6, T7, T8, T9>(T1 v1, T2 v2, T3 v3, T4 v4, T5 v5, T6 v6, T7 v7, T8 v8, T9 v9)
	{
		this.last = this.next;
		int value = HashCode.Combine<T1, T2, T3, T4, T5, T6, T7, T8>(v1, v2, v3, v4, v5, v6, v7, v8);
		this.next = HashCode.Combine<int, T9>(value, v9);
	}

	// Token: 0x06002C00 RID: 11264 RVA: 0x00121868 File Offset: 0x0011FA68
	public void Poll<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(T1 v1, T2 v2, T3 v3, T4 v4, T5 v5, T6 v6, T7 v7, T8 v8, T9 v9, T10 v10)
	{
		this.last = this.next;
		int value = HashCode.Combine<T1, T2, T3, T4, T5, T6, T7, T8>(v1, v2, v3, v4, v5, v6, v7, v8);
		this.next = HashCode.Combine<int, T9, T10>(value, v9, v10);
	}

	// Token: 0x06002C01 RID: 11265 RVA: 0x001218A4 File Offset: 0x0011FAA4
	public void Poll<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(T1 v1, T2 v2, T3 v3, T4 v4, T5 v5, T6 v6, T7 v7, T8 v8, T9 v9, T10 v10, T11 v11)
	{
		this.last = this.next;
		int value = HashCode.Combine<T1, T2, T3, T4, T5, T6, T7, T8>(v1, v2, v3, v4, v5, v6, v7, v8);
		this.next = HashCode.Combine<int, T9, T10, T11>(value, v9, v10, v11);
	}

	// Token: 0x06002C02 RID: 11266 RVA: 0x001218E4 File Offset: 0x0011FAE4
	public void Poll<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(T1 v1, T2 v2, T3 v3, T4 v4, T5 v5, T6 v6, T7 v7, T8 v8, T9 v9, T10 v10, T11 v11, T12 v12)
	{
		this.last = this.next;
		int value = HashCode.Combine<T1, T2, T3, T4, T5, T6, T7, T8>(v1, v2, v3, v4, v5, v6, v7, v8);
		this.next = HashCode.Combine<int, T9, T10, T11, T12>(value, v9, v10, v11, v12);
	}

	// Token: 0x06002C03 RID: 11267 RVA: 0x00121924 File Offset: 0x0011FB24
	public void Poll<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(T1 v1, T2 v2, T3 v3, T4 v4, T5 v5, T6 v6, T7 v7, T8 v8, T9 v9, T10 v10, T11 v11, T12 v12, T13 v13)
	{
		this.last = this.next;
		int value = HashCode.Combine<T1, T2, T3, T4, T5, T6, T7, T8>(v1, v2, v3, v4, v5, v6, v7, v8);
		this.next = HashCode.Combine<int, T9, T10, T11, T12, T13>(value, v9, v10, v11, v12, v13);
	}

	// Token: 0x06002C04 RID: 11268 RVA: 0x00121968 File Offset: 0x0011FB68
	public void Poll<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(T1 v1, T2 v2, T3 v3, T4 v4, T5 v5, T6 v6, T7 v7, T8 v8, T9 v9, T10 v10, T11 v11, T12 v12, T13 v13, T14 v14)
	{
		this.last = this.next;
		int value = HashCode.Combine<T1, T2, T3, T4, T5, T6, T7, T8>(v1, v2, v3, v4, v5, v6, v7, v8);
		this.next = HashCode.Combine<int, T9, T10, T11, T12, T13, T14>(value, v9, v10, v11, v12, v13, v14);
	}

	// Token: 0x06002C05 RID: 11269 RVA: 0x001219AC File Offset: 0x0011FBAC
	public void Poll<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>(T1 v1, T2 v2, T3 v3, T4 v4, T5 v5, T6 v6, T7 v7, T8 v8, T9 v9, T10 v10, T11 v11, T12 v12, T13 v13, T14 v14, T15 v15)
	{
		this.last = this.next;
		int value = HashCode.Combine<T1, T2, T3, T4, T5, T6, T7, T8>(v1, v2, v3, v4, v5, v6, v7, v8);
		this.next = HashCode.Combine<int, T9, T10, T11, T12, T13, T14, T15>(value, v9, v10, v11, v12, v13, v14, v15);
	}

	// Token: 0x06002C06 RID: 11270 RVA: 0x001219F4 File Offset: 0x0011FBF4
	public void Poll<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>(T1 v1, T2 v2, T3 v3, T4 v4, T5 v5, T6 v6, T7 v7, T8 v8, T9 v9, T10 v10, T11 v11, T12 v12, T13 v13, T14 v14, T15 v15, T16 v16)
	{
		this.last = this.next;
		int value = HashCode.Combine<T1, T2, T3, T4, T5, T6, T7, T8>(v1, v2, v3, v4, v5, v6, v7, v8);
		int value2 = HashCode.Combine<int, T9, T10, T11, T12, T13, T14, T15>(value, v9, v10, v11, v12, v13, v14, v15);
		this.next = HashCode.Combine<int, int, T16>(value, value2, v16);
	}

	// Token: 0x0400314C RID: 12620
	public int last;

	// Token: 0x0400314D RID: 12621
	public int next;
}
