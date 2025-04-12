using System;
using UnityEngine;

// Token: 0x0200087A RID: 2170
public class BezierCurve : MonoBehaviour
{
	// Token: 0x06003478 RID: 13432 RVA: 0x0013B8C8 File Offset: 0x00139AC8
	public Vector3 GetPoint(float t)
	{
		return base.transform.TransformPoint(Bezier.GetPoint(this.points[0], this.points[1], this.points[2], this.points[3], t));
	}

	// Token: 0x06003479 RID: 13433 RVA: 0x0013B918 File Offset: 0x00139B18
	public Vector3 GetVelocity(float t)
	{
		return base.transform.TransformPoint(Bezier.GetFirstDerivative(this.points[0], this.points[1], this.points[2], this.points[3], t)) - base.transform.position;
	}

	// Token: 0x0600347A RID: 13434 RVA: 0x0013B978 File Offset: 0x00139B78
	public Vector3 GetDirection(float t)
	{
		return this.GetVelocity(t).normalized;
	}

	// Token: 0x0600347B RID: 13435 RVA: 0x0013B994 File Offset: 0x00139B94
	public void Reset()
	{
		this.points = new Vector3[]
		{
			new Vector3(1f, 0f, 0f),
			new Vector3(2f, 0f, 0f),
			new Vector3(3f, 0f, 0f),
			new Vector3(4f, 0f, 0f)
		};
	}

	// Token: 0x04003758 RID: 14168
	public Vector3[] points;
}
