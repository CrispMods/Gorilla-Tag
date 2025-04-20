using System;
using UnityEngine;

// Token: 0x0200020A RID: 522
public class ThermalSourceVolume : MonoBehaviour
{
	// Token: 0x06000C48 RID: 3144 RVA: 0x0003896B File Offset: 0x00036B6B
	protected void OnEnable()
	{
		ThermalManager.Register(this);
	}

	// Token: 0x06000C49 RID: 3145 RVA: 0x00038973 File Offset: 0x00036B73
	protected void OnDisable()
	{
		ThermalManager.Unregister(this);
	}

	// Token: 0x04000E94 RID: 3732
	[Tooltip("Temperature in celsius. Default is 20 which is room temperature.")]
	public float celsius = 20f;

	// Token: 0x04000E95 RID: 3733
	public float innerRadius = 0.1f;

	// Token: 0x04000E96 RID: 3734
	public float outerRadius = 1f;
}
