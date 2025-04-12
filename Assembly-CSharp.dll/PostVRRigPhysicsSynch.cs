using System;
using UnityEngine;

// Token: 0x020007B2 RID: 1970
public class PostVRRigPhysicsSynch : MonoBehaviour
{
	// Token: 0x0600308D RID: 12429 RVA: 0x0004F54C File Offset: 0x0004D74C
	private void LateUpdate()
	{
		Physics.SyncTransforms();
	}
}
