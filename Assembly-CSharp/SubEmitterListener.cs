using System;
using UnityEngine;
using UnityEngine.Events;

// Token: 0x020006D4 RID: 1748
public class SubEmitterListener : MonoBehaviour
{
	// Token: 0x06002B78 RID: 11128 RVA: 0x000D640C File Offset: 0x000D460C
	private void OnEnable()
	{
		if (this.target == null)
		{
			this.Disable();
			return;
		}
		ParticleSystem.SubEmittersModule subEmitters = this.target.subEmitters;
		if (this.subEmitterIndex < 0)
		{
			this.subEmitterIndex = 0;
		}
		this._canListen = (subEmitters.subEmittersCount > 0 && this.subEmitterIndex <= subEmitters.subEmittersCount - 1);
		if (!this._canListen)
		{
			this.Disable();
			return;
		}
		this.subEmitter = this.target.subEmitters.GetSubEmitterSystem(this.subEmitterIndex);
		ParticleSystem.MainModule main = this.subEmitter.main;
		this.interval = main.startLifetime.constantMax * main.startLifetimeMultiplier;
	}

	// Token: 0x06002B79 RID: 11129 RVA: 0x000D64C8 File Offset: 0x000D46C8
	private void OnDisable()
	{
		this._listenOnce = false;
		this._listening = false;
	}

	// Token: 0x06002B7A RID: 11130 RVA: 0x000D64D8 File Offset: 0x000D46D8
	public void ListenStart()
	{
		if (this._listening)
		{
			return;
		}
		if (this._canListen)
		{
			this.Enable();
			this._listening = true;
		}
	}

	// Token: 0x06002B7B RID: 11131 RVA: 0x000D64F8 File Offset: 0x000D46F8
	public void ListenStop()
	{
		this.Disable();
	}

	// Token: 0x06002B7C RID: 11132 RVA: 0x000D6500 File Offset: 0x000D4700
	public void ListenOnce()
	{
		if (this._listening)
		{
			return;
		}
		this.Enable();
		if (this._canListen)
		{
			this.Enable();
			this._listenOnce = true;
			this._listening = true;
		}
	}

	// Token: 0x06002B7D RID: 11133 RVA: 0x000D6530 File Offset: 0x000D4730
	private void Update()
	{
		if (!this._canListen)
		{
			return;
		}
		if (!this._listening)
		{
			return;
		}
		if (this.subEmitter.particleCount > 0 && this._sinceLastEmit >= this.interval * this.intervalScale)
		{
			this._sinceLastEmit = 0f;
			this.OnSubEmit();
			if (this._listenOnce)
			{
				this.Disable();
			}
		}
	}

	// Token: 0x06002B7E RID: 11134 RVA: 0x000D659B File Offset: 0x000D479B
	protected virtual void OnSubEmit()
	{
		UnityEvent unityEvent = this.onSubEmit;
		if (unityEvent == null)
		{
			return;
		}
		unityEvent.Invoke();
	}

	// Token: 0x06002B7F RID: 11135 RVA: 0x000D65AD File Offset: 0x000D47AD
	public void Enable()
	{
		if (!base.enabled)
		{
			base.enabled = true;
		}
	}

	// Token: 0x06002B80 RID: 11136 RVA: 0x000D65BE File Offset: 0x000D47BE
	public void Disable()
	{
		if (base.enabled)
		{
			base.enabled = false;
		}
	}

	// Token: 0x040030B2 RID: 12466
	public ParticleSystem target;

	// Token: 0x040030B3 RID: 12467
	public ParticleSystem subEmitter;

	// Token: 0x040030B4 RID: 12468
	public int subEmitterIndex;

	// Token: 0x040030B5 RID: 12469
	public UnityEvent onSubEmit;

	// Token: 0x040030B6 RID: 12470
	public float intervalScale = 1f;

	// Token: 0x040030B7 RID: 12471
	public float interval;

	// Token: 0x040030B8 RID: 12472
	[NonSerialized]
	private bool _canListen;

	// Token: 0x040030B9 RID: 12473
	[NonSerialized]
	private bool _listening;

	// Token: 0x040030BA RID: 12474
	[NonSerialized]
	private bool _listenOnce;

	// Token: 0x040030BB RID: 12475
	[NonSerialized]
	private TimeSince _sinceLastEmit;
}
