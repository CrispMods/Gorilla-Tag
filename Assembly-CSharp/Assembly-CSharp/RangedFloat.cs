using System;
using UnityEngine;

// Token: 0x0200062D RID: 1581
public class RangedFloat : MonoBehaviour, IRangedVariable<float>, IVariable<float>, IVariable
{
	// Token: 0x1700041E RID: 1054
	// (get) Token: 0x06002758 RID: 10072 RVA: 0x000C1533 File Offset: 0x000BF733
	public AnimationCurve Curve
	{
		get
		{
			return this._curve;
		}
	}

	// Token: 0x1700041F RID: 1055
	// (get) Token: 0x06002759 RID: 10073 RVA: 0x000C153B File Offset: 0x000BF73B
	public float Range
	{
		get
		{
			return this._max - this._min;
		}
	}

	// Token: 0x17000420 RID: 1056
	// (get) Token: 0x0600275A RID: 10074 RVA: 0x000C154A File Offset: 0x000BF74A
	// (set) Token: 0x0600275B RID: 10075 RVA: 0x000C1552 File Offset: 0x000BF752
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

	// Token: 0x17000421 RID: 1057
	// (get) Token: 0x0600275C RID: 10076 RVA: 0x000C155B File Offset: 0x000BF75B
	// (set) Token: 0x0600275D RID: 10077 RVA: 0x000C1563 File Offset: 0x000BF763
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

	// Token: 0x17000422 RID: 1058
	// (get) Token: 0x0600275E RID: 10078 RVA: 0x000C156C File Offset: 0x000BF76C
	// (set) Token: 0x0600275F RID: 10079 RVA: 0x000C15A1 File Offset: 0x000BF7A1
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

	// Token: 0x17000423 RID: 1059
	// (get) Token: 0x06002760 RID: 10080 RVA: 0x000C15C4 File Offset: 0x000BF7C4
	public float curved
	{
		get
		{
			return this._min + this._curve.Evaluate(this.normalized) * (this._max - this._min);
		}
	}

	// Token: 0x06002761 RID: 10081 RVA: 0x000C15EC File Offset: 0x000BF7EC
	public float Get()
	{
		return this._value;
	}

	// Token: 0x06002762 RID: 10082 RVA: 0x000C15F4 File Offset: 0x000BF7F4
	public void Set(float f)
	{
		this._value = Mathf.Clamp(f, this._min, this._max);
	}

	// Token: 0x04002B1D RID: 11037
	[SerializeField]
	private float _value = 0.5f;

	// Token: 0x04002B1E RID: 11038
	[SerializeField]
	private float _min;

	// Token: 0x04002B1F RID: 11039
	[SerializeField]
	private float _max = 1f;

	// Token: 0x04002B20 RID: 11040
	[SerializeField]
	private AnimationCurve _curve = AnimationCurve.Linear(0f, 0f, 1f, 1f);
}
