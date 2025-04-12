using System;

// Token: 0x020007F0 RID: 2032
[Serializable]
public class CallLimitType<T> where T : CallLimiter
{
	// Token: 0x06003222 RID: 12834 RVA: 0x000504C0 File Offset: 0x0004E6C0
	public static implicit operator CallLimitType<CallLimiter>(CallLimitType<T> clt)
	{
		return new CallLimitType<CallLimiter>
		{
			Key = clt.Key,
			UseNetWorkTime = clt.UseNetWorkTime,
			CallLimitSettings = clt.CallLimitSettings
		};
	}

	// Token: 0x040035B0 RID: 13744
	public FXType Key;

	// Token: 0x040035B1 RID: 13745
	public bool UseNetWorkTime;

	// Token: 0x040035B2 RID: 13746
	public T CallLimitSettings;
}
