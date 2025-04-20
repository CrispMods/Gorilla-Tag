using System;
using System.Reflection;
using UnityEngine;

// Token: 0x02000886 RID: 2182
public class OnExitPlay_SetNull : OnExitPlay_Attribute
{
	// Token: 0x060034F8 RID: 13560 RVA: 0x00052DF8 File Offset: 0x00050FF8
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
