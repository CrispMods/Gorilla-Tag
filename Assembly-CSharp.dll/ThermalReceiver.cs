﻿using System;
using GorillaTag;
using UnityEngine;

// Token: 0x020001FE RID: 510
public class ThermalReceiver : MonoBehaviour, IDynamicFloat, IResettableItem
{
	// Token: 0x17000129 RID: 297
	// (get) Token: 0x06000BF8 RID: 3064 RVA: 0x00037645 File Offset: 0x00035845
	public float Farenheit
	{
		get
		{
			return this.celsius * 1.8f + 32f;
		}
	}

	// Token: 0x1700012A RID: 298
	// (get) Token: 0x06000BF9 RID: 3065 RVA: 0x00037659 File Offset: 0x00035859
	public float floatValue
	{
		get
		{
			return this.celsius;
		}
	}

	// Token: 0x06000BFA RID: 3066 RVA: 0x00037661 File Offset: 0x00035861
	protected void Awake()
	{
		this.defaultCelsius = this.celsius;
	}

	// Token: 0x06000BFB RID: 3067 RVA: 0x0003766F File Offset: 0x0003586F
	protected void OnEnable()
	{
		ThermalManager.Register(this);
	}

	// Token: 0x06000BFC RID: 3068 RVA: 0x00037677 File Offset: 0x00035877
	protected void OnDisable()
	{
		ThermalManager.Unregister(this);
	}

	// Token: 0x06000BFD RID: 3069 RVA: 0x0003767F File Offset: 0x0003587F
	public void ResetToDefaultState()
	{
		this.celsius = this.defaultCelsius;
	}

	// Token: 0x04000E4B RID: 3659
	public float radius = 0.2f;

	// Token: 0x04000E4C RID: 3660
	[Tooltip("How fast the temperature should change overtime. 1.0 would be instantly.")]
	public float conductivity = 0.3f;

	// Token: 0x04000E4D RID: 3661
	[DebugOption]
	public float celsius;

	// Token: 0x04000E4E RID: 3662
	private float defaultCelsius;
}
