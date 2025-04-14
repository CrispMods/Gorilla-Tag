using System;
using UnityEngine;

// Token: 0x020007B2 RID: 1970
public class PostVRRigPhysicsSynch : MonoBehaviour
{
	// Token: 0x0600308D RID: 12429 RVA: 0x000E9F2A File Offset: 0x000E812A
	private void LateUpdate()
	{
		Physics.SyncTransforms();
	}
}
