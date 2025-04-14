using System;
using System.Reflection;
using UnityEngine;

// Token: 0x02000870 RID: 2160
public class OnExitPlay_Clear : OnExitPlay_Attribute
{
	// Token: 0x0600343E RID: 13374 RVA: 0x000F8E18 File Offset: 0x000F7018
	public override void OnEnterPlay(FieldInfo field)
	{
		if (!field.IsStatic)
		{
			Debug.LogError(string.Format("Can't Clear non-static field {0}.{1}", field.DeclaringType, field.Name));
			return;
		}
		field.FieldType.GetMethod("Clear").Invoke(field.GetValue(null), new object[0]);
	}
}
