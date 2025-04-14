using System;
using System.Reflection;
using UnityEngine;

// Token: 0x02000868 RID: 2152
public class OnEnterPlay_Clear : OnEnterPlay_Attribute
{
	// Token: 0x0600342E RID: 13358 RVA: 0x000F8CA4 File Offset: 0x000F6EA4
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
