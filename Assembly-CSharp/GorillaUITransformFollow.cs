using System;
using UnityEngine;

// Token: 0x020005B4 RID: 1460
public class GorillaUITransformFollow : MonoBehaviour
{
	// Token: 0x06002444 RID: 9284 RVA: 0x00030607 File Offset: 0x0002E807
	private void Start()
	{
	}

	// Token: 0x06002445 RID: 9285 RVA: 0x00101FB8 File Offset: 0x001001B8
	private void LateUpdate()
	{
		if (this.doesMove)
		{
			base.transform.rotation = this.transformToFollow.rotation;
			base.transform.position = this.transformToFollow.position + this.transformToFollow.rotation * this.offset;
		}
	}

	// Token: 0x0400283F RID: 10303
	public Transform transformToFollow;

	// Token: 0x04002840 RID: 10304
	public Vector3 offset;

	// Token: 0x04002841 RID: 10305
	public bool doesMove;
}
