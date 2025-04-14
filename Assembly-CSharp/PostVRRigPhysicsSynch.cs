using System;
using UnityEngine;

// Token: 0x020007B1 RID: 1969
public class PostVRRigPhysicsSynch : MonoBehaviour
{
	// Token: 0x06003085 RID: 12421 RVA: 0x000E9AAA File Offset: 0x000E7CAA
	private void LateUpdate()
	{
		Physics.SyncTransforms();
	}
}
