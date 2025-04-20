using System;

// Token: 0x02000807 RID: 2055
[Serializable]
public class CallLimitType<T> where T : CallLimiter
{
	// Token: 0x060032CC RID: 13004 RVA: 0x000518C2 File Offset: 0x0004FAC2
	public static implicit operator CallLimitType<CallLimiter>(CallLimitType<T> clt)
	{
		return new CallLimitType<CallLimiter>
		{
			Key = clt.Key,
			UseNetWorkTime = clt.UseNetWorkTime,
			CallLimitSettings = clt.CallLimitSettings
		};
	}

	// Token: 0x04003659 RID: 13913
	public FXType Key;

	// Token: 0x0400365A RID: 13914
	public bool UseNetWorkTime;

	// Token: 0x0400365B RID: 13915
	public T CallLimitSettings;
}
