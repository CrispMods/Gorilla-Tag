﻿using System;
using System.Reflection;
using UnityEngine;

// Token: 0x02000865 RID: 2149
public class OnEnterPlay_SetNull : OnEnterPlay_Attribute
{
	// Token: 0x06003428 RID: 13352 RVA: 0x000518EB File Offset: 0x0004FAEB
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
