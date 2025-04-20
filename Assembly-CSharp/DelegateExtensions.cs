using System;
using System.Collections.Generic;

// Token: 0x020001AD RID: 429
public static class DelegateExtensions
{
	// Token: 0x06000A43 RID: 2627 RVA: 0x000971DC File Offset: 0x000953DC
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

	// Token: 0x06000A44 RID: 2628 RVA: 0x0003733B File Offset: 0x0003553B
	public static string ToText(this Delegate[] invocationList)
	{
		return string.Join(", ", invocationList.ToStringList());
	}
}
