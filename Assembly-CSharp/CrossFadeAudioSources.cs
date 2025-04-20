using System;
using UnityEngine;

// Token: 0x0200051A RID: 1306
public class CrossFadeAudioSources : MonoBehaviour, IRangedVariable<float>, IVariable<float>, IVariable
{
	// Token: 0x06001FC3 RID: 8131 RVA: 0x0004591E File Offset: 0x00043B1E
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

	// Token: 0x06001FC4 RID: 8132 RVA: 0x00045950 File Offset: 0x00043B50
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

	// Token: 0x06001FC5 RID: 8133 RVA: 0x000F0338 File Offset: 0x000EE538
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

	// Token: 0x06001FC6 RID: 8134 RVA: 0x00045982 File Offset: 0x00043B82
	public float Get()
	{
		return this._lerp;
	}

	// Token: 0x06001FC7 RID: 8135 RVA: 0x0004598A File Offset: 0x00043B8A
	public void Set(float f)
	{
		this._lerp = Mathf.Clamp01(f);
	}

	// Token: 0x17000339 RID: 825
	// (get) Token: 0x06001FC8 RID: 8136 RVA: 0x00045998 File Offset: 0x00043B98
	// (set) Token: 0x06001FC9 RID: 8137 RVA: 0x00030607 File Offset: 0x0002E807
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

	// Token: 0x1700033A RID: 826
	// (get) Token: 0x06001FCA RID: 8138 RVA: 0x0004599F File Offset: 0x00043B9F
	// (set) Token: 0x06001FCB RID: 8139 RVA: 0x00030607 File Offset: 0x0002E807
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

	// Token: 0x1700033B RID: 827
	// (get) Token: 0x06001FCC RID: 8140 RVA: 0x0004599F File Offset: 0x00043B9F
	public float Range
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x1700033C RID: 828
	// (get) Token: 0x06001FCD RID: 8141 RVA: 0x000459A6 File Offset: 0x00043BA6
	public AnimationCurve Curve
	{
		get
		{
			return this._curve;
		}
	}

	// Token: 0x0400238E RID: 9102
	[SerializeField]
	private float _lerp;

	// Token: 0x0400238F RID: 9103
	[SerializeField]
	private AnimationCurve _curve = AnimationCurve.Linear(0f, 0f, 1f, 1f);

	// Token: 0x04002390 RID: 9104
	[Space]
	[SerializeField]
	private AudioSource source1;

	// Token: 0x04002391 RID: 9105
	[SerializeField]
	private AudioSource source2;

	// Token: 0x04002392 RID: 9106
	[Space]
	public bool lerpByClipLength;

	// Token: 0x04002393 RID: 9107
	public bool tween;

	// Token: 0x04002394 RID: 9108
	public float tweenSpeed = 16f;

	// Token: 0x04002395 RID: 9109
	private float _lastT;
}
