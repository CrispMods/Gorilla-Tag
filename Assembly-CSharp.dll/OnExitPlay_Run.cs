using System;
using System.Reflection;
using UnityEngine;

// Token: 0x02000871 RID: 2161
public class OnExitPlay_Run : OnExitPlay_Attribute
{
	// Token: 0x06003440 RID: 13376 RVA: 0x00051963 File Offset: 0x0004FB63
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
