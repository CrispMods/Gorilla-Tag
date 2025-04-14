using System;
using System.Reflection;

// Token: 0x02000864 RID: 2148
public class OnPlayChange_BaseAttribute : Attribute
{
	// Token: 0x06003425 RID: 13349 RVA: 0x000023F4 File Offset: 0x000005F4
	public virtual void OnEnterPlay(FieldInfo field)
	{
	}

	// Token: 0x06003426 RID: 13350 RVA: 0x000023F4 File Offset: 0x000005F4
	public virtual void OnEnterPlay(MethodInfo method)
	{
	}
}
