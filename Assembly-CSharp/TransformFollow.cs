using System;
using UnityEngine;

// Token: 0x0200064D RID: 1613
public class TransformFollow : MonoBehaviour
{
	// Token: 0x06002801 RID: 10241 RVA: 0x000C3F53 File Offset: 0x000C2153
	private void Awake()
	{
		this.prevPos = base.transform.position;
	}

	// Token: 0x06002802 RID: 10242 RVA: 0x000C3F68 File Offset: 0x000C2168
	private void LateUpdate()
	{
		this.prevPos = base.transform.position;
		base.transform.rotation = this.transformToFollow.rotation;
		base.transform.position = this.transformToFollow.position + this.transformToFollow.rotation * this.offset;
	}

	// Token: 0x04002BE9 RID: 11241
	public Transform transformToFollow;

	// Token: 0x04002BEA RID: 11242
	public Vector3 offset;

	// Token: 0x04002BEB RID: 11243
	public Vector3 prevPos;
}
