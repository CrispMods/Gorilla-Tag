using System;
using System.Reflection;
using UnityEngine;

// Token: 0x0200086E RID: 2158
public class OnExitPlay_Run : OnExitPlay_Attribute
{
	// Token: 0x06003434 RID: 13364 RVA: 0x000F8730 File Offset: 0x000F6930
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
