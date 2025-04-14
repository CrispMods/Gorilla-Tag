using System;
using UnityEngine;

// Token: 0x0200062C RID: 1580
public class RangedFloat : MonoBehaviour, IRangedVariable<float>, IVariable<float>, IVariable
{
	// Token: 0x1700041D RID: 1053
	// (get) Token: 0x06002750 RID: 10064 RVA: 0x000C10B3 File Offset: 0x000BF2B3
	public AnimationCurve Curve
	{
		get
		{
			return this._curve;
		}
	}

	// Token: 0x1700041E RID: 1054
	// (get) Token: 0x06002751 RID: 10065 RVA: 0x000C10BB File Offset: 0x000BF2BB
	public float Range
	{
		get
		{
			return this._max - this._min;
		}
	}

	// Token: 0x1700041F RID: 1055
	// (get) Token: 0x06002752 RID: 10066 RVA: 0x000C10CA File Offset: 0x000BF2CA
	// (set) Token: 0x06002753 RID: 10067 RVA: 0x000C10D2 File Offset: 0x000BF2D2
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

	// Token: 0x17000420 RID: 1056
	// (get) Token: 0x06002754 RID: 10068 RVA: 0x000C10DB File Offset: 0x000BF2DB
	// (set) Token: 0x06002755 RID: 10069 RVA: 0x000C10E3 File Offset: 0x000BF2E3
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

	// Token: 0x17000421 RID: 1057
	// (get) Token: 0x06002756 RID: 10070 RVA: 0x000C10EC File Offset: 0x000BF2EC
	// (set) Token: 0x06002757 RID: 10071 RVA: 0x000C1121 File Offset: 0x000BF321
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

	// Token: 0x17000422 RID: 1058
	// (get) Token: 0x06002758 RID: 10072 RVA: 0x000C1144 File Offset: 0x000BF344
	public float curved
	{
		get
		{
			return this._min + this._curve.Evaluate(this.normalized) * (this._max - this._min);
		}
	}

	// Token: 0x06002759 RID: 10073 RVA: 0x000C116C File Offset: 0x000BF36C
	public float Get()
	{
		return this._value;
	}

	// Token: 0x0600275A RID: 10074 RVA: 0x000C1174 File Offset: 0x000BF374
	public void Set(float f)
	{
		this._value = Mathf.Clamp(f, this._min, this._max);
	}

	// Token: 0x04002B17 RID: 11031
	[SerializeField]
	private float _value = 0.5f;

	// Token: 0x04002B18 RID: 11032
	[SerializeField]
	private float _min;

	// Token: 0x04002B19 RID: 11033
	[SerializeField]
	private float _max = 1f;

	// Token: 0x04002B1A RID: 11034
	[SerializeField]
	private AnimationCurve _curve = AnimationCurve.Linear(0f, 0f, 1f, 1f);
}
