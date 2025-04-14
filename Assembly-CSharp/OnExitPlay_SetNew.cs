using System;
using System.Reflection;
using UnityEngine;

// Token: 0x0200086C RID: 2156
public class OnExitPlay_SetNew : OnExitPlay_Attribute
{
	// Token: 0x06003430 RID: 13360 RVA: 0x000F87F8 File Offset: 0x000F69F8
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
