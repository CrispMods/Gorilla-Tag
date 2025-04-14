using System;
using UnityEngine;

// Token: 0x0200046C RID: 1132
public class GorillaBodyPhysics : MonoBehaviour
{
	// Token: 0x06001BB9 RID: 7097 RVA: 0x00087BE4 File Offset: 0x00085DE4
	private void FixedUpdate()
	{
		this.bodyCollider.transform.position = this.headsetTransform.position + this.bodyColliderOffset;
	}

	// Token: 0x04001E93 RID: 7827
	public GameObject bodyCollider;

	// Token: 0x04001E94 RID: 7828
	public Vector3 bodyColliderOffset;

	// Token: 0x04001E95 RID: 7829
	public Transform headsetTransform;
}
