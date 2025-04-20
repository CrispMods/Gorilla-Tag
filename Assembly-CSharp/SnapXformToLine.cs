using System;
using UnityEngine;
using UnityEngine.Events;

// Token: 0x02000616 RID: 1558
public class SnapXformToLine : MonoBehaviour
{
	// Token: 0x17000407 RID: 1031
	// (get) Token: 0x060026AF RID: 9903 RVA: 0x0004A63B File Offset: 0x0004883B
	public Vector3 linePoint
	{
		get
		{
			return this._closest;
		}
	}

	// Token: 0x17000408 RID: 1032
	// (get) Token: 0x060026B0 RID: 9904 RVA: 0x0004A643 File Offset: 0x00048843
	public float linearDistance
	{
		get
		{
			return this._linear;
		}
	}

	// Token: 0x060026B1 RID: 9905 RVA: 0x0004A64B File Offset: 0x0004884B
	public void SnapTarget(bool applyToXform = true)
	{
		this.Snap(this.target, true);
	}

	// Token: 0x060026B2 RID: 9906 RVA: 0x0004A65A File Offset: 0x0004885A
	public void SnapTarget(Vector3 point)
	{
		if (this.target)
		{
			this.target.position = this.GetSnappedPoint(this.target.position);
		}
	}

	// Token: 0x060026B3 RID: 9907 RVA: 0x00108E78 File Offset: 0x00107078
	public void SnapTargetLinear(float t)
	{
		if (this.target && this.from && this.to)
		{
			this.target.position = Vector3.Lerp(this.from.position, this.to.position, t);
		}
	}

	// Token: 0x060026B4 RID: 9908 RVA: 0x0004A685 File Offset: 0x00048885
	public Vector3 GetSnappedPoint(Transform t)
	{
		return this.GetSnappedPoint(t.position);
	}

	// Token: 0x060026B5 RID: 9909 RVA: 0x00108ED4 File Offset: 0x001070D4
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

	// Token: 0x060026B6 RID: 9910 RVA: 0x00108F24 File Offset: 0x00107124
	public void Snap(Transform xform, bool applyToXform = true)
	{
		if (!this.apply || !xform || !this.from || !this.to)
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
		float num3 = Mathf.Approximately(num, 0f) ? 0f : (num2 / (num + Mathf.Epsilon));
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
			if (!Mathf.Approximately(closest.x, vector.x) || !Mathf.Approximately(closest.y, vector.y) || !Mathf.Approximately(closest.z, vector.z))
			{
				UnityEvent<Vector3> unityEvent = this.onPositionChanged;
				if (unityEvent != null)
				{
					unityEvent.Invoke(this._closest);
				}
			}
			if (!Mathf.Approximately(linear, num3))
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
				xform.up = Vector3.Lerp(this.from.up.normalized, this.to.up.normalized, this._linear);
			}
		}
	}

	// Token: 0x060026B7 RID: 9911 RVA: 0x0004A693 File Offset: 0x00048893
	private void OnDisable()
	{
		if (this.resetOnDisable)
		{
			this.SnapTargetLinear(0f);
		}
	}

	// Token: 0x060026B8 RID: 9912 RVA: 0x0004A6A8 File Offset: 0x000488A8
	private void LateUpdate()
	{
		this.SnapTarget(true);
	}

	// Token: 0x060026B9 RID: 9913 RVA: 0x001090EC File Offset: 0x001072EC
	private static Vector3 GetClosestPointOnLine(Vector3 p, Vector3 a, Vector3 b)
	{
		Vector3 lhs = p - a;
		Vector3 vector = b - a;
		float sqrMagnitude = vector.sqrMagnitude;
		float d = Mathf.Clamp(Vector3.Dot(lhs, vector) / sqrMagnitude, 0f, 1f);
		return a + vector * d;
	}

	// Token: 0x04002AA8 RID: 10920
	public bool apply = true;

	// Token: 0x04002AA9 RID: 10921
	public bool snapOrientation = true;

	// Token: 0x04002AAA RID: 10922
	public bool resetOnDisable = true;

	// Token: 0x04002AAB RID: 10923
	[Space]
	public Transform target;

	// Token: 0x04002AAC RID: 10924
	[Space]
	public Transform from;

	// Token: 0x04002AAD RID: 10925
	public Transform to;

	// Token: 0x04002AAE RID: 10926
	private Vector3 _closest;

	// Token: 0x04002AAF RID: 10927
	private float _linear;

	// Token: 0x04002AB0 RID: 10928
	public Ref<IRangedVariable<float>> output;

	// Token: 0x04002AB1 RID: 10929
	public UnityEvent<float> onLinearDistanceChanged;

	// Token: 0x04002AB2 RID: 10930
	public UnityEvent<Vector3> onPositionChanged;
}
