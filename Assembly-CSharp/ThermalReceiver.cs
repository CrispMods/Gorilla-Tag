using System;
using GorillaTag;
using UnityEngine;

// Token: 0x020001FE RID: 510
public class ThermalReceiver : MonoBehaviour, IDynamicFloat, IResettableItem
{
	// Token: 0x17000129 RID: 297
	// (get) Token: 0x06000BF6 RID: 3062 RVA: 0x0003F3E9 File Offset: 0x0003D5E9
	public float Farenheit
	{
		get
		{
			return this.celsius * 1.8f + 32f;
		}
	}

	// Token: 0x1700012A RID: 298
	// (get) Token: 0x06000BF7 RID: 3063 RVA: 0x0003F3FD File Offset: 0x0003D5FD
	public float floatValue
	{
		get
		{
			return this.celsius;
		}
	}

	// Token: 0x06000BF8 RID: 3064 RVA: 0x0003F405 File Offset: 0x0003D605
	protected void Awake()
	{
		this.defaultCelsius = this.celsius;
	}

	// Token: 0x06000BF9 RID: 3065 RVA: 0x0003F413 File Offset: 0x0003D613
	protected void OnEnable()
	{
		ThermalManager.Register(this);
	}

	// Token: 0x06000BFA RID: 3066 RVA: 0x0003F41B File Offset: 0x0003D61B
	protected void OnDisable()
	{
		ThermalManager.Unregister(this);
	}

	// Token: 0x06000BFB RID: 3067 RVA: 0x0003F423 File Offset: 0x0003D623
	public void ResetToDefaultState()
	{
		this.celsius = this.defaultCelsius;
	}

	// Token: 0x04000E4A RID: 3658
	public float radius = 0.2f;

	// Token: 0x04000E4B RID: 3659
	[Tooltip("How fast the temperature should change overtime. 1.0 would be instantly.")]
	public float conductivity = 0.3f;

	// Token: 0x04000E4C RID: 3660
	[DebugOption]
	public float celsius;

	// Token: 0x04000E4D RID: 3661
	private float defaultCelsius;
}
