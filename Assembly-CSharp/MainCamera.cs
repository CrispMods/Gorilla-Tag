using System;
using UnityEngine;

// Token: 0x020006C6 RID: 1734
public class MainCamera : MonoBehaviourStatic<MainCamera>
{
	// Token: 0x06002AE6 RID: 10982 RVA: 0x000D4E1D File Offset: 0x000D301D
	public static implicit operator Camera(MainCamera mc)
	{
		return mc.camera;
	}

	// Token: 0x04003080 RID: 12416
	public Camera camera;
}
