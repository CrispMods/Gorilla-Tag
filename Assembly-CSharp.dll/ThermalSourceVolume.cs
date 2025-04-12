﻿using System;
using UnityEngine;

// Token: 0x020001FF RID: 511
public class ThermalSourceVolume : MonoBehaviour
{
	// Token: 0x06000BFF RID: 3071 RVA: 0x000376AB File Offset: 0x000358AB
	protected void OnEnable()
	{
		ThermalManager.Register(this);
	}

	// Token: 0x06000C00 RID: 3072 RVA: 0x000376B3 File Offset: 0x000358B3
	protected void OnDisable()
	{
		ThermalManager.Unregister(this);
	}

	// Token: 0x04000E4F RID: 3663
	[Tooltip("Temperature in celsius. Default is 20 which is room temperature.")]
	public float celsius = 20f;

	// Token: 0x04000E50 RID: 3664
	public float innerRadius = 0.1f;

	// Token: 0x04000E51 RID: 3665
	public float outerRadius = 1f;
}
