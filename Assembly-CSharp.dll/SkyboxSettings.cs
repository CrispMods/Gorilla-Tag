using System;
using UnityEngine;

// Token: 0x020001F4 RID: 500
[ExecuteInEditMode]
public class SkyboxSettings : MonoBehaviour
{
	// Token: 0x06000BC9 RID: 3017 RVA: 0x0003745F File Offset: 0x0003565F
	private void OnEnable()
	{
		if (this._skyMaterial)
		{
			RenderSettings.skybox = this._skyMaterial;
		}
	}

	// Token: 0x04000E29 RID: 3625
	[SerializeField]
	private Material _skyMaterial;
}
