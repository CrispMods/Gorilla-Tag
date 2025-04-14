using System;
using UnityEngine;
using UnityEngine.Events;

// Token: 0x020006D5 RID: 1749
public class SubEmitterListener : MonoBehaviour
{
	// Token: 0x06002B80 RID: 11136 RVA: 0x000D688C File Offset: 0x000D4A8C
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

	// Token: 0x06002B81 RID: 11137 RVA: 0x000D6948 File Offset: 0x000D4B48
	private void OnDisable()
	{
		this._listenOnce = false;
		this._listening = false;
	}

	// Token: 0x06002B82 RID: 11138 RVA: 0x000D6958 File Offset: 0x000D4B58
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

	// Token: 0x06002B83 RID: 11139 RVA: 0x000D6978 File Offset: 0x000D4B78
	public void ListenStop()
	{
		this.Disable();
	}

	// Token: 0x06002B84 RID: 11140 RVA: 0x000D6980 File Offset: 0x000D4B80
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

	// Token: 0x06002B85 RID: 11141 RVA: 0x000D69B0 File Offset: 0x000D4BB0
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

	// Token: 0x06002B86 RID: 11142 RVA: 0x000D6A1B File Offset: 0x000D4C1B
	protected virtual void OnSubEmit()
	{
		UnityEvent unityEvent = this.onSubEmit;
		if (unityEvent == null)
		{
			return;
		}
		unityEvent.Invoke();
	}

	// Token: 0x06002B87 RID: 11143 RVA: 0x000D6A2D File Offset: 0x000D4C2D
	public void Enable()
	{
		if (!base.enabled)
		{
			base.enabled = true;
		}
	}

	// Token: 0x06002B88 RID: 11144 RVA: 0x000D6A3E File Offset: 0x000D4C3E
	public void Disable()
	{
		if (base.enabled)
		{
			base.enabled = false;
		}
	}

	// Token: 0x040030B8 RID: 12472
	public ParticleSystem target;

	// Token: 0x040030B9 RID: 12473
	public ParticleSystem subEmitter;

	// Token: 0x040030BA RID: 12474
	public int subEmitterIndex;

	// Token: 0x040030BB RID: 12475
	public UnityEvent onSubEmit;

	// Token: 0x040030BC RID: 12476
	public float intervalScale = 1f;

	// Token: 0x040030BD RID: 12477
	public float interval;

	// Token: 0x040030BE RID: 12478
	[NonSerialized]
	private bool _canListen;

	// Token: 0x040030BF RID: 12479
	[NonSerialized]
	private bool _listening;

	// Token: 0x040030C0 RID: 12480
	[NonSerialized]
	private bool _listenOnce;

	// Token: 0x040030C1 RID: 12481
	[NonSerialized]
	private TimeSince _sinceLastEmit;
}
