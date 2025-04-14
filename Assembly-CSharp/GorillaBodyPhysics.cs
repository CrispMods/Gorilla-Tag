using System;
using UnityEngine;

// Token: 0x0200046C RID: 1132
public class GorillaBodyPhysics : MonoBehaviour
{
	// Token: 0x06001BB6 RID: 7094 RVA: 0x00087860 File Offset: 0x00085A60
	private void FixedUpdate()
	{
		this.bodyCollider.transform.position = this.headsetTransform.position + this.bodyColliderOffset;
	}

	// Token: 0x04001E92 RID: 7826
	public GameObject bodyCollider;

	// Token: 0x04001E93 RID: 7827
	public Vector3 bodyColliderOffset;

	// Token: 0x04001E94 RID: 7828
	public Transform headsetTransform;
}
