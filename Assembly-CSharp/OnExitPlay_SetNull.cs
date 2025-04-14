using System;
using System.Reflection;
using UnityEngine;

// Token: 0x0200086A RID: 2154
public class OnExitPlay_SetNull : OnExitPlay_Attribute
{
	// Token: 0x0600342C RID: 13356 RVA: 0x000F8609 File Offset: 0x000F6809
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
