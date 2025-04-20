using System;
using UnityEngine;

// Token: 0x0200060B RID: 1547
public class RangedFloat : MonoBehaviour, IRangedVariable<float>, IVariable<float>, IVariable
{
	// Token: 0x170003FB RID: 1019
	// (get) Token: 0x0600267B RID: 9851 RVA: 0x0004A3E1 File Offset: 0x000485E1
	public AnimationCurve Curve
	{
		get
		{
			return this._curve;
		}
	}

	// Token: 0x170003FC RID: 1020
	// (get) Token: 0x0600267C RID: 9852 RVA: 0x0004A3E9 File Offset: 0x000485E9
	public float Range
	{
		get
		{
			return this._max - this._min;
		}
	}

	// Token: 0x170003FD RID: 1021
	// (get) Token: 0x0600267D RID: 9853 RVA: 0x0004A3F8 File Offset: 0x000485F8
	// (set) Token: 0x0600267E RID: 9854 RVA: 0x0004A400 File Offset: 0x00048600
	public float Min
	{
		get
		{
			return this._min;
		}
		set
		{
			this._min = value;
		}
	}

	// Token: 0x170003FE RID: 1022
	// (get) Token: 0x0600267F RID: 9855 RVA: 0x0004A409 File Offset: 0x00048609
	// (set) Token: 0x06002680 RID: 9856 RVA: 0x0004A411 File Offset: 0x00048611
	public float Max
	{
		get
		{
			return this._max;
		}
		set
		{
			this._max = value;
		}
	}

	// Token: 0x170003FF RID: 1023
	// (get) Token: 0x06002681 RID: 9857 RVA: 0x0004A41A File Offset: 0x0004861A
	// (set) Token: 0x06002682 RID: 9858 RVA: 0x0004A44F File Offset: 0x0004864F
	public float normalized
	{
		get
		{
			if (!this.Range.Approx0(1E-06f))
			{
				return (this._value - this._min) / (this._max - this.Min);
			}
			return 0f;
		}
		set
		{
			this._value = this._min + Mathf.Clamp01(value) * (this._max - this._min);
		}
	}

	// Token: 0x17000400 RID: 1024
	// (get) Token: 0x06002683 RID: 9859 RVA: 0x0004A472 File Offset: 0x00048672
	public float curved
	{
		get
		{
			return this._min + this._curve.Evaluate(this.normalized) * (this._max - this._min);
		}
	}

	// Token: 0x06002684 RID: 9860 RVA: 0x0004A49A File Offset: 0x0004869A
	public float Get()
	{
		return this._value;
	}

	// Token: 0x06002685 RID: 9861 RVA: 0x0004A4A2 File Offset: 0x000486A2
	public void Set(float f)
	{
		this._value = Mathf.Clamp(f, this._min, this._max);
	}

	// Token: 0x04002A7D RID: 10877
	[SerializeField]
	private float _value = 0.5f;

	// Token: 0x04002A7E RID: 10878
	[SerializeField]
	private float _min;

	// Token: 0x04002A7F RID: 10879
	[SerializeField]
	private float _max = 1f;

	// Token: 0x04002A80 RID: 10880
	[SerializeField]
	private AnimationCurve _curve = AnimationCurve.Linear(0f, 0f, 1f, 1f);
}
