using System;
using UnityEngine;

// Token: 0x02000862 RID: 2146
public static class EchoUtils
{
	// Token: 0x06003455 RID: 13397 RVA: 0x000527B6 File Offset: 0x000509B6
	[HideInCallstack]
	public static T Echo<T>(this T message)
	{
		Debug.Log(message);
		return message;
	}

	// Token: 0x06003456 RID: 13398 RVA: 0x000527C4 File Offset: 0x000509C4
	[HideInCallstack]
	public static T Echo<T>(this T message, UnityEngine.Object context)
	{
		Debug.Log(message, context);
		return message;
	}
}
