using System;
using UnityEngine;

// Token: 0x02000478 RID: 1144
public class GorillaBodyPhysics : MonoBehaviour
{
	// Token: 0x06001C0A RID: 7178 RVA: 0x0004342F File Offset: 0x0004162F
	private void FixedUpdate()
	{
		this.bodyCollider.transform.position = this.headsetTransform.position + this.bodyColliderOffset;
	}

	// Token: 0x04001EE1 RID: 7905
	public GameObject bodyCollider;

	// Token: 0x04001EE2 RID: 7906
	public Vector3 bodyColliderOffset;

	// Token: 0x04001EE3 RID: 7907
	public Transform headsetTransform;
}
