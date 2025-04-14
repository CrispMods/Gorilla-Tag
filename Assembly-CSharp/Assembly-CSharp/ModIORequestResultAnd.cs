using System;

// Token: 0x020005F1 RID: 1521
public struct ModIORequestResultAnd<T>
{
	// Token: 0x060025D5 RID: 9685 RVA: 0x000BAC88 File Offset: 0x000B8E88
	public static ModIORequestResultAnd<T> CreateFailureResult(string inMessage)
	{
		return new ModIORequestResultAnd<T>
		{
			result = ModIORequestResult.CreateFailureResult(inMessage)
		};
	}

	// Token: 0x060025D6 RID: 9686 RVA: 0x000BACAC File Offset: 0x000B8EAC
	public static ModIORequestResultAnd<T> CreateSuccessResult(T payload)
	{
		return new ModIORequestResultAnd<T>
		{
			result = ModIORequestResult.CreateSuccessResult(),
			data = payload
		};
	}

	// Token: 0x040029EB RID: 10731
	public ModIORequestResult result;

	// Token: 0x040029EC RID: 10732
	public T data;
}
