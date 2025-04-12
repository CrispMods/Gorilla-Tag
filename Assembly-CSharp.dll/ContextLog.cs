using System;
using System.Runtime.CompilerServices;
using Cysharp.Text;
using UnityEngine;

// Token: 0x020006A6 RID: 1702
public static class ContextLog
{
	// Token: 0x06002A55 RID: 10837 RVA: 0x0004BB53 File Offset: 0x00049D53
	public static void Log<T0, T1>(this T0 ctx, T1 arg1)
	{
		Debug.Log(ZString.Concat<string, T1>(ContextLog.GetPrefix<T0>(ref ctx), arg1));
	}

	// Token: 0x06002A56 RID: 10838 RVA: 0x0011AC3C File Offset: 0x00118E3C
	public static void LogCall<T0, T1>(this T0 ctx, T1 arg1, [CallerMemberName] string call = null)
	{
		string prefix = ContextLog.GetPrefix<T0>(ref ctx);
		string arg2 = ZString.Concat<string, string, string>("{.", call, "()} ");
		Debug.Log(ZString.Concat<string, string, T1>(prefix, arg2, arg1));
	}

	// Token: 0x06002A57 RID: 10839 RVA: 0x0011AC70 File Offset: 0x00118E70
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
