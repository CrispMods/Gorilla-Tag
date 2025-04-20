using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// Token: 0x02000894 RID: 2196
public class BezierSpline : MonoBehaviour
{
	// Token: 0x0600353D RID: 13629 RVA: 0x00141004 File Offset: 0x0013F204
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

	// Token: 0x0600353E RID: 13630 RVA: 0x00141068 File Offset: 0x0013F268
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

	// Token: 0x0600353F RID: 13631 RVA: 0x00141100 File Offset: 0x0013F300
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

	// Token: 0x17000577 RID: 1399
	// (get) Token: 0x06003540 RID: 13632 RVA: 0x0005312B File Offset: 0x0005132B
	// (set) Token: 0x06003541 RID: 13633 RVA: 0x00053133 File Offset: 0x00051333
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

	// Token: 0x17000578 RID: 1400
	// (get) Token: 0x06003542 RID: 13634 RVA: 0x0005316B File Offset: 0x0005136B
	public int ControlPointCount
	{
		get
		{
			return this.points.Length;
		}
	}

	// Token: 0x06003543 RID: 13635 RVA: 0x00053175 File Offset: 0x00051375
	public Vector3 GetControlPoint(int index)
	{
		return this.points[index];
	}

	// Token: 0x06003544 RID: 13636 RVA: 0x001411EC File Offset: 0x0013F3EC
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

	// Token: 0x06003545 RID: 13637 RVA: 0x00053183 File Offset: 0x00051383
	public BezierControlPointMode GetControlPointMode(int index)
	{
		return this.modes[(index + 1) / 3];
	}

	// Token: 0x06003546 RID: 13638 RVA: 0x00141380 File Offset: 0x0013F580
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

	// Token: 0x06003547 RID: 13639 RVA: 0x001413D8 File Offset: 0x0013F5D8
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

	// Token: 0x17000579 RID: 1401
	// (get) Token: 0x06003548 RID: 13640 RVA: 0x00053191 File Offset: 0x00051391
	public int CurveCount
	{
		get
		{
			return (this.points.Length - 1) / 3;
		}
	}

	// Token: 0x06003549 RID: 13641 RVA: 0x0005319F File Offset: 0x0005139F
	public Vector3 GetPoint(float t, bool ConstantVelocity)
	{
		if (ConstantVelocity)
		{
			return this.GetPoint(this.getPathFromTime(t));
		}
		return this.GetPoint(t);
	}

	// Token: 0x0600354A RID: 13642 RVA: 0x001414C8 File Offset: 0x0013F6C8
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

	// Token: 0x0600354B RID: 13643 RVA: 0x00141558 File Offset: 0x0013F758
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

	// Token: 0x0600354C RID: 13644 RVA: 0x001415DC File Offset: 0x0013F7DC
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

	// Token: 0x0600354D RID: 13645 RVA: 0x000531B9 File Offset: 0x000513B9
	public Vector3 GetDirection(float t, bool ConstantVelocity)
	{
		if (ConstantVelocity)
		{
			return this.GetDirection(this.getPathFromTime(t));
		}
		return this.GetDirection(t);
	}

	// Token: 0x0600354E RID: 13646 RVA: 0x0014167C File Offset: 0x0013F87C
	public Vector3 GetDirection(float t)
	{
		return this.GetVelocity(t).normalized;
	}

	// Token: 0x0600354F RID: 13647 RVA: 0x00141698 File Offset: 0x0013F898
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

	// Token: 0x06003550 RID: 13648 RVA: 0x000531D3 File Offset: 0x000513D3
	public void RemoveLastCurve()
	{
		if (this.points.Length <= 4)
		{
			return;
		}
		Array.Resize<Vector3>(ref this.points, this.points.Length - 3);
		Array.Resize<BezierControlPointMode>(ref this.modes, this.modes.Length - 1);
	}

	// Token: 0x06003551 RID: 13649 RVA: 0x001417D4 File Offset: 0x0013F9D4
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

	// Token: 0x06003552 RID: 13650 RVA: 0x0014185C File Offset: 0x0013FA5C
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

	// Token: 0x04003807 RID: 14343
	[SerializeField]
	private Vector3[] points;

	// Token: 0x04003808 RID: 14344
	[SerializeField]
	private BezierControlPointMode[] modes;

	// Token: 0x04003809 RID: 14345
	[SerializeField]
	private bool loop;

	// Token: 0x0400380A RID: 14346
	private float _totalArcLength;

	// Token: 0x0400380B RID: 14347
	private float[] _timesTable;

	// Token: 0x0400380C RID: 14348
	private float[] _lengthsTable;
}
