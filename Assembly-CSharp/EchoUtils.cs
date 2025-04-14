using System;
using UnityEngine;

// Token: 0x02000848 RID: 2120
public static class EchoUtils
{
	// Token: 0x0600339A RID: 13210 RVA: 0x000F65EA File Offset: 0x000F47EA
	[HideInCallstack]
	public static T Echo<T>(this T message)
	{
		Debug.Log(message);
		return message;
	}

	// Token: 0x0600339B RID: 13211 RVA: 0x000F65F8 File Offset: 0x000F47F8
	[HideInCallstack]
	public static T Echo<T>(this T message, Object context)
	{
		Debug.Log(message, context);
		return message;
	}
}
