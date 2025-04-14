using System;
using UnityEngine;

// Token: 0x02000877 RID: 2167
public class BezierCurve : MonoBehaviour
{
	// Token: 0x0600346C RID: 13420 RVA: 0x000FA090 File Offset: 0x000F8290
	public Vector3 GetPoint(float t)
	{
		return base.transform.TransformPoint(Bezier.GetPoint(this.points[0], this.points[1], this.points[2], this.points[3], t));
	}

	// Token: 0x0600346D RID: 13421 RVA: 0x000FA0E0 File Offset: 0x000F82E0
	public Vector3 GetVelocity(float t)
	{
		return base.transform.TransformPoint(Bezier.GetFirstDerivative(this.points[0], this.points[1], this.points[2], this.points[3], t)) - base.transform.position;
	}

	// Token: 0x0600346E RID: 13422 RVA: 0x000FA140 File Offset: 0x000F8340
	public Vector3 GetDirection(float t)
	{
		return this.GetVelocity(t).normalized;
	}

	// Token: 0x0600346F RID: 13423 RVA: 0x000FA15C File Offset: 0x000F835C
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

	// Token: 0x04003746 RID: 14150
	public Vector3[] points;
}
