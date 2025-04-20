using System;
using System.Reflection;
using UnityEngine;

// Token: 0x0200087F RID: 2175
public class OnEnterPlay_Set : OnEnterPlay_Attribute
{
	// Token: 0x060034EA RID: 13546 RVA: 0x00052E2E File Offset: 0x0005102E
	public OnEnterPlay_Set(object value)
	{
		this.value = value;
	}

	// Token: 0x060034EB RID: 13547 RVA: 0x00052E3D File Offset: 0x0005103D
	public override void OnEnterPlay(FieldInfo field)
	{
		if (!field.IsStatic)
		{
			Debug.LogError(string.Format("Can't Set non-static field {0}.{1}", field.DeclaringType, field.Name));
			return;
		}
		field.SetValue(null, this.value);
	}

	// Token: 0x040037D9 RID: 14297
	private object value;
}
