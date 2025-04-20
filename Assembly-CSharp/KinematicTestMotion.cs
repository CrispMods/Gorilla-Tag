using System;
using UnityEngine;

// Token: 0x02000838 RID: 2104
public class KinematicTestMotion : MonoBehaviour
{
	// Token: 0x060033AE RID: 13230 RVA: 0x00051FC0 File Offset: 0x000501C0
	private void FixedUpdate()
	{
		if (this.updateType != KinematicTestMotion.UpdateType.FixedUpdate)
		{
			return;
		}
		this.UpdatePosition(Time.time);
	}

	// Token: 0x060033AF RID: 13231 RVA: 0x00051FD7 File Offset: 0x000501D7
	private void Update()
	{
		if (this.updateType != KinematicTestMotion.UpdateType.Update)
		{
			return;
		}
		this.UpdatePosition(Time.time);
	}

	// Token: 0x060033B0 RID: 13232 RVA: 0x00051FED File Offset: 0x000501ED
	private void LateUpdate()
	{
		if (this.updateType != KinematicTestMotion.UpdateType.LateUpdate)
		{
			return;
		}
		this.UpdatePosition(Time.time);
	}

	// Token: 0x060033B1 RID: 13233 RVA: 0x0013BF90 File Offset: 0x0013A190
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

	// Token: 0x04003725 RID: 14117
	public Transform start;

	// Token: 0x04003726 RID: 14118
	public Transform end;

	// Token: 0x04003727 RID: 14119
	public Rigidbody rigidbody;

	// Token: 0x04003728 RID: 14120
	public KinematicTestMotion.UpdateType updateType;

	// Token: 0x04003729 RID: 14121
	public KinematicTestMotion.MoveType moveType = KinematicTestMotion.MoveType.RigidbodyMovePosition;

	// Token: 0x0400372A RID: 14122
	public float period = 4f;

	// Token: 0x02000839 RID: 2105
	public enum UpdateType
	{
		// Token: 0x0400372C RID: 14124
		Update,
		// Token: 0x0400372D RID: 14125
		LateUpdate,
		// Token: 0x0400372E RID: 14126
		FixedUpdate
	}

	// Token: 0x0200083A RID: 2106
	public enum MoveType
	{
		// Token: 0x04003730 RID: 14128
		TransformPosition,
		// Token: 0x04003731 RID: 14129
		RigidbodyMovePosition
	}
}
