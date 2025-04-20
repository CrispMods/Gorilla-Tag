using System;
using System.Reflection;
using UnityEngine;

// Token: 0x02000881 RID: 2177
public class OnEnterPlay_Clear : OnEnterPlay_Attribute
{
	// Token: 0x060034EE RID: 13550 RVA: 0x0013F864 File Offset: 0x0013DA64
	public override void OnEnterPlay(FieldInfo field)
	{
		if (!field.IsStatic)
		{
			Debug.LogError(string.Format("Can't Clear non-static field {0}.{1}", field.DeclaringType, field.Name));
			return;
		}
		field.FieldType.GetMethod("Clear").Invoke(field.GetValue(null), new object[0]);
	}
}
