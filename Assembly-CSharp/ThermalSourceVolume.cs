using System;
using UnityEngine;

// Token: 0x020001FF RID: 511
public class ThermalSourceVolume : MonoBehaviour
{
	// Token: 0x06000BFD RID: 3069 RVA: 0x0003F44F File Offset: 0x0003D64F
	protected void OnEnable()
	{
		ThermalManager.Register(this);
	}

	// Token: 0x06000BFE RID: 3070 RVA: 0x0003F457 File Offset: 0x0003D657
	protected void OnDisable()
	{
		ThermalManager.Unregister(this);
	}

	// Token: 0x04000E4E RID: 3662
	[Tooltip("Temperature in celsius. Default is 20 which is room temperature.")]
	public float celsius = 20f;

	// Token: 0x04000E4F RID: 3663
	public float innerRadius = 0.1f;

	// Token: 0x04000E50 RID: 3664
	public float outerRadius = 1f;
}
