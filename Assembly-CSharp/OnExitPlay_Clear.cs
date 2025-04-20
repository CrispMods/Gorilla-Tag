using System;
using System.Reflection;
using UnityEngine;

// Token: 0x02000889 RID: 2185
public class OnExitPlay_Clear : OnExitPlay_Attribute
{
	// Token: 0x060034FE RID: 13566 RVA: 0x0013F864 File Offset: 0x0013DA64
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
