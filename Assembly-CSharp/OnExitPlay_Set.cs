using System;
using System.Reflection;
using UnityEngine;

// Token: 0x02000887 RID: 2183
public class OnExitPlay_Set : OnExitPlay_Attribute
{
	// Token: 0x060034FA RID: 13562 RVA: 0x00052ED1 File Offset: 0x000510D1
	public OnExitPlay_Set(object value)
	{
		this.value = value;
	}

	// Token: 0x060034FB RID: 13563 RVA: 0x00052EE0 File Offset: 0x000510E0
	public override void OnEnterPlay(FieldInfo field)
	{
		if (!field.IsStatic)
		{
			Debug.LogError(string.Format("Can't Set non-static field {0}.{1}", field.DeclaringType, field.Name));
			return;
		}
		field.SetValue(null, this.value);
	}

	// Token: 0x040037DC RID: 14300
	private object value;
}
