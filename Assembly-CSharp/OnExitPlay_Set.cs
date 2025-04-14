using System;
using System.Reflection;
using UnityEngine;

// Token: 0x0200086B RID: 2155
public class OnExitPlay_Set : OnExitPlay_Attribute
{
	// Token: 0x0600342E RID: 13358 RVA: 0x000F87B5 File Offset: 0x000F69B5
	public OnExitPlay_Set(object value)
	{
		this.value = value;
	}

	// Token: 0x0600342F RID: 13359 RVA: 0x000F87C4 File Offset: 0x000F69C4
	public override void OnEnterPlay(FieldInfo field)
	{
		if (!field.IsStatic)
		{
			Debug.LogError(string.Format("Can't Set non-static field {0}.{1}", field.DeclaringType, field.Name));
			return;
		}
		field.SetValue(null, this.value);
	}

	// Token: 0x0400371C RID: 14108
	private object value;
}
