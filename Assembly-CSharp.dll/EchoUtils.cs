using System;
using UnityEngine;

// Token: 0x0200084B RID: 2123
public static class EchoUtils
{
	// Token: 0x060033A6 RID: 13222 RVA: 0x000513A8 File Offset: 0x0004F5A8
	[HideInCallstack]
	public static T Echo<T>(this T message)
	{
		Debug.Log(message);
		return message;
	}

	// Token: 0x060033A7 RID: 13223 RVA: 0x000513B6 File Offset: 0x0004F5B6
	[HideInCallstack]
	public static T Echo<T>(this T message, UnityEngine.Object context)
	{
		Debug.Log(message, context);
		return message;
	}
}
