using System;
using UnityEngine;

// Token: 0x0200081E RID: 2078
public class KinematicTestMotion : MonoBehaviour
{
	// Token: 0x060032F3 RID: 13043 RVA: 0x000F400A File Offset: 0x000F220A
	private void FixedUpdate()
	{
		if (this.updateType != KinematicTestMotion.UpdateType.FixedUpdate)
		{
			return;
		}
		this.UpdatePosition(Time.time);
	}

	// Token: 0x060032F4 RID: 13044 RVA: 0x000F4021 File Offset: 0x000F2221
	private void Update()
	{
		if (this.updateType != KinematicTestMotion.UpdateType.Update)
		{
			return;
		}
		this.UpdatePosition(Time.time);
	}

	// Token: 0x060032F5 RID: 13045 RVA: 0x000F4037 File Offset: 0x000F2237
	private void LateUpdate()
	{
		if (this.updateType != KinematicTestMotion.UpdateType.LateUpdate)
		{
			return;
		}
		this.UpdatePosition(Time.time);
	}

	// Token: 0x060032F6 RID: 13046 RVA: 0x000F4050 File Offset: 0x000F2250
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

	// Token: 0x04003669 RID: 13929
	public Transform start;

	// Token: 0x0400366A RID: 13930
	public Transform end;

	// Token: 0x0400366B RID: 13931
	public Rigidbody rigidbody;

	// Token: 0x0400366C RID: 13932
	public KinematicTestMotion.UpdateType updateType;

	// Token: 0x0400366D RID: 13933
	public KinematicTestMotion.MoveType moveType = KinematicTestMotion.MoveType.RigidbodyMovePosition;

	// Token: 0x0400366E RID: 13934
	public float period = 4f;

	// Token: 0x0200081F RID: 2079
	public enum UpdateType
	{
		// Token: 0x04003670 RID: 13936
		Update,
		// Token: 0x04003671 RID: 13937
		LateUpdate,
		// Token: 0x04003672 RID: 13938
		FixedUpdate
	}

	// Token: 0x02000820 RID: 2080
	public enum MoveType
	{
		// Token: 0x04003674 RID: 13940
		TransformPosition,
		// Token: 0x04003675 RID: 13941
		RigidbodyMovePosition
	}
}
