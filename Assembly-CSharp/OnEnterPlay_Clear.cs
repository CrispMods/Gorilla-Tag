using System;
using System.Reflection;
using UnityEngine;

// Token: 0x02000865 RID: 2149
public class OnEnterPlay_Clear : OnEnterPlay_Attribute
{
	// Token: 0x06003422 RID: 13346 RVA: 0x000F86DC File Offset: 0x000F68DC
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
