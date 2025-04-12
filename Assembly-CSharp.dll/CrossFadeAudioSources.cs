using System;
using UnityEngine;

// Token: 0x0200050D RID: 1293
public class CrossFadeAudioSources : MonoBehaviour, IRangedVariable<float>, IVariable<float>, IVariable
{
	// Token: 0x06001F6D RID: 8045 RVA: 0x0004457F File Offset: 0x0004277F
	public void Play()
	{
		if (this.source1)
		{
			this.source1.Play();
		}
		if (this.source2)
		{
			this.source2.Play();
		}
	}

	// Token: 0x06001F6E RID: 8046 RVA: 0x000445B1 File Offset: 0x000427B1
	public void Stop()
	{
		if (this.source1)
		{
			this.source1.Stop();
		}
		if (this.source2)
		{
			this.source2.Stop();
		}
	}

	// Token: 0x06001F6F RID: 8047 RVA: 0x000ED5B4 File Offset: 0x000EB7B4
	private void Update()
	{
		if (!this.source1 || !this.source2)
		{
			return;
		}
		float num = this._curve.Evaluate(this._lerp);
		float num2;
		if (this.tween)
		{
			num2 = MathUtils.Xlerp(this._lastT, num, Time.deltaTime, this.tweenSpeed);
		}
		else
		{
			num2 = (this.lerpByClipLength ? this._curve.Evaluate((float)this.source1.timeSamples / (float)this.source1.clip.samples) : num);
		}
		this._lastT = num2;
		this.source2.volume = num2;
		this.source1.volume = 1f - num2;
	}

	// Token: 0x06001F70 RID: 8048 RVA: 0x000445E3 File Offset: 0x000427E3
	public float Get()
	{
		return this._lerp;
	}

	// Token: 0x06001F71 RID: 8049 RVA: 0x000445EB File Offset: 0x000427EB
	public void Set(float f)
	{
		this._lerp = Mathf.Clamp01(f);
	}

	// Token: 0x17000332 RID: 818
	// (get) Token: 0x06001F72 RID: 8050 RVA: 0x000445F9 File Offset: 0x000427F9
	// (set) Token: 0x06001F73 RID: 8051 RVA: 0x0002F75F File Offset: 0x0002D95F
	public float Min
	{
		get
		{
			return 0f;
		}
		set
		{
		}
	}

	// Token: 0x17000333 RID: 819
	// (get) Token: 0x06001F74 RID: 8052 RVA: 0x00044600 File Offset: 0x00042800
	// (set) Token: 0x06001F75 RID: 8053 RVA: 0x0002F75F File Offset: 0x0002D95F
	public float Max
	{
		get
		{
			return 1f;
		}
		set
		{
		}
	}

	// Token: 0x17000334 RID: 820
	// (get) Token: 0x06001F76 RID: 8054 RVA: 0x00044600 File Offset: 0x00042800
	public float Range
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x17000335 RID: 821
	// (get) Token: 0x06001F77 RID: 8055 RVA: 0x00044607 File Offset: 0x00042807
	public AnimationCurve Curve
	{
		get
		{
			return this._curve;
		}
	}

	// Token: 0x0400233C RID: 9020
	[SerializeField]
	private float _lerp;

	// Token: 0x0400233D RID: 9021
	[SerializeField]
	private AnimationCurve _curve = AnimationCurve.Linear(0f, 0f, 1f, 1f);

	// Token: 0x0400233E RID: 9022
	[Space]
	[SerializeField]
	private AudioSource source1;

	// Token: 0x0400233F RID: 9023
	[SerializeField]
	private AudioSource source2;

	// Token: 0x04002340 RID: 9024
	[Space]
	public bool lerpByClipLength;

	// Token: 0x04002341 RID: 9025
	public bool tween;

	// Token: 0x04002342 RID: 9026
	public float tweenSpeed = 16f;

	// Token: 0x04002343 RID: 9027
	private float _lastT;
}
