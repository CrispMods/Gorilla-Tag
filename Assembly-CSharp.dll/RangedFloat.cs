using System;
using UnityEngine;

// Token: 0x0200062D RID: 1581
public class RangedFloat : MonoBehaviour, IRangedVariable<float>, IVariable<float>, IVariable
{
	// Token: 0x1700041E RID: 1054
	// (get) Token: 0x06002758 RID: 10072 RVA: 0x00049E4C File Offset: 0x0004804C
	public AnimationCurve Curve
	{
		get
		{
			return this._curve;
		}
	}

	// Token: 0x1700041F RID: 1055
	// (get) Token: 0x06002759 RID: 10073 RVA: 0x00049E54 File Offset: 0x00048054
	public float Range
	{
		get
		{
			return this._max - this._min;
		}
	}

	// Token: 0x17000420 RID: 1056
	// (get) Token: 0x0600275A RID: 10074 RVA: 0x00049E63 File Offset: 0x00048063
	// (set) Token: 0x0600275B RID: 10075 RVA: 0x00049E6B File Offset: 0x0004806B
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
	// (get) Token: 0x0600275C RID: 10076 RVA: 0x00049E74 File Offset: 0x00048074
	// (set) Token: 0x0600275D RID: 10077 RVA: 0x00049E7C File Offset: 0x0004807C
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
	// (get) Token: 0x0600275E RID: 10078 RVA: 0x00049E85 File Offset: 0x00048085
	// (set) Token: 0x0600275F RID: 10079 RVA: 0x00049EBA File Offset: 0x000480BA
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
	// (get) Token: 0x06002760 RID: 10080 RVA: 0x00049EDD File Offset: 0x000480DD
	public float curved
	{
		get
		{
			return this._min + this._curve.Evaluate(this.normalized) * (this._max - this._min);
		}
	}

	// Token: 0x06002761 RID: 10081 RVA: 0x00049F05 File Offset: 0x00048105
	public float Get()
	{
		return this._value;
	}

	// Token: 0x06002762 RID: 10082 RVA: 0x00049F0D File Offset: 0x0004810D
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
