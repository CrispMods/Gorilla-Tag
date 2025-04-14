using System;
using System.Reflection;
using UnityEngine;

// Token: 0x0200086D RID: 2157
public class OnExitPlay_Clear : OnExitPlay_Attribute
{
	// Token: 0x06003432 RID: 13362 RVA: 0x000F8850 File Offset: 0x000F6A50
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
