using System;
using UnityEngine;

// Token: 0x0200064E RID: 1614
public class TransformFollow : MonoBehaviour
{
	// Token: 0x06002809 RID: 10249 RVA: 0x0004A68E File Offset: 0x0004888E
	private void Awake()
	{
		this.prevPos = base.transform.position;
	}

	// Token: 0x0600280A RID: 10250 RVA: 0x0010CDF4 File Offset: 0x0010AFF4
	private void LateUpdate()
	{
		this.prevPos = base.transform.position;
		base.transform.rotation = this.transformToFollow.rotation;
		base.transform.position = this.transformToFollow.position + this.transformToFollow.rotation * this.offset;
	}

	// Token: 0x04002BEF RID: 11247
	public Transform transformToFollow;

	// Token: 0x04002BF0 RID: 11248
	public Vector3 offset;

	// Token: 0x04002BF1 RID: 11249
	public Vector3 prevPos;
}
