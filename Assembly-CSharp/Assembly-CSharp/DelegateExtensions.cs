using System;
using System.Collections.Generic;

// Token: 0x020001A2 RID: 418
public static class DelegateExtensions
{
	// Token: 0x060009F9 RID: 2553 RVA: 0x00037674 File Offset: 0x00035874
	public static List<string> ToStringList(this Delegate[] invocationList)
	{
		List<string> list = new List<string>();
		if (invocationList != null)
		{
			foreach (Delegate @delegate in invocationList)
			{
				string name = @delegate.Method.Name;
				string str = (@delegate.Target != null) ? @delegate.Target.GetType().FullName : "Static Method";
				list.Add(str + "." + name);
			}
		}
		return list;
	}

	// Token: 0x060009FA RID: 2554 RVA: 0x000376E1 File Offset: 0x000358E1
	public static string ToText(this Delegate[] invocationList)
	{
		return string.Join(", ", invocationList.ToStringList());
	}
}
