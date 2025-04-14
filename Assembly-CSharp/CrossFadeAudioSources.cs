using System;
using UnityEngine;

// Token: 0x0200050D RID: 1293
public class CrossFadeAudioSources : MonoBehaviour, IRangedVariable<float>, IVariable<float>, IVariable
{
	// Token: 0x06001F6A RID: 8042 RVA: 0x0009E6B8 File Offset: 0x0009C8B8
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

	// Token: 0x06001F6B RID: 8043 RVA: 0x0009E6EA File Offset: 0x0009C8EA
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

	// Token: 0x06001F6C RID: 8044 RVA: 0x0009E71C File Offset: 0x0009C91C
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

	// Token: 0x06001F6D RID: 8045 RVA: 0x0009E7D2 File Offset: 0x0009C9D2
	public float Get()
	{
		return this._lerp;
	}

	// Token: 0x06001F6E RID: 8046 RVA: 0x0009E7DA File Offset: 0x0009C9DA
	public void Set(float f)
	{
		this._lerp = Mathf.Clamp01(f);
	}

	// Token: 0x17000332 RID: 818
	// (get) Token: 0x06001F6F RID: 8047 RVA: 0x0009E7E8 File Offset: 0x0009C9E8
	// (set) Token: 0x06001F70 RID: 8048 RVA: 0x000023F4 File Offset: 0x000005F4
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
	// (get) Token: 0x06001F71 RID: 8049 RVA: 0x0009E7EF File Offset: 0x0009C9EF
	// (set) Token: 0x06001F72 RID: 8050 RVA: 0x000023F4 File Offset: 0x000005F4
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
	// (get) Token: 0x06001F73 RID: 8051 RVA: 0x0009E7EF File Offset: 0x0009C9EF
	public float Range
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x17000335 RID: 821
	// (get) Token: 0x06001F74 RID: 8052 RVA: 0x0009E7F6 File Offset: 0x0009C9F6
	public AnimationCurve Curve
	{
		get
		{
			return this._curve;
		}
	}

	// Token: 0x0400233B RID: 9019
	[SerializeField]
	private float _lerp;

	// Token: 0x0400233C RID: 9020
	[SerializeField]
	private AnimationCurve _curve = AnimationCurve.Linear(0f, 0f, 1f, 1f);

	// Token: 0x0400233D RID: 9021
	[Space]
	[SerializeField]
	private AudioSource source1;

	// Token: 0x0400233E RID: 9022
	[SerializeField]
	private AudioSource source2;

	// Token: 0x0400233F RID: 9023
	[Space]
	public bool lerpByClipLength;

	// Token: 0x04002340 RID: 9024
	public bool tween;

	// Token: 0x04002341 RID: 9025
	public float tweenSpeed = 16f;

	// Token: 0x04002342 RID: 9026
	private float _lastT;
}
