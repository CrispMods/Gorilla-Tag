using System;
using System.Collections.Generic;

// Token: 0x020001A2 RID: 418
public static class DelegateExtensions
{
	// Token: 0x060009F7 RID: 2551 RVA: 0x00037350 File Offset: 0x00035550
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

	// Token: 0x060009F8 RID: 2552 RVA: 0x000373BD File Offset: 0x000355BD
	public static string ToText(this Delegate[] invocationList)
	{
		return string.Join(", ", invocationList.ToStringList());
	}
}
