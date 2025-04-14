using System;
using UnityEngine;

// Token: 0x02000470 RID: 1136
public class GorillaHandHistory : MonoBehaviour
{
	// Token: 0x06001BC1 RID: 7105 RVA: 0x00087A98 File Offset: 0x00085C98
	private void Start()
	{
		this.direction = default(Vector3);
		this.lastPosition = default(Vector3);
	}

	// Token: 0x06001BC2 RID: 7106 RVA: 0x00087AB2 File Offset: 0x00085CB2
	private void FixedUpdate()
	{
		this.direction = this.lastPosition - base.transform.position;
		this.lastLastPosition = this.lastPosition;
		this.lastPosition = base.transform.position;
	}

	// Token: 0x04001EA4 RID: 7844
	public Vector3 direction;

	// Token: 0x04001EA5 RID: 7845
	private Vector3 lastPosition;

	// Token: 0x04001EA6 RID: 7846
	private Vector3 lastLastPosition;
}
