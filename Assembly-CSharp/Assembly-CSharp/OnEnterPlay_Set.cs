using System;
using System.Reflection;
using UnityEngine;

// Token: 0x02000866 RID: 2150
public class OnEnterPlay_Set : OnEnterPlay_Attribute
{
	// Token: 0x0600342A RID: 13354 RVA: 0x000F8C07 File Offset: 0x000F6E07
	public OnEnterPlay_Set(object value)
	{
		this.value = value;
	}

	// Token: 0x0600342B RID: 13355 RVA: 0x000F8C16 File Offset: 0x000F6E16
	public override void OnEnterPlay(FieldInfo field)
	{
		if (!field.IsStatic)
		{
			Debug.LogError(string.Format("Can't Set non-static field {0}.{1}", field.DeclaringType, field.Name));
			return;
		}
		field.SetValue(null, this.value);
	}

	// Token: 0x0400372B RID: 14123
	private object value;
}
