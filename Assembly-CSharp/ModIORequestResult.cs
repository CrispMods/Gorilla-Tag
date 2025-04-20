using System;

// Token: 0x0200067A RID: 1658
public struct ModIORequestResult
{
	// Token: 0x06002927 RID: 10535 RVA: 0x001150D8 File Offset: 0x001132D8
	public static ModIORequestResult CreateFailureResult(string inMessage)
	{
		ModIORequestResult result;
		result.success = false;
		result.message = inMessage;
		return result;
	}

	// Token: 0x06002928 RID: 10536 RVA: 0x001150F8 File Offset: 0x001132F8
	public static ModIORequestResult CreateSuccessResult()
	{
		ModIORequestResult result;
		result.success = true;
		result.message = "";
		return result;
	}

	// Token: 0x04002E7E RID: 11902
	public bool success;

	// Token: 0x04002E7F RID: 11903
	public string message;
}
