using System;
using System.Reflection;

// Token: 0x02000861 RID: 2145
public class OnPlayChange_BaseAttribute : Attribute
{
	// Token: 0x06003419 RID: 13337 RVA: 0x000023F4 File Offset: 0x000005F4
	public virtual void OnEnterPlay(FieldInfo field)
	{
	}

	// Token: 0x0600341A RID: 13338 RVA: 0x000023F4 File Offset: 0x000005F4
	public virtual void OnEnterPlay(MethodInfo method)
	{
	}
}
