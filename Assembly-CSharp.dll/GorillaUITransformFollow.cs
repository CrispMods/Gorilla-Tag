using System;
using UnityEngine;

// Token: 0x020005A7 RID: 1447
public class GorillaUITransformFollow : MonoBehaviour
{
	// Token: 0x060023EC RID: 9196 RVA: 0x0002F75F File Offset: 0x0002D95F
	private void Start()
	{
	}

	// Token: 0x060023ED RID: 9197 RVA: 0x000FF184 File Offset: 0x000FD384
	private void LateUpdate()
	{
		if (this.doesMove)
		{
			base.transform.rotation = this.transformToFollow.rotation;
			base.transform.position = this.transformToFollow.position + this.transformToFollow.rotation * this.offset;
		}
	}

	// Token: 0x040027E9 RID: 10217
	public Transform transformToFollow;

	// Token: 0x040027EA RID: 10218
	public Vector3 offset;

	// Token: 0x040027EB RID: 10219
	public bool doesMove;
}
