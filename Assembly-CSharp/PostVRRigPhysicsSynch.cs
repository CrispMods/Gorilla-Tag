using System;
using UnityEngine;

// Token: 0x020007C9 RID: 1993
public class PostVRRigPhysicsSynch : MonoBehaviour
{
	// Token: 0x06003137 RID: 12599 RVA: 0x0005094E File Offset: 0x0004EB4E
	private void LateUpdate()
	{
		Physics.SyncTransforms();
	}
}
