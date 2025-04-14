using System;
using UnityEngine;

// Token: 0x020006C7 RID: 1735
public class MainCamera : MonoBehaviourStatic<MainCamera>
{
	// Token: 0x06002AEE RID: 10990 RVA: 0x000D529D File Offset: 0x000D349D
	public static implicit operator Camera(MainCamera mc)
	{
		return mc.camera;
	}

	// Token: 0x04003086 RID: 12422
	public Camera camera;
}
