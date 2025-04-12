using System;
using System.Reflection;
using UnityEngine;

// Token: 0x02000869 RID: 2153
public class OnEnterPlay_Run : OnEnterPlay_Attribute
{
	// Token: 0x06003430 RID: 13360 RVA: 0x00051963 File Offset: 0x0004FB63
	public override void OnEnterPlay(MethodInfo method)
	{
		if (!method.IsStatic)
		{
			Debug.LogError(string.Format("Can't Run non-static method {0}.{1}", method.DeclaringType, method.Name));
			return;
		}
		method.Invoke(null, new object[0]);
	}
}
