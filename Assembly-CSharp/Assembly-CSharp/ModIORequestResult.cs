using System;

// Token: 0x020005F0 RID: 1520
public struct ModIORequestResult
{
	// Token: 0x060025D3 RID: 9683 RVA: 0x000BAC44 File Offset: 0x000B8E44
	public static ModIORequestResult CreateFailureResult(string inMessage)
	{
		ModIORequestResult result;
		result.success = false;
		result.message = inMessage;
		return result;
	}

	// Token: 0x060025D4 RID: 9684 RVA: 0x000BAC64 File Offset: 0x000B8E64
	public static ModIORequestResult CreateSuccessResult()
	{
		ModIORequestResult result;
		result.success = true;
		result.message = "";
		return result;
	}

	// Token: 0x040029E9 RID: 10729
	public bool success;

	// Token: 0x040029EA RID: 10730
	public string message;
}
