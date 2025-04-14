using System;

// Token: 0x020005EF RID: 1519
public struct ModIORequestResult
{
	// Token: 0x060025CB RID: 9675 RVA: 0x000BA7C4 File Offset: 0x000B89C4
	public static ModIORequestResult CreateFailureResult(string inMessage)
	{
		ModIORequestResult result;
		result.success = false;
		result.message = inMessage;
		return result;
	}

	// Token: 0x060025CC RID: 9676 RVA: 0x000BA7E4 File Offset: 0x000B89E4
	public static ModIORequestResult CreateSuccessResult()
	{
		ModIORequestResult result;
		result.success = true;
		result.message = "";
		return result;
	}

	// Token: 0x040029E3 RID: 10723
	public bool success;

	// Token: 0x040029E4 RID: 10724
	public string message;
}
