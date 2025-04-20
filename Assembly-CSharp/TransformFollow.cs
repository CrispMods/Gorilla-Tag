using System;
using UnityEngine;

// Token: 0x0200062C RID: 1580
public class TransformFollow : MonoBehaviour
{
	// Token: 0x0600272C RID: 10028 RVA: 0x0004AC23 File Offset: 0x00048E23
	private void Awake()
	{
		this.prevPos = base.transform.position;
	}

	// Token: 0x0600272D RID: 10029 RVA: 0x0010B21C File Offset: 0x0010941C
	private void LateUpdate()
	{
		this.prevPos = base.transform.position;
		base.transform.rotation = this.transformToFollow.rotation;
		base.transform.position = this.transformToFollow.position + this.transformToFollow.rotation * this.offset;
	}

	// Token: 0x04002B4F RID: 11087
	public Transform transformToFollow;

	// Token: 0x04002B50 RID: 11088
	public Vector3 offset;

	// Token: 0x04002B51 RID: 11089
	public Vector3 prevPos;
}
