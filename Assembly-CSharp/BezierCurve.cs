using System;
using UnityEngine;

// Token: 0x02000893 RID: 2195
public class BezierCurve : MonoBehaviour
{
	// Token: 0x06003538 RID: 13624 RVA: 0x00140EB0 File Offset: 0x0013F0B0
	public Vector3 GetPoint(float t)
	{
		return base.transform.TransformPoint(Bezier.GetPoint(this.points[0], this.points[1], this.points[2], this.points[3], t));
	}

	// Token: 0x06003539 RID: 13625 RVA: 0x00140F00 File Offset: 0x0013F100
	public Vector3 GetVelocity(float t)
	{
		return base.transform.TransformPoint(Bezier.GetFirstDerivative(this.points[0], this.points[1], this.points[2], this.points[3], t)) - base.transform.position;
	}

	// Token: 0x0600353A RID: 13626 RVA: 0x00140F60 File Offset: 0x0013F160
	public Vector3 GetDirection(float t)
	{
		return this.GetVelocity(t).normalized;
	}

	// Token: 0x0600353B RID: 13627 RVA: 0x00140F7C File Offset: 0x0013F17C
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

	// Token: 0x04003806 RID: 14342
	public Vector3[] points;
}
