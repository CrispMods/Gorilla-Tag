using System;
using UnityEngine;

// Token: 0x020006DB RID: 1755
public class MainCamera : MonoBehaviourStatic<MainCamera>
{
	// Token: 0x06002B7C RID: 11132 RVA: 0x0004D5F9 File Offset: 0x0004B7F9
	public static implicit operator Camera(MainCamera mc)
	{
		return mc.camera;
	}

	// Token: 0x0400311D RID: 12573
	public Camera camera;
}
