using System;
using System.Reflection;
using UnityEngine;

// Token: 0x0200086D RID: 2157
public class OnExitPlay_SetNull : OnExitPlay_Attribute
{
	// Token: 0x06003438 RID: 13368 RVA: 0x000518EB File Offset: 0x0004FAEB
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
