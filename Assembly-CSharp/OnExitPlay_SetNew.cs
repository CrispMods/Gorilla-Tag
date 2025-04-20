using System;
using System.Reflection;
using UnityEngine;

// Token: 0x02000888 RID: 2184
public class OnExitPlay_SetNew : OnExitPlay_Attribute
{
	// Token: 0x060034FC RID: 13564 RVA: 0x0013F80C File Offset: 0x0013DA0C
	public override void OnEnterPlay(FieldInfo field)
	{
		if (!field.IsStatic)
		{
			Debug.LogError(string.Format("Can't SetNew non-static field {0}.{1}", field.DeclaringType, field.Name));
			return;
		}
		object value = field.FieldType.GetConstructor(new Type[0]).Invoke(new object[0]);
		field.SetValue(null, value);
	}
}
