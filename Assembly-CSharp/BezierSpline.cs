using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// Token: 0x02000878 RID: 2168
public class BezierSpline : MonoBehaviour
{
	// Token: 0x06003471 RID: 13425 RVA: 0x000FA1E4 File Offset: 0x000F83E4
	private void Awake()
	{
		float num = 0f;
		for (int i = 1; i < this.points.Length; i++)
		{
			num += (this.points[i] - this.points[i - 1]).magnitude;
		}
		int subdivisions = Mathf.RoundToInt(num / 0.1f);
		this.buildTimesLenghtsTables(subdivisions);
	}

	// Token: 0x06003472 RID: 13426 RVA: 0x000FA248 File Offset: 0x000F8448
	private void buildTimesLenghtsTables(int subdivisions)
	{
		this._totalArcLength = 0f;
		float num = 1f / (float)subdivisions;
		this._timesTable = new float[subdivisions];
		this._lengthsTable = new float[subdivisions];
		Vector3 b = this.GetPoint(0f);
		for (int i = 1; i <= subdivisions; i++)
		{
			float num2 = num * (float)i;
			Vector3 point = this.GetPoint(num2);
			this._totalArcLength += Vector3.Distance(point, b);
			b = point;
			this._timesTable[i - 1] = num2;
			this._lengthsTable[i - 1] = this._totalArcLength;
		}
	}

	// Token: 0x06003473 RID: 13427 RVA: 0x000FA2E0 File Offset: 0x000F84E0
	private float getPathFromTime(float t)
	{
		if (float.IsNaN(this._totalArcLength) || this._totalArcLength == 0f)
		{
			return t;
		}
		if (t > 0f && t < 1f)
		{
			float num = this._totalArcLength * t;
			float num2 = 0f;
			float num3 = 0f;
			float num4 = 0f;
			float num5 = 0f;
			int num6 = this._lengthsTable.Length;
			int i = 0;
			while (i < num6)
			{
				if (this._lengthsTable[i] > num)
				{
					num4 = this._timesTable[i];
					num5 = this._lengthsTable[i];
					if (i > 0)
					{
						num3 = this._lengthsTable[i - 1];
						break;
					}
					break;
				}
				else
				{
					num2 = this._timesTable[i];
					i++;
				}
			}
			t = num2 + (num - num3) / (num5 - num3) * (num4 - num2);
		}
		if (t > 1f)
		{
			t = 1f;
		}
		else if (t < 0f)
		{
			t = 0f;
		}
		return t;
	}

	// Token: 0x17000566 RID: 1382
	// (get) Token: 0x06003474 RID: 13428 RVA: 0x000FA3CB File Offset: 0x000F85CB
	// (set) Token: 0x06003475 RID: 13429 RVA: 0x000FA3D3 File Offset: 0x000F85D3
	public bool Loop
	{
		get
		{
			return this.loop;
		}
		set
		{
			this.loop = value;
			if (value)
			{
				this.modes[this.modes.Length - 1] = this.modes[0];
				this.SetControlPoint(0, this.points[0]);
			}
		}
	}

	// Token: 0x17000567 RID: 1383
	// (get) Token: 0x06003476 RID: 13430 RVA: 0x000FA40B File Offset: 0x000F860B
	public int ControlPointCount
	{
		get
		{
			return this.points.Length;
		}
	}

	// Token: 0x06003477 RID: 13431 RVA: 0x000FA415 File Offset: 0x000F8615
	public Vector3 GetControlPoint(int index)
	{
		return this.points[index];
	}

	// Token: 0x06003478 RID: 13432 RVA: 0x000FA424 File Offset: 0x000F8624
	public void SetControlPoint(int index, Vector3 point)
	{
		if (index % 3 == 0)
		{
			Vector3 b = point - this.points[index];
			if (this.loop)
			{
				if (index == 0)
				{
					this.points[1] += b;
					this.points[this.points.Length - 2] += b;
					this.points[this.points.Length - 1] = point;
				}
				else if (index == this.points.Length - 1)
				{
					this.points[0] = point;
					this.points[1] += b;
					this.points[index - 1] += b;
				}
				else
				{
					this.points[index - 1] += b;
					this.points[index + 1] += b;
				}
			}
			else
			{
				if (index > 0)
				{
					this.points[index - 1] += b;
				}
				if (index + 1 < this.points.Length)
				{
					this.points[index + 1] += b;
				}
			}
		}
		this.points[index] = point;
		this.EnforceMode(index);
	}

	// Token: 0x06003479 RID: 13433 RVA: 0x000FA5B6 File Offset: 0x000F87B6
	public BezierControlPointMode GetControlPointMode(int index)
	{
		return this.modes[(index + 1) / 3];
	}

	// Token: 0x0600347A RID: 13434 RVA: 0x000FA5C4 File Offset: 0x000F87C4
	public void SetControlPointMode(int index, BezierControlPointMode mode)
	{
		int num = (index + 1) / 3;
		this.modes[num] = mode;
		if (this.loop)
		{
			if (num == 0)
			{
				this.modes[this.modes.Length - 1] = mode;
			}
			else if (num == this.modes.Length - 1)
			{
				this.modes[0] = mode;
			}
		}
		this.EnforceMode(index);
	}

	// Token: 0x0600347B RID: 13435 RVA: 0x000FA61C File Offset: 0x000F881C
	private void EnforceMode(int index)
	{
		int num = (index + 1) / 3;
		BezierControlPointMode bezierControlPointMode = this.modes[num];
		if (bezierControlPointMode == BezierControlPointMode.Free || (!this.loop && (num == 0 || num == this.modes.Length - 1)))
		{
			return;
		}
		int num2 = num * 3;
		int num3;
		int num4;
		if (index <= num2)
		{
			num3 = num2 - 1;
			if (num3 < 0)
			{
				num3 = this.points.Length - 2;
			}
			num4 = num2 + 1;
			if (num4 >= this.points.Length)
			{
				num4 = 1;
			}
		}
		else
		{
			num3 = num2 + 1;
			if (num3 >= this.points.Length)
			{
				num3 = 1;
			}
			num4 = num2 - 1;
			if (num4 < 0)
			{
				num4 = this.points.Length - 2;
			}
		}
		Vector3 a = this.points[num2];
		Vector3 b = a - this.points[num3];
		if (bezierControlPointMode == BezierControlPointMode.Aligned)
		{
			b = b.normalized * Vector3.Distance(a, this.points[num4]);
		}
		this.points[num4] = a + b;
	}

	// Token: 0x17000568 RID: 1384
	// (get) Token: 0x0600347C RID: 13436 RVA: 0x000FA70B File Offset: 0x000F890B
	public int CurveCount
	{
		get
		{
			return (this.points.Length - 1) / 3;
		}
	}

	// Token: 0x0600347D RID: 13437 RVA: 0x000FA719 File Offset: 0x000F8919
	public Vector3 GetPoint(float t, bool ConstantVelocity)
	{
		if (ConstantVelocity)
		{
			return this.GetPoint(this.getPathFromTime(t));
		}
		return this.GetPoint(t);
	}

	// Token: 0x0600347E RID: 13438 RVA: 0x000FA734 File Offset: 0x000F8934
	public Vector3 GetPoint(float t)
	{
		int num;
		if (t >= 1f)
		{
			t = 1f;
			num = this.points.Length - 4;
		}
		else
		{
			t = Mathf.Clamp01(t) * (float)this.CurveCount;
			num = (int)t;
			t -= (float)num;
			num *= 3;
		}
		return base.transform.TransformPoint(Bezier.GetPoint(this.points[num], this.points[num + 1], this.points[num + 2], this.points[num + 3], t));
	}

	// Token: 0x0600347F RID: 13439 RVA: 0x000FA7C4 File Offset: 0x000F89C4
	public Vector3 GetPointLocal(float t)
	{
		int num;
		if (t >= 1f)
		{
			t = 1f;
			num = this.points.Length - 4;
		}
		else
		{
			t = Mathf.Clamp01(t) * (float)this.CurveCount;
			num = (int)t;
			t -= (float)num;
			num *= 3;
		}
		return Bezier.GetPoint(this.points[num], this.points[num + 1], this.points[num + 2], this.points[num + 3], t);
	}

	// Token: 0x06003480 RID: 13440 RVA: 0x000FA848 File Offset: 0x000F8A48
	public Vector3 GetVelocity(float t)
	{
		int num;
		if (t >= 1f)
		{
			t = 1f;
			num = this.points.Length - 4;
		}
		else
		{
			t = Mathf.Clamp01(t) * (float)this.CurveCount;
			num = (int)t;
			t -= (float)num;
			num *= 3;
		}
		return base.transform.TransformPoint(Bezier.GetFirstDerivative(this.points[num], this.points[num + 1], this.points[num + 2], this.points[num + 3], t)) - base.transform.position;
	}

	// Token: 0x06003481 RID: 13441 RVA: 0x000FA8E5 File Offset: 0x000F8AE5
	public Vector3 GetDirection(float t, bool ConstantVelocity)
	{
		if (ConstantVelocity)
		{
			return this.GetDirection(this.getPathFromTime(t));
		}
		return this.GetDirection(t);
	}

	// Token: 0x06003482 RID: 13442 RVA: 0x000FA900 File Offset: 0x000F8B00
	public Vector3 GetDirection(float t)
	{
		return this.GetVelocity(t).normalized;
	}

	// Token: 0x06003483 RID: 13443 RVA: 0x000FA91C File Offset: 0x000F8B1C
	public void AddCurve()
	{
		Vector3 vector = this.points[this.points.Length - 1];
		Array.Resize<Vector3>(ref this.points, this.points.Length + 3);
		vector.x += 1f;
		this.points[this.points.Length - 3] = vector;
		vector.x += 1f;
		this.points[this.points.Length - 2] = vector;
		vector.x += 1f;
		this.points[this.points.Length - 1] = vector;
		Array.Resize<BezierControlPointMode>(ref this.modes, this.modes.Length + 1);
		this.modes[this.modes.Length - 1] = this.modes[this.modes.Length - 2];
		this.EnforceMode(this.points.Length - 4);
		if (this.loop)
		{
			this.points[this.points.Length - 1] = this.points[0];
			this.modes[this.modes.Length - 1] = this.modes[0];
			this.EnforceMode(0);
		}
	}

	// Token: 0x06003484 RID: 13444 RVA: 0x000FAA56 File Offset: 0x000F8C56
	public void RemoveLastCurve()
	{
		if (this.points.Length <= 4)
		{
			return;
		}
		Array.Resize<Vector3>(ref this.points, this.points.Length - 3);
		Array.Resize<BezierControlPointMode>(ref this.modes, this.modes.Length - 1);
	}

	// Token: 0x06003485 RID: 13445 RVA: 0x000FAA90 File Offset: 0x000F8C90
	public void RemoveCurve(int index)
	{
		if (this.points.Length <= 4)
		{
			return;
		}
		List<Vector3> list = this.points.ToList<Vector3>();
		int num = 4;
		while (num < this.points.Length && index - 3 > num)
		{
			num += 3;
		}
		for (int i = 0; i < 3; i++)
		{
			list.RemoveAt(num);
		}
		this.points = list.ToArray();
		int index2 = (num - 4) / 3;
		List<BezierControlPointMode> list2 = this.modes.ToList<BezierControlPointMode>();
		list2.RemoveAt(index2);
		this.modes = list2.ToArray();
	}

	// Token: 0x06003486 RID: 13446 RVA: 0x000FAB18 File Offset: 0x000F8D18
	public void Reset()
	{
		this.points = new Vector3[]
		{
			new Vector3(0f, -1f, 0f),
			new Vector3(0f, -1f, 2f),
			new Vector3(0f, -1f, 4f),
			new Vector3(0f, -1f, 6f)
		};
		this.modes = new BezierControlPointMode[2];
	}

	// Token: 0x04003747 RID: 14151
	[SerializeField]
	private Vector3[] points;

	// Token: 0x04003748 RID: 14152
	[SerializeField]
	private BezierControlPointMode[] modes;

	// Token: 0x04003749 RID: 14153
	[SerializeField]
	private bool loop;

	// Token: 0x0400374A RID: 14154
	private float _totalArcLength;

	// Token: 0x0400374B RID: 14155
	private float[] _timesTable;

	// Token: 0x0400374C RID: 14156
	private float[] _lengthsTable;
}
