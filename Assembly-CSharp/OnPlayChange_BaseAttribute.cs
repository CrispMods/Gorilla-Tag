using System;
using System.Reflection;

// Token: 0x0200087D RID: 2173
public class OnPlayChange_BaseAttribute : Attribute
{
	// Token: 0x060034E5 RID: 13541 RVA: 0x00030607 File Offset: 0x0002E807
	public virtual void OnEnterPlay(FieldInfo field)
	{
	}

	// Token: 0x060034E6 RID: 13542 RVA: 0x00030607 File Offset: 0x0002E807
	public virtual void OnEnterPlay(MethodInfo method)
	{
	}
}
