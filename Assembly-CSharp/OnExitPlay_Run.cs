using System;
using System.Reflection;
using UnityEngine;

// Token: 0x0200088A RID: 2186
public class OnExitPlay_Run : OnExitPlay_Attribute
{
	// Token: 0x06003500 RID: 13568 RVA: 0x00052E70 File Offset: 0x00051070
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
