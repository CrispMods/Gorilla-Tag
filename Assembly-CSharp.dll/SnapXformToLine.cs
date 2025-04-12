using System;
using UnityEngine;
using UnityEngine.Events;

// Token: 0x02000638 RID: 1592
public class SnapXformToLine : MonoBehaviour
{
	// Token: 0x1700042A RID: 1066
	// (get) Token: 0x0600278C RID: 10124 RVA: 0x0004A0A6 File Offset: 0x000482A6
	public Vector3 linePoint
	{
		get
		{
			return this._closest;
		}
	}

	// Token: 0x1700042B RID: 1067
	// (get) Token: 0x0600278D RID: 10125 RVA: 0x0004A0AE File Offset: 0x000482AE
	public float linearDistance
	{
		get
		{
			return this._linear;
		}
	}

	// Token: 0x0600278E RID: 10126 RVA: 0x0004A0B6 File Offset: 0x000482B6
	public void SnapTarget(bool applyToXform = true)
	{
		this.Snap(this.target, true);
	}

	// Token: 0x0600278F RID: 10127 RVA: 0x0004A0C5 File Offset: 0x000482C5
	public void SnapTarget(Vector3 point)
	{
		if (this.target)
		{
			this.target.position = this.GetSnappedPoint(this.target.position);
		}
	}

	// Token: 0x06002790 RID: 10128 RVA: 0x0010AA8C File Offset: 0x00108C8C
	public void SnapTargetLinear(float t)
	{
		if (this.target && this.from && this.to)
		{
			this.target.position = Vector3.Lerp(this.from.position, this.to.position, t);
		}
	}

	// Token: 0x06002791 RID: 10129 RVA: 0x0004A0F0 File Offset: 0x000482F0
	public Vector3 GetSnappedPoint(Transform t)
	{
		return this.GetSnappedPoint(t.position);
	}

	// Token: 0x06002792 RID: 10130 RVA: 0x0010AAE8 File Offset: 0x00108CE8
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

	// Token: 0x06002793 RID: 10131 RVA: 0x0010AB38 File Offset: 0x00108D38
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

	// Token: 0x06002794 RID: 10132 RVA: 0x0004A0FE File Offset: 0x000482FE
	private void OnDisable()
	{
		if (this.resetOnDisable)
		{
			this.SnapTargetLinear(0f);
		}
	}

	// Token: 0x06002795 RID: 10133 RVA: 0x0004A113 File Offset: 0x00048313
	private void LateUpdate()
	{
		this.SnapTarget(true);
	}

	// Token: 0x06002796 RID: 10134 RVA: 0x0010ACC0 File Offset: 0x00108EC0
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

	// Token: 0x04002B48 RID: 11080
	public bool apply = true;

	// Token: 0x04002B49 RID: 11081
	public bool snapOrientation = true;

	// Token: 0x04002B4A RID: 11082
	public bool resetOnDisable = true;

	// Token: 0x04002B4B RID: 11083
	[Space]
	public Transform target;

	// Token: 0x04002B4C RID: 11084
	[Space]
	public Transform from;

	// Token: 0x04002B4D RID: 11085
	public Transform to;

	// Token: 0x04002B4E RID: 11086
	private Vector3 _closest;

	// Token: 0x04002B4F RID: 11087
	private float _linear;

	// Token: 0x04002B50 RID: 11088
	public Ref<IRangedVariable<float>> output;

	// Token: 0x04002B51 RID: 11089
	public UnityEvent<float> onLinearDistanceChanged;

	// Token: 0x04002B52 RID: 11090
	public UnityEvent<Vector3> onPositionChanged;
}
