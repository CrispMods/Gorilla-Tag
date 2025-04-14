using System;
using UnityEngine;

// Token: 0x0200084B RID: 2123
public static class EchoUtils
{
	// Token: 0x060033A6 RID: 13222 RVA: 0x000F6BB2 File Offset: 0x000F4DB2
	[HideInCallstack]
	public static T Echo<T>(this T message)
	{
		Debug.Log(message);
		return message;
	}

	// Token: 0x060033A7 RID: 13223 RVA: 0x000F6BC0 File Offset: 0x000F4DC0
	[HideInCallstack]
	public static T Echo<T>(this T message, Object context)
	{
		Debug.Log(message, context);
		return message;
	}
}
