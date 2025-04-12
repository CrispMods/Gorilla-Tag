using System;
using UnityEngine;

// Token: 0x02000470 RID: 1136
public class GorillaHandHistory : MonoBehaviour
{
	// Token: 0x06001BC4 RID: 7108 RVA: 0x00042178 File Offset: 0x00040378
	private void Start()
	{
		this.direction = default(Vector3);
		this.lastPosition = default(Vector3);
	}

	// Token: 0x06001BC5 RID: 7109 RVA: 0x00042192 File Offset: 0x00040392
	private void FixedUpdate()
	{
		this.direction = this.lastPosition - base.transform.position;
		this.lastLastPosition = this.lastPosition;
		this.lastPosition = base.transform.position;
	}

	// Token: 0x04001EA5 RID: 7845
	public Vector3 direction;

	// Token: 0x04001EA6 RID: 7846
	private Vector3 lastPosition;

	// Token: 0x04001EA7 RID: 7847
	private Vector3 lastLastPosition;
}
