using System;
using UnityEngine;

// Token: 0x020001FF RID: 511
[ExecuteInEditMode]
public class SkyboxSettings : MonoBehaviour
{
	// Token: 0x06000C12 RID: 3090 RVA: 0x0003871F File Offset: 0x0003691F
	private void OnEnable()
	{
		if (this._skyMaterial)
		{
			RenderSettings.skybox = this._skyMaterial;
		}
	}

	// Token: 0x04000E6E RID: 3694
	[SerializeField]
	private Material _skyMaterial;
}
