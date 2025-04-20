using System;
using UnityEngine;
using UnityEngine.Events;

// Token: 0x020006E9 RID: 1769
public class SubEmitterListener : MonoBehaviour
{
	// Token: 0x06002C0E RID: 11278 RVA: 0x00121AA0 File Offset: 0x0011FCA0
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

	// Token: 0x06002C0F RID: 11279 RVA: 0x0004DD94 File Offset: 0x0004BF94
	private void OnDisable()
	{
		this._listenOnce = false;
		this._listening = false;
	}

	// Token: 0x06002C10 RID: 11280 RVA: 0x0004DDA4 File Offset: 0x0004BFA4
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

	// Token: 0x06002C11 RID: 11281 RVA: 0x0004DDC4 File Offset: 0x0004BFC4
	public void ListenStop()
	{
		this.Disable();
	}

	// Token: 0x06002C12 RID: 11282 RVA: 0x0004DDCC File Offset: 0x0004BFCC
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

	// Token: 0x06002C13 RID: 11283 RVA: 0x00121B5C File Offset: 0x0011FD5C
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

	// Token: 0x06002C14 RID: 11284 RVA: 0x0004DDF9 File Offset: 0x0004BFF9
	protected virtual void OnSubEmit()
	{
		UnityEvent unityEvent = this.onSubEmit;
		if (unityEvent == null)
		{
			return;
		}
		unityEvent.Invoke();
	}

	// Token: 0x06002C15 RID: 11285 RVA: 0x0004DE0B File Offset: 0x0004C00B
	public void Enable()
	{
		if (!base.enabled)
		{
			base.enabled = true;
		}
	}

	// Token: 0x06002C16 RID: 11286 RVA: 0x0004DE1C File Offset: 0x0004C01C
	public void Disable()
	{
		if (base.enabled)
		{
			base.enabled = false;
		}
	}

	// Token: 0x0400314F RID: 12623
	public ParticleSystem target;

	// Token: 0x04003150 RID: 12624
	public ParticleSystem subEmitter;

	// Token: 0x04003151 RID: 12625
	public int subEmitterIndex;

	// Token: 0x04003152 RID: 12626
	public UnityEvent onSubEmit;

	// Token: 0x04003153 RID: 12627
	public float intervalScale = 1f;

	// Token: 0x04003154 RID: 12628
	public float interval;

	// Token: 0x04003155 RID: 12629
	[NonSerialized]
	private bool _canListen;

	// Token: 0x04003156 RID: 12630
	[NonSerialized]
	private bool _listening;

	// Token: 0x04003157 RID: 12631
	[NonSerialized]
	private bool _listenOnce;

	// Token: 0x04003158 RID: 12632
	[NonSerialized]
	private TimeSince _sinceLastEmit;
}
