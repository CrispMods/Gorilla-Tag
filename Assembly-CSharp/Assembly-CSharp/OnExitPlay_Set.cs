using System;
using System.Reflection;
using UnityEngine;

// Token: 0x0200086E RID: 2158
public class OnExitPlay_Set : OnExitPlay_Attribute
{
	// Token: 0x0600343A RID: 13370 RVA: 0x000F8D7D File Offset: 0x000F6F7D
	public OnExitPlay_Set(object value)
	{
		this.value = value;
	}

	// Token: 0x0600343B RID: 13371 RVA: 0x000F8D8C File Offset: 0x000F6F8C
	public override void OnEnterPlay(FieldInfo field)
	{
		if (!field.IsStatic)
		{
			Debug.LogError(string.Format("Can't Set non-static field {0}.{1}", field.DeclaringType, field.Name));
			return;
		}
		field.SetValue(null, this.value);
	}

	// Token: 0x0400372E RID: 14126
	private object value;
}
