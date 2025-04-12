using System;
using System.Reflection;
using UnityEngine;

// Token: 0x02000867 RID: 2151
public class OnEnterPlay_SetNew : OnEnterPlay_Attribute
{
	// Token: 0x0600342C RID: 13356 RVA: 0x0013A224 File Offset: 0x00138424
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
