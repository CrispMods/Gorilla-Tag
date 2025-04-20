using System;

// Token: 0x0200067B RID: 1659
public struct ModIORequestResultAnd<T>
{
	// Token: 0x06002929 RID: 10537 RVA: 0x0011511C File Offset: 0x0011331C
	public static ModIORequestResultAnd<T> CreateFailureResult(string inMessage)
	{
		return new ModIORequestResultAnd<T>
		{
			result = ModIORequestResult.CreateFailureResult(inMessage)
		};
	}

	// Token: 0x0600292A RID: 10538 RVA: 0x00115140 File Offset: 0x00113340
	public static ModIORequestResultAnd<T> CreateSuccessResult(T payload)
	{
		return new ModIORequestResultAnd<T>
		{
			result = ModIORequestResult.CreateSuccessResult(),
			data = payload
		};
	}

	// Token: 0x04002E80 RID: 11904
	public ModIORequestResult result;

	// Token: 0x04002E81 RID: 11905
	public T data;
}
