using System;

// Token: 0x020007ED RID: 2029
[Serializable]
public class CallLimitType<T> where T : CallLimiter
{
	// Token: 0x06003216 RID: 12822 RVA: 0x000F080F File Offset: 0x000EEA0F
	public static implicit operator CallLimitType<CallLimiter>(CallLimitType<T> clt)
	{
		return new CallLimitType<CallLimiter>
		{
			Key = clt.Key,
			UseNetWorkTime = clt.UseNetWorkTime,
			CallLimitSettings = clt.CallLimitSettings
		};
	}

	// Token: 0x0400359E RID: 13726
	public FXType Key;

	// Token: 0x0400359F RID: 13727
	public bool UseNetWorkTime;

	// Token: 0x040035A0 RID: 13728
	public T CallLimitSettings;
}
