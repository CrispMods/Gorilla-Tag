using System;
using System.Runtime.CompilerServices;
using Cysharp.Text;
using UnityEngine;

// Token: 0x020006A5 RID: 1701
public static class ContextLog
{
	// Token: 0x06002A4D RID: 10829 RVA: 0x000D326F File Offset: 0x000D146F
	public static void Log<T0, T1>(this T0 ctx, T1 arg1)
	{
		Debug.Log(ZString.Concat<string, T1>(ContextLog.GetPrefix<T0>(ref ctx), arg1));
	}

	// Token: 0x06002A4E RID: 10830 RVA: 0x000D3284 File Offset: 0x000D1484
	public static void LogCall<T0, T1>(this T0 ctx, T1 arg1, [CallerMemberName] string call = null)
	{
		string prefix = ContextLog.GetPrefix<T0>(ref ctx);
		string arg2 = ZString.Concat<string, string, string>("{.", call, "()} ");
		Debug.Log(ZString.Concat<string, string, T1>(prefix, arg2, arg1));
	}

	// Token: 0x06002A4F RID: 10831 RVA: 0x000D32B8 File Offset: 0x000D14B8
	private static string GetPrefix<T>(ref T ctx)
	{
		if (ctx == null)
		{
			return string.Empty;
		}
		Type type = ctx as Type;
		string arg;
		if (type != null)
		{
			arg = type.Name;
		}
		else
		{
			string text = ctx as string;
			if (text != null)
			{
				arg = text;
			}
			else
			{
				arg = ctx.GetType().Name;
			}
		}
		return ZString.Concat<string, string, string>("[", arg, "] ");
	}
}
