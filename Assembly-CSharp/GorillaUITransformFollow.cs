using System;
using UnityEngine;

// Token: 0x020005A6 RID: 1446
public class GorillaUITransformFollow : MonoBehaviour
{
	// Token: 0x060023E4 RID: 9188 RVA: 0x000023F4 File Offset: 0x000005F4
	private void Start()
	{
	}

	// Token: 0x060023E5 RID: 9189 RVA: 0x000B3088 File Offset: 0x000B1288
	private void LateUpdate()
	{
		if (this.doesMove)
		{
			base.transform.rotation = this.transformToFollow.rotation;
			base.transform.position = this.transformToFollow.position + this.transformToFollow.rotation * this.offset;
		}
	}

	// Token: 0x040027E3 RID: 10211
	public Transform transformToFollow;

	// Token: 0x040027E4 RID: 10212
	public Vector3 offset;

	// Token: 0x040027E5 RID: 10213
	public bool doesMove;
}
