using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

// Token: 0x02000438 RID: 1080
public class GumBubble : LerpComponent
{
	// Token: 0x06001A97 RID: 6807 RVA: 0x000830B5 File Offset: 0x000812B5
	private void Awake()
	{
		base.enabled = false;
		base.gameObject.SetActive(false);
	}

	// Token: 0x06001A98 RID: 6808 RVA: 0x000830CA File Offset: 0x000812CA
	public void InflateDelayed()
	{
		this.InflateDelayed(this._delayInflate);
	}

	// Token: 0x06001A99 RID: 6809 RVA: 0x000830D8 File Offset: 0x000812D8
	public void InflateDelayed(float delay)
	{
		if (delay < 0f)
		{
			delay = 0f;
		}
		base.Invoke("Inflate", delay);
	}

	// Token: 0x06001A9A RID: 6810 RVA: 0x000830F8 File Offset: 0x000812F8
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

	// Token: 0x06001A9B RID: 6811 RVA: 0x0008317C File Offset: 0x0008137C
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

	// Token: 0x06001A9C RID: 6812 RVA: 0x00083200 File Offset: 0x00081400
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

	// Token: 0x06001A9D RID: 6813 RVA: 0x00083294 File Offset: 0x00081494
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

	// Token: 0x04001D5C RID: 7516
	public Transform target;

	// Token: 0x04001D5D RID: 7517
	public Vector3 targetScale = Vector3.one;

	// Token: 0x04001D5E RID: 7518
	[SerializeField]
	private AnimationCurve _lerpCurve;

	// Token: 0x04001D5F RID: 7519
	public AudioSource audioSource;

	// Token: 0x04001D60 RID: 7520
	[SerializeField]
	private AudioClip _sfxInflate;

	// Token: 0x04001D61 RID: 7521
	[SerializeField]
	private AudioClip _sfxPop;

	// Token: 0x04001D62 RID: 7522
	[SerializeField]
	private float _delayInflate = 1.16f;

	// Token: 0x04001D63 RID: 7523
	[FormerlySerializedAs("_popDelay")]
	[SerializeField]
	private float _delayPop = 0.5f;

	// Token: 0x04001D64 RID: 7524
	[SerializeField]
	private bool _animating;

	// Token: 0x04001D65 RID: 7525
	public UnityEvent onPop;

	// Token: 0x04001D66 RID: 7526
	public UnityEvent onInflate;

	// Token: 0x04001D67 RID: 7527
	[NonSerialized]
	private bool _done;

	// Token: 0x04001D68 RID: 7528
	[NonSerialized]
	private TimeSince _sinceInflate;
}
