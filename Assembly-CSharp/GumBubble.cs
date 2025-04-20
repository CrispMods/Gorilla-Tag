using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

// Token: 0x02000444 RID: 1092
public class GumBubble : LerpComponent
{
	// Token: 0x06001AEB RID: 6891 RVA: 0x000423E5 File Offset: 0x000405E5
	private void Awake()
	{
		base.enabled = false;
		base.gameObject.SetActive(false);
	}

	// Token: 0x06001AEC RID: 6892 RVA: 0x000423FA File Offset: 0x000405FA
	public void InflateDelayed()
	{
		this.InflateDelayed(this._delayInflate);
	}

	// Token: 0x06001AED RID: 6893 RVA: 0x00042408 File Offset: 0x00040608
	public void InflateDelayed(float delay)
	{
		if (delay < 0f)
		{
			delay = 0f;
		}
		base.Invoke("Inflate", delay);
	}

	// Token: 0x06001AEE RID: 6894 RVA: 0x000D81A8 File Offset: 0x000D63A8
	public void Inflate()
	{
		base.gameObject.SetActive(true);
		base.enabled = true;
		if (this._animating)
		{
			return;
		}
		this._animating = true;
		this._sinceInflate = 0f;
		if (this.audioSource != null && this._sfxInflate != null)
		{
			this.audioSource.GTPlayOneShot(this._sfxInflate, 1f);
		}
		UnityEvent unityEvent = this.onInflate;
		if (unityEvent == null)
		{
			return;
		}
		unityEvent.Invoke();
	}

	// Token: 0x06001AEF RID: 6895 RVA: 0x000D822C File Offset: 0x000D642C
	public void Pop()
	{
		this._lerp = 0f;
		base.RenderLerp();
		if (this.audioSource != null && this._sfxPop != null)
		{
			this.audioSource.GTPlayOneShot(this._sfxPop, 1f);
		}
		UnityEvent unityEvent = this.onPop;
		if (unityEvent != null)
		{
			unityEvent.Invoke();
		}
		this._done = false;
		this._animating = false;
		base.enabled = false;
		base.gameObject.SetActive(false);
	}

	// Token: 0x06001AF0 RID: 6896 RVA: 0x000D82B0 File Offset: 0x000D64B0
	private void Update()
	{
		float t = Mathf.Clamp01(this._sinceInflate / this._lerpLength);
		this._lerp = Mathf.Lerp(0f, 1f, t);
		if (this._lerp <= 1f && !this._done)
		{
			base.RenderLerp();
			if (Mathf.Approximately(this._lerp, 1f))
			{
				this._done = true;
			}
		}
		float num = this._lerpLength + this._delayPop;
		if (this._sinceInflate >= num)
		{
			this.Pop();
		}
	}

	// Token: 0x06001AF1 RID: 6897 RVA: 0x000D8344 File Offset: 0x000D6544
	protected override void OnLerp(float t)
	{
		if (!this.target)
		{
			return;
		}
		if (this._lerpCurve == null)
		{
			GTDev.LogError<string>("[GumBubble] Missing lerp curve", this, null);
			return;
		}
		this.target.localScale = this.targetScale * this._lerpCurve.Evaluate(t);
	}

	// Token: 0x04001DAB RID: 7595
	public Transform target;

	// Token: 0x04001DAC RID: 7596
	public Vector3 targetScale = Vector3.one;

	// Token: 0x04001DAD RID: 7597
	[SerializeField]
	private AnimationCurve _lerpCurve;

	// Token: 0x04001DAE RID: 7598
	public AudioSource audioSource;

	// Token: 0x04001DAF RID: 7599
	[SerializeField]
	private AudioClip _sfxInflate;

	// Token: 0x04001DB0 RID: 7600
	[SerializeField]
	private AudioClip _sfxPop;

	// Token: 0x04001DB1 RID: 7601
	[SerializeField]
	private float _delayInflate = 1.16f;

	// Token: 0x04001DB2 RID: 7602
	[FormerlySerializedAs("_popDelay")]
	[SerializeField]
	private float _delayPop = 0.5f;

	// Token: 0x04001DB3 RID: 7603
	[SerializeField]
	private bool _animating;

	// Token: 0x04001DB4 RID: 7604
	public UnityEvent onPop;

	// Token: 0x04001DB5 RID: 7605
	public UnityEvent onInflate;

	// Token: 0x04001DB6 RID: 7606
	[NonSerialized]
	private bool _done;

	// Token: 0x04001DB7 RID: 7607
	[NonSerialized]
	private TimeSince _sinceInflate;
}
