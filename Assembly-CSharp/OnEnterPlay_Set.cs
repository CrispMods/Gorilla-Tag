using System;
using System.Reflection;
using UnityEngine;

// Token: 0x02000863 RID: 2147
public class OnEnterPlay_Set : OnEnterPlay_Attribute
{
	// Token: 0x0600341E RID: 13342 RVA: 0x000F863F File Offset: 0x000F683F
	public OnEnterPlay_Set(object value)
	{
		this.value = value;
	}

	// Token: 0x0600341F RID: 13343 RVA: 0x000F864E File Offset: 0x000F684E
	public override void OnEnterPlay(FieldInfo field)
	{
		if (!field.IsStatic)
		{
			Debug.LogError(string.Format("Can't Set non-static field {0}.{1}", field.DeclaringType, field.Name));
			return;
		}
		field.SetValue(null, this.value);
	}

	// Token: 0x04003719 RID: 14105
	private object value;
}
