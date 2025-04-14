using System;
using UnityEngine;
using UnityEngine.Events;

// Token: 0x02000637 RID: 1591
public class SnapXformToLine : MonoBehaviour
{
	// Token: 0x17000429 RID: 1065
	// (get) Token: 0x06002784 RID: 10116 RVA: 0x000C1607 File Offset: 0x000BF807
	public Vector3 linePoint
	{
		get
		{
			return this._closest;
		}
	}

	// Token: 0x1700042A RID: 1066
	// (get) Token: 0x06002785 RID: 10117 RVA: 0x000C160F File Offset: 0x000BF80F
	public float linearDistance
	{
		get
		{
			return this._linear;
		}
	}

	// Token: 0x06002786 RID: 10118 RVA: 0x000C1617 File Offset: 0x000BF817
	public void SnapTarget(bool applyToXform = true)
	{
		this.Snap(this.target, true);
	}

	// Token: 0x06002787 RID: 10119 RVA: 0x000C1626 File Offset: 0x000BF826
	public void SnapTarget(Vector3 point)
	{
		if (this.target)
		{
			this.target.position = this.GetSnappedPoint(this.target.position);
		}
	}

	// Token: 0x06002788 RID: 10120 RVA: 0x000C1654 File Offset: 0x000BF854
	public void SnapTargetLinear(float t)
	{
		if (this.target && this.from && this.to)
		{
			this.target.position = Vector3.Lerp(this.from.position, this.to.position, t);
		}
	}

	// Token: 0x06002789 RID: 10121 RVA: 0x000C16AF File Offset: 0x000BF8AF
	public Vector3 GetSnappedPoint(Transform t)
	{
		return this.GetSnappedPoint(t.position);
	}

	// Token: 0x0600278A RID: 10122 RVA: 0x000C16C0 File Offset: 0x000BF8C0
	public Vector3 GetSnappedPoint(Vector3 point)
	{
		if (!this.apply)
		{
			return point;
		}
		if (!this.from || !this.to)
		{
			return point;
		}
		return SnapXformToLine.GetClosestPointOnLine(point, this.from.position, this.to.position);
	}

	// Token: 0x0600278B RID: 10123 RVA: 0x000C1710 File Offset: 0x000BF910
	public void Snap(Transform xform, bool applyToXform = true)
	{
		if (!this.apply)
		{
			return;
		}
		if (!xform || !this.from || !this.to)
		{
			return;
		}
		Vector3 position = xform.position;
		Vector3 position2 = this.from.position;
		Vector3 position3 = this.to.position;
		Vector3 closestPointOnLine = SnapXformToLine.GetClosestPointOnLine(position, position2, position3);
		float num = Vector3.Distance(position2, position3);
		float num2 = Vector3.Distance(closestPointOnLine, position2);
		Vector3 closest = this._closest;
		Vector3 vector = closestPointOnLine;
		float linear = this._linear;
		float num3 = num.Approx0(1E-06f) ? 0f : (num2 / num);
		this._closest = vector;
		this._linear = num3;
		if (this.output)
		{
			IRangedVariable<float> asT = this.output.AsT;
			asT.Set(asT.Min + this._linear * asT.Range);
		}
		if (applyToXform)
		{
			xform.position = this._closest;
			if (!closest.Approx(vector, 1E-05f))
			{
				UnityEvent<Vector3> unityEvent = this.onPositionChanged;
				if (unityEvent != null)
				{
					unityEvent.Invoke(this._closest);
				}
			}
			if (!linear.Approx(num3, 1E-06f))
			{
				UnityEvent<float> unityEvent2 = this.onLinearDistanceChanged;
				if (unityEvent2 != null)
				{
					unityEvent2.Invoke(this._linear);
				}
			}
			if (this.snapOrientation)
			{
				xform.forward = (position3 - position2).normalized;
				xform.up = Vector3.Lerp(this.from.up, this.to.up, this._linear);
			}
		}
	}

	// Token: 0x0600278C RID: 10124 RVA: 0x000C1895 File Offset: 0x000BFA95
	private void OnDisable()
	{
		if (this.resetOnDisable)
		{
			this.SnapTargetLinear(0f);
		}
	}

	// Token: 0x0600278D RID: 10125 RVA: 0x000C18AA File Offset: 0x000BFAAA
	private void LateUpdate()
	{
		this.SnapTarget(true);
	}

	// Token: 0x0600278E RID: 10126 RVA: 0x000C18B4 File Offset: 0x000BFAB4
	private static Vector3 GetClosestPointOnLine(Vector3 p, Vector3 a, Vector3 b)
	{
		Vector3 lhs = p - a;
		Vector3 vector = b - a;
		float sqrMagnitude = vector.sqrMagnitude;
		float num = Vector3.Dot(lhs, vector) / sqrMagnitude;
		if (num < 0f)
		{
			return a;
		}
		if (num > 1f)
		{
			return b;
		}
		return a + vector * num;
	}

	// Token: 0x04002B42 RID: 11074
	public bool apply = true;

	// Token: 0x04002B43 RID: 11075
	public bool snapOrientation = true;

	// Token: 0x04002B44 RID: 11076
	public bool resetOnDisable = true;

	// Token: 0x04002B45 RID: 11077
	[Space]
	public Transform target;

	// Token: 0x04002B46 RID: 11078
	[Space]
	public Transform from;

	// Token: 0x04002B47 RID: 11079
	public Transform to;

	// Token: 0x04002B48 RID: 11080
	private Vector3 _closest;

	// Token: 0x04002B49 RID: 11081
	private float _linear;

	// Token: 0x04002B4A RID: 11082
	public Ref<IRangedVariable<float>> output;

	// Token: 0x04002B4B RID: 11083
	public UnityEvent<float> onLinearDistanceChanged;

	// Token: 0x04002B4C RID: 11084
	public UnityEvent<Vector3> onPositionChanged;
}
