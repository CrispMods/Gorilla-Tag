using System;
using UnityEngine;

// Token: 0x020001F4 RID: 500
[ExecuteInEditMode]
public class SkyboxSettings : MonoBehaviour
{
	// Token: 0x06000BC7 RID: 3015 RVA: 0x0003E78A File Offset: 0x0003C98A
	private void OnEnable()
	{
		if (this._skyMaterial)
		{
			RenderSettings.skybox = this._skyMaterial;
		}
	}

	// Token: 0x04000E28 RID: 3624
	[SerializeField]
	private Material _skyMaterial;
}
