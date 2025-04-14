using System;
using UnityEngine;

// Token: 0x020005A7 RID: 1447
public class GorillaUITransformFollow : MonoBehaviour
{
	// Token: 0x060023EC RID: 9196 RVA: 0x000023F4 File Offset: 0x000005F4
	private void Start()
	{
	}

	// Token: 0x060023ED RID: 9197 RVA: 0x000B3508 File Offset: 0x000B1708
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
