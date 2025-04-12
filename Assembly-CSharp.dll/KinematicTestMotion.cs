using System;
using UnityEngine;

// Token: 0x02000821 RID: 2081
public class KinematicTestMotion : MonoBehaviour
{
	// Token: 0x060032FF RID: 13055 RVA: 0x00050BB2 File Offset: 0x0004EDB2
	private void FixedUpdate()
	{
		if (this.updateType != KinematicTestMotion.UpdateType.FixedUpdate)
		{
			return;
		}
		this.UpdatePosition(Time.time);
	}

	// Token: 0x06003300 RID: 13056 RVA: 0x00050BC9 File Offset: 0x0004EDC9
	private void Update()
	{
		if (this.updateType != KinematicTestMotion.UpdateType.Update)
		{
			return;
		}
		this.UpdatePosition(Time.time);
	}

	// Token: 0x06003301 RID: 13057 RVA: 0x00050BDF File Offset: 0x0004EDDF
	private void LateUpdate()
	{
		if (this.updateType != KinematicTestMotion.UpdateType.LateUpdate)
		{
			return;
		}
		this.UpdatePosition(Time.time);
	}

	// Token: 0x06003302 RID: 13058 RVA: 0x00136A38 File Offset: 0x00134C38
	private void UpdatePosition(float time)
	{
		float t = Mathf.Sin(time * 2f * 3.1415927f * this.period) * 0.5f + 0.5f;
		Vector3 position = Vector3.Lerp(this.start.position, this.end.position, t);
		if (this.moveType == KinematicTestMotion.MoveType.TransformPosition)
		{
			base.transform.position = position;
			return;
		}
		if (this.moveType == KinematicTestMotion.MoveType.RigidbodyMovePosition)
		{
			this.rigidbody.MovePosition(position);
		}
	}

	// Token: 0x0400367B RID: 13947
	public Transform start;

	// Token: 0x0400367C RID: 13948
	public Transform end;

	// Token: 0x0400367D RID: 13949
	public Rigidbody rigidbody;

	// Token: 0x0400367E RID: 13950
	public KinematicTestMotion.UpdateType updateType;

	// Token: 0x0400367F RID: 13951
	public KinematicTestMotion.MoveType moveType = KinematicTestMotion.MoveType.RigidbodyMovePosition;

	// Token: 0x04003680 RID: 13952
	public float period = 4f;

	// Token: 0x02000822 RID: 2082
	public enum UpdateType
	{
		// Token: 0x04003682 RID: 13954
		Update,
		// Token: 0x04003683 RID: 13955
		LateUpdate,
		// Token: 0x04003684 RID: 13956
		FixedUpdate
	}

	// Token: 0x02000823 RID: 2083
	public enum MoveType
	{
		// Token: 0x04003686 RID: 13958
		TransformPosition,
		// Token: 0x04003687 RID: 13959
		RigidbodyMovePosition
	}
}
