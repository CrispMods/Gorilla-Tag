using System;
using UnityEngine;

// Token: 0x0200047C RID: 1148
public class GorillaHandHistory : MonoBehaviour
{
	// Token: 0x06001C15 RID: 7189 RVA: 0x000434B1 File Offset: 0x000416B1
	private void Start()
	{
		this.direction = default(Vector3);
		this.lastPosition = default(Vector3);
	}

	// Token: 0x06001C16 RID: 7190 RVA: 0x000434CB File Offset: 0x000416CB
	private void FixedUpdate()
	{
		this.direction = this.lastPosition - base.transform.position;
		this.lastLastPosition = this.lastPosition;
		this.lastPosition = base.transform.position;
	}

	// Token: 0x04001EF3 RID: 7923
	public Vector3 direction;

	// Token: 0x04001EF4 RID: 7924
	private Vector3 lastPosition;

	// Token: 0x04001EF5 RID: 7925
	private Vector3 lastLastPosition;
}
