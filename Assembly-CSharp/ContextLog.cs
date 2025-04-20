using System;
using System.Runtime.CompilerServices;
using Cysharp.Text;
using UnityEngine;

// Token: 0x020006BA RID: 1722
public static class ContextLog
{
	// Token: 0x06002AE3 RID: 10979 RVA: 0x0004CE98 File Offset: 0x0004B098
	public static void Log<T0, T1>(this T0 ctx, T1 arg1)
	{
		Debug.Log(ZString.Concat<string, T1>(ContextLog.GetPrefix<T0>(ref ctx), arg1));
	}

	// Token: 0x06002AE4 RID: 10980 RVA: 0x0011F7F4 File Offset: 0x0011D9F4
	public static void LogCall<T0, T1>(this T0 ctx, T1 arg1, [CallerMemberName] string call = null)
	{
		string prefix = ContextLog.GetPrefix<T0>(ref ctx);
		string arg2 = ZString.Concat<string, string, string>("{.", call, "()} ");
		Debug.Log(ZString.Concat<string, string, T1>(prefix, arg2, arg1));
	}

	// Token: 0x06002AE5 RID: 10981 RVA: 0x0011F828 File Offset: 0x0011DA28
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
