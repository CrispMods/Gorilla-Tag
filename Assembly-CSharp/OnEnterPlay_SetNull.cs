using System;
using System.Reflection;
using UnityEngine;

// Token: 0x0200087E RID: 2174
public class OnEnterPlay_SetNull : OnEnterPlay_Attribute
{
	// Token: 0x060034E8 RID: 13544 RVA: 0x00052DF8 File Offset: 0x00050FF8
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
