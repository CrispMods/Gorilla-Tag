using System;
using System.Reflection;
using UnityEngine;

// Token: 0x02000882 RID: 2178
public class OnEnterPlay_Run : OnEnterPlay_Attribute
{
	// Token: 0x060034F0 RID: 13552 RVA: 0x00052E70 File Offset: 0x00051070
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
