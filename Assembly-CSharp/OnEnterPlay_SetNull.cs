using System;
using System.Reflection;
using UnityEngine;

// Token: 0x02000862 RID: 2146
public class OnEnterPlay_SetNull : OnEnterPlay_Attribute
{
	// Token: 0x0600341C RID: 13340 RVA: 0x000F8609 File Offset: 0x000F6809
	public override void OnEnterPlay(FieldInfo field)
	{
		if (!field.IsStatic)
		{
			Debug.LogError(string.Format("Can't SetNull non-static field {0}.{1}", field.DeclaringType, field.Name));
			return;
		}
		field.SetValue(null, null);
	}
}
