using System;
using System.Reflection;
using UnityEngine;

// Token: 0x02000864 RID: 2148
public class OnEnterPlay_SetNew : OnEnterPlay_Attribute
{
	// Token: 0x06003420 RID: 13344 RVA: 0x000F8684 File Offset: 0x000F6884
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
