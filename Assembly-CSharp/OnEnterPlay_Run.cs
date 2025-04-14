using System;
using System.Reflection;
using UnityEngine;

// Token: 0x02000866 RID: 2150
public class OnEnterPlay_Run : OnEnterPlay_Attribute
{
	// Token: 0x06003424 RID: 13348 RVA: 0x000F8730 File Offset: 0x000F6930
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
