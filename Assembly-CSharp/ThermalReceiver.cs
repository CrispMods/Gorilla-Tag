using System;
using GorillaTag;
using UnityEngine;

// Token: 0x02000209 RID: 521
public class ThermalReceiver : MonoBehaviour, IDynamicFloat, IResettableItem
{
	// Token: 0x17000130 RID: 304
	// (get) Token: 0x06000C41 RID: 3137 RVA: 0x00038905 File Offset: 0x00036B05
	public float Farenheit
	{
		get
		{
			return this.celsius * 1.8f + 32f;
		}
	}

	// Token: 0x17000131 RID: 305
	// (get) Token: 0x06000C42 RID: 3138 RVA: 0x00038919 File Offset: 0x00036B19
	public float floatValue
	{
		get
		{
			return this.celsius;
		}
	}

	// Token: 0x06000C43 RID: 3139 RVA: 0x00038921 File Offset: 0x00036B21
	protected void Awake()
	{
		this.defaultCelsius = this.celsius;
	}

	// Token: 0x06000C44 RID: 3140 RVA: 0x0003892F File Offset: 0x00036B2F
	protected void OnEnable()
	{
		ThermalManager.Register(this);
	}

	// Token: 0x06000C45 RID: 3141 RVA: 0x00038937 File Offset: 0x00036B37
	protected void OnDisable()
	{
		ThermalManager.Unregister(this);
	}

	// Token: 0x06000C46 RID: 3142 RVA: 0x0003893F File Offset: 0x00036B3F
	public void ResetToDefaultState()
	{
		this.celsius = this.defaultCelsius;
	}

	// Token: 0x04000E90 RID: 3728
	public float radius = 0.2f;

	// Token: 0x04000E91 RID: 3729
	[Tooltip("How fast the temperature should change overtime. 1.0 would be instantly.")]
	public float conductivity = 0.3f;

	// Token: 0x04000E92 RID: 3730
	[DebugOption]
	public float celsius;

	// Token: 0x04000E93 RID: 3731
	private float defaultCelsius;
}
