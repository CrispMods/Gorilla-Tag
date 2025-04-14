using System;

// Token: 0x020005F0 RID: 1520
public struct ModIORequestResultAnd<T>
{
	// Token: 0x060025CD RID: 9677 RVA: 0x000BA808 File Offset: 0x000B8A08
	public static ModIORequestResultAnd<T> CreateFailureResult(string inMessage)
	{
		return new ModIORequestResultAnd<T>
		{
			result = ModIORequestResult.CreateFailureResult(inMessage)
		};
	}

	// Token: 0x060025CE RID: 9678 RVA: 0x000BA82C File Offset: 0x000B8A2C
	public static ModIORequestResultAnd<T> CreateSuccessResult(T payload)
	{
		return new ModIORequestResultAnd<T>
		{
			result = ModIORequestResult.CreateSuccessResult(),
			data = payload
		};
	}

	// Token: 0x040029E5 RID: 10725
	public ModIORequestResult result;

	// Token: 0x040029E6 RID: 10726
	public T data;
}
